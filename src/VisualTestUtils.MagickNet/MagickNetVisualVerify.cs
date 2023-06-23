// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ImageMagick;

namespace VisualTestUtils.MagickNet
{
    /// <summary>
    /// Verify images using ImageMagick.
    /// </summary>
    public class MagickNetVisualVerify : IVisualVerify
    {
        private ErrorMetric error;
        private MagickFormat magickFormat;
        private IImageDirectory imageDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickNetVisualVerify"/> class.
        /// </summary>
        /// <param name="imageDirectory">Directory info.</param>
        /// <param name="error">Error metric.</param>
        /// <param name="magickFormat">Image format.</param>
        public MagickNetVisualVerify(IImageDirectory imageDirectory, ErrorMetric error = ErrorMetric.Fuzz, MagickFormat magickFormat = MagickFormat.Png)
        {
            this.imageDirectory = imageDirectory;
            this.error = error;
            this.magickFormat = magickFormat;
        }

        /// <inheritdoc/>
        public double Verify(ImageSnapshot baselineImage, ImageSnapshot actualImage)
        {
            var magickBaselineImage = new MagickImage(baselineImage.Data);
            var magickActualImage = new MagickImage(actualImage.Data);

            return magickBaselineImage.Compare(magickActualImage, this.error, Channels.Red) * 100.0;
        }
    }
}
