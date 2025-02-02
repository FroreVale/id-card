using System.ComponentModel.DataAnnotations;

namespace UserAuthAPI.Models
{
    public class IdCardDetailsRequest
    {
        public required string AdmissionNo { get; set; }
        
        public required string ParentName { get; set; }
        
        public required string PhoneNo { get; set; }
    }
}
