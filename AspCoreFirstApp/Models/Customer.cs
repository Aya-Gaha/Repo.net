using System.ComponentModel.DataAnnotations;

namespace AspCoreFirstApp.Models;

public class Customer
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    // Foreign Key
    public int MembershipTypeId { get; set; }

    // Navigation property (nullable to avoid model binding validation errors)
    public MembershipType? MembershipType { get; set; }
}