using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace ThreePointSix.Models {
    public class Post {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<BsonDateTime> Dates { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string Message { get; set; }

        public List<string> Authors { get; set; }

        // Number of times post has been submitted
        public int SubmissionCount { get; set; }

        public Post() {
            // Set default values
            Dates = new List<BsonDateTime>();
            Authors = new List<string>();
            SubmissionCount = 1;
        }

        // Returns a sorted list of the most popular 10 posts
        // Only more efficient than just doing posts.sort[0..numToGet] if
        // numToGet << number of posts
        public static List<Post> GetTopXPosts(List<Post> posts, int numToGet) {

            // Not using a dictionary here due to inability to search easily
            int[] topXIndices = new int[numToGet];
            int[] topXValues = new int[numToGet];

            int minPopularity = Int32.MaxValue;
            // Pre-loop inits
            Post currentPost;
            int currentSubCount;

            // Get the top 10 indices and values
            for (int i = 0; i < posts.Count; i++) {

                currentPost = posts[i];
                currentSubCount = currentPost.SubmissionCount;

                // For the first numToGet posts, the index object is not yet full, so indices can be added
                // without checking
                if (i < numToGet) {
                    topXIndices[i] = i;
                    topXValues[i] = currentSubCount;

                    minPopularity = minPopularity < currentSubCount ? minPopularity : currentSubCount;

                    // Sort the array on the last addition
                    if (i == numToGet -1){
                        Array.Sort(topXValues, topXIndices);
                    }
                }
                else if (currentSubCount > minPopularity){
                    
                    // Remove lowest popularity entry and replace with new value
                    topXValues[0] = currentSubCount;
                    topXIndices[0] = i;

                    // Resort array
                    Array.Sort(topXValues, topXIndices);
                    minPopularity = topXValues[0];
                }
            }

            // Make new output array that contains only the top numToGet posts
            List<Post> postsOut = new List<Post>();

            for (int i = numToGet -1; i > -1; i--){
                postsOut.Add(posts[topXIndices[i]]);
            }

            return postsOut;
        }

    }
}