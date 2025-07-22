using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.ExamSet
{
    public class DropDownRequest
    {
        public int Quantity { get; set; }
        public int Chapter { get; set; } 
        public int Topic { get; set; }
        public int Class { get; set; } 

    }
}
