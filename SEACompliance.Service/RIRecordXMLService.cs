using System;
using System.Text;
using SEACompliance.Core.Common;
using SEACompliance.Model;
using System.IO;
using System.Xml;
using SEACompliance.Model.RIRecordXMLModel;

namespace SEACompliance.Service
{
    public class RIRecordXMLService
    {
        public static RIRecordXMLService Instance { get { return Singleton<RIRecordXMLService>.GetInstance(); } }

        private string fileName { get; set; }

        private string GetFileName(snapshotResource obj)
        {
            string _p = System.Configuration.ConfigurationManager.AppSettings["XMLPath"].ToString();
            string path = _p + "\\" + obj.filepath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fileName = _p.Replace("\\", "/") + "/" + obj.filepath + "/" + obj.filename;
            return fileName;
        }

        /// <summary>
        /// 主文件
        /// </summary>
        /// <param name="obj"></param>
        public void Toxml(WrapXml obj)
        {
            string fileName = GetFileName(obj);
            XmlDocument xmldoc = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<?xml version='1.0' encoding='UTF-8' ?>");
            sb.Append(@"<snapshotResource>");
            sb.Append(GenerHead(obj._head));
            sb.Append(@"<body>");
            if (obj != null)
            {
                if (obj._wrapbody != null)
                {
                    if (obj._wrapbody._paragraph != null && obj._wrapbody._paragraph.Count > 0)
                    {
                        for (int i = 0; i < obj._wrapbody._paragraph.Count; i++)
                        {
                            sb.Append(@"<paragraph type='" + obj._wrapbody._paragraph[i].Attr_type + "'><checkitemID>" + obj._wrapbody._paragraph[i].checkitemID + "</checkitemID></paragraph>");
                        }
                    }
                }
            }
            sb.Append(@"</body>");
            sb.Append(@"</snapshotResource>");


            xmldoc.LoadXml(sb.ToString());
            xmldoc.Save(fileName + ".xml"); //保存 
        }

        /// <summary>
        /// 主文件 子文件 的公用 Meta
        /// </summary>
        /// <param name="obj"></param>
        public void Toxml_Meta(MetaXml obj)
        {
            try
            {
                string fileName = GetFileName(obj);
                XmlDocument xmldoc = new XmlDocument();
                StringBuilder sb = new StringBuilder();
                sb.Append(@"<?xml version='1.0' encoding='UTF-8' ?>");
                sb.Append(@"<snapshotResource>");
                sb.Append(GenerHead(obj._head));
                sb.Append(@"<meta>");
                if (obj != null)
                {
                    if (obj._meta != null)
                    {
                        if (obj._meta._metadata != null && obj._meta._metadata.Count > 0)
                        {
                            for (int i = 0; i < obj._meta._metadata.Count; i++)
                            {
                                if (obj._meta._metadata[i].ObjSub != null && obj._meta._metadata[i].ObjSub.text != null)
                                {
                                    if (obj._meta._metadata[i].Attr_citm_id != null && obj._meta._metadata[i].Attr_citm_id != "")
                                    {
                                        if (!string.IsNullOrEmpty(obj._meta._metadata[i].Attr_doc_id) && !string.IsNullOrEmpty(obj._meta._metadata[i].Attr_content))
                                        {
                                            if (!string.IsNullOrEmpty(obj._meta._metadata[i].Attr_url))
                                            {
                                                sb.Append(@"<metadata type='" + obj._meta._metadata[i].Attr_type + "' citm_id= '" + obj._meta._metadata[i].Attr_citm_id + "' doc_id= '" + obj._meta._metadata[i].Attr_doc_id + "' url= '" + obj._meta._metadata[i].Attr_url + "' content= '" + obj._meta._metadata[i].Attr_content + "'><title>" + obj._meta._metadata[i].ObjSub.text + "</title></metadata>");
                                            }
                                            else
                                            {
                                                sb.Append(@"<metadata type='" + obj._meta._metadata[i].Attr_type + "' citm_id= '" + obj._meta._metadata[i].Attr_citm_id + "' doc_id= '" + obj._meta._metadata[i].Attr_doc_id + "' content= '" + obj._meta._metadata[i].Attr_content + "'><title>" + obj._meta._metadata[i].ObjSub.text + "</title></metadata>");
                                            }
                                        }
                                        else
                                        {
                                            sb.Append(@"<metadata type='" + obj._meta._metadata[i].Attr_type + "' citm_id= '" + obj._meta._metadata[i].Attr_citm_id + "'><text>" + obj._meta._metadata[i].ObjSub.text + "</text></metadata>");
                                        }
                                    }
                                    else
                                    {
                                        sb.Append(@"<metadata type='" + obj._meta._metadata[i].Attr_type + "'><text>" + obj._meta._metadata[i].ObjSub.text + "</text></metadata>");
                                    }
                                }
                                if (obj._meta._metadata[i].ObjMain != null && obj._meta._metadata[i].ObjMain._paragraph != null)
                                {
                                    sb.Append(@"<metadata type='" + obj._meta._metadata[i].Attr_type + "' citm_id= '" + obj._meta._metadata[i].Attr_citm_id + "' hasMeta='" + obj._meta._metadata[i].Attr_hasMeta + "'><text><paragraph type='" + obj._meta._metadata[i].ObjMain._paragraph.Attr_type + "' num='" + obj._meta._metadata[i].ObjMain._paragraph.Attr_num + "'><markupText>" + obj._meta._metadata[i].ObjMain._paragraph.markupText + "</markupText></paragraph></text></metadata>");
                                }

                            }
                        }
                    }
                }
                sb.Append(@"</meta>");
                sb.Append(@"</snapshotResource>");
                xmldoc.LoadXml(sb.ToString());
                xmldoc.Save(fileName + ".xml");
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        /// <summary>
        /// 子文件
        /// </summary>
        /// <param name="obj"></param>
        public void Toxml_Sub(Sub_Xml obj)
        {
            string fileName = GetFileName(obj);
            XmlDocument xmldoc = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<?xml version='1.0' encoding='UTF-8' ?>");
            sb.Append(@"<snapshotResource>");
            sb.Append(GenerHead(obj._head));
            sb.Append(@"<body>");
            if (obj != null)
            {
                sb.Append(@"<paragraph type='" + obj._sub_body.Attr_type + "' num='" + obj._sub_body.Attr_num + "'><markupText>" + obj._sub_body.markupText + "</markupText></paragraph>");
            }
            sb.Append(@"</body>");
            sb.Append(@"</snapshotResource>");
            xmldoc.LoadXml(sb.ToString());
            xmldoc.Save(fileName + ".xml");
        }

        private string GenerHead(head h)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<head>");
            if (h != null)
            {
                sb.Append(@"<documentID>" + h.documentID + "</documentID>");
                sb.Append(@"<contentFormat>" + h.contentFormat + "</contentFormat>");
                sb.Append(@"<subContentFormat>" + h.subContentFormat + "</subContentFormat>");
                sb.Append(@"<contentType>" + h.contentType + "</contentType>");
                sb.Append(@"<documentType>" + h.documentType + "</documentType>");
                sb.Append(@"<effectiveDate>" + h.effectiveDate + "</effectiveDate>");
                sb.Append(@"<expiredDate>" + h.expiredDate + "</expiredDate>");
                sb.Append(@"<entityID  id='" + h.entityID + "' />");
                sb.Append(@"<elementID id='" + h.elementID + "' />");
            }
            sb.Append(@"</head>");
            return sb.ToString();
        }
    }
}
