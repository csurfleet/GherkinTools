using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace GherkinTools.VisualStudio
{
    /// <summary>Classification type definition export for GherkinClassifier</summary>
    internal static class GherkinClassifierClassificationDefinition
    {
        // This disables "The field is never used" compiler's warning. Justification: the field is used by MEF.
#pragma warning disable 169

        /// <summary>Defines the "GherkinClassifier" classification type.</summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name(GherkinClassifierFormat.Name)]
        private static ClassificationTypeDefinition typeDefinition;

#pragma warning restore 169
    }
}
