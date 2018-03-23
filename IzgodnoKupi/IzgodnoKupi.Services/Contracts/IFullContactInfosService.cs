using IzgodnoKupi.Data.Model;
using System;
using System.Linq;

namespace IzgodnoKupi.Services.Contracts
{
    public interface IFullContactInfosService
    {
        void Add(FullContactInfo fullContactInfo);

        IQueryable<FullContactInfo> GetAllByUser(string id);

        FullContactInfo GetById(Guid? id);

        FullContactInfo GetDefaultByUser(string id);

        void Update(FullContactInfo contactInfo);
    }
}
