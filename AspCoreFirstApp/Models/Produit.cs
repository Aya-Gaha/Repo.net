namespace AspCoreFirstApp.Models;

public class Produit
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Prix { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
}
