using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DataAccessLayer.Models
{
    public class Rating
    {
        [Key]
        [Column("rating_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Score { get; set; }

        [ForeignKey(nameof(Models.User))]
        public int UserId { get; set; }
        
        [ForeignKey(nameof(Models.Artwork))]
        public int ArtworkId { get; set; }
        public bool IsDeleted { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Artwork? Artwork { get; set; }
    }
}
