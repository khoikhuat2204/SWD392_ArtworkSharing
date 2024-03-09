using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ActiveSubscription
    {
        [Key]
        [Column("active_subscription_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey(nameof(Models.User))]
        public int UserId { get; set; }
        
        [ForeignKey(nameof(Models.Package))]
        public int PackageId { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public User? User { get; set; }
        public Package? Package { get; set; }
    }
}
