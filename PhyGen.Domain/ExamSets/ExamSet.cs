using PhyGen.Domain.Classes;
using PhyGen.Domain.Common;
using PhyGen.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.ExamSets
{
    public class ExamSet : Entity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ClassId { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public int? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string ReviewNote { get; set; }

        public Class Class { get; set; }
        public User Creator { get; set; }
        public User Reviewer { get; set; }
        public ICollection<ExamSetQuestion> ExamSetQuestions { get; set; }
    }

}
