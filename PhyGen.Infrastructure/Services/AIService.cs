//using Microsoft.Extensions.Configuration;
//using PhyGen.Application.Common.Interfaces;
//using PhyGen.Domain.Chapters;
//using PhyGen.Domain.Classes;
//using PhyGen.Domain.Questions;
//using PhyGen.Infrastructure.Database;
//using System.Net.Http.Json;
//using System.Text.Json;

//namespace PhyGen.Infrastructure.Services;

//public class AIService : IAIService
//{
//    private readonly HttpClient _httpClient;
//    private readonly IConfiguration _configuration;
//    private readonly ApplicationDbContext _context;

//    public AIService(HttpClient httpClient, IConfiguration configuration, ApplicationDbContext context)
//    {
//        _httpClient = httpClient;
//        _configuration = configuration;
//        _context = context;
//    }

//    public async Task<string> ExtractTextFromImage(Stream imageStream)
//    {
//        var formData = new MultipartFormDataContent();
//        formData.Add(new StreamContent(imageStream), "image", "question.jpg");

//        var response = await _httpClient.PostAsync($"{_configuration["AIService:Url"]}/ocr", formData);
//        response.EnsureSuccessStatusCode();

//        var result = await response.Content.ReadAsStringAsync();
//        return result;
//    }

//    public async Task<List<string>> SplitQuestions(string content)
//    {
//        var request = new { text = content };
//        var response = await _httpClient.PostAsJsonAsync($"{_configuration["AIService:Url"]}/split", request);
//        response.EnsureSuccessStatusCode();

//        var result = await response.Content.ReadFromJsonAsync<List<string>>();
//        return result ?? new List<string>();
//    }

//    public async Task<List<string>> ExtractPhysicsKeywords(string question)
//    {
//        var request = new { text = question };
//        var response = await _httpClient.PostAsJsonAsync($"{_configuration["AIService:Url"]}/keywords", request);
//        response.EnsureSuccessStatusCode();

//        var result = await response.Content.ReadFromJsonAsync<List<string>>();
//        return result ?? new List<string>();
//    }

//    public async Task<Question> MapQuestionToProgram(string questionText)
//    {
//        // Get AI analysis result
//        var request = new { text = questionText };
//        var response = await _httpClient.PostAsJsonAsync($"{_configuration["AIService:Url"]}/analyze-content", request);
//        response.EnsureSuccessStatusCode();

//        var aiResult = await response.Content.ReadFromJsonAsync<AIAnalysisResult>();
//        if (aiResult == null) return CreateDefaultQuestion(questionText);

//        // Match with database records
//        var matchedClass = await _context.Classes
//            .FirstOrDefaultAsync(c => c.ClassName.Contains(aiResult.Grade.ToString()));

//        var matchedChapter = await _context.Chapters
//            .FirstOrDefaultAsync(c => c.ChapterName.Contains(aiResult.ChapterName));

//        var matchedType = await _context.QuestionTypes
//            .FirstOrDefaultAsync(t => t.TypeName.Contains(aiResult.QuestionType));

//        // Create question with matched IDs
//        var question = new Question
//        {
//            QuestionText = questionText,
//            ClassId = matchedClass?.Id ?? Guid.Empty,
//            ChapterId = matchedChapter?.Id ?? Guid.Empty,
//            TypeId = matchedType?.Id ?? Guid.Empty,
//            CreatedAt = DateTime.UtcNow,
//            AIConfidence = CalculateConfidence(matchedClass, matchedChapter, matchedType)
//        };

//        return question;
//    }

//    private float CalculateConfidence(Class? matchedClass, Chapter? matchedChapter, QuestionType? matchedType)
//    {
//        float confidence = 0;
//        if (matchedClass != null) confidence += 0.33f;
//        if (matchedChapter != null) confidence += 0.33f;
//        if (matchedType != null) confidence += 0.34f;
//        return confidence;
//    }

//    private Question CreateDefaultQuestion(string questionText)
//    {
//        return new Question
//        {
//            QuestionText = questionText,
//            CreatedAt = DateTime.UtcNow,
//            AIConfidence = 0
//        };
//    }

//    public async Task<ComplexityAnalysis> AnalyzeComplexity(string question, object features)
//    {
//        var request = new { text = question, features };
//        var response = await _httpClient.PostAsJsonAsync($"{_configuration["AIService:Url"]}/complexity", request);
//        response.EnsureSuccessStatusCode();

//        var result = await response.Content.ReadFromJsonAsync<ComplexityAnalysis>();
//        return result ?? new ComplexityAnalysis();
//    }

//    public async Task<string> AnalyzeStructure(string question)
//    {
//        var request = new { text = question };
//        var response = await _httpClient.PostAsJsonAsync($"{_configuration["AIService:Url"]}/structure", request);
//        response.EnsureSuccessStatusCode();

//        var result = await response.Content.ReadAsStringAsync();
//        return result;
//    }

//    public async Task<string> DetermineQuestionType(string question)
//    {
//        var request = new { text = question };
//        var response = await _httpClient.PostAsJsonAsync($"{_configuration["AIService:Url"]}/question-type", request);
//        response.EnsureSuccessStatusCode();

//        var result = await response.Content.ReadAsStringAsync();
//        return result;
//    }

//    public async Task<string> AggregateDifficulty(List<QuestionPartAnalysis> analyses)
//    {
//        var request = new { analyses };
//        var response = await _httpClient.PostAsJsonAsync($"{_configuration["AIService:Url"]}/aggregate-difficulty", request);
//        response.EnsureSuccessStatusCode();

//        var result = await response.Content.ReadAsStringAsync();
//        return result;
//    }

//    public async Task<QuestionAnalysis> AnalyzeQuestionText(string text)
//    {
//        var content = new StringContent(
//            JsonSerializer.Serialize(new { text }), 
//            System.Text.Encoding.UTF8, 
//            "application/json");

//        var response = await _httpClient.PostAsync($"{_configuration["AIService:Url"]}/analyze", content);
//        response.EnsureSuccessStatusCode();

//        var result = await response.Content.ReadFromJsonAsync<QuestionAnalysis>();
//        return result ?? throw new Exception("Failed to analyze question");
//    }
//}