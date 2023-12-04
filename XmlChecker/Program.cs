using XmlChecker.Checker;
IXmlHandler handler = new XmlHandler();
List<string> list = new List<string>();
while (true)
{
    Console.WriteLine("");
    Console.WriteLine("XML Checker, please input your xml ");
    string tag = Console.ReadLine();
    Console.WriteLine($"Result   ==>  {handler.DetermineXml(tag)}");
}