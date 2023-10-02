using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using Xunit;

namespace Notes.Tests.Common
{
    /*
        * Этот код представляет собой набор условий для тестирования в системе xUnit для .NET. 
          Это реализация архитектурных паттернов "Test Fixture" и "Collection Fixture", 
          используемых для инициализации и очистки ресурсов, необходимых для тестирования, 
          а также для обеспечения их повторного использования в серии тестов.

        * Подробнее о классах:

            * QueryTestFixture - Этот класс, наследованный от IDisposable, создан для 
              инициализации и очистки тестовой среды. Метод Dispose используется для очистки 
              ресурсов после выполнения тестов:

                * Context - Это образец вашего контекста базы данных (Entity Framework), 
                  который инициализируется, когда создается экземпляр QueryTestFixture.

                * Mapper - Это экземпляр объекта Mapper, который создается при инициализации 
                  QueryTestFixture. Конфигурация маппера включает типы, найденные в сборке, 
                  где определён INotesDbContext.

                * Dispose - В конце каждого теста класс QueryTestFixture вызывает метод Dispose. 
                  Это происходит автоматически благодаря гарантиям, предоставляемым интерфейсом 
                  IDisposable. В этом конкретном случае он уничтожает контекст базы данных 
                  с помощью NotesContextFactory.Destroy(Context).

            * QueryCollection - Этот класс определяет коллекцию тестов, которые используют общий 
              класс QueryTestFixture. Использование ICollectionFixture<T> позволяет иметь общий 
              код инициализации и очистки (QueryTestFixture в данном случае) на протяжении всех 
              тестов в определённой коллекции тестов (QueryCollection). Указывая атрибут 
              [CollectionDefinition("QueryCollection")], вы определяете, что этот класс является 
              коллекцией тестов.

            * Эти тестовые структуры особенно полезны, когда у вас есть тесты, которые могут 
              повторно использовать дорогостоящие ресурсы, такие как контексты базы данных, 
              веб-серверы и т.д. Вместо создания этих ресурсов для каждого теста, вы создаете 
              их один раз для каждого набора тестов, уменьшая тем самым накладные расходы 
              и общее время выполнения тестов.
    */
    public class QueryTestFixture : IDisposable
    {
        public NotesDbContext Context;
        public IMapper Mapper;

        /*
            * Данный код создает тестовую среду в C#. Разберем его по частям:

                * Context = NotesContextFactory.Create(); - эта строчка создает новый контекст 
                  данных для использования в тесте. В этом контексте, NotesContextFactory.Create 
                  обычно создаёт новый экземпляр DbContext, который является основным классом, 
                  где определены все операции CRUD (Create, Read, Update, Delete) для базы данных. 
                  Фабрика в данном случае позволяет избежать прямого создания объектов DbContext, \
                  что упрощает управление его жизненным циклом и является хорошей практикой, 
                  особенно при написании тестов.

                * После этого объявляется конфигурация маппера. AutoMapper, используемый здесь, 
                  - это библиотека .NET, которая автоматически сопоставляет данные одного 
                  объекта с другим. Объявляется новая конфигурация маппера.

                * cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly)); 
                  - В AutoMapper profiles используются для определения сопоставлений. 
                  Профили представляют собой группы сопоставлений, которые можно настроить вместе. 
                  В данном случае создается новый экземпляр профиля с именем AssemblyMappingProfile, 
                  которому передается путь до сборки через отражение от интерфейса INotesDbContext.

                * Mapper = configurationProvider.CreateMapper(); - После объявления и конфигурации 
                  создается сам маппер (Mapper), который затем может быть использован для перевода 
                  одних объектов в другие.

            Все это делается в конструкторе класса QueryTestFixture, поэтому эта конфигурация 
            будет выполняться при каждом создании нового экземпляра этого класса. Это полезно 
            для тестирования, где каждый тест должен иметь свою собственную изолированную среду 
            для выполнения.
        */
        public QueryTestFixture()
        {
            Context = NotesContextFactory.Create();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(
                    typeof(INotesDbContext).Assembly));
            });
            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            NotesContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
