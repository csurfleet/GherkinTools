using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace GherkinTools.VisualStudio
{
    /// <summary>Classifier that classifies all text as an instance of the "GherkinClassifier" classification type.</summary>
    internal class GherkinClassifier : IClassifier
    {
        /// <summary>Classification type.</summary>
        private readonly IClassificationType classificationType;

        /// <summary>Initializes a new instance of the <see cref="GherkinClassifier"/> class.</summary>
        /// <param name="registry">Classification registry.</param>
        internal GherkinClassifier(IClassificationTypeRegistryService registry)
        {
            classificationType = registry.GetClassificationType(GherkinClassifierFormat.Name);
        }

        #region IClassifier

#pragma warning disable 67

        /// <summary>An event that occurs when the classification of a span of text has changed.</summary>
        /// <remarks>
        /// This event gets raised if a non-text change would affect the classification in some way,
        /// for example typing /* would cause the classification to change in C# without directly
        /// affecting the span.
        /// </remarks>
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        private static Regex _keywordRegex;

        private Regex KeywordRegex
            => _keywordRegex ??
            (_keywordRegex = new Regex(@"^\s*(Feature:|FEATURE:|Scenario:|SCENARIO:|Given|GIVEN|When|WHEN|Then|THEN|And|AND)\s+"));

#pragma warning restore 67

        /// <summary>Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text./summary>
        /// <remarks>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </remarks>
        /// <param name="span">The span currently being classified.</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var result = new List<ClassificationSpan>();

            var text = span.GetText();
            var match = KeywordRegex.Match(text);

            while (match.Success)
            {
                result.Add(new ClassificationSpan(
                    new SnapshotSpan(span.Snapshot, new Span(span.Start + match.Groups[1].Index, match.Groups[1].Length)),
                    classificationType));

                match = match.NextMatch();
            }

            return result;
        }

        #endregion
    }
}