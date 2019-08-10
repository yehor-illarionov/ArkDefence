using ArkDefence.AspNetCore.Host.Data;
using ArkDefence.AspNetCore.Host.Models;
using Coravel.Pro.Features.Resources.Fields;
using Coravel.Pro.Features.Resources.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boolean = Coravel.Pro.Features.Resources.Fields.Boolean;

namespace ArkDefence.AspNetCore.Host.Resources
{
    public class Cards : IResource
    {
        private readonly ApplicationDbContext _dbcontext;

        public Cards(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
        }

        public async Task CreateAsync(IDictionary<string, object> formData)
        {
            var temp = new Card(formData["Id"] as string);
            var person = await _dbcontext.ArkDefence_Users.FindAsync(long.Parse(formData["PersonId"] as string));
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            temp.Person = person;
            _dbcontext.Add(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<string> formData)
        {
            var collection = _dbcontext.ArkDefence_Cards.Where(t => formData.Contains(t.Id));
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
                    Text.ForProperty("PersonId"),
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
            return await _dbcontext.ArkDefence_Cards.FindAsync(entityId);
        }

        public IQueryable<object> Select(string filter)
        {
            return this._dbcontext.ArkDefence_Cards.Select(t => new
            {
                t.Id,
                t.PersonId,
                t.Deleted
            }).Where(t => string.IsNullOrEmpty(filter) || t.PersonId.ToString().Contains(filter) || t.Deleted == bool.Parse(filter));
        }

        public async Task UpdateAsync(IDictionary<string, object> formData)
        {
            var id = formData["Id"] as string;
            var temp = await _dbcontext.ArkDefence_Cards.FindAsync(id);
            var person = await _dbcontext.ArkDefence_Users.FindAsync(long.Parse(formData["PersonId"] as string));
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            temp.Person = person;
            temp.Deleted = bool.Parse(formData["Deleted"].ToString());
            _dbcontext.Update(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }
    }
}
