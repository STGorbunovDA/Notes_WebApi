using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Notes.WebApi
{
    /*
        * Данный код на языке C# является настройкой API документации для проекта на ASP.NET Core 
          с использованием библиотеки Swagger. Использование Swagger позволяет автоматически 
          создавать документацию для API, которую можно удобно просматривать в веб-интерфейсе.

        * Передача экземпляра IApiVersionDescriptionProvider в конструктор запрашивает сведения 
          о версиях API из системы.

        * Метод Configure(SwaggerGenOptions options) настраивает опции Swagger:

            * Он проходится по всем версиям API, полученным от _provider.ApiVersionDescriptions, 
              и для каждой версии задаёт настройки документации через options.SwaggerDoc. 
              Здесь устанавливаются основные сведения: версия API, заголовок, описание, ссылки 
              на условия использования, контакты и лицензию.

            * Для каждой версии API добавляется секция безопасности с помощью 
              options.AddSecurityDefinition, где устанавливаются настройки для токена 
              авторизации ("Bearer Token").

            * С помощью options.AddSecurityRequirement, явным образом требуется использование 
              только что определённой схемы безопасности.

            * options.CustomOperationIds определяет способ генерации идентификаторов операций 
              в Swagger. Если информация о методе доступна, то в качестве идентификатора 
              операции используется имя метода.

        В целом, этот класс служит для конфигурации Swagger в соответствии с требованиями 
        к документированию API и безопасности.
    */
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
            _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var apiVersion = description.ApiVersion.ToString();
                options.SwaggerDoc(description.GroupName,
                    new OpenApiInfo
                    {
                        Version = apiVersion,
                        Title = $"Notes API {apiVersion}",
                        Description =
                            "Мой первый WEB API",
                        TermsOfService =
                            new Uri("https://github.com/STGorbunovDA/Notes_WebApi"),
                        License = new OpenApiLicense
                        {
                            Name = "Telegram",
                            Url =
                                new Uri("https://t.me/DA_Gorbunov")
                        }
                    });

                options.AddSecurityDefinition($"AuthToken {apiVersion}",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Name = "Authorization",
                        Description = "Authorization token"
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = $"AuthToken {apiVersion}"
                            }
                        },
                        new string[] { }
                    }
                });

                options.CustomOperationIds(apiDescription =>
                    apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
                        ? methodInfo.Name
                        : null);
            }
        }
    }
}
