using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace HonsProject
{
    
    class getData
    {
            //gets and sets the unique id within the BSON document
            [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
            public ObjectId Id { get; set; }
            //gets and sets the username field within the BSON document
            [BsonElement("username")]
            public string username { get; set; }
            //gets and sets the latitude field within the BSON document 
            [BsonElement("latitude")]
            public double latitude { get; set; }
            //gets and sets the longitude field within the BSON document
            [BsonElement("longitude")]
            public double longitude { get; set; }
            //gets and sets the image_uploaded field within the BSON document
            [BsonElement("image_uploaded")]
            public string imageUploaded { get; set; }
            //gets and sets the last_comment field within the BSON document
            [BsonElement("last_comment")]
            public string lastComment { get; set; }
       
    }
}
