using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_EXCHANGE_PROFILE
    {
        public string ProfileName;

        public byte NumberOfCurrency;

        public ST_EXCHANGE[] ExchangeRecords;

        public ST_EXCHANGE_PROFILE()
        {
            ProfileName = "";
            ExchangeRecords = new ST_EXCHANGE[6];
            for (int i = 0; i < 6; i++)
            {
                ExchangeRecords[i] = new ST_EXCHANGE();
            }
        }

        public bool isEmptyFields()
        {
            if (string.IsNullOrWhiteSpace(ProfileName) || NumberOfCurrency == 0)
            {
                return true;
            }
            return false;
        }
    }

}
