using System.Collections.Generic;
using Membership.Core.DataModels;

namespace Membership.Core.Repositories
{
    public interface IMemberTypeRepository
    {
        MemberType Get(int typeId);

        IEnumerable<MemberType> GetAllTypes();

        IEnumerable<MemberRemoval> GetRemovalCodes();
    }
}
