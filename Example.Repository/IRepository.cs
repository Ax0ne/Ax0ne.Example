
/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/17 16:47:54
 *  FileName:IRepository.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Example.Domain;
using Example.Infrastructure;

namespace Example.Repository
{
    public interface IRepository<T> : IUnitOfWork where T : Entity
    {
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="isTracking">if set to <c>true</c> [is tracking].</param>
        /// <returns>T.</returns>
        T Get(int key, bool isTracking = true);
        /// <summary>
        /// Gets the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isTracking">if set to <c>true</c> [is tracking].</param>
        /// <returns>T.</returns>
        T Get(Expression<Func<T, bool>> predicate, bool isTracking = true);
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isTracking">if set to <c>true</c> [is tracking].</param>
        /// <returns>List&lt;T&gt;.</returns>
        List<T> GetList(Expression<Func<T, bool>> predicate, bool isTracking = false);
        /// <summary>
        /// Deletes the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="isSave">if set to <c>true</c> [is save].</param>
        /// <returns>System.Int32.</returns>
        int Delete(Expression<Func<T, bool>> predicate, bool isSave = true);

        /// <summary>
        /// Creates the or update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isSave">if set to <c>true</c> [is save].</param>
        /// <returns>System.Int32.</returns>
        int CreateOrUpdate(T entity, bool isSave = true);
    }
}
