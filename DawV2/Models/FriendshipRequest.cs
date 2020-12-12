using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DawV2.Models
{
    public class FriendshipRequest
    {
        [Key]
        public int FriendshipRequestId { get; set; }
        //[Key]
        //[Column(Order = 0)]
        [ForeignKey("Requester")]
        public string RequesterId { get; set; }

        //[Key]
        //[Column(Order = 1)]
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        public string Message { get; set; }
        public bool IsSeen { get; set; }
        public bool IsAccepted { get; set; }

        public virtual ApplicationUser Requester { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
    }
}