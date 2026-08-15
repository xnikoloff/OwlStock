using OwlStock.Domain.Enumerations;
using OwlStock.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace OwlStock.Services.Implementations
{
    public class PhotoResizer : IPhotoResizer
    {
        /// <summary>
        /// Resized a photo file based on a given size
        /// </summary>
        /// <param name="fileData">The photo file as a byte array</param>
        /// <param name="photoSize">The requested size</param>
        /// <returns>Byte array of the modified photo file</returns>
        public byte[] Resize(byte[] fileData, PhotoSize photoSize)
        {
            using var image = Image.Load(fileData);

            image.Mutate(x => x.Resize(GetSize(new Size(image.Width, image.Height), photoSize)));

            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, new JpegEncoder());

            // prepare result to byte[]
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Gets the resize properties.
        /// </summary>
        /// <param name="originalSize">The original size of the file</param>
        /// <param name="newSize">The requested new size of the file</param>
        /// <returns>Object of type Size containing the data of the new size</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid size is requested</exception>
        private static Size GetSize(Size originalSize, PhotoSize newSize)
        {
            //small size returns three times smaller dimentions
            switch (newSize)
            {
                case PhotoSize.Small:
                {
                    originalSize.Width /= 3;
                    originalSize.Height /= 3;

                    return originalSize;
                }

                //medium size returns two times smaller dimentions
                case PhotoSize.Medium:
                {
                    originalSize.Width /= 2;
                    originalSize.Height /= 2;

                    return originalSize;
                }

                //large size returns 1.2 times smaller dimentions
                case PhotoSize.Large:
                {
                    originalSize.Width = (int)Math.Round(originalSize.Width / 1.2);
                    originalSize.Height = (int)Math.Round(originalSize.Height / 1.2);

                    return originalSize;
                }

                //original size returns the original dimentions
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
