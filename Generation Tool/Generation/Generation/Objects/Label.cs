using Generation.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generation.Objects
{
    // This class represents a textbox object that inherits from the Variable class
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
}
