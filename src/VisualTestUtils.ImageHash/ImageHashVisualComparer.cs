// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using VisualTestUtils.ImageHash.HashAlgorithms;

namespace VisualTestUtils.ImageHash
{
    /// <summary>
    /// Verify images using SkiaSharp.
    /// </summary>
    public class ImageHashVisualComparer : IVisualComparer
    {
        private IHashAlgorithm hashAlgorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageHashVisualComparer"/> class, defaulting
        /// to use the <see cref="DifferenceHash"/> algorithm.
        /// </summary>
        public ImageHashVisualComparer()
            : this(new DifferenceHash())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageHashVisualComparer"/> class.
        /// </summary>
        /// <param name="hashAlgorithm">The Hash Algorithm.</param>
        public ImageHashVisualComparer(IHashAlgorithm hashAlgorithm)
        {
            this.hashAlgorithm = hashAlgorithm;
        }

        /// <inheritdoc/>
        public double Compare(ImageSnapshot baselineImage, ImageSnapshot actualImage) =>
            CompareHash.Similarity(this.hashAlgorithm.Hash(baselineImage.Data), this.hashAlgorithm.Hash(actualImage.Data)) / 100.0;
    }
}
