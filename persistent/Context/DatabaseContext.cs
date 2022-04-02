using Application.Interfaces.Contexts;
using Domain.Attributes;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace persistent.Context
{
    public class DatabaseContext:DbContext,IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
       

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries().Where(x=>x.State==EntityState.Modified || x.State==EntityState.Deleted || x.State==EntityState.Added);
            foreach (var entry in modifiedEntries)
            {
                var getEntityType=entry.Context.Model.FindEntityType(entry.GetType());
               
                if (getEntityType != null)
                {
                    //shodow property
                    var insert = getEntityType.FindProperty("InsertTime");
                    var delete = getEntityType.FindProperty("RemoveTime");
                    var isRemove = getEntityType.FindProperty("IsRemove");
                    var update = getEntityType.FindProperty("UpdateTime");

                    if (entry.State == EntityState.Added && insert != null)
                        entry.Property("InsertTime").CurrentValue = DateTime.Now;

                    if (entry.State == EntityState.Deleted && delete != null)
                        entry.Property("RemoveTime").CurrentValue = DateTime.Now;

                    if (entry.State == EntityState.Deleted && delete != null && isRemove != null)
                    {
                        entry.Property("RemoveTime").CurrentValue = DateTime.Now;
                        entry.Property("IsRemove").CurrentValue = true;
                    }

                    if (entry.State == EntityState.Modified && update != null)
                        entry.Property("UpdateTime").CurrentValue = DateTime.Now;
                }

            }
         
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Any())
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("InsertTime");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("UpdateTime");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("RemoveTime");
                    modelBuilder.Entity(entityType.Name).Property<bool>("IsRemove");
                }
            }
            base.OnModelCreating(modelBuilder);
        }

    }
}
