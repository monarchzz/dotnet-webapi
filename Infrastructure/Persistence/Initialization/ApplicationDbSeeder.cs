using Application.Common.Auth;
using Domain.Catalog;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace Infrastructure.Persistence.Initialization;

internal class ApplicationDbSeeder
{
    private readonly CustomSeederRunner _seederRunner;
    private readonly ILogger<ApplicationDbSeeder> _logger;
    private readonly IPasswordHelper _passwordHelper;

    public ApplicationDbSeeder(
        CustomSeederRunner seederRunner,
        ILogger<ApplicationDbSeeder> logger,
        IPasswordHelper passwordHelper
    )
    {
        _seederRunner = seederRunner;
        _logger = logger;
        _passwordHelper = passwordHelper;
    }

    public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        await SeedAdminUserAsync(dbContext, cancellationToken);
        await _seederRunner.RunSeedersAsync(cancellationToken);
    }


    private async Task SeedAdminUserAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var adminUser = new User(
            firstName: "Admin",
            lastName: "User",
            email: "admin@gmail.com",
            password: _passwordHelper.HashPassword("123123"),
            role: Role.Admin
        );

        await dbContext.Users.AddAsync(adminUser, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeding Default Admin User");
    }
}