﻿using System.Text;
using FluentResults;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Upload;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace Zeepkist.WorkshopApi.Google;

internal class CloudStorageUploadService : IUploadService
{
    private readonly GoogleOptions googleOptions;
    private StorageClient? cachedStorageClient;

    public CloudStorageUploadService(IOptions<GoogleOptions> googleOptions)
    {
        this.googleOptions = googleOptions.Value;
    }

    private StorageClient GetOrCreateStorageClient()
    {
        if (cachedStorageClient != null)
            return cachedStorageClient;

        byte[] bytes = Convert.FromBase64String(googleOptions.Credentials);
        string json = Encoding.UTF8.GetString(bytes);

        GoogleCredential credential = GoogleCredential.FromJson(json);
        if (credential.IsCreateScopedRequired)
            credential = credential.CreateScoped("https://www.googleapis.com/auth/devstorage.read_write");

        cachedStorageClient = StorageClient.Create(credential);
        return cachedStorageClient;
    }

    public async Task<Result<string>> UploadFile(string identifier, byte[] buffer, CancellationToken ct = default)
    {
        Result<Object> result;

        try
        {
            result = await Upload(buffer, "files", identifier + ".zip", "application/zip", ct);
        }
        catch (Exception e)
        {
            return Result.Fail(new ExceptionalError(e));
        }

        if (result.IsFailed)
            return result.ToResult();

        return result.Value.MediaLink;
    }

    private async Task<Result<Object>> Upload(
        byte[] bytes,
        string folder,
        string name,
        string contentType,
        CancellationToken ct = default
    )
    {
        StorageClient client = GetOrCreateStorageClient();

        bool hasCompleted = false;
        bool hasFailed = false;
        Exception? exception = null;
        Object? uploadedObject;

        using (MemoryStream stream = new(bytes))
        {
            Progress<IUploadProgress> progress = new();
            progress.ProgressChanged += (sender, p) =>
            {
                if (p.Status == UploadStatus.Completed)
                {
                    hasCompleted = true;
                }
                else if (p.Status == UploadStatus.Failed)
                {
                    hasCompleted = true;
                    hasFailed = true;
                    exception = p.Exception;
                }
            };

            uploadedObject = await client.UploadObjectAsync(googleOptions.Bucket,
                $"{folder}/{name}",
                contentType,
                stream,
                progress: progress,
                cancellationToken: ct);
        }

        while (!hasCompleted)
        {
            await Task.Yield();
        }

        return hasFailed
            ? Result.Fail(new ExceptionalError(exception ?? new Exception("Unable to upload data")))
            : uploadedObject;
    }
}
