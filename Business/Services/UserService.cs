using Business.Interfaces;
using Core.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Responses;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<PetitionResponse> CreateUser(User user)
        {
            try
            {
                User oldUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (oldUser == null)
                {
                    _db.Users.Add(user);
                    if (await _db.SaveChangesAsync() <= 0)
                    {
                        return new PetitionResponse
                        {
                            success = false,
                            message = "Error al insertar usuario en la base de datos",
                            result = null
                        };
                    }
                    else
                    {
                        return new PetitionResponse
                        {
                            success = true,
                            message = "Se guardó usuario exitosamente",
                            result = user
                        };
                    }
                }
                else
                {
                    return new PetitionResponse
                    {
                        success = false,
                        message = "Ya existe este nombre de usuario",
                        result = null
                    };
                }
            }
            catch (Exception ex) {
                return new PetitionResponse
                {
                    success = false,
                    message = $"Error al guardar usuario: {ex.Message}",
                    result = null
                };
            }
        }

        public async Task<PetitionResponse> GetUser(int id)
        {
            try
            {
                User user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user != null)
                {
                    return new PetitionResponse
                    {
                        success = true,
                        message = "Usuario encontrado",
                        result = user
                    };
                }
                else
                {
                    return new PetitionResponse
                    {
                        success = false,
                        message = "Usuario no encontrado",
                        result = null
                    };
                }
            }
            catch (Exception ex) 
            {
                return new PetitionResponse
                {
                    success = false,
                    message = $"Error al buscar usuario: {ex.Message}",
                    result = null
                };
            }
        }

        public async Task<PetitionResponse> GetUsers()
        {
            try
            {
                List<User> users = await _db.Users.ToListAsync();
                if (users.Count > 0)
                {
                    return new PetitionResponse
                    {
                        success = true,
                        message = "Usuarios encontrados",
                        result = users
                    };
                }
                else
                {
                    return new PetitionResponse
                    {
                        success = false,
                        message = "Usuarios no encontrados",
                        result = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new PetitionResponse
                {
                    success = false,
                    message = $"Error al buscar usuarios: {ex.Message}",
                    result = null
                };
            }
        }

        public async Task<PetitionResponse> UpdateUser(User user)
        {
            try
            {
                User oldUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (oldUser != null)
                {

                    _db.Entry(oldUser).CurrentValues.SetValues(user);
                    if (await _db.SaveChangesAsync() <= 0)
                    {
                        return new PetitionResponse
                        {
                            success = false,
                            message = "Error al actualizar usuario en la base de datos",
                            result = null
                        };
                    }
                    else
                    {
                        return new PetitionResponse
                        {
                            success = true,
                            message = "Se actualizó usuario exitosamente",
                            result = user
                        };
                    }
                }
                else
                {
                    return new PetitionResponse
                    {
                        success = false,
                        message = "El usuario no existe en la base de datos",
                        result = null
                    };
                }
            }
            catch (Exception ex) 
            {
                return new PetitionResponse
                {
                    success = false,
                    message = $"Error al actualizar usuario: {ex.Message}",
                    result = null
                };
            }
        }
    }
}
