using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Text;
using Telerik.RazorConverter;
using Telerik.RazorConverter.Razor.Converters;
using Telerik.RazorConverter.Razor.DOM;


namespace UmbracoTemplateConverter
{
    public partial class ConverterDashboard : System.Web.UI.UserControl
    {
        [Import]
        private IWebFormsParser Parser
        {
            get;
            set;
        }

        [Import]
        private IWebFormsConverter<IRazorNode> Converter
        {
            get;
            set;
        }

        [Import]
        private IRenderer<IRazorNode> Renderer
        {
            get;
            set;
        }


        public ConverterDashboard()
        {
            var catalog = new AssemblyCatalog(typeof(IWebFormsParser).Assembly);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);   
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Convert(object sender, EventArgs e)
        {
            var inputFolder = MapPath("~/masterpages");
            var outputFolder = MapPath("~/views");

            var files = System.IO.Directory.GetFiles(inputFolder, "*.master");
            StringBuilder output = new StringBuilder("<pre>");
            var directoryHandler = new DirectoryHandler(inputFolder, outputFolder);

            foreach (var file in files)
            {
                Console.WriteLine("Converting {0}", file);

                var webFormsPageSource = File.ReadAllText(file, Encoding.UTF8);
                var webFormsDocument = Parser.Parse(webFormsPageSource);
                var razorDom = Converter.Convert(webFormsDocument);
                var razorPage = Renderer.Render(razorDom);

                var outputFileName = ReplaceExtension(directoryHandler.GetOutputFileName(file), ".cshtml");

                output.AppendFormat("Writing    {0}", outputFileName);
                EnsureDirectory(Path.GetDirectoryName(outputFileName));
                File.WriteAllText(outputFileName, razorPage, Encoding.UTF8);

                output.Append("Done\n");
            }

            output.Append("</pre>");
            lt_output.Text = output.ToString();
        }

        private static void EnsureDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static string ReplaceExtension(string fileName, string newExtension)
        {
            var targetFolder = Path.GetDirectoryName(fileName);
            return Path.Combine(targetFolder, Path.GetFileNameWithoutExtension(fileName) + newExtension);
        }
    }
}