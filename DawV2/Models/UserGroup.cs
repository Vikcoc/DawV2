using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DawV2.Models
{
    public class UserGroup
    {
        [Key] [Column(Order = 0)] public string ApplicationUserId { get; set; }

        [Key] [Column(Order = 1)] public int GroupId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Group Group { get; set; }
    }
}