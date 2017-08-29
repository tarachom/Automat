
using System.Xml.XPath;

namespace Automat
{
    /// <summary>
    /// Умова IF
    /// </summary>
    public class StateIf
    {
        /// <summary>
        /// Констурктор
        /// </summary>
        /// <param name="ifNode">Вітка умови IF</param>
        public StateIf(XPathNavigator ifNode)
        {
            IfValue = ReadNode(ifNode, "value");
            IfType = ifNode.GetAttribute("type", "");
            State = ReadNode(ifNode, "state");
            BufferName = ifNode.GetAttribute("buffer", "");

            string _Counter = ifNode.GetAttribute("counter", "");
            if (_Counter != "") Counter = int.Parse(_Counter);
        }

        /// <summary>
        /// Функція читає ноду
        /// </summary>
        /// <param name="rootNode">Вітка в якій потрібно знайти ноду</param>
        /// <param name="node">Назва ноди</param>
        /// <returns></returns>
        private string ReadNode(XPathNavigator rootNode, string node)
        {
            XPathNavigator ItemNode = rootNode.SelectSingleNode(node);

            if (ItemNode != null)
                return ItemNode.Value;
            else
                return "";
        }

        /// <summary>
        /// Значення умови
        /// </summary>
        public string IfValue { get; set; }

        /// <summary>
        /// Тип умови
        /// </summary>
        public string IfType { get; set; }

        /// <summary>
        /// Назва стану
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Назва буферу
        /// </summary>
        public string BufferName { get; set; }

        /// <summary>
        /// Кількість повторень умови для її деактивації
        /// Тобто якщо умова виконується певну кількість раз, після цього вона стає неактивною
        /// </summary>
        public int Counter { get; set; }
    }
}
