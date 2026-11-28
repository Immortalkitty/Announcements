using Announcements.DTOs;
using Announcements.Models;

namespace Announcements.Services;

public class AnnouncementsService : IAnnouncementsService
{
    private readonly List<Announcement> _announcements =
    [
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
    ];

    public IEnumerable<AnnouncementDto> GetAll()
    {
        var now = DateTimeOffset.UtcNow;

        return _announcements
            .Where(announcement => announcement.ExpiresAt is null || announcement.ExpiresAt > now)
            .OrderByDescending(announcement => announcement.CreatedAt)
            .ThenByDescending(announcement => announcement.Id)
            .Select(ToDto)
            .ToList();
    }

    public AnnouncementDto GetById(int id, int authorId)
    {
        var announcement = _announcements.FirstOrDefault(item => item.Id == id);
        if (announcement is null)
        {
            throw new KeyNotFoundException($"Nie znaleziono ogłoszenia o ID {id}.");
        }

        if (announcement.ExpiresAt <= DateTimeOffset.UtcNow && announcement.AuthorId != authorId)
        {
            throw new KeyNotFoundException($"Nie znaleziono ogłoszenia o ID {id}.");
        }

        return ToDto(announcement);
    }

    public int Create(CreateAnnouncementDto dto, int authorId)
    {
        var title = dto.Title.Trim();
        EnsureTitleIsUnique(title, authorId);

        var newId = _announcements.Count > 0
            ? _announcements.Max(announcement => announcement.Id) + 1
            : 1;

        _announcements.Add(new Announcement
        {
            Id = newId,
            Title = title,
            Content = dto.Content,
            ExpiresAt = dto.ExpiresAt,
            AuthorId = authorId
        });

        return newId;
    }

    public void Update(int id, UpdateAnnouncementDto dto, int authorId)
    {
        var announcement = GetOwnedAnnouncement(id, authorId);
        var title = dto.Title.Trim();
        EnsureTitleIsUnique(title, authorId, id);

        announcement.Title = title;
        announcement.Content = dto.Content;
        announcement.ExpiresAt = dto.ExpiresAt;
    }

    public void Delete(int id, int authorId)
    {
        var announcement = GetOwnedAnnouncement(id, authorId);
        _announcements.Remove(announcement);
    }

    private Announcement GetOwnedAnnouncement(int id, int authorId)
    {
        var announcement = _announcements.FirstOrDefault(item => item.Id == id);
        if (announcement is null)
        {
            throw new KeyNotFoundException($"Nie znaleziono ogłoszenia o ID {id}.");
        }

        if (announcement.AuthorId != authorId)
        {
            throw new UnauthorizedAccessException("Możesz zmieniać tylko własne ogłoszenia.");
        }

        return announcement;
    }

    private void EnsureTitleIsUnique(string title, int authorId, int? excludedAnnouncementId = null)
    {
        var duplicateExists = _announcements.Any(announcement =>
            announcement.AuthorId == authorId
            && announcement.Id != excludedAnnouncementId
            && string.Equals(announcement.Title, title, StringComparison.OrdinalIgnoreCase));

        if (duplicateExists)
        {
            throw new InvalidOperationException("Masz już ogłoszenie o takim tytule.");
        }
    }

    private static AnnouncementDto ToDto(Announcement announcement)
    {
        return new AnnouncementDto
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            CreatedAt = announcement.CreatedAt,
            ExpiresAt = announcement.ExpiresAt
        };
    }
}
