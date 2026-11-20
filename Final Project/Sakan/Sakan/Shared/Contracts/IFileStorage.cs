namespace Sakan.Shared.Contracts;

public interface IFileStorage
{
    /// <param name="folder">One segment only, e.g. "identity", "evidence", "listings".</param>
    /// <returns>A storage key to save in the database (never a public URL).</returns>
    Task<string> SaveAsync(Stream content, string fileName, string folder, CancellationToken ct = default);

    /// <returns>null if the key does not exist.</returns>
    Task<Stream?> OpenReadAsync(string storageKey, CancellationToken ct = default);

    Task DeleteAsync(string storageKey, CancellationToken ct = default);
}
