using SampleProject.DAL.Constants;
using SampleProject.DAL.Interface;
using SampleProject.Models;
using System.Data.SqlClient;
using System.Globalization;

namespace SampleProject.DAL
{
    public class UserProductRepository : ProductRepository, IUserProductRepository
    {
        private readonly IUserRepository _userRepository;

        private readonly IProductRepository productRepository;
        public UserProductRepository(IUserRepository userRepository, IProductRepository productRepository)
        {
            _userRepository = userRepository;
            this.productRepository = productRepository;
        }
        public Task<bool> PurchaseProduct(int userId, int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProductList> GetAllUserProductList()
        {
            var users = await _userRepository.GetAll();
            var Products = await productRepository.GetAll();
            return new UserProductList()
            {
                Users = users.ToList(),
                Products = Products.ToList()
            };
        }
        public async Task<IEnumerable<UserProductResponse>> GetAll()
        {

            List<UserProductResponse> userList = new List<UserProductResponse>();
            _connection.Open();
            using (_command = new SqlCommand(SqlConstant.GetAllUserProducts, _connection))
            {



                var result = ExecuteReader(_command);

                while (result.Read())
                {


                    UserProductResponse userProduct = new UserProductResponse()
                    {
                        Id = result.GetInt32(result.GetOrdinal("Id")),
                        User = await _userRepository.GetById(result.GetInt32(result.GetOrdinal("UserId"))),
                        Product = await productRepository.GetById(result.GetInt32(result.GetOrdinal("ProductId")))

                    };
                    userList.Add(userProduct);
                }

                //result.Close();
                _command.Connection.Close();
            }
            return userList;
        }
    }

}
