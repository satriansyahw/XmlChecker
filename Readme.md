# C# :
A simple handler in XML Check

# Game Of Rule
1. Each starting element must have a corresponding ending element
2. Nesting of elements within each other must be well nested, which means start first must end last. For example, <tutorial><topic>XML</topic></tutorial> is a correct way of nesting but <tutorial><topic>XML</tutorial></topic> is not
3. Tag Name, first character only letter allowed, can't contains special chars ex: "`", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "{", "}", "[", "]", "|", "\"", ":", ";", "<", ",", ">", ".", "?", "/" 
4. opening tag and a closing tag as matched only if the strings in both tags are identical, ex: <tutorial date="01/01/2000">XML</tutorial> not allowed but <tutorial date="01/01/2000">XML</tutorial date> allowed

## Run On Visual Studio
Open the project in Visual studio and click Start
On the console, input xml and enter

## Project Info
Console application with dot net core 6.0