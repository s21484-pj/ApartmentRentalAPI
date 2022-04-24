namespace ApartmentRental.Infrastracture.Entities;

public class Address : BaseEntity
{
    public string Street { get; set; }
    public string? FlatNumber { get; set; }
    public string? BuildingNumber { get; set; }
    public string City { get; set; }
    public string Postcode { get; set; }
    public string Country { get; set; }
}