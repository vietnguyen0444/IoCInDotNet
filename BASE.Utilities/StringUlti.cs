using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OH.Utilities
{
    public static class StringUlti
    {
        public static string ToSafeUrl(this string s)
        {
            s = s.Convert_Chuoi_Khong_Dau();
            s = Regex.Replace(s, @"[^a-zA-Z0-9]", "-");
            s = Regex.Replace(s, @"-{2,}", "-");

            return s.ToLower();
            //return s.Replace("/", "_").Replace("+", "-").Replace(" ", "-").Replace(",", "").Replace(".", "");
        }

        public static string Convert_Chuoi_Khong_Dau(this string s)
        {
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string Remove_Khoang_Trang(this string s)
        {
            return s.Replace(" ", "");
        }

        public static string ToUpperCaseFirst(this string p)
        {
            if (string.IsNullOrEmpty(p))
            {
                return string.Empty;
            }

            return char.ToUpper(p[0]) + p.Substring(1);
        }

        public static string CutString(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return str.Substring(0, Math.Min(length, str.Length));
        }

        public static bool WordContains(this string p, string term)
        {
            var i = p.IndexOf(term);
            var j = i + term.Length;
            var last = p.Length - 1;
            if (i < 0)
                return false;
            if (i == 0 || !Char.IsLetter(p[i - 1]))
            {
                if (j <= last && !Char.IsLetter(p[j]))
                    return true;
            }

            return false;
        }

        public static bool WordPartialContains(this string p, string term)
        {
            var i = p.IndexOf(term);
            //var j = i + term.Length;
            //var last = p.Length - 1;
            if (i < 0)
                return false;
            if (i == 0 || !Char.IsLetter(p[i - 1]))
            {
                return true;
            }

            return false;
        }

        public static string HidePhoneString(this string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return "";
            var hide_text = "****";
            var len = phone.Length;
            var num = 4;
            if (len >= num)
            {
                hide_text = phone.Substring(len - num, num);
            }

            return "******" + hide_text;
        }

        public static string HideEmail(this string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var hide_text = email.Split("@".ToCharArray());
                if (hide_text.Length >= 1)
                {
                    return "******@" + hide_text[hide_text.Length - 1];
                }
            }

            return "";
        }

        public static String GetValidYoutubeVideoId(String youtubeUrl)
        {
            if (string.IsNullOrWhiteSpace(youtubeUrl))
            {
                return "";
            }
            youtubeUrl = youtubeUrl.Trim();
            String regexPattern = "(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+";// @"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?|watch)\/|.*[?&amp;]v=)|youtu\.be\/)([^""&amp;?\/ ]{11})";
            Regex regex = new Regex(regexPattern);
            var regRes = regex.Match(youtubeUrl);
            if (regRes.Success)
            {
                return regRes.Groups[1].Value;
            }
            return null;
        }

        public static string GetValidUrl(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }
            String regexPattern = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
            Regex regex = new Regex(regexPattern);
            var regRes = regex.Match(text);
            if (regRes.Success)
            {
                return regRes.Groups[1].Value;
            }
            return null;
        }

        public static string UpperCaseFirst_2(this string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            string result = "";

            //lấy danh sách các từ  

            string[] words = s.Split(' ');

            foreach (string word in words)
            {
                // từ nào là các khoảng trắng thừa thì bỏ  
                if (word.Trim() != "")
                {
                    if (word.Length > 1)
                        result += word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower() + " ";
                    else
                        result += word.ToUpper() + " ";
                }

            }
            return result.Trim();
        }

        public class ConvertMoney
        {
            private static string Chu(string gNumber)
            {
                string result = "";
                switch (gNumber)
                {
                    case "0":
                        result = "không";
                        break;
                    case "1":
                        result = "một";
                        break;
                    case "2":
                        result = "hai";
                        break;
                    case "3":
                        result = "ba";
                        break;
                    case "4":
                        result = "bốn";
                        break;
                    case "5":
                        result = "năm";
                        break;
                    case "6":
                        result = "sáu";
                        break;
                    case "7":
                        result = "bảy";
                        break;
                    case "8":
                        result = "tám";
                        break;
                    case "9":
                        result = "chín";
                        break;
                }
                return result;
            }
            private static string Donvi(string so)
            {
                string Kdonvi = "";

                if (so.Equals("1"))
                    Kdonvi = "";
                if (so.Equals("2"))
                    Kdonvi = "nghìn";
                if (so.Equals("3"))
                    Kdonvi = "triệu";
                if (so.Equals("4"))
                    Kdonvi = "tỷ";
                if (so.Equals("5"))
                    Kdonvi = "nghìn tỷ";
                if (so.Equals("6"))
                    Kdonvi = "triệu tỷ";
                if (so.Equals("7"))
                    Kdonvi = "tỷ tỷ";

                return Kdonvi;
            }
            private static string Tach(string tach3)
            {
                string Ktach = "";
                if (tach3.Equals("000"))
                    return "";
                if (tach3.Length == 3)
                {
                    string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                    string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                    string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                    if (tr.Equals("0") && ch.Equals("0"))
                        Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                    if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                        Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                    if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                        Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                    if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                        Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                    if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                        Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                    if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                        Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                    if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                        Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                    if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                        Ktach = " không trăm mười ";
                    if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                        Ktach = " không trăm mười lăm ";
                    if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                    if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                    if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                    if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                    if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                    if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                        Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";
                }
                return Ktach;
            }
            public static string SoTienBangChu(double gNum, bool showDonVi = true)
            {
                if (gNum == 0)
                {
                    if (showDonVi)
                    {
                        return "Không đồng";
                    }
                    return "Không";
                }

                string donVi = showDonVi ? " đồng chẵn." : "";

                string lso_chu = "";
                string tach_mod = "";
                string tach_conlai = "";
                double Num = Math.Round(gNum, 0);
                string gN = Convert.ToString(Num);
                int m = Convert.ToInt32(gN.Length / 3);
                int mod = gN.Length - m * 3;
                string dau = "[+]";

                // Dau [+ , - ]
                if (gNum < 0)
                    dau = "[-]";
                dau = "";

                // Tach hang lon nhat
                if (mod.Equals(1))
                    tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
                if (mod.Equals(2))
                    tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
                if (mod.Equals(0))
                    tach_mod = "000";
                // Tach hang con lai sau mod :
                if (Num.ToString().Length > 2)
                    tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

                ///don vi hang mod
                int im = m + 1;
                if (mod > 0)
                    lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
                /// Tach 3 trong tach_conlai

                int i = m;
                int _m = m;
                int j = 1;
                string tach3 = "";
                string tach3_ = "";

                while (i > 0)
                {
                    tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                    tach3_ = tach3;
                    lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                    m = _m + 1 - j;
                    if (!tach3_.Equals("000"))
                        lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                    tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                    i = i - 1;
                    j = j + 1;
                }
                if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                    lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
                if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                    lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
                if (lso_chu.Trim().Length > 0)
                    lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + donVi;

                return lso_chu.ToString().Trim();

            }
            public static double ConvertMoneyStringToNumber(string moneyString)
            {
                var splitMoney = moneyString.Split(" ".ToCharArray());
                double money = 0;
                string val = splitMoney.First().Replace(".", ",");

                if (splitMoney.Length > 3) //3 tỷ 600 triệu
                {
                    val = splitMoney.First() + "," + splitMoney.Skip(2).First();
                }
                else if (splitMoney.Length == 2 && moneyString.ToLower().Convert_Chuoi_Khong_Dau().Contains("trieu")) //850 triệu
                {
                    val = $"0,{splitMoney.First()}";
                }

                //Format 1,5 hoặc 1,5 tỷ
                if (double.TryParse(val, out money))
                {
                    if (moneyString.ToLower().Convert_Chuoi_Khong_Dau().Contains("ty"))
                    {
                        return money * 1000000000;
                    }
                }

                return 0;
            }
        }

        public static string RemoveHtmlTag(this string value)
        {
            var txt = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
            if (txt.Contains("\r") || txt.Contains("\n"))
            {
                txt = txt.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");//Duong - 26/03/2019
            }
            txt = Regex.Replace(txt, @"\s{2,}", " ");


            return txt;
        }

    }
}