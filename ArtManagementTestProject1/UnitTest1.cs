using Art_gall.Model;
using Art_gall.Service;

namespace ArtManagementTestProject1
{

    [TestFixture]
    public class Tests
    {

        [Test]


        public void Test_GetArtworkDetailsFromUser_Input()
        {
            // Arrange
            Artwork artwork = new Artwork();
            string title = "Test Title";
            string description = "Test Description";
            DateTime creationDate = new DateTime(2022, 05, 05);
            string medium = "Test Medium";
            string imageURL = "Test Image URL";

            // Act
            using (var sw = new System.IO.StringWriter())
            {
                //redirects the standard output stream (console output) to the StringWriter
                Console.SetOut(sw);
                //This line redirects the standard input stream (console input) to a StringReader
                Console.SetIn(new System.IO.StringReader($"{title}\n{description}\n{creationDate.ToString("yyyy-MM-dd")}\n{medium}\n{imageURL}\n"));

                ArtworkManagement.GetArtworkDetailsFromUser(artwork);

                string expectedOutput = $"Enter artwork details:{Environment.NewLine}" +
                                        $"Title: Description: Creation Date (YYYY-MM-DD): Medium: Image URL: ";
                Assert.AreEqual(expectedOutput, sw.ToString());

                // Assert
                Assert.AreEqual(title, artwork.Title);
                Assert.AreEqual(description, artwork.Description);
                Assert.AreEqual(creationDate, artwork.CreationDate);
                Assert.AreEqual(medium, artwork.Medium);
                Assert.AreEqual(imageURL, artwork.ImageURL);
            }
        }
        [Test]
       
        public void UpdateArtworkDetailsFromUser_InputValidData()
        {
            // Arrange
            Artwork artwork = new Artwork();
            int artworkID = 1;
            string title = "Title";
            string description = "Description";
            DateTime creationDate = new DateTime(2022, 05, 05);
            string medium = "Updated Test Medium";
            string imageURL = "Updated Test Image URL";

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Console.SetIn(new StringReader($"{artworkID}\n{title}\n{description}\n{creationDate:yyyy-MM-dd}\n{medium}\n{imageURL}\n"));

                ArtworkManagement.UpdateArtworkDetailsFromUser(artwork);

                string expectedOutput = $"Enter The artwork ID which you want to get updated {Environment.NewLine}" +
                                        $"ArtworkID: Enter updated artwork details:{Environment.NewLine}" +
                                        $"Title: Description: Creation Date (YYYY-MM-DD): Medium: Image URL: ";

                // Normalize newlines
                string actualOutput = sw.ToString().Replace("\r\n", "\n");
                expectedOutput = expectedOutput.Replace("\r\n", "\n");

                Assert.AreEqual(expectedOutput, actualOutput);

                // Assert
                Assert.AreEqual(artworkID, artwork.ArtworkID);
                Assert.AreEqual(title, artwork.Title);
                Assert.AreEqual(description, artwork.Description);
                Assert.AreEqual(creationDate, artwork.CreationDate);
                Assert.AreEqual(medium, artwork.Medium);
                Assert.AreEqual(imageURL, artwork.ImageURL);
            }
        }
    }
}