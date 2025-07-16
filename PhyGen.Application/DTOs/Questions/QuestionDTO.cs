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
    public int? Difficulty { get; set; }
    public int? Chapter { get; set; }
    public int? Topic { get; set; }
    public string? ChapterName { get; set; }
    public string? TopicName { get; set; }
}