using System.ComponentModel;

namespace NorthwindCRUD
{
    public class ValidationError
    {
        public ValidationError() 
        {
            string dataField;
            string message = "No! No! Your input does not pass validation!";
        }

        public string DataField { get; set; }

        public string Message { get; set; }
    }
}
