using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string YearOfRelease { get; set; }
        public int RunningTime { get; set; }
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<MovieRating> MovieRatings { get; set; }
    }
}
