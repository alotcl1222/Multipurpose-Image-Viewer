using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   
using SQLite;

namespace MIV.Utils
{   
    /// <summary>
    /// ジェネリックなレポジトリのインターフェイス
    /// </summary>
    /// <see cref="http://stackoverflow.com/questions/8766794/how-to-create-generic-data-access-object-dao-crud-methods-with-linq-to-sql"/>
    /// <seealso cref="https://opensharp.wordpress.com/2014/10/05/using-respository-pattern-in-sqlite-with-generic-repository-advantage/"/>
    public interface IGenericRepository : IDisposable
    {
        void Insert<T>(T entity);
        void Update<T>(T entity);
        bool Delete<T>(T entity);
        T Select<T>(string pk) where T : new();
        IEnumerable<T> SelectAll<T>() where T : new();
    }

    /// <summary>
    /// ジェネリックなレポジトリのクラス
    /// </summary>
    public class GenericRepository : IGenericRepository
    {
        #region Property
        protected SQLiteConnection DB { get; set; } 
        #endregion

        #region Constructor
        public GenericRepository(string dbPath)
        {
            this.DB = new SQLiteConnection(dbPath);
        }
        #endregion

        #region Generic Repository
        virtual public void Insert<T>(T entity)
        {            
            try
            {
                int iRes = DB.Insert(entity);
            }      
            catch
            {
                // 失敗。すでにあるprimary keyを追加した時とか
            }                
            return;
        }

        public void Update<T>(T entity)
        {
            int iRes = DB.Update(entity);
            return ;
        }

        public bool Delete<T>(T entity)
        {
            int iRes = DB.Delete(entity);
            return iRes.Equals(1);
        }

        public T Select<T>(string pk) where T : new()
        {
            var map = DB.GetMapping(typeof(T));
            return DB.Query<T>(map.GetByPrimaryKeySql, pk).FirstOrDefault();
        }  

        public IEnumerable<T> SelectAll<T>() where T : new()
        {
            return new TableQuery<T>(DB).ToArray();
        }

        #endregion

        #region IDispose Region
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DB.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
                  
    }
}
