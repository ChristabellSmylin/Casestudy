using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Art_gall.Model
{

    public class Artwork
    {
        public int ArtworkID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string Medium { get; set; }
        public string ImageURL { get; set; }
        // Navigation property for relationship with Artist
        public int ArtistID { get; set; }
        public Artist Artist { get; set; }
        // Navigation property for many-to-many relationship with Gallery
        public ICollection<ArtworkGallery> ArtworkGalleries { get; set; }
    }

}

