using Microsoft.AspNetCore.Http;
using PhyGen.Application.DTOs;
using PhyGen.Domain.Common;

public interface IQuestionService
{
    Task<Result> ProcessQuestionImage(IFormFile file, CancellationToken cancellationToken = default);
    //Task<Result<QuestionAnalysisDto>> AnalyzeQuestion(int questionId, CancellationToken cancellationToken = default);
    //Task<Result<IEnumerable<QuestionDto>>> SearchQuestions(QuestionSearchRequest request, CancellationToken cancellationToken = default);
}