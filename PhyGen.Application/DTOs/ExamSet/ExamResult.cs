using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Application.DTOs.ExamSet
{
    public class ExamResult
    {
        public Guid Id { get; set; }
        public string? Question { get; set; }
        public string? ClassName { get; set; }
        public string? ChapterName { get; set; }
        public string? TopicName { get; set; }
        public int? Difficulty { get; set; }
        public string? A { get; set; }
        public string? B { get; set; }
        public string? C { get; set; }
        public string? D { get; set; }
        public string? Answer { get; set; }
    }
}
