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
    public class Terminals : IResource
    {
        private readonly ApplicationDbContext _dbcontext;

        public Terminals(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
        }

        public async Task CreateAsync(IDictionary<string, object> formData)
        {
            var temp = new Terminal(formData["Id"] as string);
            temp.Alias = formData["Alias"] as string;
            var controller = await _dbcontext.ArkDefence_SystemController.FindAsync(formData["SystemControllerId"] as string);
            if (controller != null)
            {
                temp.SystemController = controller;
            }
            else
            {
                throw new ArgumentNullException(nameof(controller));
            }
            temp.Port = formData["Port"] as string;
            _dbcontext.Add(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<string> formData)
        {
            var collection = _dbcontext.ArkDefence_Terminals.Where(t => formData.Contains(t.Id));
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
                    Text.ForProperty("Alias"),
                    Text.ForProperty("SystemControllerId"),
                    Text.ForProperty("Port"),
                  //  Text.ForProperty("ImageUri"),
                    Boolean.ForProperty("IsDatabaseFull"),
                    Boolean.ForProperty("Deleted")
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
            return await _dbcontext.ArkDefence_Terminals.FindAsync(entityId);
        }

        public IQueryable<object> Select(string filter)
        {
            return this._dbcontext.ArkDefence_Terminals.Select(t => new
            {
                t.Id,
                t.Alias,
                t.IsDatabaseFull,
                t.Deleted
            }).Where(t => string.IsNullOrEmpty(filter) || t.Alias.Contains(filter) || t.IsDatabaseFull == bool.Parse(filter) || t.Deleted == bool.Parse(filter));
        }

        public async Task UpdateAsync(IDictionary<string, object> formData)
        {
            var id = formData["Id"] as string;
            var temp = await _dbcontext.ArkDefence_Terminals.FindAsync(id);
            if (temp == null)
            {
                throw new ArgumentNullException(nameof(temp));
            }
            temp.Alias = formData["Alias"] as string;
            var controller = await _dbcontext.ArkDefence_SystemController.FindAsync(formData["SystemControllerId"] as string);
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }
          
            temp.SystemController = controller;
            temp.Port = formData["Port"] as string;
            temp.IsDatabaseFull = bool.Parse(formData["IsDatabaseFull"].ToString());
            temp.Deleted = bool.Parse(formData["Deleted"].ToString());
            _dbcontext.Update(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }
    }
}
