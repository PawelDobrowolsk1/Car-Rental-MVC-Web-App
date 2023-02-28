using AutoMapper;
using Car_Rental_MVC.Entities;
using Car_Rental_MVC.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace Car_Rental_MVC.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarRentalManagerContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _accessor;

        public UnitOfWork(CarRentalManagerContext context, IMapper mapper , 
            IPasswordHasher<User> passwordHasher, IHttpContextAccessor accessor)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _accessor = accessor;
            Car = new CarRepository(_context, _mapper);
            User = new UserRepository(_context, _mapper, _passwordHasher, _accessor);
        }
        public ICarRepository Car { get; private set; }
        public IUserRepository User { get; private set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
