// Copyright (c) Bret Johnson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualTestUtils.SkiaSharp.ImageHash
{
    /// <summary>
    /// Verify images using SkiaSharp.
    /// </summary>
    public class SkiaSharpImageVerify : IImageVerify
    {
        private IImageHash hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaSharpImageVerify"/> class.
        /// </summary>
        /// <param name="hash">The Hash Algorithm.</param>
        public SkiaSharpImageVerify(IImageHash hash)
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
