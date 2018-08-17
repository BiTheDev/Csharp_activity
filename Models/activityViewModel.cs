using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace belt.Models
{
    public class activityViewModel{


        [Required]
        [MinLength(2)]
        public string name{get;set;}

        [Required]
        public DateTime date{get;set;}
        [Required]
        public TimeSpan Time{get;set;}

        [Required]
        public int duration{get;set;}

        [Required]

        public string timetype{get;set;}

        [Required]
        [MinLength(10)]
        public string description{get;set;}
    }
}