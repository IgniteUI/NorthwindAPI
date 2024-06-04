using System.Globalization;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Services
{
    public interface IPagingService
    {
        PagedResultDto<TDto> GetPagedData<TEntity, TDto>(IEnumerable<TEntity> data, int? skip, int? top, string? orderBy);
    }

    public class PagingService : IPagingService
    {
        private readonly IMapper mapper;

        public PagingService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public PagedResultDto<TDto> GetPagedData<TEntity, TDto>(IEnumerable<TEntity> data, int? skip, int? top, string? orderBy)
        {
            var dataArray = data.ToArray();
            var totalRecords = dataArray.Length;

            // Default skip and top if not provided
            int skipRecordsAmount = skip ?? 0;
            int currentSize = top ?? totalRecords;

            // Apply ordering if specified
            if (!string.IsNullOrEmpty(orderBy))
            {
                var orderByParts = orderBy.Split(' ');
                var field = orderByParts[0];
                var order = orderByParts.Length > 1 ? orderByParts[1] : "ASC";

                var propertyInfo = typeof(TEntity).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    dataArray = order.ToUpper(CultureInfo.InvariantCulture) == "DESC"
                        ? dataArray.OrderByDescending(e => propertyInfo.GetValue(e, null)).ToArray()
                        : dataArray.OrderBy(e => propertyInfo.GetValue(e, null)).ToArray();
                }
            }

            // Apply pagination
            var pagedData = dataArray
                .Skip(skipRecordsAmount)
                .Take(currentSize)
                .ToArray();

            // Calculate total pages
            int totalPages = (int)Math.Ceiling(totalRecords / (double)currentSize);

            // Map the results to ProductDto
            var pagedDataDtos = mapper.Map<TDto[]>(pagedData);

            return new PagedResultDto<TDto>
            {
                Items = pagedDataDtos,
                TotalRecordsCount = totalRecords,
                PageSize = currentSize,
                PageNumber = (skipRecordsAmount / currentSize) + 1,
                TotalPages = totalPages,
            };
        }
    }
}
