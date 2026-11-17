using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{

    public class FileStoreRepository : IFileStoreRepository
    {
        private const string DbConnectionName = "MembershipDB";
        public FileStoreRepository() { }
        public ImageRec Get(Guid imageId)
        {
            const string query = "SELECT * FROM SYS_FileStore WHERE ImageId = @imageId";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                // Returns null when the image is not on file.
                return connection.QueryFirstOrDefault<ImageRec>(query, new { ImageId = imageId });
            }
        }

        public bool InsertImageRecord(ImageRec imageRecord)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                var isSuccess = connection.Insert(imageRecord);
                return isSuccess > 0;
            }
        }
        public bool UpdateImageRecord(Guid imageId, byte[]imageData)
        {
            // Only replace the image bytes; a full-entity Update would also blank the Description.
            const string query = "UPDATE SYS_FileStore SET ImageData = @ImageData WHERE ImageId = @ImageId";
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Execute(query, new { ImageId = imageId, ImageData = imageData }) > 0;
            }
        }
        public bool DeleteImageRecord(Guid imageId)
        {
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Delete(new ImageRec { ImageId = imageId });
            }

        }

    }
}