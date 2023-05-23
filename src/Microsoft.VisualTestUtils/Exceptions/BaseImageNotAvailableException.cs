// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualTestUtils
{
    /// <summary>
    /// Thrown when the baseline image has not been created or is not available.
    /// </summary>
    public class BaseImageNotAvailableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseImageNotAvailableException"/> class.
        /// Thrown when the baseline image has not been created or is not available.
        /// </summary>
        /// <param name="baselineDirectory">Directory path to the baseline directory.</param>
        /// <param name="diffsDirectory">Directory path to the diffs directory.</param>
        /// <param name="imageFileName">Name of the image file.</param>
        public BaseImageNotAvailableException(string baselineDirectory, string diffsDirectory, string imageFileName)
            : base($"Baseline snapshot not yet created: {Path.Combine(baselineDirectory, imageFileName)}\n" +
                    $"Ensure new snapshot is correct:    {Path.Combine(diffsDirectory, imageFileName)}\n" +
                    $"and if so, copy it to your baseline directory")
        {
        }
    }
}
