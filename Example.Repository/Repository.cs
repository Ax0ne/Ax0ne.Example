/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/17 16:46:18
 *  FileName:RepositoryBase.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Example.Domain;

namespace Example.Repository
{
    public class Repository<T>:IRepositoryBase<T> where T:Entity
    {
        private DbContextBase _dbContext { get; set; }
        private DbSet<T> Entity { get; set; }
        public Repository(DbContextBase dbContext)
        {
            _dbContext = dbContext;
            Entity = dbContext.Set<T>();
        }
        // 注意访问级别
        protected internal DbContextBase DbContext { get { return _dbContext; } }

        DbContextTransaction Transaction { get; set; }

        public T Get(int key, bool isTracking = true)
        {
            return Entity.Find(key);
        }

        public T Get(Expression<Func<T, bool>> predicate, bool isTracking = true)
        {
            if (isTracking)
                return Entity.Where(predicate).FirstOrDefault();
            return Entity.AsNoTracking().Where(predicate).FirstOrDefault();
        }

        public List<T> GetList(Expression<Func<T, bool>> predicate, bool isTracking = false)
        {
            if (isTracking)
                return Entity.Where(predicate).ToList();
            return Entity.AsNoTracking().Where(predicate).ToList();
        }

        /// <summary>
        /// Deletes the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isSave">if set to <c>true</c> [is save].</param>
        /// <returns>System.Int32.</returns>
        public int Delete(Expression<Func<T, bool>> predicate, bool isSave = true)
        {
            var result = GetList(predicate, isSave);
            if (result == null || !result.Any()) return 0;
            foreach (var item in result)
            {
                if(isSave)
                    DbContext.Entry(item).State = EntityState.Deleted;
            }
            return Save();
        }

        public int CreateOrUpdate(T entity, bool isSave = true)
        {
            if (entity.Id == 0)
            {
                if (DbContext.Entry(entity).State == EntityState.Detached)
                    Entity.Attach(entity);
                DbContext.Entry(entity).State = EntityState.Added;
            }
            else
            {
                if (DbContext.Entry(entity).State == EntityState.Detached)
                    Entity.Attach(entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }
            return Save();
        }

        public void BeginTransaction(System.Data.IsolationLevel level= System.Data.IsolationLevel.Chaos)
        {
            Transaction = DbContext.Database.BeginTransaction(level);
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }
        public int Save(bool isAsync=false)
        {
            int result = 0;
            try
            {
                if (!isAsync)
                    result = DbContext.SaveChanges();
                else
                    result = DbContext.SaveChangesAsync().Result;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityErrors in ex.EntityValidationErrors)
                {
                    foreach (var error in entityErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("实体:{0}->属性:{1}->错误:{2}",entityErrors.Entry.Entity.ToString(),error.PropertyName,error.ErrorMessage));
                    }
                }
                throw ex;
            }
            catch (System.Data.Common.DbException dbException)
            {
                System.Diagnostics.Debug.WriteLine((dbException.InnerException ?? dbException).Message);
            }
            return result;
        }


        /// <summary>
        /// EF 跟踪状态级别事物,使用时操作实体对象的方法isSave参数要等于false
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EFStateTransactionCommit()
        {
            try
            {
                return DbContext.SaveChanges() > 0;
            }
            catch
            {
                DbContext.ChangeTracker.Entries().ToList().ForEach(entity => entity.State = EntityState.Unchanged);
                return false;
            }
        }
    }
}
