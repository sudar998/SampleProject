using Microsoft.AspNetCore.Identity;
using SampleProject.DAL.Constants;
using SampleProject.DAL.Interface;
using SampleProject.Models;
using System.Data.SqlClient;

namespace SampleProject.DAL
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository()
        {

        }
        public async Task Create(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _command = new SqlCommand(SqlConstant.InsertUserData, _connection);
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            _command.Parameters.AddWithValue("@Name", user.Name);
            _command.Parameters.AddWithValue("@SecurityNumber", user.SecurityNumber);
            _command.Parameters.AddWithValue("@Email" ,user.Email); 
            _command.Parameters.AddWithValue("@Password" , user.Password);
            _command.Parameters.AddWithValue("@isDeleted", 0);
            _command.Parameters.AddWithValue("@Salt", user.Salt);


            await ExecuteNonQuery(_command);

        }

       

        public async Task<IEnumerable<User>> GetAll()
        {
            _command = new SqlCommand(SqlConstant.GetAllUsers, _connection);
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            List<User> userList = new List<User>();
            var result = ExecuteReader(_command);
            while (result.Read())
            {
                User user = new User()
                {
                    Id =  Convert.ToInt32( result["Id"]),
                    Name = result["Name"].ToString(), 
                    Email =  result["Email"].ToString(),
                    SecurityNumber =Convert.ToInt32( result["SecurityNumber"]),
                };
                userList.Add(user);
            }
            result.Close();
            return userList;
        }


        public async Task<bool> Delete(int id)
        {
            if (id == 0) throw new Exception($"Id {id} is not valid");
            _command = new SqlCommand(SqlConstant.GetUserById, _connection);
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            _command.Parameters.AddWithValue("Id", id);


            var response = ExecuteReader(_command);

            if (!response.Read()) return false;
            response.Close();
            _command = new SqlCommand(SqlConstant.DeleteUser, _connection);
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            _command.Parameters.AddWithValue("@Id", id);
            await ExecuteNonQuery(_command);
            return true;

        }
        public async Task<User> GetBySecurityNumber(int securityNumber)
        {
            if (securityNumber == 0) throw new Exception($"Security Number {securityNumber} is not valid");
            if (_connection == null)
            {
                _connection = new SqlConnection(GetConnectionString());
                _connection.Open();
            }

            _command = new SqlCommand(SqlConstant.GetUserBySecurityNumber, _connection);
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            _command.Parameters.AddWithValue("@SecurityNumber", securityNumber);
            User? user = null;
            var result = ExecuteReader(_command);
            if (result.Read())
            {
                user = new User()
                {
                    Id = Convert.ToInt32(result["Id"]),
                    Name = result["Name"].ToString(),
                    Email = result["Email"].ToString(),
                    Password = result["Password"].ToString(),
                    Salt = result["Salt"].ToString(),
                    SecurityNumber = Convert.ToInt32(result["SecurityNumber"]),

                };

            }
            result.Close();
            return user!;
        }

        public async Task<User> GetById(int Id)
        {
            if (Id == 0) throw new Exception($"ID {Id} is not valid");
            if (_connection == null)
            {
                _connection = new SqlConnection(GetConnectionString());
                _connection.Open();
            }

            _command = new SqlCommand(SqlConstant.GetUserById, _connection);
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            _command.Parameters.AddWithValue("@Id", Id);
            User? user = null;
            var result = ExecuteReader(_command);
            if (result.Read())
            {
                user = new User()
                {
                    Id = Convert.ToInt32(result["Id"]),
                    Name = result["Name"].ToString(),
                    Email = result["Email"].ToString(),
                    Password = result["Password"].ToString(),
                    SecurityNumber = Convert.ToInt32(result["SecurityNumber"]),

                };

            }
            result.Close();
            return user!;
        }

        public async Task<bool> Update(User user, int id)
        {
            if (user == null) throw new Exception($"User is not specified"); 
            if (id == 0) throw new Exception($"Id {id} is not valid");
            _command = new SqlCommand(SqlConstant.GetUserById, _connection);
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            _command.Parameters.AddWithValue("Id", id);


            var response = ExecuteReader(_command);

            if (!response.Read()) return false;

            response.Close();
            _command = new SqlCommand(SqlConstant.UpdateUser, _connection);
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            _command.Parameters.AddWithValue("@Name", user.Name);
            _command.Parameters.AddWithValue("@Email", user.Email);
            _command.Parameters.AddWithValue("@Password", user.Password);
            _command.Parameters.AddWithValue("@Id", id); 
            await ExecuteNonQuery(_command);

            return true; 
        }


        
    }
}
