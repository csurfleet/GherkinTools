using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace GherkinTools.VisualStudio
{
    internal static class FileAndContentTypes
    {
        /// <summary>This is the glue that sticks <see cref="gherkinFileExtensionDefinition"/> to <see cref="GherkinClassifierProvider"/>.</summary>
        [Export]
        [Name("gherkinformat")]
        [BaseDefinition("text")]
        internal static ContentTypeDefinition gherkinContentTypeDefinition;

        /// <summary>Maps the gherkinformat to files with the .feature extension.</summary>
        [Export]
        [FileExtension(".feature")]
        [ContentType("gherkinformat")]
        internal static FileExtensionToContentTypeDefinition gherkinFileExtensionDefinition;
    }
}
