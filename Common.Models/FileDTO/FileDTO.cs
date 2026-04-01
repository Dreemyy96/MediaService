using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.Models.FileDTO;

/// <summary>
/// DTO для передачи файлов внутри приложения
/// </summary>
public sealed record FileDTO : IDisposable, IAsyncDisposable
{
    private bool _disposed;

    /// <summary>
    /// Имя файла
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// MimeType файла
    /// </summary>
    public string MimeType { get; }

    /// <summary>
    /// Стрим с контентом файла
    /// </summary>
    public Stream ContentStream { get; }

    public FileDTO(string name, string mimeType, Stream contentStream)
    {
        FileName = name;
        ContentStream = contentStream;
        MimeType = mimeType;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (ContentStream is not null)
        {
            await ContentStream.DisposeAsync().ConfigureAwait(false);
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            ContentStream?.Dispose();
        }

        _disposed = true;
    }
}