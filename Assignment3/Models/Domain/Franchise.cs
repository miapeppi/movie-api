using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3.Models.Domain
{
    [Table("Franchise")]
    public class Franchise
    {
        // Primary key
        [Key]
        public int Id { get; set; }

        // Fields
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        // Relationships
        public ICollection<Movie> Movies { get; set; }
    }
}
