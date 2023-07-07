namespace VisualTestUtils
{
    public class VisualRegressionTester
    {
        private readonly string snapshotsDirectory;
        private readonly string snapshotsDiffDirectory;
        private readonly IVisualComparer visualComparer;
        private readonly double failureThreshold;
        private readonly IVisualDiffGenerator visualDiffGenerator;

        /// <summary>
        /// Initialize visual regression testing, configured as specified.
        /// </summary>
        /// <param name="testRootDirectory">The root directory for the tests. This directory should have a "snapshots" subdirectory with the baseline images.</param>
        /// <param name="visualComparer">The instance of <see cref="IVisualComparer"/> that will be used for image comparison.</param>
        /// <param name="visualDiffGenerator">The instance of <see cref="IVisualDiffGenerator"/> that will be used for generating image diff.</param>
        /// <param name="failureThreshold">The maximum percent difference that is allowed between the baseline and actual snapshot images. Default value is .01, meaning the images must be at least 99% the same.).</param>
        /// <param name="ciArtifactsDirectory">If running in CI, this should be set to the CI artifacts directory. When running locally, it can be null (the default). If specified, the "snapshots-diff" subdirectory will be created here,
        /// holding any regression test failures. If not specified, "snapshots-diff" will be created in <paramref name="testRootDirectory"/>. </param>
        public VisualRegressionTester(string testRootDirectory, IVisualComparer visualComparer, IVisualDiffGenerator visualDiffGenerator, double failureThreshold = 0.01, string? ciArtifactsDirectory = null)
        {
            this.snapshotsDirectory = Path.Combine(testRootDirectory, "snapshots");
            this.snapshotsDiffDirectory = Path.Combine(ciArtifactsDirectory ?? testRootDirectory, "snapshots-diff");

            this.visualComparer = visualComparer;
            this.visualDiffGenerator = visualDiffGenerator;
            this.failureThreshold = failureThreshold;
        }

        /// <summary>
        /// Test the actual image by comparing it to the baseline image, failing if the image is different or the baseline does not exist.
        /// On failure, the actual image will be saved to the "snapshots-diff" directory along with a diff image (if there is one) visually showing
        /// what parts of the image are different.
        /// </summary>
        /// <param name="name">Image name (with no extension); it's often the same as the test name.</param>
        /// <param name="actualImage">Actual image screenshot.</param>
        /// <param name="environmentName">Optional name for the test environment (e.g. device type, like
        /// "android"). If present it's used as the parent directory for the images. It not present, all images are stored directly in the "snapshots" directory.</param>
        public virtual void Test(string name, ImageSnapshot actualImage, string? environmentName = null)
        {
            string imageFileName = $"{name}{actualImage.Format.GetFileExtension()}";

            string snapshotsEnvironmentDirectory = GetEnvironmentDirectory(this.snapshotsDirectory, environmentName);
            string baselineImagePath = Path.Combine(snapshotsEnvironmentDirectory, imageFileName);

            string diffEnvironmentDirectory = GetEnvironmentDirectory(this.snapshotsDiffDirectory, environmentName);

            if (!File.Exists(baselineImagePath))
            {
                Directory.CreateDirectory(diffEnvironmentDirectory);
                actualImage.Save(diffEnvironmentDirectory, name);

                this.Fail(
                    $"Baseline snapshot not yet created: {baselineImagePath}\n" +
                    $"Ensure new snapshot is correct:    {Path.Combine(diffEnvironmentDirectory, imageFileName)}\n" +
                    $"and if so, copy it to the snapshots directory." +
                    $"\n" +
                    $"Command: vdiff {this.snapshotsDirectory} {this.snapshotsDiffDirectory}\n");

                return;
            }

            var baselineImage = new ImageSnapshot(baselineImagePath);

            double percentDifference = this.visualComparer.Compare(baselineImage, actualImage);
            if (percentDifference > this.failureThreshold)
            {
                Directory.CreateDirectory(diffEnvironmentDirectory);
                actualImage.Save(diffEnvironmentDirectory, name);

                ImageSnapshot diffImage = this.visualDiffGenerator.GenerateDiff(baselineImage, actualImage);
                diffImage.Save(diffEnvironmentDirectory, name + "-diff");

                string formattedPercentDifference = string.Format("{0:0.00}", percentDifference * 100.0);
                this.Fail(
                    $"Snapshot different than baseline: {imageFileName} ({formattedPercentDifference}% difference)\n" +
                    $"If the correct baseline has changed (this isn't a a bug), then update the baseline image.\n" +
                    $"\n" +
                    $"Command: vdiff {this.snapshotsDirectory} {this.snapshotsDiffDirectory}\n");
            }
            else
            {
                // If the test passed, delete any previous diff image
                Directory.Delete(actualImage.GetFilePath(diffEnvironmentDirectory, name));
                Directory.Delete(Path.Combine(diffEnvironmentDirectory, name + "-diff.png"));
            }
        }

        public static string GetEnvironmentDirectory(string baseDirectoryName, string? environmentName) =>
            environmentName == null ? baseDirectoryName : Path.Combine(baseDirectoryName, environmentName);

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
