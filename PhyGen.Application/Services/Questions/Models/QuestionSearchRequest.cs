//using FluentValidation;

//public class QuestionSearchRequest
//{
//    public string? Topic { get; set; }
//    public int? Grade { get; set; }
//    public string? Chapter { get; set; }
//    public int Page { get; set; } = 1;
//    public int PageSize { get; set; } = 10;
//}

//public class QuestionSearchRequestValidator : AbstractValidator<QuestionSearchRequest>
//{
//    public QuestionSearchRequestValidator()
//    {
//        RuleFor(x => x.Page)
//            .GreaterThan(0);

//        RuleFor(x => x.PageSize)
//            .GreaterThan(0)
//            .LessThanOrEqualTo(50);

//        When(x => x.Grade.HasValue, () =>
//        {
//            RuleFor(x => x.Grade)
//                .InclusiveBetween(10, 12);
//        });
//    }
//}