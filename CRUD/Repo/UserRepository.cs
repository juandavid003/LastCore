using System;
using System.Collections.Generic;
using System.Linq;
using DB;

public class UserRepository
{
    private readonly DB_CrudLogInEntities _dbContext;

    public UserRepository(string connectionString)
    {
        _dbContext = new DB_CrudLogInEntities(connectionString);
    }

    public IEnumerable<UserWithRoleDto> GetAllUsers()
    {
        return (from user in _dbContext.users
                join role in _dbContext.roles on user.roleId equals role.id
                select new UserWithRoleDto
                {
                    Id = user.id,
                    FirstName = user.firstName,
                    LastName = user.lastName,
                    BirthDate = user.birthDate,
                    RoleName = role.name,
                    CreatedAt = user.createdAt,
                    UpdatedAt = user.updatedAt,
                    Password = user.password,
                    Status = user.status,
                }).ToList();
    }

    public UserWithRoleDto GetUserById(int id)
    {
        return (from user in _dbContext.users
                join role in _dbContext.roles on user.roleId equals role.id
                where user.id == id
                select new UserWithRoleDto
                {
                    Id = user.id,
                    FirstName = user.firstName,
                    LastName = user.lastName,
                    BirthDate = user.birthDate,
                    RoleName = role.name,
                    CreatedAt = user.createdAt,
                    UpdatedAt = user.updatedAt,
                    Password = user.password,
                    Status = user.status,
                }).FirstOrDefault();
    }

    public void AddUser(user newUser)
    {
        _dbContext.users.Add(newUser);
        _dbContext.SaveChanges();
    }

    public bool UpdateUser(int id, user updatedUser)
    {
        var existingUser = _dbContext.users.FirstOrDefault(u => u.id == id);
        if (existingUser == null)
            return false;

        existingUser.firstName = updatedUser.firstName;
        existingUser.lastName = updatedUser.lastName;
        existingUser.password = updatedUser.password;
        existingUser.birthDate = updatedUser.birthDate;
        existingUser.roleId = updatedUser.roleId;
        existingUser.updatedAt = updatedUser.updatedAt;
        existingUser.createdAt = updatedUser.createdAt;
        existingUser.status = updatedUser.status;

        _dbContext.SaveChanges();
        return true;
    }

    public bool DeleteUser(int id)
    {
        var user = _dbContext.users.FirstOrDefault(u => u.id == id);
        if (user == null)
            return false;

        _dbContext.users.Remove(user);
        _dbContext.SaveChanges();
        return true;
    }
}
