using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3.Models.Domain
{
    [Table("Movie")]
    public class Movie
    {
        // Primary key
        [Key]
        public int Id { get; set; }

        // Fields
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Genre { get; set; }

        public int ReleaseYear { get; set; }

        [MaxLength(100)]
        public string Director { get; set; }

        [MaxLength(200)]
        public string Picture { get; set; }

        [MaxLength(200)]
        public string Trailer { get; set; }

        // Relationships
        
        public int? FranchiseId { get; set; }
        public Franchise Franchise { get; set; }
        public ICollection<Character> Characters { get; set; }
    }
}
