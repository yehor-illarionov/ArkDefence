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
    public class Tennants : IResource
    {
        private readonly ApplicationDbContext _dbcontext;

        public Tennants(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
        }

        public async Task CreateAsync(IDictionary<string, object> formData)
        {
            var temp = new Tennant(formData["Id"] as string);
            temp.Alias = formData["Alias"] as string;
            var email = formData["Email"] as string;
            if (!String.IsNullOrEmpty(email))
            {
                temp.Email = email;
            }
            var phone = formData["Phone"] as string;
            if (!String.IsNullOrEmpty(phone))
            {
                temp.Phone = phone;
            }
            _dbcontext.Add(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<string> formData)
        {
            //this._dbcontext.ArkDefence_Tennant.RemoveRange(
            //    formData.Select(id => new Tennant(id)));
            var collection = _dbcontext.ArkDefence_Tennant.Where(t => formData.Contains(t.Id));
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
                    Boolean.ForProperty("Deleted")
                }),
                new Section("Contacts", new IField[]
                {
                    Text.ForProperty("Email"),
                    Text.ForProperty("Phone"),
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
            return await _dbcontext.ArkDefence_Tennant.FindAsync(entityId);
        }

        public IQueryable<object> Select(string filter)
        {
            return this._dbcontext.ArkDefence_Tennant.Select(t => new
            {
                t.Id,
                t.Alias,
                t.Email,
                t.Phone,
                t.Deleted
            }).Where(t => string.IsNullOrEmpty(filter) || t.Alias.Contains(filter) || t.Email.Contains(filter) || t.Deleted== bool.Parse(filter));
        }

        public async Task UpdateAsync(IDictionary<string, object> formData)
        {
            var id = formData["Id"] as string;

            var temp = await _dbcontext.ArkDefence_Tennant.FindAsync(id);
            temp.Alias = formData["Alias"] as string;
            var email = formData["Email"] as string;
            if (!String.IsNullOrEmpty(email))
            {
                temp.Email = email;
            }
            temp.Deleted = bool.Parse(formData["Deleted"].ToString());
            var phone = formData["Phone"] as string;
            if (!String.IsNullOrEmpty(phone))
            {
                temp.Phone = phone;
            }
            _dbcontext.Update(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }
    }
}
