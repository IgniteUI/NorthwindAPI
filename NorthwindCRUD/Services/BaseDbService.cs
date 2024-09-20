using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Services
{
    public abstract class BaseDbService<TDto, TDb, TId>
        where TDto : class, IBaseDto
        where TDb : class, IBaseDb, new()
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;
        private readonly IPagingService pagingService;

        public BaseDbService(DataContext dataContext, IMapper mapper, IPagingService pagingService)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
            this.pagingService = pagingService;
        }

        public TDto[] GetAll()
        {
            TDb dtoInstance = new TDb();

            IQueryable<TDb> query = this.dataContext.Set<TDb>();
            foreach (var include in dtoInstance.GetIncludes())
            {
                query = query.Include(include);
            }

            var dbResult = query.ToArray();
            return mapper.Map<TDto[]>(dbResult);
        }

        public IQueryable<TDb> GetAllAsQueryable()
        {
            IQueryable<TDb> query = this.dataContext.Set<TDb>();
            return query;
        }

        public TDto? GetById(TId id)
        {
            var dbResult = GetDbById(id);

            return mapper.Map<TDto>(dbResult);
        }

        public TDto? Delete(TId id)
        {
            var dbResult = GetDbById(id);
            if (dbResult == null)
            {
                return null;
            }

            this.dataContext.Remove(dbResult);
            this.dataContext.SaveChanges();
            return mapper.Map<TDto>(dbResult);
        }

        private TDb GetDbById(TId id)
        {
            TDb dtoInstance = new TDb();

            IQueryable<TDb> query = this.dataContext.Set<TDb>();
            foreach (var include in dtoInstance.GetIncludes())
            {
                query = query.Include(include);
            }

            var keyProperty = typeof(TDb).GetProperties()
                .FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));

            if (keyProperty == null)
            {
                throw new Exception("No key property found on entity");
            }

            TDb? dbResult = query.FirstOrDefault(entity =>
                EF.Property<TId>(entity, keyProperty.Name).Equals(id));

            return dbResult;
        }

        public PagedResultDto<TDto> GetWithPageSkip(int? skip = null, int? top = null, int? pageIndex = null, int? size = null, string? orderBy = null)
        {
            var query = GetAllAsQueryable();

            var pagedResult = pagingService.FetchPagedData<TDb, TDto>(query, skip, top, pageIndex, size, orderBy);

            return pagedResult;
        }

        public async Task<TDto> Upsert(TDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Find the key property using reflection (assumes you have a KeyAttribute defined)
            var keyProperty = typeof(TDb).GetProperties()
                .FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));

            if (keyProperty == null)
            {
                throw new InvalidOperationException($"Entity {typeof(TDb).Name} has no key property defined.");
            }

            // Assuming your DTO has a property named "Id" that corresponds to the primary key
            var dtoId = (int?)keyProperty.GetValue(model);

            if (model == null || dtoId == 0)
            {
                // New entity (insert)
                var newEntity = mapper.Map<TDb>(model);
                await dataContext.Set<TDb>().AddAsync(newEntity);
                await dataContext.SaveChangesAsync();
                var mappedResult = mapper.Map<TDto>(newEntity);
                return mappedResult;
            }
            else
            {
                var existingEntity = await dataContext.Set<TDto>().FindAsync(dtoId);
                if (existingEntity == null)
                {
                    throw new Exception($"Entity with id {dtoId} not found.");
                }

                mapper.Map(model, existingEntity, opts => opts.Items["IsPatch"] = true);
                await dataContext.SaveChangesAsync();
                var mappedResult = mapper.Map<TDto>(existingEntity);
                return mappedResult;
            }
        }

        public CountResultDto GetCount()
        {
            return new CountResultDto()
            {
                Count = this.dataContext.Set<TDb>().Count(),
            };
        }
    }
}
