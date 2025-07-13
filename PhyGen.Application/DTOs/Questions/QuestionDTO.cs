using PhyGen.Domain.Questions;

namespace PhyGen.Application.DTOs.Questions;

public class QuestionDTO
{
    public string? Question { get; set; }
    public string? A { get; set; }
    public string? B { get; set; }
    public string? C { get; set; }
    public string? D { get; set; }
    public string? Answer { get; set; }
    public string? Difficulty { get; set; }
    public string? Chapter { get; set; }
    public string? Topic { get; set; }
}