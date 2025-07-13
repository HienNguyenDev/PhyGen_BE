using Microsoft.AspNetCore.Http;
using PhyGen.Application.Common;
using PhyGen.Application.DTOs;
using PhyGen.Application.DTOs.Questions;
using PhyGen.Domain.Common;
using PhyGen.Domain.Questions;

public interface IQuestionServices
{
    Task<ApiResult<List<QuestionDTO>>> ProcessQuestionImageFromPythonAsync(IFormFile file, CancellationToken cancellationToken);
    //Task<Result<QuestionAnalysisDto>> AnalyzeQuestion(int questionId, CancellationToken cancellationToken = default);
    //Task<Result<IEnumerable<QuestionDto>>> SearchQuestions(QuestionSearchRequest request, CancellationToken cancellationToken = default);
    Task<ApiResult<Question>> GetByIdAsync(Guid id);
    Task<ApiResult<Page<Question>>> GetPaginatedAsync(int pageNumber, int pageSize, int? classId = null, int? chapterId = null, string? searchTerm = null);
    Task<ApiResult<Question>> CreateAsync(Question question, CancellationToken cancellationToken);
    Task<ApiResult<Question>> UpdateAsync(Question question, CancellationToken cancellationToken);
    Task<ApiResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
}