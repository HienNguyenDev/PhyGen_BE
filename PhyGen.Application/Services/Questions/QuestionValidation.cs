using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace PhyGen.Application.Services.Questions
{
    public class QuestionFileValidation : AbstractValidator<IFormFile>
    {
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };

        public QuestionFileValidation()
        {
            RuleFor(file => file.FileName)
                .NotEmpty()
                .Must(fileName => allowedExtensions.Contains(Path.GetExtension(fileName).ToLower()))
                .WithMessage($"Only these file formats are allowed: {string.Join(", ", allowedExtensions)}");
        }
    }
}
