using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OH.Utilities;

namespace OH.Utilities
{
    public static class DateTimeUlti
    {
        public static DateTime MinDate(DateTime s1, DateTime s2)
        {
            return s1 < s2 ? s1 : s2;
        }

        public static DateTime MaxDate(DateTime s1, DateTime s2)
        {
            return s1 > s2 ? s1 : s2;
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int diff = endOfWeek - dt.DayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(1 * diff).Date;
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime EndOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
        }

        public static int Current_Quarter(this DateTime dt)
        {
            return (dt.Month - 1) / 3 + 1;
        }

        public static int Current_WeekOfMonth(this DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }

        public static DateTime StartOfQuarter(this DateTime dt)
        {
            int quarterNumber = dt.Current_Quarter();
            return new DateTime(dt.Year, (quarterNumber - 1) * 3 + 1, 1);
        }

        public static DateTime EndOfQuarter(this DateTime dt)
        {
            return dt.StartOfQuarter().AddMonths(3).AddDays(-1);
        }

        public static DateTime StartOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        public static DateTime EndOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 12, 31);
        }

        public static DateTime Parse(string date)
        {
            //return DateTime.ParseExact(date, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.AssumeUniversal);
            return DateTime.ParseExact(date, "dd/MM/yyyy", new CultureInfo("vi-VN"));
        }

        public static bool IsValid(this DateTime dt)
        {
            return dt != DateTime.MinValue && dt != DateTime.MaxValue;
        }

        public static bool IsValid(this DateTime? dt)
        {
            return dt != null && dt != DateTime.MinValue && dt != DateTime.MaxValue;
        }

        public static bool IsTimeNull(this DateTime dt)
        {
            return dt.Hour == 0 && dt.Minute == 0;
        }

        public static bool IsTimeNull(this DateTime? dt)
        {
            return dt != null && dt.Value.Hour == 0;
        }

        public static bool IsTimeNullValid(this DateTime? dt)
        {
            return dt.IsValid() && !dt.IsTimeNull();
        }

        public static bool IsTimeNullValid(this DateTime dt)
        {
            return dt.IsValid() && !dt.IsTimeNull();
        }

        public static bool IsWeekend(this DateTime dt)
        {
            return (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);
        }

        public static bool IsEndOfMonth(this DateTime dt)
        {
            return dt.AddDays(1).Day == 1;
        }

        public static string ToDateString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
        public static string ToDateString_InName(this DateTime date)
        {
            return date.ToString("yyyyMMdd_") + $"{date.Hour}{date.Minute}";
        }
        public static string ToDateString_InName_Short(this DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        public static string ToHoursString(this DateTime date)
        {
            return date.ToString("HH:mm");
        }

        public static string ToDateStringText(this DateTime date)
        {
            return $"Ngày {date.Day} tháng {date.Month} năm {date.Year}";
        }

        public static string ToDateTimeString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public static string ToDateTimeString(this DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
        }

        public static string ToDateString(this DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : "";
        }

        public static string ToMonthString(this DateTime date)
        {
            return date.ToString("MM/yyyy");
        }

        public static string ToMonthString(this DateTime? date)
        {
            return date.HasValue ? date.Value.ToMonthString() : "";
        }

        public static DateTime? ToBirthDay(this DateTime? date)
        {
            if (date == null)
                return null;
            return date.Value.ToBirthDay();
        }

        public static DateTime? ToBirthDay(this DateTime date)
        {
            DateTime? birthday = null;
            try
            {
                birthday = new DateTime(DateTime.Today.Year, date.Month, date.Day);
            }
            catch (Exception ex)
            {
                return null;
            }

            return birthday;
        }

        public static string GetDayOfWeek(this DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
                return "CN";
            return ((int)date.DayOfWeek + 1).ToString();
        }

        /// <summary>
        /// Hàm tính khoảng ngày từ ngày bắt đầu đến ngày kết thúc (VD: từ 1/12/2014 đến 4/12/2014 sẽ có 4 ngày)
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static int Get2DaysDuration(DateTime startDate, DateTime endDate)
        {
            //nếu date2 - date1 sẽ chỉ có 3 ngày (4-1=3), phải cộng thêm 1 đơn vị
            //date2 > date1
            if (endDate < startDate)
                return 0;
            return (endDate.Date - startDate.Date).Days + 1;
        }

        private static readonly long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

        public static long ToJavaScriptMilliseconds(this DateTime dt)
        {
            return (long)((dt.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
        }

        public static double ToTimestamp(this DateTime dt)
        {
            return (dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime ToDatetime(this string dt)
        {
            if (dt.Contains(" "))
            {
                try
                {
                    return DateTime.ParseExact(dt, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    return DateTime.ParseExact(dt, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
                }
            }

            try
            {
                return DateTime.Parse(dt);
            }
            catch (Exception e)
            {
                string[] formats = { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "dd-MM-yyyy", "MM-dd-yyyy", "yyyy-MM-dd", "MMMM dd, yyyy" };
                return DateTime.ParseExact(dt, formats, new CultureInfo("vi-VN"), DateTimeStyles.None);

            }
        }

        public static DateTime ToFullDatetime(this string dt)
        {
            if (dt.Contains(" "))
            {
                try
                {
                    return DateTime.ParseExact(dt, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    return DateTime.ParseExact(dt, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                }
            }

            try
            {
                return DateTime.Parse(dt);
            }
            catch (Exception e)
            {
                string[] formats = { "dd/MM/yyyy hh:mm:ss", "MM/dd/yyyy hh:mm:ss", "yyyy/MM/dd hh:mm:ss", "dd-MM-yyyy hh:mm:ss", "MM-dd-yyyy hh:mm:ss", "yyyy-MM-dd hh:mm:ss", "MMMM dd, yyyy hh:mm:ss" };
                return DateTime.ParseExact(dt, formats, new CultureInfo("vi-VN"), DateTimeStyles.None);

            }
        }
        public static string ToDaysAgoDatetimeString(this DateTime dt)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.ToUniversalTime().Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "Một giây trước" : ts.Seconds + " giây trước";

            if (delta < 2 * MINUTE)
                return "Một phút trước";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " phút trước";

            if (delta < 90 * MINUTE)
                return "Một giờ trước";

            if (delta < 24 * HOUR)
                return ts.Hours + " giờ trước";

            if (delta < 48 * HOUR)
                return "Hôm qua";

            if (delta < 30 * DAY)
                return ts.Days + " ngày trước";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "Một tháng trước" : months + " tháng trước";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "Một năm trước" : years + " năm trước";
            }
        }

        public static DateTime? ConvertDaysAgoToDatetime(this string dateString)
        {
            const string SECOND = "giay";
            const string MINUTE = "phut";
            const string HOUR = "gio";
            const string DAY = "ngay";
            const string WEEK = "tuan";
            const string MONTH = "thang";
            const string YEAR = "nam";
            const string TODAY = "homnay";
            const string YESTERDAY = "homqua";

            var today = DateTime.Today;
            int num;
            dateString = dateString.ToLower().Convert_Chuoi_Khong_Dau();
            var splitDate = dateString.Split(" ".ToCharArray());
            var val = splitDate.First();

            if (int.TryParse(val, out num))
            {
                if (dateString.Contains(SECOND) || dateString.Contains(MINUTE) || dateString.Contains(HOUR))
                    return today;
                if (dateString.Contains(DAY))
                    return today.AddDays(-Convert.ToDouble(val));
                if (dateString.Contains(WEEK))
                    return today.AddDays(-(Convert.ToDouble(val) * 7));
                if (dateString.Contains(MONTH))
                    return today.AddMonths(-Convert.ToInt32(val));
                if (dateString.Contains(YEAR))
                    return today.AddYears(-Convert.ToInt32(val));
            }

            if (dateString.Remove_Khoang_Trang().Contains(TODAY))
                return today;
            if (dateString.Remove_Khoang_Trang().Contains(YESTERDAY))
                return today.AddDays(-1);

            foreach (var date in splitDate)
            {
                DateTime outDate = ParseExactDatetime(date);
                if (outDate != DateTime.MinValue)
                    return outDate;
            }

            return null;
        }

        public static DateTime ParseExactDatetime(string date)
        {
            DateTime outDate;
            string formatType = "dd/MM/yyyy";
            if (date.Contains('-'))
                formatType = "dd-MM-yyyy";

            if (DateTime.TryParseExact(date, formatType, CultureInfo.InvariantCulture, DateTimeStyles.None, out outDate))
                return outDate;

            return DateTime.MinValue;
        }

        public static List<DateTime> GetMonthsFromTwoDates(DateTime from, DateTime to)
        {
            if (from > to) return GetMonthsFromTwoDates(to, from);

            //var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));
            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            //if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            //{
            //    monthDiff -= 1;
            //}

            List<DateTime> results = new List<DateTime>();
            for (int i = monthDiff; i >= 0; i--)
            {
                results.Add(to.AddMonths(-i));
            }

            return results;
        }

        public static List<DateTime> GetDaysFromTwoDates(DateTime from, DateTime to)
        {
            if (from > to) return GetDaysFromTwoDates(to, from);
            var dates = new List<DateTime>();

            for (var dt = from; dt <= to; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            return dates;
        }

        public static bool HasDatesDiff(DateTime Start_Date_1, DateTime End_Date_1, DateTime Start_Date_2, DateTime End_Date_2)
        {
            return !(Start_Date_1 > End_Date_2 || End_Date_1 < Start_Date_2);
            //return (Start_Date_1 <= End_Date_2 && End_Date_1 >= Start_Date_2);
        }

        public static float GetDatesDiff(DateTime Start_Date_1, DateTime End_Date_1, DateTime Start_Date_2, DateTime End_Date_2)
        {
            if (!HasDatesDiff(Start_Date_1, End_Date_1, Start_Date_2, End_Date_2))
                return 0;
            var Max_Start_Date = MaxDate(Start_Date_1, Start_Date_2);
            var Min_End_Date = MinDate(End_Date_1, End_Date_2);
            return Get2DaysDuration(Max_Start_Date, Min_End_Date);
        }

        public static string ParseToDateString(string date)
        {
            var response = "";
            if (date.Split(':').Length == 2)
            {
                response = date;
            }
            else
            {
                try
                {
                    string[] formats = { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "dd-MM-yyyy", "MM-dd-yyyy", "yyyy-MM-dd", "MMMM dd, yyyy" };
                    response = DateTime.ParseExact(date, formats, new CultureInfo("vi-VN"), DateTimeStyles.None)

                        .ToString("dd/MM/yyyy");
                }
                catch (Exception ex)
                {
                    response = date;
                }
            }
            return response;
        }

        public static int IndexWeekOfMonth(this DateTime date)//Dùng cho CheckInLog. Tháng chia ra các đoạn 7 ngày (1->5 đoạn). xem ngày này thược đoạn thứ mấy.
        {
            var day = date.Day;
            if (day < 8)
            {
                return 1;
            }
            else if (day > 7 && day < 15)
            {
                return 2;
            }
            else if (day > 14 && day < 22)
            {
                return 3;
            }
            else if (day > 21 && day < 29)
            {
                return 4;
            }
            return 5;
        }
    }
}