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
    public class GenreResolver : IValueResolver<Movie, MovieDto, string>
    {
        public string Resolve(Movie source, MovieDto destination, string destMember, ResolutionContext context)
        {
            if (source.MovieGenres.Count > 0)
            {
                return string.Join(",", source.MovieGenres.Select(x => x.Genre.GenreTitle));
            }

            return "";
        }
    }
}
