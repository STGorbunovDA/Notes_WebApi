using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using System.Reflection;
using Notes.Persistence;
using Notes.Application;

namespace Notes.WebApi
{
    public class Startup
    { 
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * Здесь используется библиотека AutoMapper для регистрации маппингов и профилей в приложении. 
               В этом блоке кода добавляются профили для распределенных маппингов из текущей сборки 
               и из сборки, которая содержит INotesDbContext.

             * Ключевые особенности кода:

                * services.AddAutoMapper() - интеграция AutoMapper с дополнением внедрения зависимостей 
                  (Dependency Injection) для .NET Core, которое регистрирует сервисы AutoMapper.

                * config => - лямбда-выражение, которое принимает конфигурационный объект AutoMapper 
                  (тип IMapperConfigurationExpression). Здесь используется это выражение для передачи 
                  в метод AddProfile экземпляров класса AssemblyMappingProfile.

                * Внутри config. вызывается метод AddProfile дважды с разными аргументами:

                    * a. new AssemblyMappingProfile(Assembly.GetExecutingAssembly()) - создается 
                      экземпляр класса AssemblyMappingProfile с аргументом "текущая исполняемая сборка", 
                      полученная с помощью метода Assembly.GetExecutingAssembly(). 
                      Этот профиль сканирует все типы текущей исполняемой сборки, 
                      реализующие интерфейс IMapWith<>, и применяет соответствующие маппинги.

                    * b. new AssemblyMappingProfile(typeof(INotesDbContext).Assembly) - создается 
                      экземпляр класса AssemblyMappingProfile с аргументом "сборка, содержащая интерфейс 
                      INotesDbContext". Получение сборки выполняется с помощью свойства Assembly, 
                      вызываемого для типа INotesDbContext. Этот профиль сканирует все типы этой 
                      конкретной сборки, реализующие интерфейс IMapWith<>, и применяет соответствующие маппинги.

                Итак, код добавляет профили маппинга с помощью созданных экземпляров AssemblyMappingProfile 
                на основе различных сборок. Это позволяет автоматически настраивать маппинг 
                между связанными объектами при использовании библиотеки AutoMapper.
            */
            services.AddAutoMapper(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
                config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
            });

            services.AddApplication();
            services.AddPersistence(Configuration);
            services.AddControllers();


            /*
             * Метод AddCors() предназначен для регистрации и настройки сервиса 
               CORS (Cross-Origin Resource Sharing) в ASP.NET Core. CORS - это механизм 
               безопасности, который позволяет веб-приложениям указывать, какие источники 
               должны иметь разрешение на доступ к ресурсам.

             * В данном примере:

                * services.AddCors(options => { ... }); – вызывается метод расширения 
                * AddCors для IServiceCollection, который принимает лямбда-выражение, 
                * описывающее настройки CORS для сервиса.

                * options.AddPolicy("AllowAll", policy => { ... }); – добавляется новая 
                  политика CORS с именем "AllowAll". Это лямбда-выражение определяет настройки 
                  политики CORS для указанного имени.

            * Внутри лямбда-выражения политики используются следующие методы:

                * policy.AllowAnyHeader(); – разрешает запросы с абсолютно любыми HTTP-заголовками.

                * policy.AllowAnyMethod(); – разрешает запросы с любыми HTTP-методами (GET, POST, PUT, DELETE и т. д.).

                * policy.AllowAnyOrigin(); – разрешает запросы из любого источника (домена).

            В результате, указанная политика CORS "AllowAll" разрешает кросс-доменные запросы 
            от любых источников с любыми методами и заголовками.

            Важно отметить, что такая политика считается небезопасной и может использоваться 
            только для разработки или тестирования. Для продакшн-среды рекомендуется создавать 
            более строгие политики CORS, которые разрешают только определенные методы,
            заголовки и источники вместо использования AllowAny* методов.
            */
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapControllers();
            });
        }
    }
}
