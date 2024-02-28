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
    public class User
    {
        [Key]
        [Column("user_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsDeleted { get; set; }

        [JsonIgnore]
        public ICollection<Reservation>? Reservations { get; set; }
        [JsonIgnore]
        public ICollection<Rating>? Ratings { get; set; }
        [JsonIgnore]
        public ICollection<Report>? Reports { get; set; }
        [JsonIgnore]
        public ICollection<User>? Artists { get; set; }
        [JsonIgnore]
        public ICollection<Artwork>? Artworks { get; set; }
        [JsonIgnore]
        public ICollection<Package>? Packages { get; set; }
    }

}
