import React, { FC, ReactElement, useRef, useEffect, useState } from 'react';
import { CreateNoteDto, Client, NoteLookupDto } from '../api/api';
import { FormControl } from 'react-bootstrap';

/*
    * Этот код представляет собой React-компонент в TypeScript, который использует пользовательский 
      API-клиент для взаимодействия с неким API сервера заметок.

    * Вот что происходит на каждой части кода:

        * Мы импортируем необходимые модули из 'react' и 'react-bootstrap'. Функциональный компонент (FC), 
          хук состояния (useState), хук эффекта (useEffect), и еще несколько других вещей из 'react', 
          а также FormControl из 'react-bootstrap' для создания доступного примитива формы.

        * Создаем экземпляр apiClient, который будет обрабатывать взаимодействие API. 
          Он принимает базовый URL сервера при создании экземпляра.

        * Есть функция createNote, которая асинхронно отправляет POST запрос на сервер для создания новой заметки.

        * Создаем функциональный компонент NoteList:

            * a. Инициализируем textInput как null с помощью useRef.

            * b. Создаем состояние notes для хранения заметок, изначально задаем его как undefined.

            * c. getNotes - асинхронная функция, которая получает все заметки с сервера и устанавливает их в состояние notes.

            * d. useEffect срабатывает после рендеринга компонента и запускает загрузку заметок с сервера через задержку в 500 мс.

            * e. Обработчик handleKeyPress запускается при нажатии клавиши в элементе ввода и, если эта клавиша - 'Enter', 
                 он создает новую заметку с введенным текстом, после чего очищает поле ввода и обновляет список заметок.

        * В основной разметке видим поле для ввода текста и секцию, где отображаются все заметки. 
          Заметка - это, по сути, строка текста. Каждая заметка отображается на новой строке.

    В целом, мы создаем простейшую веб-страницу для заметок, которая позволяет получать заметки с сервера, 
    создавать новые при нажатии клавиши 'Enter', и отображать их на странице.
*/

const apiClient = new Client('https://localhost:44384');

async function createNote(note: CreateNoteDto) {
    await apiClient.create('1.0', note);
    console.log('Note is created.');
}

const NoteList: FC<{}> = (): ReactElement => {
    let textInput = useRef(null);
    const [notes, setNotes] = useState<NoteLookupDto[] | undefined>(undefined);

    async function getNotes() {
        const noteListVm = await apiClient.getAll('1.0');
        setNotes(noteListVm.notes);
    }

    useEffect(() => {
        setTimeout(getNotes, 500);
    }, []);

    const handleKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter') {
            const note: CreateNoteDto = {
                title: event.currentTarget.value,
            };
            createNote(note);
            event.currentTarget.value = '';
            setTimeout(getNotes, 500);
        }
    };

    return (
        <div>
            Notes
            <div>
                <FormControl ref={textInput} onKeyPress={handleKeyPress} />
            </div>
            <section>
                {notes?.map((note) => (
                    <div>{note.title}</div>
                ))}
            </section>
        </div>
    );
};
export default NoteList;