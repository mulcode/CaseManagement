using CourtDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourtDataAccess.Interfaces
{

        public interface IReturnOfService
        {
            void AddReturnOfService(ReturnOfService returnInfo);

            IList<ReturnOfService> GetReturnOfServicesBySummonId(int summonId);
        }
    
}
