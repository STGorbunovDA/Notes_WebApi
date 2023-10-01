using Notes.Identity.Data;

namespace Notes.Identity
{
    public class Program
    {
        /*
            * ���� ������� ���� ������������ ����� �������� ������ ������ ���������� ASP.NET Core, 
              � ��������� �������������� ������� ������������� ���� ������ AuthDbContext, 
              ������ ��� ��������� ����������.

            * ������� ��������� ���-���� (CreateHostBuilder(args).Build()), ������� ����� ��������� 
              ��������� ������ ����������, � ����� ������������ �������, ����� ��� ��������� 
              ������������, �����������, DI (��������� ������������) � ������.

            * ����� ��������� ������� ����� ��� ����� (��������) � ������� 
              host.Services.CreateScope(). ��� ������� ����� ������������ ���������� 
              ��������� ������ ����������� �����. ��� �����, ��������� ��������� 
              ������ ����� ���� ���������������� ��� ������������� (Scoped), 
              ��� ��������, ��� ��� ����� ����������� �� ���������� ����� ������� 
              (��� ������� �����) � ������������ ����� ��� ����������.

            * �����, �� ����� scope, �� �������� serviceProvider, ������� ��������� 
              ��� �������� ������ � �������, ������� ���� ���������������� � ���������� 
              ��������� ������������.

            * �� �������� �������� ������ AuthDbContext, ������� 
              serviceProvider.GetRequiredService<AuthDbContext>(). 
              ����� GetRequiredService ���������� ��������� �������, ���� �� ����������, 
              ����� ���������� ����������.

            * ����� ���������� ����������� ����� DbInitializer.Initialize(context), 
              ������� �������������� ���� ������ (���� ��� ��� �� �������).

            * ���� � ���� �������� ��������� ������, ��� ���������������, � �� ���������� 
              � ������ ILogger<Program> ��� ������ ���������������� ��������� �� ������.

            � �������, ����� ������ �� ����� using, �� ��������� ����, ������� host.Run(). 
            ��� �������� ����� �������� HTTP-��������.
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
