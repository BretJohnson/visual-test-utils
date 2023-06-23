// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace VisualTestUtils
{
    /// <summary>
    /// Interface for image verification.
    /// </summary>
    public interface IVisualVerify
    {
        /// <summary>
        /// Verifies the image against the provided baseline.
        /// </summary>
        /// <param name="baselineImage">Baseline Image Bytes.</param>
        /// <param name="actualImage">Actual Image Bytes.</param>
        /// <returns>Percentage difference.</returns>
        double Verify(ImageSnapshot baselineImage, ImageSnapshot actualImage);
    }
}
