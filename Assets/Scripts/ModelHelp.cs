using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataBase
{
    public class ModelHelp : Attribute
    {
       public bool IsCreated { get; set; }
        public string FieldName { get; set; }
        public string Type { get; set; }
        public bool IsPrimaryKey { get; set; }

        public bool IsCanbeNull { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isCreated"></param>
        /// <param name="fieldName">Name in the table</param>
        /// <param name="type">type of the parameter</param>
        /// <param name="isPrimaryKey">if is primary</param>
        /// <param name="isCanbeNull">Can be Null or not</param>
        public ModelHelp(bool isCreated, string fieldName, string type,bool isPrimaryKey=false,bool isCanbeNull=true)
        {
            IsCreated = isCreated;
            FieldName = fieldName;
            Type = type;
            IsPrimaryKey = isPrimaryKey;
            IsCanbeNull = isCanbeNull;
        }
    }
}

