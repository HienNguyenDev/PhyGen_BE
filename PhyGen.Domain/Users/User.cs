using PhyGen.Domain.ExamSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IdentityId { get; set; }
        public UserRole Role { get; set; }
        public UserStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ExamSet> CreatedExamSets { get; set; }
        public ICollection<ExamSet> ReviewedExamSets { get; set; }
    }

}
