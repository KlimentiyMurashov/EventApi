namespace EventsApi.Middleware
{
	public class MiddlewareException
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<MiddlewareException> _logger;

		public MiddlewareException(RequestDelegate next, ILogger<MiddlewareException> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context); 
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Something went wrong: {ex.Message}");

				await HandleExceptionAsync(context, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = exception switch
			{
				ArgumentNullException => StatusCodes.Status400BadRequest,
				InvalidOperationException => StatusCodes.Status404NotFound, 
				UnauthorizedAccessException => StatusCodes.Status401Unauthorized, 
				_ => StatusCodes.Status500InternalServerError 
			};

			var result = new
			{
				StatusCode = context.Response.StatusCode,
				Message = exception.Message,
				Details = context.Response.StatusCode == StatusCodes.Status500InternalServerError
							? "Internal server error"
							: null
			};

			return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
		}
	}
}
