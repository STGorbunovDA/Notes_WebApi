using FluentValidation;

namespace Notes.Application.Notes.Commands.CreateNote
{
    /*
        * Этот класс "CreateNoteCommandValidator" проверяет, что введенные данные 
          для создания заметки (представленные в виде объекта типа "CreateNoteCommand") 
          удовлетворяют определенным условиям валидации.

        * В коде используется FluentValidation, популярная библиотека для .NET, 
          которая позволяет создавать валидационные правила, используя четкую 
          и производительную синтаксическую конструкцию.

        * Внутри конструктора устанавливаются два правила:

            * RuleFor(createNoteCommand => createNoteCommand.Title).NotEmpty().MaximumLength(250);
              Это правило говорит о том, что свойство "Title" в объекте CreateNoteCommand 
              не должно быть пустым (NotEmpty()) и длина его должна быть не более 
              250 символов (MaximumLength(250)).

            * RuleFor(createNoteCommand => createNoteCommand.UserId).NotEqual(Guid.Empty);
              Это правило говорит о том, что свойство "UserId" в объекте CreateNoteCommand 
              не должно быть равно пустому GUID (Guid.Empty).

        Таким образом, данный код отвечает за валидацию данных, нужных для создания 
        заметки, прежде чем эти данные будут использованы далее в программе.
    */
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(createNoteCommand =>
                createNoteCommand.Title).NotEmpty().MaximumLength(250);
            RuleFor(createNoteCommand =>
                createNoteCommand.UserId).NotEqual(Guid.Empty);
        }
    }
}
