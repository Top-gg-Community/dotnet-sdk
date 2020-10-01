using System;
using System.Net;

namespace DiscordBotList.Internal
{
    internal class ApiResult<T>
    {
        private ApiResult() { }

        internal static ApiResult<T> FromSuccess(T value)
            => new ApiResult<T> { Value = value, IsSuccess = true };

        internal static ApiResult<T> FromError(Exception ex)
            => new ApiResult<T> { Value = default, ErrorReason = ex.Message, IsSuccess = false };

        internal static ApiResult<T> FromHttpError(HttpStatusCode statusCode)
            => new ApiResult<T> { Value = default, ErrorReason = statusCode.ToString(), IsSuccess = false };

        internal T Value { get; private set; }

        internal string ErrorReason { get; private set; }

        internal bool IsSuccess { get; private set; }
    }
}
