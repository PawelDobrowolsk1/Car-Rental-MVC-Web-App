using AutoMapper;
using Car_Rental_MVC.Entities;
using Car_Rental_MVC.Repositories.IRepositories;

namespace Car_Rental_MVC.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarRentalManagerContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(CarRentalManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            Car = new CarRepository2(_context, _mapper);
        }
        public ICarRepository2 Car { get; private set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
