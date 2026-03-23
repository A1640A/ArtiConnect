using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class _ST_PAYMENT_REQUEST_ORGINAL_DATA
    {
        public uint TransactionAmount;

        public uint LoyaltyAmount;

        public ushort NumberOfinstallments;

        public byte[] AuthorizationCode;

        public byte[] rrn;

        public byte[] TransactionDate;

        public byte[] MerchantId;

        public byte TransactionType;

        public byte[] referenceCodeOfTransaction;
    }
}
