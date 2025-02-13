using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.DbModels
{
    public class AssetDb : IAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Ticker { get; set; }

        public string HoldingName { get; set; }

        public double Positions { get; set; }

        public int HoldingPeriod { get; set; }

        public double CurrentPrice { get; set; }

        public double BoughtPrice { get; set; }
    }
}
