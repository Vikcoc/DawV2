using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DawV2.Models
{
    public class Comment
    {
        [Key] public int CommentId { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        [Required(ErrorMessage = "Continutul comentariului este obligatoriu!")]
        public string Content { get; set; }
        public bool IdEdited { get; set; }

        public virtual Post Post { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}