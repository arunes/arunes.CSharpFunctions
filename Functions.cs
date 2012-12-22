using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Management;

/// <summary>
/// Genel string, sayı, tarih vs.. fonksiyonları 
/// Son düzenlenme : 24.06.2008
/// </summary>
namespace arunes
{
    public static class Functions
    {
        #region String Fonksiyonları

        /// <summary>
        /// Verilen tabledan istenen sutunu virgülle ayırarak döndürür
        /// </summary>
        /// <param name="table">Tablo</param>
        /// <param name="columnName">Sutun ismi</param>
        /// <param name="seperator">Ayırıcı</param>
        /// <returns></returns>
        public static string GetSeperatedColumn(DataTable table, string columnName, string seperator)
        {
            return GetSeperatedColumn(table, null, columnName, seperator);
        }

        /// <summary>
        /// Verilen tabledan istenen sutunu virgülle ayırarak döndürür
        /// </summary>
        /// <param name="table">Tablo</param>
        /// <param name="filter">Filtre</param>
        /// <param name="columnName">Sutun ismi</param>
        /// <param name="seperator">Ayırıcı</param>
        /// <returns></returns>
        public static string GetSeperatedColumn(DataTable table, string filter, string columnName, string seperator)
        {
            string[] cols = new string[0];
            foreach (DataRow dbRow in table.Select(filter))
            {
                Array.Resize(ref cols, cols.Length + 1);
                cols[cols.Length - 1] = dbRow[columnName].ToString();
            }
            return string.Join(seperator, cols);
        }

        /// <summary>
        /// Verilen stringin alfa-numeric olup olmadığına bakar
        /// </summary>
        /// <param name="text">Kontrol edilecek string</param>
        /// <returns></returns>
        public static bool IsAlphaNumeric(string text)
        {
            Regex alphaNumericPattern = new Regex("[^a-zA-Z0-9]");
            return !alphaNumericPattern.IsMatch(text);
        }

        /// <summary>
        /// Belirlenen değerlere göre stringin içinden ilk denk gelen bölümü geri döndürür.
        /// </summary>
        /// <param name="start">Başlangıç değeri</param>
        /// <param name="end">Bitiş değeri</param>
        /// <param name="strAll">Tüm string</param>
        /// <returns></returns>
        public static string GetStartToEnd(string start, string end, string strAll)
        {
            int startIndex = strAll.IndexOf(start);
            int endIndex = strAll.IndexOf(end, (startIndex + start.Length));
            if (startIndex > -1 && endIndex > -1)
                return strAll.Substring(startIndex + start.Length, endIndex - (startIndex + start.Length));
            else
                return "";
        }

        /// <summary>
        /// Verilen stringin arasında belli bölümü alır
        /// </summary>
        /// <param name="startStr">Başlangıç</param>
        /// <param name="endStr">Bitiş</param>
        /// <param name="strAll">Tüm string</param>
        /// <returns></returns>
        public static string[] GetAllTextParts(string startStr, string endStr, string strAll)
        {
            Regex r; Match m; string returnValue = "";

            strAll = strAll.Replace("\n", "|n|").Replace("\r", "|r|");
            r = new Regex(startStr + "(.*?)" + endStr, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            for (m = r.Match(strAll); m.Success; m = m.NextMatch())
            {
                returnValue += "|" + m.Groups[1].ToString().Replace(startStr, "").Replace(endStr, "");
            }
            char[] sep = { '|' };
            returnValue = returnValue.Replace("|n|", "\n").Replace("|r|", "\r");
            return returnValue.Split(sep, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Verilen dictionarynin elemanlarını karışık sırayla döndürür
        /// </summary>
        /// <typeparam name="K">Key tipi</typeparam>
        /// <typeparam name="V">Value tipi</typeparam>
        /// <param name="inputDictionary">Karıştırılacak Dictionary</param>
        /// <returns></returns>
        public static Dictionary<K, V> ShuffleDictionary<K, V>(Dictionary<K, V> inputDictionary)
        {
            Dictionary<K, V> randomDictionary = new Dictionary<K, V>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputDictionary.Count > 0)
            {
                randomIndex = r.Next(0, inputDictionary.Count);
                KeyValuePair<K, V> key = inputDictionary.ElementAt(randomIndex);
                randomDictionary.Add(key.Key, key.Value);
                inputDictionary.Remove(key.Key);
            }

            return randomDictionary;
        }

        /// <summary>
        /// Verilen array listin elemanlarını karışık sırayla döndürür
        /// </summary>
        /// <param name="array">Karıştırılacak arraylist</param>
        /// <returns></returns>
        public static object[] ShuffleArray(object[] array)
        {
            Random rnd = new Random();
            for (int ind = array.Length - 1; ind > 0; --ind)
            {
                int position = rnd.Next(ind);
                object temp = array[ind];
                array[ind] = array[position];
                array[position] = temp;
            }

            return array;
        }

        /// <summary>
        /// Array içinde arama yapar, yoksa -1 döner
        /// </summary>
        /// <param name="findValue">Aranacak değer</param>
        /// <param name="array">Array</param>
        /// <param name="type">0: tam uyan, 1:içinde geçen</param>
        /// <returns></returns>
        public static int SearchInArray(string findValue, string[] array, int type)
        {
            int retVal = -1;

            if (type == 0) return Array.IndexOf(array, findValue);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].IndexOf(findValue.ToString()) > -1)
                    return i;
            }

            return retVal;
        }

        /// <summary>
        /// script taglarının arasına yazar
        /// </summary>
        /// <param name="script">Yazılacak javascript</param>
        /// <returns></returns>
        public static string PlaceInsideJSTags(string script)
        {
            return
                "<script type=\"text/javascript\">" + Environment.NewLine +
                    script + Environment.NewLine +
                " </script>";
        }

        /// <summary>
        /// Stringi querystring için encode eder
        /// </summary>
        /// <param name="value">Encode edilecek değer</param>
        /// <returns></returns>
        public static string EncodeString(string value)
        {
            return value
            .Replace("ç", "%C3%A7")
            .Replace("Ç", "%C3%87")
            .Replace("ğ", "%C4%9F")
            .Replace("Ğ", "%C4%9E")
            .Replace("ı", "%C4%B1")
            .Replace("İ", "%C4%B0")
            .Replace("ö", "%C3%B6")
            .Replace("Ö", "%C3%96")
            .Replace("ş", "%C5%9F")
            .Replace("Ş", "%C5%9E")
            .Replace("ü", "%C3%BC")
            .Replace("Ü", "%C3%9C");
        }

        /// <summary>
        /// Encode edilmiş stringi decode eder
        /// </summary>
        /// <param name="value">Decode edilecek değer</param>
        /// <returns></returns>
        public static string DecodeString(string value)
        {
            return value
                .Replace("%C3%A7", "ç")
                .Replace("%C3%87", "Ç")
                .Replace("%C4%9F", "ğ")
                .Replace("%C4%9E", "Ğ")
                .Replace("%C4%B1", "ı")
                .Replace("%C4%B0", "İ")
                .Replace("%C3%B6", "ö")
                .Replace("%C3%96", "Ö")
                .Replace("%C5%9F", "ş")
                .Replace("%C5%9E", "Ş")
                .Replace("%C3%BC", "ü")
                .Replace("%C3%9C", "Ü");
        }

        /// <summary>
        /// TinyMCE'nin bozduğu türkçe karakterleri düzeltir.
        /// </summary>
        /// <param name="input">TinyMCE'den gelen string</param>
        /// <returns></returns>
        public static string FixTinyMCEString(string input)
        {
            return input
                .Replace("&ccedil;", "ç")
                .Replace("&Ccedil;", "Ç")
                .Replace("&ouml;", "ö")
                .Replace("&Ouml;", "Ö")
                .Replace("&uuml;", "ü")
                .Replace("&Uuml;", "Ü");
        }

