using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace belt.Models
{
    public class activity{

        public activity(){
            activityparticpants = new List<participants>();
        }
        [Key]
        public int id{get;set;}
        public int usersid{get;set;}
        public string name{get;set;}
        public DateTime date{get;set;}
        public TimeSpan time{get;set;}
        public int duration{get;set;}
        public string timetype{get;set;}
        public string coordinator{get;set;}
        public int participant{get;set;}
        public string description{get;set;}
        public DateTime created_at{get;set;}
        public DateTime updated_at{get;set;}
        public List<participants> activityparticpants{get;set;}

    }
}