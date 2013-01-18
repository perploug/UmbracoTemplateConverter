using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.RazorConverter.Razor.DOM;
using Telerik.RazorConverter.WebForms.DOM;

namespace Telerik.RazorConverter.Razor.Converters
{
    public class FormTagConverter : INodeConverter<IRazorNode>
    {
        private IRazorNodeConverterProvider NodeConverterProvider
        {
            get;
            set;
        }

        public FormTagConverter(IRazorNodeConverterProvider converterProvider
                                  )
        {
            NodeConverterProvider = converterProvider;
       
        }

        public IList<IRazorNode> ConvertNode(IWebFormsNode node)
        {
            var contentTag = node as IWebFormsServerControlNode;
            var convertedChildren = new List<IRazorNode>();


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
            return serverControlNode != null && serverControlNode.TagName.ToLowerInvariant() == "form";
        }
    }
}
