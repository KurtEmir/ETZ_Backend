namespace ETZ.Persistence.Context;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public sealed class ETZDbContextFactory : IDesignTimeDbContextFactory<ETZDbContext>
{
    public ETZDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ETZDbContext>();

        optionsBuilder.UseNpgsql("Server=localhost,5432;Database=ETZ;User Id=postgres;Password=a.A12345;TrustServerCertificate=True;");

        return new ETZDbContext(optionsBuilder.Options);
    }
}