        /// <summary>
        /// HTML bloğundaki kodun 1 parçasını alır 
        /// </summary>
        /// <param name="strPart">Aranan Parça (Örn: title)</param>
        /// <param name="strAll">Tüm html stringi</param>
        /// <returns></returns>
        public static string GetPartOfHtmlBlock(string strPart, string strAll)
        {
            string startStr = "<" + strPart + ">";
            string endStr = startStr.Replace("<", "</");

            int str = strAll.IndexOf(startStr);
            int end = strAll.IndexOf(endStr);
            if (str < 0 && end < 0)
                return "";
            else
            {
                string partStr;
                partStr = strAll.Substring(str, end - str).Replace(startStr, "");
                return partStr;
            }
        }

        /// <summary>
        /// Verilen yazıyı sql injection için ve genel format olarak düzenler
        /// </summary>
        /// <param name="text">Düzenlenecek yazı</param>
        /// <returns></returns>
        public static string FixLongTextStyle(string text)
        {
            string nText = text
                .Replace("[", "&#091;")
                .Replace("]", "&#093;")
                .Replace("=", "&#061;")
                .Replace("'", "&acute;")
                .Replace("--", "- -")
                .Replace(".", ". ")
                .Replace(",", ", ")
                .Replace(" .", ". ")
                .Replace(" ,", ",")
                .Replace("  ", " ")
                .Replace("select", "sel&#101;ct")
                .Replace("join", "jo&#105;n")
                .Replace("union", "un&#105;on")
                .Replace("where", "wh&#101;re")
                .Replace("insert", "ins&#101;rt")
                .Replace("delete", "del&#101;te")
                .Replace("update", "up&#100;ate")
                .Replace("like", "lik&#101;")
                .Replace("drop", "dro&#112;")
                .Replace("create", "cr&#101;ate")
                .Replace("modify", "mod&#105;fy")
                .Replace("rename", "ren&#097;me")
                .Replace("alter", "alt&#101;r")
                .Replace("cast", "ca&#115;t");

            return nText;
        }

        /// <summary>
        /// Mevcut querystring'e değer ekler
        /// </summary>
        /// <param name="valueName">Değer Adı</param>
        /// <param name="value">Değer</param>
        /// <returns></returns>
        public static string AddValueToQueryString(string valueName, string value)
        {
            return AddValueToQueryString(valueName, value, HttpContext.Current.Request.QueryString.ToString());
        }

        /// <summary>
        /// Mevcut querystring'e değer ekler
        /// </summary>
        /// <param name="valueName">Değer Adı</param>
        /// <param name="value">Değer</param>
        /// <param name="queryString">Düzenleme yapılacak querystring</param>
        /// <returns></returns>
        public static string AddValueToQueryString(string valueName, string value, string queryString)
        {
            string currentQueryString = queryString;
            int isExists = 0; int iFound = 0;
            string retVal = currentQueryString;

            if (currentQueryString != "" && currentQueryString != null)
            {
                isExists = currentQueryString.IndexOf(valueName);
                if (isExists > -1)
                {
                    string[] splitUrl = currentQueryString.Split('&');

                    if (splitUrl.Length > 0)
                    {
                        for (int i = 0; i < splitUrl.Length; i++)
                        {
                            if (splitUrl[i].StartsWith(valueName))
                                iFound = i;
                        }

                        retVal = currentQueryString.Replace(splitUrl[iFound], valueName + "=" + value);
                    }
                    else
                        retVal = valueName + "=" + value;
                }
                else
                    retVal = currentQueryString + "&" + valueName + "=" + value;
            }
            else
                retVal = valueName + "=" + value;

            retVal = retVal.TrimStart(new char[] { '?' });
            return retVal.Length > 0 ? "?" + retVal : "";
        }

        /// <summary>
        /// Mevcut querystringden değer çıkartır
        /// </summary>
        /// <param name="key">Silinecek değer</param>
        /// <returns></returns>
        public static string RemoveValueFromQueryString(string key)
        {
            return RemoveValueFromQueryString(key, HttpContext.Current.Request.QueryString.ToString());
        }

        /// <summary>
        /// Mevcut querystringden değer çıkartır
        /// </summary>
        /// <param name="key">Silinecek değer</param>
        /// <param name="queryString">Düzenleme yapılacak querystring</param>
        /// <returns></returns>
        public static string RemoveValueFromQueryString(string key, string queryString)
        {
            string currentQueryString = queryString;
            string retVal = currentQueryString;

            if (currentQueryString != "" && currentQueryString != null)
            {
                if (currentQueryString.IndexOf(key) > -1)
                {
                    List<string> splitUrl = currentQueryString.Split('&').ToList();

                    int removeIndex = -1;
                    for (int i = 0; i < splitUrl.Count; i++)
                    {
                        if (splitUrl[i].Split('=')[0] == key)
                        {
                            removeIndex = i;
                            break;
                        }
                    }

                    if (removeIndex > -1) splitUrl.RemoveAt(removeIndex);
                    retVal = string.Join("&", splitUrl.ToArray());
                }
            }

            retVal = retVal.TrimStart(new char[] { '?' });
            return retVal.Length > 0 ? "?" + retVal : "";
        }

        /// <summary>
        /// &amp; şeklindeki textleri düzeltir
        /// </summary>
        /// <param name="text">Düzeltilecek yazı</param>
        /// <returns></returns>
        public static string ConvertToHTML(string text)
        {
            if (text == "" || text == null) return "";

            text = text
                .Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&#33;", "!")
                .Replace("&#34;", "\"")
                .Replace("&#35;", "#")
                .Replace("&#36;", "$")
                .Replace("&#37;", "%")
                .Replace("&#39;", "'")
                .Replace("&#42;", "*")
                .Replace("&#63;", "?");

            return text.Trim();
        }

        /// <summary>
        /// Sql injectiona karşı sorguya girmeden önce gelen stringi temizler.
        /// </summary>
        /// <param name="text">Temizlenecek string</param>
        /// <returns></returns>
        public static string ClearBadString(string text)
        {
            return ClearBadString(text, false);
        }

        /// <summary>
        /// Stringi istenmeyen verilere karşı temizler
        /// </summary>
        /// <param name="text">Temizlenecek string</param>
        /// <param name="full">Sql injectiona karşı tam temizlik</param>
        /// <returns></returns>
        public static string ClearBadString(string text, bool full)
        {
            if (text == "" || text == null) return "";

            text = text.Replace("'", "`");

            if (full)
                text = text
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("@", "&#64;")
                    .Replace("&", "&amp;")
                    .Replace("#", "&#35;")
                    .Replace("!", "&#33;")
                    .Replace("$", "&#36;")
                    .Replace("%", "&#37;")
                    .Replace("%", "&#37;")
                    .Replace("\"", "&#39;")
                    .Replace("*", "&#42;")
                    .Replace("?", "&#63;")
                    .Replace("--", "- -");

            return text.Trim();
        }

        /// <summary>
        /// Verilen stringdeki tüm html karakterlerini temizler
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Temizlenecek string</returns>
        public static string ClearHTML(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            return Regex.Replace(text, "<[^>]*>", "");
        }

