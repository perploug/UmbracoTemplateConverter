using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.RazorConverter.Razor.DOM;
using Telerik.RazorConverter.WebForms.DOM;

namespace Telerik.RazorConverter.Razor.Converters
{
    public class HeadServerTagConverter : INodeConverter<IRazorNode>
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

        public HeadServerTagConverter(IRazorNodeConverterProvider converterProvider,
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
            var contentPlaceHolderID = contentTag.Attributes["id"];
            var convertedChildren = new List<IRazorNode>();

                var start = new RazorTextNodeFactory().CreateTextNode("<head>");
                convertedChildren.Add(start);

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
                convertedChildren.Add(new RazorTextNodeFactory().CreateTextNode("</head>"));


            return convertedChildren;
        }

        public bool CanConvertNode(IWebFormsNode node)
        {
            var serverControlNode = node as IWebFormsServerControlNode;
            return serverControlNode != null && serverControlNode.TagName.ToLowerInvariant() == "head";
        }
    }
}
