using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Package
    {
        [Key]
        [Column("package_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int UploadsPerDay { get; set; }
        public int TotalUploads { get; set; }
        public bool IsDeleted { get; set; }
        public string? MonthlyStripePlanId { get; set; } // Added MonthlyStripePlanId
        public string? YearlyStripePlanId { get; set; } // Added YearlyStripePlanId

        public ICollection<ActiveSubscription>? ActiveSubscriptions { get; set; }
    }
}
