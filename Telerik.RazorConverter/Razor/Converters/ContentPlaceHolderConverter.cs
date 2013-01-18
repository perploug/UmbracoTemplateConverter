using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.RazorConverter.Razor.DOM;
using Telerik.RazorConverter.WebForms.DOM;

namespace Telerik.RazorConverter.Razor.Converters
{
    public class ContentPlaceholderTagConverter : INodeConverter<IRazorNode>
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

        public ContentPlaceholderTagConverter(IRazorNodeConverterProvider converterProvider,
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

            if(node.Children.Any())
            {
                var start = new RazorTextNodeFactory().CreateTextNode("@if (IsSectionDefined(\"" + contentPlaceHolderID + "\")) {\n @RenderSection(\"" + contentPlaceHolderID + "\");\n}\nelse {\n");
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
                convertedChildren.Add(new RazorTextNodeFactory().CreateTextNode("\n}"));
   
            }else
            {
                var tag = new RazorTextNodeFactory().CreateTextNode("@RenderSection(\"" + contentPlaceHolderID + "\", false)");
                convertedChildren.Add(tag);
            }

            
            return convertedChildren;
        }

        public bool CanConvertNode(IWebFormsNode node)
        {
            var serverControlNode = node as IWebFormsServerControlNode;
            return serverControlNode != null && serverControlNode.TagName.ToLowerInvariant() == "asp:contentplaceholder";
        }
    }
}
