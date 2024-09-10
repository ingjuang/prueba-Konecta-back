using Business.Interfaces;
using System;
using Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Responses;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Core.DTOs;

namespace Business.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _db;

        public LoginService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetUser(LoginDto dto)
        {
            return await _db.Users.FirstOrDefaultAsync(x=>x.UserName == dto.UserName && x.Password == dto.Password);
            
        }
    }
}
