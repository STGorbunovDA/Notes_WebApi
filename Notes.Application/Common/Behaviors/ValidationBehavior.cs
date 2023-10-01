using FluentValidation;
using MediatR;

namespace Notes.Application.Common.Behaviors
{
    /*
        * В данном коде создается общий класс ValidationBehavior<TRequest, TResponse>, 
          который реализует интерфейс IPipelineBehavior<TRequest, TResponse>. 
          ValidationBehavior выполняет валидацию входных данных при работе с запросами 
          в системе, когда запрос (типа TRequest) отправляется и возвращает результат (типа TResponse).

        * private readonly IEnumerable<IValidator<TRequest>> _validators; - это коллекция 
          валидаторов, подходящих для проверки данных запроса (TRequest).

        * Конструктор класса принимает эту коллекцию в качестве аргумента и инициализирует 
          приватное поле _validators.

        * Метод Handle - это основной метод класса, в котором выполняется валидация. 
          Он принимает три аргумента: объект запроса, следующий делегат обработчика и токен отмены.

        * Подробнее об алгоритме работы метода Handle:

            * var context = new ValidationContext<TRequest>(request); - создается новый 
              контекст валидации для входящего запроса.

            * var failures = _validators.Select(v => v.Validate(context))
                                        .SelectMany(result => result.Errors)
                                        .Where(failure => failure != null)
                                        .ToList();
            Здесь происходит следующее: для каждого валидатора из _validators 
            выполняется метод Validate с контекстом валидации, привязанным к запросу. 
            Результат валидации содержит коллекцию ошибок (если они есть). 
            Все эти ошибки собираются из результатов всех валидаторов, и если ошибка 
            не равна null, она добавляется в список ошибок failures.

            * if (failures.Count != 0){ throw new ValidationException(failures); }
            Если в списке failures имеются записи (то есть есть ошибки валидации), 
            то генерируется исключение ValidationException со списком ошибок.

            * return next(); - если дошли до этой строки, значит, ошибок валидации не было, 
              и вызов передается следующему обработчику в пайплайне.

        Этот класс обеспечивает промежуточный слой валидации для любого запроса/команды перед 
        его обработкой в системе, что помогает отлавливать ошибки на раннем этапе 
        и отклонять невалидные запросы.
    */
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
            _validators = validators;

        public Task<TResponse> Handle(TRequest request,
             RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
            return next();
        }
    }
}
