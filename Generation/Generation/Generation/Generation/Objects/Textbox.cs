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
}
