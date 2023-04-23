using static System.Net.Mime.MediaTypeNames;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace WebAuthentication.Data
{
    public class UserRepository
    {
        private string _connectionString { get; set; }

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Ad> GetAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"select * from Ad";
            

            connection.Open();
            var reader = cmd.ExecuteReader();

        
            List<Ad> ads = new List<Ad>();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    DateListed = (DateTime)reader["DateListed"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Details = (string)reader["Details"],
                    UserId = (int)reader["UserId"],
                });
            }
          
            return ads;

        }
        public void AddUser(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"insert into Users(Name, Email, PasswordHash)
                                values(@name, @email, @hash)";

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hash", passwordHash);
            

            connection.Open();
            cmd.ExecuteNonQuery();

        }
        public User Login(string email, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

              User user = new User();
           
              user.Id = (int)reader["Id"];
              user.Name = (string)reader["Name"];
              user.Email = (string)reader["Email"];
              user.PasswordHash = (string)reader["PasswordHash"];

              var isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isValid)
            {
                return null;
            }

            return user;

        }
        public int GetUserId(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Email = @email; SELECT SCOPE_IDENTITY()";

            cmd.Parameters.AddWithValue("@email", email);

           

            connection.Open();
           
            User user = new User();
           
            int id = (int)cmd.ExecuteScalar();
            return id;

        }
        public string GetName(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT NAME FROM Users WHERE Id = @id";

            cmd.Parameters.AddWithValue("@id", id);


            connection.Open();

            var reader = cmd.ExecuteReader();

            User user = new User();
            while (reader.Read())
            {
                user.Name = (string)reader["Name"];
            }

            return user.Name;

        }
        public void AddAd(Ad ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"insert into Ad(Name, dateListed, PhoneNumber, Details, UserId)
                                values(@name, @dateListed, @phoneNumber, @details, @userId)";

           
            cmd.Parameters.AddWithValue("@name", ad.Name);
            cmd.Parameters.AddWithValue("@dateListed", ad.DateListed);
            cmd.Parameters.AddWithValue("@phonenumber", ad.PhoneNumber);
            cmd.Parameters.AddWithValue("@details", ad.Details);
            cmd.Parameters.AddWithValue("@userId", ad.UserId);

            connection.Open();
            cmd.ExecuteNonQuery();

        }
        public void DeleteAd(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"Delete from Ad where Id = @id";


            cmd.Parameters.AddWithValue("@id", id);
          

            connection.Open();
            cmd.ExecuteNonQuery();

        }
        public List<Ad> GetMyAds(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"select * from Ad Where UserId = @id";
            cmd.Parameters.AddWithValue("@id", id);


            connection.Open();
            var reader = cmd.ExecuteReader();


            List<Ad> ads = new List<Ad>();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    DateListed = (DateTime)reader["DateListed"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Details = (string)reader["Details"],
                    UserId = (int)reader["UserId"],
                });
            }

            return ads;

        }
    }
}