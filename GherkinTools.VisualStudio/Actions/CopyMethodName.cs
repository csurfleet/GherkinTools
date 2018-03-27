using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GherkinTools.VisualStudio.Actions
{
    /// <summary>A light bulb suggestion allowing a gherkin line to be copied in a form that can be used as a method name.</summary>
    internal class CopyMethodName : ISuggestedAction
    {
        private readonly ITrackingSpan _span;
        private readonly ITextSnapshot _snapshot;
        private readonly string _textBeforeConversion;
        private string _textAfterConversion;
        private const string _display = "Copy a C#-method name-compatible version of this step to the clipboard";

        public CopyMethodName(ITrackingSpan span)
        {
            _span = span;
            _snapshot = span.TextBuffer.CurrentSnapshot;
            _textBeforeConversion = span.GetText(_snapshot);
        }

        public string DisplayText => _display;

        public string IconAutomationText => null;

        public string TextAfterConversion => _textAfterConversion ?? (_textAfterConversion = _textBeforeConversion.Replace(' ', '_').ToLower());

        ImageMoniker ISuggestedAction.IconMoniker => default(ImageMoniker);

        public string InputGestureText => null;

        public bool HasActionSets => false;

        public Task<IEnumerable<SuggestedActionSet>> GetActionSetsAsync(CancellationToken cancellationToken) => null;

        public bool HasPreview => true;

        public Task<object> GetPreviewAsync(CancellationToken cancellationToken)
        {
            var textBlock = new TextBlock();
            textBlock.Padding = new Thickness(5);
            textBlock.Inlines.Add(new Run() { Text = TextAfterConversion });
            return Task.FromResult<object>(textBlock);
        }

        public void Dispose()
        {
        }

        public void Invoke(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            _span.TextBuffer.Replace(_span.GetSpan(_snapshot), TextAfterConversion);
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = Guid.Empty;
            return false;
        }
    }
}
