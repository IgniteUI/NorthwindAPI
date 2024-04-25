using System.ComponentModel;

namespace NorthwindCRUD.Models.Errors
{
    public class ValidationError : CustomError
    {
        public ValidationError()
        {
        }

        public string DataField { get; set; }
    }
}
