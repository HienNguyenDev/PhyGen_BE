using PhyGen.Domain.Classes;
using PhyGen.Domain.Common;
using PhyGen.Domain.Questions;
using PhyGen.Domain.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Chapters
{
    public class Chapter : Entity
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string ChapterName { get; set; }
        public string ChapterCode { get; set; }
        public Class Class { get; set; }

        public ICollection<Topic> Topics { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<QuestionTemplate> QuestionTemplates { get; set; }
    }
}
