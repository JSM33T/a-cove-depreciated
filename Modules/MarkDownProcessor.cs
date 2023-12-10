using Markdig;

namespace laymaann.Modules
{
    public class MarkDownProcessor
    {
        public static string ConvertMarkdownToHtml(string markdownContent)
        {
            //Use Markdig to perform the conversion
            var pipeline = new MarkdownPipelineBuilder().Build();
            return Markdig.Markdown.ToHtml(markdownContent, pipeline);
        }

    }
}
