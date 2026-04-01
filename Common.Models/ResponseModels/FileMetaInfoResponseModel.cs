using System;

namespace Common.Models.ResponseModels;

public class FileMetaInfoResponseModel
{
    /// <summary>
    /// Id записи PK
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя оригинального файла
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// MimeType файла
    /// </summary>
    public string MimeType { get; set; }

    /// <summary>
    /// Id создателя записи
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Дата создания записи
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <summary>
    /// Индикатор стадии прогрузки файла в хранилилище
    /// </summary>
    public string State { get; set; }
}