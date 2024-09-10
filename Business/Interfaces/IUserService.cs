using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Responses;

namespace Business.Interfaces
{
    public interface IUserService
    {
        public Task<PetitionResponse> CreateUser(User user);

        public Task<PetitionResponse> UpdateUser(User user);
        public Task<PetitionResponse> GetUser(int id);
        public Task<PetitionResponse> GetUsers();

    }
}
