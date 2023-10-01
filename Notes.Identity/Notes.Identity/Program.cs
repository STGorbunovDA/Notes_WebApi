using Notes.Identity.Data;

namespace Notes.Identity
{
    public class Program
    {
        /*
            * Этот участок кода представляет собой основной запуск вашего приложения ASP.NET Core, 
              с некоторой дополнительной логикой инициализации базы данных AuthDbContext, 
              прежде чем запустить приложение.

            * Сначала создается веб-хост (CreateHostBuilder(args).Build()), который будет управлять 
              жизненным циклом приложения, а также предоставить сервисы, такие как поддержка 
              конфигурации, логирование, DI (внедрение зависимостей) и другие.

            * Затем создается область жизни для служб (сервисов) с помощью 
              host.Services.CreateScope(). Эта область будет обеспечивать управление 
              жизненным циклом экземпляров служб. Это важно, поскольку некоторые 
              службы могут быть зарегистрированы как периодические (Scoped), 
              что означает, что они будут создаваться на протяжении всего запроса 
              (или области жизни) и уничтожаться после его завершения.

            * Далее, из этого scope, мы получаем serviceProvider, который позволяет 
              нам получить доступ к службам, которые были зарегистрированы в контейнере 
              внедрения зависимостей.

            * Мы пытаемся получить сервис AuthDbContext, вызывая 
              serviceProvider.GetRequiredService<AuthDbContext>(). 
              Здесь GetRequiredService возвращает экземпляр сервиса, если он существует, 
              иначе вызывается исключение.

            * Затем вызывается статический метод DbInitializer.Initialize(context), 
              который инициализирует базу данных (если она еще не создана).

            * Если в этом процессе произошла ошибка, она перехватывается, и мы обращаемся 
              к службе ILogger<Program> для записи соответствующего сообщения об ошибке.

            И наконец, после выхода из блока using, мы запускаем хост, вызывая host.Run(). 
            Это начинает прием входящих HTTP-запросов.
        */
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<AuthDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception exception)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An error occurred while app initialization");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
