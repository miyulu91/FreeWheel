using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string YearOfRelease { get; set; }
        public int RunningTime { get; set; }
        public string Genres { get; set; }
        public decimal AverageRating { get; set; }
    }
}
