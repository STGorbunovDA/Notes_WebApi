namespace Notes.WebApi.Middleware
{
    /*
        * Этот класс — это расширение для IApplicationBuilder, позволяющее интегрировать 
          использование обработчика исключений CustomExceptionHandlerMiddleware в цепочку 
          промежуточного программного обеспечения (миддлваров) для приложения ASP.NET Core.

        * Со внутренней стороны:

            * public static IApplicationBuilder UseCustomExceptionHandler
              (this IApplicationBuilder builder) это метод расширения для IApplicationBuilder. 
              this перед IApplicationBuilder builder означает, что этот метод может быть вызван 
              на любом объекте IApplicationBuilder.

            * Внутри этого расширения используется метод UseMiddleware<T>() 
              стандартного IApplicationBuilder, который добавляет промежуточное ПО обработки исключений.

        * На практике этот метод корректно интегрирует промежуточное ПО обработки исключений 
          в приложение. Вы можете использовать его в Startup.cs в следующем виде:
            * app.UseCustomExceptionHandler();
            
        * Это будет означать, что ваш кастомный обработчик исключений подключен 
          и будет обрабатывать все исключения, которые могут возникнуть при обработке 
          HTTP запросов приложением.
    */
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this
            IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
