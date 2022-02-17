using System.Collections.Generic;
using Membership.Core.Member.DataModels;

namespace Membership.Core.Member.Repositories
{
    public interface IMemberTypeRepository
    {
        MemberType Get(int typeId);

        IEnumerable<MemberType> GetAllTypes();

        IEnumerable<MemberRemoval> GetRemovalCodes();
    }
}
