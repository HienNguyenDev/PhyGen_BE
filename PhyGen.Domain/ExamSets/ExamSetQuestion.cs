using PhyGen.Domain.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.ExamSets
{
    public class ExamSetQuestion
    {
        public Guid Id { get; set; }
        public Guid ExamSetId { get; set; }
        public Guid QuestionId { get; set; }
        public int Order { get; set; }

        public ExamSet ExamSet { get; set; }
        public Question Question { get; set; }
    }

}
