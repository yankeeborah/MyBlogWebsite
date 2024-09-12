using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    //to pass a model from seperate repositories
    public class HomeViewModel
    {
        public IEnumerable<BlogPost> BlogPosts { get; set; }    
        public IEnumerable<Tag> Tags { get; set; }
    }
}
