using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public class Entity<TKey>: ISoftDelete, ICreationTime
    {
        public Entity()
        {
            CreationTime = DateTime.UtcNow;
            Deleted = false;
        }

        /// <summary>
        /// throws ArgumentNullException if id is null
        /// </summary>
        /// <param name="id"></param>
        public Entity(TKey id)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            Id = id;
            CreationTime = DateTime.UtcNow;
            Deleted = false;
        }

        [Key]
        public TKey Id { get; set; }
        public DateTime CreationTime { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletionTime { get; set; }

        public void SoftDelete()
        {
            this.Deleted = true;
            this.DeletionTime = DateTime.UtcNow;
        }
    }

    public interface IAsyncRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        Task<TEntity> GetById(TKey id);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Remove(TEntity entity);

        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate);

        Task<ICollection<TEntity>> GetPage(int page, int count);
        Task<(int totalCount, int totalPages)> GetTotalPages(int count);

        Task<TKey> CountAll();
        Task<TKey> CountWhere(Expression<Func<TEntity, bool>> predicate);
    }
}
