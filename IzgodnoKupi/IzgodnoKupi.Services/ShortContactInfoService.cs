using Bytes2you.Validation;
using IzgodnoKupi.Data.Contracts;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;

namespace IzgodnoKupi.Services
{
    public class ShortContactInfoService : IShortContactInfoService
    {
        private readonly ISaveContext context;
        private readonly IEfRepository<ShortContactInfo> shortContactInfo;

        public ShortContactInfoService(IEfRepository<ShortContactInfo> shortContactInfo, ISaveContext context)
        {
            Guard.WhenArgument(shortContactInfo, "shortContactInfo").IsNull().Throw();
            Guard.WhenArgument(context, "context").IsNull().Throw();

            this.shortContactInfo = shortContactInfo;
            this.context = context;
        }
        public void Add(ShortContactInfo shortContactInfo)
        {
            this.shortContactInfo.Add(shortContactInfo);
            this.context.Commit();
        }
    }
}
