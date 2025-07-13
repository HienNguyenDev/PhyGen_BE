using PhyGen.Domain.Chapters;
using PhyGen.Domain.Classes;
using PhyGen.Domain.Common;
using PhyGen.Domain.ExamSets;
using PhyGen.Domain.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Questions
{
    public class Question : Entity
    {
        public Guid Id { get; set; }
        public Guid QuestionOptionSetId { get; set; }
        public int? ClassId { get; set; }
        public int? ChapterId { get; set; }
        public int? TopicId { get; set; }
        public string QuestionContent { get; set; }
        public int? Difficulty { get; set; }
        public DateTime CreatedAt { get; set; }
        public Class Class { get; set; }
        public Chapter Chapter { get; set; }
        public Topic Topic { get; set; }
        public QuestionOptionSet QuestionOptionSet { get; set; }
        public ICollection<ExamSetQuestion> ExamSetQuestions { get; set; }
    }
}
