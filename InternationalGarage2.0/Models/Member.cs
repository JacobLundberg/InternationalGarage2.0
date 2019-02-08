using System.ComponentModel.DataAnnotations;

namespace InternationalGarage2_0.Models
{
    public class Member
    {
        [Key]
        public int Id { set; get; }
        [Required,MaxLength(30),MinLength(3)]
        [RegularExpression(@"^[A-Z][a-zA-Z' ']*$", ErrorMessage = "Invalid name.")]
        [Display(Name ="Member Name")]
        public string Name { get; set; }
    }
}
