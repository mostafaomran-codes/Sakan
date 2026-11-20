using System.Text.RegularExpressions;
using Sakan.Shared.Contracts;

namespace Sakan.Infrastructure;

/// <summary>
/// Stores files OUTSIDE wwwroot (default App_Data/uploads). Files are never served directly:
/// expose them through an authorized controller action that calls OpenReadAsync.
/// </summary>
public class LocalFileStorage : IFileStorage
{
    private readonly string _root;

    public LocalFileStorage(IConfiguration config, IWebHostEnvironment env)
    {
        var configured = config["Storage:RootPath"] ?? "App_Data/uploads";
        var path = Path.IsPathRooted(configured) ? configured : Path.Combine(env.ContentRootPath, configured);
        _root = Path.GetFullPath(path);
        Directory.CreateDirectory(_root);
    }

    public async Task<string> SaveAsync(Stream content, string fileName, string folder, CancellationToken ct = default)
    {
        var key = $"{SanitizeFolder(folder)}/{Guid.NewGuid():N}{SafeExtension(fileName)}";
        var fullPath = ResolvePath(key);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var target = File.Create(fullPath);
        await content.CopyToAsync(target, ct);
        return key;
    }

    public Task<Stream?> OpenReadAsync(string storageKey, CancellationToken ct = default)
    {
        var path = ResolvePath(storageKey);
        Stream? stream = File.Exists(path)
            ? new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
            : null;
        return Task.FromResult(stream);
    }

    public Task DeleteAsync(string storageKey, CancellationToken ct = default)
    {
        var path = ResolvePath(storageKey);
        if (File.Exists(path)) File.Delete(path);
        return Task.CompletedTask;
    }

    private string ResolvePath(string key)
    {
        var full = Path.GetFullPath(Path.Combine(_root, key));
        var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        if (!full.StartsWith(_root + Path.DirectorySeparatorChar, comparison))
            throw new InvalidOperationException("Invalid storage key.");
        return full;
    }

    private static string SanitizeFolder(string folder)
    {
        var cleaned = new string(folder.Where(c => char.IsLetterOrDigit(c) || c is '-' or '_').ToArray());
        return cleaned.Length == 0 ? "misc" : cleaned;
    }

    private static string SafeExtension(string fileName)
    {
        var ext = Path.GetExtension(fileName);
        return Regex.IsMatch(ext, @"^\.[A-Za-z0-9]{1,8}$") ? ext.ToLowerInvariant() : string.Empty;
    }
}
