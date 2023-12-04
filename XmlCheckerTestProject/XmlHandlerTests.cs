using XmlChecker.Checker;

namespace XmlCheckerTestProject
{
    [TestClass]
    public class XmlHandlerTests
    {
        IXmlHandler handler = new XmlHandler();
        [TestMethod]
        public void ValidTagName_Given_CorrectName_Return_true()
        {
            string tag = "design";
            bool isValid  = handler.IsValidTagName(tag);
            Assert.IsTrue(isValid);
        }
        [TestMethod]
        public void ValidTagName_Given_SpecialCharsName_Return_false()
        {
            string tag = "design%";
            bool isValid = handler.IsValidTagName(tag);
            Assert.IsFalse(isValid);
        }
        [TestMethod]
        public void ValidTagName_Given_FirstCharNotLetter_Return_false()
        {
            string tag = "1design";
            bool isValid = handler.IsValidTagName(tag);
            Assert.IsFalse(isValid);
        }
        [TestMethod]
        public void RemoveTagSymbol_GivenOpeningBracket_Return_LeftTrim1()
        {
            string tag = "<design";
            string newTag = handler.RemoveTagSymbol(tag);
            Assert.AreEqual("design", newTag);
        }
        [TestMethod]
        public void RemoveTagSymbol_GivenSlash_Return_LeftTrim2()
        {
            string tag = "</design";
            string newTag = handler.RemoveTagSymbol(tag);
            Assert.AreEqual("design", newTag);
        }
        [TestMethod]
        public void RemoveTagSymbol_GivenClosingBracket_Return_RightTrim1()
        {
            string tag = "design>";
            string newTag = handler.RemoveTagSymbol(tag);
            Assert.AreEqual("design", newTag);
        }
        [TestMethod]
        public void BetweenString_GivenInput_Return_String()
        {
            string input = "design contemporer 1945";
            string result = handler.GetStringBetweenString(input, "design","1945");
            Assert.AreEqual(" contemporer ", result);
        }
        [TestMethod]
        public void BetweenString_GivenInputLessThanFromTo_Return_Empty()
        {
            string input = "design";
            string result = handler.GetStringBetweenString(input, "design", "1945");
            Assert.AreEqual(string.Empty, result);
        }
        [TestMethod]
        public void ClosingTag_GivenInputLessThanFromTo_Return_String()
        {
            string input = "<Design><Code>hello world</Code></Design><People>";
            string result = handler.GetStringAfterClosingTag("</Design", input);
            Assert.AreEqual("<People>", result);
        }
        [TestMethod]
        public void ClosingTag_GivenInputLessThanFromTo_Return_Empty()
        {
            string input = "<Design><Code>hello world</Code></Design>";
            string result = handler.GetStringAfterClosingTag("</Design", input);
            Assert.AreEqual(string.Empty, result);
        }
        [TestMethod]
        public void ClosingTagProperties_GivenTagWithSpaces_Return_String()
        {
            string input = @"<tutorial lang=""1"">XML</tutorial    lang>";
            string result = handler.GetClosingTagWithTagProperties(@"<tutorial lang=""1"">", input);
            Assert.AreEqual("</tutorial    lang", result);
        }
        [TestMethod]
        public void ClosingTagProperties_GivenTagWithoutSpace_Return_String()
        {
            string input = @"<tutorial lang=""1"">XML</tutorial lang>";
            string result = handler.GetClosingTagWithTagProperties(@"<tutorial lang=""1"">", input);
            Assert.AreEqual("</tutorial lang", result);
        }

        [TestMethod]
        public void ClosingTagProperties_GivenInputLessThanFromTo_Return_Empty()
        {
            string input = @" < tutorial lang=""1"">XML</tutorial >";
            string result = handler.GetClosingTagWithTagProperties(@" < tutorial lang=""1"">", input);
            Assert.AreEqual(string.Empty, result);
        }
        [TestMethod]
        [DataRow("<Design>", false)]
        [DataRow("</Design>", false)]
        [DataRow("Design>", false)]
        [DataRow("<tutorial date=\"01/01/2000\">XML</tutorial>", false)]
        [DataRow("<Design><Code>hello world</Code></Design>", true)]
        [DataRow("<Design><Code>hello world</Code></Design><People>", false)]
        [DataRow("<People><Design><Code>hello world</People></Code></Design>", false)]
        [DataRow("<People age=”1”>hello world</People>", false)]
        [DataRow("<tutorial><topic>XML</tutorial></topic>", false)]
        [DataRow(@"<tutorial lang=""1""><topic>XML</topic></tutorial>", false)]
        [DataRow(@"<tutorial lang=""1"">XML</tutorial    lang>", true)]
        public void DeterminteXml(string tag, bool expected)
        {
            Assert.AreEqual(expected, handler.DetermineXml(tag));
        }
    }
}