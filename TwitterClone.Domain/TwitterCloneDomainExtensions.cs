using System;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using TwitterClone.Domain.Commands;
using TwitterClone.Domain.Database;

namespace TwitterClone.Domain;

public static class TwitterCloneDomainExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> dbOptionsAction)
    {
        return services.AddMediatR(typeof(CreateUserCommand))
            .AddDbContext<TwitterCloneDbContext>(dbOptionsAction); ;
    }
}