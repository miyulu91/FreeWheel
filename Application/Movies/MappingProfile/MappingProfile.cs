using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Movies.Resolvers;
using AutoMapper;
using Domain;

namespace Application.Movies.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDto>()
                .ForMember(d => d.Genres, o => o.MapFrom<GenreResolver>())
                .ForMember(d => d.AverageRating, o => o.MapFrom<AverageRatingResolver>());
        }
    }
}
