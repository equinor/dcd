using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using api.Dtos;
using api.Models;

using AutoMapper;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;
    private readonly string _containerName;

    public BlobStorageService(BlobServiceClient blobServiceClient, IImageRepository imageRepository, IConfiguration configuration, IMapper mapper)
    {
        _blobServiceClient = blobServiceClient;
        _imageRepository = imageRepository;
        _mapper = mapper;
        _containerName = configuration["azureStorageAccountImageContainerName"]
                         ?? throw new InvalidOperationException("Container name configuration is missing.");

        if (string.IsNullOrEmpty(_containerName))
        {
            throw new InvalidOperationException("Container name configuration is missing or empty.");
        }
    }

    public async Task<ImageDto> SaveImage(IFormFile image, Guid caseId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient($"{caseId}/{image.FileName}");

        await using var stream = image.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });

        var imageUrl = blobClient.Uri.ToString();
        var createTime = DateTimeOffset.UtcNow;

        var imageEntity = new Image
        {
            Url = imageUrl,
            CreateTime = createTime,
            CaseId = caseId,
        };

        await _imageRepository.AddImage(imageEntity);

        var imageDto = _mapper.Map<ImageDto>(imageEntity);

        if (imageDto == null)
        {
            throw new InvalidOperationException("Image mapping failed.");
        }

        return imageDto;
    }

    public async Task<List<ImageDto>> GetCaseImages(Guid caseId)
    {
        var images = await _imageRepository.GetImagesByCaseId(caseId);

        var imageDtos = _mapper.Map<List<ImageDto>>(images);

        if (imageDtos == null)
        {
            throw new InvalidOperationException("Image mapping failed.");
        }
        return imageDtos;
    }
}
