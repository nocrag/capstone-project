using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmployeePersonalInfoDTO : BaseEntity
    {
        [Display(Name = "Employee")]
        [Required(ErrorMessage = "Employee ID is required")]
        [StringLength(8, ErrorMessage = "Employee ID cannot exceed 8 digits")]
        public string? Id { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and contain at least one uppercase letter, one number, and one special character.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Initial")]
        public char? MiddleInitial { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Street Address is required")]
        [Display(Name = "Street Address")]
        public string? StreetAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Province is required")]
        [Display(Name = "Province")]
        public string? Province { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [Display(Name = "Postal Code")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$", ErrorMessage = "Postal Code must be in Canadian format (e.g., A1A 1A1).")]
        public string? PostalCode { get; set; }
    }
}
