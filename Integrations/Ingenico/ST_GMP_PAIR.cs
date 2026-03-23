using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ST_GMP_PAIR
    {
        public string szProcOrderNumber;

        public string szProcDate;

        public string szProcTime;

        public string szExternalDeviceBrand;

        public string szExternalDeviceModel;

        public string szExternalDeviceSerialNumber;

        public string szEcrSerialNumber;

        public ST_GMP_PAIR()
        {
            szProcOrderNumber = "";
            szProcDate = "";
            szProcTime = "";
            szExternalDeviceBrand = "";
            szExternalDeviceModel = "";
            szExternalDeviceSerialNumber = "";
            szEcrSerialNumber = "";
        }
    }

}
