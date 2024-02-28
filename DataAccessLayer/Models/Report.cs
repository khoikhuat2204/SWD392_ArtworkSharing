using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Report
    {
        [Key]
        [Column("report_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public int CauseId { get; set; }

        public int UserId { get; set; }
        public int ArtworkId { get; set; }

        public ReportCause? Cause { get; set; }
        public User? User { get; set; }
        public Artwork? Artwork { get; set; }
    }
}
