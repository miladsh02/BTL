using BTL.Data;
using Domain.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BTL.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public UsersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> UsersRolePanel()
        {
            var usersDto = await (
                from u in _dbContext.Users
                join s in _dbContext.Students on u.Id equals s.UserId
                join ur in _dbContext.UserRoles on u.Id equals ur.UserId
                join r in _dbContext.Roles on ur.RoleId equals r.Id 
                    select  new UserRoleDto()
                    {
                        UserId = u.Id,
                        RoleName = r.Name,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        UniversityId = s.UniversityId,
                        NationalCode = s.NationalCode 
                    }).ToListAsync();

            return View(usersDto);
        }
        
        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> RolesList()
        {
            var users = await _dbContext.Roles.ToListAsync();
            return View(users);

        }

        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> UserRoleDetail(UserRoleDto userDto)
        {

            var usersRoles = await (
                from u in _dbContext.Users
                join s in _dbContext.Students on u.Id equals s.UserId
                join ur in _dbContext.UserRoles on u.Id equals ur.UserId
                join r in _dbContext.Roles on ur.RoleId equals r.Id
                select r.Name).ToListAsync();

            var roleNames = await _dbContext.Roles
                .Select(r => r.Name)
                .ToListAsync();

            var results = new UserRoleDetailDto()
            {
                UserId = userDto.UserId,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UniversityId = userDto.UniversityId,
                NationalCode = userDto.NationalCode
            };
            results.UserRoleName?.AddRange(usersRoles);
            results.RoleNameList?.AddRange(roleNames);

            return View(results);

        }

        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> AddRoleToUser(UserRoleDetailDto userDto,string roleName)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role is not null)
            {

                _dbContext.UserRoles.Add(new IdentityUserRole<string>()
                {
                    RoleId = role.Id,
                    UserId = userDto.UserId
                });
                userDto.UserRoleName.Add(roleName);

                await _dbContext.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(UsersController.UserRoleDetail), userDto);
        }

        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> RemoveRoleFromUser(UserRoleDetailDto userDto, string roleName)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role is not null)
            {
                var obj = new IdentityUserRole<string>()
                {
                    RoleId = role.Id,
                    UserId = userDto.UserId
                };

                userDto.UserRoleName!.Remove(roleName);

                _dbContext.UserRoles.Remove(obj);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(UsersController.UserRoleDetail), userDto);
        }

        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> AddRole(string? roleName)
        {
            if (!roleName.IsNullOrEmpty())
            {
                _dbContext.Roles.Add(new IdentityRole()
                {
                    Id = new Guid().ToString(),
                    ConcurrencyStamp = String.Empty,
                    Name = roleName,
                    NormalizedName = roleName

                });
                await _dbContext.SaveChangesAsync();
            }

            var roles = await _dbContext.Roles.ToListAsync();
            return View(roles);
        }
    }
}
