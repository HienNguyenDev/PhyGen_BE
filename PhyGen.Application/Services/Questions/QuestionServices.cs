using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PhyGen.Application.Abstractions.Data;
using PhyGen.Application.Common;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Application.DTOs;
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
using Tesseract;

namespace PhyGen.Application.Services.Questions
{
    public class QuestionServices : IQuestionServices
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        //private readonly ICacheService _cacheService;

        public QuestionServices(
            IQuestionRepository questionRepository, HttpClient httpClient, IUnitOfWork unitOfWork)
        //ICacheService cacheService)
        {
            _questionRepository = questionRepository;
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
            //_cacheService = cacheService;
        }
        public async Task<ApiResult<List<QuestionDTO>>> ProcessQuestionImageFromPythonAsync(IFormFile file, CancellationToken cancellationToken)
        {
            string tempImagePath = null;
            string url = "http://20.255.189.211:8000/question/label-data";

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
                    Chapter = q.Chapter,
                    Topic = q.Topic,
                }).ToList();

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

        public async Task<ApiResult<Page<Question>>> GetPaginatedAsync(int pageNumber, int pageSize, Guid? classId = null, Guid? chapterId = null, string? searchTerm = null)
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
    }
}
