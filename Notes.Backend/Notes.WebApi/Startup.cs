using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using System.Reflection;
using Notes.Persistence;
using Notes.Application;
using Notes.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Notes.WebApi
{
    //https://localhost:7174/swagger/v1/swagger.json
    //
    public class Startup
    { 
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

            /*
                * Данный код настраивает аутентификацию через JSON Web Tokens (JWT) в приложении 
                  ASP.NET Core.

                * Вот что делает каждая строка:

                    * services.AddAuthentication(...): Этот метод добавляет сервисы для аутентификации 
                      в приложение. Аутентификация — это процесс идентификации пользователя. 
                      Это тот механизм, который уточняет, кто именно пытается получить доступ 
                      к приложению или определенным его частям.

                    * config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;: 
                      Устанавливает схему аутентификации по умолчанию как Bearer (как и определено 
                      в стандарте JWT). Схема аутентификации указывает, какой механизм аутентификации 
                      нужно использовать, когда пользователь пытается получить доступ 
                      к защищенным ресурсам.

                    * config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;: 
                      Устанавливает схему вызова по умолчанию как Bearer. Схема вызова определяет, 
                      как приложение ответит, когда неаутентифицированный пользователь пытается 
                      получить доступ к ресурсу, требующему аутентификацию.

                    * .AddJwtBearer("Bearer", options => ...): Здесь конфигурируется использование 
                      токенов Bearer для аутентификации. Затем идут опции, в которых настраивается 
                      обработчик JWT.

                    * options.Authority = "https://localhost:44384/";: Здесь задается URL, который 
                      используется для получения открытого ключа, который можно использовать 
                      для валидации подписи токена.

                    * options.Audience = "NotesWebAPI";: Аудитория, для которой предназначены токены. 
                      Это обычно идентификатор вашего приложения.

                    * options.RequireHttpsMetadata = false;: Опция HTTPS Metadata указывает, должен 
                      ли обработчик JWT требовать HTTPS для обращения к endpoint сервера для получения 
                      информации о ключах. Если эта опция установлена в false, то метаданные могут 
                      быть получены и через HTTP.

                Все эти настройки важны для безопасности вашего веб-приложения: они обеспечивают 
                его способность корректно и безопасно управлять аутентификацией пользователей.
            */
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer("Bearer", options =>
               {
                   // options.Authority = "https://localhost:5264/";
                   // options.Authority = "https://localhost:7288/"
                   // options.Authority = "https://localhost:44384/";
                   // options.Authority = "https://localhost:45756/";

                   options.Authority = "https://localhost:44384/";
                   options.Audience = "NotesWebAPI";
                   options.RequireHttpsMetadata = false;
               });

            /*
                * Этот код является конфигурацией для ASP.NET Core с использованием библиотеки 
                  Swagger, для управления версиями API и для генерации интерактивной документации.

                * Давайте рассмотрим каждую строку:

                    * services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
                      Это метод, который добавляет сервис ApiExplorer с поддержкой версионности. 
                      Это позволяет классам контроллеров в приложении быть организованными 
                      не только по именам контроллеров и их действий, но и по версиям API. 
                      'v'VVV' определяет формат имени группы. 'VVV' означает версию в формате 
                      Major.Minor.Patch.

                    * services.AddTransient<IConfigureOptions<SwaggerGenOptions>,ConfigureSwaggerOptions>();
                      Метод AddTransient добавляет сервис в коллекцию служб с паттерном 
                      жизненного цикла transient. Это означает, что каждый раз, когда сервис 
                      требуется, создается новый экземпляр. Здесь сервис конфигурации для 
                      SwaggerGen добавлен так, чтобы когда это потребуется, будет создан 
                      экземпляр класса ConfigureSwaggerOptions.

                    * services.AddSwaggerGen(); Swagger- это инструмент, который генерирует 
                      пользовательский интерфейс для ваших API. Этот метод службы добавляет 
                      Swagger generator к сервисам DI (Dependency Injection) вашего приложения.

                    * services.AddApiVersioning(); Этот метод добавляет поддержку версионности 
                      API к вашему приложению. Это значит, что вы можете иметь несколько версий 
                      каждого вашего API, и клиенты могут выбирать, какую версию они хотят использовать.

                В общем, этот блок кода подготавливает приложение к управлению версиями API 
                и генерации документации с помощью Swagger.
            */
            services.AddVersionedApiExplorer(options =>
                options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
                    ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            services.AddApiVersioning();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
           IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            /*
                * Этот код настраивает интерфейс пользователя Swagger для .NET Core приложения.

                * Итак, давайте разберем, что здесь происходит:

                    * app.UseSwaggerUI(config => {...});: Настройка и активация интерфейса 
                      пользователя Swagger. Swagger UI - это сгенерированный веб-интерфейс, 
                      который позволяет визуализировать и взаимодействовать с документацией 
                      API в браузере.

                    * foreach (var description in provider.ApiVersionDescriptions):
                      Этот цикл проходит через все версии API, предоставляемые внедренным 
                      провайдером версий (provider.ApiVersionDescriptions). ApiVersionDescription - 
                      это класс, который предоставляет описание версии API.

                    * $"/swagger/{description.GroupName}/swagger.json": описывает путь к файлу 
                      JSON Swagger для определенной версии API. GroupName - это свойство, которое 
                      указывает на идентификатор версии.

                    * description.GroupName.ToUpperInvariant(): Это название, которое будет 
                      отображаться в выборе версий в интерфейсе пользователя Swagger. Имя группы 
                      (то есть версия API) преобразуется в верхний регистр.

                    * config.RoutePrefix = string.Empty;: Это устанавливает префикс маршрута, 
                      по которому будет доступен Swagger UI. В данном случае он установлен 
                      в пустую строку, что означает, что Swagger UI будет доступен непосредственно 
                      по корневому URL-адресу приложения (например, http://localhost:5000/).

                После выполнения этого кода, вы сможете открыть ваш браузер и перейти к URL 
                вашего приложения, чтобы увидеть красиво оформленную страницу с вашими API 
                и их документацией.
            */
            app.UseSwaggerUI(config =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    config.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                    config.RoutePrefix = string.Empty;
                }
            });
            app.UseCustomExceptionHandler();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseApiVersioning();
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
