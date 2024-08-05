using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;

        //to use it with the get and post as we cannot use the object of Dbcontect directly bacuse of constructor scope
        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult SubmitTag(AddTagRequest addTagRequest)
        {
            //Mapping AddTagRequest to Tag domain Model
            var tag = new Tag()
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,    

            };
            bloggieDbContext.Tags.Add(tag);
            bloggieDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            //use DbContext to read the tags
            var tags = bloggieDbContext.Tags.ToList();

            return View(tags);
        }
        [HttpGet]
        public IActionResult Edit(Guid id)  //name ofparameter has to match with the route attribute in view
        {
            //1st method
            //var tag = bloggieDbContext.Tags.Find(id);

            //2nd method
            var tag = bloggieDbContext.Tags.FirstOrDefault(x=>x.Id== id);
            if (tag != null) 
            {
                var editTagRequest = new EditTagRequest()
                {
                    Id = id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(null);// when tag is not found
        }

        [HttpPost]
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag()
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            var existingTag = bloggieDbContext.Tags.Find(tag.Id);
            if (existingTag != null) 
            { 
                existingTag.Name = tag.Name;    
                existingTag.DisplayName = tag.DisplayName;

                bloggieDbContext.SaveChanges();//save changes
                return RedirectToAction("List");//show successful notification
            }
            return RedirectToAction("Edit",new {id=editTagRequest.Id});//show failure notification
        }

        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var tag = bloggieDbContext.Tags.Find(editTagRequest.Id);
            if (tag != null)
            {
                bloggieDbContext.Tags.Remove(tag);//Remove from the Tags table

                bloggieDbContext.SaveChanges();//save changes
                return RedirectToAction("List");//show successful notification
            }
            //show an error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });//show failure notification
        }
    }
}
