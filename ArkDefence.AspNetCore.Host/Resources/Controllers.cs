using ArkDefence.AspNetCore.Host.Data;
using Coravel.Pro.Features.Resources.Fields;
using Coravel.Pro.Features.Resources.Interfaces;
using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boolean = Coravel.Pro.Features.Resources.Fields.Boolean;

namespace ArkDefence.AspNetCore.Host.Resources
{
    public class Controllers : IResource
    {
        private readonly ApplicationDbContext _dbcontext;

        public Controllers(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
        }

        public async Task CreateAsync(IDictionary<string, object> formData)
        {
            var temp = new SystemController(formData["Id"] as string);
            temp.ClientSecret = formData["ClientSecret"] as string;
            temp.Alias = formData["Alias"] as string;
            temp.Version = formData["Version"] as string;
            var tennant = await _dbcontext.ArkDefence_Tennant.FindAsync(formData["TennantId"] as string);
            if (tennant != null)
            {
                temp.Tennant = tennant;
            }
            else
            {
                throw new ArgumentNullException(nameof(tennant));
            }
            _dbcontext.Add(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<string> formData)
        {
            var collection = _dbcontext.ArkDefence_SystemController.Where(t => formData.Contains(t.Id));
            await collection.ForEachAsync(t =>
            {
                t.SoftDelete();
            });
            _dbcontext.EnsureAutoHistory();
            await this._dbcontext.SaveChangesAsync();
        }

        public IEnumerable<IField> Fields()
        {
            return new IField[]
           {
                new Section("Basics", new IField[]
                {
                    Text.ForProperty("Id"),
                    Text.ForProperty("ClientSecret"),
                    Text.ForProperty("Alias"),
                    Boolean.ForProperty("Deleted"),
                    Text.ForProperty("Version"),
                    Text.ForProperty("TennantId")
                }),
                new Section("Time",new IField[]
                {
                    Readonly.ForProperty("CreationTime"),
                    Readonly.ForProperty("DeletionTime")
                })
           };
        }

        public async Task<object> FindAsync(string entityId)
        {
            return await _dbcontext.ArkDefence_SystemController.FindAsync(entityId);
        }

        public IQueryable<object> Select(string filter)
        {
            return this._dbcontext.ArkDefence_SystemController.Select(t => new
            {
                t.Id,
                t.Alias,
                t.Deleted
            }).Where(t => string.IsNullOrEmpty(filter) || t.Alias.Contains(filter) || t.Deleted == bool.Parse(filter));
        }

        public async Task UpdateAsync(IDictionary<string, object> formData)
        {
            var id = formData["Id"] as string;
            var temp = await _dbcontext.ArkDefence_SystemController.FindAsync(id);
            temp.ClientSecret = formData["ClientSecret"] as string;
            temp.Alias = formData["Alias"] as string;
            temp.Version = formData["Version"] as string;
            temp.Deleted = bool.Parse(formData["Deleted"].ToString());
            var tennant = await _dbcontext.ArkDefence_Tennant.FindAsync(formData["TennantId"] as string);
            if (tennant != null)
            {
                temp.Tennant = tennant;
            }
            else
            {
                throw new ArgumentNullException(nameof(tennant));
            }
            _dbcontext.Update(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }
    }
}
