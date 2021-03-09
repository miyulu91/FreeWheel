using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Models;
using AutoMapper;
using Data;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Movies
{
    public class TopMoviesByUser
    {

        // IRequest is a mediator interface type. It will determine what needs to be returned by the query
        public class Query : IRequest<Result<List<MovieDto>>>
        {
            public string Username { get; set; }
        }

        // IRequest handler handles the query and the return type. i.e take care of the handling the request. getting
        public class Handler : IRequestHandler<Query, Result<List<MovieDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            // where a constructor is generated, the constructor will be executed first
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            // this is where the logic happens.
            public async Task<Result<List<MovieDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);

                // error handling method and the logic resides in the Base Controller
                // return 404 Not Found if no user found
                if (user == null)
                    return null;

                // get the top 5 movies by specific user
                var topMovies = await _context.Movies.Where(x => x.MovieRatings.Any(m => m.AppUser.Id == user.Id))
                    .OrderByDescending(o => o.MovieRatings.First().Rating)
                    .ThenBy(o => o.MovieRatings.First().Movie.Title)
                    .Take(5)
                    .ToListAsync();

                // error handling method and the logic resides in the Base Controller
                // return 404 Not Found if no movie data
                if (topMovies.Count == 0)
                    return null;

                var movies = _mapper.Map<List<Movie>, List<MovieDto>>(topMovies);

                return Result<List<MovieDto>>.Success(movies);
            }
        }
    }
}
