using SVGtoWEBP;

const string svgUrl = "https://www.svgrepo.com/download/288629/ram.svg";
const string assetsDirectory = "src/assets";
const int webpWidth = 500;
const int webpHeight = 500;

var directoryInfo = Directory.CreateDirectory(assetsDirectory);

// Fetch SVG from URL
var svgContent = await SvGtoWebp.FetchSvg(svgUrl);

// Validate and save SVG
if (SvGtoWebp.IsSvg(svgContent)) {
	var svgPath = Path.Combine(assetsDirectory, $"{Guid.NewGuid()}.svg");
	File.WriteAllText(svgPath, svgContent);

	// Convert SVG to WEBP
	var webpPath = Path.Combine(assetsDirectory, $"{Guid.NewGuid()}.webp");
	SvGtoWebp.ConvertSvgToWebp(svgPath, webpPath, webpWidth, webpHeight);
} else {
	Console.WriteLine("Invalid SVG content.");
}