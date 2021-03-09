using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.Movies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MovieController : BaseController
    {
        [Route("listMovies")]
        [HttpGet]
        public async Task<IActionResult> ListMovies(string title, string yearOfRelease, string genre)
        {
            var result = await Mediator.Send(new ListMovies.Query(title, yearOfRelease, genre));

            return HandleResult(result);
        }

        [Route("topMoviesByAverage")]
        [HttpGet]
        public async Task<IActionResult> TopMoviesByAverage()
        {
            var result = await Mediator.Send(new TopMoviesByAverage.Query());

            return HandleResult(result);
        }

        [Route("topMoviesByUser")]
        [HttpGet]
        public async Task<IActionResult> TopMoviesByUser(string username)
        {
            var result = await Mediator.Send(new TopMoviesByUser.Query
            {
                Username = username
            });

            return HandleResult(result);
        }

        [Route("addMovieRating")]
        [HttpPost]
        public async Task<IActionResult>AddMovieRating(AddMovieRating.Command command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }
}
