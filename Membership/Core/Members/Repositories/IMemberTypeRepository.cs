using System.Collections.Generic;
using Membership.Core.Members.DataModels;

namespace Membership.Core.Members.Repositories
{
    public interface IMemberTypeRepository
    {
        MemberType Get(int typeId);

        IEnumerable<MemberType> GetAllTypes();

        IEnumerable<MemberRemoval> GetRemovalCodes();
    }
}
