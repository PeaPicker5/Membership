using System;

namespace Membership.Core.DataModels;

public class ImageRec
{
    public ImageRec() { }
    public ImageRec(Guid imageId, string description, byte[] imageData)
    {
        ImageId = imageId;
        Description = description;
        ImageData = imageData;
    }

    public Guid ImageId { get; set; }
    public string Description { get; set; }
    public byte[] ImageData { get; set; }

}