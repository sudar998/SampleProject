using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Migrations;
using SampleProject.DAL.Constants;
using SampleProject.DAL.Interface;
using System.Data.SqlClient;

namespace SampleProject.DAL
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        //private IConfiguration _configuration { get; set; }
        protected SqlCommand _command;
        protected SqlConnection _connection { get; set; }

        private IConfiguration Configuration { get; set; }

        public Repository()
        {

            _connection = new SqlConnection(GetConnectionString());

            //ConnectionString = new SqlConnection(""); 
        }

        public string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString(SqlConstant.ConnectionStringName)!;
        }

        public async Task Migrate()
        {


            using (_command = new SqlCommand(SqlConstant.CreateUserTable, _connection))
            {

                await ExecuteNonQuery(_command);

            }

            using(_command = new SqlCommand(SqlConstant.CreateProductTable, _connection))
            {
                   await ExecuteNonQuery(_command); 
            }
            using (_command = new SqlCommand(SqlConstant.CreateUserProductTable , _connection))
            {
                  await ExecuteNonQuery(_command);  
            }
             

        }

        protected SqlDataReader ExecuteReader(SqlCommand command)
        {
            try
            {
                if(_connection == null) _connection= new SqlConnection(GetConnectionString());  
                if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }
               
                var reader = _command.ExecuteReader();
                _connection = null;
                

                return reader;
            }
            catch (Exception ex)

            {
                throw new Exception($"{ex.Message}");
            }
            finally
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                }
            }
        }
        protected async Task ExecuteNonQuery(SqlCommand command)
        {
            try
            {

                if (command.Connection == null)
                {
                    command.Connection = new SqlConnection(GetConnectionString());
                }
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                await command.ExecuteNonQueryAsync();


            }
            catch (Exception ex)
            {
                throw new Exception($"Exception occured : {ex.Message}");
            }

        }
    }
}
