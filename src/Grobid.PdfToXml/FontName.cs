using System;

namespace Grobid.PdfToXml
{
    public class FontName
    {
        private FontName() {}

        public string FullName { get; private set; }

        public string Tag { get; private set; }
        public string Name { get; private set; }
        public string Weight { get; private set; }

        public bool IsItalic
        {
            get
            {
                return this.FullName.IndexOf("italic", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       this.FullName.IndexOf("oblique", StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        public bool IsBold
        {
            get { return this.FullName.IndexOf("bold", StringComparison.OrdinalIgnoreCase) >= 0; }
        }

        public static FontName Parse(string name)
        {
            var fontName = new FontName()
            {
                FullName = name,
            };

            int indexOfPlusSign = name.IndexOf('+');
            int indexOfMinusSign = name.IndexOf('-');

            fontName.Tag = name.Substring(0, indexOfPlusSign);

            if (indexOfMinusSign == -1)
            {
                fontName.Name = name.Substring(indexOfPlusSign + 1);
                fontName.Weight = String.Empty;
            }
            else
            {
                fontName.Name = name.Substring(indexOfPlusSign + 1, indexOfMinusSign - indexOfPlusSign - 1);
                fontName.Weight = name.Substring(indexOfMinusSign + 1, name.Length - indexOfMinusSign - 1);
            }

            return fontName;
        }
    }
}
