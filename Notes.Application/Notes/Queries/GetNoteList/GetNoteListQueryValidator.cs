using FluentValidation;

namespace Notes.Application.Notes.Queries.GetNoteList
{
    /*
        * Класс GetNoteListQueryValidator также является валидатором, но в этот раз 
          он работает с объектом GetNoteListQuery. Вероятно, GetNoteListQuery представляет 
          собой запрос на получение списка заметок для определенного пользователя.

        * Инициализатор GetNoteListQueryValidator устанавливает одно правило:

            * RuleFor(x => x.UserId).NotEqual(Guid.Empty);
              Это правило гарантирует, что свойство "UserId" в объекте GetNoteListQuery 
              не равно пустому GUID (Guid.Empty). "UserId", вероятно, представляет собой 
              идентификатор пользователя, список заметок которого мы хотим получить. 
              Правило гарантирует, что идентификатор пользователя является допустимым, 
              и помогает предотвратить попытки выполнения операции с недопустимыми 
              данными пользователя.

        В общем, GetNoteListQueryValidator используется для проверки валидности запроса 
        на получение списка заметок перед тем, как обрабатывать этот запрос.
    */
    public class GetNoteListQueryValidator : AbstractValidator<GetNoteListQuery>
    {
        public GetNoteListQueryValidator() 
        {
            RuleFor(x => x.UserId).NotEqual(Guid.Empty);
        }
    }
}
