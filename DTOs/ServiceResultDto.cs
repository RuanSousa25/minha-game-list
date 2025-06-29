namespace GamesList.DTOs
{
    public class ServiceResultDto<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public static ServiceResultDto<T> Ok(T data) => new() { Success = true, Data = data };
        public static ServiceResultDto<T> Fail(string errorMessage) => new() { Success = true, Message = errorMessage };
        
    }
}