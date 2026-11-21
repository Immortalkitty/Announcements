using Announcements.DTOs;
using Announcements.Models;
using Microsoft.AspNetCore.Mvc;

namespace Announcements.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController : ControllerBase
{
    private static List<Announcement> _announcements = new List<Announcement>
    {
        new Announcement
        {
            Id = 1,
            Title = "Witamy",
            Content = "To jest pierwsze ogłoszenie.",
            AuthorId = 1
        },
        new Announcement
        {
            Id = 2,
            Title = "Aktualności",
            Content = "Tutaj pojawią się najnowsze informacje.",
            AuthorId = 1
        }
    };

    [HttpGet]
    public ActionResult<IEnumerable<AnnouncementDto>> GetAll()
    {
        var dtos = _announcements.Select(announcement => new AnnouncementDto
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            CreatedAt = announcement.CreatedAt
        });

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public ActionResult<AnnouncementDto> GetById(int id)
    {
        var announcement = _announcements.FirstOrDefault(item => item.Id == id);
        if (announcement == null)
        {
            return NotFound("Nie znaleziono takiego ogłoszenia!");
        }

        var dto = new AnnouncementDto
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            CreatedAt = announcement.CreatedAt
        };

        return Ok(dto);
    }

    [HttpPost]
    public ActionResult Create(CreateAnnouncementDto dto)
    {
        var newId = _announcements.Any() ? _announcements.Max(announcement => announcement.Id) + 1 : 1;
        var newAnnouncement = new Announcement
        {
            Id = newId,
            Title = dto.Title,
            Content = dto.Content,
            AuthorId = 1
        };

        _announcements.Add(newAnnouncement);

        return CreatedAtAction(nameof(GetById), new { id = newId }, dto);
    }
}
