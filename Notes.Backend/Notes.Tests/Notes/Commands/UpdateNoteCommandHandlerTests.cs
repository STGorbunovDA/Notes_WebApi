using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Tests.Common;
using Xunit;

namespace Notes.Tests.Notes.Commands
{
    /*
        * Этот код -- это набор тестов для класса UpdateNoteCommandHandler в C# используя 
          xUnit для юнит тестирования. UpdateNoteCommandHandler -- это командный обработчик, 
          который, как предполагается, обновляет заметки в каком-то контексте данных 
          (вероятно, в базе данных). В этом коде проверяются три основных сценария:

            * UpdateNoteCommandHandler_Success: Этот тест проверяет, что обработчик команды 
              успешно обновляет заметку с правильным идентификатором и идентификатором 
              пользователя. Наименование теста указывает, что ожидается успешное выполнение 
              тестируемого сценария.

            * UpdateNoteCommandHandler_FailOnWrongId: Этот тест проверяет, что обработчик 
              команды правильно сработает с ошибкой, если идентификатор заметки не найден. 
              Исключение NotFoundException должно быть выдано, если предоставлен несуществующий 
              идентификатор заметки. Это помогает гарантировать, что некорректные запросы 
              на обновление не будут проходить.

            * UpdateNoteCommandHandler_FailOnWrongUserId: Этот тест проверяет, что обработчик 
              команды правильно сработает с ошибкой, если представлен неверный идентификатор 
              пользователя. Опять же, ожидается, что будет выброшено исключение NotFoundException, 
              если идентификатор пользователя не корректен.

        Каждый тест поделен на три части: установка условий (Arrange), выполнение действия (Act) 
        и утверждение результата (Assert), что является общепринятым подходом при написании 
        автоматических тестов.
    */
    public class UpdateNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateNoteCommandHandler_Success()
        {
            // Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            var updatedTitle = "new title";

            // Act
            await handler.Handle(new UpdateNoteCommand
            {
                Id = NotesContextFactory.NoteIdForUpdate,
                UserId = NotesContextFactory.UserBId,
                Title = updatedTitle
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(await Context.Notes.SingleOrDefaultAsync(note =>
                note.Id == NotesContextFactory.NoteIdForUpdate &&
                note.Title == updatedTitle));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new UpdateNoteCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new UpdateNoteCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = NotesContextFactory.UserAId
                    },
                    CancellationToken.None));
        }

        [Fact]
        public async Task UpdateNoteCommandHandler_FailOnWrongUserId()
        {
            // Arrange
            var handler = new UpdateNoteCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(
                    new UpdateNoteCommand
                    {
                        Id = NotesContextFactory.NoteIdForUpdate,
                        UserId = NotesContextFactory.UserAId
                    },
                    CancellationToken.None);
            });
        }
    }
}
