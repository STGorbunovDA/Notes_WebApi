using MediatR;

namespace Notes.Application.Notes.Queries.GetNoteList
{
    /*
        * Здесь определён класс GetNoteListQuery, который реализует интерфейс IRequest<NoteListVm>. 
          Этот класс представляет собой запрос, целью которого является получение списка заметок 
          (представленных типом NoteListVm) для определенного пользователя.

        * Класс содержит одно свойство:

            * UserId (тип Guid): идентификатор пользователя, для которого 
              запрос будет получать список заметок.

        * Класс GetNoteListQuery используется для передачи данных запроса 
          в обработчик запроса, обычно называемый GetNoteListQueryHandler 
          (хотя имя может быть любым). Обработчик будет получать список заметок 
          для указанного UserId и возвращать его в форме объекта типа NoteListVm.
    */
    public class GetNoteListQuery : IRequest<NoteListVm>
    {
        public Guid UserId { get; set; }
    }
}
