using APICLIENTES.Models;
using APICLIENTES.Models.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICLIENTES
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
              {
                  config.CreateMap<ClienteDTO, Cliente>();
                  config.CreateMap<Cliente, ClienteDTO>();
              });
            return mappingConfig;
        }
    }
}
