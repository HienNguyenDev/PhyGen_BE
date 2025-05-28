
using PhyGen.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Questions
{
    public class QuestionType : Entity
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }
        public string Structure { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<QuestionTemplate> QuestionTemplates { get; set; }
    }

}
