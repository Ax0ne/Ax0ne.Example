/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/17 18:27:39
 *  FileName:IUserRepository.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example.Domain.Models;

namespace Example.Repository.Interface
{
    public interface IUserRepository:IRepository<User>
    {
        string GetAddressByUserId(int id);
    }
}
