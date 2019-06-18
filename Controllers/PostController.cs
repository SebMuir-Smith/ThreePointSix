using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ThreePointSix.DBConnectors;
using ThreePointSix.Models;

namespace ThreePointSix.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase {

        // Interface and instance of the database connection service
        private readonly PostDBService _PostDBService;

        public PostController(PostDBService service) {
            _PostDBService = service;
        }

        // GET api/Posts
        // Returns <11 top posts
        [HttpGet]
        public ActionResult<List<Post>> Get() {
            // Pre-Sorted post list
            List<Post> allPosts = _PostDBService.Get();

            // Only return top 10 or less posts
            int numberOfPostsToGet = Math.Min(allPosts.Count,10);

            return Models.Post.GetTopXPosts(allPosts,numberOfPostsToGet);
        }

        // POST api/values
        // Creates a new post with the given post JSON object
        [HttpPost]
        public void Post([FromBody] Post postIn) {

            // Pre-Sorted post list
            List<Post> allPosts = _PostDBService.Get();

            // Extract messages and convert to lowercase
            List<string> allPostMessages = allPosts.ConvertAll(post => post.Message.ToLower());

            // Check if message allready exists
            int foundIndex = allPostMessages.BinarySearch(postIn.Message.ToLower());

            // If not found then make new post object
            Post postOut;
            if (foundIndex == -1) {
                // Deep clone the old object's message
                postOut = new Post();
                postOut.Message = postIn.Message;
            } else {
                // Use the prexisting post object if found
                postOut = allPosts[foundIndex];
                postOut.SubmissionCount++;
            }

            postOut.Dates.Add(new BsonDateTime(DateTime.Now));
            // First index of the input array is the name of message sender
            postOut.Authors.Add(postIn.Authors[0]);

            // Update the previous post entry if the message allready exists
            if (foundIndex == -1){
            _PostDBService.Create(postOut);
            }
            else {
                _PostDBService.Update(postOut.Id,postOut);
            }
        }

    }
}