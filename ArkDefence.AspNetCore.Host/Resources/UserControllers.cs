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
    public class UserControllers : IResource
    {
        private readonly ApplicationDbContext _dbcontext;

        public UserControllers(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
        }

        public async Task CreateAsync(IDictionary<string, object> formData)
        {
            var temp = new PersonSystemController();
            var person = await _dbcontext.ArkDefence_Users.FindAsync(long.Parse(formData["PersonId"] as string));
            if (person == null) {
                throw new ArgumentNullException(nameof(person));
            }
            temp.Person = person;
            var controller= await _dbcontext.ArkDefence_SystemController.FindAsync(formData["ControllerId"] as string);
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }
            temp.SystemController = controller;
            _dbcontext.Add(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<string> formData)
        {
            this._dbcontext.ArkDefence_PersonSystemController.RemoveRange(
                      formData.Select(id => new PersonSystemController() { Id = int.Parse(id) })
            );
            await this._dbcontext.SaveChangesAsync();
        }

        public IEnumerable<IField> Fields()
        {
            return new IField[]
            {
                new Section("Basics", new IField[]
                {
                    Readonly.ForProperty("Id"),
                    Text.ForProperty("PersonId"),
                    Text.ForProperty("ControllerId"),
                    //Text.ForProperty("TennantId"),
                    //Text.ForProperty("Name"),
                    //Text.ForProperty("ImageUri"),
                    //Boolean.ForProperty("Deleted")
                }),
                //new Section("Time",new IField[]
                //{
                //    Readonly.ForProperty("CreationTime"),
                //    Readonly.ForProperty("DeletionTime")
                //})
            };
        }

        public async Task<object> FindAsync(string entityId)
        {
            return await _dbcontext.ArkDefence_PersonSystemController.FindAsync(long.Parse(entityId));
        }

        public IQueryable<object> Select(string filter)
        {
            return this._dbcontext.ArkDefence_PersonSystemController.Select(t => new
            {
                t.Id,
                t.PersonId,
                t.SystemControllerId
            }).Where(t => string.IsNullOrEmpty(filter) || t.PersonId.ToString().Contains(filter) || t.SystemControllerId.Contains(filter));
        }

        public async Task UpdateAsync(IDictionary<string, object> formData)
        {
            var id = long.Parse(formData["Id"] as string);
            var temp = await _dbcontext.ArkDefence_PersonSystemController.FindAsync(id);
            var person = await _dbcontext.ArkDefence_Users.FindAsync(long.Parse(formData["PersonId"] as string));
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }
            temp.Person = person;
            var controller = await _dbcontext.ArkDefence_SystemController.FindAsync(formData["ControllerId"] as string);
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }
            temp.SystemController = controller;
            _dbcontext.Update(temp);
            _dbcontext.EnsureAutoHistory();
            await _dbcontext.SaveChangesAsync();
        }
    }
}
