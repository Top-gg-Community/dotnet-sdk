using System;
using System.Net;

namespace DiscordBotsList.Api.Objects
{
    public class ApiResult<T>
    {
        private ApiResult()
        {
        }

        public T Value { get; private set; }

        /// <summary>
        ///     The error reason for this API request, if any.
        /// </summary>
        public string ErrorReason { get; private set; }

        public bool IsSuccess { get; private set; }

        internal static ApiResult<T> FromSuccess(T value)
        {
            return new ApiResult<T> { Value = value, IsSuccess = true };
        }

        internal static ApiResult<T> FromError(Exception ex)
        {
            return new ApiResult<T> { Value = default, ErrorReason = ex.Message, IsSuccess = false };
        }

        internal static ApiResult<T>
            FromHttpError(
                HttpStatusCode statusCode) // This could be altered to collect an object that provides more information
        {
            return new ApiResult<T> { Value = default, ErrorReason = statusCode.ToString(), IsSuccess = false };
        }
    }
}