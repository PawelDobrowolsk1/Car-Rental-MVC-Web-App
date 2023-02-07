using AutoMapper;
using Car_Rental_MVC.Entities;
using Car_Rental_MVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Car_Rental_MVC.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarRentalManagerContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public UserRepository(CarRentalManagerContext context, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public bool EmailInUse(RegisterModelDto dto)
        {
            var user = _context.Users.Any(u => u.Email == dto.Email);

            if (user)
            {
                return true;
            }

            return false;
        }

        public ClaimsPrincipal GenerateClaimsPrincipal(LoginModelDto dto)
        {
            var user = _context.Users
                .Include(r => r.Role)
                .FirstOrDefault(e => e.Email == dto.Email);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Email}" ),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        public IEnumerable<UserModelDto> GetAllUsers()
        {
            var users = _context.Users
                .Include(r => r.Role)
                .ToList();

            if (!users.Any())
            {
                return null;
            }
            var usersDto = new List<UserModelDto>();
            foreach (var user in users)
            {
                var userDto = new UserModelDto()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    NumberRentedCars = _context.RentInfo.Where(u => u.Id == user.Id).Count(),
                    Role = user.Role.Name
                };
                usersDto.Add(userDto);
            }

            return usersDto;
        }

        public void RegisterUser(RegisterModelDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;
            _context.Add(newUser);
            _context.SaveChanges();
        }

        public bool UserNameOrPasswordInvalid(LoginModelDto dto)
        {
            var user = _context.Users
                .Include(r => r.Role)
                .FirstOrDefault(e => e.Email == dto.Email);

            if (user is null)
            {
                return true;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return true;
            }

            return false;
        }
    }
}
