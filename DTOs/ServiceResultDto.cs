namespace GamesList.DTOs
{
    public class ServiceResultDto<T>
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        // 200 - OK
        public static ServiceResultDto<T> Ok(T data) => new()
        {
            StatusCode = 200,
            Success = true,
            Message = "Operação realizada com sucesso.",
            Data = data
        };

        public static ServiceResultDto<T> Ok(string message) => new()
        {
            StatusCode = 200,
            Success = true,
            Message = message
        };

        public static ServiceResultDto<T> Ok() => new()
        {
            StatusCode = 200,
            Success = true,
            Message = "Operação realizada com sucesso."
        };

        // 201 - Created
        public static ServiceResultDto<T> Created(T data, string message = "Recurso criado com sucesso.") => new()
        {
            StatusCode = 201,
            Success = true,
            Message = message,
            Data = data
        };

        // 204 - No Content
        public static ServiceResultDto<T> NoContent() => new()
        {
            StatusCode = 204,
            Success = true,
            Message = "Sem conteúdo para retornar.",
            Data = default
        };

        // 400 - Bad Request
        public static ServiceResultDto<T> BadRequest(string message = "Requisição inválida.") => new()
        {
            StatusCode = 400,
            Success = false,
            Message = message
        };

        // 401 - Unauthorized
        public static ServiceResultDto<T> Unauthorized(string message = "Não autorizado.") => new()
        {
            StatusCode = 401,
            Success = false,
            Message = message
        };

        // 403 - Forbidden
        public static ServiceResultDto<T> Forbidden(string message = "Acesso negado.") => new()
        {
            StatusCode = 403,
            Success = false,
            Message = message
        };

        // 404 - Not Found
        public static ServiceResultDto<T> NotFound(string message = "Recurso não encontrado.") => new()
        {
            StatusCode = 404,
            Success = false,
            Message = message
        };

        // 409 - Conflict
        public static ServiceResultDto<T> Conflict(string message = "Conflito de dados.") => new()
        {
            StatusCode = 409,
            Success = false,
            Message = message
        };

        // 422 - Unprocessable Entity
        public static ServiceResultDto<T> ValidationError(string message = "Erro de validação.") => new()
        {
            StatusCode = 422,
            Success = false,
            Message = message
        };

        // 500 - Internal Server Error
        public static ServiceResultDto<T> ServerError(string message = "Erro interno no servidor.") => new()
        {
            StatusCode = 500,
            Success = false,
            Message = message
        };

        // Personalizado (fallback)
        public static ServiceResultDto<T> Custom(int statusCode, string message, bool success = false, T? data = default) => new()
        {
            StatusCode = statusCode,
            Success = success,
            Message = message,
            Data = data
        };
    }
}
