using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.Service.Interface;
using SEACompliance.DAL.Interface;
using SEACompliance.DataBase;
using SEACompliance.DAL;
using SEACompliance.Core.Common;

namespace SEACompliance.Service
{
    public class RIRecordTypeService : IRIRecordTypeService
    {
        private IRIRecordTypeProvider _Iobj;

        public RIRecordTypeService(IRIRecordTypeProvider pobj)
        {
            _Iobj = pobj;
        }

        public List<RIRecord_Type> GetAll()
        {
            return _Iobj.GetAll();
        }

        /// <summary>
        /// 筛选module
        /// </summary>
        /// <returns></returns>
        public List<RIRecord_Type> GetAll_Module()
        {
            List<RIRecord_Type> ret = new List<RIRecord_Type>();
            var query = from c in GetAll()
                        where c.Status == 0 && c.Depth == 1
                        select c;
            ret = query.Cast<RIRecord_Type>().ToList<RIRecord_Type>();
            
            return ret;
        }

        public List<RIRecord_Type> GetSome_Topic(string _ParentCode)
        {
            var query = from c in GetAll()
                        where c.Status == 0 && c.Depth == 2 && c.ParentCode == _ParentCode
                        select c;
            return query.Cast<RIRecord_Type>().ToList<RIRecord_Type>(); ;
        }

        public List<RIRecord_Type> GetSome_Sub(string _ParentCode)
        {
            var query = from c in GetAll()
                        where c.Status == 0 && c.Depth == 3 && c.ParentCode == _ParentCode
                        select c;
            return query.Cast<RIRecord_Type>().ToList<RIRecord_Type>(); ;
        }


        public RIRecord_Type AddRecordType(RIRecord_Type et)
        {
            return _Iobj.AddRecordType(et);
        }

        public RIRecord_Type UpdateRecordType(RIRecord_Type et)
        {
            return _Iobj.UpdateRecordType(et);
        }

        public bool DeleteRecordType(int id)
        {
            return _Iobj.DeleteRecordType(id);
        }
    }
}
