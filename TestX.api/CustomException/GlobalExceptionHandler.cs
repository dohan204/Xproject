namespace TestX.api.CustomException
{
    public class GlobalExceptionHandler 
    {
        public static IResult HandleException(Exception ex)
        {
            if(ex is ApiException apiEx)
            {
                var response = new
                {
                    error = apiEx.ErrorMessage,
                    message = apiEx.Message,
                    details = apiEx is ValidateException valEx ? valEx.Errors : null
                };
                return Results.Json(response, statusCode: (int)apiEx.StatusCode);
            }
            return Results.Json(new
            {
                error = "Internal_Error",
                message = "Có lỗi đã sảy ra, vui lòng thử lại sau."
            }, statusCode: 500);
        }
    }
}
