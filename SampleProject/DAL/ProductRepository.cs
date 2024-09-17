using SampleProject.DAL.Constants;
using SampleProject.DAL.Interface;
using SampleProject.Models;
using System.Data.SqlClient;

namespace SampleProject.DAL
{
    public class ProductRepository :  Repository<Product>,  IProductRepository
    {
        public async Task Create(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            _command = new SqlCommand(SqlConstant.InserProductData, _connection);
            _command.Parameters.AddWithValue("@Name", product.Name);
            _command.Parameters.AddWithValue("@AvailableQuantity", product.AvailableQuantity);
            _command.Parameters.AddWithValue("@isDeleted", 0);

            await ExecuteNonQuery(_command);

        }


        public async Task<IEnumerable<Product>> GetAll()
        {
            _command = new SqlCommand(SqlConstant.GetAllProducts, _connection);
            List<Product> productList = new List<Product>();
            var result = ExecuteReader(_command);
            while (result.Read())
            {
                Product product = new Product()
                {
                    Id = Convert.ToInt32(result["Id"]),
                    Name = result["Name"].ToString(),
                    AvailableQuantity = Convert.ToInt32(result["AvailableQuantity"])

                };
                productList.Add(product);
            }

            result.Close(); 
            return productList;
        }

  

        public async Task<bool> Delete(int id)
        {
            if (id == 0) throw new Exception($"Id {id} is not valid");
            _command = new SqlCommand(SqlConstant.GetProductById, _connection);
            _command.Parameters.AddWithValue("Id", id);


            var response = ExecuteReader(_command);



            if (!response.Read()) return false;
            response.Close(); 
            _command = new SqlCommand(SqlConstant.DeleteProduct, _connection);
            _command.Parameters.AddWithValue("@Id", id);
            await ExecuteNonQuery(_command);
            return true;

        }
        public async Task<Product> GetById(int Id)
        {
            if (Id == 0) throw new Exception($"ID {Id} is not valid");

           if(_connection == null)
            {
                _connection = new SqlConnection(GetConnectionString());
                _connection.Open(); 
            }
            _command = new SqlCommand(SqlConstant.GetProductById, _connection);
            _command.Parameters.AddWithValue("@Id", Id);
            Product? product = null;
            var result = ExecuteReader(_command);
            if (result.Read())
            {
                product = new Product()
                {
                    Id= result.GetInt32(result.GetOrdinal("Id")),
                    Name = result.GetString(result.GetOrdinal("Name")),
                    AvailableQuantity = result.GetInt32(result.GetOrdinal("AvailableQuantity"))
                };

            }
            result.Close(); 
            return product!;
        }

        public async Task<bool> Update(Product product, int id)
        {
            if (product == null) throw new Exception($"Product is not specified");
            if (id == 0) throw new Exception($"Id {id} is not valid");
            _command = new SqlCommand(SqlConstant.GetProductById, _connection);
            _command.Parameters.AddWithValue("Id", id);


            var response = ExecuteReader(_command);

            if (!response.Read()) return false;

            response.Close();
            _command = new SqlCommand(SqlConstant.UpdateProduct, _connection);
            _command.Parameters.AddWithValue("@Name", product.Name);
            _command.Parameters.AddWithValue("@AvailableQuantity" , product.AvailableQuantity);
            _command.Parameters.AddWithValue("@Id", id);
            await ExecuteNonQuery(_command);

            return true;
        }
    }
}

