
namespace Automat
{
    /// <summary>
    /// Буфер та значення
    /// </summary>
    public class StateBlock
    {
        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="name">Назва</param>
        /// <param name="value">Значення</param>
        public StateBlock(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Назва 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Значення
        /// </summary>
        public string Value { get; private set; }
    }
}
