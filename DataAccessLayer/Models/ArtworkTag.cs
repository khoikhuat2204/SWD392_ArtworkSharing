using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

public class ArtworkTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int ArtworkId { get; set; }
    [Required]
    public int TagId { get; set; }
    
    [ForeignKey("ArtworkId")]
    public virtual Artwork Artwork { get; set; }
    
    [ForeignKey("TagId")]
    public virtual Tag Tag { get; set; }
}
