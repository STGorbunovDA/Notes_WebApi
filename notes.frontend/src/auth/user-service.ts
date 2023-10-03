import { UserManager, UserManagerSettings } from 'oidc-client';
import { setAuthHeader } from './auth-headers';

/*
    * Этот код используется для аутентификации пользователя в веб-приложении с использованием OpenID Connect (OIDC).

    * Вот более подробный обзор того, что происходит в каждой части кода:

        * Импорт необходимых библиотек и модулей. UserManager и UserManagerSettings из oidc-client используются 
          для управления процессом аутентификации, в то время как setAuthHeader из ./auth-headers устанавливает 
          заголовки авторизации для запросов, отправляемых на сервер.

        * Определение настроек OIDC-клиента. Эти настройки указывают на детали OIDC, такие как идентификатор клиента, 
          URL-адрес перенаправления (куда пользователь будет перенаправлен после аутентификации), тип ответа, 
          требования к области действия, сервер-авторитет и URL-адрес перенаправления после выхода из системы.

        * Создание экземпляра UserManager с указанными настройками.

        * Декларирование функции loadUser(), которая загружает информацию о текущем пользователе, если он аутентифицирован, 
          и устанавливает токен авторизации.

        * Декларирование функций signinRedirect(), signinRedirectCallback(), signoutRedirect() и signoutRedirectCallback(). 
          Эти функции работают с UserManager, чтобы способствовать процессу входа/выхода в систему и обработке перенаправлений OIDC.

        * Экспорт UserManager для использования в других частях приложения.

    В данном коде пользователь будет перенаправлен на сервер аутентификации (определенный в authority) 
    при вызове signinRedirect(), где он сможет войти в свою учетную запись. После успешной аутентификации 
    пользователь будет перенаправлен обратно на redirect_uri, а функцию signinRedirectCallback() можно вызвать 
    для обработки полученного ответа от сервера аутентификации. Функции signoutRedirect() и signoutRedirectCallback() 
    работают аналогичным образом для процесса выхода из системы.
*/
const userManagerSettings: UserManagerSettings = {
    client_id: 'notes-web-app',
    redirect_uri: 'http://localhost:3000/signin-oidc',
    response_type: 'code',
    scope: 'openid profile NotesWebAPI',
    authority: 'https://localhost:44384/',
    post_logout_redirect_uri: 'http://localhost:3000/signout-oidc',
};

const userManager = new UserManager(userManagerSettings);
export async function loadUser() {
    const user = await userManager.getUser();
    console.log('User: ', user);
    const token = user?.access_token;
    setAuthHeader(token);
}

export const signinRedirect = () => userManager.signinRedirect();

export const signinRedirectCallback = () =>
    userManager.signinRedirectCallback();

export const signoutRedirect = (args?: any) => {
    userManager.clearStaleState();
    userManager.removeUser();
    return userManager.signoutRedirect(args);
};

export const signoutRedirectCallback = () => {
    userManager.clearStaleState();
    userManager.removeUser();
    return userManager.signoutRedirectCallback();
};

export default userManager;