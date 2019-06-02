﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.Model
{
    public class DbRow : IId
    {
        long id;
        //long parentId;

        public DbRow(Int64 id)
        {
            Id = id;

        }

        public DbRow()
        {
        }

        [PrimaryKey, AutoIncrement]
        public Int64 Id { get => id; set => id = value; }

    }




}