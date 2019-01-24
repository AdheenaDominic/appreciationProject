using System;
using System.ComponentModel.DataAnnotations;

namespace AppreciationCards.Models
{
    public partial class Messages
    {
        public int MessageId { get; set; }

        public bool Unread { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Content { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Recipient")]
        public string ToName { get; set; }

        [StringLength(100)]
        [Display(Name = "Sender")]
        public string FromName { get; set; }

        [Display(Name = "Message Date")]
        public DateTime MessageDate { get; set; }

        [Display(Name = "Xero Values")]
        public int ValueId { get; set; }

        public string Value { get; set; }
    }
}

