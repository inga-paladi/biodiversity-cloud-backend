namespace BiodiversityCloudApp.Common;

public static class PhotoMimeType
{
    public const string Jpeg = "image/jpeg";
    public const string Png = "image/png";
    public const string Gif = "image/gif";
    public const string Bmp = "image/bmp";
    public const string Tiff = "image/tiff";
    public const string Webp = "image/webp";

    public static readonly string[] SupportedMimeTypes = [
        Jpeg,
        Png,
        Gif,
        Bmp,
        Tiff,
        Webp
    ];
}
