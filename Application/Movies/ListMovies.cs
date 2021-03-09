using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Models;
using AutoMapper;
using Castle.Core.Internal;
using Data;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Movies
{
    public class ListMovies
    {

        // IRequest is a mediator interface type. It will determine what needs to be returned by the query
        public class Query : IRequest<Result<List<MovieDto>>>
        {
            public string Title { get; }
            public string YearOfRelease { get; }
            public string Genre { get; }

            public Query(string title, string yearOfRelease, string genre)
            {
                Title = title;
                YearOfRelease = yearOfRelease;
                Genre = genre;
            }
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
                // Cancellation token can be used to prevent the operation from carrying on with the request if the user cancels the request.
                // return 400 Bad Request if all the criteria are empty
                if (string.IsNullOrWhiteSpace(request.Title) && string.IsNullOrWhiteSpace(request.Genre) &&
                    string.IsNullOrWhiteSpace(request.YearOfRelease))
                    return Result<List<MovieDto>>.Failure("No criteria is given");

                var queryable = _context.Movies.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Title))
                    queryable = queryable.Where(x => x.Title.Contains(request.Title));
                if (!string.IsNullOrWhiteSpace(request.Genre))
                    queryable = queryable.Where(x => x.MovieGenres.Any(m => m.Genre.GenreTitle == request.Genre));
                if (!string.IsNullOrWhiteSpace(request.YearOfRelease))
                    queryable = queryable.Where(x => x.YearOfRelease == request.YearOfRelease);

                var filteredMovies = await queryable.OrderBy(x => x.Title).ToListAsync();

                // error handling method and the logic resides in the Base Controller
                // return 404 Not Found if no movie data
                if (filteredMovies.Count == 0)
                    return null;

                var movies = _mapper.Map<List<Movie>, List<MovieDto>>(filteredMovies);

                return Result<List<MovieDto>>.Success(movies);
            }
        }
    }
}
