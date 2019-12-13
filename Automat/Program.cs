using System;
using System.Xml.XPath;
using System.IO;
using System.Text;
using System.Xml;

namespace Automat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 150;
            Console.WindowHeight = 50;

            string file_text = File.ReadAllText(@"D:\a.txt", Encoding.GetEncoding("windows-1251"));

            States StatesCommand = new States(@"D:\..\command.xml", "html");

            StatesCommand.Read(file_text);

            Console.ReadLine();
        }

        
    }
}
