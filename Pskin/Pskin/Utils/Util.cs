using System;
using System.Text.RegularExpressions;

namespace Pskin.Utils
{
    public class Util
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        public static bool IsEmail(string email)
        {
            var emailPattern = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            return Regex.IsMatch(email.ToLower(), emailPattern);
        }

        public static string TimeAgo(DateTime date)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return "Hace un momento";

            if (delta < 2 * MINUTE)
                return "Hace un minuto";

            if (delta < 45 * MINUTE)
                return "Hace " + ts.Minutes + " minutos";

            if (delta < 90 * MINUTE)
                return "Hace una hora";

            if (delta < 24 * HOUR)
                return "Hace " + ts.Hours + " horas";

            if (delta < 48 * HOUR)
                return "Ayer";

            if (delta < 30 * DAY)
                return "Hace " + ts.Days + " días";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "Hace un mes" : "Hace " + months + " meses";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "Hace un año" : "Hace " + years + " años s";
            }
        }
    }
}
