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
using Microsoft.EntityFrameworkCore;

namespace Application.Movies
{
    public class TopMoviesByAverage
    {

        // IRequest is a mediator interface type. It will determine what needs to be returned by the query
        public class Query : IRequest<Result<List<MovieDto>>> { }

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
                // cancellation token is used to prevent the operation from carrying on with the request if the user cancels the request.
                // get top 5 movies by average
                var topMovies = await _context.Movies
                    .OrderByDescending(m => m.MovieRatings.Average(x => x.Rating))
                    .ThenBy(o => o.Title)
                    .Take(5).ToListAsync();

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
