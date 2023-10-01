using Notes.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notes.Persistence.EntityTypeConfigurations
{
    /*
        * Данный код на языке программирования C# представляет собой класс конфигурации 
          для сущности Note. Конфигурация реализует интерфейс IEntityTypeConfiguration<Note> 
          из Entity Framework (EF), который используется для настройки сведений 
          о сущностях и связях между ними. EF — это популярная Object-Relational Mapping (ORM) 
          библиотека, которая позволяет вам работать с базами данных, используя объекты .NET.

        * Вот краткое описание каждой строки кода внутри метода Configure:

            * builder.HasKey(note => note.Id);: Устанавливает свойство Id объекта Note 
              в качестве первичного ключа таблицы. Первичный ключ используется 
              для уникальной идентификации каждой строки в таблице.

            * builder.HasIndex(note => note.Id).IsUnique();: Создает уникальный 
              индекс на основе свойства Id объекта Note. Уникальный индекс гарантирует, 
              что значения Id не будут повторяться, и помогает ускорить поиск по таблице.

            * builder.Property(note => note.Title).HasMaxLength(250);: Определяет свойство 
              Title объекта Note и устанавливает максимальную длину строки в 250 символов. 
              Это ограничение применяется для защиты данных и избежания проблем с производительностью.

        Класс NoteConfiguration будет использоваться при настройке Entity Framework 
        DbContext для определения правил конфигурации таблицы, основанной на объекте Note.
    */
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(note => note.Id);
            builder.HasIndex(note => note.Id).IsUnique();
            builder.Property(note => note.Title).HasMaxLength(250);
        }
    }
}
