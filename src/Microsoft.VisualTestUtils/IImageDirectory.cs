// Copyright (c) Bret Johnson. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualTestUtils
{
    /// <summary>
    /// Used to read and write images to the baseline and output diff images directories.
    /// </summary>
    public interface IImageDirectory
    {
        /// <summary>
        /// Gets the baseline images directory.
        /// </summary>
        string BaselineImagesDirectory { get; }

        /// <summary>
        /// Gets the output diff images directory.
        /// </summary>
        string OutputDiffImagesDirectory { get; }

        /// <summary>
        /// Reads an image from the baseline images directory.
        /// </summary>
        /// <param name="fileName">The name of the image file.</param>
        /// <param name="imageBytes">The byte array to store the image data.</param>
        /// <returns>True if reading was successful; false otherwise.</returns>
        bool ReadImageFromBaselineDirectory(string fileName, out byte[] imageBytes);

        /// <summary>
        /// Writes the given image bytes to the output diff images directory.
        /// </summary>
        /// <param name="fileName">The name of the file to write.</param>
        /// <param name="imageBytes">The image bytes to write.</param>
        /// <returns>True if writing was successful; false otherwise.</returns>
        bool WriteImageToDiffDirectory(string fileName, byte[] imageBytes);

        /// <summary>
        /// Writes the given image bytes to the output diff images directory.
        /// </summary>
        /// <param name="fileName">The name of the file to write.</param>
        /// <returns>True if writing was successful; false otherwise.</returns>
        bool WriteImageToDiffDirectory(string fileName);
    }
}
