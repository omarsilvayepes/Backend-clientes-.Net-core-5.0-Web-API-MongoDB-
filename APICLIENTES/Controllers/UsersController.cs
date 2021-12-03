using APICLIENTES.Models.DTO;
using APICLIENTES.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICLIENTES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //inyectando el servicio
        private readonly IUserRepositorio _userRepositorio;
        protected ResponseDTO responseDTO;


        public UsersController(IUserRepositorio userRepositorio)
        {
           _userRepositorio = userRepositorio;
            responseDTO = new ResponseDTO();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            string respuesta = await _userRepositorio.Register(new Models.User {UserName=userDTO.UserName},userDTO.PassWord);
            if (respuesta.Equals("Usuario ya esta registrado"))
            {
                responseDTO.IsSuccess = false;
                responseDTO.DisplayMessage = "Usuario ya esta Registrado";
                return BadRequest(responseDTO);
            }
            if (respuesta.Equals("500"))
            {
                responseDTO.IsSuccess = false;
                responseDTO.DisplayMessage = "Error al Crear Usuario";
                return BadRequest(responseDTO);
            }
            
            responseDTO.DisplayMessage = "Usuario Registrado con exito";
            JWTPackage jWTPackage = new JWTPackage();
            jWTPackage.UserName = userDTO.UserName;
            jWTPackage.Token = respuesta;
            responseDTO.result = jWTPackage;
            return Ok(responseDTO);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            string respuesta = await _userRepositorio.Login(userDTO.UserName,userDTO.PassWord);
            if (respuesta.Equals("NoUser"))
            {
                responseDTO.IsSuccess = false;
                responseDTO.DisplayMessage = "Usuario No esta Registrado";
                return BadRequest(responseDTO);
            }
            if (respuesta.Equals("PassWordWrong"))
            {
                responseDTO.IsSuccess = false;
                responseDTO.DisplayMessage = "PassWord Incorrecta";
                return BadRequest(responseDTO);
            }
            responseDTO.DisplayMessage = "Usuario Logueado con exito";
            JWTPackage jWTPackage = new JWTPackage();
            jWTPackage.UserName = userDTO.UserName;
            jWTPackage.Token = respuesta;
            responseDTO.result = jWTPackage;
            return Ok(responseDTO);
        }

        public class JWTPackage
        {
            public string UserName { get; set; }
            public string Token { get; set; }
        }
    }
}
