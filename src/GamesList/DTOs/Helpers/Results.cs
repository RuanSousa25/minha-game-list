namespace GamesList.Dtos.Helpers
{
    public class Results
    {
        public static ServiceResultDto<T> Ok<T>(T data) => ServiceResultDto<T>.Ok(data);
        public static ServiceResultDto<T> Ok<T>(string message) => ServiceResultDto<T>.Ok(message);
        public static ServiceResultDto<T> Ok<T>() => ServiceResultDto<T>.Ok();
        public static ServiceResultDto<T> Created<T>(T data) => ServiceResultDto<T>.Created(data);
        public static ServiceResultDto<T> NotFound<T>(string message) => ServiceResultDto<T>.NotFound(message);
        public static ServiceResultDto<T> BadRequest<T>(string message) => ServiceResultDto<T>.BadRequest(message);
        public static ServiceResultDto<T> ServerError<T>(string message) => ServiceResultDto<T>.ServerError(message);
        public static ServiceResultDto<T> Unauthorized<T>(string message) => ServiceResultDto<T>.Unauthorized(message);
        public static ServiceResultDto<T> Forbidden<T>(string message) => ServiceResultDto<T>.Forbidden(message);
        public static ServiceResultDto<T> ValidationError<T>(string message) => ServiceResultDto<T>.ValidationError(message);
        public static ServiceResultDto<T> Conflict<T>(string message) => ServiceResultDto<T>.Conflict(message);

    }
}