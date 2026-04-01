using System.Threading;
using System.Threading.Tasks;
using Common.Models.FileDTO;
using Common.Models.ResponseModels;

namespace ServiceLayer.Services.MediaManagmentService;

public interface IMediaManagementService
{
    public Task<FileMetaInfoResponseModel> UploadFileAsync(FileDTO file, CancellationToken cancellationToken);
}