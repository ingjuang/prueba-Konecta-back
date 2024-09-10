using Business.Interfaces;
using Core.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class TwoFactorService : ITwoFactorService
    {
        private readonly AppDbContext _db;

        public TwoFactorService(AppDbContext db)
        {
            _db = db;
        }

        public async Task SetSecret(string userName, string code)
        {
            User user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user is not null)
            {
                user.TwoFactorSecret = code;
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<string?> GetSecret(string userName)
        {
            User user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user is not null)
            {
                return user.TwoFactorSecret;
            }
            else
            {
                return "";
            }
        }

        public async Task<bool> HaveTFA(string userName)
        {
            User user = await _db.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            return user.TwoFactorSecret is not null? true : false;
        }
    }
}
