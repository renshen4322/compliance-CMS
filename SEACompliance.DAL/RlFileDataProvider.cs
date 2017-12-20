using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using SEACompliance.DataBase;
using SEACompliance.DAL.Interface;
using System.Data.SqlClient;
using System.Data;
using SEACompliance.Model;
using AutoMapper;
using SEACompliance.Core.ExceptionApi;
using System.IO;
using SEACompliance.Core.Common;

namespace SEACompliance.DAL
{
    public class RlFileDataProvider : IRlFileDataProvider
    {

        public List<RIFileModel> GetFilesById(string documentId,string flagInfo = null)
        {
            var query = new Sql("select * from [APlatformAppSchema].[lnRIFile] where DocumentID=@0 order by CREATETIME ", new SqlParameter { DbType = DbType.String, Value = documentId });

            if (documentId.Equals("-1"))
            {
                query= new Sql("select * from [APlatformAppSchema].[lnRIFile] where DocumentID=@0 and CreateUser=@1 order by CREATETIME ", new SqlParameter { DbType = DbType.String, Value = documentId },new SqlParameter { DbType = DbType.String, Value = flagInfo });
            }
            List<RIFileModel> list = null;
            try
            {
                List<lnRIFile> riList = lnRIFile.Fetch(query);
                list = Mapper.Map<List<RIFileModel>>(riList);
            }
            catch (Exception ex)
            {
                LogHelper.Error<RlFileDataProvider>("Get All lnRIFile Failed! \n", ex);
                throw new RequestErrorException(string.Format("Get All lnRIFile Failed documentId_{0}_!", documentId + "__" + ex.Message));
            }
            return list;
        }

        public RIFileModel GetFileBydocId(string docId)
        {
            RIFileModel _filemode = null;
            var _sql = new Sql("select * from [APlatformAppSchema].[lnRIFile] where DocID=@0", new SqlParameter { DbType = DbType.String, Value = docId });
            try
            {
                lnRIFile _riFile = lnRIFile.SingleOrDefault(_sql);
                _filemode = Mapper.Map<RIFileModel>(_riFile);
            }
            catch (Exception ex) 
            {
                throw new RequestErrorException(string.Format("select {0} failed", docId + "__" + ex.Message));
            }
            return _filemode;
        }

        public void DeleteFileById(string docId)
        {
            var _sql = new Sql("delete from [APlatformAppSchema].[lnRIFile] where DocID=@0", new SqlParameter { DbType = DbType.String, Value = docId });
            var _sqlStr = new Sql("delete from [APlatformAppSchema].[lnRIRelRecordTools] where FileDocID=@0", new SqlParameter { DbType = DbType.String, Value = docId });
            try
            {
                lnRIFile.repo.BeginTransaction();
                int result = lnRIFile.repo.Execute(_sql);
                lnRIRelRecordTool.repo.Execute(_sqlStr);
                lnRIFile.repo.CompleteTransaction();

            }
            catch (Exception ex)
            {
                lnRIFile.repo.AbortTransaction();
                throw new RequestErrorException(string.Format("delete {0} failed db error", docId + "__" + ex.Message));
            }
        }

        public void DelFileByDocmentIdForZero(string _userName)
        {
            var _sql = new Sql("delete from [APlatformAppSchema].[lnRIFile] where DocumentID='-1' and CreateUser=@0" ,new SqlParameter { DbType = DbType.String, Value = _userName });
            try
            {
                int result = lnRIFile.repo.Execute(_sql);

            }
            catch (Exception ex)
            {

                throw new RequestErrorException(string.Format("delete DocumentID {0} failed db error", "-1" + "__" + ex.Message));
            }
        }

        public RIFileModel UpdateFile(RIFileModel cplnRI)
        {
            RIFileModel result = null;
            var _sql = new Sql(@"update [APlatformAppSchema].[lnRIFile] set Title=@0 ,Content=@1, UPDATETIME =GETDATE() 
                                            where DocID=@2 ", new SqlParameter { DbType = DbType.String, Value = cplnRI.Title },
                                            new SqlParameter { DbType = DbType.String, Value = cplnRI.Content },
                                            new SqlParameter { DbType = DbType.String, Value = cplnRI.DocID });
            try
            {
                var reFileDocid = lnRIFile.repo.Execute(_sql);
                result = cplnRI;
            }
            catch (Exception ex)
            {
                throw new RequestErrorException(string.Format("update fileInfo {0} failed", cplnRI.DocID + "__" + ex.Message));
            }
            return result;
        }

