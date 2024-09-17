using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace SampleProject.DAL.Constants
{
    public class SqlConstant
    {

        public const string ConnectionStringName = "SampleConnection";
        public const string CreateUserTable = @"CREATE TABLE [User](Id INT PRIMARY KEY IDENTITY(1,1) ,SecurityNumber int unique not null ,Name varchar(50) NOT NULL , Email varchar(50) not null , Password varchar(50) not null, isDeleted bit Not Null);";

        public const string CreateProductTable = @"Create Table Product(Id int primary key identity(1,1) , Name varchar(50) not null, AvailableQuantity int not null , isDeleted bit Not Null);";


        public const string CreateUserProductTable = @"Create table UserProducts(Id int primary key identity(1,1) , UserId int not null , isDeleted bit Not Null, foreign key(UserId) references [User](Id) , ProductId int not null, foreign key
         (ProductId) references Product(Id));";

        public const string InsertUserData = @"Insert into [User](Name, SecurityNumber,Email,Password,IsDeleted) values(@Name, @SecurityNumber,@Email,@Password,@isDeleted);";

        public const string InserProductData = @"Insert into Product(Name, AvailableQuantity,isDeleted)  values(@Name,@AvailableQuantity,@isDeleted);";
        public const string GetAllUsers = @"Select * from [User];";


        public const string GetAllUserProducts = @"select * from userproducts up join [user] u 
  on up.userid = u.Id  join product p on up.productId = p.Id;";
        public const string GetAllProducts = @"Select * from Product;";
        public const string GetUserById = @"Select * from [User] where Id=@Id;";

        public const string GetProductById = @"Select * from Product where Id=@Id;";
        public const string DeleteUser = @"Delete from [User] where Id=@Id;";
        public const string DeleteProduct = @"Delete from Product where Id=@Id;";



        public const string UpdateUser = @"Update [User] set Name=@Name, Email=@Email , Password=@Password where Id=@Id;";

        public const string UpdateProduct = @"Update Product set Name=@Name, AvailableQuantity=@AvailableQuantity where Id=@Id;";

    }
}
