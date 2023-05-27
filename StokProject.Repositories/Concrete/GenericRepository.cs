using Microsoft.EntityFrameworkCore;
using StokProject.Entities.Entities;
using StokProject.Repositories.Abstract;
using StokProject.Repositories.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StokProject.Repositories.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StokProjectContext _context;

        public GenericRepository(StokProjectContext context)
        {
            _context = context;
        }
        public bool Add(T item)
        {
            try
            {
                item.AddedDate = DateTime.Now;
                _context.Set<T>().Add(item);
                return Save() > 0;  //bir tek nesne gelip eklenme işlemi yapılacağı için
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //_context.Set<T>().AddRange(items);
                    foreach (T item in items)
                    {
                        item.AddedDate = DateTime.Now;
                        _context.Set<T>().Add(item);
                    }
                    scope.Complete();

                    return Save() > 0; //1 veya daha fazla satır etkileniyorsa true dönecek
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Update(T item)
        {
            try
            {
                item.ModifiedDate = DateTime.Now;
                _context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {

                return false;
            }           
        }

        public bool Remove(T item)
        {
            try
            {
                item.IsActive = false;
                _context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Remove(int id)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    T item = GetByID(id);
                    item.IsActive = false;
                    scope.Complete();
                    return Update(item);
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var collection = GetDefault(exp); //verilen ifadeye göre verilen nesneleri collection'a atıyoruz
                    int counter = 0;

                    foreach (var item in collection)
                    {
                        item.IsActive = false;
                        bool opResult = Update(item); //db'den silmiyoruz. Durumu eaktif olarak işaretliyoruz. Bunu da Update() metodu ile gerçekleştiriyoruz. İşlem sonucunu da opResult'ta tutuyoruz(Update işlemi sonucu true ya da false)
                        if (opResult) counter++; //eğer ilgili item'ın güncellenme işlemi gerçeklşti ise sayacı bir arttırıyoruz. Sonrasında foreach kendi sayacı (iterasyonu) sayesinde bir sonraki item'a geçiyor.
                    }
                    if (collection.Count == counter)
                    {
                        scope.Complete(); //koleksiyondaki eleman sayısı ile silme işlemi gerçekleşen eleman sayısı eşit ise bu işlem tamamen başarılıdır.
                        return true; // başarılı olduğu için true döndür.
                    }
                    else
                    {
                        scope.Dispose(); // aksi halde bu scope dispose et
                        return false; // başarsız olduğu için false döndür.
                    }
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public T GetByID(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public IQueryable<T> GetByID(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.ID == id);
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().FirstOrDefault(exp);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().Where(exp).ToList();
        }

        public List<T> GetActive()
        {
            return _context.Set<T>().Where(x=>x.IsActive == true).ToList();
        }

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.IsActive == true);
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(exp);
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
      
        public bool Any(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().Any(exp);
        }

        public bool Activate(int id)
        {
            T item = GetByID(id);
            item.IsActive= true;
            return Update(item);
        }
        public int Save()
        {
            return _context.SaveChanges();
        }
        public void DetachtEntity(T item)
        {
            _context.Entry<T>(item).State = EntityState.Detached; //bir entryi (item) takip etmeyi bırakmak için kullanılır.
        }
    }
}
