using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

public class FavoriteArtwork
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ArtworkId { get; set; }

    [Required]
    public DateTime Timestamp { get; set; } = DateTime.Now;

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    [ForeignKey("ArtworkId")]
    public virtual Artwork Artwork { get; set; }
}