using Art_gall.DAO;
using Art_gall.Model;
using static Art_gall.Exceptions.ExceptionHandling;

namespace ArtManagementTestProject1
{
    public class TestClass
    {

        private ArtworkAnalysisServiceImpl crimeAnalysisService;
        [SetUp]
        public void Setup()
        {
            crimeAnalysisService = new ArtworkAnalysisServiceImpl();

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test()]
        public void GetArtworkList_Test()
        {
          
            List<Artwork> artworkList = crimeAnalysisService.GetArtworkList();

            // It verifies that the artworkList variable is not null.
            // Assert
            Assert.IsNotNull(artworkList);
        }


        public void AddArtwork_Test()
        {

            var artworkManagementTests = new Tests();


            var newArtwork = new Artwork();

            artworkManagementTests.Test_GetArtworkDetailsFromUser_Input();

            // Act
            var result = crimeAnalysisService.AddArtwork(newArtwork);

            // Assert
            Assert.IsTrue(result);
            
        }


        [Test]
        public void RemoveArtwork_Test()
        {
            // Arrange
            int nonExistentArtworkID = 10; 

            // Act & Assert
            Assert.Throws<ArtWorkNotFoundException>(() => crimeAnalysisService.RemoveArtwork(nonExistentArtworkID),
                "Expected RemoveArtwork to throw ArtWorkNotFoundException for non-existent artwork ID.");

        }

        [Test]
         public void SearchArtworksTest()
        {
            string keyword = "nature";
            var searchResults = crimeAnalysisService.SearchArtworks(keyword);

            // Assert
            Assert.IsNotNull(searchResults); // Assert that the result is not null
            Assert.Greater(searchResults.Count, 0);//It verifies that the count of  searchResults list is greater than 0
        }

        

    }
}
