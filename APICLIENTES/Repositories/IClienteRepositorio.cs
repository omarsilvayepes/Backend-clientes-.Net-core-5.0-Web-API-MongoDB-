using APICLIENTES.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICLIENTES.Repositories
{
    public interface IClienteRepositorio
    {
        Task<List<ClienteDTO>> getClientes();
        Task<ClienteDTO> getClienteById(string id);
        Task<ClienteDTO> createOrUpdate(ClienteDTO clienteDTO);
        Task deleteCliente(string id);
    }
}
