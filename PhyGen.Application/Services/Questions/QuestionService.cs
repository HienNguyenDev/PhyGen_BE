using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Abstractions.Data;
using PhyGen.Domain.Common;
using PhyGen.Domain.Questions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace PhyGen.Application.Services.Questions
{
    public class QuestionService : IQuestionService
    {
        private readonly IApplicationDbContext _context;
        private readonly IAIService _aiService;
        private readonly IFileService _fileService;
        private readonly ICacheService _cacheService;

        public QuestionService(
            IApplicationDbContext context,
            IAIService aiService,
            IFileService fileService,
            ICacheService cacheService)
        {
            _context = context;
            _aiService = aiService;
            _fileService = fileService;
            _cacheService = cacheService;
        }

        public async Task<Result<QuestionDto>> ProcessQuestionImage(IFormFile file, CancellationToken cancellationToken = default)
        {
            try
            {
                using var stream = file.OpenReadStream();
                
                // Save image temporarily
                var tempImagePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.png");
                using (var fileStream = File.Create(tempImagePath))
                {
                    await stream.CopyToAsync(fileStream, cancellationToken);
                }

                // Preprocess image
                using (var image = Image.FromFile(tempImagePath))
                using (var processedImage = PreprocessImage(image))
                {
                    var processedPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_processed.png");
                    processedImage.Save(processedPath);

                    // Process with Tesseract OCR
                    string extractedText;
                    using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                    {
                        engine.SetVariable("preserve_interword_spaces", "1");
                        
                        using (var img = Pix.LoadFromFile(processedPath))
                        using (var page = engine.Process(img))
                        {
                            extractedText = page.GetText().Trim();
                        }
                    }

                    // Clean up temp files
                    File.Delete(tempImagePath);
                    File.Delete(processedPath);

                    // Process with AI for additional analysis
                    stream.Position = 0;
                    var aiAnalysis = await _aiService.ExtractTextFromImage(stream);

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = await _fileService.SaveFileAsync(stream, fileName);

                    var question = new Question
                    {
                        Content = extractedText,
                        AIAnalyzedContent = aiAnalysis,
                        ImagePath = filePath,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.Questions.AddAsync(question, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    return Result.Success(new QuestionDto
                    {
                        Id = question.Id,
                        Content = question.Content,
                        ImagePath = question.ImagePath
                    });
                }
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<QuestionDto>("Operation was cancelled");
            }
            catch (Exception ex)
            {
                return Result.Failure<QuestionDto>($"Failed to process question image: {ex.Message}");
            }
        }

        private Bitmap PreprocessImage(Image image)
        {
            var bitmap = new Bitmap(image);
            
            // Convert to grayscale and increase contrast
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    int gray = (int)((pixel.R * 0.3) + (pixel.G * 0.59) + (pixel.B * 0.11));
                    
                    // Increase contrast
                    if (gray > 127)
                        gray = 255;
                    else
                        gray = 0;
                        
                    bitmap.SetPixel(i, j, Color.FromArgb(pixel.A, gray, gray, gray));
                }
            }
            
            return bitmap;
        }

        public async Task<Result<QuestionAnalysisDto>> AnalyzeQuestion(Guid questionId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"question_analysis_{questionId}";
                var cached = await _cacheService.GetAsync<QuestionAnalysisDto>(cacheKey);
                if (cached != null)
                {
                    return Result.Success(cached);
                }

                var question = await _context.Questions
                    .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);

                if (question == null)
                {
                    return Result.Failure<QuestionAnalysisDto>("Question not found");
                }

                // Split questions into separate parts
                var questionParts = await _aiService.SplitQuestions(question.);

                var analysisResults = new List<QuestionPartAnalysis>();
                
                foreach (var part in questionParts)
                {
                    // Extract physics keywords using NLP
                    var keywords = await _aiService.ExtractPhysicsKeywords(part);

                    // Map keywords to curriculum database
                    var curriculumMapping = await _aiService.MapToCurriculum(keywords);

                    // Analyze question complexity and type
                    var complexity = await _aiService.AnalyzeComplexity(part, new
                    {
                        Length = part.Length,
                        FormulaCount = part.Count(c => c == '='),
                        KeywordCount = keywords.Count,
                        Structure = await _aiService.AnalyzeStructure(part)
                    });

                    analysisResults.Add(new QuestionPartAnalysis
                    {
                        Content = part,
                        Keywords = keywords,
                        Grade = curriculumMapping.Grade,
                        Chapter = curriculumMapping.Chapter,
                        QuestionType = await _aiService.DetermineQuestionType(part),
                        DifficultyLevel = complexity.DifficultyLevel,
                        ConfidenceScore = complexity.ConfidenceScore
                    });
                }

                var analysisDto = new QuestionAnalysisDto
                {
                    QuestionParts = analysisResults,
                    Grade = analysisResults.GroupBy(x => x.Grade)
                                 .OrderByDescending(g => g.Count())
                                 .First().Key,
                    Chapter = analysisResults.GroupBy(x => x.Chapter)
                                   .OrderByDescending(g => g.Count())
                                   .First().Key,
                    Keywords = analysisResults.SelectMany(x => x.Keywords).Distinct().ToList(),
                    DifficultyLevel = await _aiService.AggregateDifficulty(analysisResults),
                    AnalysisConfidence = analysisResults.Average(x => x.ConfidenceScore)
                };

                await _cacheService.SetAsync(cacheKey, analysisDto, TimeSpan.FromHours(24), cancellationToken);

                return Result.Success(analysisDto);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<QuestionAnalysisDto>("Operation was cancelled");
            }
            catch (Exception ex)
            {
                return Result.Failure<QuestionAnalysisDto>($"Failed to analyze question: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<QuestionDto>>> SearchQuestions(QuestionSearchRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _context.Questions.AsQueryable();

                if (!string.IsNullOrEmpty(request.Topic))
                    query = query.Where(q => q.Topic == request.Topic);

                if (request.Grade.HasValue)
                    query = query.Where(q => q.Grade == request.Grade);

                if (!string.IsNullOrEmpty(request.Chapter))
                    query = query.Where(q => q.Chapter == request.Chapter);

                var questions = await query.ToListAsync(cancellationToken);

                var dtos = questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Content = q.Content,
                    ImagePath = q.ImagePath,
                    Grade = q.Grade,
                    Chapter = q.Chapter,
                    Topic = q.Topic
                });

                return Result.Success(dtos);
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<IEnumerable<QuestionDto>>("Operation was cancelled");
            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<QuestionDto>>($"Failed to search questions: {ex.Message}");
            }
        }
    }
}
