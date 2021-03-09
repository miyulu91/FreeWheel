using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MovieRating
    {
        [Key]
        public int Id { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int Rating { get; set; }
    }
}
