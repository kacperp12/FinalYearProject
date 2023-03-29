using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generation
{
    // This interface defines common properties that can be implemented by classes representing variables
    public interface ICommonProperties
    {
        string Name { get; set; }
        string Type { get; set; }
        public ArrayList Children { get; set; }
    }
}
