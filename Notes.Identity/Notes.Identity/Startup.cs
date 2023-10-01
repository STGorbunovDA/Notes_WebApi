// https://localhost:7288/.well-known/openid-configuration

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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

            /*
                * В ASP.NET Core вызов services.AddControllersWithViews(); 
                  в методе ConfigureServices добавляет сервисы, необходимые 
                  для использования контроллеров и представлений в приложении MVC.

                * Под "сервисами" здесь подразумеваются ряд встроенных функций и ресурсов, 
                  которые ASP.NET Core предлагает для облегчения создания веб-приложений. 
                  "Контроллеры" в MVC (Model-View-Controller) отвечают за обработку входящих 
                  HTTP-запросов, определение действий в ответ на эти запросы и обработку логики 
                  приложения. "Представления" отвечают за генерацию HTML-ответа, который 
                  будет отправлен обратно клиенту.

                * AddControllersWithViews добавляет поддержку для контроллеров, а также 
                  представлений, что позволяет вам создать динамические веб-страницы 
                  с использованием кода C# и Razor Syntax.

                Если вы планируете создать API, которое сгенерирует единственно 
                JSON или какие-либо другие форматы данных, то вы можете вместо этого 
                использовать services.AddControllers() поскольку для таких случаев 
                поддержка представлений не требуется. Более того, при создании приложения 
                Razor Pages (модель визуализации представлений альтернативная MVC в ASP.NET Core) 
                вы бы вызвали services.AddRazorPages().
            */
            services.AddControllersWithViews();
        }
        /*
            * public void Configure(IApplicationBuilder app, IWebHostEnvironment env): Это метод, 
              который настраивает HTTP-запрос пайплайна вашего приложения. Это то место, 
              где вы настраиваете как middleware будет обрабатываться. Здесь IApplicationBuilder 
              используется для настройки приложения, а IWebHostEnvironment используется 
              для взаимодействия с окружением, в котором работает ваше приложение.

              * if (env.IsDevelopment()): Это условие проверяет, находится ли приложение 
                в режиме разработки.

              * app.UseDeveloperExceptionPage();: Если приложение в режиме разработки, 
                это middleware будет обрабатывать исключения и возвращать страницу ошибки 
                для разработчиков.

              * app.UseRouting();: Это middleware используется для настройки маршрутизации в приложении.

              * app.UseIdentityServer();: Это middleware, которое добавляет IdentityServer 
                в пайплайн обработки запросов. IdentityServer обрабатывает аутентификацию и авторизацию.

              * app.UseEndpoints(...): Это middleware определит, как приложение будет реагировать 
                на индивидуальные HTTP-запросы. Внутри этого делегата определен один endpoint:

                    * endpoints.MapGet("/", async context => ...): это route, который будет 
                      реагировать на HTTP GET запросы к корневому URL ("/"). Когда кто-то 
                      посещает корневой URL, данный код будет работать, и в ответ будет 
                      отправлять сообщение "Hello World!" на страницу браузера.

              Все делегаты app.Use... используются для конфигурации pipeline обработки HTTP-запросов. 
              Приложение будет использовать эти middleware в том порядке, в котором они указаны. 
              Каждый из них может иметь возможность обрабатывать запрос и/или передавать его 
              дальше в pipeline.
        */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /*
                * Код, который вы привели, является частью настройки ASP.NET Core приложения 
                  для обслуживания статических файлов.

                * Давайте разберем, что каждая строка делает:

                    * app.UseStaticFiles(...): Это блок настроек, соединяющий очередь 
                      обработки запросов в ASP.NET Core с обработчиком статических файлов. 
                      Он говорит приложению "если запрос приходит для файла, а не для действия 
                      MVC или другого endpoint, попытайся найти этот файл и послать его ответом". 
                      Статические файлы обычно включают CSS, JavaScript, изображения и другие файлы, 
                      которые не генерируются динамически.

                    * new StaticFileOptions { ... }: Это объект настроек, который конфигурирует 
                      serveware для обслуживания статических файлов. Вы можете настроить различные 
                      опции для определения, как и откуда обслуживать статические файлы.

                    * FileProvider = new PhysicalFileProvider(...): Это опция FileProvider, 
                      которая определяет, где искать статические файлы. PhysicalFileProvider 
                      говорит приложению искать файлы на физическом диске. 
                      Path.Combine(env.ContentRootPath, "Styles") конкатенирует путь к корневому 
                      каталогу контента приложения с папкой "Styles".

                    * RequestPath = "/styles": Это опция, которая задает виртуальный путь 
                      для запросов к статическим файлам. Если запрос приходит для файла 
                      вида "/styles/app.css", middleware будет искать файл "app.css" 
                      в папке "Styles" корневой директории приложения.

                Итак, вы настроили ваше приложение на обслуживание статических файлов 
                из подкаталога "Styles" корневой директории приложения.
            */
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Styles")),
                RequestPath = "/styles"
            });

            app.UseRouting();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                /*
                    * Данный код конфигурирует маршруты для ваших контроллеров в ASP.NET Core 
                      приложении. Вызов endpoints.MapDefaultControllerRoute() устанавливает 
                      дефолтную схему маршрутизации для MVC контроллеров.

                    * Схема "дефолтного" маршрута к контроллеру определяется следующим образом: 
                      "{controller=Home}/{action=Index}/{id?}". Итак, этот шаблон маршрута 
                      определяется следующим образом:

                        * {controller=Home}: Это определяет контроллер, который будет использован. 
                          Если контроллер не указан в URL, то по умолчанию используется "Home".

                        * {action=Index}: Это определяет действие (или метод), которое будет вызвано 
                          на контроллере. Если действие не указано в URL, то по умолчанию 
                          используется действие "Index".

                        * {id?}: Это параметр, который может быть передан действию. Вопросительный 
                          знак "?" означает, что параметр необязательный.

                    Таким образом, если вы введете URL, например - "http://localhost:5000", 
                    то маршрутизация по умолчанию перенаправит вас к действию "Index" 
                    в контроллере "Home", так как определенное в URL действие и контроллер 
                    отсутствуют. Если введите URL "http://localhost:5000/Products/Edit/5", 
                    то вы будете перенаправлены к действию "Edit" в контроллере "Products", 
                    где "5" станет значением параметра "id".
                */
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
