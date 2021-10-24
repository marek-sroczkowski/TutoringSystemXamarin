using SQLite;
using System.IO;
using TutoringSystemMobile.Droid.Persistance;
using TutoringSystemMobile.Services.SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(SqLiteDb))]
namespace TutoringSystemMobile.Droid.Persistance
{
    class SqLiteDb : ISqliteDb
    {
        public SQLiteConnection GetConnection()
        {
            var path = GetDatabasePath();

            return new SQLiteConnection(path);
        }

        private string GetDatabasePath()
        {
            var databaseName = "TutoringSystemMobile.db3";
            var documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(documentPath, databaseName);
        }
    }
}