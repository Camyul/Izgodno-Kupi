using Bytes2you.Validation;
using IzgodnoKupi.Data.Contracts;

namespace IzgodnoKupi.Data.SaveContext
{
    public class SaveContext : ISaveContext
    {
        private readonly ApplicationDbContext context;

        public SaveContext(ApplicationDbContext context)
        {
            Guard.WhenArgument(context, "Db context is null!").IsNull().Throw();

            this.context = context;
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }
    }
}
