using PhyGen.Domain.Questions;

namespace PhyGen.Application.DTOs.Questions;

public class QuestionDTO
{
    public int Number { get; set; }
    public string Text { get; set; }
    public string A { get; set; }
    public string B { get; set; }
    public string C { get; set; }
    public string D { get; set; }
    public string? Correct { get; set; }
}