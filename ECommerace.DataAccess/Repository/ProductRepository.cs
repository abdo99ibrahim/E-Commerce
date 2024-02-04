using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db), IProductRepository

    {
        private ApplicationDbContext _db = db;
        public void Update(Product obj)
        {
            _db.products.Update(obj);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
