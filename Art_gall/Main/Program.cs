using Art_gall.DAO;
using Art_gall.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using static Art_gall.Util.DBPropertyUtil;

namespace Art_gall.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            bool loggedIn = false;
            User currentUser = null;

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
                // Display menu options based on login status
                if (!loggedIn)
                {
                    Console.WriteLine("\nPlease Enter the below options");
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("0. Exit");
                }
                else
                {
                    Console.WriteLine("***********Welcome to Virtual Art Gallery************");
                    Console.WriteLine("1. View all artworks");
                    Console.WriteLine("2. Find artwork by ID");
                    Console.WriteLine("3. Add artwork");
                    Console.WriteLine("4. Update artwork");
                    Console.WriteLine("5. Remove artwork by ID");
                    Console.WriteLine("6. Search artworks");
                    Console.WriteLine("7. Add artwork to favorites");
                    Console.WriteLine("8. Remove artwork from favorites");
                    Console.WriteLine("9. View user's favorite artworks");
                    Console.WriteLine("10. Add Artwork to gallery");
                    Console.WriteLine("11. Get artwork By gallery");
                    Console.WriteLine("0. Logout");
                }
                Console.WriteLine("Enter your choice:");

                // Get user input
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                // Perform actions based on user choice
                if (!loggedIn)
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Please Enter Login Credentials");
                            Console.WriteLine("Enter your username: ");
                            string username = Console.ReadLine();

                            Console.WriteLine("Enter your password: ");
                            string password = Console.ReadLine();

                            User loginUser = userdetails.LoginbyUser(username, password);
                            if (loginUser != null)
                            {
                                loggedIn = true;
                                currentUser = loginUser;
                                Console.WriteLine($"User LOGIN successfully with Id {loginUser.UserID}.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to Login With User details provided.");
                            }
                            break;

                        case 2:
                            Console.WriteLine("Please Enter New Registration Credentials");
                            User addUser = new User();
                            bool isAddUser = userdetails.RegisterUser(addUser);
                            if (isAddUser)
                            {
                                Console.WriteLine("User Registered Successfully!!!!.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to add User.");
                            }
                            break;

                        case 0:
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Please select a valid option.");
                            break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("--------------- Get total Artwork List Start -----------------------");
                            List<Artwork> artworkList = virtualArtGallery.GetArtworkList();
                            //foreach (Artwork artwork in artworkList)
                            //{
                            //    Console.WriteLine($"ArtworkID: {artwork.ArtworkID}\n Title: {artwork.Title}\n Description: {artwork.Description}\n");
                            //}
                            Console.WriteLine("---------------------- Get total Artwork List END -------------------");
                            break;

                        case 2:
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

                        case 3:
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

                        case 4:
                            Console.WriteLine("----------------------- Update Artwork Start ------------------------");
                            Artwork updateArtwork = new Artwork();
                            Console.WriteLine("Artwork List:");
                            List<Artwork> artworkListupdate = virtualArtGallery.GetArtworkList();

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

                        case 5:
                            Console.WriteLine("----------------------- Remove Artwork By ID Start ------------------------");
                            Console.WriteLine("Artwork List:");
                            List<Artwork> artworkListremove = virtualArtGallery.GetArtworkList();
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

                        case 6:
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

                        case 7:
                            Console.WriteLine("-------- Add to Favorites Started ---------------------");

                            Console.Write("Enter the UserID: ");
                            int addUserId;
                            if (!int.TryParse(Console.ReadLine(), out addUserId))
                            {
                                Console.WriteLine("Invalid UserID. Please enter a valid number.");
                                break;
                            }

                            Console.WriteLine("Artwork List:");
                            List<Artwork> artworkLists = virtualArtGallery.GetArtworkList();

                            Console.Write("Enter the ArtworkID to Add to Favourites: ");
                            int addArtworkId;
                            if (!int.TryParse(Console.ReadLine(), out addArtworkId))
                            {
                                Console.WriteLine("Invalid ArtworkID. Please enter a valid number.");
                                break;
                            }

                            // Attempt to add the artwork to favorites
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

                        case 8:
                            Console.WriteLine("-------- Remove from Favorites Started ---------------------");
                            Console.Write("Enter the UserID: ");
                            int removeUserId = int.Parse(Console.ReadLine());

                            Console.WriteLine("Artwork List:");
                            List<Artwork> artworkListremovefav = virtualArtGallery.GetArtworkList();
                            Console.Write("Enter the ArtworkID To remove from favourites: ");
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

                        case 9:
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

                        case 10:
                            Console.WriteLine("-------- Add Artwork to Gallery Started ---------------------");
                            Console.WriteLine("Artwork List:");
                            List<Artwork> artworkListaddartworkgallery= virtualArtGallery.GetArtworkList();
                            Console.Write("Enter the Artwork ID to Add to Gallery: ");
                            int artworkId = int.Parse(Console.ReadLine());
                            Console.WriteLine("List of Galleries");
                            List<Gallery> galleryList = virtualArtGallery.GetGalleryList();
                            Console.Write("Enter the Gallery ID : ");
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

                        case 11:
                            Console.WriteLine("-------- Get  Artworks by Gallery Started ---------------------");
                            List<Gallery> ArtworkbygalleryList = virtualArtGallery.GetGalleryList();
                            Console.Write("Enter the GalleryID from the list of galleries: ");
                            int GalleryId = int.Parse(Console.ReadLine());
                            List<Artwork> getartworksbygallery = virtualArtGallery.GetArtworkByGallery(GalleryId);

                            if (getartworksbygallery.Count > 0)
                            {
                                Console.WriteLine(" Artworks By Gallery is------ :");
                                foreach (Artwork getArtworkgallery in getartworksbygallery)
                                {
                                    Console.WriteLine($"ArtworkID: {getArtworkgallery.ArtworkID}\n Title: {getArtworkgallery.Title}\n Description: {getArtworkgallery.Description}\n CreationDate: {getArtworkgallery.CreationDate}\n Medium: {getArtworkgallery.Medium}\n ImageURL: {getArtworkgallery.ImageURL}\n");
                                }
                            }
                            else
                            {
                                Console.WriteLine("This gallery has no  artworks.");
                            }
                            Console.WriteLine("-------- Get  Artworks to Gallery END ---------------------");
                            break;

                        case 0:
                            Console.WriteLine("Logout Starts");
                            userdetails.Logout();
                            loggedIn = false;
                            currentUser = null;
                            Console.WriteLine("Logged out successfully.");
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please select a valid option.");
                            break;
                    }
                }
            }
        }
    }
}
