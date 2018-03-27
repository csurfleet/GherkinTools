using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace GherkinTools.VisualStudio
{
    [Export(typeof(ISuggestedActionsSourceProvider))]
    [Name("Gherkin Suggested Actions")]
    [ContentType(FileAndContentTypes.GherkinFormat)]
    internal class SuggestedActionsSourceProvider
    {
        [Import(typeof(ITextStructureNavigatorSelectorService))]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        public ISuggestedActionsSource CreateSuggestedActionsSource(ITextView textView, ITextBuffer textBuffer)
        {
            if (textBuffer == null && textView == null)
                return null;

            return new SuggestedActionsSource(this, textView, textBuffer);
        }
    }
}
