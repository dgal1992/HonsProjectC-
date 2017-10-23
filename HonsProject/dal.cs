using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using System.Configuration;
using MongoDB.Bson;


namespace HonsProject
{
    class dal 
    {
        //string which paths the mongodb server
        private string connectionString = "mongodb://localhost/?safe=true";

        //provides database name
        //provides collection within dbname
        private string dbName = "honsProject";
        private string collectionName = "honsData";

        // Default constructor.        
        public dal()
        {
        }
        
        public void DeleteData(string id)
        {
            //declares collection
            MongoCollection<getData> collection = GetDataCollection();
            // ObjectId called
            ObjectId i = new ObjectId(id.Replace("\"", ""));
            //query finds the id 
            var query = Query.EQ("_id", i);
            //removes the documents containing the ID
            collection.Remove(query);

        }
        
        // Creates a record and inserts it into the collection in MongoDB.
        public void CreateRecord(getData data)
        {
            //declares collection 
            MongoCollection<getData> collection = GetDataCollection();
            try
            {
                //inserts the new document in MongoDB database
                //.Insert = MongoCollection commmand
                collection.Insert(data);
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
            }
        }

        private MongoCollection<getData> GetDataCollection()
        {
            //creates a server connection variable
            MongoServer server = MongoServer.Create(connectionString);
            //creates a database variable from the server variable
            MongoDatabase database = server[dbName];
            //creates collection variable to append collection data
            MongoCollection<getData> DataCollection = database.GetCollection<getData>(collectionName);
            //returns the relevant data from the collection
            return DataCollection;
        }
                  
        internal List<getData> SearchByData(getData gd)
        {
            //declares MongoDB Collection
            MongoCollection<getData> collection = GetDataCollection();

            //ttt equals the collection queried and gets the relevant data  
            var ttt = (from p in collection.AsQueryable()
                       where p.username.ToUpper().Contains(gd.username.ToUpper())
                       select new // selects the data found
                       {
                           p.username,
                           p.Id,
                           p.latitude,
                           p.longitude,
                           p.imageUploaded,
                           p.lastComment
                       });
            //declares list to fill the data
            List<getData> list = new List<getData>();
            foreach (var t2 in ttt)
            {
                //adds to list the data equalling to the getData variables
                list.Add(new getData {
                    Id = t2.Id,
                    username = t2.username,
                    latitude = t2.latitude,
                    longitude = t2.longitude,
                    imageUploaded = t2.imageUploaded,
                    lastComment = t2.lastComment });
            }
            //returns the data in a list
            return list;
        }
    }
}

