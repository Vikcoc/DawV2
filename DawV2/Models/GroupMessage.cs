using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DawV2.Models
{
    public class GroupMessage
    {
        [Key] public int MessageId { get; set; }

        public int GroupId { get; set; }
        public string ApplicationUserId { get; set; }
        public string Message { get; set; }
        public bool IsEdited { get; set; }

        public virtual Group Group { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}