using PdfSharp.Fonts;



public class CustomFontResolver : IFontResolver
{
    public byte[] GetFont(string faceName)
    {
        if (faceName == "Arial")
        {
            return File.ReadAllBytes("/usr/share/fonts/truetype/msttcorefonts/arial.ttf");
        }
        throw new ArgumentException($"Font {faceName} not found.");
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        return new FontResolverInfo("Arial");
    }
}
