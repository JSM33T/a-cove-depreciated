using Markdig;

namespace almondcove.Modules
{
    public class MarkDownProcessor
    {
        public static string ConvertMarkdownToHtml(string markdownContent)
        {
            var pipeline = new MarkdownPipelineBuilder().Build();
            return Markdig.Markdown.ToHtml(markdownContent, pipeline);
        }

    }
}
