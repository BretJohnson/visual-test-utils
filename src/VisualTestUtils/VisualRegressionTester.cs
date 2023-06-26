namespace VisualTestUtils
{
    public class VisualRegressionTester
    {
        private readonly string snapshotsBaselineDirectory;
        private readonly string snapshotsDiffDirectory;
        private readonly IVisualComparer visualComparer;
        private readonly double differenceThreshold;
        private readonly IVisualDiffGenerator visualDiffGenerator;

        public VisualRegressionTester(string testProjectRootDirectory, IVisualComparer visualComparer, IVisualDiffGenerator visualDiffGenerator, double differenceThreshold = 99.0, string? ciArtifactsDirectory = null)
        {
            this.snapshotsBaselineDirectory = Path.Combine(testProjectRootDirectory, "snapshots-baseline");
            this.snapshotsDiffDirectory = Path.Combine(ciArtifactsDirectory ?? testProjectRootDirectory, "snapshots-diff");

            this.visualComparer = visualComparer;
            this.visualDiffGenerator = visualDiffGenerator;
            this.differenceThreshold = differenceThreshold;
        }

        public virtual void Test(string name, ImageSnapshot actualImage)
        {
            string imageFileName = $"{name}.{actualImage.Format.GetFileExtension()}";

            string baselineImageFile = Path.Combine(this.snapshotsBaselineDirectory, imageFileName);

            if (!File.Exists(baselineImageFile))
            {
                Directory.CreateDirectory(this.snapshotsDiffDirectory);

                actualImage.Save(this.snapshotsDiffDirectory, imageFileName);

                this.Fail(
                    $"Baseline snapshot not yet created: {Path.Combine(this.snapshotsBaselineDirectory, imageFileName)}\n" +
                    $"Ensure new snapshot is correct:    {Path.Combine(this.snapshotsDiffDirectory, imageFileName)}\n" +
                    $"and if so, copy it to the snapshots-baseline directory");

                return;
            }

            ImageSnapshot baselineImage = new ImageSnapshot(baselineImageFile);

            double difference = this.visualComparer.Compare(baselineImage, actualImage);
            if (difference < this.differenceThreshold)
            {
                this.Fail($"Snapshot different than baseline: {imageFileName} ({difference}% difference)");
            }
        }

        public void Fail(string message)
        {
            throw new VisualTestFailedException(message);
        }
    }
}
