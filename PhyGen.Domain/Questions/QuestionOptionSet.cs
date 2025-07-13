using PhyGen.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Questions
{
    public class QuestionOptionSet : Entity
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string? Correct { get; set; }
        public Question Question { get; set; }
    }
}
