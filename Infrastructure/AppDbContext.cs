using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

public class AppDbContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider)
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public Guid CurrentTenantId => _tenantProvider.TenantId;

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Plan> Plans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global query filters for multi-tenancy
        modelBuilder.Entity<Project>().HasQueryFilter(p => p.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<TaskItem>().HasQueryFilter(t => t.Project.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<Subscription>().HasQueryFilter(s => s.TenantId == _tenantProvider.TenantId);
    }
}
