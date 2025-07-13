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
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public string TopicName { get; set; }
        public Chapter Chapter { get; set; }
    }

}
