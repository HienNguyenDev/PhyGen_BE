using PhyGen.Domain.Chapters;
using PhyGen.Domain.Classes;
using PhyGen.Domain.Common;
using PhyGen.Domain.ExamSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Questions
{
    public class Question : Entity
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; }
        public Guid ClassId { get; set; }
        public Guid ChapterId { get; set; }
        public Guid TypeId { get; set; }
        public string SourceFile { get; set; }
        public DateTime CreatedAt { get; set; }
        public float AIConfidence { get; set; }

        public Class Class { get; set; }
        public Chapter Chapter { get; set; }
        public QuestionType Type { get; set; }
        public ICollection<ExamSetQuestion> ExamSetQuestions { get; set; }
    }

}
