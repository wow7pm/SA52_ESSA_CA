using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Models
{
    public class ShoppingCart
    {
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string GuestId { get; set; }

        [MaxLength(36)]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [MaxLength(50)]
        public string ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
   
        [Range(1,1000)]
        public int Count { get; set; }

        [NotMapped]
        public double price { get; set; }


    }
}