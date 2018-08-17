using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace belt.Models
{
    public class users{

        public users(){
            Userparticipant = new List<participants>();
            Useractivity = new List<activity>();

        }
        
        [Key]
        public int id{get;set;}
        

        [Required]
        public string first_name{get; set;}

        [Required]
        public string last_name{get; set;}


        [Required]
        public string email {get; set;}

        [Required]
        public string password {get; set;}

        public List<participants> Userparticipant{get;set;}
        public List<activity> Useractivity{get;set;}

        public DateTime created_at{get;set;}
        public DateTime updated_at{get; set;}

    }
}