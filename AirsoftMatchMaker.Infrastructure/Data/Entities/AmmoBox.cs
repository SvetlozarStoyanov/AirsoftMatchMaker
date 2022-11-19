using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftMatchMaker.Infrastructure.Data.Entities
{
    public class AmmoBox
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(Entities.Vendor.Id))]
        public int? VendorId { get; set; }
        public virtual Vendor? Vendor { get; set; }
    }
}
