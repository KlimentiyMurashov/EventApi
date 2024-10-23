using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<User>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<RefreshToken> RefreshTokens { get; set; }
	public DbSet<Event> Events { get; set; }
	public DbSet<Participant> Participants { get; set; }
	public DbSet<EventRegistration> EventRegistrations { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<EventRegistration>()
			.HasKey(er => new { er.ParticipantId, er.EventId });

		modelBuilder.Entity<EventRegistration>()
			.HasOne(er => er.Participant)
			.WithMany(p => p.EventRegistrations)
			.HasForeignKey(er => er.ParticipantId);

		modelBuilder.Entity<EventRegistration>()
			.HasOne(er => er.Event)
			.WithMany(e => e.EventRegistrations)
			.HasForeignKey(er => er.EventId);

		modelBuilder.Entity<RefreshToken>()
			.HasKey(rt => rt.Id);

		modelBuilder.Entity<RefreshToken>()
			.HasIndex(rt => rt.Token)
			.IsUnique();
	}
}
