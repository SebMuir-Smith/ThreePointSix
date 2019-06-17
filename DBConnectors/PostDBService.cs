using ThreePointSix.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace ThreePointSix.DBConnectors {
    public class PostDBService {

        // The database
        private readonly IMongoCollection<Post> _Posts;

        // Initialise the database connection object
        public PostDBService(IDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Posts = database.GetCollection<Post>(settings.CollectionName);
        }

        public List<Post> Get() =>
            _Posts.Find(Post => true).ToList();

        public Post Get(string id) =>
            _Posts.Find<Post>(Post => Post.Id == id).FirstOrDefault();

        public Post Create(Post Post) {
            _Posts.InsertOne(Post);
            return Post;
        }

        public void Update(string id, Post PostIn) =>
            _Posts.ReplaceOne(Post => Post.Id == id, PostIn);

        public void Remove(Post PostIn) =>
            _Posts.DeleteOne(Post => Post.Id == PostIn.Id);

        public void Remove(string id) =>
            _Posts.DeleteOne(Post => Post.Id == id);
    }
}