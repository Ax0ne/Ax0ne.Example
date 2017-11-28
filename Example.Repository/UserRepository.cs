/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/17 18:28:41
 *  FileName:UserRepository.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example.Domain;
using Example.Domain.Models;
using Example.Repository.Interface;

namespace Example.Repository
{
    public class UserRepository:Repository<User>,IUserRepository
    {
        public UserRepository(ExampleContext context):base(context)
        {

        }


        public string GetAddressByUserId(int id)
        {
            return "Ax0ne Address";
        }
    }
}
