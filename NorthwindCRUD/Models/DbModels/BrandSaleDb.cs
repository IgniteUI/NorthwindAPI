using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class BrandSaleDb : IBrandSale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Store { get; set; }

        public string Brand { get; set; }

        public string Country { get; set; }

        public double Cost { get; set; }

        [JsonProperty("Date")]
        public string? SaleDate { get; set; }

        public double Sale { get; set; }
    }
}
