using System.Runtime.Serialization;

namespace NorthwindCRUD.Helpers
{
    public class Enums
    {
        public enum Shipping
        {
            [EnumMember(Value = "SeaFreight")]
            SeaFreight,

            [EnumMember(Value = "GroundTransport")]
            GroundTransport,

            [EnumMember(Value = "AirCargo")]
            AirCargo,
        }
    }
}
