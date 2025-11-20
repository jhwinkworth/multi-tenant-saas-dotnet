using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider)
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public Guid CurrentTenantId => _tenantProvider.CurrentTenantId;

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
        modelBuilder.Entity<Project>().HasQueryFilter(p => p.TenantId == CurrentTenantId);
        modelBuilder.Entity<TaskItem>().HasQueryFilter(t => t.Project.TenantId == CurrentTenantId);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.TenantId == CurrentTenantId);
        modelBuilder.Entity<Subscription>().HasQueryFilter(s => s.TenantId == CurrentTenantId);

        // Plans can be global or tenant-specific depending on your SaaS model
    }
}
