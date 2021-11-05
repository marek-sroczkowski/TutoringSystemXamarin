using System.Linq;

namespace TutoringSystemMobile.Extensions
{
    public static class StringExtensions
    {
        public static long GetIdByLocation(this string location)
        {
            var locationItems = location?.Split('/');
            if (locationItems is null || locationItems.Length == 0)
                return -1;

            var id = locationItems.LastOrDefault();

            if (!long.TryParse(id, out long result))
                return -1;

            return result;
        }
    }
}
