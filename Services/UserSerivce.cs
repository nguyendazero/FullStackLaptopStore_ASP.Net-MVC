using Microsoft.EntityFrameworkCore;
using DotNet.BookStore.Models;
using System.Collections.Generic;
using System.Linq;
using DotNet.BookStore.Data;

namespace DotNet.BookStore.Services
{
    public interface IUserService
    {
        List<User> GetAllUsers(string? keySearch = null);
        User? GetUserById(int id);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);

        User? GetUserByUsernameAndPass(string userName, string password);
    }

    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;

        public UserService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public User? GetUserByUsernameAndPass(string userName, string password)
        {
            return _dataContext.Users
                .FirstOrDefault(u => u.UserName == userName && u.Password == password);
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
            _dataContext.Users.Add(user);
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
    }
}
