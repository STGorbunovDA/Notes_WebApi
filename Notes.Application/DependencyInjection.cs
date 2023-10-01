using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Common.Behaviors;
using System.Reflection;

namespace Notes.Application
{
    /*
        * В этом классе DependencyInjection мы видим статический метод расширения AddApplication. 
          Этот метод помогает настроить и объединить несколько элементов конфигурации 
          в единую логику и позволяет добавить все соответствующие службы (сервисы) 
          в коллекцию IServiceCollection, которая служит контейнером зависимостей в ASP.NET Core. 
          Это делается для облегчения использования этих служб в других частях приложения.

        * Подробнее о каждой строке:

            * services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies
              (Assembly.GetExecutingAssembly())); - добавляет службы, связанные с библиотекой MediatR, 
              в коллекцию служб. MediatR используется для реализации шаблона "Медиатор", 
              выступая в качестве посредника между объектами в приложении. В данной строке 
              все службы MediatR, связанные с текущей сборкой, регистрируются в системе.

            * services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }); - 
              автоматически находит и регистрирует валидаторы из библиотеки FluentValidation, 
              которые есть в текущем сборке.

            * services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); - 
              регистрирует ValidationBehavior<,> в качестве временного (transient) сервиса для 
              каждого типа IPipelineBehavior<,>. Здесь используется шаблон "Поведение в пайплайне" 
              (Pipeline Behavior), который является промежуточным посредником (middleware) 
              в MediatR и позволяет выполнять логику перед и после обработки команды или запроса.

            * return services; - Метод заканчивается возвратом измененной коллекции служб, 
              чтобы вы могли организовать "сцепление" (chaining) методов при настройке.

        Таким образом, этот метод помогает организовать всю конфигурацию сервисов, 
        связанных с MediatR и валидацией FluentValidation, в одном месте и облегчает ее использование.
    */
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
