// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ImageMagick;

namespace Microsoft.VisualTestUtils.MagickNet
{
    /// <summary>
    /// Verify images using ImageMagick.
    /// </summary>
    public class MagickNetImageVerify : IImageVerify
    {
        private ErrorMetric error;
        private MagickFormat magickFormat;
        private IImageDirectory imageDirectory;

        public MagickNetImageVerify(IImageDirectory imageDirectory, ErrorMetric error = ErrorMetric.Fuzz, MagickFormat magickFormat = MagickFormat.Png)
        {
            this.imageDirectory = imageDirectory;
            this.error = error;
            this.magickFormat = magickFormat;
        }

        /// <inheritdoc/>
        public bool VerifyImage(byte[] baselineImageBytes, byte[] actualImageBytes, string imageFileName, double percentageDifferenceThreshold, out double percentageDifference)
        {
            MagickImage baselineImage = new MagickImage(baselineImageBytes);
            MagickImage actualImage = new MagickImage(actualImageBytes);

            MagickImage diffImage = new MagickImage();
            diffImage.Format = this.magickFormat;

            percentageDifference = baselineImage.Compare(actualImage, this.error, diffImage, Channels.Red) * 100.0;

            if (percentageDifference > percentageDifferenceThreshold)
            {
                this.imageDirectory.WriteImageToDiffDirectory(imageFileName);
                this.imageDirectory.WriteImageToDiffDirectory(Path.GetFileNameWithoutExtension(imageFileName) + "-diff.png");
                return false;
            }

            return true;
        }
    }
}
