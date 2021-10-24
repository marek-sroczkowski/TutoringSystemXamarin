using SQLite;

namespace TutoringSystemMobile.Services.SQLite
{
    public interface ISqliteDb
    {
        SQLiteConnection GetConnection();
    }
}
