using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Tools.Model;
public class FileModel
{
    [Key]
    public Guid ID { get; set; }

    public string Name { get; set; }
    public long Length { get; set; }
    public string Path { get; set; }
    public string Description { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime CreationTimeUtc { get; set; }
    public DateTime LastAccessTime { get; set; }
    public DateTime LastAccessTimeUtc { get; set; }
    public DateTime LastWriteTime { get; set; }
    public DateTime LastWriteTimeUtc { get; set; }
    public FileAttributes Attributes { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}