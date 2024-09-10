using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ITwoFactorService
    {
        public Task SetSecret(string userName, string code);

        public Task<string> GetSecret(string userName);

        public Task<bool> HaveTFA(string userName);
    }
}
