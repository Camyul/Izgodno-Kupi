using Bytes2you.Validation;
using IzgodnoKupi.Data.Contracts;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;

namespace IzgodnoKupi.Services
{
    public class FullContactInfosService : IFullContactInfosService
    {
        private readonly ISaveContext context;
        private readonly IEfRepository<FullContactInfo> fullContactInfosRepo;

        public FullContactInfosService(IEfRepository<FullContactInfo> fullContactInfosRepo, ISaveContext context)
        {
            Guard.WhenArgument(fullContactInfosRepo, "fullContactInfosRepo").IsNull().Throw();
            Guard.WhenArgument(context, "context").IsNull().Throw();

            this.fullContactInfosRepo = fullContactInfosRepo;
            this.context = context;
        }
        public void Add(FullContactInfo fullContactInfo)
        {
            this.fullContactInfosRepo.Add(fullContactInfo);
            this.context.Commit();
        }
    }
}
