namespace ApartmentRental.Infrastracture.Entities;

public class Account : BaseEntity
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; }
}