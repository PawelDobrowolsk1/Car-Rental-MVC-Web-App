using AutoMapper;
using Car_Rental_MVC.Entities;
using Car_Rental_MVC.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Car_Rental_MVC.Repositories
{
    public class Repository<T, Dto> : IRepository<T, Dto> where T : class
                                                          where Dto : class
    {
        private readonly CarRentalManagerContext _context;
        private readonly IMapper _mapper;
        internal DbSet<T> dbSet;

        public Repository(CarRentalManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            dbSet = _context.Set<T>();
        }
        public async Task<Dto> GetFirstOrDefaultDtoAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach(var property in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            var queryDto = _mapper.Map<Dto>(query.FirstOrDefault());

            return await Task.FromResult(queryDto);
        }

        public async Task<IEnumerable<Dto>> GetAllDtoAsync()
        {
            IQueryable<T> query = dbSet;

            var queryDto = _mapper.Map<IEnumerable<Dto>>(query);

            return await Task.FromResult(queryDto.ToList());
        }

        public async Task AddAsync(T entity)
        {
            dbSet.Add(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await Task.CompletedTask;
        }
    }
}
