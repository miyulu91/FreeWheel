using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace API.Tests
{
    [TestFixture]
    public class MovieControllerTests
    {
        [Test]
        public void ListMovies_EmptyParams_ThrowError()
        {
            var obj = new MovieController();

            Assert.ThrowsAsync<Exception>(() => obj.ListMovies("", "", ""));
        }

        //TODO more test cases and more scenarios can be added
    }
}
