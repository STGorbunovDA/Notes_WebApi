using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Commands.UpdateNote;

namespace Notes.WebApi.Models
{
    /*
        * В данном примере представлен класс UpdateNoteDto, который реализует интерфейс IMapWith<T> 
          с параметром типа UpdateNoteCommand. Этот класс является Data Transfer Object (DTO) 
          для обновления заметки и обеспечивает разделение внутреннего представления данных 
          и представления данных, используемого при обмене данными между клиентом и сервером 
          или между слоями приложения.

        * В классе UpdateNoteDto определены следующие части:

           * public Guid Id { get; set; }: свойство для хранения идентификатора заметки.

           * public string Title { get; set; }: свойство для хранения нового заголовка заметки.

           * public string Details { get; set; }: свойство для хранения новых деталей заметки.

           * Метод public void Mapping(Profile profile) реализует маппинг (сопоставление) 
             между UpdateNoteDto и UpdateNoteCommand. Библиотека AutoMapper используется 
             для автоматизации процесса маппинга между различными объектами.

           * Внутри метода Mapping определяется маппинг между указанными типами:
             
                * Здесь создается маппинг между UpdateNoteDto и UpdateNoteCommand, 
                  определяя, что свойства Id, Title и Details должны быть скопированы 
                  из объекта UpdateNoteDto в объект UpdateNoteCommand.

        В результате, этот класс предоставляет объявление DTO-объекта и определение 
        маппинга между UpdateNoteDto и UpdateNoteCommand, что облегчает преобразование 
        данных и их передачу между различными частями приложения при обновлении заметки.
    */
    public class UpdateNoteDto : IMapWith<UpdateNoteCommand>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateNoteDto, UpdateNoteCommand>()
                .ForMember(noteCommand => noteCommand.Id,
                    opt => opt.MapFrom(noteDto => noteDto.Id))
                .ForMember(noteCommand => noteCommand.Title,
                    opt => opt.MapFrom(noteDto => noteDto.Title))
                .ForMember(noteCommand => noteCommand.Details,
                    opt => opt.MapFrom(noteDto => noteDto.Details));
        }
    }
}
