using OwlStock.Domain.Enumerations;
using OwlStock.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace OwlStock.Services.Implementations
{
    public class PhotoResizer : IPhotoResizer
    {
        public byte[] Resize(byte[] fileData, PhotoSize photoSize)
        {
            using var image = Image.Load(fileData);

            image.Mutate(x => x.Resize(GetSize(new Size(image.Width, image.Height), photoSize)));

            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, new JpegEncoder());

            // prepare result to byte[]
            return memoryStream.ToArray();
        }

        private static Size GetSize(Size originalSize, PhotoSize newSize)
        {
            switch (newSize)
            {
                case PhotoSize.Small:
                {
                    originalSize.Width /= 3;
                    originalSize.Height /= 3;

                    return originalSize;
                }

                case PhotoSize.Medium:
                {
                    originalSize.Width /= 2;
                    originalSize.Height /= 2;

                    return originalSize;
                }

                case PhotoSize.Large:
                {
                    originalSize.Width = (int)Math.Round(originalSize.Width / 1.2);
                    originalSize.Height = (int)Math.Round(originalSize.Height / 1.2);

                    return originalSize;
                }

                case PhotoSize.OriginalSize:
                {
                    return originalSize;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException($"{newSize} is not a valid value");
                }
            }
        }
    }
}
