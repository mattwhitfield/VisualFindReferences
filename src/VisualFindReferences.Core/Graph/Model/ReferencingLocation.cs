using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;

namespace VisualFindReferences.Core.Graph.Model
{
    public class ReferencingLocation
    {
        public ReferencingLocation(ReferenceLocation location, SourceText referencingSourceText)
        {
            Location = location;
            ReferencingSourceText = referencingSourceText;
            _lineIndex = ReferencingSourceText.Lines.IndexOf(Location.Location.SourceSpan.Start);
        }

        private int _lineIndex;

        public string Text
        {
            get
            {
                if (_lineIndex >= 0)
                {
                    var line = ReferencingSourceText.Lines[_lineIndex];
                    return ReferencingSourceText.GetSubText(line.Span).ToString().Trim();
                }

                return string.Empty;
            }
        }

        public string LinePrompt
        {
            get
            {
                var document = Location.Document.Name;
                if (string.IsNullOrWhiteSpace(document))
                {
                    document = "Unknown file";
                }

                if (_lineIndex >= 0)
                {
                    return document + ", Line: " + (_lineIndex + 1).ToString();
                }

                return document + ", Unknown position";
            }
        }

        public ReferenceLocation Location { get; }

        public SourceText ReferencingSourceText { get; }
    }
}
