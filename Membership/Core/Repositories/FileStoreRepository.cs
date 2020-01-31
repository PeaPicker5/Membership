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
                var retVal = connection.Query<ImageRec>(query, new { ImageId = imageId }).ToList();
                return retVal[0];
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
            using (IDbConnection connection = new SqlConnection(Helper.ConnVal(DbConnectionName)))
            {
                return connection.Update(new ImageRec
                    {
                        ImageId=imageId, 
                        ImageData = imageData
                    });
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