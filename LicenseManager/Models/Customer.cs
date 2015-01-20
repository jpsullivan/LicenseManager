using System;

namespace LicenseManager.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BillingContact { get; set; }
        public string BillingContactEmail { get; set; }
        public string TechnicalContact { get; set; }
        public string TechnicalContactEmail { get; set; }
        public bool IsHosted { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}