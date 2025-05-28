using Microsoft.EntityFrameworkCore;
using PhyGen.Domain.Chapters;
using PhyGen.Domain.Classes;
using PhyGen.Domain.ExamSets;
using PhyGen.Domain.Questions;
using PhyGen.Domain.Topics;
using PhyGen.Domain.Users;

namespace PhyGen.Application.Abstractions.Data;

public interface IApplicationDbContext 
{
    DbSet<Class> Classes { get; set; }
    DbSet<Chapter> Chapters { get; set; }
    DbSet<Topic> Topics { get; set; }
    DbSet<QuestionType> QuestionTypes { get; set; }
    DbSet<QuestionTemplate> QuestionTemplates { get; set; }
    DbSet<Question> Questions { get; set; }
    DbSet<ExamSet> ExamSets { get; set; }
    DbSet<ExamSetQuestion> ExamSetQuestions { get; set; }
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
