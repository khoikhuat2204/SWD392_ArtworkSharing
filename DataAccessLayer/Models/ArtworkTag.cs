using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

public class ArtworkTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey(nameof(Artwork))]
    public int ArtworkId { get; set; }
    public Artwork? Artwork { get; set; }
    
    [ForeignKey(nameof(Tag))]
    public int TagId { get; set; }
    public Tag? Tag { get; set; }
    
}