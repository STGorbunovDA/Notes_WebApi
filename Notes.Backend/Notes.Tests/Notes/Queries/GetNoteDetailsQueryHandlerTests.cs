using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Persistence;
using Notes.Tests.Common;
using Shouldly;
using Xunit;

namespace Notes.Tests.Notes.Queries
{
    /*
        * Этот код представляет собой тест на языке программирования C#, используя тестовую 
          единицу XUnit. В частности, он тестирует взаимодействие с системой заметок:

            * Определение класса: GetNoteDetailsQueryHandlerTests — это класс, который содержит 
              тесты для тестирования GetNoteDetailsQueryHandler. Этот обработчик запросов
              отвечает за получение деталей конкретной заметки.

            * В этом классе определены два поля: Context и Mapper. Context представляет 
              контекст данных Entity Framework, который используется для взаимодействия 
              с базой данных. Mapper является экземпляром интерфейса IMapper из библиотеки 
              Automapper, используемой для маппинга и преобразования одних типов данных в другие.

            * В конструкторе этого класса (public GetNoteDetailsQueryHandlerTests
              (QueryTestFixture fixture)) контекст данных и объект маппера инициализируются 
              с помощью инъекций зависимостей через аргумент QueryTestFixture.

            * Метод GetNoteDetailsQueryHandler_Success — это фактический тест. Тестирование 
              происходит в трех шагах: конфигурация (Arrange), выполнение (Act) и проверка (Assert).

            * В фазе Arrange, создается обработчик запросов с некоторым предопределенным 
              контекстом и объектом маппера.

            * В фазе Act, обработчик вызывается с определенными аргументами — ID пользователя 
              и ID заметки. Действие выполняется асинхронно, поэтому используется оператор await.

            * На этапе Assert тест проверяет, что результат соответствует ожидаемому. 
              Здесь проводится серия проверок - что результат является типа NoteDetailsVm, 
              что поле Title равно "Title2" и что CreationDate равно текущей дате.

        Заметьте, в этом сценарии тестирования использован синтаксис Fluent Assertions. 
        Вместо классических Assert.Equal(expected, actual), здесь встречаются 
        result.Title.ShouldBe("Title2"). Это делает код более читабым и понятным.
    */
    [Collection("QueryCollection")]
    public class GetNoteDetailsQueryHandlerTests
    {
        private readonly NotesDbContext Context;
        private readonly IMapper Mapper;

        public GetNoteDetailsQueryHandlerTests(QueryTestFixture fixture)
        {
            Context = fixture.Context;
            Mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetNoteDetailsQueryHandler_Success()
        {
            // Arrange
            var handler = new GetNoteDetailsQueryHandler(Context, Mapper);

            // Act
            var result = await handler.Handle(
                new GetNoteDetailsQuery
                {
                    UserId = NotesContextFactory.UserBId,
                    Id = Guid.Parse("11600F50-8DC1-4E32-B0A5-ADE9D44F5F0A")
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<NoteDetailsVm>();
            result.Title.ShouldBe("Title2");
            result.CreationDate.ShouldBe(DateTime.Today);
        }
    }
}
