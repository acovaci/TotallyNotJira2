using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TotallyNotJira.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        public ApplicationUser [] User { get; set; }
        
        public string TeamName { get; set; }

        public string ProjectId { get; set; }
    }
}