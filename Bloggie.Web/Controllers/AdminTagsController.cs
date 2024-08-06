using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Add()      //no need to make async as not doing any I/O
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            //Mapping AddTagRequest to Tag domain Model
            var tag = new Tag()
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,    

            };
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            //use DbContext to read the tags
            var tags = await bloggieDbContext.Tags.ToListAsync();

            return View(tags);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)  //name ofparameter has to match with the route attribute in view
        {
            //1st method
            //var tag = bloggieDbContext.Tags.Find(id);

            //2nd method
            var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x=>x.Id== id);
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
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag()
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);
            if (existingTag != null) 
            { 
                existingTag.Name = tag.Name;    
                existingTag.DisplayName = tag.DisplayName;

                await bloggieDbContext.SaveChangesAsync();//save changes
                return RedirectToAction("List");//show successful notification
            }
            return RedirectToAction("Edit",new {id=editTagRequest.Id});//show failure notification
        }

        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await bloggieDbContext.Tags.FindAsync(editTagRequest.Id);
            if (tag != null)
            {
                bloggieDbContext.Tags.Remove(tag);//Remove from the Tags table

                await bloggieDbContext.SaveChangesAsync();//save changes
                return RedirectToAction("List");//show successful notification
            }
            //show an error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });//show failure notification
        }
    }
}
