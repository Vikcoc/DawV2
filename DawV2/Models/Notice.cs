using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DawV2.Models
{
    public class Notice
    {
        [Key] public int NoticeId { get; set; }

        public string ApplicationUserId { get; set; }
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        public bool Seen { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}