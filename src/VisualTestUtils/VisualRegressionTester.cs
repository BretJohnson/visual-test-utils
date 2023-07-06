namespace VisualTestUtils
{
    public class VisualRegressionTester
    {
        private readonly string snapshotsBaselineDirectory;
        private readonly string snapshotsDiffDirectory;
        private readonly IVisualComparer visualComparer;
        private readonly double percentDifferenceThreshold;
        private readonly IVisualDiffGenerator visualDiffGenerator;

        /// <summary>
        /// Initialize visual regression testing, configured as specified.
        /// </summary>
        /// <param name="testRootDirectory">The root directory for the tests. This directory should have a "snapshots-baseline" subdirectory with the baseline images.</param>
        /// <param name="visualComparer">The instance of <see cref="IVisualComparer"/> that will be used for image comparison.</param>
        /// <param name="visualDiffGenerator">The instance of <see cref="IVisualDiffGenerator"/> that will be used for generating image diff.</param>
        /// <param name="percentDifferenceThreshold">The maximum percent difference that is allowed between the baseline and actual snapshot images. Default value is 1.0, meaning the images must be at least 99% the same.).</param>
        /// <param name="ciArtifactsDirectory">If running in CI, this should be set to the CI artifacts directory. When running locally, it can be null (the default). If specified, the "snapshots-diff" subdirectory will be created here,
        /// holding any regression test failures. If not specified, "snapshots-diff" will be created in <paramref name="testRootDirectory"/>. </param>
        public VisualRegressionTester(string testRootDirectory, IVisualComparer visualComparer, IVisualDiffGenerator visualDiffGenerator, double percentDifferenceThreshold = 1.0, string? ciArtifactsDirectory = null)
        {
            this.snapshotsBaselineDirectory = Path.Combine(testRootDirectory, "snapshots-baseline");
            this.snapshotsDiffDirectory = Path.Combine(ciArtifactsDirectory ?? testRootDirectory, "snapshots-diff");

            this.visualComparer = visualComparer;
            this.visualDiffGenerator = visualDiffGenerator;
            this.percentDifferenceThreshold = percentDifferenceThreshold;
        }

        public virtual void Test(string name, ImageSnapshot actualImage)
        {
            string imageFileName = $"{name}{actualImage.Format.GetFileExtension()}";

            string baselineImagePath = Path.Combine(this.snapshotsBaselineDirectory, imageFileName);

            if (!File.Exists(baselineImagePath))
            {
                Directory.CreateDirectory(this.snapshotsDiffDirectory);

                actualImage.Save(this.snapshotsDiffDirectory, name);

                this.Fail(
                    $"Baseline snapshot not yet created: {Path.Combine(this.snapshotsBaselineDirectory, imageFileName)}\n" +
                    $"Ensure new snapshot is correct:    {Path.Combine(this.snapshotsDiffDirectory, imageFileName)}\n" +
                    $"and if so, copy it to the snapshots-baseline directory." +
                    $"\n" +
                    $"Command: vdiff {this.snapshotsBaselineDirectory} {this.snapshotsDiffDirectory}\n");

                return;
            }

            ImageSnapshot baselineImage = new ImageSnapshot(baselineImagePath);

            double percentDifference = this.visualComparer.Compare(baselineImage, actualImage);
            if (percentDifference > this.percentDifferenceThreshold)
            {
                string formattedPercentDifference = string.Format("{0:0.00}", percentDifference);
                this.Fail(
                    $"Snapshot different than baseline: {imageFileName} ({formattedPercentDifference}% difference)\n" +
                    $"If the correct baseline has changed (this isn't a a bug), then update the baseline image.\n" +
                    $"\n" +
                    $"Command: vdiff {this.snapshotsBaselineDirectory} {this.snapshotsDiffDirectory}\n");
                return;
            }
        }

        public void Fail(string message)
        {
            // For multiline messages, ensure they start on a new line to be better formatted in VS test explorer results
            if (message.Contains('\n') && !message.StartsWith("\n"))
            {
                message = "\n" + message;
            }

            throw new VisualTestFailedException(message);
        }
    }
}
