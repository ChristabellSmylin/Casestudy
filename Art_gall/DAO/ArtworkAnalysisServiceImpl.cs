using Art_gall.Model;
using Art_gall.Service;
using Art_gall.Util;
using Microsoft.Data.SqlClient;
using static Art_gall.Exceptions.ExceptionHandling;

namespace Art_gall.DAO
{
    public class ArtworkAnalysisServiceImpl : IVirtualArtGallery
    {

        // Connection string for connecting to the database
        private string connectionString;

        //constructor
        public ArtworkAnalysisServiceImpl()
        {
            connectionString = PropertyUtil.GetPropertyString();
        }
        public List<Artwork> GetArtworkList()
        {
            List<Artwork> artworkList = new List<Artwork>();

            try
            {
                using (SqlConnection connection = DBPropertyUtil.DBConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT * FROM Artwork";

                    using (SqlCommand command = new SqlCommand(query, connection))//execution of sql query
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Artwork artwork = new Artwork
                                {
                                    ArtworkID = reader.GetInt32(reader.GetOrdinal("ArtworkID")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                    ImageURL = reader.GetString(reader.GetOrdinal("ImageURL")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Medium = reader.GetString(reader.GetOrdinal("Medium"))
                                };

                                artworkList.Add(artwork);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArtWorkNotFoundException(ex.Message);
            }

            foreach (Artwork artwork in artworkList)
            {
                Console.WriteLine($"ArtworkID: {artwork.ArtworkID} \n Title: {artwork.Title} \n CreationDate: {artwork.CreationDate} \n  ImageURL: {artwork.ImageURL}\n Description: {artwork.Description}\n Medium: {artwork.Medium}\n");
            }
            return artworkList;

        }

        public bool AddArtwork(Artwork artwork)
        {
            if (artwork == null)
            {
                throw new ArgumentNullException(nameof(artwork));
            }

            if (artwork.ArtworkID != 0)
            {
                throw new ArgumentException("Artwork ID must be 0 for new artworks", nameof(artwork));
            }
            // Get input from the user
            ArtworkManagement.GetArtworkDetailsFromUser(artwork);

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                string sql = "INSERT INTO Artwork (Title, Description, CreationDate, Medium, ImageURL) VALUES (@Title, @Description, @CreationDate, @Medium, @ImageURL); SELECT SCOPE_IDENTITY();";

                command.CommandText = sql;
                command.Parameters.AddWithValue("@Title", artwork.Title);
                command.Parameters.AddWithValue("@Description", artwork.Description);
                command.Parameters.AddWithValue("@CreationDate", artwork.CreationDate);
                command.Parameters.AddWithValue("@Medium", artwork.Medium);
                command.Parameters.AddWithValue("@ImageURL", artwork.ImageURL);

                connection.Open();

                // Execute the command and get the last inserted identity value
                object result = command.ExecuteScalar();
                int lastInsertedId = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                if (lastInsertedId > 0)
                {
                    // Set the ArtworkID of the provided artwork object
                    artwork.ArtworkID = lastInsertedId;
                    
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to insert artwork.");
                    return false;
                }

            }

            // Connection will be automatically closed when exiting the using block
        }

        //remove//
        public bool RemoveArtwork(int artworkID)
        {
            try
            {
                if (artworkID <= 0)
                {
                    throw new ArgumentException("Artwork ID must be greater than 0", nameof(artworkID));
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    string sql = "DELETE FROM Artwork WHERE ArtworkID = @ArtworkID";

                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@ArtworkID", artworkID);

                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                    
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Failed to remove artwork. Artwork with specified ID not found.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArtWorkNotFoundException(ex.Message);
            }

            // Connection will be automatically closed when exiting the using block
        }

        public bool UpdateArtwork(Artwork updateartwork)
        {
            try
            {
                ArtworkManagement.UpdateArtworkDetailsFromUser(updateartwork);
                if (updateartwork == null)
                {
                    throw new ArgumentNullException(nameof(updateartwork));
                }

                if (updateartwork.ArtworkID == 0)
                {
                    throw new ArgumentException("Artwork ID cannot be 0 for updating existing artwork", nameof(updateartwork));
                }


                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    string sql = "UPDATE Artwork SET Title = @Title, Description = @Description, CreationDate = @CreationDate, Medium = @Medium, ImageURL = @ImageURL WHERE ArtworkID = @ArtworkID;";

                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Title", updateartwork.Title);
                    command.Parameters.AddWithValue("@Description", updateartwork.Description);
                    command.Parameters.AddWithValue("@CreationDate", updateartwork.CreationDate);
                    command.Parameters.AddWithValue("@Medium", updateartwork.Medium);
                    command.Parameters.AddWithValue("@ImageURL", updateartwork.ImageURL);
                    command.Parameters.AddWithValue("@ArtworkID", updateartwork.ArtworkID);

                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                      
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Failed to update artwork.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArtWorkNotFoundException(ex.Message);
            }
        }

        public Artwork GetArtworkById(int artworkID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Artwork WHERE ArtworkID = @ArtworkID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ArtworkID", artworkID);

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Artwork artwork = new Artwork
                                {
                                    ArtworkID = Convert.ToInt32(reader["ArtworkID"]),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    CreationDate = reader["CreationDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreationDate"]) : DateTime.MinValue,
                                    Medium = reader["Medium"] != DBNull.Value ? reader["Medium"].ToString() : string.Empty,
                                    ImageURL = reader["ImageURL"] != DBNull.Value ? reader["ImageURL"].ToString() : string.Empty,
                                    ArtistID = reader["ArtistID"] != DBNull.Value ? Convert.ToInt32(reader["ArtistID"]) : 0
                                };
                                Console.WriteLine($"ArtworkID: {artwork.ArtworkID} \n Title: {artwork.Title} \n CreationDate: {artwork.CreationDate} \n ImageURL: {artwork.ImageURL} \n Description: {artwork.Description}\n Medium: {artwork.Medium}");

                                return artwork;
                            }
                            else
                            {
                                return null;
                            }

                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArtWorkNotFoundException(ex.Message);
            }
        }



