
using System.Collections.Generic;
using System.Xml.XPath;

namespace Automat
{
    /// <summary>
    /// Автомат станів та умов
    /// </summary>
    public class States
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pathToXmlFileConditions">Шлях до ХМЛ файлу станів та умов</param>
        /// <param name="namespaceConditions">Активний простір імен</param>
        /// <param name="defaultState">Початковий стан</param>
        public States(string pathToXmlFileConditions, string namespaceConditions, string defaultState = "")
        {
            XPathDocument xpDoc = new XPathDocument(pathToXmlFileConditions);
            DocNavigator = xpDoc.CreateNavigator();

            SetNamespaceConditions(namespaceConditions, defaultState);

            Init();
        }

        /// <summary>
        /// Встановити активний простір імен, обнулити всі колекції умов та задати початковий стан
        /// </summary>
        /// <param name="namespaceConditions">Активний простір імен</param>
        /// <param name="defaultState">Початковий стан</param>
        public void SetNamespaceConditions(string namespaceConditions, string defaultState = "")
        {
            //Корінна вітка простору імен
            NamespaceNode = DocNavigator.SelectSingleNode("/root/namespace[@name='" + namespaceConditions + "']");

            //Колекція станів та умов
            StateConditionsCollection = new Dictionary<string, StateConditions>();

            //Початковий стан по замовчуванню
            SetCurrentStateCondition(defaultState);

            Init();
        }

        /// <summary>
        /// Обнулення та ініціалізація
        /// </summary>
        private void Init()
        {
            BufferName = "";
            BufferTemp = "";
            IfCounter = 0;

            StateBlockList = new List<StateBlock>();
        }

        //
        // ПОЛЯ
        //

        /// <summary>
        /// ХМЛ документ з набором умов та станів
        /// </summary>
        private XPathNavigator DocNavigator { get; set; }

        /// <summary>
        /// Корінна вітка умов
        /// </summary>
        private XPathNavigator NamespaceNode { get; set; }

        /// <summary>
        /// Колекція станів та умов
        /// </summary>
        private Dictionary<string, StateConditions> StateConditionsCollection { get; set; }

        /// <summary>
        /// Активний поточний набір умов
        /// </summary>
        private StateConditions CurrentStateCondition { get; set; }

        /// <summary>
        /// Активний поточний стан
        /// </summary>
        private string CurrentState { get; set; }

        /// <summary>
        /// Поточна назва буфера
        /// </summary>
        private string BufferName { get; set; }

        /// <summary>
        /// Тимчасовий буфер
        /// </summary>
        private string BufferTemp { get; set; }

        /// <summary>
        /// Зібрані блоки
        /// </summary>
        public List<StateBlock> StateBlockList { get; set; }

        /// <summary>
        /// Лічильник повторення умови
        /// </summary>
        private int IfCounter { get; set; }

        /// <summary>
        /// Поточний текст
        /// </summary>
        private string Text { get; set; }

        //
        // ФУНКЦІЇ
        //

        /// <summary>
        /// Встановлює Активний набір умов та Активний стан
        /// </summary>
        /// <param name="state">Активний стан</param>
        private void SetCurrentStateCondition(string state)
        {
            //Якщо стан ще не загружений в память, тоді його потрібно загрузити
            if (!StateConditionsCollection.ContainsKey(state))
            { 
                StateConditions StateConditionItem = new StateConditions();
                StateConditionItem.State = state;

                string statePath = (state == "" ? "" : state + "/");

                XPathNodeIterator ifList = NamespaceNode.Select(statePath + "if");
                while (ifList.MoveNext())
                    StateConditionItem.ConditionsIf.Add(new StateIf(ifList.Current));

                XPathNodeIterator elseList = NamespaceNode.Select(statePath + "else");
                while (elseList.MoveNext())
                    StateConditionItem.ConditionsElse.Add(new StateElse(elseList.Current));

                StateConditionsCollection.Add(state, StateConditionItem);
            }

            //Активний набір умов
            CurrentStateCondition = StateConditionsCollection[state];

            //Активний стан
            CurrentState = state;
        }

        /// <summary>
        /// Записує в буфер значення та змінює поточний буфер. 
        /// Якщо буфер помінявся, зібрані блоки записується в BufferValue
        /// </summary>
        /// <param name="bufferName">Назва буфера</param>
        /// <param name="value">Значення</param>
        private void SetBufferValue(string bufferName, string value)
        {
            if (BufferName != bufferName)
            {
                if (BufferName != "")
                    StateBlockList.Add(new StateBlock(BufferName, BufferTemp));

                BufferTemp = value;
                BufferName = bufferName;
            }
            else
                BufferTemp += value;
        }

        /// <summary>
        /// Функція викликається перед початком читання тексту
        /// </summary>
        private void StartRead()
        {
            Init();
        }

        /// <summary>
        /// Функція викликається по завершенню читання тексту
        /// </summary>
        private void EndRead()
        {
            //Якщо заданий буфер і в бефері щось є тоді добавляєм в зібрані блоки
            if (BufferName != "" && BufferTemp != "")
                StateBlockList.Add(new StateBlock(BufferName, BufferTemp));

            BufferTemp = "";
            BufferName = "";
        }

        /// <summary>
        /// Читає текст
        /// </summary>
        /// <param name="text">Текст</param>
        public void Read(string text)
        {
            Text = text;

            StartRead();

            for (int i = 0; i < Text.Length; i++)
                ReadOneChar(Text[i].ToString());

            EndRead();
        }

        /// <summary>
        /// Функція читає один символ аналізує умови та змінює стан
        /// </summary>
        /// <param name="oneChar">Символ</param>
        private void ReadOneChar(string oneChar)
        {
            bool ifSatisfied = false;

            foreach (StateIf StateIfItem in CurrentStateCondition.ConditionsIf)
            {
                if (StateIfItem.IfType == "eq")
                    ifSatisfied = (StateIfItem.IfValue == oneChar);
                else if (StateIfItem.IfType == "in")
                    ifSatisfied = (StateIfItem.IfValue.IndexOf(oneChar) != -1);

                if (ifSatisfied)
                {
                    if (StateIfItem.Counter != 0)
                    {
                        IfCounter++;

                        if (StateIfItem.Counter == IfCounter)
                        {
                            IfCounter = 0;
                            ifSatisfied = false;
                        }
                    }

                    SetCurrentStateCondition(StateIfItem.State);

                    SetBufferValue(StateIfItem.BufferName, oneChar);

                    break;
                }
            }

            //Якщо if не спрацювало
            if (!ifSatisfied)
            {
                foreach (StateElse StateElseItem in CurrentStateCondition.ConditionsElse)
                {
                    SetCurrentStateCondition(StateElseItem.State);

                    SetBufferValue(StateElseItem.BufferName, oneChar);

                    break;
                }
            }
        }
    }
}
