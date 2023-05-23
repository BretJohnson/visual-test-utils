// Copyright (c) Bret Johnson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualTestUtils
{
    /// <summary>
    /// Handles Screenshots.
    /// </summary>
    public interface IScreenshot
    {
        /// <summary>
        /// Captures a screenshot of the current state of the test device.
        /// </summary>
        /// <param name="imageBytes">Output parameter containing the byte array of the screenshot image.</param>
        /// <returns>True if the screenshot was taken successfully, false otherwise.</returns>
        bool TakeScreenshot(out byte[]? imageBytes);
    }
}
