using PhyGen.Domain.Chapters;
using PhyGen.Domain.Common;
using PhyGen.Domain.ExamSets;
using PhyGen.Domain.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Classes
{
    public class Class : Entity
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }

        public ICollection<Chapter> Chapters { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<ExamSet> ExamSets { get; set; }
    }

}
