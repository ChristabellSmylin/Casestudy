using Art_gall.DAO;
using Art_gall.Model;

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
            //var artworkList = crimeAnalysisService.GetArtworkList();

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
            // You can add more assertions based on your specific requirements
        }


        [Test]
        public void RemoveArtwork_Test()
        {
            // Arrange
            int artworkIdToRemove = 7; // Provide an artwork ID to remove

            // Act
            var result = crimeAnalysisService.RemoveArtwork(artworkIdToRemove);

            // Assert
            Assert.IsTrue(result);

        }

        [Test]
         public void SearchArtworksTest()
        {
            string keyword = "nature";
            var searchResults = crimeAnalysisService.SearchArtworks(keyword);

            // Assert
            Assert.IsNotNull(searchResults); // Assert that the result is not null
            Assert.Greater(searchResults.Count, 0);
        }

        

    }
}
