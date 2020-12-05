using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DawV2.Models
{
    public class Post
    {
        [Key] public int PostId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        [Required(ErrorMessage = "Continutul postarii este obligatoriu!")]
        public string Content { get; set; }
        public bool IsEdited { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}