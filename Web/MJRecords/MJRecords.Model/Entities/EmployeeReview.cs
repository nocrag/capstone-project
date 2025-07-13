using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmployeeReview : BaseEntity
    {
        //[Required(ErrorMessage = "Review ID is required")]
        public int Id { get; set; }

        [Display(Name = "Employee")]
        [Required(ErrorMessage = "Employee is Required to make a review")]
        public string? EmployeeId { get; set; }

        [Display(Name = "Rating Option")]
        [Required(ErrorMessage = "Rating option is required to make a review")]
        public int RatingOptionsId { get; set; }

        [Required(ErrorMessage = "A comment is required to make a review")]
        public string? Comment { get; set; }

        [Display(Name = "Review Date")]
        [Required(ErrorMessage = "A date is required for this review")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        [Display(Name = "Year Quarter")]
        public string? YearQuarter { get; set; }
    }
}
