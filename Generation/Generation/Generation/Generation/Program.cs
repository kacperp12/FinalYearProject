using DotLiquid;
using Generation;
using Generation.Objects;
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

    static void Main(string[] args)
    {
        // Create a list to store the created objects
        List<ICommonProperties> objects = new List<ICommonProperties>();

        // Define a flag to indicate if the user has finished creating objects
        bool finish = false;

        // Define a dictionary to map object types to object IDs
        Dictionary<int, string> objectTypes = new Dictionary<int, string>
        {
            { 1, "button" },
            { 2, "led" },
            { 3, "textbox" },
            { 4, "label" },
        };

        // Define a dictionary to map object IDs to functions that create the object
        Dictionary<int, Func<ICommonProperties>> possibleObjects = new Dictionary<int, Func<ICommonProperties>>();

        // Loop through the object types and add the corresponding creator function to the dictionary
        foreach (var kvp in objectTypes)
        {
            possibleObjects.Add(kvp.Key, () => CreateObject(kvp.Value));
        }

        // Counter to keep track of the number of objects created
        var numObjects = 0;

        // Loop until the user chooses to finish
        while (!finish)
        {
            // Display a menu of object types to choose from
            StringBuilder sb = new StringBuilder();
            sb.Append("Please select an object to create:\n");
            sb.Append("1. Button\n");
            sb.Append("2. LED\n");
            sb.Append("3. Textbox\n");
            sb.Append("4. Label\n");
            sb.Append("5. Finish\n");
            Console.WriteLine(sb.ToString());

            // Read the user's choice from the console
            int choice;
            int.TryParse(Console.ReadLine(), out choice);

            // If the choice is valid, create the object and add it to the list of objects
            if (possibleObjects.TryGetValue(choice, out Func<ICommonProperties> creator))
            {
                objects.Add(creator());
                objects.Add(new Label($"Object {numObjects + 1}", 100, controlTop));
                numObjects++;
            }
            // If the user chooses to finish, set the flag to true and exit the loop
            else if (choice == 5)
            {
                finish = true;
            }
            // If the choice is invalid, display an error message
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        // Serialize the list of objects to YAML format
        var serializer = new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.Preserve)
            .WithIndentedSequences()
            .Build();
        var yamlObj = serializer.Serialize(objects);

        // Fix the YAML formatting to remove unnecessary empty lines
        var yaml = fixYamlObj(yamlObj, objects);

        // Load a Liquid template file and render it with the YAML data
        var projectDirectory = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\"));
        var templateFilePath = Path.Combine(projectDirectory, "TextFile1.liquid");
        var template = Template.Parse(File.ReadAllText(templateFilePath));
        var hash = Hash.FromAnonymousObject(new { yaml });
        var output = template.Render(hash);

        // Write the rendered output to a file
        string dir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\..\\..\\"));
        string nodePath = Path.Combine(dir, "Nodes", "UI", "UI.yaml");

        if (File.Exists(nodePath))
            File.Delete(nodePath);
        File.WriteAllText(nodePath, output);
    }

    // This method prompts the user for an integer input within a certain range and returns the input value.
    static int GetIntInput(string message, int maxVal)
    {
        int input;
        bool validInput = false;

        // Use a do-while loop to repeatedly prompt the user for input until a valid input value is entered.
        do
        {
            // Display the prompt message to the user.
            Console.WriteLine(message);

            // Use int.TryParse to check whether the user input can be parsed as an integer, and whether it is within the specified range.
            validInput = int.TryParse(Console.ReadLine(), out input) && input >= 0 && input <= maxVal;

            // If the input is invalid, display an error message and continue the loop.
            if (!validInput)
            {
                Console.WriteLine($"Invalid input. Please enter a number between 0 and {maxVal}.");
            }
        } while (!validInput);

        // If a valid input value is entered, return the input value as an integer.
        return input;
    }

    // This function fixes the formatting of the YAML output
    static string fixYamlObj(string yamlObj, List<ICommonProperties> objects)
    {
        // Split the YAML output into lines
        string[] lines = yamlObj.Split('\n');

        // Loop through each line
        for (int i = 0; i < lines.Length; i++)
        {
            foreach (ICommonProperties obj in objects)
            {
                if (lines[i].Contains("- Name: " + obj.Name))
                {
                    // If this is the first object, don't add any indentation
                    if (obj == objects[0])
                    {
                        break;
                    }

                    // Add two spaces of indentation to the line
                    lines[i] = "  " + lines[i];
                    break;
                }
                else if(lines[i].Contains("Type: " + obj.Type))
                {
                    lines[i] = "  " + lines[i];
                    break;
                }
                else if (lines[i].Contains("Children"))
                {
                    lines[i] = "  " + lines[i];
                    break;
                }
                // Remove single quotes from LocaleId values
                else if (lines[i].Contains("LocaleId"))
                {
                    lines[i] = lines[i].Replace("'", "");
                }
            }
        }

        // Join the lines back into a single string
        string yaml = string.Join('\n', lines);
        return yaml;
    }

    // This function creates a new object of the specified object type and returns it
    private static ICommonProperties CreateObject(string controlType)
    {
        // Prompt the user to enter a name for the object
        Console.WriteLine($"Please enter a name for the {controlType}:");
        string controlName = Console.ReadLine();

        // Prompt the user to enter the top margin for the object
        // using a helper function GetIntInput to ensure the input is an integer
        controlTop = GetIntInput("Please enter the top margin:", 400);

        // Create a new object of the specified object type
        switch (controlType)
        {
            case "button":
                return new Button(controlName, 200, controlTop);
            case "led":
                return new LED(controlName, 200, controlTop);
            case "textbox":
                return new Textbox(controlName, 200, controlTop);
            case "label":
                return new Label(controlName, 200, controlTop);
            default:
                throw new ArgumentException($"Invalid control type: {controlType}");
        }
    }
}