using PhyGen.Domain.Chapters;
using PhyGen.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Questions
{
    public class QuestionTemplate : Entity
    {
        public Guid Id { get; set; }
        public Guid ChapterId { get; set; }
        public Guid TypeId { get; set; }
        public string TemplateText { get; set; }

        public Chapter Chapter { get; set; }
        public QuestionType Type { get; set; }
    }

}
