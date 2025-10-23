using System.Net;

namespace TestX.api.CustomException
{
    public abstract class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorMessage { get; }
        protected ApiException(string message, HttpStatusCode statusCode, string errorMessage): base(message)
        {
            StatusCode = statusCode;
            ErrorMessage = message;
        }

        protected ApiException(string message, HttpStatusCode statuscode, string errorMessage, Exception? innerException): base(message, innerException)
        {
            StatusCode = statuscode;
            ErrorMessage = errorMessage;
        }
    }
    // xử lý lỗi 404
    public class NotFoundException : ApiException
    {
        public NotFoundException(string resource, object key)
        : base($"{resource} với {key} không tồn tại", HttpStatusCode.NotFound, "RESOURCE_NOT_FOUND")
        { }
    }
    // xử lý Khi sai dữ liệu đầu vào.
    public class ValidateException : ApiException
    {
        public Dictionary<string, string[]> Errors { get; }
        public ValidateException(Dictionary<string, string[]> errors): base("Dữ liệu không hợp lệ.", HttpStatusCode.BadRequest, "Validation_Error")
        {
            Errors = errors;
        }

        public ValidateException(string field, string error) : base($"Trường {field}: {error}"
            , HttpStatusCode.BadRequest, "Validation_Error")
        {
            Errors = new Dictionary<string, string[]>()
            {
                {field, new[] {error} }
            };
        }
    }
    // xử lý lỗi nghiệp vụ
    public class BusinessException : ApiException
    {
        public BusinessException(string message, string errorCode = "BUSINESS_ERROR")
        : base(message, HttpStatusCode.BadRequest, errorCode)
        { }
    }
    // 403
    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message = "Bạn không có quyền truy cập")
            : base(message, HttpStatusCode.Forbidden, "FORBIDDEN")
        { }
    }
    // lỗi chưa xác thực
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message = "Vui lòng đăng nhập")
            : base(message, HttpStatusCode.Unauthorized, "UNAUTHORIZED")
        { }
    }
    public class ConflictException : ApiException
    {
        public ConflictException(string message)
            : base(message, HttpStatusCode.Conflict, "CONFLICT")
        { }
    }
}
