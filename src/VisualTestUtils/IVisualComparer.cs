namespace VisualTestUtils
{
    /// <summary>
    /// Interface for image visual comparison.
    /// </summary>
    public interface IVisualComparer
    {
        /// <summary>
        /// Compare the image against the provided baseline, returning the percentage difference (1.0 = 100%).
        /// </summary>
        /// <param name="baselineImage">Baseline Image Bytes.</param>
        /// <param name="actualImage">Actual Image Bytes.</param>
        /// <returns>Percentage difference.</returns>
        double Compare(ImageSnapshot baselineImage, ImageSnapshot actualImage);
    }
}
