using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Notes.Application
{
    /*
     * Этот код представляет собой статический класс DependencyInjection, который содержит метод 
       расширения AddApplication для IServiceCollection. Метод расширения используется для регистрации 
       MediatR и связанных с ним сервисов в приложении.

     * Важные аспекты кода:

        * public static IServiceCollection AddApplication(this IServiceCollection services) 
            - это статический метод расширения AddApplication для IServiceCollection. 
            Метод расширения полезен, когда вы хотите добавить новый метод для существующего 
            типа без наследования или модификации исходного кода.

        * services.AddMediatR() - вызывается для регистрации сервисов MediatR в проекте. 
          MediatR - это библиотека, которая предоставляет реализацию медиатора для обработки запросов, 
          отправляемых через медиатор. Внедрение зависимостей и регистрация экземпляра MediatR 
          позволяют применять шаблон "медиатор" в проекте.

        * cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()) - 
          лямбда-выражение (обычно передается Action<IServiceConfiguration>), обрабатывающее передачу 
          аргумента конфигурации в метод AddMediatR. Метод RegisterServicesFromAssemblies 
          вызывается для текущей исполняемой сборки с помощью Assembly.GetExecutingAssembly(). 
          Этот метод подключает сервисы из ассоциированных сборок, что обеспечивает изоляцию 
          различных слоев приложения.

        * return services; - метод возвращает экземпляр IServiceCollection 
          после выполнения своей работы. Это предоставляет возможность для дальнейшей регистрации 
          и настройки сервисов приложения.

        В итоге, статический класс DependencyInjection с методом расширения AddApplication 
        предназначен для простой регистрации MediatR и его сервисов в контексте разрешения 
        зависимостей .NET. Это способствует чистоте и структурированности кода, делая приложение 
        более модульным и удобным для поддержки.
    */
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
