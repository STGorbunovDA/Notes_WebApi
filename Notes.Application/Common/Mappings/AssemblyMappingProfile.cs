using AutoMapper;
using System.Reflection;

namespace Notes.Application.Common.Mappings
{
    /*
      * Здесь используется библиотека AutoMapper для регистрации маппингов и профилей в приложении. 
        В этом блоке кода добавляются профили для распределенных маппингов из текущей сборки и из сборки, 
        которая содержит INotesDbContext.

      * Ключевые особенности кода:

        * services.AddAutoMapper() - интеграция AutoMapper с дополнением внедрения 
        * зависимостей (Dependency Injection) для .NET Core, которое регистрирует сервисы AutoMapper.

        * config => - лямбда-выражение, которое принимает конфигурационный объект 
          AutoMapper (тип IMapperConfigurationExpression). Здесь используется это выражение 
          для передачи в метод AddProfile экземпляров класса AssemblyMappingProfile.

          * Внутри config. вызывается метод AddProfile дважды с разными аргументами:

                * a. new AssemblyMappingProfile(Assembly.GetExecutingAssembly()) 
                  - создается экземпляр класса AssemblyMappingProfile с аргументом "текущая исполняемая сборка", 
                  полученная с помощью метода Assembly.GetExecutingAssembly(). 
                  Этот профиль сканирует все типы текущей исполняемой сборки, реализующие интерфейс IMapWith<>, 
                  и применяет соответствующие маппинги.

                * b. new AssemblyMappingProfile(typeof(INotesDbContext).Assembly) 
                  - создается экземпляр класса AssemblyMappingProfile с аргументом "сборка, 
                  содержащая интерфейс INotesDbContext". Получение сборки выполняется с помощью 
                  свойства Assembly, вызываемого для типа INotesDbContext. Этот профиль сканирует 
                  все типы этой конкретной сборки, реализующие интерфейс IMapWith<>, и применяет 
                  соответствующие маппинги.

        Итак, код добавляет профили маппинга с помощью созданных экземпляров AssemblyMappingProfile 
        на основе различных сборок. Это позволяет автоматически настраивать маппинг между связанными 
        объектами при использовании библиотеки AutoMapper.
    */
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile(Assembly assembly) =>
            ApplyMappingsFromAssembly(assembly);

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
