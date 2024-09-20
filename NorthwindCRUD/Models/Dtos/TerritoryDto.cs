﻿using NorthwindCRUD.Models.Contracts;

namespace NorthwindCRUD.Models.Dtos
{
    public class TerritoryDto : IBaseDto, ITerritory
    {
        public string TerritoryId { get; set; }

        public string TerritoryDescription { get; set; }

        public int? RegionId { get; set; }
    }
}
