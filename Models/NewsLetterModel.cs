using System.ComponentModel.DataAnnotations;

namespace WebApplicationShimi.Models
{
    public class NewsLetterModel
    {
        [EmailAddress]
        [Required]
        [Display(Name = "Enter a email address")]
        public string EmailAddress { get; set; }
    }
}