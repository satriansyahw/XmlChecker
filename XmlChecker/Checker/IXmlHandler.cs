using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlChecker.Checker
{
    public interface IXmlHandler
    {
        public bool DetermineXml(string tag);
        public string RemoveTagSymbol(string tag);
        public string GetClosingTagWithTagProperties(string openingTag, string processedTag);
        public string GetStringBetweenString(string input, string from, string to);
        public string GetStringAfterClosingTag(string closingTag, string processedTag);
        public void Log(string message);
        public bool IsValidTagName(string tag);
    }
}
