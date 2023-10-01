using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Notes.WebApi.Controllers
{
    /*
        * В данном примере определен абстрактный класс BaseController, 
          который расширяет класс ControllerBase. Этот базовый контроллер предоставляет 
          ряд общих функций для всех контроллеров в приложении.

        * В классе BaseController определены следующие особенности:

            * [ApiController]: атрибут, указывающий, что данный класс представляет 
              собой контроллер, используемый для обработки веб-API запросов. 
              Этот атрибут активирует определенные функции, специфичные для веб-API, 
              такие как автоматическая валидация модели и вывод информации об ошибках.

            * [Route("api/[controller]/[action]")]: атрибут, определяющий схему 
              маршрутизации для всех контроллеров, наследующих этот базовый класс. 
              Здесь используются заполнители [controller] и [action], которые 
              будут автоматически заменены на имена контроллера и действия. 
              Например, для контроллера NotesController и действия Get, 
              маршрут будет api/Notes/Get.

            * private IMediator _mediator: поле для хранения ссылки на экземпляр класса, 
              реализующего интерфейс IMediator. IMediator является частью паттерна 
              "Медиатор" (MediatR), используемого для обработки команд в приложении.

            * protected IMediator Mediator: свойство, предоставляющее доступ 
              к экземпляру IMediator. Если экземпляр еще не был инициализирован, 
              оно получает объект IMediator из сервисов текущего запроса.

            * internal Guid UserId: свойство, возвращающее идентификатор пользователя, 
              если пользователь аутентифицирован. Если пользователь не аутентифицирован, 
              оно возвращает Guid.Empty. Значение идентификатора пользователя получается 
              из списка утверждений у аутентифицированного пользователя (User.Claims).

        В результате, этот базовый контроллер упрощает код контроллеров, 
        наследующих его, предоставляя общие свойства и реализации маршрутизации по умолчанию.
    */
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        internal Guid UserId => !User.Identity.IsAuthenticated
            ? Guid.Empty
            : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
