using System;
using Dapper.Contrib.Extensions;

namespace Membership.Core.DataModels
{
    [Table("SYS_FileStore")]
    public class ImageRec
    {
        public ImageRec() { }
        public ImageRec(Guid imageId, string description, byte[] imageData)
        {
            ImageId = imageId;
            Description = description;
            ImageData = imageData;
        }

        [ExplicitKey] public Guid ImageId { get; set; }
        public string Description { get; set; }
        public byte[] ImageData { get; set; }

    }
}
