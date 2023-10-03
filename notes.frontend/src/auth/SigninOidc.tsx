import { useEffect, FC } from 'react';
import { useNavigate } from 'react-router-dom';
import { signinRedirectCallback } from './user-service';

/*
    * Это компонент (функциональный) React, который используется для реализации OpenID Connect signin редиректа во фронтенд приложении.

    * Вот, что здесь происходит:

        * Компонент React, SigninOidc, объявлен как функциональный компонент (FC) без пропсов ({}).

        * Внутри компонента используется хук useNavigate из react-router-dom для управления навигацией по приложению. 
          navigate - это функция, которую вы можете использовать для перенаправления пользователя на другие маршруты 
          в вашем приложении.

        * Хук эффекта useEffect вызывается только один раз (поскольку второй аргумент - пустой массив зависимостей), 
          когда компонент монтируется. Внутри этого эффекта выполняется асинхронная функция signinAsync.

        * Функция signinAsync выполняет signinRedirectCallback - функцию, которую, судя по всему, передали из внешнего 
          сервиса для обработки редиректа и авторизации пользователя посредством OpenID Connect.

        * После успешной авторизации пользователь перенаправляется на главную страницу ('/') приложения.

        * Во время редиректа и выполнения всех необходимых операций отображается сообщение "Redirecting...", 
          информирующее пользователя о происходящем. Приведенный разметка React возвращает этот текст в обертке div.

        * В конце компонент экспортируется как модуль по умолчанию.

    Все эти действия служат обработке результата выхода пользователя через OpenID Connect, и, если все проходит успешно, 
    пользователь перенаправляется на главную страницу после входа в систему.
*/
const SigninOidc: FC<{}> = () => {
    const navigate = useNavigate();
    useEffect(() => {
        async function signinAsync() {
            await signinRedirectCallback();
            navigate('/');
        }
        signinAsync();
    }, [navigate]);

    return <div>Redirecting...</div>;
};

export default SigninOidc;