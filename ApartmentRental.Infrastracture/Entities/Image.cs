using System.Collections;

namespace ApartmentRental.Infrastracture.Entities;

public class Image : BaseEntity
{
    public byte[] Data { get; set; }

    public int ApartmentId { get; set; }
    public Apartment Apartment { get; set; }
    
}