using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Interfaces;

namespace Notes.Persistence
{
    /*
     * Этот код представляет собой статический класс DependencyInjection, который содержит метод 
       расширения AddPersistence для IServiceCollection. Задача данного метода заключается 
       в настройке и регистрации сервисов для работы с постоянным хранилищем данных 
       (в данном случае - база данных SQLite) в контексте разрешения зависимостей .NET.

       * Вот что происходит в этом коде:

            * public static IServiceCollection AddPersistence(this IServiceCollection services, 
              IConfiguration configuration) – это статический метод расширения AddPersistence 
              для IServiceCollection. В качестве аргумента он принимает объект 
              IConfiguration для работы с конфигурацией приложения.

            * var connectionString = configuration["DbConnection"]; - строка подключения к базе данных 
              извлекается из конфигурации приложения с помощью ключа "DbConnection".

            * services.AddDbContext<NotesDbContext>(options => { options.UseSqlite(connectionString); }); 
              – регистрируется контекст базы данных NotesDbContext, который будет использоваться 
              для работы с базой данных SQLite. options.UseSqlite(connectionString) настраивает 
              контекст для использования базы данных SQLite с полученной строкой подключения.

            * services.AddScoped<INotesDbContext>(provider => provider.GetService<NotesDbContext>()); 
              - регистрируется и настраивается сопоставление интерфейса INotesDbContext 
              с созданием экземпляров класса NotesDbContext. Здесь используется паттерн "scoped", 
              который обеспечивает создание одного экземпляра на протяжении жизни одного скоупа, 
              то есть в рамках одного HTTP-запроса.

            * return services; - метод возвращает экземпляр IServiceCollection после 
              выполнения регистрации и настройки сервисов. Это предоставляет возможность 
              для дальнейшей регистрации и настройки сервисов приложения.

        В итоге, статический класс DependencyInjection с методом расширения AddPersistence 
        предназначен для регистрации и настройки сервисов для работы с постоянным хранилищем данных 
        (в данном случае - база данных SQLite) и их регистрации в контексте разрешения зависимостей .NET. 
        Это помогает структурировать код, делая его более поддерживаемым и модульным.

    */
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection
            services, IConfiguration configuration)
        {
            var connectionString = configuration["DbConnection"];
            services.AddDbContext<NotesDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
            services.AddScoped<INotesDbContext>(provider =>
                provider.GetService<NotesDbContext>());
            return services;
        }
    }
}
