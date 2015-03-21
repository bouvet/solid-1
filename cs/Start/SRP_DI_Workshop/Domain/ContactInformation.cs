using System;

namespace SRP_DI_Workshop.Domain
{
    [Serializable]
    public class ContactInformation
    {
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool AllowNotificationBySms { get; set; }
    }
}