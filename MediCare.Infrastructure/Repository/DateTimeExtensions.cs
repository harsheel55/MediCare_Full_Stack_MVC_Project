namespace MediCare_MVC_Project.MediCare.Infrastructure.Repository
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime date)
        {
            int diff = date.DayOfWeek - DayOfWeek.Sunday;
            if (diff < 0) diff += 7;
            return date.AddDays(-diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime date)
        {
            int diff = DayOfWeek.Saturday - date.DayOfWeek;
            if (diff < 0) diff += 7;
            return date.AddDays(diff).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }
    }
}