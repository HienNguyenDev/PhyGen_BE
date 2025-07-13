using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.OCR
{
    public class OcrQuestionDTO
    {
        public string? Question { get; set; }
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public string? D { get; set; }
        public string? Answer { get; set; }
        public string? Difficulty { get; set; }
        public string? Chapter { get; set; }
        public string? Topic { get; set; }
    }
}
