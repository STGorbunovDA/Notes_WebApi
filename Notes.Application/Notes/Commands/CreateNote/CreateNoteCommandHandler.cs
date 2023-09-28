using MediatR;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.CreateNote
{
    /*
         * Этот код определяет класс CreateNoteCommandHandler, который реализует интерфейс 
           IRequestHandler<CreateNoteCommand, Guid>. Это обработчик команды создания заметки, 
           который отвечает за выполнение бизнес-логики, такой как создание нового объекта заметки 
           и сохранение его в базе данных.

         * Обработчик имеет две основных части:

            * Конструктор: Принимает INotesDbContext как параметр и инициализирует поле _dbContext. 
              Это позволяет обработчику работать с контекстом базы данных и добавлять новые заметки.

            * Метод Handle: Асинхронный метод, в котором выполняется бизнес-логика обработчика. 
              Он принимает два параметра: request, объект команды CreateNoteCommand, который содержит 
              информацию, необходимую для создания новой заметки, и cancellationToken, который используется 
              для отмены операции, если это необходимо.

            * В методе Handle происходит следующее:

                * Создается объект Note со свойствами, установленными из данных запроса 
                  (например, UserId, Title, Details) и некоторыми дополнительными свойствами 
                  (новый идентификатор заметки, дата создания).

                * Объект заметки добавляется в контекст базы данных с помощью метода AddAsync.

                * Изменения сохраняются в базе данных с помощью метода SaveChangesAsync.

                * Возвращается идентификатор созданной заметки.

        Когда система получает объект CreateNoteCommand, она передает его посреднику (MediatR), 
        который выбирает соответствующий обработчик. В этом случае это CreateNoteCommandHandler. 
        Затем обработчик выполняет свою логику и возвращает результат (созданный идентификатор заметки).
    */
    public class CreateNoteCommandHandler
        : IRequestHandler<CreateNoteCommand, Guid>
    {
        private readonly INotesDbContext _dbContext;

        public CreateNoteCommandHandler(INotesDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Guid> Handle(CreateNoteCommand request, 
            CancellationToken cancellationToken)
        {
            var note = new Note
            {
                UserId = request.UserId,
                Title = request.Title,
                Details = request.Details,
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                EditDate = null
            };

            await _dbContext.Notes.AddAsync(note, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return note.Id;
        }
    }
}