        public List<Artwork> SearchArtworks(string keyword)
        {
            try
            {
                List<Artwork> searchResults = new List<Artwork>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Artwork WHERE Title LIKE @Keyword OR Description LIKE @Keyword";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Add parameter for keyword
                        cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Artwork artwork = new Artwork
                                {
                                    ArtworkID = Convert.ToInt32(reader["ArtworkID"]),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    CreationDate = reader["CreationDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreationDate"]) : DateTime.MinValue,
                                    Medium = reader["Medium"] != DBNull.Value ? reader["Medium"].ToString() : string.Empty,
                                    ImageURL = reader["ImageURL"] != DBNull.Value ? reader["ImageURL"].ToString() : string.Empty,
                                    ArtistID = reader["ArtistID"] != DBNull.Value ? Convert.ToInt32(reader["ArtistID"]) : 0
                                };
                                searchResults.Add(artwork);
                            }
                        }
                    }
                }
                Console.WriteLine("-------------------------------search results---------------------------------------------");
                foreach (Artwork artwork in searchResults)
                {
                    Console.WriteLine($"ArtworkID: {artwork.ArtworkID}\n Title: {artwork.Title}\n CreationDate: {artwork.CreationDate}\n ImageURL: {artwork.ImageURL}\n Description: {artwork.Description} \n Medium: {artwork.Medium}");
                }
                return searchResults;
            }
            catch (Exception ex)
            {
                throw new ArtWorkNotFoundException(ex.Message);
            }
        }

        //Add art work to favourites
        public bool AddArtworkToFavorite(int userId, int artworkId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO User_Favorite_Artwork (UserID, ArtworkID) " +
                                   "VALUES (@UserID, @ArtworkID)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@ArtworkID", artworkId);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        // return rowsAffected > 0;
                        if (rowsAffected > 0)
                        {
                           
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Failed to Add artWork to fav .");
                            return false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArtWorkNotFoundException(ex.Message); ;

            }
        }

        public bool RemoveArtworkFromFavorite(int userId, int artworkId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM User_Favorite_Artwork WHERE UserID = @UserID AND ArtworkID = @ArtworkID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@ArtworkID", artworkId);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Failed to Remove artwork.");
                            return false;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw new ArtWorkNotFoundException(ex.Message);

            }

        }

        public List<Artwork> GetUserFavoriteArtworks(int userId)
        {
            List<Artwork> favoriteArtworks = new List<Artwork>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT a.* " +
                                   "FROM Artwork AS a  " +
                                   "INNER JOIN User_Favorite_Artwork AS ufa ON a.ArtworkID = ufa.ArtworkID " +
                                   "WHERE ufa.UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Artwork artwork = new Artwork
                                {
                                    ArtworkID = Convert.ToInt32(reader["ArtworkID"]),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    CreationDate = reader["CreationDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreationDate"]) : DateTime.MinValue,
                                    Medium = reader["Medium"] != DBNull.Value ? reader["Medium"].ToString() : string.Empty,
                                    ImageURL = reader["ImageURL"] != DBNull.Value ? reader["ImageURL"].ToString() : string.Empty,
                                    ArtistID = reader["ArtistID"] != DBNull.Value ? Convert.ToInt32(reader["ArtistID"]) : 0
                                };
                                favoriteArtworks.Add(artwork);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserNotFoundException(ex.Message);
                //Console.WriteLine("Error getting user's favorite artworks: " + ex.Message);
            }

            return favoriteArtworks;
        }

        //ADD ART WORK TO GALLERY//

        public bool AddArtworktoGallery(int artworkId, int galleryId)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Artwork_Gallery (ArtworkID, GalleryID) " +
                                   "VALUES (@ArtworkID, @GalleryID)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@ArtworkID", artworkId);
                        cmd.Parameters.AddWithValue("@GalleryID", galleryId);

                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        // return rowsAffected > 0;
                        if (rowsAffected > 0)
                        {
                           
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Failed to Add  to Gallery .");
                            return false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new GalleryNotFoundException(ex.Message); ;

            }
        }

        public List<Artwork> GetFavoriteArtworkGallery(int galleryId)
        {
            List<Artwork> favoriteArtworksgallery = new List<Artwork>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT a.*
                                  FROM Artwork AS a
                                  INNER JOIN Artwork_Gallery AS g ON a.ArtworkID = g.ArtworkID
                                  WHERE g.GalleryID = @GalleryID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Correct parameter name and value
                        cmd.Parameters.AddWithValue("@GalleryID", galleryId);

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Artwork artwork = new Artwork
                                {
                                    ArtworkID = Convert.ToInt32(reader["ArtworkID"]),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    CreationDate = reader["CreationDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreationDate"]) : DateTime.MinValue,
                                    Medium = reader["Medium"] != DBNull.Value ? reader["Medium"].ToString() : string.Empty,
                                    ImageURL = reader["ImageURL"] != DBNull.Value ? reader["ImageURL"].ToString() : string.Empty,
                                    //ArtistID = reader["ArtistID"] != DBNull.Value ? Convert.ToInt32(reader["ArtistID"]) : 0
                                };
                                favoriteArtworksgallery.Add(artwork);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArtWorkNotFoundException(ex.Message);
            }

            return favoriteArtworksgallery;
        }

        public List<Gallery> GetGalleryList()
        {
            List<Gallery> galleryList = new List<Gallery>();

            try
            {
                using (SqlConnection connection = DBPropertyUtil.DBConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT * FROM Gallery";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Gallery gallery = new Gallery
                                {
                                    GalleryID = reader.GetInt32(reader.GetOrdinal("GalleryID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Website = reader.GetString(reader.GetOrdinal("Website")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Location = reader.GetString(reader.GetOrdinal("Location")),
                                    OpeningHours = reader.GetString(reader.GetOrdinal("OpeningHours")),
                                };

                                galleryList.Add(gallery);
                            }
                        }
                    }
                }
            }
           
            catch (Exception ex)
            {
                throw new GalleryNotFoundException(ex.Message);
            }

            foreach (Gallery gallery in galleryList)
            {
                Console.WriteLine($"GalleryID: {gallery.GalleryID} \nName: {gallery.Name} \n Website: {gallery.Website}\nDescription: {gallery.Description} \nLocation: {gallery.Location}\nOpeningHours: {gallery.OpeningHours}\n");
            }
            return galleryList;
        }

        public List<Artwork> GetArtworkByGallery(int galleryId)
        {

            List<Artwork> getartworkbygallery = new List<Artwork>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT a.*
                                  FROM Artwork AS a
                                  INNER JOIN Artwork_Gallery AS g ON a.ArtworkID = g.ArtworkID
                                  WHERE g.GalleryID = @GalleryID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Correct parameter name and value
                        cmd.Parameters.AddWithValue("@GalleryID", galleryId);

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Artwork artwork = new Artwork
                                {
                                    ArtworkID = Convert.ToInt32(reader["ArtworkID"]),
                                    Title = reader["Title"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    CreationDate = reader["CreationDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreationDate"]) : DateTime.MinValue,
                                    Medium = reader["Medium"] != DBNull.Value ? reader["Medium"].ToString() : string.Empty,
                                    ImageURL = reader["ImageURL"] != DBNull.Value ? reader["ImageURL"].ToString() : string.Empty,
                                    
                                };
                                getartworkbygallery.Add(artwork);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new GalleryNotFoundException(ex.Message);
            }

            return getartworkbygallery;
        }
    }
}


