using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3.Models.Domain
{
   [Table("Character")]
   public class Character
   {
       // Primary key
       [Key]
       public int Id { get; set; }

       // Fields
       [Required]
       [MaxLength(100)]
       public string FullName { get; set; }

       [MaxLength(100)]
       public string Alias { get; set; }

       [MaxLength(30)]
       public string Gender { get; set; }

       [MaxLength(200)]
       public string Picture { get; set; }

       // Relationships
       public ICollection<Movie> Movies { get; set; }
   }
}
