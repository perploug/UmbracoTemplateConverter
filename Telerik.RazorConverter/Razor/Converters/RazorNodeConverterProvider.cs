namespace Telerik.RazorConverter.Razor.Converters
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Telerik.RazorConverter;
    using Telerik.RazorConverter.Razor.DOM;

    [Export(typeof(IRazorNodeConverterProvider))]
    public class RazorNodeConverterProvider : IRazorNodeConverterProvider
    {
        [ImportingConstructor]
        public RazorNodeConverterProvider(  IRazorDirectiveNodeFactory directiveNodeFactory,
                                            IRazorSectionNodeFactory sectionNodeFactory,
                                            IRazorCodeNodeFactory codeNodeFactory,
                                            IRazorTextNodeFactory textNodeFactory,
                                            IRazorCommentNodeFactory commentNodeFactory,
                                            IRazorExpressionNodeFactory expressionNodeFactory,
                                            IContentTagConverterConfiguration contentTagConverterConfig)
        {
            NodeConverters = new INodeConverter<IRazorNode>[] {
                new DirectiveConverter(directiveNodeFactory),
                new ContentTagConverter(this, sectionNodeFactory, contentTagConverterConfig),
                new ContentPlaceholderTagConverter(this, sectionNodeFactory, contentTagConverterConfig),
                new HeadServerTagConverter(this, sectionNodeFactory, contentTagConverterConfig),
                new PlaceholderTagConverter(this),
                 new FormTagConverter(this),
                new CodeGroupConverter(this),
                new CodeBlockConverter(codeNodeFactory),
                new TextNodeConverter(textNodeFactory),
                new CommentNodeConverter(commentNodeFactory),
                new ExpressionBlockConverter(expressionNodeFactory),
                new MacroTagConverter(this, sectionNodeFactory, contentTagConverterConfig),
                new ItemTagConverter(this, sectionNodeFactory, contentTagConverterConfig),
            };
        }

        public IList<INodeConverter<IRazorNode>> NodeConverters
        {
            get;
            private set;
        }
    }
}
