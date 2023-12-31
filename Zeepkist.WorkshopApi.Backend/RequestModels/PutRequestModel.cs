﻿namespace TNRD.Zeepkist.WorkshopApi.Backend.RequestModels;

public class PutRequestModel<TModel>
{
    [FromQueryParams] public int Id { get; set; }
    [FromBody] public TModel Model { get; set; } = default!;
}
