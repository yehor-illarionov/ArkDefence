using ArkDefence.AspNetCore.Host.Data;
using Coravel.Pro.Features.Resources.Fields;
using Coravel.Pro.Features.Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkDefence.AspNetCore.Host.Resources
{
    public class UserControllers : IResource
    {
        private readonly ApplicationDbContext _dbcontext;

        public UserControllers(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
        }

        public Task CreateAsync(IDictionary<string, object> formData)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IEnumerable<string> formData)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IField> Fields()
        {
            throw new NotImplementedException();
        }

        public Task<object> FindAsync(string entityId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<object> Select(string filter)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IDictionary<string, object> formData)
        {
            throw new NotImplementedException();
        }
    }
}
