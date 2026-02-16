using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AspCoreFirstApp.Models.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AddAuditLogs(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AddAuditLogs(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void AddAuditLogs(DbContext? context)
    {
        if (context is null) return;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Where(e => e.Entity is not AuditLog)
            .ToList();

        if (entries.Count == 0) return;

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            var audit = new AuditLog
            {
                TableName = entry.Metadata.GetTableName() ?? entry.Metadata.ClrType.Name,
                Action = entry.State.ToString(),
                EntityKey = GetPrimaryKey(entry),
                Changes = SerializeChanges(entry),
                Date = now
            };

            context.Set<AuditLog>().Add(audit);
        }
    }

    private static string GetPrimaryKey(EntityEntry entry)
    {
        var pk = entry.Metadata.FindPrimaryKey();
        if (pk is null) return string.Empty;

        var parts = pk.Properties
            .Select(p => $"{p.Name}={entry.Property(p.Name).CurrentValue}");

        return string.Join(";", parts);
    }

    private static string? SerializeChanges(EntityEntry entry)
    {
        try
        {
            var changes = new Dictionary<string, object?>();

            foreach (var prop in entry.Properties)
            {
                if (prop.Metadata.IsPrimaryKey()) continue;

                if (entry.State == EntityState.Added)
                {
                    changes[prop.Metadata.Name] = new { New = prop.CurrentValue };
                }
                else if (entry.State == EntityState.Deleted)
                {
                    changes[prop.Metadata.Name] = new { Old = prop.OriginalValue };
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (!prop.IsModified) continue;
                    changes[prop.Metadata.Name] = new { Old = prop.OriginalValue, New = prop.CurrentValue };
                }
            }

            return changes.Count == 0 ? null : JsonSerializer.Serialize(changes, JsonOptions);
        }
        catch
        {
            return null;
        }
    }
}
