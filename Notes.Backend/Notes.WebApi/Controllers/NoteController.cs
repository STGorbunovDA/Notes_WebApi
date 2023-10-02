using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.Application.Notes.Commands.DeleteCommand;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.WebApi.Models;

namespace Notes.WebApi.Controllers
{
    /*
        * В этом примере представлен контроллер NoteController, который наследуется 
          от базового контроллера BaseController. NoteController отвечает за выполнение 
          CRUD операций (создание, чтение, обновление и удаление) с записями заметок (Notes). 

        * Вот что делает каждый метод:

            * Конструктор: принимает объект IMapper и инициализирует приватное поле _mapper. 
              Это поле будет использоваться для преобразования между разными типами объектов.

            * GetAll(): этот метод обрабатывает HTTP GET запрос и возвращает 
              все записи заметок для текущего пользователя. Он создает объект 
              GetNoteListQuery с идентификатором пользователя и отправляет его 
              с помощью медиатора. Результат возвращается как объект NoteListVm 
              в виде HTTP 200 (ОК) ответа.

            * Get(Guid id): этот метод обрабатывает HTTP GET запрос с указанным 
              идентификатором заметки и возвращает информацию о заметке. 
              Он создает объект GetNoteDetailsQuery с идентификатором пользователя 
              и идентификатором заметки, отправляет его с помощью медиатора 
              и возвращает результат как объект NoteDetailsVm в виде HTTP 200 (ОК) ответа.

            * Create(CreateNoteDto createNoteDto): этот метод обрабатывает HTTP POST 
              запрос и создает новую заметку с данными, переданными в теле запроса. 
              Используется AutoMapper для преобразования объекта CreateNoteDto 
              в объект CreateNoteCommand с последующей передачей идентификатора 
              пользователя. Затем отправляет команду с помощью медиатора и возвращает 
              идентификатор созданной заметки в виде HTTP 200 (ОК) ответа.

            * Update(UpdateNoteDto updateNoteDto): этот метод обрабатывает HTTP PUT 
              запрос и обновляет существующую заметку с данными, переданными в теле запроса. 
              Используется AutoMapper для преобразования объекта UpdateNoteDto 
              в объект UpdateNoteCommand с последующей передачей идентификатора 
              пользователя. Затем отправляет команду с помощью медиатора и возвращает 
              HTTP 204 (No Content) ответ.

            * Delete(Guid id): этот метод обрабатывает HTTP DELETE запрос и 
              удаляет заметку с указанным идентификатором. Создает объект 
              DeleteNoteCommand с идентификатором пользователя и идентификатором 
              заметки, отправляет его с помощью медиатора и возвращает HTTP 204 (No Content) ответ.

        В данном контроллере используется медиатор (MediatR) для обработки команд 
        и запросов, а также AutoMapper для преобразования объектов между различными 
        слоями приложения (DTO, ViewModel и т.д.).
    */

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class NoteController : BaseController
    {
        private readonly IMapper _mapper;

        public NoteController(IMapper mapper) => _mapper = mapper;

        /// <summary>
        /// Gets the list of notes
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /note
        /// </remarks>
        /// <returns>Returns NoteListVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<NoteListVm>> GetAll()
        {
            var query = new GetNoteListQuery
            {
                UserId = UserId
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Gets the note by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /note/D34D349E-43B8-429E-BCA4-793C932FD580
        /// </remarks>
        /// <param name="id">Note id (guid)</param>
        /// <returns>Returns NoteDetailsVm</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user in unauthorized</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<NoteDetailsVm>> Get(Guid id)
        {
            var query = new GetNoteDetailsQuery
            {
                UserId = UserId,
                Id = id
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        /// <summary>
        /// Creates the note
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /note
        /// {
        ///     title: "note title",
        ///     details: "note details"
        /// }
        /// </remarks>
        /// <param name="createNoteDto">CreateNoteDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto)
        {
            var command = _mapper.Map<CreateNoteCommand>(createNoteDto);
            command.UserId = UserId;
            var noteId = await Mediator.Send(command);
            return Ok(noteId);
        }

        /// <summary>
        /// Updates the note
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT /note
        /// {
        ///     title: "updated note title"
        /// }
        /// </remarks>
        /// <param name="updateNoteDto">UpdateNoteDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
        {
            var command = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
            command.UserId = UserId;
            await Mediator.Send(command);
            return NoContent();
        }
        /// <summary>
        /// Deletes the note by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE /note/88DEB432-062F-43DE-8DCD-8B6EF79073D3
        /// </remarks>
        /// <param name="id">Id of the note (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteNoteCommand
            {
                Id = id,
                UserId = UserId
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
