﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class Job
    {
        [Required(ErrorMessage = "Job ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Job Title is Required")]
        [Display(Name = "Job Title")]
        public string? Title { get; set; }
    }
}
