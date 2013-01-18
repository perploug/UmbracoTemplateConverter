using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.RazorConverter.Razor.DOM;
using Telerik.RazorConverter.WebForms.DOM;

namespace Telerik.RazorConverter.Razor.Converters
{
    class ItemTagConverter: INodeConverter<IRazorNode>
    {
         private IRazorNodeConverterProvider NodeConverterProvider
        {
            get;
            set;
        }

        private IRazorSectionNodeFactory SectionNodeFactory
        {
            get;
            set;
        }

        private IContentTagConverterConfiguration Configuration
        {
            get;
            set;
        }

        public ItemTagConverter(IRazorNodeConverterProvider converterProvider,
                                    IRazorSectionNodeFactory sectionFactory,
                                    IContentTagConverterConfiguration converterConfiguration)
        {
            NodeConverterProvider = converterProvider;
            SectionNodeFactory = sectionFactory;
            Configuration = converterConfiguration;
        }


        public IList<IRazorNode> ConvertNode(IWebFormsNode node)
        {
            var contentTag = node as IWebFormsServerControlNode;
            var alias = contentTag.Attributes["field"];
            
            StringBuilder sb = new StringBuilder("@Umbraco.Field(\"" + alias + "\""); 
            if(contentTag.Attributes.Count > 2)
            {
                
                var attrs = contentTag.Attributes.Where(x => x.Key.ToLower() != "runat" && x.Key.ToLower() != "field" && x.Key.ToLower() != "id").ToList();

                if (attrs.Any())
                {
                    sb.Append(",");
                    int intVal;
                    bool boolval;

                    for (int i = 0; i < attrs.Count; i++)
                    {
                        var kv = attrs[i];
                        var value = kv.Value;
                        
                        if(int.TryParse(value, out intVal) || bool.TryParse(value, out boolval))
                            sb.Append(kv.Key + ":" + kv.Value + "");
                        else
                            sb.Append(kv.Key + ":\"" + kv.Value + "\"");
                            
                        
                        if (i != attrs.Count - 1)
                            sb.Append(",");
                    }
                }
            }

            sb.Append(")");

            var convertedChildren = new List<IRazorNode>();
            var macroHelper = new RazorTextNodeFactory().CreateTextNode(sb.ToString());
            convertedChildren.Add(macroHelper);

            foreach (var childNode in node.Children)
            {
                foreach (var converter in NodeConverterProvider.NodeConverters)
                {
                    if (converter.CanConvertNode(childNode))
                    {
                        convertedChildren.AddRange(converter.ConvertNode(childNode));
                    }
                }
            }
            
            return convertedChildren;
        }

        public bool CanConvertNode(IWebFormsNode node)
        {
            var serverControlNode = node as IWebFormsServerControlNode;
            return serverControlNode != null && serverControlNode.TagName.ToLowerInvariant() == "umbraco:item";
        }
    }
    
}
