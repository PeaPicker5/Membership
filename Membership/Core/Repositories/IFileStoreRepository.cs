using System;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{
    public interface IFileStoreRepository
    {
        ImageRec Get(Guid imageId);

        bool InsertImageRecord(ImageRec imageRec);
        bool UpdateImageRecord(Guid imageId, byte[]imageData);
        bool DeleteImageRecord(Guid imageId);
    }
}
