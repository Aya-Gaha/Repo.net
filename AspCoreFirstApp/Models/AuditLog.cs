namespace AspCoreFirstApp.Models;

public class AuditLog
{
    public DateTime Date { get; set; }

    public string? Changes { get; set; }

    public string EntityKey { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string TableName { get; set; } = string.Empty;

    public int Id { get; set; }
}


