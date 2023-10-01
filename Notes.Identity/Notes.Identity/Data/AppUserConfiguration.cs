using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Notes.Identity.Models;

namespace Notes.Identity.Data
{
    /*
        * В вашем коде представлен класс AppUserConfiguration, который реализует интерфейс 
          IEntityTypeConfiguration<AppUser> из Entity Framework Core. Этот интерфейс 
          используется для конфигурации модели AppUser в контексте базы данных Entity Framework.

        * Интерфейс IEntityTypeConfiguration<T> содержит один метод, Configure, который 
          принимает EntityTypeBuilder<T>. Вы используете для создания и настройки маппингов 
          и связей в базе данных для конкретного типа entity, в этом случае AppUser.

        * В методе Configure вы настраиваете первичный ключ (Id) для AppUser с помощью 
          строителя сущностей, EntityTypeBuilder<T>. В данном случае вы указываете, 
          что свойство Id является ключом для модели AppUser, поэтому каждый экземпляр 
          AppUser в базе данных должен иметь уникальное значение Id.

        * Код builder.HasKey(x => x.Id); говорит о том, что Id используется в качестве 
          первичного ключа в таблице базы данных для AppUser.

        В контексте Entity Framework при первичном ключе создается индекс, что обеспечивает 
        более быстрый поиск по этому полю. Кроме того, значения первичного ключа должны 
        быть уникальными и ненулевыми.
    */
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
