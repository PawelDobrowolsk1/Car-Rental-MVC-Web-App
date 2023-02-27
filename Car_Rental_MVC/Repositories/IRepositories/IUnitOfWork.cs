namespace Car_Rental_MVC.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        ICarRepository2 Car { get; }
        Task SaveAsync();
    }
}
