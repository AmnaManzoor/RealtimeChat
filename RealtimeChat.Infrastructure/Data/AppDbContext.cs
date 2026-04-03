using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealtimeChat.Domain.Entities;

namespace RealtimeChat.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for the chat application.
/// </summary>
public class AppDbContext : IdentityDbContext<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">Database context options.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the messages dataset.
    /// </summary>
    public DbSet<Message> Messages { get; set; }

    /// <summary>
    /// Gets or sets the chat rooms dataset.
    /// </summary>
    public DbSet<ChatRoom> ChatRooms { get; set; }

    /// <summary>
    /// Gets or sets the chat room members dataset.
    /// </summary>
    public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }

    /// <summary>
    /// Gets or sets the message reactions dataset.
    /// </summary>
    public DbSet<MessageReaction> MessageReactions { get; set; }

    /// <summary>
    /// Gets or sets the refresh tokens dataset.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(m => m.Room)
            .WithMany()
            .HasForeignKey(m => m.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ChatRoom>()
            .HasIndex(r => r.Name)
            .IsUnique();

        builder.Entity<ChatRoomMember>()
            .HasIndex(m => new { m.RoomId, m.UserId })
            .IsUnique();

        builder.Entity<ChatRoomMember>()
            .HasOne(m => m.Room)
            .WithMany(r => r.Members)
            .HasForeignKey(m => m.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ChatRoomMember>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<MessageReaction>()
            .HasOne(r => r.Message)
            .WithMany()
            .HasForeignKey(r => r.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<MessageReaction>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RefreshToken>()
            .HasIndex(rt => rt.TokenHash)
            .IsUnique();
    }
}
