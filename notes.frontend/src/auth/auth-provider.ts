import React, { FC, useEffect, useRef, ReactNode  } from 'react';
import { User, UserManager } from 'oidc-client';
import { setAuthHeader } from './auth-headers';

/*
    * Этот React компонент является кастомным провайдером аутентификации, который использует библиотеку 'oidc-client' 
      для аутентификации пользователей.

    * Сначала он принимает два пропса: 'userManager' (который является экземпляром UserManager из библиотеки oidc-client) 
      для управления вопросами аутентификации и 'children', которые являются дочерними элементами, которые AuthProvider обернет.

    * Внутри useEffect, компонент настраивает несколько обработчиков событий для различных событий, связанных с аутентификацией 
      пользователя (например, когда пользователь загружается, когда пользователь разгружается, когда токен доступа истекает и т. д.). 
      Каждый из этих обработчиков событий просто выводит в консоль, что произошло, и, если необходимо, устанавливает заголовок 
      авторизации для последующих запросов API.

    * Кроме того, при вызове функции очистки (которая вызывается, когда компонент будет удален из DOM), обработчики событий отключаются, 
      чтобы избежать утечки памяти.

    * Наконец, компонент возвращает только одного потомка из дочерних элементов пропса. Это необходимо, чтобы гарантировать, 
      что AuthProvider может оборачивать только один элемент.

    В общих чертах, этот компонент помогает интегрировать в ваше приложение поддержку протокола аутентификации 
    OpenID Connect (OIDC) с помощью библиотеки oidc-client, а также добавляет механизм для установки заголовка 
    авторизации для каждого запроса (возможно, к REST API) после авторизации пользователя.
*/

type AuthProviderProps = {
    userManager: UserManager;
    children: ReactNode;
};

const AuthProvider: FC<AuthProviderProps> = ({
    userManager: manager,
    children,
}): any => {
    let userManager = useRef<UserManager>();
    useEffect(() => {
        userManager.current = manager;
        const onUserLoaded = (user: User) => {
            console.log('User loaded: ', user);
            setAuthHeader(user.access_token);
        };
        const onUserUnloaded = () => {
            setAuthHeader(null);
            console.log('User unloaded');
        };
        const onAccessTokenExpiring = () => {
            console.log('User token expiring');
        };
        const onAccessTokenExpired = () => {
            console.log('User token expired');
        };
        const onUserSignedOut = () => {
            console.log('User signed out');
        };

        userManager.current.events.addUserLoaded(onUserLoaded);
        userManager.current.events.addUserUnloaded(onUserUnloaded);
        userManager.current.events.addAccessTokenExpiring(
            onAccessTokenExpiring
        );
        userManager.current.events.addAccessTokenExpired(onAccessTokenExpired);
        userManager.current.events.addUserSignedOut(onUserSignedOut);

        return function cleanup() {
            if (userManager && userManager.current) {
                userManager.current.events.removeUserLoaded(onUserLoaded);
                userManager.current.events.removeUserUnloaded(onUserUnloaded);
                userManager.current.events.removeAccessTokenExpiring(
                    onAccessTokenExpiring
                );
                userManager.current.events.removeAccessTokenExpired(
                    onAccessTokenExpired
                );
                userManager.current.events.removeUserSignedOut(onUserSignedOut);
            }
        };
    }, [manager]);

    return React.Children.only(children);
};

export default AuthProvider;