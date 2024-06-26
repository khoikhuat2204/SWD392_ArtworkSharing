﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Enum;

namespace DataAccessLayer.Models
{
    public class Artwork
    {
        [Key]
        [Column("artwork_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? ImagePath { get; set; }
        [ForeignKey(nameof(ArtworkType))]
        public int TypeId { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public ArtworkStatus ArtworkStatus { get; set; }
        public User? Artist { get; set; }
        public ArtworkType? ArtworkType { get; set; }

        public ICollection<Report>? Reports { get; set; }
        public virtual ICollection<ArtworkTag> ArtworkTags { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }

    }
}
