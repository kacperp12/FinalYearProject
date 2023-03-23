using DotLiquid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

class Program
{
    static int controlTop;

    static int GetIntInput(string message, int maxVal)
    {
        int input;
        bool validInput = false;
        do
        {
            Console.WriteLine(message);
            validInput = int.TryParse(Console.ReadLine(), out input) && input >= 0 && input <= maxVal;
            if (!validInput)
            {
                Console.WriteLine($"Invalid input. Please enter a number between 0 and {maxVal}.");
            }
        } while (!validInput);
        return input;
    }

    static string fixYamlObj(string yamlObj, List<object> objects)
    {
        string[] lines = yamlObj.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            foreach (ICommonProperties obj in objects)
            {
                if (lines[i].Contains("- Name: " + obj.Name))
                {
                    if (obj == objects[0])
                    {
                        break;
                    }

                    lines[i] = "  " + lines[i];
                    break;
                }

                if (lines[i].Contains("Type: " + obj.Type))
                {
                    lines[i] = "  " + lines[i];
                    break;
                }

                if (lines[i].Contains("Children"))
                {
                    lines[i] = "  " + lines[i];
                    break;
                }

                if (lines[i].Contains("LocaleId"))
                {
                    lines[i] = lines[i].Replace("'", "");
                }
            }
        }

        string yaml = string.Join('\n', lines);
        return yaml;
    }

    static void Main(string[] args)
    {
        List<object> objects = new List<object>();
        bool finish = false;
        Dictionary<int, string> controlTypes = new Dictionary<int, string>
            {
                { 1, "button" },
                { 2, "led" },
                { 3, "textbox" },
                { 4, "label" },
            };

        Dictionary<int, Func<object>> possibleObjects = new Dictionary<int, Func<object>>();

        foreach (var kvp in controlTypes)
        {
            possibleObjects.Add(kvp.Key, () => CreateControl(kvp.Value));
        }

        var numObjects = 0;

        while (!finish)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Please select an object to create:\n");
            sb.Append("1. Button\n");
            sb.Append("2. LED\n");
            sb.Append("3. Textbox\n");
            sb.Append("4. Label\n");
            sb.Append("5. Finish\n");
            Console.WriteLine(sb.ToString());

            int choice;
            int.TryParse(Console.ReadLine(), out choice);

            if (possibleObjects.TryGetValue(choice, out Func<object> creator))
            {
                objects.Add(creator());
                
                objects.Add(new Label($"Object {numObjects + 1}", 50, controlTop));
                numObjects++;
            }
            else if (choice == 5)
            {
                finish = true;
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        var serializer = new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve)
            .WithIndentedSequences()
            .Build();

        var yamlObj = serializer.Serialize(objects);

        var yaml = fixYamlObj(yamlObj, objects);

        var projectDirectory = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\"));
        var templateFilePath = Path.Combine(projectDirectory, "TextFile1.liquid");
        var template = Template.Parse(File.ReadAllText(templateFilePath));

        var hash = Hash.FromAnonymousObject(new { yaml });
        var output = template.Render(hash);

        string dir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\..\\"));
        string nodePath = Path.Combine(dir, "Nodes", "UI", "UI.yaml");

        if (File.Exists(nodePath))
            File.Delete(nodePath);

        File.WriteAllText(nodePath, output);
    }

    private static object CreateControl(string controlType)
    {
        Console.WriteLine($"Please enter a name for the {controlType}:");
        string controlName = Console.ReadLine();
        int controlLeft = GetIntInput("Please enter the left margin:", 400);
        controlTop = GetIntInput("Please enter the top margin:", 400);

        switch (controlType)
        {
            case "button":
                return new Button(controlName, controlLeft, controlTop);
            case "led":
                return new LED(controlName, controlLeft, controlTop);
            case "textbox":
                return new Textbox(controlName, controlLeft, controlTop);
            case "label":
                return new Label(controlName, controlLeft, controlTop);
            default:
                throw new ArgumentException($"Invalid control type: {controlType}");
        }
    }
}

public interface ICommonProperties
{
    string Name { get; set; }
    string Type { get; set; }
    public ArrayList Children { get; set; }
}

class Variable : ICommonProperties
{
    public string Name { get; set; }
    public virtual string Type { get; set; }
    public virtual ArrayList Children { get; set; }
}

class Button : Variable
{
    public Button(string name, int leftMargin, int topMargin)
    {
        Name = name;
        Type = "Button";
        Children = new ArrayList();
        Children.Add(new Text(name));
        Children.Add(new LeftMargin(leftMargin));
        Children.Add(new TopMargin(topMargin));
    }
}
class LED : Variable
{
    public LED(string name, int leftMargin, int topMargin)
    {
        Name = name;
        Type = "Led";
        Children = new ArrayList();
        Children.Add(new Active());
        Children.Add(new Dimensions("Width"));
        Children.Add(new Dimensions("Height"));
        Children.Add(new LeftMargin(leftMargin));
        Children.Add(new TopMargin(topMargin));
    }
}

class Textbox : Variable
{
    public Textbox(string name, int leftMargin, int topMargin)
    {
        Name = name;
        Type = "TextBox";
        Children = new ArrayList();
        Children.Add(new Text(name, "{\"NamespaceIndex\":-1,\"LocaleId\":\"\",\"Text\":\"\"}"));
        Children.Add(new Dimensions("Width"));
        Children.Add(new LeftMargin(leftMargin));
        Children.Add(new TopMargin(topMargin));
    }
}

class Label : Variable
{
    public Label(string name, int leftMargin, int topMargin)
    {
        Name = name;
        Type = "Label";
        Children = new ArrayList();
        Children.Add(new Text(name));
        Children.Add(new LeftMargin(leftMargin));
        Children.Add(new TopMargin(topMargin));
    }
}

class Active
{
    public string Name { get; set; }
    public string Type { get; }
    public string DataType { get; }
    public bool Value { get; }
    public Active()
    {
        Name = "Active";
        Type = "BaseDataVariableType";
        DataType = "Boolean";
        Value = false;
    }
}

class ExtendedVariable
{
    public string Name { get; set; }
    public virtual string Type { get; set; }
    public virtual string DataType { get; set; }
    public virtual string ModellingRule { get; set; }
    public virtual string Value { get; set; }
}

class Dimensions : ExtendedVariable
{
    public Dimensions(string name)
    {
        Name = name;
        Type = "BaseVariableType";
        DataType = "Size";
        ModellingRule = "Optional";
        Value = "50.0";
    }
}

class Text : ExtendedVariable
{
    public Text(string name)
    {
        Name = "Text";
        Type = "BaseDataVariableType";
        DataType = "LocalizedText";
        ModellingRule = "Optional";
        Value = "{\"LocaleId\":\"en-US\",\"Text\":\"" + name + "\"}";
    }

    public Text(string name, string value)
    {
        Name = "Text";
        Type = "BaseDataVariableType";
        DataType = "LocalizedText";
        ModellingRule = "Optional";
        Value = value;
    }
}

class LeftMargin : ExtendedVariable
{
    public LeftMargin(int value)
    {
        Name = "LeftMargin";
        Type = "BaseVariableType";
        DataType = "Size";
        ModellingRule = "Optional";
        Value = value.ToString();
    }
}

class TopMargin : ExtendedVariable
{
    public TopMargin(int value)
    {
        Name = "TopMargin";
        Type = "BaseVariableType";
        DataType = "Size";
        ModellingRule = "Optional";
        Value = value.ToString();
    }
}