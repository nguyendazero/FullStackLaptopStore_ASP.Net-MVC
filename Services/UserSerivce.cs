using Microsoft.EntityFrameworkCore;
using DotNet.LaptopStore.Models;
using System.Collections.Generic;
using System.Linq;
using DotNet.LaptopStore.Data;

namespace DotNet.LaptopStore.Services
{
    public interface IUserService
    {
        List<User> GetAllUsers(string? keySearch = null);
        User? GetUserById(int id);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        public User? GetUserByUsername(string username);
        User? GetUserByUsernameAndPassword(string username, string password);
        Task<User?> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
    }

    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;

        public UserService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public User? GetUserByUsername(string username)
        {
            return _dataContext.Users.FirstOrDefault(u => u.UserName == username);
        }

        public User? GetUserByUsernameAndPassword(string username, string password)
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user == null)
            {
                Console.WriteLine("No user found with the provided username and password");
            }
            return user;
        }


        public List<User> GetAllUsers(string? keySearch = null)
        {
            return _dataContext.Users
                .Include(u => u.Cart)
                .Include(u => u.LikeRatings)
                .Include(u => u.Orders)
                .Include(u => u.Ratings)
                .Where(u => string.IsNullOrEmpty(keySearch) || u.FullName.Contains(keySearch) || u.Email.Contains(keySearch))
                .ToList();
        }

        public User? GetUserById(int id)
        {
            return _dataContext.Users
                .Include(u => u.Cart)
                .Include(u => u.LikeRatings)
                .Include(u => u.Orders)
                .Include(u => u.Ratings)
                .FirstOrDefault(u => u.Id == id);
        }

        public void CreateUser(User user)
        {
            // Thêm User vào CSDL
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();

            // Khởi tạo và thêm Cart cho User
            var cart = new Cart { UserId = user.Id };
            _dataContext.Carts.Add(cart);
            _dataContext.SaveChanges();
        }


        public void UpdateUser(User user)
        {
            var existingUser = _dataContext.Users
                .FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null) return;

            existingUser.FullName = user.FullName;
            existingUser.Address = user.Address;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Email = user.Email;
            existingUser.Telephone = user.Telephone;
            existingUser.Gender = user.Gender;
            existingUser.UserName = user.UserName;
            existingUser.Password = user.Password;
            existingUser.Role = user.Role;

            _dataContext.Users.Update(existingUser);
            _dataContext.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _dataContext.Users
                .FirstOrDefault(u => u.Id == id);
            if (user == null) return;

            _dataContext.Users.Remove(user);
            _dataContext.SaveChanges();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {
            _dataContext.Update(user);
            await _dataContext.SaveChangesAsync();
        }
    }
}
