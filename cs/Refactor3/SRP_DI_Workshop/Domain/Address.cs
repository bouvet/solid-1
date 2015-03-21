using System;

namespace SRP_DI_Workshop.Domain
{
    [Serializable]
    public class Address
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string Apartment { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
