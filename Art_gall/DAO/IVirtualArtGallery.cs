using Art_gall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art_gall.DAO
{
  

    public interface IVirtualArtGallery
    {
        List<Artwork> GetArtworkList();
        bool AddArtwork(Artwork artwork);
        bool UpdateArtwork(Artwork artwork);
        bool RemoveArtwork(int artworkID);
        Artwork GetArtworkById(int artworkID);
        List<Artwork> SearchArtworks(string keyword);
        bool AddArtworkToFavorite(int userId, int artworkId);
        bool RemoveArtworkFromFavorite(int userId, int artworkId);
        List<Artwork> GetUserFavoriteArtworks(int userId);
        bool AddArtworktoGallery(int artworkId, int galleryId);
        List<Artwork> GetUserFavoriteArtworkGallery(int artworkId);


    }
   
}
