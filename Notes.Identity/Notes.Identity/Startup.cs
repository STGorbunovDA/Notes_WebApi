// https://localhost:7288/.well-known/openid-configuration

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Notes.Identity.Data;
using Notes.Identity.Models;

namespace Notes.Identity
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; }

        public Startup(IConfiguration configuration) =>
            AppConfiguration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            /*
                * Этот фрагмент кода использует ASP.NET Core и Entity Framework Core 
                  для настройки приложения на использование SQLite в качестве 
                  сервера базы данных.

                * Сначала он извлекает строку соединения с базой данных из конфигурации 
                  приложения AppConfiguration.GetValue<string>("DbConnection"). 
                  Эта строка соединения содержит информацию о том, где находится база данных 
                  и как к ней подключиться.

                * Затем, с помощью services.AddDbContext<AuthDbContext>(), он добавляет контекст 
                  базы данных AuthDbContext в контейнер служб. AuthDbContext является классом, 
                  который унаследован от DbContext и предоставляет интерфейс между вашим 
                  приложением и базой данных.

                * Внутри AddDbContext, вызывается options.UseSqlite(connectionString). 
                  Это указывает вашему приложению использовать SQLite в качестве провайдера 
                  базы данных и дает ему строку подключения, которую он должен использовать 
                  для установки этого соединения.

                В итоге, после выполнения этого кода, вашему приложению будет предоставлен 
                контекст базы данных AuthDbContext, который он может использовать 
                для взаимодействия с базой данных SQLite, указанной в строке 
                подключения.
            */
            var connectionString = AppConfiguration.GetValue<string>("DbConnection");

            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            /*
                * Следующий фрагмент кода настраивает службы идентификации (пользовательских 
                  учетных данных) для приложения ASP.NET Core.

                * services.AddIdentity<AppUser, IdentityRole>(config => {...}) регистрирует 
                  службы, необходимые для работы с системой идентификации. Здесь AppUser — 
                  это ваш пользовательский класс, расширяющий базовый класс IdentityUser. 
                  IdentityRole используется для поддержки ролей пользователей.

                * Внутри этого метода в аргументе лямбда-функции (config) вы можете определить 
                  политику пароля, которую должны соблюдать пользователи при создании 
                  или изменении своего пароля. В данном случае минимальная длина пароля 
                  установлена в 4 символа, а требования к наличию цифр, неалфавитных 
                  символов и символов в верхнем регистре отключены.

                * .AddEntityFrameworkStores<AuthDbContext>() сообщает ASP.NET Core Identity 
                  использовать AuthDbContext в качестве хранилища для данных идентификации. 
                  Это означает, что информация о пользователях и их ролях будет храниться 
                  в базе данных, которую управляет AuthDbContext.

                * .AddDefaultTokenProviders() добавляет стандартные "провайдеры токенов", 
                  которые используются для генерации токенов в целях подтверждения электронной 
                  почты, сброса пароля и т. д.

                В общем, этот код предоставляет способ аутентификации и авторизации пользователей 
                в вашем приложении с защитой пароля и хранением данных в базе данных через AuthDbContext.
            */
            services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            /*
                 * Этот код сначала регистрирует службу IdentityServer4 в контейнере зависимостей 
                   ASP.NET Core, а затем настраивает её.

                 * Вот что делает каждая строка:

                     * services.AddIdentityServer(): Инициализирует и добавляет IdentityServer 
                       в контекст внедрения зависимостей ASP.NET. Это регистрирует все необходимые 
                       сервисы для работы IdentityServer.

                     * .AddAspNetIdentity<AppUser>(): Интегрирует IdentityServer с ASP.NET Core Identity. 
                       Это позволяет IdentityServer использовать настройки и пользовательские данные, 
                       управляемые ASP.NET Core Identity. AppUser — это класс, который представляет 
                       пользователя в вашей системе.

                     * .AddInMemoryApiResources(new List<ApiResource>()): Добавляет в память ресурсы API, 
                       которые будут поддерживаться IdentityServer. 

                     * .AddInMemoryIdentityResources(new List<IdentityResource>()): Добавляет в память 
                       ресурсы Identity. Ресурс Identity представляет набор claim (утверждений) 
                       пользователей, возвращаемых при успешной аутентификации.

                     * .AddInMemoryApiScopes(new List<ApiScope>()): Добавляет в память области 
                       видимости API (ApiScopes). Область видимости API представляет разрешения, 
                       которые клиентские приложения могут запросить для доступа к API. 

                     * .AddInMemoryClients(new List<Client>()): Добавляет в память доверенных 
                       клиентов, которые могут получить токены от IdentityServer. Клиент в этом 
                       контексте это приложение, пытающееся получить доступ к определенному API. 

                     * .AddDeveloperSigningCredential(): Добавляет временный ключ, который 
                       IdentityServer будет использовать для подписания JWT (JSON Web Tokens). 
                       Обычно используется во время разработки и тестирования. Для продакшен-среды, 
                       рекомендуется использовать устойчивые и безопасные ключи, а не темпоральный 
                       ключ разработчика.

                 В общем, этот код определяет все вещи, которые IdentityServer4 требуется знать 
                 для обработки запросов на аутентификацию и выдачу токенов - какие клиенты, 
                 какие API, какие пользовательские claim он должен управлять, и как подписывать 
                 токены.
             */
            services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryApiResources(Configuration.ApiResources)
                .AddInMemoryIdentityResources(Configuration.IdentityResources)
                .AddInMemoryApiScopes(Configuration.ApiScopes)
                .AddInMemoryClients(Configuration.Clients)
                .AddDeveloperSigningCredential();

            /*
                * Этот сниппет кода используется для настройки куки аутентификации 
                  в приложении ASP.NET Core. В нем определяются следующие настройки:

                    * config.Cookie.Name = "Notes.Identity.Cookie";: Это имя куки, которое 
                      будет использоваться для хранения информации об идентификации пользователя. 
                      Клиенту будет отправлен этот файл cookie, и он будет отправлять его 
                      обратно с каждым запросом, чтобы сервер мог идентифицировать 
                      аутентифицированных пользователей.

                    * config.LoginPath = "/Auth/Login";: Это путь, на который перенаправят 
                      пользователя, если он пытается получить доступ к защищенной части 
                      приложения без предварительной аутентификации. В этом случае пользователь 
                      будет перенаправлен на страницу входа.

                    * config.LogoutPath = "/Auth/Logout";: Это указывает путь, который 
                      будет обрабатывать процесс выхода из системы.

                В общем, services.ConfigureApplicationCookie() используется для конфигурации 
                поведения и свойств куки аутентификации в вашем приложении, что важно для 
                обработки и поддержки статуса аутентификации пользователя при навигации 
                по вашему веб-приложению.
            */
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Notes.Identity.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
