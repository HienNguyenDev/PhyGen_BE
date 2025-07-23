using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Abstractions.Authentication;
using PhyGen.Application.Common;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Application.DTOs.ExamSet;
using PhyGen.Application.DTOs.Questions;
using PhyGen.Application.Services.Questions;
using PhyGen.Domain.Classes;
using PhyGen.Domain.Common;
using PhyGen.Domain.ExamSets;
using PhyGen.Domain.Questions;

namespace PhyGen.Application.Services.ExamSets
{
    public class ExamSetServices : IExamSetServices
    {
        private readonly IExamSetRepository _examSetRepository;    
        private readonly IQuestionServices _questionServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly HttpClient _httpClient;

        public ExamSetServices(IExamSetRepository examSetRepository, IQuestionServices questionServices, IUnitOfWork unitOfWork, IUserContext userContext,  HttpClient httpClient, IDateTimeProvider dateTimeProvider)
        {
            _examSetRepository = examSetRepository;
            _questionServices = questionServices;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _httpClient = httpClient;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ApiResult<List<ExamResult>>> GenerateExamFromDropDownAsync(int quantity, int Chapter, int Topic, int Class)
        {
            try
            {
                var questions = await _questionServices.GenerateQuestionsFromDropDownAsync(quantity, Chapter, Topic, Class);

                var examSet = new ExamSet
                {
                    Id = Guid.NewGuid(),
                    Title = "ĐỀ THI VẬT LÍ",
                    Description = "Đề thi được tạo ra bởi AI",
                    ClassId = Class,
                    CreatedBy = _userContext.UserId, 
                    CreatedAt = _dateTimeProvider.UtcNow,
                    Status = "Generated",
                    ExamSetQuestions = new List<ExamSetQuestion>()
                };
                int order = 1;
                foreach (var q in questions)
                {
                    examSet.ExamSetQuestions.Add(new ExamSetQuestion
                    {
                        Id = Guid.NewGuid(),
                        ExamSetId = examSet.Id,
                        QuestionId = q.Id,
                        Order = order++
                    });
                }
                await _examSetRepository.AddAsync(examSet);
                await _unitOfWork.SaveChangesAsync();

                var examResults = questions.Select(q => new ExamResult
                {
                    Id = q.Id,
                    Question = q.QuestionContent,
                    ClassName = q.Class?.ClassName,
                    ChapterId = q.ChapterId,
                    ChapterName = q.Chapter?.ChapterName,
                    TopicId = q.TopicId,
                    TopicName = q.Topic?.TopicName,
                    Difficulty = q.Difficulty,
                    A = q.QuestionOptionSet?.A,
                    B = q.QuestionOptionSet?.B,
                    C = q.QuestionOptionSet?.C,
                    D = q.QuestionOptionSet?.D,
                    Answer = q.QuestionOptionSet?.Correct
                }).ToList();

                return ApiResult<List<ExamResult>>.Success(examResults, "Exam generated successfully from dropdown");
            }
            catch (Exception ex)
            {
                return ApiResult<List<ExamResult>>.Failure("Error: " + ex.Message);
            }
        }


        public async Task<ApiResult<List<ExamResult>>> GenerateExamFromPromptAsync(string prompt)
        {
            var pythonApiUrl = "http://20.212.140.228:8000/prompt/prompt";

            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(new { prompt }),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(pythonApiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return ApiResult<List<ExamResult>>.Failure($"Python API error: {response.StatusCode} - {errorMsg}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PromptApiResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Result?.Requests == null)
                    return ApiResult<List<ExamResult>>.Failure("Invalid response structure from prompt API");

                var questions = await _questionServices.GenerateQuestionsFromRequestsAsync(result.Result.Requests);

                var examSet = new ExamSet
                {
                    Id = Guid.NewGuid(),
                    Title = "ĐỀ THI VẬT LÍ",
                    Description = "Đề thi được tạo ra bởi AI",
                    ClassId = 1,
                    CreatedBy = _userContext.UserId,
                    CreatedAt = _dateTimeProvider.UtcNow,
                    Status = "Generated",
                    ExamSetQuestions = new List<ExamSetQuestion>()
                };

                int order = 1;
                foreach (var q in questions)    
                {
                    examSet.ExamSetQuestions.Add(new ExamSetQuestion
                    {
                        Id = Guid.NewGuid(),
                        ExamSetId = examSet.Id,
                        QuestionId = q.Id,
                        Order = order++
                    });
                }

                await _examSetRepository.AddAsync(examSet);
                await _unitOfWork.SaveChangesAsync();

                var examResults = questions.Select(q => new ExamResult
                {
                    Id = q.Id,
                    Question = q.QuestionContent,
                    ClassName = q.Class?.ClassName,
                    ChapterName = q.Chapter?.ChapterName,
                    TopicName = q.Topic?.TopicName,
                    Difficulty = q.Difficulty,
                    A = q.QuestionOptionSet.A,
                    B = q.QuestionOptionSet.B,
                    C = q.QuestionOptionSet.C,
                    D = q.QuestionOptionSet.D,
                    Answer = q.QuestionOptionSet.Correct
                }).ToList();

                return ApiResult<List<ExamResult>>.Success(examResults, "Exam generated successfully from prompt");
            }
            catch (Exception ex)
            {
                return ApiResult<List<ExamResult>>.Failure($"Exception occurred: {ex.Message}");
            }
        }
        public async Task<(byte[] FileBytes, string FileName)> GenerateExamDocxAsync(Guid examSetId)
        {
            var examSet = await _examSetRepository.GetByIdWithQuestionsAsync(examSetId);
            if (examSet == null)
                throw new Exception("Exam set not found.");

            using var ms = new MemoryStream();
            using (var wordDoc = WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
            {
                var mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());

                body.AppendChild(new Paragraph(new Run(new Text("ĐỀ THI VẬT LÍ")))
                {
                    ParagraphProperties = new ParagraphProperties(
                        new Justification() { Val = JustificationValues.Center })
                });

                body.AppendChild(new Paragraph(new Run(new Text("")))); 

                int index = 1;
                foreach (var q in examSet.ExamSetQuestions.OrderBy(x => x.Order))
                {
                    var question = q.Question;
                    body.AppendChild(new Paragraph(new Run(new Text($"Câu {index}: {ConvertLatexToReadable(question.QuestionContent)}"))));
                    body.AppendChild(new Paragraph(new Run(new Text($"A. {ConvertLatexToReadable(question.QuestionOptionSet.A)}"))));
                    body.AppendChild(new Paragraph(new Run(new Text($"B. {ConvertLatexToReadable(question.QuestionOptionSet.B)}"))));
                    body.AppendChild(new Paragraph(new Run(new Text($"C. {ConvertLatexToReadable(question.QuestionOptionSet.C)}"))));
                    body.AppendChild(new Paragraph(new Run(new Text($"D. {ConvertLatexToReadable(question.QuestionOptionSet.D)}"))));
                    body.AppendChild(new Paragraph(new Run(new Text(""))));

                    index++;
                }

                mainPart.Document.Save();
            }

            var fileName = $"DeThiVatLi_{_dateTimeProvider.UtcNow:yyyyMMddHHmmss}.docx";
            return (ms.ToArray(), fileName);
        }

        public async Task<ApiResult<ExamSet>> GetByIdAsync(Guid id)
        {
            var examSet = await _examSetRepository.GetByIdAsync(id);
            if (examSet == null)
                return ApiResult<ExamSet>.Failure("ExamSet not found");
            return ApiResult<ExamSet>.Success(examSet, "ExamSet retrieved successfully");
        }

        public async Task<ApiResult<Page<ExamSet>>> GetPaginatedAsync(int pageNumber, int pageSize, int? classId = null, Guid? createdBy = null, string? searchTerm = null)
        {
            var page = await _examSetRepository.GetExamSetsPaginatedAsync(pageNumber, pageSize, classId, createdBy, searchTerm);
            return ApiResult<Page<ExamSet>>.Success(page, "ExamSets paginated successfully");
        }

        public async Task<ApiResult<ExamSet>> CreateAsync(ExamSet examSet, CancellationToken cancellationToken)
        {
            await _examSetRepository.AddAsync(examSet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<ExamSet>.Success(examSet, "ExamSet created successfully");
        }

        public async Task<ApiResult<ExamSet>> UpdateAsync(ExamSet examSet, CancellationToken cancellationToken)
        {
            var existing = await _examSetRepository.GetByIdAsync(examSet.Id);
            if (existing == null)
                return ApiResult<ExamSet>.Failure("ExamSet not found");
            await _examSetRepository.UpdateAsync(examSet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<ExamSet>.Success(examSet, "ExamSet updated successfully");
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var examSet = await _examSetRepository.GetByIdAsync(id);
            if (examSet == null)
                return ApiResult<bool>.Failure("ExamSet not found");
            await _examSetRepository.RemoveAsync(examSet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<bool>.Success(true, "ExamSet deleted successfully");
        }
        private string ConvertLatexToReadable(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var output = input;

            // 1. Remove basic LaTeX formatting
            output = output.Replace("$", "")
                           .Replace(@"\,", " ")
                           .Replace(@"\quad", "    ")
                           .Replace(@"\text{", "")
                           .Replace(@"\mathrm{", "")
                           .Replace("}", "")
                           .Replace("{", "");

            // 2. Greek letters and constants
            output = output.Replace(@"\pi", "π")
                           .Replace(@"\alpha", "α")
                           .Replace(@"\beta", "β")
                           .Replace(@"\gamma", "γ")
                           .Replace(@"\delta", "δ")
                           .Replace(@"\epsilon", "ε")
                           .Replace(@"\lambda", "λ")
                           .Replace(@"\mu", "μ")
                           .Replace(@"\omega", "ω")
                           .Replace(@"\Omega", "Ω")
                           .Replace(@"\Delta", "Δ")
                           .Replace(@"\theta", "θ")
                           .Replace(@"\tau", "τ");

            // 3. Superscripts (common powers of 10)
            output = output.Replace("10^{-4}", "10⁻⁴")
                           .Replace("10^{-3}", "10⁻³")
                           .Replace("10^{-2}", "10⁻²")
                           .Replace("10^{-1}", "10⁻¹")
                           .Replace("10^0", "10⁰")
                           .Replace("10^1", "10¹")
                           .Replace("10^2", "10²")
                           .Replace("10^3", "10³")
                           .Replace("10^4", "10⁴");

            // 4. Multiplication symbol
            output = output.Replace(@"\cdot", "×");

            // 5. Fractions (\frac{a}{b} => a/b)
            output = Regex.Replace(output, @"\\frac\{([^{}]+)\}\{([^{}]+)\}", "$1/$2");

            // 6. Common physics units (just in case)
            output = output.Replace("rad", "rad")
                           .Replace("s", "s")
                           .Replace("m", "m")
                           .Replace("kg", "kg")
                           .Replace("N", "N")
                           .Replace("J", "J")
                           .Replace("Hz", "Hz")
                           .Replace("Pa", "Pa")
                           .Replace("W", "W")
                           .Replace("V", "V")
                           .Replace("A", "A")
                           .Replace("C", "C");

            return output.Trim();
        }

        public async Task<ApiResult<List<ExamSet>>> GetExamSetsByCurrentUserAsync()
        {
            var userId = _userContext.UserId;

            var examSets = await _examSetRepository.GetExamSetsByCreatorAsync(userId);

            if (examSets == null || !examSets.Any())
                return ApiResult<List<ExamSet>>.Failure("No exam sets found for current user");

            return ApiResult<List<ExamSet>>.Success(examSets.ToList(), "Exam sets retrieved successfully");
        }

    }
}
