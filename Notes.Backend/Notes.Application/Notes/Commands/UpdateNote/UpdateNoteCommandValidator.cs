using FluentValidation;

namespace Notes.Application.Notes.Commands.UpdateNote
{
    /*
        * Здесь мы видим валидатор для класса "UpdateNoteCommand". Этот валидатор проверяет 
          данные, предоставленные для обновления существующей заметки, и включает в себя 
          следующие правила:

            * RuleFor(updateNoteCommand => updateNoteCommand.UserId).NotEqual(Guid.Empty);
              Это правило проверяет, что свойство "UserId" в объекте UpdateNoteCommand не 
              равно пустому GUID. Скорее всего, "UserId" — это идентификатор пользователя, 
              который производит обновление или к которому принадлежит заметка.

            * RuleFor(updateNoteCommand => updateNoteCommand.Id).NotEqual(Guid.Empty);
              Это правило проверяет, что свойство "Id" в объекте UpdateNoteCommand не равно пустому GUID. "Id", вероятно, это уникальный идентификатор заметки, которую нужно обновить.

            * RuleFor(updateNoteCommand => updateNoteCommand.Title).NotEmpty().MaximumLength(250);
              Это правило обеспечивает, что свойство "Title" в объекте UpdateNoteCommand 
              не пустое и его длина не превышает 250 символов.

        Таким образом, данный класс "UpdateNoteCommandValidator" выступает защитой 
        от попыток обновления заметок с недопустимыми данными.
    */
    public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteCommandValidator()
        {
            RuleFor(updateNoteCommand => updateNoteCommand.UserId).NotEqual(Guid.Empty);
            RuleFor(updateNoteCommand => updateNoteCommand.Id).NotEqual(Guid.Empty);
            RuleFor(updateNoteCommand => updateNoteCommand.Title)
                .NotEmpty().MaximumLength(250);
        }
    }
}
