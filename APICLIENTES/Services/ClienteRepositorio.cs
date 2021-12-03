using APICLIENTES.Models;
using APICLIENTES.Models.DTO;
using APICLIENTES.Repositories;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICLIENTES.Services
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly IMongoCollection<Cliente> mongoCollection;
        private IMapper mapper;

        public ClienteRepositorio(IDatabaseSettings settings, IMapper mapper)//conexionBD accede a DataBaseSetting y a su vez este a appsetting.json
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            mongoCollection = database.GetCollection<Cliente>(settings.CollectionName);

            this.mapper = mapper;
        }

        public async Task<ClienteDTO> createOrUpdate(ClienteDTO clienteDTO)
        {
            Cliente cliente = mapper.Map<ClienteDTO, Cliente>(clienteDTO);
            Cliente foundClient = await mongoCollection.FindAsync(new BsonDocument { { "CC", cliente.CC } })
                .Result.FirstOrDefaultAsync();
            //validar si es creacion o actualizacion.
            if (cliente.Id != null)
            {
                var clienteDB = Builders<Cliente>.Filter.Eq(resultado => resultado.Id, cliente.Id);//actualizacion
                await mongoCollection.ReplaceOneAsync(clienteDB, cliente);
                return mapper.Map<Cliente, ClienteDTO>(cliente);
            }
            if (foundClient==null)
            {
                await mongoCollection.InsertOneAsync(cliente);
                return mapper.Map<Cliente, ClienteDTO>(cliente);
            }

            return null;
        }

        public async Task deleteCliente(string id)
        {
            var cliente = Builders<Cliente>.Filter.Eq(r => r.Id, id);
            await mongoCollection.DeleteOneAsync(cliente);
        }

        public async Task<ClienteDTO> getClienteById(string id)
        {
            Cliente cliente = await mongoCollection.FindAsync(new BsonDocument { { "_id", new ObjectId(id) } })
             .Result.FirstOrDefaultAsync();

            return mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<List<ClienteDTO>> getClientes()
        {
            List<Cliente> lista = await mongoCollection.FindAsync(new BsonDocument()).Result.ToListAsync();
            return mapper.Map<List<ClienteDTO>>(lista);// convertir A DTO Para retornarle a el Usuario.
        }
    }
}
