using System;
using System.Windows.Forms;
using Dapper.Contrib.Extensions;
using Membership.Core.Members.DataModels;

namespace Membership.Core.DataModels
{
    public class SelectableItem : IEquatable<SelectableItem>
    {

        public SelectableItem() { }

        public SelectableItem(Guid itemId, string name, bool isSelected)
        {
            ItemId = itemId;
            Name = name;
            IsSelected = isSelected;
        }

        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        [Computed] public SelectableMember.enumCheckStatus CheckStatus { get; set; }


        #region IEquatable Implementation

        public bool Equals(SelectableItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ItemId.Equals(other.ItemId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SelectableItem) obj);
        }

        public override int GetHashCode()
        {
            return ItemId.GetHashCode();
        }

        #endregion
    }
}
