using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.ExamSet
{
    public class QuestionRequestRoot
    {
        [JsonPropertyName("total_quantity")]
        public int TotalQuantity { get; set; }

        [JsonPropertyName("requests")]
        public List<QuestionRequest> Requests { get; set; }
    }
}
