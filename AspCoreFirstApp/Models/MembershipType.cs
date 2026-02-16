using System.ComponentModel.DataAnnotations;

namespace AspCoreFirstApp.Models;

public class MembershipType
{
    public int Id { get; set; }

    public decimal SignUpFee { get; set; }

    public int DurationInMonth { get; set; }

    public int DiscountRate { get; set; }

    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
