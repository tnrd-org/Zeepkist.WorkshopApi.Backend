﻿namespace Zeepkist.WorkshopApi.Endpoints.MedalTimes.Post;

public class RequestModel
{
    public bool IsValid { get; set; }
    public float Validation { get; set; }
    public float Gold { get; set; }
    public float Silver { get; set; }
    public float Bronze { get; set; }
}
