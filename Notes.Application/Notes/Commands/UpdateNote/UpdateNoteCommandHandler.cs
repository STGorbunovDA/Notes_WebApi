using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Commands.UpdateNote
{
    /*
        * Этот фрагмент кода представляет собой класс UpdateNoteCommandHandler, 
          который является обработчиком команды обновления заметки и реализует интерфейс 
          IRequestHandler<UpdateNoteCommand, Unit>. Задачей обработчика является применение 
          изменений к сущности заметки в базе данных.

        * Класс имеет следующие основные элементы:

            * Поле _dbContext: Это приватное поле, которое хранит ссылку на контекст базы данных 
              (реализующий интерфейс INotesDbContext). Контекст базы данных будет 
              внедрен через конструктор класса.

            * Конструктор: Конструктор класса принимает контекст базы данных (INotesDbContext) 
              как аргумент и присваивает его значение приватному полю _dbContext.

            * Метод Handle: Этот метод реализует интерфейс IRequestHandler. Он должен выполнить 
              обновление заметки на основе данных, предоставленных в объекте команды UpdateNoteCommand. 
              Метод выполняет следующие действия:

                * Найти заметку в базе данных с использованием контекста базы данных, 
                  ища заметку с идентификатором, соответствующим значению request.Id.
                * Проверить, найдена ли заметка и является ли пользователь, 
                  выполняющий команду, владельцем этой заметки (entity.UserId == request.UserId). 
                  Если заметка не найдена или пользователь не соответствует, 
                  выбрасывается исключение NotFoundException.
                * Обновить свойства заметки (детали, заголовок и дату редактирования) согласно данным 
                  из объекта команды.
                * Сохранить изменения в базе данных с использованием SaveChangesAsync.
                * Вернуть значение Unit.Value, что указывает на успешное выполнение команды.
                
        В результате выполнения данного обработчика происходит обновление 
        заметки в базе данных с новыми деталями, заголовком и датой редактирования, 
        если все условия соблюдены.
    */
    public class UpdateNoteCommandHandler
        : IRequestHandler<UpdateNoteCommand, Unit>
    {
        private readonly INotesDbContext _dbContext;

        public UpdateNoteCommandHandler(INotesDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Unit> Handle(UpdateNoteCommand request,
           CancellationToken cancellationToken)
        {
            var entity =
                await _dbContext.Notes.FirstOrDefaultAsync(note =>
                    note.Id == request.Id, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            entity.Details = request.Details;
            entity.Title = request.Title;
            entity.EditDate = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
