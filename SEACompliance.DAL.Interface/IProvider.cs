using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.DAL.Interface
{
    public interface IProvider<T> where T : new()
    {
        T GetById(int id);
        IEnumerable<T> GetAll(T model);
        T Save(T item);
        int New(T item);
        int Delete(T item);
    }
}
