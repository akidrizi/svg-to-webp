using System.Text.RegularExpressions;
using SkiaSharp;
using Svg.Skia;

namespace SVGtoWEBP;

public static partial class SvGtoWebp {

	public static async Task<string> FetchSvg(string url) {
		using var client = new HttpClient();
		var response = await client.GetAsync(url);
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadAsStringAsync();
	}

	public static bool IsSvg(string content) {
		return SvgRegex().IsMatch(content);
	}

	public static void ConvertSvgToWebp(string svgPath, string webpPath, int width, int height) {
		using var stream = File.OpenRead(svgPath);
		var svg = new SKSvg();
		var picture = svg.Load(stream);

		if (picture != null) {
			using var bitmap = new SKBitmap(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul));
			using var canvas = new SKCanvas(bitmap);
			canvas.Clear(SKColors.Transparent);

			var xRatio = width / picture.CullRect.Width;
			var yRatio = height / picture.CullRect.Height;
			var ratio = Math.Min(xRatio, yRatio);
			var x = (width - ratio * picture.CullRect.Width) / 2;
			var y = (height - ratio * picture.CullRect.Height) / 2;

			canvas.Scale(ratio);
			canvas.Translate(x, y);
			canvas.DrawPicture(picture);
			canvas.Flush();

			using var image = SKImage.FromBitmap(bitmap);
			using var data = image.Encode(SKEncodedImageFormat.Webp, 100);
			using var outputStream = File.OpenWrite(webpPath);
			data.SaveTo(outputStream);
		} else {
			Console.WriteLine("Failed to load SVG.");
		}
	}

	[GeneratedRegex(@"<svg[^>]*>(.*?)<\/svg>", RegexOptions.Singleline)]
	private static partial Regex SvgRegex();

}