        /// <summary>
        /// Verilen stringin solundan belirlenen kadar karakteri gösterir
        /// </summary>
        /// <param name="text">Düzenlenecek string</param>
        /// <param name="length">Alınacak uzunluk</param>
        /// <param name="ifBigger">Eğer yazı uzunsa sonuna gelecek yazı (Örn:...)</param>
        /// <returns></returns>
        public static string GetFromLeft(string text, int length, string ifBigger)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (text.Length > length)
                return text.Substring(0, length) + ifBigger;
            else
                return text;
        }

        /// <summary>
        /// Objenin boşmu dolumu olduğunu kontrol eder
        /// </summary>
        /// <param name="input">Obje</param>
        /// <param name="emptyReturn">Eğer boşsa dönecek değer</param>
        /// <returns></returns>
        public static string CheckNullOrEmpty(object input, string emptyReturn)
        {
            if (input != null)
            {
                if (input.ToString().Trim().Length > 0)
                    emptyReturn = input.ToString();
            }
            return emptyReturn;
        }

        /// <summary>
        /// 2 Koşullu if else pratik kullanımı
        /// </summary>
        /// <param name="condition">Karşılaştırma</param>
        /// <param name="ifTrue">Doğruysa dönecek değer</param>
        /// <param name="ifFalse">Yanlışsa dönecek değer</param>
        /// <returns></returns>
        public static string IIF(bool condition, string ifTrue, string ifFalse)
        {
            if (condition)
                return ifTrue;
            else
                return ifFalse;
        }

        /// <summary>
        /// Mail'in syntax ının doğruluğunu kontrol eder
        /// </summary>
        /// <param name="eMail">Kontrol edilecek e-mail adresi</param>
        /// <returns></returns>
        public static bool IsValidEmail(string eMail)
        {
            return Regex.IsMatch(eMail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// Tarihin doğru olup olmadığını kontrol eder
        /// </summary>
        /// <param name="date">Kontrol edilecek tarih</param>
        /// <returns></returns>
        public static bool IsValidDate(object date)
        {
            if (date == null) return false;

            DateTime temp;
            if (DateTime.TryParse(date.ToString(), out temp))
                return true;
            else
                return false;
            //return IsValidDate(date, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Tarihin doğru olup olmadığını kontrol eder
        /// </summary>
        /// <param name="date">Kontrol edilecek tarih</param>
        /// <param name="culture">Kontrol yapılacak culture</param>
        /// <returns></returns>
        public static bool IsValidDate(object date, CultureInfo culture)
        {
            if (date == null) return false;

            DateTime tmpDate;
            if (DateTime.TryParse(date.ToString(), culture, DateTimeStyles.None, out tmpDate))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Tarihin doğru olup olmadığını kontrol eder
        /// </summary>
        /// <param name="date">Kontrol edilecek tarih</param>
        /// <param name="dateFormat">Kontrol edilecek tarih formatı</param>
        /// <returns></returns>
        public static bool IsValidDate(object date, string dateFormat)
        {
            if (date == null) return false;
            try
            {
                DateTime.ParseExact(date.ToString(), dateFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Verilen değeri belirtilen formatdan tarihe çevirir
        /// </summary>
        /// <param name="date">Tarih</param>
        /// <param name="dateFormat">Format</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(object date, string dateFormat)
        {
            try
            {
                return DateTime.ParseExact(date.ToString().Replace(".", "/"), dateFormat,
                    System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Sayının doğru olup olmadığını kontrol eder
        /// </summary>
        /// <param name="number">Kontrol edilecek sayı</param>
        /// <returns></returns>
        public static bool IsValidNumber(object number)
        {
            if (number == null) return false;

            Int64 tmpNumber;
            if (Int64.TryParse(number.ToString(), out tmpNumber))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Guidin doğru olup olmadığını kontrol eder
        /// </summary>
        /// <param name="guid">Kontrol edilecek guid</param>
        /// <returns></returns>
        public static bool IsValidGuid(object guid)
        {
            if (guid == null) return false;
            Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
            return isGuid.IsMatch(guid.ToString());
        }

        /// <summary>
        /// Verilen stringi MD5 ile şifreler
        /// </summary>
        /// <param name="textToMd5">Şifrelenecek string</param>
        /// <returns></returns>
        public static string MD5(string textToMd5)
        {
            string ret = "";
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(textToMd5);
            data = md5.ComputeHash(data);

            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2");

            return ret.ToUpper();
        }

        /// <summary>
        /// Unix timestamp tarihi .net DateTime objesine dönüştürür
        /// </summary>
        /// <param name="timestamp">Unix Timestamp</param>
        /// <returns></returns>
        public static DateTime ConvertFromUnixTimeStamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        /// <summary>
        /// DateTime objesini Unix timestamp değerine dönüştürür
        /// </summary>
        /// <param name="date">Tarih</param>
        /// <returns></returns>
        public static double ConvertToUnixTimeStamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (date - origin).TotalSeconds;
        }

        /// <summary>
        /// Yaş hesaplar
        /// </summary>
        /// <param name="birthDate">Doğum tarihi</param>
        /// <returns></returns>
        public static short CalculateAge(DateTime birthDate)
        {
            string calc = (Convert.ToInt32(DateTime.Today.ToString("yyyyMMdd")) - Convert.ToInt32(birthDate.ToString("yyyyMMdd"))).ToString();
            return Convert.ToInt16(calc.Substring(0, 4));
        }

        /// <summary>
        /// Türkçe karakterleri temizler (ç,ğ,ı,ö vs..)
        /// </summary>
        /// <param name="input">Temizlenecek yazı</param>
        /// <returns></returns>
        public static string ReplaceTurkishChars(string input)
        {
            return input
                .Replace("ç", "c")
                .Replace("Ç", "C")
                .Replace("ğ", "g")
                .Replace("Ğ", "G")
                .Replace("ı", "i")
                .Replace("İ", "I")
                .Replace("ö", "o")
                .Replace("Ö", "O")
                .Replace("ş", "s")
                .Replace("Ş", "S")
                .Replace("ü", "u")
                .Replace("Ü", "U");
        }

        /// <summary>
        /// Verilen stringdeki tüm Türkçe karakterleri ve sembolleri temizler
        /// </summary>
        /// <param name="input">Temizlenecek string</param>
        /// <returns></returns>
        public static string FixStringJustChars(string input)
        {
            return Regex.Replace(ReplaceTurkishChars(input), "[^A-Za-z0-9]", "");
        }

        /// <summary>
        /// Verilen stringi dosya adı formatına uygun yazar
        /// </summary>
        /// <param name="input"></param>
        /// <param name="spaceChar">Boşluk karakteri yerine hangi karakterin kullanılacağını belirtir</param>
        /// <param name="limit">Kaç karakter döneceğini belirtir, 0:Tüm adı döndürür</param>
        /// <returns></returns>
        public static string ConvertToFileName(string input, string spaceChar, int limit)
        {
            string[] fileNameParts = input.Split('.');
            string fileExt = fileNameParts.Length > 1 ? fileNameParts.Last() : string.Empty;
            if (fileExt != string.Empty) Array.Resize(ref fileNameParts, fileNameParts.Length - 1);
            string fileName = string.Join(".", fileNameParts);

            fileName = fileName.Replace(" ", spaceChar);
            fileName = ReplaceTurkishChars(fileName);
            fileName = fileName.ToLower(new CultureInfo(1033));
            fileName = Regex.Replace(fileName, @"[^A-Z^a-z^0-9^.^-]", "");
            fileName += fileExt != string.Empty ? "." + fileExt : "";
            return limit == 0 ? fileName : GetFromLeft(fileName, limit, "");
        }

        /// <summary>
        /// Verilen yazıyı başlığa çevirir (İlk harflerini büyütür)
        /// </summary>
        /// <param name="text">Düzenlenecek yazı</param>
        /// <returns></returns>
        public static string ConvertToTitleCase(string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        /// <summary>
        /// Verilen IP adresini, belirtilen wildcard ile kontrol eder
        /// </summary>
        /// <param name="ipAddress">Kontrol edilecek ip adresi</param>
        /// <param name="wildCard">IP adresi maskesi Örn: (255.255.*.*)</param>
        /// <returns></returns>
        public static bool CheckIPAddress(string ipAddress, string wildCard)
        {
            string[] ipSplit = ipAddress.Split('.');
            string[] wcSplit = wildCard.Split('.');
            bool retVal = true;

            if (ipSplit.Length != wcSplit.Length) return false;

            for (int i = 0; i < ipSplit.Length; i++)
            {
                if (ipSplit[i] != wcSplit[i] && wcSplit[i] != "*")
                {
                    retVal = false; break;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Verilen stringin tersini döndürür
        /// </summary>
        /// <param name="str">Ters çevirilecek string</param>
        /// <returns></returns>
        public static string Reverse(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        #endregion

        #region Sayı Fonksiyonları

        /// <summary>
        /// Verilen sayıların birbirlerine tam bölünüp bölünmediğini kontrol eder
        /// </summary>
        /// <param name="dividend">Bölünen</param>
        /// <param name="divider">Bölen</param>
        /// <returns></returns>
        public static bool DivideInteger(int dividend, int divider)
        {
            double dbDividend = Convert.ToDouble(dividend);
            double dbDivider = Convert.ToDouble(divider);

            double result = (dbDividend / dbDivider);
            if (Math.Floor(result) == result)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verilen sayıyı aşağı yuvarlar
        /// </summary>
        /// <param name="valueToRound">Yuvarlanacak sayı</param>
        /// <returns></returns>
        public static long RoundDown(double valueToRound)
        {
            double floorValue = Math.Floor(valueToRound);
            if ((valueToRound - floorValue) > .5)
                return Convert.ToInt64((floorValue + 1));
            else
                return Convert.ToInt64(floorValue);
        }

        /// <summary>
        /// Verilen sayıyı yukarı yuvarlar
        /// </summary>
        /// <param name="valueToRound">Yuvarlanacak sayı</param>
        /// <returns></returns>
        public static long RoundUp(double valueToRound)
        {
            double floorValue = Math.Floor(valueToRound);
            if (valueToRound != floorValue)
                return Convert.ToInt64(floorValue + 1);
            else
                return Convert.ToInt64(floorValue);
        }

        /// <summary>
        /// Verilen stringin soluna verilen sayıda değer ekler
        /// </summary>
        /// <param name="input">Verilen değer</param>
        /// <param name="willAdded">Eklenecek değer</param>
        /// <param name="length">Uzunluk</param>
        /// <param name="where">1: Soluna, 2: Sağına</param>
        /// <returns></returns>
        public static string AddValueToString(string input, string willAdded, int length, int where)
        {
            while (input.Length < length)
            {
                if (where == 1)
                    input = willAdded + input;
                else
                    input = input + willAdded;
            }
            return input;
        }

        #endregion

        #region Resim Fonksiyonları

        /// <summary>
        /// Resimi boyutlandırır ve kaydeder
        /// </summary>
        /// <param name="imagePath">Mevcut resimin yolu</param>
        /// <param name="newImagePath">Yeni resim yolu</param>
        /// <param name="width">Genişlik</param>
        /// <param name="height">Yükseklik</param>
        /// <returns></returns>
        public static bool ResizeAndSaveImage(string imagePath, string newImagePath, int width, int height)
        {
            System.Drawing.Image oldImg = System.Drawing.Image.FromFile(@imagePath);
            int oldImgWidth = oldImg.Width;
            int oldImgHeight = oldImg.Height;
            double mRatio = 1.0; double wRatio = 1.0; double hRatio = 1.0;

            if (width < oldImgWidth || height < oldImgHeight)
            { // yeni boyutlardan biri eskisinden küçük olmalı

                if (height != 0)
                    hRatio = (Convert.ToDouble(oldImgHeight) / Convert.ToDouble(height));

                if (width != 0)
                    wRatio = (Convert.ToDouble(oldImgWidth) / Convert.ToDouble(width));

                if (hRatio > wRatio) mRatio = hRatio; else mRatio = wRatio;

                width = Convert.ToInt16((Convert.ToDouble(oldImgWidth) / mRatio));
                height = Convert.ToInt16((Convert.ToDouble(oldImgHeight) / mRatio));

                // resize ediyoruz
                Bitmap b = new Bitmap(width, height);
                Graphics g = Graphics.FromImage((System.Drawing.Image)b);

                g.DrawImage(oldImg, 0, 0, width, height);
                g.Dispose();
                // ----------------

                System.Drawing.Image newImg = (System.Drawing.Image)b;

                if (newImagePath == "")
                { // üstüne yazacağız 
                    oldImg.Dispose();
                    newImg.Save(@imagePath);
                }
                else
                    newImg.Save(@newImagePath);

                return true;
            }
            else
                return false;
        }

        #endregion

        #region Dosya Fonksiyonları

        /// <summary>
        /// Dosya boyutunu verir
        /// </summary>
        /// <param name="filePath">Boyutu alınacak dosya yolu</param>
        /// <param name="unit">gb, mb, kb, b</param>
        /// <returns></returns>
        public static long GetFileSize(string filePath, string unit)
        {
            long fileSize = new FileInfo(filePath).Length;
            switch (unit)
            {
                case "gb": return (fileSize / 1024 / 1024 / 1024);
                case "mb": return (fileSize / 1024 / 1024);
                case "kb": return (fileSize / 1024);
                default: return (fileSize);
            }
        }

        /// <summary>
        /// Dosyanın mime-type ını verir
        /// </summary>
        /// <param name="fileName">Mime-type'ı bulunacak dosya adı</param>
        /// <returns></returns>
        public static string GetMimeType(string fileName)
        {
            switch (System.IO.Path.GetExtension(fileName).ToLower())
            {
                case ".3dm": return "x-world/x-3dmf";
                case ".3dmf": return "x-world/x-3dmf";
                case ".a": return "application/octet-stream";
                case ".aab": return "application/x-authorware-bin";
                case ".aam": return "application/x-authorware-map";
                case ".aas": return "application/x-authorware-seg";
                case ".abc": return "text/vnd.abc";
                case ".acgi": return "text/html";
                case ".afl": return "video/animaflex";
                case ".ai": return "application/postscript";
                case ".aif": return "audio/aiff";
                case ".aifc": return "audio/aiff";
                case ".aiff": return "audio/aiff";
                case ".aim": return "application/x-aim";
                case ".aip": return "text/x-audiosoft-intra";
                case ".ani": return "application/x-navi-animation";
                case ".aos": return "application/x-nokia-9000-communicator-add-on-software";
                case ".aps": return "application/mime";
                case ".arc": return "application/octet-stream";
                case ".arj": return "application/arj";
                case ".art": return "image/x-jg";
                case ".asf": return "video/x-ms-asf";
                case ".asm": return "text/x-asm";
                case ".asp": return "text/asp";
                case ".asx": return "video/x-ms-asf";
                case ".au": return "audio/basic";
                case ".avi": return "video/avi";
                case ".avs": return "video/avs-video";
                case ".bcpio": return "application/x-bcpio";
                case ".bin": return "application/octet-stream";
                case ".bm": return "image/bmp";
                case ".bmp": return "image/bmp";
                case ".boo": return "application/book";
                case ".book": return "application/book";
                case ".boz": return "application/x-bzip2";
                case ".bsh": return "application/x-bsh";
                case ".bz": return "application/x-bzip";
                case ".bz2": return "application/x-bzip2";
                case ".c": return "text/plain";
                case ".c++": return "text/plain";
                case ".cat": return "application/vnd.ms-pki.seccat";
                case ".cc": return "text/plain";
                case ".ccad": return "application/clariscad";
                case ".cco": return "application/x-cocoa";
                case ".cdf": return "application/cdf";
                case ".cer": return "application/pkix-cert";
                case ".cha": return "application/x-chat";
                case ".chat": return "application/x-chat";
                case ".class": return "application/java";
                case ".com": return "application/octet-stream";
                case ".conf": return "text/plain";
                case ".cpio": return "application/x-cpio";
                case ".cpp": return "text/x-c";
                case ".cpt": return "application/x-cpt";
                case ".crl": return "application/pkcs-crl";
                case ".crt": return "application/pkix-cert";
                case ".csh": return "application/x-csh";
                case ".css": return "text/css";
                case ".cxx": return "text/plain";
                case ".dcr": return "application/x-director";
                case ".deepv": return "application/x-deepv";
                case ".def": return "text/plain";
                case ".der": return "application/x-x509-ca-cert";
                case ".dif": return "video/x-dv";
                case ".dir": return "application/x-director";
                case ".dl": return "video/dl";
                case ".doc": return "application/msword";
                case ".dot": return "application/msword";
                case ".dp": return "application/commonground";
                case ".drw": return "application/drafting";
                case ".dump": return "application/octet-stream";
                case ".dv": return "video/x-dv";
                case ".dvi": return "application/x-dvi";
                case ".dwf": return "model/vnd.dwf";
                case ".dwg": return "image/vnd.dwg";
                case ".dxf": return "image/vnd.dwg";
                case ".dxr": return "application/x-director";
                case ".el": return "text/x-script.elisp";
                case ".elc": return "application/x-elc";
                case ".env": return "application/x-envoy";
                case ".eps": return "application/postscript";
                case ".es": return "application/x-esrehber";
                case ".etx": return "text/x-setext";
                case ".evy": return "application/envoy";
                case ".exe": return "application/octet-stream";
                case ".f": return "text/plain";
                case ".f77": return "text/x-fortran";
                case ".f90": return "text/plain";
                case ".fdf": return "application/vnd.fdf";
                case ".fif": return "image/fif";
                case ".fli": return "video/fli";
                case ".flo": return "image/florian";
                case ".flx": return "text/vnd.fmi.flexstor";
                case ".fmf": return "video/x-atomic3d-feature";
                case ".for": return "text/x-fortran";
                case ".fpx": return "image/vnd.fpx";
                case ".frl": return "application/freeloader";
                case ".funk": return "audio/make";
                case ".g": return "text/plain";
                case ".g3": return "image/g3fax";
                case ".gif": return "image/gif";
                case ".gl": return "video/gl";
                case ".gsd": return "audio/x-gsm";
                case ".gsm": return "audio/x-gsm";
                case ".gsp": return "application/x-gsp";
                case ".gss": return "application/x-gss";
                case ".gtar": return "application/x-gtar";
                case ".gz": return "application/x-gzip";
                case ".gzip": return "application/x-gzip";
                case ".h": return "text/plain";
                case ".hdf": return "application/x-hdf";
                case ".help": return "application/x-helpfile";
                case ".hgl": return "application/vnd.hp-hpgl";
                case ".hh": return "text/plain";
                case ".hlb": return "text/x-script";
                case ".hlp": return "application/hlp";
                case ".hpg": return "application/vnd.hp-hpgl";
                case ".hpgl": return "application/vnd.hp-hpgl";
                case ".hqx": return "application/binhex";
                case ".hta": return "application/hta";
                case ".htc": return "text/x-component";
                case ".htm": return "text/html";
                case ".html": return "text/html";
                case ".htmls": return "text/html";
                case ".htt": return "text/webviewhtml";
                case ".htx": return "text/html";
                case ".ice": return "x-conference/x-cooltalk";
                case ".ico": return "image/x-icon";
                case ".idc": return "text/plain";
                case ".ief": return "image/ief";
                case ".iefs": return "image/ief";
                case ".iges": return "application/iges";
                case ".igs": return "application/iges";
                case ".ima": return "application/x-ima";
                case ".imap": return "application/x-httpd-imap";
                case ".inf": return "application/inf";
                case ".ins": return "application/x-internett-signup";
                case ".ip": return "application/x-ip2";
                case ".isu": return "video/x-isvideo";
                case ".it": return "audio/it";
                case ".iv": return "application/x-inventor";
                case ".ivr": return "i-world/i-vrml";
                case ".ivy": return "application/x-livescreen";
                case ".jam": return "audio/x-jam";
                case ".jav": return "text/plain";
                case ".java": return "text/plain";
                case ".jcm": return "application/x-java-commerce";
                case ".jfif": return "image/jpeg";
                case ".jfif-tbnl": return "image/jpeg";
                case ".jpe": return "image/jpeg";
                case ".jpeg": return "image/jpeg";
                case ".jpg": return "image/jpeg";
                case ".jps": return "image/x-jps";
                case ".js": return "application/x-javascript";
                case ".jut": return "image/jutvision";
                case ".kar": return "audio/midi";
                case ".ksh": return "application/x-ksh";
                case ".la": return "audio/nspaudio";
                case ".lam": return "audio/x-liveaudio";
                case ".latex": return "application/x-latex";
                case ".lha": return "application/octet-stream";
                case ".lhx": return "application/octet-stream";
                case ".list": return "text/plain";
                case ".lma": return "audio/nspaudio";
                case ".log": return "text/plain";
                case ".lsp": return "application/x-lisp";
                case ".lst": return "text/plain";
                case ".lsx": return "text/x-la-asf";
                case ".ltx": return "application/x-latex";
                case ".lzh": return "application/octet-stream";
                case ".lzx": return "application/octet-stream";
                case ".m": return "text/plain";
                case ".m1v": return "video/mpeg";
                case ".m2a": return "audio/mpeg";
                case ".m2v": return "video/mpeg";
                case ".m3u": return "audio/x-mpequrl";
                case ".man": return "application/x-troff-man";
                case ".map": return "application/x-navimap";
                case ".mar": return "text/plain";
                case ".mbd": return "application/mbedlet";
                case ".mc$": return "application/x-magic-cap-package-1.0";
                case ".mcd": return "application/mcad";
                case ".mcf": return "text/mcf";
                case ".mcp": return "application/netmc";
                case ".me": return "application/x-troff-me";
                case ".mht": return "message/rfc822";
                case ".mhtml": return "message/rfc822";
                case ".mid": return "audio/midi";
                case ".midi": return "audio/midi";
                case ".mif": return "application/x-mif";
                case ".mime": return "message/rfc822";
                case ".mjf": return "audio/x-vnd.audioexplosion.mjuicemediafile";
                case ".mjpg": return "video/x-motion-jpeg";
                case ".mm": return "application/base64";
                case ".mme": return "application/base64";
                case ".mod": return "audio/mod";
                case ".moov": return "video/quicktime";
                case ".mov": return "video/quicktime";
                case ".movie": return "video/x-sgi-movie";
                case ".mp2": return "audio/mpeg";
                case ".mp3": return "audio/mpeg";
                case ".mpa": return "audio/mpeg";
                case ".mpc": return "application/x-project";
                case ".mpe": return "video/mpeg";
                case ".mpeg": return "video/mpeg";
                case ".mpg": return "video/mpeg";
                case ".mpga": return "audio/mpeg";
                case ".mpp": return "application/vnd.ms-project";
                case ".mpt": return "application/vnd.ms-project";
                case ".mpv": return "application/vnd.ms-project";
                case ".mpx": return "application/vnd.ms-project";
                case ".mrc": return "application/marc";
                case ".ms": return "application/x-troff-ms";
                case ".mv": return "video/x-sgi-movie";
                case ".my": return "audio/make";
                case ".mzz": return "application/x-vnd.audioexplosion.mzz";
                case ".nap": return "image/naplps";
                case ".naplps": return "image/naplps";
                case ".nc": return "application/x-netcdf";
                case ".ncm": return "application/vnd.nokia.configuration-message";
                case ".nif": return "image/x-niff";
                case ".niff": return "image/x-niff";
                case ".nix": return "application/x-mix-transfer";
                case ".nsc": return "application/x-conference";
                case ".nvd": return "application/x-navidoc";
                case ".o": return "application/octet-stream";
                case ".oda": return "application/oda";
                case ".omc": return "application/x-omc";
                case ".omcd": return "application/x-omcdatamaker";
                case ".omcr": return "application/x-omcregerator";
                case ".p": return "text/x-pascal";
                case ".p10": return "application/pkcs10";
                case ".p12": return "application/pkcs-12";
                case ".p7a": return "application/x-pkcs7-signature";
                case ".p7c": return "application/pkcs7-mime";
                case ".p7m": return "application/pkcs7-mime";
                case ".p7r": return "application/x-pkcs7-certreqresp";
                case ".p7s": return "application/pkcs7-signature";
                case ".part": return "application/pro_eng";
                case ".pas": return "text/pascal";
                case ".pbm": return "image/x-portable-bitmap";
                case ".pcl": return "application/vnd.hp-pcl";
                case ".pct": return "image/x-pict";
                case ".pcx": return "image/x-pcx";
                case ".pdb": return "chemical/x-pdb";
                case ".pdf": return "application/pdf";
                case ".pfunk": return "audio/make";
                case ".pgm": return "image/x-portable-greymap";
                case ".pic": return "image/pict";
                case ".pict": return "image/pict";
                case ".pkg": return "application/x-newton-compatible-pkg";
                case ".pko": return "application/vnd.ms-pki.pko";
                case ".pl": return "text/plain";
                case ".plx": return "application/x-pixclscript";
                case ".pm": return "image/x-xpixmap";
                case ".pm4": return "application/x-pagemaker";
                case ".pm5": return "application/x-pagemaker";
                case ".png": return "image/png";
                case ".pnm": return "application/x-portable-anymap";
                case ".pot": return "application/vnd.ms-powerpoint";
                case ".pov": return "model/x-pov";
                case ".ppa": return "application/vnd.ms-powerpoint";
                case ".ppm": return "image/x-portable-pixmap";
                case ".pps": return "application/vnd.ms-powerpoint";
                case ".ppt": return "application/vnd.ms-powerpoint";
                case ".ppz": return "application/vnd.ms-powerpoint";
                case ".pre": return "application/x-freelance";
                case ".prt": return "application/pro_eng";
                case ".ps": return "application/postscript";
                case ".psd": return "application/octet-stream";
                case ".pvu": return "paleovu/x-pv";
                case ".pwz": return "application/vnd.ms-powerpoint";
                case ".py": return "text/x-script.phyton";
                case ".pyc": return "applicaiton/x-bytecode.python";
                case ".qcp": return "audio/vnd.qcelp";
                case ".qd3": return "x-world/x-3dmf";
                case ".qd3d": return "x-world/x-3dmf";
                case ".qif": return "image/x-quicktime";
                case ".qt": return "video/quicktime";
                case ".qtc": return "video/x-qtc";
                case ".qti": return "image/x-quicktime";
                case ".qtif": return "image/x-quicktime";
                case ".ra": return "audio/x-pn-realaudio";
                case ".ram": return "audio/x-pn-realaudio";
                case ".ras": return "application/x-cmu-raster";
                case ".rast": return "image/cmu-raster";
                case ".rexx": return "text/x-script.rexx";
                case ".rf": return "image/vnd.rn-realflash";
                case ".rgb": return "image/x-rgb";
                case ".rm": return "application/vnd.rn-realmedia";
                case ".rmi": return "audio/mid";
                case ".rmm": return "audio/x-pn-realaudio";
                case ".rmp": return "audio/x-pn-realaudio";
                case ".rng": return "application/ringing-tones";
                case ".rnx": return "application/vnd.rn-realplayer";
                case ".roff": return "application/x-troff";
                case ".rp": return "image/vnd.rn-realpix";
                case ".rpm": return "audio/x-pn-realaudio-plugin";
                case ".rt": return "text/richtext";
                case ".rtf": return "text/richtext";
                case ".rtx": return "text/richtext";
                case ".rv": return "video/vnd.rn-realvideo";
                case ".s": return "text/x-asm";
                case ".s3m": return "audio/s3m";
                case ".saveme": return "application/octet-stream";
                case ".sbk": return "application/x-tbook";
                case ".scm": return "application/x-lotusscreencam";
                case ".sdml": return "text/plain";
                case ".sdp": return "application/sdp";
                case ".sdr": return "application/sounder";
                case ".sea": return "application/sea";
                case ".set": return "application/set";
                case ".sgm": return "text/sgml";
                case ".sgml": return "text/sgml";
                case ".sh": return "application/x-sh";
                case ".shar": return "application/x-shar";
                case ".shtml": return "text/html";
                case ".sid": return "audio/x-psid";
                case ".sit": return "application/x-sit";
                case ".skd": return "application/x-koan";
                case ".skm": return "application/x-koan";
                case ".skp": return "application/x-koan";
                case ".skt": return "application/x-koan";
                case ".sl": return "application/x-seelogo";
                case ".smi": return "application/smil";
                case ".smil": return "application/smil";
                case ".snd": return "audio/basic";
                case ".sol": return "application/solids";
                case ".spc": return "text/x-speech";
                case ".spl": return "application/futuresplash";
                case ".spr": return "application/x-sprite";
                case ".sprite": return "application/x-sprite";
                case ".src": return "application/x-wais-source";
                case ".ssi": return "text/x-server-parsed-html";
                case ".ssm": return "application/streamingmedia";
                case ".sst": return "application/vnd.ms-pki.certstore";
                case ".step": return "application/step";
                case ".stl": return "application/sla";
                case ".stp": return "application/step";
                case ".sv4cpio": return "application/x-sv4cpio";
                case ".sv4crc": return "application/x-sv4crc";
                case ".svf": return "image/vnd.dwg";
                case ".svr": return "application/x-world";
                case ".swf": return "application/x-shockwave-flash";
                case ".t": return "application/x-troff";
                case ".talk": return "text/x-speech";
                case ".tar": return "application/x-tar";
                case ".tbk": return "application/toolbook";
                case ".tcl": return "application/x-tcl";
                case ".tcsh": return "text/x-script.tcsh";
                case ".tex": return "application/x-tex";
                case ".texi": return "application/x-texinfo";
                case ".texinfo": return "application/x-texinfo";
                case ".text": return "text/plain";
                case ".tgz": return "application/x-compressed";
                case ".tif": return "image/tiff";
                case ".tiff": return "image/tiff";
                case ".tr": return "application/x-troff";
                case ".tsi": return "audio/tsp-audio";
                case ".tsp": return "application/dsptype";
                case ".tsv": return "text/tab-separated-values";
                case ".turbot": return "image/florian";
                case ".txt": return "text/plain";
                case ".uil": return "text/x-uil";
                case ".uni": return "text/uri-list";
                case ".unis": return "text/uri-list";
                case ".unv": return "application/i-deas";
                case ".uri": return "text/uri-list";
                case ".uris": return "text/uri-list";
                case ".ustar": return "application/x-ustar";
                case ".uu": return "application/octet-stream";
                case ".uue": return "text/x-uuencode";
                case ".vcd": return "application/x-cdlink";
                case ".vcs": return "text/x-vcalendar";
                case ".vda": return "application/vda";
                case ".vdo": return "video/vdo";
                case ".vew": return "application/groupwise";
                case ".viv": return "video/vivo";
                case ".vivo": return "video/vivo";
                case ".vmd": return "application/vocaltec-media-desc";
                case ".vmf": return "application/vocaltec-media-file";
                case ".voc": return "audio/voc";
                case ".vos": return "video/vosaic";
                case ".vox": return "audio/voxware";
                case ".vqe": return "audio/x-twinvq-plugin";
                case ".vqf": return "audio/x-twinvq";
                case ".vql": return "audio/x-twinvq-plugin";
                case ".vrml": return "application/x-vrml";
                case ".vrt": return "x-world/x-vrt";
                case ".vsd": return "application/x-visio";
                case ".vst": return "application/x-visio";
                case ".vsw": return "application/x-visio";
                case ".w60": return "application/wordperfect6.0";
                case ".w61": return "application/wordperfect6.1";
                case ".w6w": return "application/msword";
                case ".wav": return "audio/wav";
                case ".wb1": return "application/x-qpro";
                case ".wbmp": return "image/vnd.wap.wbmp";
                case ".web": return "application/vnd.xara";
                case ".wiz": return "application/msword";
                case ".wk1": return "application/x-123";
                case ".wmf": return "windows/metafile";
                case ".wml": return "text/vnd.wap.wml";
                case ".wmlc": return "application/vnd.wap.wmlc";
                case ".wmls": return "text/vnd.wap.wmlscript";
                case ".wmlsc": return "application/vnd.wap.wmlscriptc";
                case ".word": return "application/msword";
                case ".wp": return "application/wordperfect";
                case ".wp5": return "application/wordperfect";
                case ".wp6": return "application/wordperfect";
                case ".wpd": return "application/wordperfect";
                case ".wq1": return "application/x-lotus";
                case ".wri": return "application/mswrite";
                case ".wrl": return "application/x-world";
                case ".wrz": return "x-world/x-vrml";
                case ".wsc": return "text/scriplet";
                case ".wsrc": return "application/x-wais-source";
                case ".wtk": return "application/x-wintalk";
                case ".xbm": return "image/x-xbitmap";
                case ".xdr": return "video/x-amt-demorun";
                case ".xgz": return "xgl/drawing";
                case ".xif": return "image/vnd.xiff";
                case ".xl": return "application/excel";
                case ".xla": return "application/vnd.ms-excel";
                case ".xlb": return "application/vnd.ms-excel";
                case ".xlc": return "application/vnd.ms-excel";
                case ".xld": return "application/vnd.ms-excel";
                case ".xlk": return "application/vnd.ms-excel";
                case ".xll": return "application/vnd.ms-excel";
                case ".xlm": return "application/vnd.ms-excel";
                case ".xls": return "application/vnd.ms-excel";
                case ".xlt": return "application/vnd.ms-excel";
                case ".xlv": return "application/vnd.ms-excel";
                case ".xlw": return "application/vnd.ms-excel";
                case ".xm": return "audio/xm";
                case ".xml": return "application/xml";
                case ".xmz": return "xgl/movie";
                case ".xpix": return "application/x-vnd.ls-xpix";
                case ".xpm": return "image/xpm";
                case ".x-png": return "image/png";
                case ".xsr": return "video/x-amt-showrun";
                case ".xwd": return "image/x-xwd";
                case ".xyz": return "chemical/x-pdb";
                case ".z": return "application/x-compressed";
                case ".zip": return "application/zip";
                case ".zoo": return "application/octet-stream";
                case ".zsh": return "text/x-script.zsh";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".dotx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                case ".docm": return "application/vnd.ms-word.document.macroEnabled.12";
                case ".dotm": return "application/vnd.ms-word.template.macroEnabled.12";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".xltx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                case ".xlsm": return "application/vnd.ms-excel.sheet.macroEnabled.12";
                case ".xltm": return "application/vnd.ms-excel.template.macroEnabled.12";
                case ".xlam": return "application/vnd.ms-excel.addin.macroEnabled.12";
                case ".xlsb": return "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
                case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".potx": return "application/vnd.openxmlformats-officedocument.presentationml.template";
                case ".ppsx": return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                case ".ppam": return "application/vnd.ms-powerpoint.addin.macroEnabled.12";
                case ".pptm": return "application/vnd.ms-powerpoint.presentation.macroEnabled.12";
                case ".potm": return "application/vnd.ms-powerpoint.template.macroEnabled.12";
                case ".ppsm": return "application/vnd.ms-powerpoint.slideshow.macroEnabled.12";
                default: // bulamadık
                    string mime = null;
                    string ext = System.IO.Path.GetExtension(fileName).ToLower();
                    Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                    if (rk != null && rk.GetValue("Content Type") != null)
                        mime = rk.GetValue("Content Type").ToString();

                    return mime ?? "application/octetstream";
            }
        }

        /// <summary>
        /// Dosyanın olup olmadığını kontrol eder
        /// </summary>
        /// <param name="filePath">Kontrol edilecek dosya</param>
        /// <returns></returns>
        public static bool CheckFile(string filePath)
        {
            if (File.Exists(filePath))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Klasörün olup olmadığını kontrol eder
        /// </summary>
        /// <param name="folderPath">Kontrol edilecek klasör</param>
        /// <returns></returns>
        public static bool CheckFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Verilen dosyayı siler
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DeleteFile(string filePath)
        {
            if (CheckFile(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// Verilen klasörü alt klasörleriyle birlikte siler
        /// </summary>
        /// <param name="folderPath">Silinecek klasör</param>
        /// <returns></returns>
        public static bool DeleteFolder(string folderPath)
        {
            if (CheckFolder(folderPath))
            {
                try
                {
                    Directory.Delete(folderPath, true);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// Alt klasörleriyle birlikte klasörü kopyalar
        /// </summary>
        /// <param name="source">Kopyalanacak klasör</param>
        /// <param name="destination">Hedef klasör</param>
        public static void CopyDirectory(string source, string destination)
        {
            // klasör yoksa yeni oluşturuyoruz
            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);

            // klasördeki dosya ve alt klasörler arasında döngü kurduk
            foreach (var entry in Directory.GetFileSystemEntries(source))
            {
                if (Directory.Exists(entry))
                { // alt klasör kopyalayalım
                    CopyDirectory(entry, Path.Combine(destination, Path.GetFileName(entry)));
                }
                else
                { // dosya kopyalayalım
                    File.Copy(entry, Path.Combine(destination, Path.GetFileName(entry)), true);
                }
            }
        }

        /// <summary>
        /// Mevcut sayfanın dosya adını verir (Sadece Web)
        /// </summary>
        /// <returns></returns>
        public static string GetPageName()
        {
            return HttpContext.Current.Request.Url.AbsolutePath.Substring(HttpContext.Current.Request.Url.AbsolutePath.LastIndexOf("/") + 1, (HttpContext.Current.Request.Url.AbsolutePath.Length - HttpContext.Current.Request.Url.AbsolutePath.LastIndexOf("/") - 1)).Split('?')[0];
        }

        /// <summary>
        /// Dosya adı, query hariç tam yolu döndürür
        /// </summary>
        /// <returns></returns>
        public static string GetAbsolutePath()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsoluteUri.Split('/')[HttpContext.Current.Request.Url.AbsoluteUri.Split('/').Length - 1], "");
        }

        #endregion

        #region Tarih/Saat Fonksiyonları

        /// <summary>
        /// Verilen tarihler arasında tarih farkını verir
        /// </summary>
        /// <param name="firstDate">Başlangıç tarihi</param>
        /// <param name="lastDate">Bitiş tarihi</param>
        /// <param name="result">year, month, week, day, hour, minute, second, milisecond</param>
        /// <returns></returns>
        public static long DateDiff(DateTime firstDate, DateTime lastDate, string result)
        {
            TimeSpan ts = firstDate - lastDate;
            switch (result)
            {
                case "year": return Convert.ToInt64(ts.TotalDays / 365);
                case "month": return Convert.ToInt64(ts.TotalDays / 30);
                case "week": return Convert.ToInt64(ts.TotalDays / 7);
                case "day": return Convert.ToInt64(ts.TotalDays);
                case "hour": return Convert.ToInt64(ts.TotalHours);
                case "minute": return Convert.ToInt64(ts.TotalMinutes);
                case "second": return Convert.ToInt64(ts.TotalSeconds);
                case "milisecond": return Convert.ToInt64(ts.TotalMilliseconds);
                default: return 0;
            }
        }

        /// <summary>
        /// Verilen tarihi belirlenen formata çevirir
        /// </summary>
        /// <param name="date">Tarih</param>
        /// <param name="dateFormat">Format</param>
        /// <returns></returns>
        public static string ConvertDate(DateTime date, string dateFormat)
        {
            return date.ToString(dateFormat);
        }

        /// <summary>
        /// Sql tarih sorgusu hazırlar
        /// </summary>
        /// <param name="sdField">Başlangıç tarihi alanı</param>
        /// <param name="edField">Bitiş tarihi alanı</param>
        /// <param name="startDate">Başlangıç tarihi</param>
        /// <param name="endDate">Bitiş tarihi</param>
        /// <param name="dateFormat">Tarih formatı</param>
        /// <returns></returns>
        public static string GenerateDateFilterSql(string sdField, string edField, DateTime startDate, DateTime endDate, string dateFormat)
        {
            string filterStr = "";

            if (endDate == Convert.ToDateTime("1900-01-01"))
                filterStr += "'" + ConvertDate(startDate, dateFormat) + "' BETWEEN " + sdField + " AND (CASE WHEN " + edField + " IS NULL THEN DATEADD(year, 5, getDate()) ELSE " + edField + " END)";
            else
            {
                filterStr += "('" + ConvertDate(startDate, dateFormat) + "' BETWEEN " + sdField + " AND (CASE WHEN " + edField + " IS NULL THEN DATEADD(year, 5, getDate()) ELSE " + edField + " END) OR ";
                filterStr += "'" + ConvertDate(endDate, dateFormat) + "' BETWEEN " + sdField + " AND (CASE WHEN " + edField + " IS NULL THEN DATEADD(year, 5, getDate()) ELSE " + edField + " END)) OR ";
                filterStr += "('" + ConvertDate(startDate, dateFormat) + "' < " + sdField + " AND ";
                filterStr += "'" + ConvertDate(endDate, dateFormat) + "' > (CASE WHEN " + edField + " IS NULL THEN getDate() ELSE " + edField + " END))";
            }

            return "(" + filterStr + ")";
        }

        /// <summary>
        /// Saniye olarak verilen süreyi tam zamana çevirir
        /// </summary>
        /// <param name="seconds">Saniye</param>
        /// <param name="showOnlyFirstTwo">Sadece ilk iki değeri göster (saat, dakika)</param>
        /// <param name="lngSecond">Saniye yazısı (Örn. sn.)</param>
        /// <param name="lngMinute">Dakika yazısı (Örn. dk.)</param>
        /// <param name="lngHour">Saat yazısı (Örn. s.)</param>
        /// <returns></returns>
        public static string ConvertSeconds(double seconds, bool showOnlyFirstTwo, string lngSecond, string lngMinute, string lngHour)
        {
            string retVal = "";
            if (seconds < 60)
            { // dakikadan küçükse
                retVal = RoundUp(seconds) + " " + lngSecond;
            }
            else if (seconds > 59 && seconds < 3600)
            { //dakikadan büyük saatten küçükse
                retVal = Math.Floor(seconds / 60) + " " + lngMinute;
                if ((Math.Floor(seconds / 60) * 60) < seconds)
                { // saniye küsür var
                    retVal += " " + RoundUp((seconds - Math.Floor(seconds / 60) * 60)) + " " + lngSecond;
                }
            }
            else
            { // saatse
                retVal = Math.Floor(seconds / 3600) + " " + " " + lngHour;
                if ((Math.Floor(seconds / 3600) * 3600) < seconds)
                { // saniye küsür var belkide dakika
                    double extra = seconds - (Math.Floor(seconds / 3600) * 3600);

                    if (extra > 59)
                    { // demekki dakika var
                        retVal += " " + Math.Floor(extra / 60) + " " + lngMinute;
                    }

                    if (!showOnlyFirstTwo) // sadece ilk ikisini göster
                    {
                        if ((Math.Floor(extra / 60) * 60) < extra)
                        { // saniye küsürüde var maşallah
                            retVal += " " + RoundUp((extra - (Math.Floor(extra / 60) * 60))) + " " + lngSecond;
                        }
                    }
                }
            }
            return retVal;
        }

        #endregion

        #region Data Fonksiyonları

        /// <summary>
        /// DataTable'a filtre uygulayıp dönen ilk satırda belirlenen değeri verir
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="filter">Filtre</param>
        /// <param name="column">Sütun</param>
        /// <returns></returns>
        public static object SelectFromDataTable(DataTable dt, string filter, string column)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = filter;

            try
            {
                return dv.ToTable().Rows[0][column];
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Diğer Fonksiyonlar

        /// <summary>
        /// Kullanıcının IP adresi (IPV6 dahil)
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            string IP4Address = String.Empty;

            foreach (System.Net.IPAddress IPA in System.Net.Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (System.Net.IPAddress IPA in System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }

        /// <summary>
        /// Verilen kontrolü html stringi olarak döndürür
        /// </summary>
        /// <param name="control">Kontrol</param>
        /// <returns></returns>
        public static string RenderControl(Control control)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);

            control.RenderControl(hw);
            return sb.ToString();
        }

        /// <summary>
        /// Bilgisayarın internete bağlı olup olmadığını kontrol eder
        /// </summary>
        /// <param name="webSite">Bağlanmaya çalışılacak websitesi, null gelirse google</param>
        /// <returns></returns>
        public static bool IsInternetConnected(string webSite)
        {
            try
            {
                webSite = webSite == null ? "www.google.com" : webSite;
                System.Net.IPHostEntry ip = System.Net.Dns.GetHostEntry(webSite);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// HDD seri numarasını alır
        /// </summary>
        /// <returns>HDD Seri Numarası</returns>
        public static string GetHardDiskID()
        {
            ManagementClass partionsClass = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection partions = partionsClass.GetInstances();

            string hdd = string.Empty;
            foreach (ManagementObject partion in partions)
            {
                hdd = Convert.ToString(partion["VolumeSerialNumber"]);

                if (hdd != string.Empty)
                    return hdd;
            }

            return hdd;
        }

        /// <summary>
        /// MAC adresini alır
        /// </summary>
        /// <returns>MAC Adresi</returns>
        public static string GetMACAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string MACAddress = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                if (MACAddress == String.Empty) // only return MAC Address from first card
                {
                    if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }

            MACAddress = MACAddress.Replace(":", "");
            return MACAddress;
        }
        #endregion
    }
}