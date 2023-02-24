using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace SocialNetworkBE.Controllers
{
    public class CommentController : ApiController
    {
        // GET: Comment
        List<Comment> comments = new List<Comment>();

        public CommentController() { }

        public CommentController(List<Comment> comments)
        {
            this.comments = comments;
        }

        public IEnumerable<Comment> GetAllProducts()
        {
            return comments;
        }

        public async Task<IEnumerable<Comment>> GetAllProductsAsync()
        {
            return await Task.FromResult(GetAllProducts());
        }
    }
}