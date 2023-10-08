using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;
using System;
using System.Globalization;

namespace NorthwindCRUD.Services
{
    public class SalesService
    {
        private readonly DataContext dataContext;

        public SalesService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public SalesDto[] GetSalesDataByCategoryAndYear(string categoryName, int? orderYear)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentException("Category name is required.", nameof(categoryName));
            }
            var salesData = this.dataContext.OrderDetails
                .Include(o => o.Order)
                .Include(od => od.Product)
                .Include(op => op.Product.Category)
            .ToList();

            var filteredData = salesData
                .Where(od => od.Product.Category.Name == categoryName)
            .ToList();

            if (orderYear != null)
            {
                filteredData = salesData
                .Where(od => od.Product.Category.Name == categoryName &&
                            ConvertToOrderDate(od.Order.OrderDate).Year == orderYear)
                .ToList();
            }

            var sales = filteredData.Select(od => new SalesDto
            {
                ProductId = od.Product.ProductId,
                QuantitySold = od.Quantity,
                SaleAmount = od.Quantity * od.UnitPrice
            }).ToArray();

            return sales;
        }

        public SalesDto[] RetrieveSalesDataByCountry(string startDate, string endDate, string country)
        {

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                throw new ArgumentException("startDate and endDate are required.");
            }

            if (!DateTime.TryParse(startDate, out DateTime parsedStartDate) || !DateTime.TryParse(endDate, out DateTime parsedEndDate))
            {
                throw new ArgumentException("Invalid date format for startDate or endDate.");
            }
            string normalizedCountry = country.ToLower();

            var salesData = this.dataContext.OrderDetails
                .Include(o => o.Order)
                .Include(c => c.Order.ShipAddress)
                .Include(oc => oc.Order.Customer)
                .Include(od => od.Product)
                .Include(op => op.Product.Category)
            .ToList();

            var filteredData = salesData
            .Where(od => od.Order.ShipAddress.Country.ToLower() == normalizedCountry && DateTime.Parse(od.Order.OrderDate) >= parsedStartDate && DateTime.Parse(od.Order.RequiredDate) <= parsedEndDate)
            .ToList();

            var sales = filteredData.Select(od => new SalesDto
            {
                ProductId = od.Product.ProductId,
                QuantitySold = od.Quantity,
                SaleAmount = od.Quantity * od.UnitPrice
            }).ToArray();

            return sales;
        }

        public SalesDto[] RetrieveSalesDataByYear(int year, int? startMonth, int? endMonth)
        {
            if (year == 0)
            {
               throw new ArgumentException("Year is required.", nameof(year));
            }
            var salesData = this.dataContext.OrderDetails
                .Include(o => o.Order)
                .Include(od => od.Product)
            .ToList();

            var filteredData = salesData
            .Where(od => ConvertToOrderDate(od.Order.OrderDate).Year == year)
            .ToList();


            if (startMonth != 0)
            {
                filteredData = filteredData.Where(od =>
                    ConvertToOrderDate(od.Order.OrderDate).Month >= startMonth).ToList();
            }

            if (endMonth != 0)
            {
                filteredData = filteredData.Where(od =>
                    ConvertToOrderDate(od.Order.OrderDate).Month <= endMonth).ToList();
            }

            var sales = filteredData.Select(od => new SalesDto
            {
                ProductId = od.Product.ProductId,
                QuantitySold = od.Quantity,
                SaleAmount = od.Quantity * od.UnitPrice
            }).ToArray();

            return sales;
        }

        private DateTime ConvertToOrderDate(string dateString)
        {
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            throw new ArgumentException("Invalid date format");
        }
    }
}
