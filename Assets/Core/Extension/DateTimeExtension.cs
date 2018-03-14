//************************************************
//Brief: DateTime Extension
//
//Author: Liuhaixia
//E-Mail: 609043941@qq.com
//
//History: 2017/05/04 Created by Liuhaixia
//************************************************
using System;

namespace Gowild {
    public static class DateTimeExtension {

        const String TIME_FORMAT_1 = "{0}:{1}:{2}";
        const String TIME_FORMAT_CN = "{0}天{1}时{2}分{3}秒";
        const String TIME_FORMAT_DEFAUT = "yyyy-MM-dd HH:mm:ss";
        static readonly DateTime START_TIME = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Local);

        /// <summary>
        /// 毫秒转化为时间
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Int64 ms) {
            return START_TIME.AddMilliseconds(ms);
        }

        /// <summary>
        /// 检测时间间隔天数
        /// </summary>
        /// <param name="after">截止时间</param>
        /// <param name="before">开始时间</param>
        /// <returns>间隔天数</returns>
        public static Int32 CheckDateDistance(DateTime after, DateTime before) {
            if (after.CompareTo(before) == 0) {
                return 0;
            }

            var timeDist = after - before;
            Int32 result = (Int32)Math.Floor(timeDist.TotalSeconds);

            DateTime tempAfter = new DateTime(2000, 1, 1, after.Hour, after.Minute, after.Second);
            DateTime tempBefore = new DateTime(2000, 1, 1, before.Hour, before.Minute, before.Second);

            if (tempBefore > tempAfter) {
                result++;
            }
            return result;
        }

        /// <summary>
        /// 检测日期区间有效性
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <returns>是否有效</returns>
        public static Boolean CheckDateValid(DateTime currentDate, DateTime startDate, DateTime endDate) {
            return (currentDate.CompareTo(startDate) >= 0) && (currentDate.CompareTo(endDate) <= 0);
        }


        #region Time to string

        /// <summary>
        /// String Time To String
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static String TimeToString(this String msString) {

            Int64 ms = 0;
            try {
                ms = Convert.ToInt64(msString);
            } catch {
                ms = 0;
            }

            Int32 hour = (Int32)(ms / (3600 * 1000));
            Int32 min = (Int32)(ms % (3600 * 1000) / (60 * 1000));
            Int32 sec = (Int32)(ms % (3600 * 1000) % (60 * 1000));

            return String.Format(TIME_FORMAT_1, hour, min, sec);
        }

        /// <summary>
        /// Long Time To String
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static String TimeToString(this Int32 seconds) {
            Int32 hour = (Int32)(seconds / 3600);
            Int32 min = (Int32)(seconds % 3600 / 60);
            Int32 sec = (Int32)(seconds % 3600 % 60);

            return String.Format(TIME_FORMAT_1, hour.ToString("D2"), min.ToString("D2"), sec.ToString("D2"));
        }

        /// <summary>
        /// Long Time To String
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static String TimeToString(this Int64 seconds) {
            Int32 hour = (Int32)(seconds / 3600);
            Int32 min = (Int32)(seconds % 3600 / 60);
            Int32 sec = (Int32)(seconds % 3600 % 60);

            return String.Format(TIME_FORMAT_1, hour.ToString("D2"), min.ToString("D2"), sec.ToString("D2"));
        }

        /// <summary>
        /// Long Time To String
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static String TimeToStringCN(this Int32 seconds) {
            Int32 day = seconds / (24 * 60 * 60);
            Int32 hour = seconds % (24 * 60 * 60) / 3600;
            Int32 min = seconds % (24 * 60 * 60) % 3600 / 60;
            Int32 sec = seconds % (24 * 60 * 60) % 3600 % 60;

            return String.Format(TIME_FORMAT_CN, day, hour, min, sec);
        }

        /// <summary>
        /// Long Time To String
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static String TimeToStringCN(this Int64 seconds) {
            Int32 day = (Int32)(seconds / (24 * 60 * 60));
            Int32 hour = (Int32)(seconds % (24 * 60 * 60) / 3600);
            Int32 min = (Int32)(seconds % (24 * 60 * 60) % 3600 / 60);
            Int32 sec = (Int32)(seconds % (24 * 60 * 60) % 3600 % 60);

            return String.Format(TIME_FORMAT_CN, day, hour, min, sec);
        }

        /// <summary>
        /// 毫秒转化为时间字符串
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static String TimeToStringDefault(this Int64 ms) {
            return ms.ToDateTime().ToString(TIME_FORMAT_DEFAUT);
        }

        #endregion


        /// <summary>
        /// 相对于现在剩余时间(毫秒)
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static String RemainTimeString(this Int64 ms) {
            DateTime now = DateTime.Now;
            DateTime targetTime = START_TIME.AddMilliseconds(ms);

            String result = "00:00:00";
            if (targetTime > now) {
                TimeSpan timeSpan = targetTime - now;
                result = String.Format(TIME_FORMAT_1, timeSpan.Hours.ToString("D2"), timeSpan.Minutes.ToString("D2"), timeSpan.Seconds.ToString("D2"));
            }

            return result;
        }

        /// <summary>
        /// 相对于现在剩余时间(毫秒)
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static Int64 RemainTimeLong(this Int64 ms) {
            DateTime now = DateTime.Now;
            DateTime targetTime = START_TIME.AddMilliseconds(ms);

            Int64 result = 0;
            if (targetTime > now) {
                result = (targetTime - now).Milliseconds;
            }

            return result;
        }

        /// <summary>
        /// 过期检测
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static Boolean CheckTimeExpire(this Int64 ms) {
            DateTime now = DateTime.Now;
            DateTime targetTime = START_TIME.AddMilliseconds(ms);
            if (targetTime > now) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// 获取当前时间秒数
        /// </summary>
        /// <returns></returns>
        public static Int64 CurrentTimeSecond() {
            return DateTime.Now.Second + DateTime.Now.Minute * 60 + DateTime.Now.Hour * 3600;
        }

        /// <summary>
        /// 获取当前时间毫秒数
        /// </summary>
        /// <returns></returns>
        public static Int64 CurrentTimeMillisecond() {
            return DateTime.Now.Hour * 3600000 + DateTime.Now.Minute * 60000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
        }

    }//end class
}//end namespace