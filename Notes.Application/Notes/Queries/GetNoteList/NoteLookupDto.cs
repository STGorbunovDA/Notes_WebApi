using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Domain;

namespace Notes.Application.Notes.Queries.GetNoteList
{
    /*
        * Здесь определен класс NoteLookupDto, который является Data Transfer Object (DTO) 
          для объектов класса Note. DTO используется для передачи данных между слоями 
          приложения или для сериализации при взаимодействии с API. Класс NoteLookupDto 
          также реализует интерфейс IMapWith<Note>, что, возможно, означает наличие 
          функциональности отображения данных между Note и NoteLookupDto.

        * Класс содержит два свойства:

            * Id (тип Guid): уникальный идентификатор заметки.
            * Title (тип string): заголовок заметки.
        * Кроме того, класс содержит метод Mapping(Profile profile), 
          который определяет правила отображения свойств между классами Note и NoteLookupDto 
          с использованием библиотеки AutoMapper. AutoMapper помогает автоматически выполнять 
          преобразования между объектами разных типов.

        * В методе Mapping:

            * Создается новое отображение из класса Note в класс NoteLookupDto 
              с использованием метода CreateMap. Это указывает AutoMapper, что нужно 
              создать отображение между этими двумя классами.

            * Затем определяются два отображения между соответствующими свойствами классов:
            * 
                * Метод ForMember указывает на свойство noteDto.Id и настраивает его 
                  с помощью opt.MapFrom, чтобы данные брались из свойства note.Id.
                * Метод ForMember указывает на свойство noteDto.Title и настраивает 
                  его с помощью opt.MapFrom, чтобы данные брались из свойства note.Title.

        После определения этих отображений, AutoMapper сможет автоматически преобразовывать 
        объекты типа Note в объекты типа NoteLookupDto и наоборот.
    */
    public class NoteLookupDto : IMapWith<Note>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Note, NoteLookupDto>()
               .ForMember(noteDto => noteDto.Id,
               opt => opt.MapFrom(note => note.Id))
               .ForMember(noteDto => noteDto.Title,
               opt => opt.MapFrom(note => note.Title));
        }
    }
}
