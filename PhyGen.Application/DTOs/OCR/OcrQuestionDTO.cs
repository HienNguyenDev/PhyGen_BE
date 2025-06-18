using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.OCR
{
    public class OcrQuestionDTO
    {
        public int Number { get; set; }
        public string Question { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string? Correct { get; set; }
    }
}
