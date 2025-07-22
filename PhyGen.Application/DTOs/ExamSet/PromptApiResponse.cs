using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.ExamSet
{
    public class PromptApiResponse
    {
        [JsonPropertyName("result")]
        public QuestionRequestRoot Result { get; set; }
    }
}
