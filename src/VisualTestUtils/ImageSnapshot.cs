namespace VisualTestUtils;

public class ImageSnapshot
{
    public ImageSnapshot(byte[] data, ImageSnapshotFormat format)
    {
        this.Data = data;
        this.Format = format;
    }

    public ImageSnapshot(string path)
    {
        this.Data = File.ReadAllBytes(path);
        this.Format = ImageSnapshotFormatExtensions.GetImageFormat(path);
    }

    /// <summary>
    /// Gets image data as bytes, in the associated image format.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Gets image format.
    /// </summary>
    public ImageSnapshotFormat Format { get; }

    /// <summary>
    /// Saves the image to the specified directory with the specified file name base.
    /// </summary>
    /// <param name="directory">Directory to save the image snapshot.</param>
    /// <param name="fileNameBase">File name base for the saved image snapshot.</param>
    public void Save(string directory, string fileNameBase)
    {
        string extension = this.Format.GetFileExtension();
        var filePath = Path.Combine(directory, fileNameBase + extension);
        File.WriteAllBytes(filePath, this.Data);
    }
}
