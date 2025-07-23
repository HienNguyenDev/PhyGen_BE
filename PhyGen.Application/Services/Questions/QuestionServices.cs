using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Abstractions.Data;
using PhyGen.Application.Common;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Application.DTOs;
using PhyGen.Application.DTOs.ExamSet;
using PhyGen.Application.DTOs.OCR;
using PhyGen.Application.DTOs.Questions;
using PhyGen.Domain.Common;
using PhyGen.Domain.Questions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PhyGen.Application.Services.Questions
{
    public class QuestionServices : IQuestionServices
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IChapterRepository _chapterRepository;
        private readonly ITopicRepository _topicRepository;
        //private readonly ICacheService _cacheService;

        public QuestionServices(
            IQuestionRepository questionRepository, HttpClient httpClient, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, ITopicRepository topicRepository, IChapterRepository chapterRepository)
        //ICacheService cacheService)
        {
            _questionRepository = questionRepository;
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
            _dateTimeProvider = dateTimeProvider;
            _topicRepository = topicRepository;
            _chapterRepository = chapterRepository;
            //_cacheService = cacheService;
        }
        public async Task<ApiResult<List<QuestionDTO>>> ProcessQuestionImageFromPythonAsync(IFormFile file, CancellationToken cancellationToken)
        {
            string tempImagePath = null;
            string url = "http://20.212.140.228:8000/question/label-data";

            try
            {
                // 1. Validate input
                if (file == null || file.Length == 0)
                    return ApiResult<List<QuestionDTO>>.Failure("No image file was provided.");

                // 2. Save uploaded image temporarily
                tempImagePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");
                await using (var fileStream = File.Create(tempImagePath))
                {
                    await file.CopyToAsync(fileStream, cancellationToken);
                }

                // 3. Prepare multipart/form-data
                using var content = new MultipartFormDataContent();
                await using var stream = new FileStream(tempImagePath, FileMode.Open, FileAccess.Read);
                var imageContent = new StreamContent(stream);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType); // image/png or image/jpeg

                // 🟡 KEY phải là "file" để FastAPI nhận đúng
                content.Add(imageContent, "file", Path.GetFileName(tempImagePath));

                // 4. Gửi ảnh sang Python OCR API
                var response = await _httpClient.PostAsync(url, content, cancellationToken);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken);

                var ocrResult = JsonSerializer.Deserialize<OcrResponseDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (ocrResult?.Questions == null || ocrResult.Questions.Count == 0)
                    return ApiResult<List<QuestionDTO>>.Failure("OCR succeeded but no questions were detected.");

                // 5. Map to DTO
                
                var questions = ocrResult.Questions.Select(q => new QuestionDTO
                {
                    Question = q.Question,
                    A = q.A,
                    B = q.B,
                    C = q.C,
                    D = q.D,
                    Answer = q.Answer,
                    Difficulty = q.Difficulty,
                    Chapter = q.Chapter_id,     
                    Topic = q.Topic_id
                }).ToList();

                var chapterIds = questions
                    .Where(q => q.Chapter.HasValue)
                    .Select(q => q.Chapter.Value)
                    .Distinct()
                    .ToList();

                var topicIds = questions
                    .Where(q => q.Topic.HasValue)
                    .Select(q => q.Topic.Value)
                    .Distinct()
                    .ToList();

                var chapterDict = new Dictionary<int, string>();
                foreach (var chapterId in chapterIds)
                {
                    var chapter = await _chapterRepository.GetByIdAsync(chapterId);
                    if (chapter != null)
                        chapterDict[chapterId] = chapter.ChapterName;
                }

                var topicDict = new Dictionary<int, string>();
                foreach (var topicId in topicIds)
                {
                    var topic = await _topicRepository.GetByIdAsync(topicId);
                    if (topic != null)
                        topicDict[topicId] = topic.TopicName;
                }

                foreach (var dto in questions)
                {
                    if (dto.Chapter.HasValue && chapterDict.TryGetValue(dto.Chapter.Value, out var chapterName))
                        dto.ChapterName = chapterName;

                    if (dto.Topic.HasValue && topicDict.TryGetValue(dto.Topic.Value, out var topicName))
                        dto.TopicName = topicName;
                }

                //6.Save DB
                foreach (var questionDto in questions)
                {
                    var questionId = Guid.NewGuid();
                    var optionSetId = Guid.NewGuid();

                    var question = new Question
                    {
                        Id = questionId,
                        ClassId = 1,
                        ChapterId = questionDto.Chapter ?? 0,
                        TopicId = questionDto.Topic ?? 0,
                        QuestionContent = questionDto.Question ?? string.Empty,
                        Difficulty = questionDto.Difficulty ?? 1,
                        CreatedAt = _dateTimeProvider.UtcNow,
                        QuestionOptionSet = new QuestionOptionSet
                        {
                            Id = optionSetId,
                            QuestionId = questionId,
                            A = questionDto.A,
                            B = questionDto.B,
                            C = questionDto.C,
                            D = questionDto.D,
                            Correct = questionDto.Answer
                        },
                    };
                    await _questionRepository.AddAsync(question);
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return ApiResult<List<QuestionDTO>>.Success(questions, "Image processed and questions extracted successfully.");
            }
            catch (OperationCanceledException)
            {
                return ApiResult<List<QuestionDTO>>.Failure("The operation was canceled.");
            }
            catch (Exception ex)
            {
                return ApiResult<List<QuestionDTO>>.Failure($"An error occurred: {ex.Message}");
            }
            finally
            {
                // 6. Cleanup temp file
                try
                {
                    if (!string.IsNullOrEmpty(tempImagePath) && File.Exists(tempImagePath))
                        File.Delete(tempImagePath);
                }
                catch { }
            }
        }
        public async Task<ApiResult<Question>> GetByIdAsync(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
                return ApiResult<Question>.Failure("Question not found");
            return ApiResult<Question>.Success(question, "Question retrieved successfully");
        }

        public async Task<ApiResult<Page<Question>>> GetPaginatedAsync(int pageNumber, int pageSize, int? classId = null, int? chapterId = null, string? searchTerm = null)
        {
            var page = await _questionRepository.GetQuestionsPaginatedAsync(pageNumber, pageSize, classId, chapterId, searchTerm);
            return ApiResult<Page<Question>>.Success(page, "Questions paginated successfully");
        }

        public async Task<ApiResult<Question>> CreateAsync(Question question, CancellationToken cancellationToken)
        {
            await _questionRepository.AddAsync(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<Question>.Success(question, "Question created successfully");
        }

        public async Task<ApiResult<Question>> UpdateAsync(Question question, CancellationToken cancellationToken)
        {
            var existing = await _questionRepository.GetByIdAsync(question.Id);
            if (existing == null)
                return ApiResult<Question>.Failure("Question not found");
            await _questionRepository.UpdateAsync(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<Question>.Success(question, "Question updated successfully");
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
                return ApiResult<bool>.Failure("Question not found");
            await _questionRepository.RemoveAsync(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<bool>.Success(true, "Question deleted successfully");
        }
        public async Task<List<Question>> GenerateQuestionsFromRequestsAsync(List<QuestionRequest> requests)
        {
            var result = new List<Question>();
            var usedQuestionIds = new HashSet<Guid>();

            foreach (var req in requests)
            {
                var query = _questionRepository.GetQueryableWithIncludes();

                if (req.ChapterId.HasValue)
                    query = query.Where(q => q.ChapterId == req.ChapterId);

                if (req.TopicId.HasValue)
                    query = query.Where(q => q.TopicId == req.TopicId);

                if (req.DifficultyLevel.HasValue)
                    query = query.Where(q => q.Difficulty == req.DifficultyLevel);

                query = query.Where(q => !usedQuestionIds.Contains(q.Id));

                var selected = await query.OrderBy(q => Guid.NewGuid())
                                          .Take(req.Quantity)
                                          .ToListAsync();

                foreach (var q in selected)
                    usedQuestionIds.Add(q.Id);

                result.AddRange(selected);
            }

            return result;
        }

        public async Task<List<Question>> GenerateQuestionsFromDropDownAsync(int quantity, int? chapterId, int? topicId, int? classId)
        {
            var usedQuestionIds = new HashSet<Guid>();

            var query = _questionRepository.GetQueryableWithIncludes();

            if (chapterId.HasValue)
                query = query.Where(q => q.ChapterId == chapterId.Value);

            if (topicId.HasValue)
                query = query.Where(q => q.TopicId == topicId.Value);

            if (classId.HasValue)
                query = query.Where(q => q.ClassId == classId.Value);

            query = query.Where(q => !usedQuestionIds.Contains(q.Id));

            var selected = await query.OrderBy(q => Guid.NewGuid()) // để random
                                      .Take(quantity)
                                      .ToListAsync();

            foreach (var q in selected)
                usedQuestionIds.Add(q.Id);

            return selected;
        }
    }
}
