using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TutoringSystemMobile.Models.Dtos.Images;
using Xamarin.Forms;

namespace TutoringSystemMobile.Services.SQLite
{
    public sealed class SQLiteManager
    {
        private static SQLiteManager instance = null;
        private static readonly object locker = new object();
        private readonly SQLiteConnection connection;

        public static SQLiteManager Instance
        {
            get
            {
                if (instance is null)
                {
                    lock (locker)
                    {
                        if (instance is null)
                        {
                            instance = new SQLiteManager();
                        }
                    }
                }

                return instance;
            }
        }

        private SQLiteManager()
        {
            connection = DependencyService.Get<ISqliteDb>().GetConnection();
            CreateDatabase();
        }

        private void CreateDatabase()
        {
            connection.CreateTable<UserImageDto>();
        }

        public IEnumerable<T> GetAll<T>() where T : class, new()
        {
            return connection.GetAllWithChildren<T>().ToList();
        }

        public T Get<T>(long id) where T : class, new()
        {
            try
            {
                return connection.Get<T>(id);
            }
            catch(Exception)
            {
                return null;
            }
        }

        public void Add<T>(T objectToAdd)
        {
            connection.Insert(objectToAdd);
        }

        public void Update<T>(T objectToUpdate)
        {
            connection.Update(objectToUpdate);
        }

        public void Remove<T>(long id)
        {
            connection.Delete<T>(id);
        }
    }
}
