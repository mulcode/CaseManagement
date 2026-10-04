using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Interfaces
{
    public interface IDefendant
    {
        IEnumerable<Defendant> GetAllDefendant();
        Defendant GetDefendant(int id);

        IEnumerable<Defendant> GetDefendants(int pageNumber, int pageSize);
        int GetTotalDefendantCount();


        void AddDefendant(Defendant defendant);
        void UpdateDefendant(Defendant defendant);
        void DeleteDefendant(int id);
    }
}
