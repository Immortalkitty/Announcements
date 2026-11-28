using Announcements.DTOs;
using Announcements.Services;
using Microsoft.AspNetCore.Mvc;

namespace Announcements.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController : ControllerBase
{
    private const int CurrentAuthorId = 1;
    private readonly IAnnouncementsService _announcementsService;

    public AnnouncementsController(IAnnouncementsService announcementsService)
    {
        _announcementsService = announcementsService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AnnouncementDto>> GetAll()
    {
        return Ok(_announcementsService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<AnnouncementDto> GetById(int id)
    {
        try
        {
            return Ok(_announcementsService.GetById(id, CurrentAuthorId));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }

    [HttpPost]
    public ActionResult Create(CreateAnnouncementDto dto)
    {
        try
        {
            var newId = _announcementsService.Create(dto, CurrentAuthorId);
            var createdAnnouncement = _announcementsService.GetById(newId, CurrentAuthorId);

            return CreatedAtAction(nameof(GetById), new { id = newId }, createdAnnouncement);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, UpdateAnnouncementDto dto)
    {
        try
        {
            _announcementsService.Update(id, dto, CurrentAuthorId);
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            _announcementsService.Delete(id, CurrentAuthorId);
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
