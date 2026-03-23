using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class EcrInterface
    {
        public Dictionary<uint, ST_DEPARTMENT[]> TransactionDepartmentList = new Dictionary<uint, ST_DEPARTMENT[]>();

        public Dictionary<uint, ST_TAX_RATE[]> TransactionTaxRateList = new Dictionary<uint, ST_TAX_RATE[]>();

        public string FriendlyName { get; set; }

        public uint Index { get; set; }
        public byte[] Id { get; set; }

        public List<TransactionHandle> TransactionHandleList { get; set; }
        public TransactionHandle ActiveTransactionHandle => TransactionHandleList.Find((TransactionHandle item) => item.Active);

        public void AddNewOrActivateTransactionHandle(TransactionHandle item)
        {
            foreach (TransactionHandle transactionHandle in TransactionHandleList)
            {
                transactionHandle.Active = false;
                if (item == transactionHandle)
                {
                    transactionHandle.Active = true;
                    return;
                }
            }
            item.Active = true;
            TransactionHandleList.Add(item);
        }
    }

}
