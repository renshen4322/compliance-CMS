using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Model.RIRecordXMLModel
{
    public class snapshotResource
    {
        public head _head { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
    }

    public class head
    {
        public string documentID { get; set; }
        public string contentFormat { get; set; }
        public string subContentFormat { get; set; }
        public string contentType { get; set; }
        public string documentType { get; set; }
        public string effectiveDate { get; set; }
        public string expiredDate { get; set; }
        public string entityID { get; set; }
        public string elementID { get; set; }
    }


    public class Meta
    {
        public List<Metadata> _metadata { get; set; }
    }
    public class Metadata
    {
        public string Attr_type { get; set; }
        public string Attr_triage { get; set; }
        public string Attr_kana { get; set; }
        public string Attr_citm_id { get; set; }
        public string Attr_hasMeta { get; set; }
        public string Attr_doc_id { get; set; }
        public string Attr_content { get; set; }
        public string Attr_url { get; set; }
        public Metadata_Main ObjMain { get; set; }
        public Metadata_Sub ObjSub { get; set; }
    }


    public class Metadata_Main : Metadata
    {
        public Metadata_paragraph _paragraph { get; set; }
    }

    public class Metadata_paragraph
    {
        public string Attr_type { get; set; }
        public string Attr_num { get; set; }
        public string markupText { get; set; }
    }
    public class Metadata_Sub : Metadata
    {
        public string text { get; set; }      
    }


    /// <summary>
    /// main xml body
    /// </summary>
    public class WrapBody
    {
        public List<paragraph> _paragraph { get; set; }

    }

    /// <summary>
    /// main xml body paragraph
    /// </summary>
    public class paragraph
    {
        public string Attr_type { get; set; }
        public string checkitemID { get; set; }

        //定义有几个citem
    }

    /// <summary>
    /// citem
    /// </summary>
    public class sub_body
    {
        public string Attr_type { get; set; }
        public string Attr_num { get; set; }
        public string markupText { get; set; }
    }

    /// <summary>
    /// main xml
    /// </summary>
    public class WrapXml : snapshotResource
    {
        public WrapBody _wrapbody { get; set; }
    }

    /// <summary>
    /// meta xml main and sub
    /// </summary>
    public class MetaXml : snapshotResource
    {
        public Meta _meta { get; set; }

    }

    /// <summary>
    /// citem
    /// </summary>
    public class Sub_Xml : snapshotResource
    {
        public sub_body _sub_body { get; set; }
    }
}
