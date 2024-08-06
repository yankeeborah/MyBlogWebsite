using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();

        Task<Tag?> GetAsync(Guid id);

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag); //Tag can be a Tag or null if it diesnt find one

        Task<Tag?> DeleteAsync(Guid id); // works on a id and if doesnt find one then it will retirn null

    }
}
