using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generation.Properties
{
    // This class represents a top margin extended variable
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

    // This class represents a left margin extended variable
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

    // This class represents an "Active" variable that is used to indicate whether an object is active or inactive
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

    // This class represents a dimensions extended variable
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

    // This class represents a text extended variable
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
}
