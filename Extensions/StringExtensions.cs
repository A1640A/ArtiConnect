using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArtiConnect.Extensions
{
    public static class StringExtensions
    {
        public static string NormalizeSearch(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return text.ToLower()
                .Replace("ı", "i")
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("ö", "o")
                .Replace("ç", "c");
        }

        public static string GetNewKod(List<string> kodList, string defaultKod)
        {
            if (!kodList.Any())
                return defaultKod; // Liste boşsa varsayılan değer

            var ornek = kodList.First();

            // Sadece sayısal değer kontrolü
            if (kodList.All(k => k.All(char.IsDigit)))
            {
                var maxSayi = kodList.Select(k => long.Parse(k)).Max();
                return (maxSayi + 1).ToString().PadLeft(ornek.Length, '0');
            }

            // Noktalı format kontrolü (56.65.73.1 gibi)
            if (ornek.Contains("."))
            {
                var sonParca = kodList.Select(k =>
                {
                    var parcalar = k.Split('.');
                    return long.Parse(parcalar[parcalar.Length - 1]);
                }).Max();

                var onParcalar = ornek.Substring(0, ornek.LastIndexOf('.') + 1);
                return $"{onParcalar}{sonParca + 1}";
            }

            // Harfli ve sayılı formatlar için gelişmiş kontrol
            var harfliFormat = new System.Text.RegularExpressions.Regex(@"^([A-Za-z]+)(\d+)$");
            if (harfliFormat.IsMatch(ornek))
            {
                // Tüm kodları kontrol et
                var maxSayi = kodList
                    .Select(k =>
                    {
                        var match = harfliFormat.Match(k);
                        var harfKisim = match.Groups[1].Value;
                        var sayiKisim = match.Groups[2].Value;

                        // Eğer harf kısmı aynıysa, sayı kısmını al
                        if (harfKisim == harfliFormat.Match(ornek).Groups[1].Value)
                        {
                            return long.Parse(sayiKisim);
                        }
                        return 0;
                    })
                    .Max();

                var harfKismi = harfliFormat.Match(ornek).Groups[1].Value;
                var sayiUzunluk = harfliFormat.Match(ornek).Groups[2].Value.Length;

                return $"{harfKismi}{(maxSayi + 1).ToString().PadLeft(sayiUzunluk, '0')}";
            }

            // Sayı ile başlayıp harf ile biten format için
            var sonHarfFormat = new System.Text.RegularExpressions.Regex(@"^(\d+)([A-Za-z]+)$");
            if (sonHarfFormat.IsMatch(ornek))
            {
                var matches = kodList
                    .Select(k => sonHarfFormat.Match(k))
                    .Where(m => m.Success)
                    .ToList();

                var sayiKismi = sonHarfFormat.Match(ornek).Groups[1].Value;
                var sonHarfler = matches
                    .Select(m => m.Groups[2].Value)
                    .OrderBy(h => h)
                    .Last();

                // Bir sonraki harfi hesapla
                var yeniHarfler = GetNextString(sonHarfler);
                return $"{sayiKismi}{yeniHarfler}";
            }

            try
            {
                string prefix = defaultKod.Substring(0, 3);

                // Listedeki prefix'e uyan kodları filtrele
                var matchingKods = kodList
                    .Where(k => k.Length >= 3 && k.Substring(0, 3) == prefix)
                    .ToList();

                // Eşleşen kod yoksa varsayılan değeri döndür
                if (!matchingKods.Any())
                    return defaultKod;

                // Sayısal kısmı ayıkla ve en büyük değeri bul
                var maxNumber = matchingKods
                    .Select(k =>
                    {
                        // Prefix'ten sonraki kısmı al
                        string numericPart = k.Substring(3);
                        // Sayısal değere çevrilebiliyorsa çevir, çevrilemiyorsa 0 al
                        return long.TryParse(numericPart, out long number) ? number : 0;
                    })
                    .Max();

                // Yeni sayıyı oluştur
                long newNumber = maxNumber + 1;

                // Sayısal kısmın uzunluğunu bul (defaultKod'dan prefix'i çıkararak)
                int numericLength = defaultKod.Length - 3;

                // Yeni kodu oluştur (prefix + padded number)
                return $"{prefix}{newNumber.ToString().PadLeft(numericLength, '0')}";
            }
            catch (Exception)
            {
                // Diğer durumlar için varsayılan davranış
                return defaultKod;
            }
        }

        public static string GetNextString(string str)
        {
            var charArray = str.ToCharArray();
            int i = charArray.Length - 1;

            while (i >= 0)
            {
                if (charArray[i] == 'Z')
                {
                    charArray[i] = 'A';
                    i--;
                }
                else
                {
                    charArray[i] = (char)(charArray[i] + 1);
                    break;
                }
            }

            // Eğer tüm harfler Z ise, yeni bir A ekle
            if (i < 0)
            {
                return new string('A', charArray.Length + 1);
            }

            return new string(charArray);
        }

        public static bool ContainsLetter<T>(this T input) where T : class
        {
            if (input == null) return false;

            string strInput = input.ToString();
            return strInput.Any(c => char.IsLetter(c));
        }

        public static bool ContainsNumber<T>(this T input) where T : class
        {
            if (input == null) return false;

            string strInput = input.ToString();
            return strInput.Any(c => char.IsDigit(c));
        }

        public static string GenerateRandomEAN13Barcode()
        {
            Random random = new Random();

            string barcodeDigits = "";
            for (int i = 0; i < 12; i++)
            {
                barcodeDigits += random.Next(0, 10).ToString();
            }

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(barcodeDigits[i].ToString());
                if (i % 2 == 0)
                {
                    sum += digit;
                }
                else
                {
                    sum += digit * 3;
                }
            }

            int checkDigit = (10 - (sum % 10)) % 10;

            string barcode = barcodeDigits + checkDigit.ToString();

            if (barcode.StartsWith("0"))
                barcode = GenerateRandomEAN13Barcode();

            return barcode;
        }

        public static string StringDegeriArttir(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && text != string.Empty)
            {
                if (text.Contains("."))
                {
                    string[] parts = text.Split('.');
                    text = parts[0];
                }

                if (string.IsNullOrEmpty(text))
                    text = "0";

                if (char.IsLetter(text[0]) && char.IsLetter(text[text.Length - 1]))
                {
                    char lastChar = text[text.Length - 1];
                    char nextChar = (char)(lastChar + 1);
                    return text.Substring(0, text.Length - 1) + nextChar;
                }
                else if (text.Contains("-"))
                {
                    string[] parts = text.Split('-');
                    if (parts.Length == 2 && parts[0].Length == 2 && parts[1].Length == 4
                        && parts[0].All(char.IsLetter) && parts[1].All(char.IsDigit))
                    {
                        string prefix = parts[0];
                        int numberPart = int.Parse(parts[1]) + 1;
                        return $"{prefix}-{numberPart:0000}";
                    }
                    else
                    {
                        return GenerateRandomEAN13Barcode(); // Veya uygun bir hata mesajı
                    }
                }
                else if (char.IsLetter(text[0]) && char.IsDigit(text[text.Length - 1]))
                {
                    string prefix = new string(text.TakeWhile(char.IsLetter).ToArray());
                    int lastNumber = int.Parse(new string(text.SkipWhile(c => char.IsLetter(c)).ToArray()));
                    int nextNumber = lastNumber + 1;
                    return prefix + nextNumber.ToString().PadLeft(text.Length - prefix.Length, '0');
                }
                else if (char.IsDigit(text[0]) && char.IsLetter(text[text.Length - 1]))
                {
                    char lastChar = text[text.Length - 1];
                    char nextChar = (char)(lastChar + 1);
                    return text.Substring(0, text.Length - 1) + nextChar;
                }
                else if (char.IsDigit(text[0]) && char.IsDigit(text[text.Length - 1]))
                {
                    int lastNumber = int.Parse(text);
                    int nextNumber = lastNumber + 1;
                    return nextNumber.ToString();
                }
                else
                {
                    return GenerateRandomEAN13Barcode();
                }
            }
            else
            {
                return "1";
            }
        }

        public static int ToInt(this string value)
        {
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            else if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
                return 0;
        }

        public static string ConvertToCustomFormat(this string input)
        {
            int lastDotIndex = input.LastIndexOf('.');
            int lastCommaIndex = input.LastIndexOf(',');

            int lastIndex = Math.Max(lastDotIndex, lastCommaIndex);

            if (lastIndex >= 0)
            {
                string beforeLastChar = input.Substring(0, lastIndex);
                string afterLastChar = input.Substring(lastIndex + 1);

                return beforeLastChar + "," + afterLastChar;
            }

            input = input.Replace(".", "");

            return input;
        }

        public static float ToBoolean(this string _string)
        {
            return float.TryParse(_string, out var _Float) ? _Float : 0;
        }

        public static bool ToBool(this string _string)
        {
            return bool.TryParse(_string, out bool _Bool) ? _Bool : false;
        }

        public static decimal ToDecimal(this string _string)
        {
            var cultureInfo = new CultureInfo("tr-TR");
            return decimal.TryParse(_string, out var _Decimal) ? _Decimal : 0;
        }

        public static double ToDouble(this string _string)
        {
            var cultureInfo = new CultureInfo("tr-TR");
            return double.TryParse(_string, out var _Double) ? _Double : 0;
        }

        public static float ToFloat(this string _string)
        {
            if (string.IsNullOrEmpty(_string) || string.IsNullOrWhiteSpace(_string)) return 0;

            var cultureInfo = new CultureInfo("tr-TR");
            return float.TryParse(_string, out var _Float) ? _Float : 0;
        }

        public static float ToFloat(this string _string, int decimals = 2)
        {
            if (string.IsNullOrEmpty(_string) || string.IsNullOrWhiteSpace(_string)) return 0;

            var cultureInfo = new CultureInfo("tr-TR");
            NumberFormatInfo numberFormat = cultureInfo.NumberFormat;

            if (float.TryParse(_string,
                               NumberStyles.Number,
                               numberFormat,
                               out var _Float))
            {
                return (float)Math.Round(_Float, decimals, MidpointRounding.AwayFromZero);
            }
            return 0;
        }

        public static string IncreaseStringNumber(string input)
        {
            Match match = Regex.Match(input, @"\d+$");

            if (match.Success)
            {
                var number = long.Parse(match.Value) + 1;

                string result = input.Substring(0, match.Index) + number.ToString().PadLeft(match.Length, '0');

                return result;
            }
            else
            {
                return input;
            }
        }

        public static string CleanEmptyLines(string input)
        {
            using (StringReader reader = new StringReader(input))
            {
                IEnumerable<string> nonEmptyLines = reader
                    .ReadToEnd()
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Trim());

                string cleanedString = string.Join(Environment.NewLine, nonEmptyLines);

                return cleanedString;
            }
        }

        /// <summary>
        /// Stringin içinde en az bir harf olup olmadığını kontrol eder.
        /// </summary>
        public static bool IsContainsLetter(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value.Any(char.IsLetter);
        }

        /// <summary>
        /// Stringin içinde en az bir rakam olup olmadığını kontrol eder.
        /// </summary>
        public static bool IsContainsDigit(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value.Any(char.IsDigit);
        }

        /// <summary>
        /// Stringin tamamen harflerden oluşup oluşmadığını kontrol eder.
        /// </summary>
        public static bool IsFullOfLetter(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value.All(char.IsLetter);
        }

        /// <summary>
        /// Stringin tamamen rakamlardan oluşup oluşmadığını kontrol eder.
        /// </summary>
        public static bool IsFullOfDigit(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return value.All(char.IsDigit);
        }
    }
}
