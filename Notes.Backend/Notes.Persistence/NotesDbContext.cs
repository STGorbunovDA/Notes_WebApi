using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;
using Notes.Domain;
using Notes.Persistence.EntityTypeConfigurations;

namespace Notes.Persistence
{
    /*
        * NotesDbContext является классом, который наследуется от базового класса 
          DbContext и реализует интерфейс INotesDbContext. DbContext является 
          основным компонентом Entity Framework (EF) и представляет сессию работы 
          с базой данных с поддержкой операций CRUD (создание, чтение, обновление, удаление) 
          для сущностей.

        * Рассмотрим каждую часть кода:

            * public DbSet<Note> Notes { get; set; }: определяет свойство Notes, 
              которое представляет собой набор из сущностей Note. Это свойство 
              позволяет взаимодействовать с данными таблицы "Notes" в базе данных.

            * public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }: 
              конструктор класса принимает DbContextOptions<NotesDbContext> как параметр options. 
              Эти параметры используются для передачи настроек конфигурации для базы данных, 
              таких как строка подключения, параметры логирования и т.д., из внешней конфигурации. 
              Вызываем конструктор базового класса с этим параметром.

            * protected override void OnModelCreating(ModelBuilder builder): 
              переопределяем метод OnModelCreating, который вызывается, когда EF инициализирует 
              инстанс NotesDbContext. Здесь происходят первоначальные настройки маппинга сущностей.

            * builder.ApplyConfiguration(new NoteConfiguration());: 
              применяем конфигурацию из класса NoteConfiguration, который содержит 
              правила маппинга сущности Note к таблице базы данных.

            * base.OnModelCreating(builder);: вызываем базовый метод OnModelCreating, 
              чтобы продолжить настройки, определённые в классе DbContext.

        В результате, NotesDbContext является контекстом Entity Framework, 
        который позволяет взаимодействовать с базой данных, содержащей таблицу "Notes".
    */
    public class NotesDbContext : DbContext, INotesDbContext
    {
        public DbSet<Note> Notes { get; set; }

        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new NoteConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
