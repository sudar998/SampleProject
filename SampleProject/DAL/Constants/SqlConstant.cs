using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace SampleProject.DAL.Constants
{
    public class SqlConstant
    {

        public const string ConnectionStringName = "SampleConnection";

        public const string CreateUserTable = "CreateUserTable";
        public const string CreateProductTable = "CreateProductTable";


        public const string CreateUserProductTable = "CreateUserProductTable";


        public const string InsertProductData = "InsertProductData";
        public const string GetAllUserProd = "GetAllUser";
        public const string GetAllUsers = "GetAllUser";

        public const string InsertUserData = "InsertUserData";



        public const string GetAllUserProduct = "GetAllUserProduct";


        public const string GetAllProducts = "SelectAllProducts";

        public const string GetUserById = "GetUserById";


        public const string GetProductById = "GetProductById";

        public const string DeleteUser = "DeleteUser";



        public const string DeleteProduct = "DeleteProduct";


        public const string UpdateUser = "UpdateUser";


        public const string UpdateProduct = "UpdateProduct";
    }
}
