// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace VisualTestUtils.ImageHash
{
    /// <summary>
    /// Verify images using SkiaSharp.
    /// </summary>
    public class ImageHashImageVerify : IImageVerify
    {
        private IImageHash hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageHashImageVerify"/> class.
        /// </summary>
        /// <param name="hash">The Hash Algorithm.</param>
        public ImageHashImageVerify(IImageHash hash)
        {
            this.hash = hash;
        }

        /// <inheritdoc/>
        public bool VerifyImage(byte[] baselineImageBytes, byte[] actualImageBytes, string imageFileName, double percentageDifferenceThreshold, out double percentageDifference)
        {
            percentageDifference = CompareHash.Similarity(this.hash.Hash(baselineImageBytes), this.hash.Hash(actualImageBytes));
            return percentageDifference >= percentageDifferenceThreshold;
        }
    }
}
