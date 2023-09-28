using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;

namespace Notes.Application.Notes.Queries.GetNoteList
{
    /*
        * В данном коде определен класс GetNoteListQueryHandler, который является 
          обработчиком запросов и реализует интерфейс IRequestHandler с типами аргументов 
          <GetNoteListQuery, NoteListVm>. Это указывает на использование паттерна 
          CQRS (Command Query Responsibility Segregation) в приложении. 
          CQRS разделяет чтение (запросы) и обновление данных (команды) 
          на разные классы-обработчики.

        * Структура класса:

            * _dbContext (тип INotesDbContext): ссылка на контекст базы данных заметок.
            
            * _mapper (тип IMapper): ссылка на экземпляр объекта AutoMapper, который 
              будет использоваться для преобразования объектов между классами.

            * Конструктор GetNoteListQueryHandler принимает два параметра: dbContext и mapper, 
              и инициализирует соответствующие private поля класса.

            * Метод Handle принимает два аргумента: request (тип GetNoteListQuery) 
              и cancellationToken (тип CancellationToken). Метод выполняет следующие действия:

                * Выполняет запрос к базе данных через _dbContext.Notes, 
                  где фильтрует заметки по UserId, используя 
                  Where(note => note.UserId == request.UserId).

                * Применяет проекцию к полученным результатам, преобразуя каждый 
                  элемент типа Note в экземпляр класса NoteLookupDto с использованием 
                  метода ProjectTo<NoteLookupDto>, который использует настройки AutoMapper 
                  из _mapper.ConfigurationProvider.

                * Загружает результаты запроса в список с использованием 
                  метода ToListAsync(cancellationToken).

                * Создает новый объект NoteListVm, инициализируя свойство Notes 
                  результатами запроса, и возвращает его.

        По сути, обработчик выполняет запрос к базе данных, получает список 
        заметок для указанного пользователя, автоматически преобразует объекты 
        Note в объекты NoteLookupDto и представляет результат в форме объекта 
        NoteListVm. Метод будет вызван другими компонентами, вероятно при 
        обработке HTTP-запросов от клиента, чтобы получить список заметок пользователя.
    */
    public class GetNoteListQueryHandler
        : IRequestHandler<GetNoteListQuery, NoteListVm>
    {
        private readonly INotesDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetNoteListQueryHandler(INotesDbContext dbContext, IMapper mapper) =>
           (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<NoteListVm> Handle(GetNoteListQuery request, 
            CancellationToken cancellationToken)
        {
            var notesQuery = await _dbContext.Notes
                .Where(note => note.UserId == request.UserId)
                .ProjectTo<NoteLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new NoteListVm { Notes = notesQuery };
        }
    }
}
