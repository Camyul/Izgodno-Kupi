using IzgodnoKupi.Data.Model;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace IzgodnoKupi.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser
    {
        private ICollection<Order> orders;
        private ICollection<FullContactInfo> fullContactInfos;

        public User()
        {
            this.Orders = new HashSet<Order>();
            this.FullContactInfos = new HashSet<FullContactInfo>();
        }

        public virtual ICollection<Order> Orders
        {
            get
            {
                return this.orders;
            }
            set
            {
                this.orders = value;
            }
        }

        public virtual ICollection<FullContactInfo> FullContactInfos
        {
            get
            {
                return this.fullContactInfos;
            }
            set
            {
                this.fullContactInfos = value;
            }
        }
    }
}
