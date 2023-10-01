using Microsoft.AspNetCore.Identity;

namespace Notes.Identity.Models
{
    /*
        * Здесь вы видите класс AppUser, который наследует от базового класса IdentityUser 
          в рамках механизма Identity в ASP.NET Core.

        * Identity -- это функциональность ASP.NET Core, которая помогает с аутентификацией 
          и управлением пользователями. IdentityUser представляет собой готовый класс, 
          включающий основные свойства, такие как Id, UserName, PasswordHash и другие.

        * В вашем примере, AppUser расширяет базовый класс IdentityUser, добавляя поля 
          FirstName и LastName. Это может быть полезно для хранения дополнительной 
          информации о пользователях, которая не включена в стандартный класс IdentityUser. 
          Таким образом, представленный класс AppUser можно использовать для управления 
          информацией о пользователях в вашем приложении.

        * Вот что делают строки кода:

        * public string FirstName { get; set; }: Это свойство для хранения имени пользователя.
        * public string LastName { get; set; }: Это свойство для хранения фамилии пользователя.
        
        * Оба свойства являются публичными, что позволяет их читать и изменять 
          из других частей приложения.
    */
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
