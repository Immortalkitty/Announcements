using System.ComponentModel.DataAnnotations;

namespace Announcements.DTOs;

public class UpdateAnnouncementDto
{
    [Required(ErrorMessage = "Tytuł ogłoszenia jest wymagany.")]
    [StringLength(100, ErrorMessage = "Tytuł ogłoszenia nie może przekraczać 100 znaków.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Treść ogłoszenia jest wymagana.")]
    [StringLength(5000, ErrorMessage = "Treść ogłoszenia nie może przekraczać 5000 znaków.")]
    public string Content { get; set; } = string.Empty;
}
