using GherkinTools.VisualStudio.Actions;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GherkinTools.VisualStudio
{
    class SuggestedActionsSource : ISuggestedActionsSource
    {
        private readonly SuggestedActionsSourceProvider _factory;
        private readonly ITextBuffer _textBuffer;
        private readonly ITextView _textView;

        public SuggestedActionsSource(SuggestedActionsSourceProvider testSuggestedActionsSourceProvider, ITextView textView, ITextBuffer textBuffer)
        {
            _factory = testSuggestedActionsSourceProvider;
            _textBuffer = textBuffer;
            _textView = textView;
        }

#pragma warning disable 0067
        public event EventHandler<EventArgs> SuggestedActionsChanged;
#pragma warning restore 0067

        public void Dispose()
        {
        }

        public IEnumerable<SuggestedActionSet> GetSuggestedActions(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken)
        {
            if (TryGetLineUnderCaret(out TextExtent extent) && extent.IsSignificant)
            {
                ITrackingSpan trackingSpan = range.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
                var copyMethodNameAction = new CopyMethodName(trackingSpan);
                return new SuggestedActionSet[] { new SuggestedActionSet("Gherkin Category", new ISuggestedAction[] { copyMethodNameAction }, "Gherkin Actions", SuggestedActionSetPriority.Medium) };
            }
            return Enumerable.Empty<SuggestedActionSet>();
        }

        public Task<bool> HasSuggestedActionsAsync(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                if (TryGetLineUnderCaret(out TextExtent extent))
                {
                    // don't display the tag if the extent has whitespace
                    return extent.IsSignificant;
                }
                return false;
            });
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = Guid.Empty;
            return false;
        }

        private bool TryGetLineUnderCaret(out TextExtent wordExtent)
        {
            ITextCaret caret = _textView.Caret;
            SnapshotPoint point;

            if (caret.Position.BufferPosition > 0)
            {
                point = caret.Position.BufferPosition - 1;
            }
            else
            {
                wordExtent = default(TextExtent);
                return false;
            }

            ITextStructureNavigator navigator = _factory.NavigatorService.GetTextStructureNavigator(_textBuffer);

            wordExtent = navigator.GetExtentOfWord(point);
            return true;
        }
    }
}
