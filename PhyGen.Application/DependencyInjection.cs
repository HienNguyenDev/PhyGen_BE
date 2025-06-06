using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PhyGen.Application.Abstractions.Behaviors;
using PhyGen.Application.Services.Questions;
namespace PhyGen.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
        services.AddScoped<QuestionService>();
        services.AddScoped<IQuestionService, QuestionService>();
        return services;
    }
}