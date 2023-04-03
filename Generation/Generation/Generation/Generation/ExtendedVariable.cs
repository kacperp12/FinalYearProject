using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generation
{
    // This class represents an extended variable with additional and different properties beyond the standard ICommonProperties interface
    public class ExtendedVariable
    {
        public string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual string DataType { get; set; }
        public virtual string ModellingRule { get; set; }
        public virtual string Value { get; set; }
    }
}
