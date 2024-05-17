using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art_gall.Model
{
    public class Gallery
    {
        public int GalleryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        // Foreign key to represent the curator
        public int ArtistID { get; set; }
        // Navigation property for one-to-many relationship with Artist
        public Artist Curator { get; set; }
        public string OpeningHours { get; set; }
        // Navigation property for many-to-many relationship with Artwork
        public ICollection<ArtworkGallery> ArtworkGalleries { get; set; }
    }
}
