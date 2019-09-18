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
    public class Users : IResource
    {
        private readonly ApplicationDbContext _dbcontext;

        public Users(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
        }

        public async Task CreateAsync(IDictionary<string, object> formData)
        {
            var temp = new Person(formData["Sub"] as string);
            temp.Name = formData["Name"] as string;
            var tennant = await _dbcontext.ArkDefence_Tennant.FindAsync(formData["TennantId"] as string);
            if (tennant != null)
            {
                temp.Tennant = tennant;
            }
            else
            {
                throw new ArgumentNullException(nameof(tennant));
            }
            var uri = formData["ImageUri"] as string;
            if (!String.IsNullOrEmpty(uri))
            {
                temp.ImageUri = uri;
            }
            _dbcontext.Add(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<string> formData)
        {
            var collection = _dbcontext.ArkDefence_Users.Where(t => formData.Contains(t.Id.ToString()));
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
                    Readonly.ForProperty("Id"),
                    Text.ForProperty("Sub"),
                    Text.ForProperty("TennantId"),
                    Text.ForProperty("Name"),
                    Text.ForProperty("ImageUri"),
                    Boolean.ForProperty("Deleted")
                }),
                //new Section("Contacts", new IField[]
                //{
                //    Text.ForProperty("Email"),
                //    Text.ForProperty("Phone"),
                //}),
                new Section("Time",new IField[]
                {
                    Readonly.ForProperty("CreationTime"),
                    Readonly.ForProperty("DeletionTime")
                })
            };
        }

        public async Task<object> FindAsync(string entityId)
        {
            return await _dbcontext.ArkDefence_Users.FindAsync(long.Parse(entityId));
        }

        public IQueryable<object> Select(string filter)
        {
            return this._dbcontext.ArkDefence_Users.Select(t => new
            {
                t.Id,
                t.Sub,
                t.Name,
                t.Deleted
            }).Where(t => string.IsNullOrEmpty(filter) || t.Name.Contains(filter) || t.Sub.Contains(filter) || t.Deleted == bool.Parse(filter));
        }

        public async Task UpdateAsync(IDictionary<string, object> formData)
        {
            var id = long.Parse(formData["Id"] as string);

            var temp = await _dbcontext.ArkDefence_Users.FindAsync(id);
            temp.Sub = formData["Sub"] as string;
            var image = formData["ImageUri"] as string;
            if (!String.IsNullOrEmpty(image))
            {
                temp.ImageUri = image;
            }
            temp.Deleted = bool.Parse(formData["Deleted"].ToString());
            temp.Name = formData["Name"] as string;

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
