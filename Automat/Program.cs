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

            string file_text = File.ReadAllText(@"D:\al.txt", Encoding.GetEncoding("windows-1251"));

            States StatesCommand = new States(@"D:\Developer\VS\Automat\Automat\command.xml", "html");

            StatesCommand.Read(file_text);

            XmlWriterSettings XMLSettings = new XmlWriterSettings();
            XMLSettings.Encoding = System.Text.Encoding.UTF8;
            XMLSettings.Indent = true;
            XMLSettings.NewLineChars = "\r\n";
            XMLSettings.WriteEndDocumentOnClose = true;

            XmlWriter XMLTableShema = XmlWriter.Create(@"D:\resultat.xml", XMLSettings);
            XMLTableShema.WriteComment(DateTime.Now.ToString());
            XMLTableShema.WriteStartElement("root");

            foreach (StateBlock block in StatesCommand.StateBlockList)
            {
                Console.WriteLine(block.Name + " = " + block.Value.Trim());

                switch (block.Name)
                {
                    case "open_tag":
                        {
                            XMLTableShema.WriteStartElement(block.Value.Trim());

                            if (block.Value.Trim() == "br")
                                XMLTableShema.WriteEndElement();

                            break;
                        }
                    case "tag_atr_name":
                        {
                            XMLTableShema.WriteStartElement("atr");
                            XMLTableShema.WriteAttributeString("name", block.Value.Trim());
                            break;
                        }
                    case "tag_atr_value":
                        {
                            XMLTableShema.WriteString(block.Value.Trim());
                            XMLTableShema.WriteEndElement();
                            break;
                        }
                    case "text":
                        {
                            if (block.Value.Trim() != "")
                            {
                                XMLTableShema.WriteStartElement(block.Name);
                                XMLTableShema.WriteCData(block.Value.Trim());
                                XMLTableShema.WriteEndElement();
                            }
                            break;
                        }
                    case "close_tag":
                        {
                            XMLTableShema.WriteEndElement();
                            break;
                        }
                    case "close_one_tag":
                        {
                            XMLTableShema.WriteEndElement();
                            break;
                        }
                    default:
                        break;
                }
            }

            XMLTableShema.WriteEndElement();
            XMLTableShema.Flush();
            XMLTableShema.Close();

            Console.ReadLine();
        }

        
    }
}
