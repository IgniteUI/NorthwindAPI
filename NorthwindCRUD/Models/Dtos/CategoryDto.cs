﻿using System.ComponentModel.DataAnnotations;
﻿using NorthwindCRUD.Models.Contracts;

﻿namespace NorthwindCRUD.Models.Dtos
{
    public class CategoryDto : ICategory
    {
        public int CategoryId { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; }
    }
}