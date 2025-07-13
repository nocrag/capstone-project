using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class RatingOptions : BaseEntity
    {
        public int Id { get; set; }

        [Display(Name = "Rating Option")]
        public string? RatingOption { get; set; }
    }
}
