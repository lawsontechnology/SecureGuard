namespace Visitor_Management_System.Core.DTOs
{
        public class BaseResponse<T>
        {
            public bool Status { get; set; }

            public string? Message { get; set; }

            public T? Data { get; set; }
        }
    
}
