using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generation
{
    // This class represents a generic variable that implements the ICommonProperties interface
    // It contains properties for the variable name, type, and children
    class Variable : ICommonProperties
    {
        public string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual ArrayList Children { get; set; }
    }
}
