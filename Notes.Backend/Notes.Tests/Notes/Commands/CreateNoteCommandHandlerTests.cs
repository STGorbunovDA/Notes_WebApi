using Microsoft.EntityFrameworkCore;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Tests.Common;
using Xunit;

namespace Notes.Tests.Notes.Commands
{
    /*
        * Этот фрагмент кода — это тестовый сценарий на языке C# для создания заметки 
          с использованием паттерна Command and Handler (Команда и Обработчик). Применяется 
          Unit Testing, которому нужна библиотека XUnit - это видно по атрибуту [Fact].

        * Вкратце, вот что происходит:

            * Arrange (Установка): Создаем экземпляр обработчика команд (handler), который 
              принимает Context. Context здесь, скорее всего, представляет доступ к базе данных. 
              Еще мы создаем два значения: noteName и noteDetails, которые будут использованы в тесте.

            * Act (Действие): Мы вызываем метод Handle обработчика, передавая новый экземпляр 
              CreateNoteCommand с параметрами Title и Details и UserId, которые мы ранее 
              установили. Метод возвращает noteId, который мы сохраняем для последующего использования.

            * Assert (Проверка): Мы утверждаем, что объект заметки с искомым ID, заголовком 
              и подробностями существует в контексте репозитория. Если заметка существует, 
              тест проходит.

        * Важно отметить, что Assert ожидает значение true. Если await Context.Notes.SingleOrDefaultAsync 
          возвращает null, тест завершится с ошибкой, потому что Assert.NotNull предполагает, 
          что возвращаемое значение не равно null.

        Тест подтверждает, что после выполнения команды CreateNoteCommand, в контексте находится 
        новая заметка со специфическими деталями.
    */
    public class CreateNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task CreateNoteCommandHandler_Success()
        {
            // Arrange
            var handler = new CreateNoteCommandHandler(Context);
            var noteName = "note name";
            var noteDetails = "note details";

            // Act
            var noteId = await handler.Handle(
                new CreateNoteCommand
                {
                    Title = noteName,
                    Details = noteDetails,
                    UserId = NotesContextFactory.UserAId
                },
                CancellationToken.None);

            // Assert
            Assert.NotNull(
                await Context.Notes.SingleOrDefaultAsync(note =>
                    note.Id == noteId && note.Title == noteName &&
                    note.Details == noteDetails));
        }
    }
}
