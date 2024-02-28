using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Reservation
    {
        [Key]
        [Column("reservation_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? Date { get; set; }

        public int UserId { get; set; }
        public int PackageId { get; set; }
        public bool IsDeleted { get; set; }
        public int ArtworkId { get; set; }

        public User? User { get; set; }
        public Artwork? Artwork { get; set; }
    }
}
