using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notes.Identity.Models;

namespace Notes.Identity.Data
{
    /*
        * В этом примере класс AuthDbContext наследуется от IdentityDbContext<AppUser>, 
          который является классом контекста базы данных Entity Framework Core, 
          специально предназначенным для использования с системой идентификации ASP.NET Core Identity.

        * IdentityDbContext<AppUser> предоставляет набор DbSet-ов для различных типов сущностей, 
          связанных с Identity, включая пользователей (AppUser в этом случае), роли и другие.

        * Конструктор AuthDbContext(DbContextOptions<AuthDbContext> options) используется 
          для передачи параметров конфигурации базы данных в базовый IdentityDbContext.

        * Метод OnModelCreating переопределен для настройки схемы базы данных. Здесь вам 
          необходимо вызвать base.OnModelCreating(builder) для применения стандартной 
          конфигурации Identity.

        * Затем вы конфигурируете таблицы базы данных, присваивая им специфические 
          имена (Users, Roles, UserRoles, UserClaim, UserLogins, UserTokens, RoleClaims) 
          согласно конвенции именования, которую вы предпочитаете, посредством вызовов 
          entity.ToTable("tableName").

        * В конце builder.ApplyConfiguration(new AppUserConfiguration()); применяет конфигурацию, 
          которую вы создали для AppUser в классе AppUserConfiguration.

        В общем, этот класс контекста базы данных специально сконфигурирован для работы 
        с ASP.NET Core Identity и обеспечивает управление сохранением и извлечением связанных 
        с идентичностью данных, таких как пользователи, роли, привязки ролей пользователей, 
        утверждения пользователей и т.д.
    */
    public class AuthDbContext : IdentityDbContext<AppUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity => entity.ToTable(name: "Users"));
            builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<string>>(entity =>
                entity.ToTable(name: "UserRoles"));
            builder.Entity<IdentityUserClaim<string>>(entity =>
                entity.ToTable(name: "UserClaim"));
            builder.Entity<IdentityUserLogin<string>>(entity =>
                entity.ToTable("UserLogins"));
            builder.Entity<IdentityUserToken<string>>(entity =>
                entity.ToTable("UserTokens"));
            builder.Entity<IdentityRoleClaim<string>>(entity =>
                entity.ToTable("RoleClaims"));

            builder.ApplyConfiguration(new AppUserConfiguration());
        }
    }
}
