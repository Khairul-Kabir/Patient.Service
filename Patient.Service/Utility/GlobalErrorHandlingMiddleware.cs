using Patient.Service.Exceptions;
using System.Net;
using System.Text.Json;
using System;
using UnauthorizedAccessException = System.UnauthorizedAccessException;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;
using NotImplementedException = System.NotImplementedException;

namespace Patient.Service.Utility
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private  readonly ILogger<GlobalErrorHandlingMiddleware> _logger;
        public GlobalErrorHandlingMiddleware(RequestDelegate requestDelegate, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;

        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        //private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        //{
        //    HttpStatusCode status;
        //    var stackTrace = String.Empty;
        //    string message;

        //    var exceptionType = exception.GetType();
        //    if (exceptionType == typeof(BadRequestException))
        //    {
        //        message = exception.Message;
        //        status = HttpStatusCode.BadRequest;
        //        stackTrace = exception.StackTrace;
        //    }
        //    else if (exceptionType == typeof(NotFoundException))
        //    {
        //        message = exception.Message;
        //        status = HttpStatusCode.NotFound;
        //        stackTrace = exception.StackTrace;
        //    }
        //    else if (exceptionType == typeof(NotImplementedException))
        //    {
        //        status = HttpStatusCode.NotImplemented;
        //        message = exception.Message;
        //        stackTrace = exception.StackTrace;
        //    }
        //    else if (exceptionType == typeof(UnauthorizedAccessException))
        //    {
        //        status = HttpStatusCode.Unauthorized;
        //        message = exception.Message;
        //        stackTrace = exception.StackTrace;
        //    }
        //    else if (exceptionType == typeof(KeyNotFoundException))
        //    {
        //        status = HttpStatusCode.Unauthorized;
        //        message = exception.Message;
        //        stackTrace = exception.StackTrace;
        //    }
        //    else
        //    {
        //        status = HttpStatusCode.InternalServerError;
        //        message = $"Custom Error Message:- {exception.Message}";
        //        stackTrace = exception.StackTrace;
        //    }

        //    var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace });
        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = (int)status;
        //    return context.Response.WriteAsync(exceptionResult);
        //}

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            var (status, message) = GetExceptionDetails(exception, logger);

            var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace = exception.StackTrace });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(exceptionResult);
        }

        private static (HttpStatusCode, string) GetExceptionDetails(Exception exception, ILogger logger)
        {
            HttpStatusCode status;
            string message;

            switch (exception)
            {
                case BadRequestException badRequestException:
                    (status, message) = (HttpStatusCode.BadRequest, badRequestException.Message);
                    break;

                case NotFoundException notFoundException:
                    (status, message) = (HttpStatusCode.NotFound, notFoundException.Message);
                    break;

                case NotImplementedException notImplementedException:
                    (status, message) = (HttpStatusCode.NotImplemented, notImplementedException.Message);
                    break;

                case UnauthorizedAccessException:
                case KeyNotFoundException:
                    (status, message) = (HttpStatusCode.Unauthorized, exception.Message);
                    break;

                default:
                    (status, message) = (HttpStatusCode.InternalServerError, $"Custom Error Message:- {exception.Message}");
                    logger.LogError($"{(int)status}- {message}");
                    break;
            }

            return (status, message);
        }

    }
}
