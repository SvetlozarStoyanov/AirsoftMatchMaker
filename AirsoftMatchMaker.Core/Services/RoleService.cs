﻿using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Roles;
using AirsoftMatchMaker.Infrastructure.Data;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> roleManager;
        public RoleService(RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<RoleListModel>> GetRequestableRolesAsync(IEnumerable<string> roleNames)
        {
            var roles = await roleManager.Roles
                .Where(r => !roleNames.Contains(r.Name) && r.Name != "Administrator")
                .Select(r => new RoleListModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                }).ToListAsync();

            return roles;
        }

        public async Task<IEnumerable<RoleListModel>> GetUserRolesAsync(IEnumerable<string> roleNames)
        {
            var roles = await roleManager.Roles
                .Where(r => roleNames.Contains(r.Name))
                .Select(r => new RoleListModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                }).ToListAsync();

            return roles;

        }
    }
}
