// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.VisualTestUtils
{
    /// <summary>
    /// Interface for image verification.
    /// </summary>
    public interface IImageVerify
    {
        /// <summary>
        /// Verifies the image against the provided baseline.
        /// </summary>
        /// <param name="baselineImageBytes">Baseline Image Bytes.</param>
        /// <param name="actualImageBytes">Actual Image Bytes.</param>
        /// <param name="imageFileName">Baseline Image Filename.</param>
        /// <param name="percentageDifferenceThreshold">Percentage Difference Threshold.</param>
        /// <param name="percentageDifference">The difference as a percentage.</param>
        /// <returns>Bool.</returns>
        bool VerifyImage(byte[] baselineImageBytes, byte[] actualImageBytes, string imageFileName, double percentageDifferenceThreshold, out double percentageDifference);
    }
}
