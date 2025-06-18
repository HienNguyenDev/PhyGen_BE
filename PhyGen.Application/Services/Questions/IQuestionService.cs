using Microsoft.AspNetCore.Http;
using PhyGen.Application.Common;
using PhyGen.Application.DTOs;
using PhyGen.Application.DTOs.Questions;
using PhyGen.Domain.Common;

public interface IQuestionService
{
    Task<ApiResult<List<QuestionDTO>>> ProcessQuestionImageFromPythonAsync(IFormFile file, CancellationToken cancellationToken);
    //Task<Result<QuestionAnalysisDto>> AnalyzeQuestion(int questionId, CancellationToken cancellationToken = default);
    //Task<Result<IEnumerable<QuestionDto>>> SearchQuestions(QuestionSearchRequest request, CancellationToken cancellationToken = default);
}