using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    /*
        * Данный фрагмент кода определяет класс NoteDetailsVm (сокращение от NoteDetailsViewModel), 
          который является представлением подробной информации о заметке. Он используется 
          для передачи данных между слоями приложения, например между слоем бизнес-логики 
          и пользовательским интерфейсом.

        * Класс NoteDetailsVm реализует интерфейс IMapWith<Note>, который указывает на то, 
          что данный класс может быть отображен из класса Note, используя AutoMapper.

        * Класс содержит следующие свойства:

            * Id (тип Guid): Идентификатор заметки.
            * Title (тип string): Заголовок заметки.
            * Details (тип string): Детали заметки.
            * CreationDate (тип DateTime): Дата создания заметки.
            * EditDate (тип DateTime?): Дата последнего редактирования заметки, может быть null, 
              если заметка ни разу не была отредактирована.

        * В методе Mapping, который имеет параметр profile типа Profile, 
          производится определение настроек отображения (mapping) между классами 
          Note и NoteDetailsVm. В данном случае используются методы из библиотеки AutoMapper, 
          которые указывают, как свойства объекта класса Note должны быть отображены 
          на соответствующие свойства объекта класса NoteDetailsVm. Это делается с помощью 
          метода ForMember, который принимает два аргумента:

            * Лямбда-выражение, указывающее на свойство объекта 
              типа NoteDetailsVm, которое требуется заполнить.
            * Лямбда-выражение с opt.MapFrom(), которое сообщает AutoMapper, 
              из какого свойства объекта типа Note нужно взять значения для указанного 
              свойства объекта типа NoteDetailsVm.

        Таким образом, класс NoteDetailsVm предназначен для хранения и передачи информации 
        о заметках между слоями приложения, а также включает настройки для отображения 
        из класса Note с использованием библиотеки AutoMapper.
    */
    public class NoteDetailsVm : IMapWith<Note>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditDate { get; set; }

        public void Mapping(Profile profile) 
        {
            profile.CreateMap<Note, NoteDetailsVm>()
                .ForMember(noteVm => noteVm.Title,
                opt => opt.MapFrom(note => note.Title))
                .ForMember(noteVm => noteVm.Details,
                opt => opt.MapFrom(note => note.Details))
                .ForMember(noteVm => noteVm.Id,
                opt => opt.MapFrom(note => note.Id))
                .ForMember(noteVm => noteVm.CreationDate,
                opt => opt.MapFrom(note => note.CreationDate))
                .ForMember(noteVm => noteVm.EditDate,
                opt => opt.MapFrom(note => note.EditDate));
        }
    }
}
