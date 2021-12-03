using APICLIENTES.Models.DTO;
using APICLIENTES.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APICLIENTES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]// solo para usuarios autorizador
    public class ClientesController : ControllerBase
    {
        //inyectando el servicio
        private readonly IClienteRepositorio clienteRepositorio;
        protected ResponseDTO responseDTO;

        public ClientesController(IClienteRepositorio clienteRepositorio)
        {
            this.clienteRepositorio = clienteRepositorio;
            responseDTO = new ResponseDTO();
        }


        // GET: api/<ClientesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var lista = await clienteRepositorio.getClientes();
                responseDTO.result = lista;
                responseDTO.DisplayMessage = "Lista de Clentes";
            }

            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.ErrorMessages = new List<string> { ex.ToString() };

            }
            return Ok(responseDTO);
        }

        // GET api/<ClientesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(string id)
        {
            var cliente = await clienteRepositorio.getClienteById(id);
            if (cliente!=null)
            {
                responseDTO.result = cliente;
                responseDTO.DisplayMessage = "Informacion del cliente";
                return Ok(responseDTO);
            }
            responseDTO.IsSuccess = false;
            responseDTO.DisplayMessage = "Cliente no existe";
            return NotFound(responseDTO);

        }

        // POST api/<ClientesController>
        [HttpPost]
        public async Task<IActionResult> PostCliente([FromBody] ClienteDTO clienteDTO)
        {
            try
            {
                ClienteDTO model = await clienteRepositorio.createOrUpdate(clienteDTO);
                if (model==null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.DisplayMessage = "Cliente ya fue creado";
                    return BadRequest(responseDTO);
                }
                responseDTO.result = model;
                responseDTO.DisplayMessage = "Cliente Creado con exito";
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {

                responseDTO.IsSuccess = false;
                responseDTO.DisplayMessage = "Error al Crear el registro";
                responseDTO.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(responseDTO);
            }

        }

        // PUT api/<ClientesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(string id, ClienteDTO clienteDTO)
        {
            try
            {
                ClienteDTO model = await clienteRepositorio.createOrUpdate(clienteDTO);
                responseDTO.result = model;
                return Ok(responseDTO);
            }
            catch (Exception ex)
            {

                responseDTO.IsSuccess = false;
                responseDTO.DisplayMessage = "Error al actualizar el registro";
                responseDTO.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(responseDTO);
            }

        }

        // DELETE api/<ClientesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(string id)
        {
            try
            {
                var cliente = await clienteRepositorio.getClienteById(id);
                if (cliente!=null)
                {
                    await clienteRepositorio.deleteCliente(id);
                    responseDTO.result = true;
                    responseDTO.DisplayMessage = "Cliente Eliminado con exito";
                    return Ok(responseDTO);
                }
                responseDTO.IsSuccess = false;
                responseDTO.DisplayMessage = "Error al eliminar cliente";
                return BadRequest(responseDTO);
            }
            catch (Exception ex)
            {

                responseDTO.IsSuccess = false;
                responseDTO.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(responseDTO);

            }

        }
    }
}
