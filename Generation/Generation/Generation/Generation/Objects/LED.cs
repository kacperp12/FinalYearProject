using Generation.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generation.Objects
{
    class LED : Variable
    {
        // This class represents an LED object that inherits from the Variable class
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
}
