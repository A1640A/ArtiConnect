using ArtiConnect.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtiConnect.Integrations.Ingenico
{
    public class ErrorManager
    {
        private static ErrorManager _instance;

        public static ErrorManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ErrorManager(selfControlledInstantiation: true);
                }
                return _instance;
            }
        }

        public ErrorManager()
        {
            throw new Exception(Resources.SingletonErrorText);
        }
        private ErrorManager(bool selfControlledInstantiation)
        {
        }

        public string HandleError(uint errorCode, LogType type, bool showOnMessageBox = false, string customLogText = "")
        {
            string text = "";
            string text2 = "";
            if (type != LogType.ClientExceptionsOperation)
            {
                byte[] array = new byte[256];
                GMPSmartDLL.GetErrorMessage(errorCode, array);
                text = GMP_Tools.SetEncoding(array);
                text = text.Replace("\0", string.Empty);
                WriteFile(text, errorCode, type, customLogText);
                if (text != "TRAN_RESULT_OK [0]")
                {
                    byte[] array2 = new byte[256];
                    GMPSmartDLL.GetErrorTurkishDescription(errorCode, array2);
                    text = GMP_Tools.SetEncoding(array2);
                    text = text.Replace("\0", string.Empty);
                    text2 = text;
                }
            }
            else
            {
                try
                {
                    string name = errorCode.ToString();
                    text = Resources.ResourceManager.GetString(name, Resources.Culture);
                }
                catch
                {
                }
                if (string.IsNullOrWhiteSpace(text))
                {
                    text = Resources._10000;
                }
                WriteFile(text, errorCode, type, customLogText);
                text2 = text;
            }
            switch (errorCode)
            {
                case 61468u:
                    text = "ÖKC Meşgul / Satış Ekranında Değil";
                    break;
                case 61467u:
                    text = "ÖKC Bilgi Alınamadı";
                    break;
                case 61472u:
                    text = "Eşleşme Gerekiyor";
                    break;
                case 61443u:
                    text = "Haberleşme Zaman Aşımı";
                    break;
                case 2358u:
                    text = "Fiş Bulunamadı. İşlemi Tekrarlayın.";
                    break;
                case 2341u:
                    text = "Başarısız. İşlemi Tekrarlayın.";
                    break;
                case 2317u:
                    text = "Hatalı. İşlemi Tekrarlayın.";
                    break;
                case 32u:
                    text = "ÖKC de KAĞIT BİTTİ";
                    break;
                case 0u:
                    text = "HAZIR";
                    break;
            }
            if (showOnMessageBox)
            {
                MessageBox.Show(text2, Resources.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return text2;
        }

        public void WriteFile(string errorText, uint errorCode, LogType type, string customLogText)
        {
            StreamWriter streamWriter = new StreamWriter("dlog.txt", append: true);
            string text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            if (!string.IsNullOrWhiteSpace(customLogText))
            {
                errorText = errorText + " ; " + customLogText;
            }
            errorText = errorText.Replace("\r\n", " ");
            errorText = errorText.Replace("\n", " ");
            streamWriter.WriteLine(text + " ; " + type.ToString() + " ; " + errorCode + " ; " + errorText);
            streamWriter.Close();
        }
    }

}
