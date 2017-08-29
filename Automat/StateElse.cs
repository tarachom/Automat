
using System.Xml.XPath;

namespace Automat
{
    /// <summary>
    /// Умова ELSE
    /// </summary>
    public class StateElse
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="elseNode">Вітка умови ELSE</param>
        public StateElse(XPathNavigator elseNode)
        {
            State = ReadNode(elseNode, "state");
            BufferName = elseNode.GetAttribute("buffer", "");
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
        /// Стан
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Назва буферу
        /// </summary>
        public string BufferName { get; set; }
    }
}
