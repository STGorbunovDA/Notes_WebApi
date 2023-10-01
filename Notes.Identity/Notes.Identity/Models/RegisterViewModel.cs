using System.ComponentModel.DataAnnotations;

namespace Notes.Identity.Models
{
    /*
        * Объект класса RegisterViewModel в ASP.NET Core представляет собой модель представления, 
          предназначенную для регистрации пользователя. Значения свойств этого класса используются 
          для передачи данных от контроллера к представлению.

        * Здесь определены следующие свойства:

            * Username: Имя пользователя, которое должно быть предоставлено 
              (что гарантируется благодаря метке [Required]).

            * Password: Пароль пользователя. Это поле также должно быть заполнено, 
              и оно будет представлено в формате пароля, что указывается 
              при помощи [DataType(DataType.Password)].

            * ConfirmPassword: Подтверждение пароля. Это поле должно совпадать с полем Password, 
              что обеспечивается аннотацией [Compare("Password")]. Она означает, что значение 
              свойства "ConfirmPassword" должно быть равно значению свойства "Password". 
              Если значений не совпадают, то валидацию не пройдут.

            * ReturnUrl: URL для возврата после регистрации. Это не является обязательным полем.

        В общем и целом, эта модель представления используется для сбора необходимых данных 
        при регистрации нового пользователя.
    */
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
    }
}
