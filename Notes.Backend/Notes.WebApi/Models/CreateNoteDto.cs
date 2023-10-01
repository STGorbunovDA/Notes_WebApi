using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Commands.CreateNote;

namespace Notes.WebApi.Models
{
    /*
        * Здесь представлен класс CreateNoteDto, который реализует интерфейс IMapWith<T> 
          c параметром типа CreateNoteCommand. Класс предназначен для представления 
          промежуточного объекта передачи данных (Data Transfer Object, DTO) 
          при создании заметки. Использование DTO-объектов позволяет разграничить 
          внутреннее представление данных в приложении от того, как данные передаются 
          между клиентом и сервером или между слоями приложения.

        * В классе CreateNoteDto определены следующие части:

          * public string Title { get; set; }: свойство для хранения заголовка заметки.

          * public string Details { get; set; }: свойство для хранения деталей заметки.

          * Метод public void Mapping(Profile profile) реализует маппинг (сопоставление) 
            между CreateNoteDto и CreateNoteCommand. Для этого используется библиотека 
            AutoMapper, которая помогает автоматизировать процесс маппинга 
            между различными объектами.

          * Внутри метода Mapping определяется маппинг между указанными типами:
          
            * Здесь создается маппинг между CreateNoteDto и CreateNoteCommand, определяя, 
              что свойства Title и Details должны быть скопированы из объекта CreateNoteDto 
              в объект CreateNoteCommand.

        Таким образом, этот класс предоставляет объявление DTO-объекта и определение 
        маппинга между CreateNoteDto и CreateNoteCommand, что упрощает преобразование 
        данных и их передачу между различными частями приложения.
    */
    public class CreateNoteDto : IMapWith<CreateNoteCommand>
    {
        public string Title { get; set; }
        public string Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateNoteDto, CreateNoteCommand>()
                .ForMember(noteCommand => noteCommand.Title,
                    opt => opt.MapFrom(noteDto => noteDto.Title))
                .ForMember(noteCommand => noteCommand.Details,
                    opt => opt.MapFrom(noteDto => noteDto.Details));
        }
    }
}
