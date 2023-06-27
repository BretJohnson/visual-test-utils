namespace VisualTestUtils
{
    public class VisualRegressionTester
    {
        private readonly string snapshotsBaselineDirectory;
        private readonly string snapshotsDiffDirectory;
        private readonly IVisualComparer visualComparer;
        private readonly double percentDifferenceThreshold;
        private readonly IVisualDiffGenerator visualDiffGenerator;

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
