﻿using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthwindCRUD.Models.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NorthwindCRUD.Services
{
    public interface IPagingService
    {
        PagedResultDto<TDto> FetchPagedData<TEntity, TDto>(
            IQueryable<TEntity> query,
            int? skip = null,
            int? top = null,
            int? pageIndex = null,
            int? size = null,
            string? orderBy = null);
    }

    public class PagingService : IPagingService
    {
        private readonly IMapper mapper;

        public PagingService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public PagedResultDto<TDto> FetchPagedData<TEntity, TDto>(
            IQueryable<TEntity> query,
            int? skip = null,
            int? top = null,
            int? pageIndex = null,
            int? size = null,
            string? orderBy = null)
        {
            // Determine if we're using skip/top or page/size
            bool isPageSize = pageIndex.HasValue || size.HasValue;

            int totalRecords = query.Count();

            // Default values if not provided
            int skipRecordsAmount = skip ?? 0;
            int currentSize = top ?? totalRecords;

            if (isPageSize)
            {
                int pageNumber = pageIndex ?? 0;
                int pageSize = size ?? totalRecords;
                skipRecordsAmount = pageNumber * pageSize;
                currentSize = pageSize;
            }

            // Apply ordering if specified
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = ApplyOrdering(query, orderBy);
            }

            // Apply pagination
            var pagedData = query
                .Skip(skipRecordsAmount)
                .Take(currentSize)
                .ToArray();

            // Calculate total pages
            int totalPages = (int)Math.Ceiling(totalRecords / (double)currentSize);

            // Map the results to TDto
            var pagedDataDtos = mapper.Map<TDto[]>(pagedData);

            return new PagedResultDto<TDto>
            {
                Items = pagedDataDtos,
                TotalRecordsCount = totalRecords,
                PageSize = currentSize,
                PageNumber = isPageSize ? ((skipRecordsAmount / currentSize) + 1) : ((skipRecordsAmount / currentSize) + 1),
                TotalPages = totalPages,
            };
        }

        private IQueryable<TEntity> ApplyOrdering<TEntity>(IQueryable<TEntity> source, string orderBy)
        {
            var orderParams = orderBy.Split(',');

            IOrderedQueryable<TEntity>? orderedQuery = null;

            foreach (var param in orderParams)
            {
                var trimmedParam = param.Trim();
                var orderByParts = trimmedParam.Split(' ');
                var field = orderByParts[0];
                var order = orderByParts.Length > 1 ? orderByParts[1] : "asc";

                var propertyInfo = typeof(TEntity).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "x");
                    var property = Expression.Property(parameter, propertyInfo);
                    var lambda = Expression.Lambda(property, parameter);

                    var methodName = order.ToUpper(CultureInfo.InvariantCulture) == "DESC"
                        ? (orderedQuery == null ? "OrderByDescending" : "ThenByDescending")
                        : (orderedQuery == null ? "OrderBy" : "ThenBy");

                    var resultExpression = Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new Type[] { source.ElementType, property.Type },
                        (orderedQuery ?? source).Expression,
                        Expression.Quote(lambda));

                    orderedQuery = (IOrderedQueryable<TEntity>)(orderedQuery == null
                        ? source.Provider.CreateQuery<TEntity>(resultExpression)
                        : orderedQuery.Provider.CreateQuery<TEntity>(resultExpression));
                }
            }

            return orderedQuery ?? source;
        }
    }
}
