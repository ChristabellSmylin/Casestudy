using Art_gall.DAO;
using Art_gall.Model;
using Microsoft.Data.SqlClient;
using static Art_gall.Util.DBPropertyUtil;

namespace Art_gall.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get SqlConnection object
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                try
                {
                    // Open the connection
                    connection.Open();
                    // Connection established, you can execute SQL queries here
                    Console.WriteLine("Connected to database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            // Create an instance of services
            ArtworkAnalysisServiceImpl virtualArtGallery = new ArtworkAnalysisServiceImpl();
            UserloginServices userdetails = new UserloginServices();

            while (true)
            {
                // Display main menu options
                Console.WriteLine("\nWELCOME TO VIRTUAL ARTWORK GALLERY ");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. View all artworks");
                Console.WriteLine("4. Find artwork by ID");
                Console.WriteLine("5. Add artwork");
                Console.WriteLine("6. Update artwork");
                Console.WriteLine("7. Remove artwork by ID");
                Console.WriteLine("8. Search artworks");
                Console.WriteLine("9. Add artwork to favorites");
                Console.WriteLine("10. Remove artwork from favorites");
                Console.WriteLine("11. View user's favorite artworks");
                Console.WriteLine("12. Add Artwork to gallary");
                Console.WriteLine("13.favourite artwork gallery");
                Console.WriteLine("0. Logout");
                Console.WriteLine("enter your choice");

                // Get user input
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                // Perform actions based on user choice
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Please Enter Login Credentials");
                        Console.WriteLine("Enter your username: ");
                        string username = Console.ReadLine();

                        Console.WriteLine("Enter your password: ");
                        string password = Console.ReadLine();

                        User LoginUser = userdetails.LoginbyUser(username, password);
                        if (LoginUser != null)
                        {
                            Console.WriteLine($"User LOGIN successfully with Id {LoginUser.UserID}.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to Login With User details provided.");
                        }
                        break;

                    case 2:
                        Console.WriteLine("Please Enter New Registration Credentials");
                        User Adduser = new User();
                        bool isadduser = userdetails.RegisterUser(Adduser);
                        if (isadduser)
                        {
                            Console.WriteLine("User added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add User.");
                        }
                        break;

                    case 3:
                        Console.WriteLine("--------------- Get total Artwork List Start -----------------------");
                        List<Artwork> artworkList = virtualArtGallery.GetArtworkList();
                        foreach (Artwork artwork in artworkList)
                        {
                            Console.WriteLine($"ArtworkID: {artwork.ArtworkID}\n Title: {artwork.Title}\n Description: {artwork.Description}");
                        }
                        Console.WriteLine("---------------------- Get total Artwork List END -------------------");
                        break;

                    case 4:
                        Console.WriteLine("----------------------- Find Artwork By ID Start --------------------");
                        Console.Write("Enter the ID of the artwork to find: ");
                        if (int.TryParse(Console.ReadLine(), out int artworkID) && artworkID > 0)
                        {
                            Artwork artwork = virtualArtGallery.GetArtworkById(artworkID);
                            if (artwork != null)
                            {
                                Console.WriteLine($"ArtworkID found successfully");
                            }
                            else
                            {
                                Console.WriteLine("Failed to find artwork with the specified ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid artwork ID. Please enter a valid integer greater than 0.");
                        }
                        Console.WriteLine("----------------------- Find Artwork By ID END ------------------------");
                        break;

                    case 5:
                        Console.WriteLine("----------------------- Add Artwork Start ------------------------");
                        Artwork newArtwork = new Artwork();

                        bool isAdded = virtualArtGallery.AddArtwork(newArtwork);
                        if (isAdded)
                        {
                            Console.WriteLine("Artwork added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add artwork.");
                        }
                        Console.WriteLine("----------------------- Add Artwork END ------------------------");
                        break;

                    case 6:
                        Console.WriteLine("----------------------- Update Artwork Start ------------------------");
                        Artwork updateArtwork = new Artwork();
                        // Set updateArtwork properties here, for example:
                        // updateArtwork.ArtworkID = 1;
                        // updateArtwork.Title = "Updated Title";
                        bool isUpdated = virtualArtGallery.UpdateArtwork(updateArtwork);
                        if (isUpdated)
                        {
                            Console.WriteLine("Artwork updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to update artwork.");
                        }
                        Console.WriteLine("----------------------- Update Artwork END ------------------------");
                        break;

                    case 7:
                        Console.WriteLine("----------------------- Remove Artwork By ID Start ------------------------");
                        Console.Write("Enter the ID of the artwork to remove: ");
                        if (int.TryParse(Console.ReadLine(), out int removeArtworkID) && removeArtworkID > 0)
                        {
                            bool isRemoved = virtualArtGallery.RemoveArtwork(removeArtworkID);
                            if (isRemoved)
                            {
                                Console.WriteLine("Artwork removed successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to remove artwork.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid artwork ID. Please enter a valid integer greater than 0.");
                        }
                        Console.WriteLine("----------------------- Remove Artwork By ID END ------------------------");
                        break;

                    case 8:
                        Console.WriteLine("-------- Search Art Results Started ---------------------");
                        Console.Write("Enter the keyword to search for: ");
                        string keyword = Console.ReadLine();
                        List<Artwork> searchResults = virtualArtGallery.SearchArtworks(keyword);
                        foreach (Artwork searchArtwork in searchResults)
                        {
                            Console.WriteLine($"Search Art found successfully");
                        }
                        Console.WriteLine("-------- Search Art Results END ---------------------");
                        break;

                    case 9:
                        Console.WriteLine("-------- Add to Favorites Started ---------------------");
                        Console.Write("Enter the UserID: ");
                        int addUserId = int.Parse(Console.ReadLine());
                        Console.Write("Enter the ArtworkID: ");
                        int addArtworkId = int.Parse(Console.ReadLine());
                        bool addSuccess = virtualArtGallery.AddArtworkToFavorite(addUserId, addArtworkId);
                        if (addSuccess)
                        {
                            Console.WriteLine("Artwork added to favorites successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add artwork to favorites.");
                        }
                        Console.WriteLine("-------- Add to Favorites END ---------------------");
                        break;

                    case 10:
                        Console.WriteLine("-------- Remove from Favorites Started ---------------------");
                        Console.Write("Enter the UserID: ");
                        int removeUserId = int.Parse(Console.ReadLine());
                        Console.Write("Enter the ArtworkID: ");
                        int removeArtworkId = int.Parse(Console.ReadLine());
                        bool removeSuccess = virtualArtGallery.RemoveArtworkFromFavorite(removeUserId, removeArtworkId);
                        if (removeSuccess)
                        {
                            Console.WriteLine("Artwork removed from favorites successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to remove artwork from favorites.");
                        }
                        Console.WriteLine("-------- Remove from Favorites END ---------------------");
                        break;

                    case 11:
                        Console.WriteLine("-------- Get User Favorite Artworks Started ---------------------");
                        Console.Write("Enter the UserID: ");
                        int favUserId = int.Parse(Console.ReadLine());
                        List<Artwork> favoriteArtworks = virtualArtGallery.GetUserFavoriteArtworks(favUserId);
                        if (favoriteArtworks.Count > 0)
                        {
                            Console.WriteLine("User's Favorite Artworks:");
                            foreach (Artwork favArtwork in favoriteArtworks)
                            {
                                Console.WriteLine($"ArtworkID: {favArtwork.ArtworkID}\n Title: {favArtwork.Title}\n Description: {favArtwork.Description}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("User has no favorite artworks.");
                        }
                        Console.WriteLine("-------- Get User Favorite Artworks END ---------------------");
                        break;

                        case 12:

                        Console.WriteLine("-------- Add to Artwork to Gallery Started ---------------------");
                        // Call the method to add artwork to gallery
                        Console.Write("Enter the Artwork ID: ");
                        int artworkId = int.Parse(Console.ReadLine());
                        Console.Write("Enter the Gallery ID: ");
                        int galleryId = int.Parse(Console.ReadLine());

                        bool addedToGallery = virtualArtGallery.AddArtworktoGallery(artworkId, galleryId);
                        if (addedToGallery)
                        {
                            Console.WriteLine("Artwork added to gallery successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add artwork to gallery.");
                        }
                        break;

                    case 13:

                        Console.WriteLine("-------- Get Favorite Artworks Gallery Started ---------------------");
                        Console.Write("Enter the UserID: ");
                        int userId = int.Parse(Console.ReadLine()); // Changed variable name to userId for clarity
                        List<Artwork> favoriteArtworksgallery = virtualArtGallery.GetUserFavoriteArtworkGallery(userId);

                        if (favoriteArtworksgallery.Count > 0)
                        {
                            Console.WriteLine("User's Favorite Artworks:");
                            foreach (Artwork favArtwork in favoriteArtworksgallery)
                            {
                                Console.WriteLine($"ArtworkID: {favArtwork.ArtworkID}\n Title: {favArtwork.Title}\n Description: {favArtwork.Description}\n CreationDate: {favArtwork.CreationDate}\n Medium: {favArtwork.Medium}\n ImageURL: {favArtwork.ImageURL}\n ArtistID: {favArtwork.ArtistID}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("This user has no favorite artworks.");
                        }
                        Console.WriteLine("-------- Get Favorite Artworks Gallery END ---------------------");
                        break;


                    case 0:
                        Console.WriteLine("Logout Starts");
                        userdetails.Logout();
                        Console.WriteLine("Logged out successfully.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
            }
        }
    }
}
