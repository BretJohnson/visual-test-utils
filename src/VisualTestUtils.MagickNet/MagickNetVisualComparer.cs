// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ImageMagick;

namespace VisualTestUtils.MagickNet
{
    /// <summary>
    /// Verify images using ImageMagick.
    /// </summary>
    public class MagickNetVisualComparer : IVisualComparer
    {
        private ErrorMetric errorMetric;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickNetVisualComparer"/> class.
        /// </summary>
        /// <param name="errorMetric">Error metric.</param>
        public MagickNetVisualComparer(ErrorMetric errorMetric = ErrorMetric.Fuzz)
        {
            this.errorMetric = errorMetric;
        }

        /// <inheritdoc/>
        public double Compare(ImageSnapshot baselineImage, ImageSnapshot actualImage)
        {
            var magickBaselineImage = new MagickImage(baselineImage.Data);
            var magickActualImage = new MagickImage(actualImage.Data);

            return magickBaselineImage.Compare(magickActualImage, this.errorMetric, Channels.Red) * 100.0;
        }
    }
}
