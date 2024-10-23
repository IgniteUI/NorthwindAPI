using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NorthwindCRUD.Helpers;
using NorthwindCRUD.Models.DbModels;
using NorthwindCRUD.Models.Dtos;

namespace NorthwindCRUD.Services
{
    public abstract class BaseDbService<TDto, TDb, TId>
            where TDto : class, IBaseDto
            where TDb : class, IBaseDb, new()
    {
        protected readonly DataContext dataContext;
        protected readonly IMapper mapper;
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

        protected TDb GetDbById(TId id)
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
                throw new InvalidOperationException("No key property found on entity");
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

        public async Task<TDto> Update(TDto model)
        {
            var keyProperty = typeof(TDto).GetProperties()
                .FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));

            if (keyProperty == null)
            {
                throw new InvalidOperationException("No key property found on entity");
            }

            var keyValue = (TId)keyProperty.GetValue(model);

            return await Update(model, keyValue);
        }

        public async Task<TDto> Update(TDto model, TId id)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var dbModel = mapper.Map<TDb>(model);

            var existingEntity = GetDbById(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }

            //todo improve
            mapper.Map(dbModel, existingEntity, opts => opts.Items["IsPatch"] = true);

            await dataContext.SaveChangesAsync();
            var mappedResult = mapper.Map<TDto>(existingEntity);
            return mappedResult;
        }

        public async Task<TDto> Create(TDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var dbModel = mapper.Map<TDb>(model);

            var keyProperty = typeof(TDb).GetProperties()
                .FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));

            if (keyProperty != null && typeof(TId) == typeof(int))
            {
                keyProperty.SetValue(dbModel, 0);
            }

            PropertyHelper<TDb>.MakePropertiesEmptyIfNull(dbModel);
            var addedEntity = await dataContext.Set<TDb>().AddAsync(dbModel);
            await dataContext.SaveChangesAsync();
            var mappedResult = mapper.Map<TDto>(dbModel);
            return mappedResult;
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
