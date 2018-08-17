using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace belt.Models
{
    public class participants{
    
    [Key]
    public int id{get;set;}
    public int usersid{get;set;}
    public int activityid{get;set;}
    public activity activity{get;set;}
    public users user{get;set;}
    }
}