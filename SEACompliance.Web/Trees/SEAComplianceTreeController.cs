using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using System.Net.Http.Formatting;

namespace SEACompliance.Web.Trees
{
    [Tree("SEACompliance", "SEACompliance", "SEACompliance Management",sortOrder:0)]
    [PluginController("SEACompliance")]
    public class SEAComplianceTreeController : TreeController
    {
        protected override TreeNode CreateRootNode(FormDataCollection queryStrings)
        {
            var node = base.CreateRootNode(queryStrings);
            node.MenuUrl = string.Empty;
            node.Name = "SEACompliance Management";
            return node;
        }
        
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            if (id == Constants.System.Root.ToInvariantString())
            {
                var nodes = new TreeNodeCollection
                {
                    CreateTreeNode("rirecord", id, queryStrings, "RIRecord Management", "icon-folder", false,
                    string.Format("SEACompliance/SEACompliance/RIRecord/{0}", id)),
                    //CreateTreeNode("rirecordtype", id, queryStrings, "RIRecordType Management", "icon-folder", false,
                    //string.Format("SEACompliance/SEACompliance/RIRecordType/{0}", id))
                };
                nodes.ForEach(o => o.MenuUrl = string.Empty);
                return nodes;
            }
            throw new NotSupportedException();
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return new MenuItemCollection();
        }
    }
}