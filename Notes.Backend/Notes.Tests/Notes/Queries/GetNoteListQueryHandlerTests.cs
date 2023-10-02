using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Persistence;
using Notes.Tests.Common;
using Shouldly;
using Xunit;

namespace Notes.Tests.Notes.Queries
{
    /*
        * Этот код относится к тестами для обработчика запросов, которые выполняются 
          в сценариях (scenario) или юнит-тестах в области создания программного обеспечения. 
          Используется фреймворк автоматизации тестирования xUnit. Этот кусок кода проверяет 
          ядро приложения, обслуживающее функциональность работы с заметками.

        * NotesDbContext Context и IMapper Mapper - эти два приватные поля класса представляют 
          контекст базы данных и объект маппера, соответственно. Контекст базы данных используется 
          для взаимодействия с базой данных, а маппер служит для преобразования данных из одного 
          объекта в другой.

        * В конструкторе класса GetNoteListQueryHandlerTests, контекст базы данных и маппер 
          инициализируются с помощью переданных аргументов от объекта fixture, который 
          содержит подготовленные для тестирования объекты.

        * Метод GetNoteListQueryHandler_Success поведенческий тест, проверяющий, что 
          при вызове метода GetNoteListQueryHandler.Handle для определенного пользователя, 
          возвращается определенное количество заметок. Этот метод выполняется асинхронно 
          и содержит три фазы:

            * Arrange - здесь обработчик запросов инициализируется с предоставленным контекстом 
              базы данных и маппером.

            * Act - обработчик запросов вызывает метод с помощью UserId и CancellationToken. 
              Это эмулирует получение списка заметок для определенного пользователя.

            * Assert - в этой фазе проверяет, что результат действия соответствует ожидаемому. 
              Есть две проверки: сначала проверяется, что тип результата соответствует NoteListVm, 
              затем проверяется, что количество элементов во возвращенном списке равно двум.

        Таким образом, этот тест проверяет, что метод GetNoteListQueryHandler.Handle 
        корректно возвращает количество заметок для пользователя, и что возвращаемый 
        тип данных соответствует ожидаемому.
    */
    [Collection("QueryCollection")]
    public class GetNoteListQueryHandlerTests
    {
        private readonly NotesDbContext Context;
        private readonly IMapper Mapper;

        public GetNoteListQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetNoteListQueryHandler_Success()
        {
            // Arrange
            var handler = new GetNoteListQueryHandler(Context, Mapper);

            // Act
            var result = await handler.Handle(
                new GetNoteListQuery
                {
                    UserId = NotesContextFactory.UserBId
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<NoteListVm>();
            result.Notes.Count.ShouldBe(2);
        }
    }
}
