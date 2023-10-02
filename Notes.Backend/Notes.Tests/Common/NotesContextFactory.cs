using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using Notes.Persistence;

namespace Notes.Tests.Common
{
    /*
        * В этом классе NotesContextFactory есть код для создания и удаления "виртуальной" базы 
          данных используя Entity Framework. Этот подход часто используется в юнит-тестах, 
          где вам нужно создать изолированное окружение базы данных для каждого индивидуального теста.

        * Есть 2 публичных статических метода, Create() и Destroy(). 
          Сначала кратко о каждом из них:

            * Create(): Этот метод создает новый экземпляр NotesDbContext, который является 
              контекстом базы данных в Entity Framework. Здесь используется подход 
              "InMemoryDatabase", который фактически создает базу данных прямо в памяти 
              (он не требует постоянного физического хранилища, например, SQL-сервера). 
              Затем мы добавляем 4 заметки (Note) в контекст базы данных с различными свойствами. 
              После этого данные сохраняются с помощью метода SaveChanges().

            * Destroy(): Этот метод удаляет виртуальную базу данных, предоставленную методом 
              Create(), а также освобождает использованный контекст базы данных (если он 
              реализует IDisposable, что DbContext реализует). Это освобождает все ресурсы, 
              связанные с контекстом базы данных.

            * 4 статических поля это уникальные идентификаторы (Guid), которые используются 
              для пользователя (UserAId и UserBId) и заметок (NoteIdForDelete и NoteIdForUpdate). 
              Последние два могут быть полезны для тестовых сценариев, в которых вы хотите обновить 
              или удалить конкретную заметку в исходных данных.

        В общем, NotesContextFactory используется для создания и удаления изолированных тестовых 
        сред, предоставляющих определенные начальные данные для тестирования.
    */
    public class NotesContextFactory
    {
        public static Guid UserAId = Guid.NewGuid();
        public static Guid UserBId = Guid.NewGuid();

        public static Guid NoteIdForDelete = Guid.NewGuid();
        public static Guid NoteIdForUpdate = Guid.NewGuid();

        public static NotesDbContext Create()
        {
            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new NotesDbContext(options);
            context.Database.EnsureCreated();
            context.Notes.AddRange(
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details1",
                    EditDate = null,
                    Id = Guid.Parse("DEE1787B-FFBA-4EF0-A196-6C2A351FCFBB"),
                    Title = "Title1",
                    UserId = UserAId
                },
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details2",
                    EditDate = null,
                    Id = Guid.Parse("11600F50-8DC1-4E32-B0A5-ADE9D44F5F0A"),
                    Title = "Title2",
                    UserId = UserBId
                },
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details3",
                    EditDate = null,
                    Id = NoteIdForDelete,
                    Title = "Title3",
                    UserId = UserAId
                },
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details4",
                    EditDate = null,
                    Id = NoteIdForUpdate,
                    Title = "Title4",
                    UserId = UserBId
                }
            );
            context.SaveChanges();
            return context;
        }

        public static void Destroy(NotesDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
