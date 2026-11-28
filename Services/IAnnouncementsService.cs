using Announcements.DTOs;

namespace Announcements.Services;

public interface IAnnouncementsService
{
    IEnumerable<AnnouncementDto> GetAll();
    AnnouncementDto GetById(int id, int authorId);
    int Create(CreateAnnouncementDto dto, int authorId);
    void Update(int id, UpdateAnnouncementDto dto, int authorId);
    void Delete(int id, int authorId);
}
