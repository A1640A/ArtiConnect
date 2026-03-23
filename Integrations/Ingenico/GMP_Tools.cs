using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Integrations.Ingenico
{
    internal class GMP_Tools
    {
        public static string SetEncoding(byte[] arr)
        {
            return Encoding.GetEncoding(65001).GetString(arr);
        }

        public static string SetEncoding(byte[] arr, int index, int len)
        {
            return Encoding.GetEncoding("iso-8859-9").GetString(arr, index, len);
        }

        public static byte[] GetBytesFromString(string str)
        {
            byte[] array = new byte[str.Length + 1];
            int num = 0;
            foreach (char c in str)
            {
                switch (c)
                {
                    case 'Ğ':
                        array[num] = 208;
                        break;
                    case 'Ü':
                        array[num] = 220;
                        break;
                    case 'Ş':
                        array[num] = 222;
                        break;
                    case 'İ':
                        array[num] = 221;
                        break;
                    case 'Ö':
                        array[num] = 214;
                        break;
                    case 'Ç':
                        array[num] = 199;
                        break;
                    case 'I':
                        array[num] = 73;
                        break;
                    case 'ğ':
                        array[num] = 240;
                        break;
                    case 'ü':
                        array[num] = 252;
                        break;
                    case 'ş':
                        array[num] = 254;
                        break;
                    case 'i':
                        array[num] = 105;
                        break;
                    case 'ö':
                        array[num] = 246;
                        break;
                    case 'ç':
                        array[num] = 231;
                        break;
                    case 'ı':
                        array[num] = 253;
                        break;
                    case '€':
                        array[num] = 128;
                        break;
                    default:
                        array[num] = Convert.ToByte(c);
                        break;
                }
                num++;
            }
            array[num] = 0;
            return array;
        }

        public static string GetStringFromBytes(byte[] byt)
        {
            string text = "";
            for (int i = 0; byt[i] != 0 && i != byt.Length; i++)
            {
                if (byt[i] == 208)
                {
                    text += "Ğ";
                    continue;
                }
                if (byt[i] == 220)
                {
                    text += "Ü";
                    continue;
                }
                if (byt[i] == 222)
                {
                    text += "Ş";
                    continue;
                }
                if (byt[i] == 221)
                {
                    text += "İ";
                    continue;
                }
                if (byt[i] == 214)
                {
                    text += "Ö";
                    continue;
                }
                if (byt[i] == 199)
                {
                    text += "Ç";
                    continue;
                }
                if (byt[i] == 73)
                {
                    text += "I";
                    continue;
                }
                if (byt[i] == 240)
                {
                    text += "ğ";
                    continue;
                }
                if (byt[i] == 252)
                {
                    text += "ü";
                    continue;
                }
                if (byt[i] == 254)
                {
                    text += "ş";
                    continue;
                }
                if (byt[i] == 105)
                {
                    text += "i";
                    continue;
                }
                if (byt[i] == 246)
                {
                    text += "ö";
                    continue;
                }
                if (byt[i] == 231)
                {
                    text += "ç";
                    continue;
                }
                if (byt[i] == 253)
                {
                    text += "ı";
                    continue;
                }
                if (byt[i] == 128)
                {
                    text += "€";
                    continue;
                }
                string text2 = text;
                char c = (char)byt[i];
                text = text2 + c;
            }
            return text;
        }

        public static void StringToByteArray(string s, byte[] Out_byteArr, ref int Out_byteArrLen)
        {
            byte[] array = new byte[s.Length / 2];
            for (int i = 0; i < array.Length; i++)
            {
                string value = s.Substring(i * 2, 2);
                array[array.Length - 1 - i] = Convert.ToByte(value, 16);
            }
            Out_byteArrLen = array.Length;
            Array.Copy(array, 0, Out_byteArr, 0, array.Length);
        }

        public static void StringToByteArray_Rev(string s, byte[] Out_byteArr, ref int Out_byteArrLen)
        {
            byte[] array = new byte[s.Length / 2];
            for (int i = 0; i < array.Length; i++)
            {
                string value = s.Substring(i * 2, 2);
                array[i] = Convert.ToByte(value, 16);
            }
            Out_byteArrLen = array.Length;
            Array.Copy(array, 0, Out_byteArr, 0, array.Length);
        }

        public static string ByteArrayToString(byte[] buffer, int bufferLen)
        {
            string text = "";
            for (int i = 0; i < bufferLen; i++)
            {
                text += buffer[i].ToString("X2");
            }
            return text;
        }
    }

}
