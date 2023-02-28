namespace Car_Rental_MVC.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        ICarRepository Car { get; }
        Task SaveAsync();
    }
}
