using FluentValidation;

namespace Notes.Application.Notes.Commands.DeleteCommand
{
    /*
        *  класс "DeleteNoteCommandValidator" выполняет валидацию объекта "DeleteNoteCommand", 
           который, скорее всего, содержит информацию, необходимую для удаления определенной заметки.

        * Валидатор устанавливает два правила:

            * RuleFor(deleteNoteCommand => deleteNoteCommand.Id).NotEqual(Guid.Empty);
              это правило говорит, что свойство "Id" в объекте DeleteNoteCommand не должно 
              быть равно пустому GUID (Guid.Empty). В смысле его сущности, "Id" скорее всего 
              представляет собой идентификатор удаляемой заметки.

            * RuleFor(deleteNoteCommand => deleteNoteCommand.UserId).NotEqual(Guid.Empty);
              это правило говорит, что свойство "UserId" в объекте DeleteNoteCommand не должно 
              быть равно пустому GUID (Guid.Empty). Вероятно, это идентификатор пользователя, 
              которому принадлежит заметка или который имеет права на её удаление.

        Таким образом, класс DeleteNoteCommandValidator гарантирует, что прежде чем 
        выполнить команду удаления, будет выполнена проверка на валидность идентификатора 
        заметки и идентификатора пользователя. 
    */
    public class DeleteNoteCommandValidator : AbstractValidator<DeleteNoteCommand>
    {
        public DeleteNoteCommandValidator()
        {
            RuleFor(deleteNoteCommand => deleteNoteCommand.Id).NotEqual(Guid.Empty);
            RuleFor(deleteNoteCommand => deleteNoteCommand.UserId).NotEqual(Guid.Empty);
        }
    }
}
