/*
    * Этот блок кода объявляет класс под названием ClientBase. В классе ClientBase 
      есть один метод transformOptions, который принимает один аргумент options. 
      Этот аргумент предположительно является объектом конфигурации для HTTP-запроса 
      (типа RequestInit).

    * В теле метода выполнено несколько операций:

        * Переменная token получает значение, хранящееся в localStorage под ключом 'token'. 
          Это значит, что сгенерированный аутентификационный токен был сохранен в localStorage 
          и теперь извлекается для дальнейшего использования.

        * После этого в options.headers добавляется поле Authorization со значением 'Bearer ' + token. 
          Механизм Bearer Authorization часто используется в схемах аутентификации, основанных на токенах, 
          где токен используется для идентификации клиента.

        * Метод завершается возвратом нового объекта options через Promise.resolve(), что означает, 
          что любой вызывающий код должен обрабатывать результат этого метода как возвращаемое 
          значение обещания (promise).

    В общем, этот код предназначен для добавления аутентификационного заголовка с помощью токена 
    из локального хранилища в предоставленные настройки запроса перед выполнением HTTP-запроса.
*/
export class ClientBase {
    protected transformOptions(options: RequestInit) {
        const token = localStorage.getItem('token');
        options.headers = {
            ...options.headers,
            Authorization: 'Bearer ' + token,
        };
        return Promise.resolve(options);
    }
}