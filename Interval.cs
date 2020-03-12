using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathStatistics
{
    class Interval
    {
        public double A { get; set; }
        public double B { get; set; }

        public override string ToString()
        {
            return $"[{A}, {B}]";
        }
    }
}
