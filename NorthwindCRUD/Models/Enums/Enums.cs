using System.Runtime.Serialization;

namespace NorthwindCRUD.Models.Enums
{
    public enum Shipping
    {
        [EnumMember(Value = "SeaFreight")]
        SeaFreight,

        [EnumMember(Value = "GroundTransport")]
        GroundTransport,

        [EnumMember(Value = "AirCargo")]
        AirCargo,

        [EnumMember(Value = "Mail")]
        Mail,
    }
}
