
namespace Souq.Api.Helper.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string? message=null)
        {
            StatusCode = statusCode;
            Message = message??DefaultNotFoundMessage(statusCode);
        }

        private string? DefaultNotFoundMessage(int statusCode)
        {
            return statusCode switch
            {
                200 => "OK - The request was successful.",
                201 => "Created - A new resource has been created.",
                204 => "No Content - The request was successful, but there is no content to return.",
                400 => "Bad Request - The server cannot process the request due to client error.",
                401 => "Unauthorized - Authentication is required.",
                403 => "Forbidden - You don't have permission to access this resource.",
                404 => "Not Found - The requested resource could not be found.",
                500 => "Internal Server Error - The server encountered an error.",
                503 => "Service Unavailable - The server is not ready to handle the request.",
                _ => "Unknown Status Code"
            };
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }


    }
}
