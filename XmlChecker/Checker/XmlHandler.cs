using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XmlChecker.Checker
{
    public class XmlHandler : IXmlHandler
    {
        private readonly string[] SpecialChars = new string[] { "`", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "{", "}", "[", "]", "|", "\"", ":", ";", "<", ",", ">", ".", "?", "/" };
        public bool DetermineXml(string tag)
        {
            if(string.IsNullOrEmpty(tag))
            {
                Log($"{tag} => invalid null or empty tag");
                return false;
            }
            string processedTag = tag.Trim();
            string openingTag = string.Empty;
            string closingTag = string.Empty;
            string value = string.Empty;
            bool looping = false;
            if(!looping)
            {
                if(!processedTag.StartsWith("<") || !processedTag.EndsWith(">"))
                {
                    Log($"{tag} => invalid processing opening and closing tag");
                    return false;
                }
            }
            looping = true;
            while(looping)
            {
                if (!processedTag.StartsWith("<") || !processedTag.EndsWith(">"))
                {
                    Log($"{tag} => invalid processing opening and closing tag");
                    return true;
                }
                value = GetStringBetweenString(processedTag, "<", ">");
                if(!IsValidTagName(value))
                {
                    Log($"{tag} => invalid processing tag name");
                    return false;
                }
                openingTag = $"<{value}>";
                if(!value.Contains('='))
                {
                    closingTag = $"</{value}";
                }
                else
                {
                    closingTag = GetClosingTagWithTagProperties(openingTag, processedTag);
                    if(string.IsNullOrEmpty(closingTag))
                    {
                        Log($"{tag} => invalid processing tag with properties");
                        return false;
                    }
                }
                value = GetStringAfterClosingTag(closingTag, processedTag);
                if(!string.IsNullOrEmpty(value))
                {
                    Log($"{tag} => invalid text/tag position not allowed after closing tag");
                    return false;
                }
                value = GetStringBetweenString(processedTag, openingTag, closingTag).TrimStart();
                if(string.IsNullOrEmpty(value))
                {
                    looping = false;
                }
                processedTag = value;
            }
            return true;
        }

        public string GetClosingTagWithTagProperties(string openingTag, string processedTag)
        {
            if(string.IsNullOrEmpty(openingTag) || string.IsNullOrEmpty(processedTag))
            {
                return string.Empty;
            }
            var tagEqual = openingTag.Split('=')[0];
            if (processedTag.Contains($"</{tagEqual} "))
            {
                return $"</{tagEqual} ";
            }
            string openingTagCheck = RemoveTagSymbol(openingTag.Split('=')[0]);
            tagEqual = openingTagCheck.Split('=')[0];
            var splitTagEqual = tagEqual.Trim().Split(' ');
            string closingTagValue = GetStringBetweenString(processedTag,openingTag, $"</{splitTagEqual[0]} ");
            string closingTag = processedTag.Substring(processedTag.IndexOf(closingTagValue) + closingTagValue.Length);
            string closingTagCheck = RemoveTagSymbol(closingTag);
            List<string>listOpening = openingTagCheck.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList().Select(name => name.Trim()).ToList();
            List<string> listClosing = closingTagCheck.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList().Select(name => name.Trim()).ToList();
            if(Enumerable.SequenceEqual(listOpening,listClosing))
            {
                if(closingTag.Trim().EndsWith('>'))
                {
                    closingTag = closingTag.Substring(0, closingTag.Trim().Length - 1);
                }
                return closingTag;
            }
            return string.Empty;
        }

        public string GetStringAfterClosingTag(string closingTag, string processedTag)
        {
            if (string.IsNullOrEmpty(closingTag) || string.IsNullOrEmpty(processedTag))
            {
                return string.Empty;
            }
            int closingEndPos = 0;
            if (processedTag.Contains(closingTag))
            {
                if (processedTag.IndexOf(closingTag) > 0)
                {
                    closingEndPos = processedTag.IndexOf(closingTag) + $"{closingTag}>".Length + GetStringBetweenString(processedTag, closingTag, ">").Length;
                }
            }
            if(processedTag.Length >= closingEndPos)
            {
                return processedTag.Substring(closingEndPos);
            }
            return string.Empty;
        }

        public string GetStringBetweenString(string input, string from, string to)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                return string.Empty;
            }
            if (!input.Contains(from) || !input.Contains(to) || input.Length < (from.Length + to.Length))
            {
                return string.Empty;
            }
            int p1 = input.IndexOf(from) + from.Length;
            int p2 = input.IndexOf(to,p1);
            return input.Substring(p1, p2-p1);
        }

        public bool IsValidTagName(string tag)
        {
            tag  = tag.Split('=')[0];
            if (!char.IsLetter(tag, 0))
            {
                return false;
            }
            foreach (var chars in SpecialChars)
            {
                if (tag.Contains(chars))
                {
                    return false;
                }
            }
            return true;
        }

        public void Log(string message)
        {
            string logFileName = $"xmlChecker_{DateTime.Now.ToString("yyyyMMdd")}.log";
            using(StreamWriter w = new StreamWriter(logFileName,true))
            {
                w.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}{message}");
            }
        }

        public string RemoveTagSymbol(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return string.Empty;
            }
            tag = tag.Trim();
            if (tag.StartsWith("<"))
            {
                tag = tag.Substring(1);
            }
            if (tag.StartsWith("/"))
            {
                tag = tag.Substring(1);
            }
            if (tag.EndsWith(">"))
            {
                tag = tag.Substring(0, tag.Length - 1);
            }
            return tag;
        }
    }
}
