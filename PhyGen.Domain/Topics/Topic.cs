using PhyGen.Domain.Chapters;
using PhyGen.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Topics
{
    public class Topic : Entity
    {
        public Guid Id { get; set; }
        public Guid ChapterId { get; set; }
        public string Keyword { get; set; }
        public string Synonyms { get; set; }
        public Chapter Chapter { get; set; }
    }

}
