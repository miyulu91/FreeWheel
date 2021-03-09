using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using Domain;

namespace Application.Movies.Resolvers
{
    public class AverageRatingResolver : IValueResolver<Movie, MovieDto, decimal>
    {
        public decimal Resolve(Movie source, MovieDto destination, decimal destMember, ResolutionContext context)
        {
            var totalRatingCount = source.MovieRatings.Count;

            if (totalRatingCount == 0)
                return 0;

            var totalRatings = source.MovieRatings.Sum(x => x.Rating);
            var averageRating = Convert.ToDecimal(totalRatings) / Convert.ToDecimal(totalRatingCount);

            return Math.Round(averageRating, 1);
        }
    }
}
