using System;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace GherkinTools.VisualStudio
{
    /// <summary>
    /// Defines an editor format for the GherkinClassifier type that has a purple background and is underlined.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Name(Name)]
    [UserVisible(true)] // This should be visible to the end user
    [Order(Before = Priority.Default)] // Set the priority to be after the default classifiers
    internal sealed class GherkinClassifierFormat : ClassificationFormatDefinition
    {
        public const string Name = "GherkinClassifier";

        /// <summary>Initializes a new instance of the <see cref="GherkinClassifierFormat"/> class.</summary>
        public GherkinClassifierFormat()
        {
            DisplayName = Name; // Human readable version of the name
            BackgroundColor = Colors.BlueViolet;
            TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }
}