        public void UpdateFilesByDocumentId(List<string> _list, string documentId)
        {
            var tags = _list.ToArray();
            var _sql = new Sql("update [APlatformAppSchema].[lnRIFile] set DocumentID=@0 where DocID in(@1)", new SqlParameter { DbType = DbType.String, Value = documentId },
                                            new SqlParameter { DbType = DbType.String, Value = tags });
            try
            {
                var reFileDocid = lnRIFile.repo.Execute(_sql);

            }
            catch (Exception ex)
            {
                throw new RequestErrorException(string.Format("update fileInfo {0} failed for", _list + "__" + ex.Message));
            }


        }
        public RIFileModel CreateFileInfo(RIFileModel cplnRI, out string docId)
        {
            RIFileModel result = null;
            if (string.IsNullOrEmpty(cplnRI.FileName))
            {
                cplnRI.FileName = "";
            }
            if (string.IsNullOrEmpty(cplnRI.Content))
            {
                cplnRI.Content = "";
            }
            lnRIFile _reFile = null;
            var _sql = new Sql(@"INSERT INTO [APlatformAppSchema].[lnRIFile]
           (DocID,
		   [Title]
           ,[MimeType]
           ,[Path]
           ,[CREATETIME]
           ,[Content]
           ,[DocumentID]
           ,[UPDATETIME]
           ,[FileName],[CreateUser])
   output inserted.DocID VALUES
           (@0
		   ,@1
           ,@2
           ,@3
           ,@4
           ,@5
           ,@6
           ,@7
           ,@8
           ,@9)", 
new SqlParameter { DbType = DbType.String, Value = cplnRI.DocID },
           new SqlParameter { DbType = DbType.String, Value = cplnRI.Title },
           new SqlParameter { DbType = DbType.String, Value = cplnRI.MimeType },
           new SqlParameter { DbType = DbType.String, Value = cplnRI.Path },
           new SqlParameter { DbType = DbType.DateTime, Value = DateTime.Now },
           new SqlParameter { DbType = DbType.String, Value = cplnRI.Content },
           new SqlParameter { DbType = DbType.String, Value = cplnRI.DocumentID },
           new SqlParameter { DbType = DbType.DateTime, Value = DateTime.Now },
           new SqlParameter { DbType = DbType.String, Value = cplnRI.FileName },
           new SqlParameter { DbType = DbType.String, Value = cplnRI.CreateUser }
           );

            try
            {
                lnRIFile.repo.BeginTransaction();
                _reFile = Mapper.Map<lnRIFile>(cplnRI);
                var reFileDocid = lnRIFile.repo.ExecuteScalar<string>(_sql);
                if (cplnRI.DocumentID != "-1")
                {
                    var _sql2 = new Sql(@"INSERT INTO APlatformAppSchema.lnRIRelRecordTools ( DocID, FileDocID )
                            VALUES(@0, @1)", new SqlParameter { DbType = DbType.String, Value = cplnRI.DocumentID },
                    new SqlParameter { DbType = DbType.String, Value = reFileDocid });
                    lnRIRelRecordTool.repo.Execute(_sql2);
                }
                lnRIFile.repo.CompleteTransaction();
                result = cplnRI;
                docId = reFileDocid;
            }
            catch (Exception ex)
            {
                lnRIFile.repo.AbortTransaction();
                throw new RequestErrorException(string.Format("CreateFile {0} failed", cplnRI.DocID + "__" + ex.Message));
            }
            return result;
        }

        public int GetFileCountByFileName(string fileName)
        {
            int result = 0;
            try
            {
                var _sql = new Sql(@" select count(1) from[APlatformAppSchema].[lnRIFile] where[FileName] = @0 ", new SqlParameter { DbType = DbType.String, Value = fileName });
                result = lnRIFile.repo.ExecuteScalar<int>(_sql);
            }
            catch (Exception ex)
            {

                throw new RequestErrorException(string.Format("select fileName__ {0} failed", fileName + "__" + ex.Message));
            }
            return result;
        }
    }
}
