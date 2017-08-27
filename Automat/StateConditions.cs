
using System.Collections.Generic;

namespace Automat
{
    /// <summary>
    /// Набір умов для певного стану
    /// </summary>
    public class StateConditions
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public StateConditions()
        {
            ConditionsIf = new List<StateIf>();
            ConditionsElse = new List<StateElse>();
        }

        /// <summary>
        /// Стан
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Колекція умов IF
        /// </summary>
        public List<StateIf> ConditionsIf { get; set; }

        /// <summary>
        /// Колекція умов ELSE
        /// </summary>
        public List<StateElse> ConditionsElse { get; set; }
    }
}
