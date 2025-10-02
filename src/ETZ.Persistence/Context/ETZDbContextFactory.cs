namespace ETZ.Persistence.Context;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public sealed class ETZDbContextFactory : IDesignTimeDbContextFactory<ETZDbContext>
{
    public ETZDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ETZDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ETZ;Username=postgres;Password=a.A12345;");

        return new ETZDbContext(optionsBuilder.Options);
    }
}


