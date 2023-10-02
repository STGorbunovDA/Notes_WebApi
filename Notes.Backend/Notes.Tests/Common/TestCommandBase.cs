using Notes.Persistence;

namespace Notes.Tests.Common
{
    /*
        * Этот код представляет собой определение абстрактного класса TestCommandBase, 
          который реализует интерфейс IDisposable из пространства имен C#. IDisposable 
          используется для освобождения неуправляемых ресурсов, а оба объявленных метода 
          связаны с поддержкой этих операций.

        * Поля класса:

            * protected readonly NotesDbContext Context;: Поле "Context" будет доступно для 
              использования всем производным классам. Это является экземпляром NotesDbContext, 
              который будет использоваться только для чтения после инициализации.

        * Методы класса:

            * public TestCommandBase(): это конструктор по умолчанию, который вызывается при 
              создании нового объекта этого класса. Внутри этого конструктора, 
              NotesContextFactory.Create() вызывается для создания нового экземпляра NotesDbContext, 
              что установливает подключение к базе данных.

            * public void Dispose(): это метод, требуемый интерфейсом IDisposable. В нём реализована 
              логика для освобождения ресурсов или очистки данных, которые не будут автоматически 
              очищены сборщиком мусора C#. В данном случае, эта функция используется для уничтожения 
              контекста базы данных с помощью NotesContextFactory.Destroy(Context);, когда он больше 
              не нужен. Это может быть важно для предотвращения утечек памяти 
              или других проблем ресурсов.

        Здесь TestCommandBase служит базовым классом для всех классов, которые взаимодействуют 
        с NotesDbContext и на этом основании требуют реализации логики очистки после использования. 
        Это обеспечивает правильное управление ресурсами базы данных.
    */
    public abstract class TestCommandBase : IDisposable
    {
        protected readonly NotesDbContext Context;

        public TestCommandBase()
        {
            Context = NotesContextFactory.Create();
        }

        public void Dispose()
        {
            NotesContextFactory.Destroy(Context);
        }
    }
}
