using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.ExamSet
{
    public class ExamSetResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ClassName { get; set; }
        public int QuestionCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }

}
