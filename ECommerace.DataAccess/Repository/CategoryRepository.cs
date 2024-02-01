using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;

namespace ECommerce.DataAccess.Repository;

public class CategoryRepository(ApplicationDbContext db) : Repository<Category>(db), ICategoryRepository
{
    private ApplicationDbContext _db = db;

    public void Update(Category obj)
    {
        _db.Categories.Update(obj);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}