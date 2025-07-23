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
        public int ChapterId { get; set; } 
        public int TopicId { get; set; }
        public int ClassId { get; set; } 

    }
}
