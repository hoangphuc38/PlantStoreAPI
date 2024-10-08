﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantStoreAPI.Model
{
    public class Voucher
    {
        [Key, MaxLength(50), Required]
        public string? ID { get; set; }

        [Key, Required]
        public int VoucherTypeId { get; set; }

        [Required, MaxLength(100)]
        public string? Name { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public double Value { get; set; }
        
        [NotMapped]
        public VoucherType? VoucherType { get; set; }
    }
}
