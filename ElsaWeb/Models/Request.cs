using System.ComponentModel.DataAnnotations;

namespace ElsaWeb.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Status { get; set; } = "Created";

        // This property will be used to store the decision
        public bool IsApproved { get; set; }
    }
}
