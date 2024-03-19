using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public virtual Artwork Artwork { get; set; }
    
    [ForeignKey("TagId")]   
    [JsonIgnore]
    public virtual Tag Tag { get; set; }
}
