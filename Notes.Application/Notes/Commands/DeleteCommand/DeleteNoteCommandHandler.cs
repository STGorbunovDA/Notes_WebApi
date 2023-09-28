using MediatR;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.DeleteCommand
{
    /*
        * Этот код определяет класс DeleteNoteCommandHandler, который реализует интерфейс 
          IRequestHandler<DeleteNoteCommand, Unit>. Это обработчик команды удаления заметки, 
          который отвечает за выполнение бизнес-логики удаления заметки из базы данных.

        * Обработчик имеет две основных части:

            * Конструктор: Принимает INotesDbContext как параметр и инициализирует поле _dbContext. 
              Это позволяет обработчику работать с контекстом базы данных и удалять заметки.

            * Метод Handle: Асинхронный метод, в котором выполняется бизнес-логика обработчика. 
              Он принимает два параметра: request, объект команды DeleteNoteCommand, 
              который содержит информацию, необходимую для удаления заметки, и cancellationToken, 
              который используется для отмены операции, если это необходимо.

            * В методе Handle происходит следующее:

                * Поиск заметки с заданным идентификатором request.Id в контексте базы данных.

                * Проверка, существует ли заметка и соответствует ли UserId заметки идентификатору 
                  пользователя из запроса request.UserId. Если заметка не найдена или пользователям 
                  не разрешено удалять заметку, возникает исключение NotFoundException.

                * Удаление найденной заметки из контекста базы данных с помощью метода Remove.

                * Сохранение изменений в базе данных с помощью метода SaveChangesAsync.

                * Возвращение типа Unit.Value, который представляет "ничего" (или "пустоту") 
                  в качестве результата выполнения команды, поскольку для команды удаления 
                  не нужно возвращать конкретное значение.

        Когда система получает объект DeleteNoteCommand, она передает его посреднику (MediatR), 
        который выбирает соответствующий обработчик. В этом случае это DeleteNoteCommandHandler. 
        Затем обработчик выполняет свою логику по удалению заметки на основе данных из команды.
    */
    public class DeleteNoteCommandHandler
        : IRequestHandler<DeleteNoteCommand, Unit>
    {
        private readonly INotesDbContext _dbContext;

        public DeleteNoteCommandHandler(INotesDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Unit> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notes
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            _dbContext.Notes.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
