using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace bookieAPI.Controllers
{
    public class CloudinaryUpload
    {
        private static readonly string cloudName = "dlzeae5mn";
        private static readonly string apiKey = "781352964829158";
        private static readonly string apiSecret = "0ll3sXB3DZJlX8LY4ged2IG2amM";
        public static void SaveImageUrlToDatabase(string imageUrl)
        {
            // Csatlakozás az adatbázishoz
            string connectionString = "your_connection_string_here";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Images (ImageUrl) VALUES (@ImageUrl)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ImageUrl", imageUrl);
                    command.ExecuteNonQuery();
                }
            }
        }
        static void Main()
        {
            // Kép feltöltése
            string filePath = @"C:\path\to\your\image.jpg";//ezt lesz a file.path jsben
            var account = new Account(cloudName, apiKey, apiSecret);
            var cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()//feltolti cloudinaryba
            {
                File = new FileDescription(filePath)
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            SaveImageUrlToDatabase(uploadResult.SecureUrl.ToString());//adatbazisba feltoltes
        }
    }
}
