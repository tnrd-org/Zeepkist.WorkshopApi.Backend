﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using TNRD.Zeepkist.WorkshopApi.Backend.Db;
using TNRD.Zeepkist.WorkshopApi.Backend.Db.Models;
using TNRD.Zeepkist.WorkshopApi.Backend.ResponseModels;

namespace TNRD.Zeepkist.WorkshopApi.Backend.Endpoints.Levels.Post;

public class Endpoint : Endpoint<RequestModel, LevelResponseModel>
{
    private readonly ZworpshopContext context;

    public Endpoint(ZworpshopContext context)
    {
        this.context = context;
    }

    public override void Configure()
    {
        Post("levels");
        Description(b => b.ExcludeFromDescription());
    }

    public override async Task HandleAsync(RequestModel req, CancellationToken ct)
    {
        LevelModel model = new()
        {
            WorkshopId = ulong.Parse(req.WorkshopId),
            AuthorId = ulong.Parse(req.AuthorId),
            Name = req.Name,
            CreatedAt = req.CreatedAt,
            UpdatedAt = req.UpdatedAt,
            ImageUrl = req.ImageUrl,
            FileUrl = req.FileUrl,
            FileUid = req.FileUid,
            FileHash = req.FileHash,
            FileAuthor = req.FileAuthor,
            Valid = req.Valid,
            Validation = req.Validation,
            Gold = req.Gold,
            Silver = req.Silver,
            Bronze = req.Bronze
        };

        EntityEntry<LevelModel> entry = await context.Levels.AddAsync(model, ct);
        await context.SaveChangesAsync(ct);
        await SendOkAsync(entry.Entity.ToResponseModel(), ct);
    }
}
