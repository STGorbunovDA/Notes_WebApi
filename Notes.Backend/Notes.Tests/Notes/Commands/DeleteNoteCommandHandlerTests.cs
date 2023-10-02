using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteCommand;
using Notes.Tests.Common;
using Xunit;

namespace Notes.Tests.Notes.Commands
{
    /*
        * Здесь представлены тесты для класса DeleteNoteCommandHandler, который, по всей видимости, 
          отвечает за обработку команды удаления заметки.

        * Тестовый класс содержит три теста:

            * DeleteNoteCommandHandler_Success: Этот тест проверяет успех операции удаления, 
              когда переданы корректные ID заметки и пользователя. Он создает инстанс обработчика 
              команды удаления заметок с контекстом, который судя по названию содержит информацию 
              о заметках и, возможно, пользователях. Затем он активирует обработчик с новой командой 
              удаления заметки с правильным ID заметки и пользователя. После выполнения команды, 
              он проверяет, что заметка больше не присутствует в контексте — это означает, что 
              операция удаления прошла успешно.

            * DeleteNoteCommandHandler_FailOnWrongId: Этот тест проверяет, что попытка удалить 
              заметку с некорректным ID заметки приводит к исключению NotFoundException. 
              Некорректный ID генерируется случайным образом с помощью Guid.NewGuid().

            * DeleteNoteCommandHandler_FailOnWrongUserId: Этот тест проверяет, что попытка 
              удалить заметку с некорректным ID пользователя также приводит к исключению 
              NotFoundException. Сначала создается новая заметка с правильным ID пользователя, 
              затем пытается удалить ее с некорректным ID пользователя.

        В целом, эти тесты проверяют, что DeleteNoteCommandHandler корректно обрабатыват команду 
        на удаление заметок, и корректно обрабатывает ситуации, когда переданы некорректные данные.
    */
    public class DeleteNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteNoteCommandHandler_Success()
        {
            // Arrange
            var handler = new DeleteNoteCommandHandler(Context);

            // Act
            await handler.Handle(new DeleteNoteCommand
            {
                Id = NotesContextFactory.NoteIdForDelete,
                UserId = NotesContextFactory.UserAId
            }, CancellationToken.None);

            // Assert
            Assert.Null(Context.Notes.SingleOrDefault(note =>
                note.Id == NotesContextFactory.NoteIdForDelete));
        }

        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongId()
        {
            // Arrange
            var handler = new DeleteNoteCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(
                    new DeleteNoteCommand
                    {
                        Id = Guid.NewGuid(),
                        UserId = NotesContextFactory.UserAId
                    },
                    CancellationToken.None));
        }

        [Fact]
        public async Task DeleteNoteCommandHandler_FailOnWrongUserId()
        {
            // Arrange
            var deleteHandler = new DeleteNoteCommandHandler(Context);
            var createHandler = new CreateNoteCommandHandler(Context);
            var noteId = await createHandler.Handle(
                new CreateNoteCommand
                {
                    Title = "NoteTitle",
                    UserId = NotesContextFactory.UserAId
                }, CancellationToken.None);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await deleteHandler.Handle(
                    new DeleteNoteCommand
                    {
                        Id = noteId,
                        UserId = NotesContextFactory.UserBId
                    }, CancellationToken.None));
        }
    }
}
