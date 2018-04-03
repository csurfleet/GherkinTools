using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace GherkinTools.VisualStudio
{
    internal static class FileAndContentTypes
    {
        public const string GherkinFormat = "gherkinformat";

        // Disable "Field is never assigned to..." compiler's warning. Justification: the fields are assigned by MEF.
#pragma warning disable 649

        /// <summary>This is the glue that sticks <see cref="gherkinFileExtensionDefinition"/> to <see cref="GherkinClassifierProvider"/>.</summary>
        [Export]
        [Name(GherkinFormat)]
        [BaseDefinition("text")]
        internal static ContentTypeDefinition gherkinContentTypeDefinition;

        /// <summary>Maps the gherkinformat to files with the .feature extension.</summary>
        [Export]
        [FileExtension(".feature")]
        [ContentType(GherkinFormat)]
        internal static FileExtensionToContentTypeDefinition gherkinFileExtensionDefinition;

#pragma warning restore 649
    }
}
