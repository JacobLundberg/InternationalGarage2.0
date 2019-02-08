using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public int NumVehicles { get; set; }

        [NotMapped]
        public IList<ParkedVehicle> MemberVehicle { get; set; }
    }
}
