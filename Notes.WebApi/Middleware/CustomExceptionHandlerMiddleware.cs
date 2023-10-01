using FluentValidation;
using Notes.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Notes.WebApi.Middleware
{
    /*
        * Это промежуточное ПО (middleware), предназначенное для обработки исключений 
          в ASP.NET Core. В ASP.NET Core промежуточное ПО - это компоненты, которые 
          реагируют на запросы HTTP и производят какую-то работу перед передачей 
          управления следующему промежуточному программному обеспечению в цепочке. 
          Они используются для многих разных задач, включая обработку исключений, 
          как в этом примере.

        * В этом классе находится метод Invoke, который вызывается для каждого HTTP-запроса. 
          Этот метод выполняет следующую последовательность действий:

            * Пробует выполнить следующее промежуточное ПО, используя await _next(context);.

            * Если исключение возникло в ходе выполнения запроса, оно будет поймано 
              блоком catch, который затем вызывает HandleExceptionAsync(context, exception), 
              чтобы обработать исключение.

            * HandleExceptionAsync проверяет тип исключения и на основе этого устанавливает 
              соответствующий HTTP-код состояния и формирует сообщение об ошибке в формате 
              JSON, которое будет возвращено в ответе.

            * Если исключение является ValidationException, то код статуса устанавливается 
              в BadRequest (400), и сериализуются в JSON ошибки валидации для включения в ответ.

            * Если исключение является NotFoundException, тогда код статуса устанавливается 
              как NotFound (404).

            * Если тип исключения другой, код состояния остается InternalServerError (500), 
              и в ответ включается сообщение исключения.

            * В конце HandleExceptionAsync записывает JSON-ответ в ответ HTTP.

        Примечательно, что в данном примере не обрабатываются все возможные типы исключений. 
        Если было выброшено исключение, которое не является ни ValidationException, 
        ни NotFoundException, оно все равно будет обрабатываться, и будет возвращен 
        код состояния InternalServerError (500).
    */
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationException.Errors);
                    break;
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }

            return context.Response.WriteAsync(result);
        }
    }
}
