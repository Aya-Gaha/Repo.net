namespace AspCoreFirstApp.Models;

public class PanierParUser
{
    public Guid Id { get; set; }
    public string UserID { get; set; } = string.Empty;
    public Guid ProduitId { get; set; }
    public List<Produit>? produits { get; set; }
}
