using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Data;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Movies
{
    public class AddMovieRating
    {

        public class Command : IRequest<Result<Unit>>
        {
            public string Title { get; set; }
            public string Username { get; set; }
            public int Rating { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);

                // error handling method and the logic resides in the Base Controller
                // return 404 Not Found if no user found
                if (user == null)
                    return null;

                var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Title == request.Title);

                // error handling method and the logic resides in the Base Controller
                // return 404 Not Found if no movie found
                if (movie == null)
                    return null;

                // error handling method and the logic resides in the Base Controller
                // return 400 Bad Request if Invalid Rating
                if (!Enumerable.Range(1, 5).Contains(request.Rating))
                    return Result<Unit>.Failure("Invalid Rating");

                var movieRating =
                    await _context.MovieRatings.FirstOrDefaultAsync(x =>
                        x.AppUser.Id == user.Id && x.Movie.Id == movie.Id);

                if (movieRating != null)
                {
                    movieRating.Rating = request.Rating;
                }
                else
                {
                    var newRating = new MovieRating
                    {
                        AppUser = user,
                        Movie = movie,
                        Rating = request.Rating
                    };

                    _context.MovieRatings.Add(newRating);
                }

                var success = await _context.SaveChangesAsync() > 0;

                // if update unsuccessful or if the new rating is same as the old rating, return following message
                if (!success) return Result<Unit>.Failure("Movie ratings not updated");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
