using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business
{
    public class BusinessResult<T>
    {
        public  enum enError:byte {NotFound,Conflict,BadRequest,Unauthorized,ServerError}
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public enError Error { get; set; }
        public T? Data { get; set; }

        public static BusinessResult<T> SuccessResult(string message = "Operation successful.", T? data = default(T))
        {
            return new BusinessResult<T> { Success = true, Message = message ,Data = data };
        }

        public static BusinessResult<T> FailureResult(string message = "Operation failed."
            ,enError error=enError.ServerError)
        {
            return new BusinessResult<T> 
            {
                Success = false,
                Message = message,
                Data = default(T),
                Error = error
            };
        }
    }
}
