using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace GherkinTools.VisualStudio
{
    /// <summary>Classifier that classifies all text as an instance of the "GherkinClassifier" classification type.</summary>
    internal class GherkinClassifier : IClassifier
    {
        private readonly IClassificationType _keywordClassification;
        private readonly IClassificationType _featureTitleClassification;
        private static Regex _keywordRegex;
        private static Regex _featureTitleRegex;

        /// <summary>Initializes a new instance of the <see cref="GherkinClassifier"/> class.</summary>
        /// <param name="registry">Classification registry.</param>
        internal GherkinClassifier(IClassificationTypeRegistryService registry)
        {
            _keywordClassification = registry.GetClassificationType(PredefinedClassificationTypeNames.Keyword);
            _featureTitleClassification = registry.GetClassificationType(PredefinedClassificationTypeNames.String);
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

#pragma warning restore 67

        private Regex KeywordRegex
            => _keywordRegex ??
            (_keywordRegex = new Regex(@"^\s*(Feature:|FEATURE:|Scenario:|SCENARIO:|Given|GIVEN|When|WHEN|Then|THEN|And|AND)\s+"));

        private Regex FeatureTitleRegex
            => _featureTitleRegex ??
            (_featureTitleRegex = new Regex(@"^\s*F(eature|EATURE):([^$\r\n]+)[$\r\n]"));

        /// <summary>Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text./summary>
        /// <remarks>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </remarks>
        /// <param name="span">The span currently being classified.</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
            => GetClassificationSpansForRegex(span, KeywordRegex, 1, _keywordClassification)
                .Concat(GetClassificationSpansForRegex(span, FeatureTitleRegex, 2, _featureTitleClassification))
                .ToList();

        private List<ClassificationSpan> GetClassificationSpansForRegex(SnapshotSpan span, Regex regex, byte groupIndex, IClassificationType classification)
            => regex.Matches(span.GetText()).Cast<Match>().Select(m =>
                new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(span.Start + m.Groups[groupIndex].Index, m.Groups[groupIndex].Length)), classification))
                .ToList();

        #endregion
    }
}