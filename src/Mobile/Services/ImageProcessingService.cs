using System;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace Microsoft.NetConf2021.Maui.Services;

public class ImageProcessingService
{
    private readonly HttpClient httpClient = new HttpClient();

    private readonly RateLimiter rateLimiter = new ConcurrencyLimiter(new ConcurrencyLimiterOptions { PermitLimit = 3, QueueProcessingOrder = QueueProcessingOrder.NewestFirst });

    private readonly object cacheDirectoryLock = new object();

    private readonly string imageCacheDirectory;

    public ImageProcessingService()
    {
        imageCacheDirectory = Path.Combine(FileSystem.CacheDirectory, "remoteimages");
    }

    public async Task<string> ProcessRemoteImage(Uri imageUri, int maxHeight = 250, int maxWidth = 250, int quality = 75, SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Png)
    {
        var imageUriHash = $"{Crc64.ComputeHashString(imageUri.ToString())}.{imageFormat}".ToLowerInvariant();

        var cachedImagePath = Path.Combine(imageCacheDirectory, imageUriHash);

        lock (cacheDirectoryLock)
        {
            if (!Directory.Exists(imageCacheDirectory))
            {
                Directory.CreateDirectory(imageCacheDirectory);
            }
        }

        if (File.Exists(cachedImagePath))
        {
            return cachedImagePath;
        }

        RateLimitLease lease = rateLimiter.AttemptAcquire();

        while (!lease.IsAcquired)
        {
            lease = await rateLimiter.AcquireAsync().ConfigureAwait(false);
        }

        SKBitmap bmp = null;

        try
        {
            using var imageStream = await httpClient.GetStreamAsync(imageUri).ConfigureAwait(false);
            using var data = SKData.Create(imageStream);
            using var codec = SKCodec.Create(data);

            var info = codec.Info;

            var maxSize = Math.Max(maxHeight, maxWidth);

            var supportedScale =
                info.Height > info.Width
                    ? (float)maxHeight / info.Height
                    : (float)maxWidth / info.Width;

            var scaledWidth = (int)(info.Width * supportedScale);
            var scaledHeight = (int)(info.Height * supportedScale);

            // decode the bitmap at the nearest size
            var nearest = new SKImageInfo(scaledWidth, scaledHeight);

            bmp = SKBitmap.Decode(codec);

            SKImageInfo desired = new SKImageInfo(scaledWidth, scaledHeight);
            bmp = bmp.Resize(desired, SKFilterQuality.High);

            using var image = SKImage.FromBitmap(bmp);
            using var encodedImage = image.Encode(imageFormat, quality);
            using var encodedImageStream = encodedImage.AsStream();
            using var file = File.Create(cachedImagePath);
            await encodedImageStream.CopyToAsync(file).ConfigureAwait(false);
        }
        finally
        {
            bmp?.Dispose();
            bmp = null;

            lease.Dispose();
        }

        return cachedImagePath;
    }
}

