using AutoMapper;
using Car_Rental_MVC.Entities;
using Car_Rental_MVC.Exceptions;
using Car_Rental_MVC.Models;
using Car_Rental_MVC.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental_MVC.Repositories
{
    public class UserRepository : Repository<User, UserModelDto>, IUserRepository
    {
        private readonly CarRentalManagerContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _accessor;

        public UserRepository(CarRentalManagerContext context, IMapper mapper,
            IPasswordHasher<User> passwordHasher, IHttpContextAccessor accessor) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _accessor = accessor;
        }
        public async Task RegisterAsync(RegisterModelDto dto)
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
            await AddAsync(newUser);
        }

        public async Task LoginAsync(LoginModelDto dto)
        {
            var user = _context.Users
                .Include(r => r.Role)
                .SingleOrDefault(e => e.Email == dto.Email) ?? throw new NotFoundException("User not found.");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Email}" ),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await _accessor.HttpContext.SignInAsync(claimsPrincipal);
        }

        public async Task LogoutAsync()
        {
            await _accessor.HttpContext.SignOutAsync();
        }

        public async Task UpdateAsync(UserModelDto userDto)
        {
            var user = _context
                .Users
                .SingleOrDefault(u => u.Id == userDto.Id) ?? throw new NotFoundException("User not found");

            if (user.Email != userDto.Email)
            {
                if (!(await GetFirstOrDefaultDtoAsync(x => x.Email == userDto.Email) is null))
                {
                    throw new Exception("This email is taken.");
                }
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.ContactNumber = userDto.ContactNumber;
            user.PostalCode = userDto.PostalCode;
            user.City = userDto.City;
            user.Street = userDto.Street;
            if (userDto.Role != null)
            {
                if (userDto.Role == "User")
                {
                    user.RoleId = 1;
                }

                if (userDto.Role == "Manager")
                {
                    user.RoleId = 2;
                }

                if (userDto.Role == "Admin")
                {
                    user.RoleId = 3;
                }
            }
            if (userDto.NewPassword != null && userDto.ConfirmNewPassword != null)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, userDto.NewPassword);
            }
            _context.Users.Update(user);
            await Task.CompletedTask;
        }

        public async Task<bool> UserNameOrPasswordInvalid(LoginModelDto dto)
        {
            var user = _context.Users.SingleOrDefault(e => e.Email == dto.Email);

            if (user is null)
            {
                return true;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return true;
            }

            return await Task.FromResult(false);
        }

        public async Task<IEnumerable<UserModelDto>> GetAllUsersDto()
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
                var userDto = _mapper.Map<UserModelDto>(user);
                userDto.NumberRentedCars = _context.RentInfo.Where(u => u.UserId == user.Id && u.IsGivenBack == false).Count();

                usersDto.Add(userDto);
            }

            return await Task.FromResult(usersDto);
        }

        public async Task<UserModelDto> GetUserDetails(int userId)
        {
            var user = _context
                .Users
                .Include(r => r.Role)
                .SingleOrDefault(u => u.Id == userId) ?? throw new NotFoundException("User not found");

            var userDto = _mapper.Map<UserModelDto>(user);

            userDto.NumberRentedCars = _context.RentInfo.Where(u => u.UserId == user.Id && u.IsGivenBack == false).Count();

            var rentedCarInfo = _context
                .RentInfo
                .Include(c => c.Car)
                .Where(u => u.UserId == user.Id && u.IsGivenBack == false)
                .ToList();

            var carsDtosList = new List<CarModelDto>();

            if (rentedCarInfo.Any())
            {
                for (int i = 0; i < rentedCarInfo.Count(); i++)
                {
                    var carDto = _mapper.Map<CarModelDto>(rentedCarInfo[i].Car);
                    carsDtosList.Add(carDto);
                }
            }

            userDto.Cars = carsDtosList;

            return await Task.FromResult(userDto);
        }
    }
}
