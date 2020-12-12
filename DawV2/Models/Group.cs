using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DawV2.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string GroupName { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<GroupMessage> GroupMessages { get; set; }
    }
}