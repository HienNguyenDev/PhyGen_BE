using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Abstractions.Data;
using PhyGen.Domain.Chapters;
using PhyGen.Domain.Classes;
using PhyGen.Domain.Common;
using PhyGen.Domain.ExamSets;
using PhyGen.Domain.Questions;
using PhyGen.Domain.Topics;
using PhyGen.Domain.Users;

namespace PhyGen.Infrastructure.Database;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Class> Classes { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<QuestionTemplate> QuestionTemplates { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<ExamSet> ExamSets { get; set; }
    public DbSet<ExamSetQuestion> ExamSetQuestions { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(Schemas.Default);
    }
}
