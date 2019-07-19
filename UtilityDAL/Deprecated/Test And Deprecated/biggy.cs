//using Biggy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UtilityDAL
{
    public class ArtistDocument
    {
        public ArtistDocument()
        {
            this.Albums = new List<Album>();
        }
        public int ArtistDocumentId { get; set; }
        public string Name { get; set; }
        public List<Album> Albums;
    }

    public partial class Album
    {
        public Album()
        {
            //this.Tracks = new HashSet<Track>();
        }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
    }

    public partial class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
    }

    //https://github.com/xivSolutions/biggy
    //public class biggy
    //{

    //    public void AddExample()
    //    {

    //        var store = new Biggy.Data.Json.JsonStore<Artist>("TestDb");
    //        var artists = new BiggyList<Artist>(store);

    //        artists.Add(new Artist { ArtistId = 1, Name = "The Wipers" });
    //        artists.Add(new Artist { ArtistId = 2, Name = "The Fastbacks" });

    //        foreach (var artist in artists)
    //        {
    //            Console.WriteLine("Id: {0} Name: {1}", artist.ArtistId, artist.Name);
    //        }


    //    }

    //    public void JsonAddExample()
    //    {

    //        var store = new Biggy.Data.Json.JsonStore<Artist>("TestDb");
    //        var artists = new BiggyList<Artist>(store);

    //        artists.Add(new Artist { ArtistId = 1, Name = "The Wipers" });
    //        artists.Add(new Artist { ArtistId = 2, Name = "The Fastbacks" });

    //        foreach (var artist in artists)
    //        {
    //            Console.WriteLine("Id: {0} Name: {1}", artist.ArtistId, artist.Name);
    //        }


    //    }

    //    public void SqliteAddExample()
    //    {

    //        var store = new Biggy.Data.Sqlite.SqliteDocumentStore<ArtistDocument>("TestDb");

    //        // This will create a table named artistdocuments in TestDb if one doesn't already exist:
    //        var artistDocs = new BiggyList<ArtistDocument>(store);

    //        var newArtist = new ArtistDocument { ArtistDocumentId = 1, Name = "Metallica" };
    //        newArtist.Albums.Add(new Album { AlbumId = 1, ArtistId = newArtist.ArtistDocumentId, Title = "Kill 'Em All" });
    //        newArtist.Albums.Add(new Album { AlbumId = 2, ArtistId = newArtist.ArtistDocumentId, Title = "Ride the Lightning" });

    //        // This will add a record to the artistdocuments table:
    //        artistDocs.Add(newArtist);

    //    }





    //    public void UpdateExample()
    //    {
    //        var store = new Biggy.Data.Json.JsonStore<ArtistDocument>("TestDb2");
    //        // This immediately load any existing artist documents from the json file:
    //        var artists = new BiggyList<ArtistDocument>(store);

    //        // This query never hits the disk - it uses LINQ directly in memory:
    //        var someArtist = artists.FirstOrDefault(a => a.Name == "Nirvana");
    //        var someAlbum = someArtist.Albums.FirstOrDefault(a => a.Title.Contains("Incest"));

    //        // Update:
    //        someAlbum.Title = "In Utero";

    //        //this writes to disk in a single flush - so it's fast too
    //        artists.Update(someArtist);
    //    }
    //}
}
