using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmployeeUpdateDTO : BaseEntity
    {
        //[Required(ErrorMessage = "Employee ID is required")]
        //[StringLength(8, ErrorMessage = "Employee ID cannot exceed 8 digits")]
        public string? Id { get; set; }

        [Display(Name = "Supervisor")]
        public string? SupervisorId { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Job")]
        [Required(ErrorMessage = "Job is required")]
        public int JobAssignmentId { get; set; }

        [Required(ErrorMessage = "Employment Status is required")]
        [Display(Name = "Employment Status")]
        public int Status { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and contain at least one uppercase letter, one number, and one special character.")]
        public string? Password { get; set; }

        public byte[]? PasswordSalt { get; set; }


        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string? LastName { get; set; }

        [Display(Name = "Middle Initial")]
        public char? MiddleInitial { get; set; }

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

        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Social Insurance Number is required")]
        [Display(Name = "Social Insurance Number")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{3}$", ErrorMessage = "The Social Insurance Number must follow the following format 123-456-789")]
        public string? SIN { get; set; }

        [Required(ErrorMessage = "Seniority Date is required")]
        [Display(Name = "Seniority Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SeniorityDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Job Start Date is required")]
        [Display(Name = "Job Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JobStartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Work Phone Number is required")]
        [Display(Name = "Work Phone")]
        [Phone]
        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$", ErrorMessage = "Phone number must be in the format (123) 456-7890.")]
        public string? WorkPhone { get; set; }

        [Required(ErrorMessage = "Cell Phone Number is required")]
        [Display(Name = "Cell Phone")]
        [Phone]
        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$", ErrorMessage = "Phone number must be in the format (123) 456-7890.")]
        public string? CellPhone { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Office Location is required")]
        [Display(Name = "Office Location")]
        public string? OfficeLocation { get; set; }

        [Display(Name = "Termination Date")]
        public DateTime? TerminationDate { get; set; }

        [Display(Name = "Retirement Date")]
        public DateTime? RetirementDate { get; set; }

        public byte[]? RecordVersion { get; set; }

    }
}
