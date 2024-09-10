using Business.Interfaces;
using Core.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserService> _logger;

        public UserService(AppDbContext db, ILogger<UserService> logger)
        {
            _db = db;
            _logger = logger;
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
                        _logger.LogError("Error al crear usuario");
                        return new PetitionResponse
                        {
                            success = false,
                            message = "Error al insertar usuario en la base de datos",
                            result = null
                        };
                    }
                    else
                    {
                        _logger.LogInformation("Usuario creado exitosamente");
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
                    _logger.LogWarning("Está intentando crear un usuario que ya existe");
                    return new PetitionResponse
                    {
                        success = false,
                        message = "Ya existe este nombre de usuario",
                        result = null
                    };
                }
            }
            catch (Exception ex) {
                _logger.LogError($"Error al crear usuario: {ex.Message}");
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
                    _logger.LogError("Error al buscar usuario");
                    return new PetitionResponse
                    {
                        success = true,
                        message = "Usuario encontrado",
                        result = user
                    };
                }
                else
                {
                    _logger.LogInformation("Usuario encontrado exitosamente");
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
                _logger.LogError($"Error al buscar usuario: {ex.Message}");
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
                    _logger.LogError("Error al buscar usuarios");
                    return new PetitionResponse
                    {
                        success = true,
                        message = "Usuarios encontrados",
                        result = users
                    };
                }
                else
                {
                    _logger.LogInformation("Usuarios encontrados exitosamente");
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
                _logger.LogError($"Error al buscar usuarios: {ex.Message}");
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
                        _logger.LogError("Error al actualizar usuario");
                        return new PetitionResponse
                        {
                            success = false,
                            message = "Error al actualizar usuario en la base de datos",
                            result = null
                        };
                    }
                    else
                    {
                        _logger.LogInformation("Se actualizó el usuario exitosamente");
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
                    _logger.LogWarning("El usuario que intenta actualizar ya está creado");
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
                _logger.LogError($"Error al actualizar usuario: {ex.Message}");
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
