using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backendApp.Dtos;
using backendApp.Models;


namespace backendApp.Mappers
{
    public static class UserMapper
    {
        public static User ToUserFromCreateDto(this CreateUserRequestDto userDto) {
            return new User 
            {
                Username = userDto.Username,
                Password = userDto.Password,
                Email = userDto.Email,
                DateJoined = userDto.DateJoined
            };
        }
    }
}