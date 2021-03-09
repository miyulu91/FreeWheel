using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Movies;
using AutoMapper;
using Data;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseLazyLoadingProxies();
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            // Configure the application to use the IMediator
            services.AddMediatR(typeof(ListMovies.Handler).Assembly);

            // Configure the application to use AutoMapper
            services.AddAutoMapper(typeof(ListMovies.Handler));

            return services;
        }
    }
}
