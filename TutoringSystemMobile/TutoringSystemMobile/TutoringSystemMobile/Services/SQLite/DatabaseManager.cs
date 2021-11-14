using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System.Linq;
using TutoringSystemMobile.Models;
using Xamarin.Forms;

namespace TutoringSystemMobile.Services.SQLite
{
    public sealed class DatabaseManager
    {
        private static DatabaseManager instance = null;
        private static readonly object locker = new object();
        private readonly SQLiteConnection _connection;

        public static DatabaseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new DatabaseManager();
                        }
                    }
                }
                return instance;
            }
        }

        private DatabaseManager()
        {
            _connection = DependencyService.Get<ISqliteDb>().GetConnection();
            CreateDatabase();
        }

        public List<T> GetALL<T>() where T : class, new()
        {
            return _connection.GetAllWithChildren<T>().ToList();
        }

        private void CreateDatabase()
        {
            //_connection.CreateTable<JwtToken>();
        }

        public void Add<T>(T objectToAdd)
        {
            _connection.Insert(objectToAdd);
        }

        public void RemoveFromDatabase<T>(int id)
        {
            _connection.Delete<T>(id);
        }
    }
}
