using Challenge.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Challenge.Infrastructure.Data;

public class ChatDbContext: DbContext
{
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetToStringConverter>(); // SqlLite workaround for DateTimeOffset sorting
    }
}