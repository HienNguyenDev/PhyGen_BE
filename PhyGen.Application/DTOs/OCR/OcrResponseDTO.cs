using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.OCR
{
    public class OcrResponseDTO
    {
        public int TotalSaved { get; set; }
        public List<OcrQuestionDTO> Questions { get; set; }
    }
}
