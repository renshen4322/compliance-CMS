using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.Service.Interface;
using AutoMapper;

namespace SEACompliance.Service
{
    public class MapperService : IMapperService
    {
        public TDestination MapModel<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }
        public TDestination MapModel<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }
        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            Mapper.Map(source, destination);
        }
    }
}
