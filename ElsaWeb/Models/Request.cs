using System.ComponentModel.DataAnnotations;

namespace ElsaWeb.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Updated to use the RequestStatus enum
        public RequestStatus Status { get; set; } = RequestStatus.Submitted;

        // This property will be used to store the decision
        public bool IsApproved { get; set; }
    }
}
