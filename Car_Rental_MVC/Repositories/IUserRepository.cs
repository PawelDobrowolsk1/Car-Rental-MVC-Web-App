using Car_Rental_MVC.Models;
using System.Security.Claims;

namespace Car_Rental_MVC.Repositories
{
    public interface IUserRepository
    {
        bool UserNameOrPasswordInvalid(LoginModelDto dto);
        ClaimsPrincipal GenerateClaimsPrincipal(LoginModelDto dto);
        void RegisterUser(RegisterModelDto dto);
        bool EmailInUse(RegisterModelDto dto);
        IEnumerable<UserModelDto> GetAllUsers();
        UserModelDto GetUserInfoDetails(string email);
        void SaveEditedUserProfile(UserModelDto userDto);
    }
}
