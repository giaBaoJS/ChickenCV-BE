using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCV.Core.Entity;

namespace TopCV.Database
{
	public class ApplicationDbContext : IdentityDbContext<User, Role, int>
	{
		public DbSet<Candidate> Candidates{ get; set; }
		public DbSet<Company> Companies { get; set; }
		public DbSet<CV> CVs { get; set; }
		public DbSet<Job> Jobs { get; set; }
		public DbSet<ApplicationDetail> ApplicationDetails { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{ }

		/*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder
			//.UseLazyLoadingProxies()
			.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TopCV;integrated security=true;Trusted_Connection=True");*/

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
				.HasMany(u => u.UserRoles)
				.WithOne()
				.HasForeignKey(ur => ur.UserId)
				.IsRequired();

			modelBuilder.Entity<Role>()
				.HasMany(r => r.UserRoles)
				.WithOne()
				.HasForeignKey(ur => ur.RoleId)
				.IsRequired();

			// ApplicationDetail has 2 keys
			modelBuilder.Entity<ApplicationDetail>()
				.HasKey(ad => new { ad.CVId, ad.JobId });

			modelBuilder.Entity<ApplicationDetail>()
				.Property(ad => ad.DateCreated)
				.HasDefaultValueSql("getdate()");

			/*modelBuilder.Entity<Candidate>()
				.HasOne(c => c.User)
				.WithOne(u => u.Candidate)
				.HasForeignKey<Candidate>(c => c.UserId)
				.IsRequired();*/

			modelBuilder.Entity<Company>()
				.HasOne(c => c.User)
				.WithOne(u => u.Company)
				.HasForeignKey<Company>(c => c.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<CV>()
				.Property(cv =>cv.DateModified)
				.HasDefaultValueSql("getdate()");

			modelBuilder.Entity<Job>()
				.Property(j => j.DateCreated)
				.HasDefaultValueSql("getdate()");
		}
	}
}
