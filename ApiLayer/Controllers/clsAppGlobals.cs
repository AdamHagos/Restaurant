using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RestaurantDTOs;

namespace RestaurantApi.Controllers
{
    public static class clsAppGlobals
    {
        public static IActionResult HandleError(Exception ex, string UKConstraintViolationMessage = "This record already exists. Please use a different value.")
        {
            switch (ex)
            {
                case SqlException sqlEx:
                    return HandleSqlException(sqlEx, UKConstraintViolationMessage);

                case TimeoutException:
                    return new ObjectResult("Request timed out. Please try again.")
                    { StatusCode = 408 };

                default:
                    return new ObjectResult("Service temporarily unavailable.")
                    { StatusCode = 503 };
            }
        }

        private static IActionResult HandleSqlException(SqlException sqlEx, string UKConstraintViolationMessage)
        {
            // Determine user-friendly message
            var userMessage = GetSafeUserMessage(sqlEx, UKConstraintViolationMessage);

            return new ObjectResult(userMessage)
            {
                StatusCode = IsServerError(sqlEx) ? 503 : 400
            };
        }

        private static string GetSafeUserMessage(SqlException sqlEx, string UKConstraintViolationMessage)
        {
            foreach (SqlError error in sqlEx.Errors)
            {
                switch (error.Number)
                {
                    // Business logic errors - can be somewhat specific
                    case 2627:
                    case 2601:
                        return UKConstraintViolationMessage;

                    // Server infrastructure errors - always generic
                    case 18456:
                    case 4060:
                    case 0:
                    case 53:
                    case -2:
                    case 1205:
                    case 17142:
                    case 40197:
                    case 40613:
                        return "Our systems are temporarily unavailable. Please try again in a few minutes.";

                    default:
                        return IsServerError(sqlEx)
                            ? "Service temporarily unavailable. Please try again later."
                            : "We encountered an error processing your request.";
                }
            }
            return "An unexpected error occurred.";
        }

        private static bool IsServerError(SqlException sqlEx)
        {
            foreach (SqlError error in sqlEx.Errors)
            {
                if (error.Number == 2627 || error.Number == 2601)
                    return false; // Business logic error

                if (error.Number == 18456 || error.Number == 0 || error.Number == 53 ||
                    error.Number == -2 || error.Number == 1205)
                    return true; // Server error
            }
            return true; // When in doubt, treat as server error
        }
    }
}
