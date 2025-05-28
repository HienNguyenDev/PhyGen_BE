using PhyGen.Domain.Common;

namespace PhyGen.Domain.Questions.Errors;

public static class QuestionErrors
{
    public static readonly Error OperationCancelled = Error.Problem(
        "Question.OperationCancelled",
        "The question processing operation was cancelled.");

    public static readonly Error ImageProcessingFailed = Error.Problem(
        "Question.ImageProcessingFailed",
        "Failed to process the question image.");

    public static Error ProcessingError(string details) => Error.Problem(
        "Question.ProcessingError",
        $"Failed to process question: {details}");

    public static Error OCRError(string details) => Error.Problem(
        "Question.OCRError",
        $"Text extraction failed: {details}");

    public static Error ExceptionError(Exception ex) => Error.Problem(
        "Question.ExceptionError",
        $"An unexpected error occurred: {ex}");
}