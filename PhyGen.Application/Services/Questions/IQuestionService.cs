public interface IQuestionService
{
    Task<Result<QuestionDto>> ProcessQuestionImage(IFormFile file, CancellationToken cancellationToken = default);
    Task<Result<QuestionAnalysisDto>> AnalyzeQuestion(int questionId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<QuestionDto>>> SearchQuestions(QuestionSearchRequest request, CancellationToken cancellationToken = default);
}