import { FC, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { signoutRedirectCallback } from './user-service';

/*
    * Данный код является примером функционального компонента в React, который использует React Router 
      для перенаправления пользователей и некую функцию signoutRedirectCallback, чтобы выполнять выход 
      пользователя из системы при использовании OpenID Connect (OIDC).
      
      * SignoutOidc объявлен как функциональный компонент без пропов (тип пустого объекта FC<{}>). 
      Внутри компонента мы используем хук useNavigate из react-router-dom для перенаправления пользователя 
      после выхода из системы.

      * Затем используется useEffect - это хук жизненного цикла React, который вызывается после рендеринга 
      компонента. useEffect принимает в качестве аргументов асинхронную функцию и массив зависимостей. 
      Вызываемая функция signoutAsync обращается к signoutRedirectCallback, которая, вероятно, обращается 
      к OpenID Connect провайдеру чтобы выполнить выход пользователя, а затем использует функцию navigate 
      для перенаправления пользователя на домашнюю страницу ('/'). В массиве зависимостей указан navigate 
      - это значит, что эффект активируется каждый раз, когда navigate меняется (в данном случае это 
        случается только один раз - при монтировании компонента).
*/

const SignoutOidc: FC<{}> = () => {
    const navigate = useNavigate();
    useEffect(() => {
        const signoutAsync = async () => {
            await signoutRedirectCallback();
            navigate('/');
        };
        signoutAsync();
    }, [navigate]);
    return <div>Redirecting...</div>;
};

export default SignoutOidc;