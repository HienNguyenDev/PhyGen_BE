using PhyGen.Api.Infrastructure;
using PhyGen.Domain.Common;

namespace PhyGen.Api.Extensions;

public static class ResultExtensions
{
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Result, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result<TIn>, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
    public static IResult MatchOk(this Result result)
    {
        if (result.IsSuccess)
        {
            return Microsoft.AspNetCore.Http.Results.Ok(ApiResult<object>.Success(null));
        }

        return CustomResults.Problem(result);
    }
    public static IResult MatchOk<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Microsoft.AspNetCore.Http.Results.Ok(ApiResult<T>.Success(result.Value));
        }
        return CustomResults.Problem(result);
    }

    public static IResult MatchCreated<T>(this Result<T> result, Func<T, string> urlFunc)
    {
        if (result.IsSuccess)
        {
            return Microsoft.AspNetCore.Http.Results.Created(
                urlFunc(result.Value),
                ApiResult<T>.Success(result.Value));
        }

        return CustomResults.Problem(result); ;
    }
}
