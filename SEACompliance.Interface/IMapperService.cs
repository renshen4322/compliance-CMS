using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Service.Interface
{
    public interface IMapperService
    {
        TDestination MapModel<TSource, TDestination>(TSource source);

        TDestination MapModel<TDestination>(object source);

        void Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
