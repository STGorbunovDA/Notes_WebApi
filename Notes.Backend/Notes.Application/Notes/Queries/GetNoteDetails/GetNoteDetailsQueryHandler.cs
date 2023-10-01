using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Interfaces;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    /*
        * Этот фрагмент кода определяет класс GetNoteDetailsQueryHandler, который реализует 
          интерфейс IRequestHandler<GetNoteDetailsQuery, NoteDetailsVm>. Это обработчик 
          запросов, имеющих тип GetNoteDetailsQuery и возвращающих объект типа NoteDetailsVm. 
          Этот обработчик используется в CQRS-паттерне или других паттернах, разделяющих запросы 
          и команды для более упорядоченной структуры кода.

        * Класс содержит два поля:

            * _dbContext (тип INotesDbContext): Интерфейс контекста базы данных заметок.
            * _mapper (тип IMapper): Интерфейс, который определен в библиотеке AutoMapper 
              и обеспечивает функциональность для маппинга между разными объектами.

        * Конструктор класса принимает два параметра, dbContext и mapper, 
          и присваивает их соответствующим полям.

        * В классе определен метод Handle, который асинхронно обрабатывает запрос 
          на получение детальной информации о заметке:

            * Метод Handle принимает параметры: запрос request типа GetNoteDetailsQuery 
              и cancellationToken типа CancellationToken.
            * Он выполняет запрос к базе данных, который ищет заметку с идентификатором, 
              указанным в request.Id, и выбирает первую найденную запись.
            * Если заметка не найдена или её UserId не соответствует request.UserId, 
              метод генерирует исключение NotFoundException.
            * Если заметка найдена и соответствует пользователю, метод Handle выполняет 
              маппинг из Note в NoteDetailsVm с использованием объекта _mapper и затем 
              возвращает этот объект типа NoteDetailsVm.

        В результате выполнения обработчика GetNoteDetailsQueryHandler, клиент получает 
        детальную информацию о заметке на основе указанных критериев и идентификатора.
    */
    public class GetNoteDetailsQueryHandler
        : IRequestHandler<GetNoteDetailsQuery, NoteDetailsVm>
    {
        private readonly INotesDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetNoteDetailsQueryHandler(INotesDbContext dbContext, IMapper mapper) =>
            (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<NoteDetailsVm> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notes
                 .FirstOrDefaultAsync(note =>
                 note.Id == request.Id, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            return _mapper.Map<NoteDetailsVm>(entity);
        }
    }
}
