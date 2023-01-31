using OwlStock.Domain;
using OwlStock.Domain.Enumerations;
using OwlStock.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace OwlStock.Services
{
    public class PhotoResizer : IPhotoResizer
    {
        public Photo Resize(Photo photo, PhotoSize photoSize)
        {
            // memory stream
            using var memoryStream = new MemoryStream();
            // file stream
            using var image = Image.Load(photo?.FileData);
            
            IResampler sampler = KnownResamplers.Lanczos3;
            bool compand = true;
            ResizeMode mode = ResizeMode.Stretch;

            var resizeOptions = new ResizeOptions
            {
                Size = GetSize(new Size() { Width = image.Width, Height = image.Height }, photoSize),
                Sampler = sampler,
                Compand = compand,
                Mode = mode
            };

            image.Mutate(x => x.Resize(resizeOptions));

            //Encode here for quality
            var encoder = new JpegEncoder()
            {
                Quality = 30 //between 5-30
            };

            //This saves to the memoryStream with encoder
            image.Save(memoryStream, encoder);
            memoryStream.Position = 0; // The position needs to be reset.

            // prepare result to byte[]
            return new Photo()
            {
                Name = photo?.Name,
                FileName = photo?.FileName,
                Description = photo?.Description,
                FileType = photo?.FileType,
                FileData = memoryStream.ToArray()
            };
        }

        private Size GetSize(Size originalSize, PhotoSize newSize)
        {
            switch (newSize)
            {
                case PhotoSize.Small:
                {
                    originalSize.Width = originalSize.Width / 3;
                    originalSize.Height = originalSize.Height / 3;

                    return originalSize;
                }

                case PhotoSize.Medium:
                {
                    originalSize.Width = originalSize.Width / 2;
                    originalSize.Height = originalSize.Height / 2;

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
