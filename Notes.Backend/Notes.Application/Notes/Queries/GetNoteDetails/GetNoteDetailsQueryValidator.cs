using FluentValidation;
using Notes.Application.Notes.Commands.CreateNote;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    /*
        * Класс GetNoteDetailsQueryValidator также является валидатором и относится 
          к классу GetNoteDetailsQuery. Вероятно, последний используется для получения 
          подробной информации о заметке по её уникальному идентификатору и идентификатору 
          пользователя. Валидатор устанавливает следующие правила для данных, 
          представленных в запросе:

            * RuleFor(note => note.Id).NotEqual(Guid.Empty);
              Это правило требует, чтобы свойство "Id" в объекте GetNoteDetailsQuery 
              не было равным пустому GUID (Guid.Empty). "Id" представляет собой уникальный 
              идентификатор заметки, информацию о которой хотим получить.

            * RuleFor(note => note.UserId).NotEqual(Guid.Empty);
              Это правило требует, чтобы свойство "UserId" в объекте GetNoteDetailsQuery 
              не было равным пустому GUID. "UserId", вероятно, представляет собой идентификатор 
              пользователя, по которому определяется достоверность запроса или владельца заметки.

         Итак, GetNoteDetailsQueryValidator используется для проверки валидности запроса 
         на получение подробностей о заметке перед тем, как производить действия с этим запросом. 
    */
    public class GetNoteDetailsQueryValidator : AbstractValidator<GetNoteDetailsQuery>
    {
        public GetNoteDetailsQueryValidator() 
        {
            RuleFor(note => note.Id).NotEqual(Guid.Empty);
            RuleFor(note => note.UserId).NotEqual(Guid.Empty);
        }
    }
}
