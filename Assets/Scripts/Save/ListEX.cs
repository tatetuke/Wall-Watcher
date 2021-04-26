using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Save
{

    public class ListElement
    {
        public System.Type type;
        public object obj;

        public ListElement()
        {
            type = null;
            obj = null;
        }

        public ListElement(object obj_)
        {
            type = obj_.GetType();
            obj = obj_;
        }

        public bool CheckType(System.Type targetType)
        {
            return type == targetType;
        }

        public T GetValue<T>()
        {
            if (!typeof(T).Equals(type))
            {
                Debug.LogWarning($"Cannot cast the type: <color=#5f5>{type}</color> to <color=#5f5>{typeof(T)}</color>");
                return default;
            }

            return (T)obj;
        }

        public void SetValue(object obj_)
        {
            type = obj_.GetType();
            obj = obj_;
        }     
        

    }

    public class ListElement<T> : ListElement
    {
        public ListElement()
        {
            type = null;
            obj = null;
        }

        public ListElement(T obj_)
        {
            type = obj_.GetType();
            obj = obj_;
        }

        public T Value
        {
            get { return (T)obj; }
            set { type = value.GetType();
                obj = value;
            }
        }

        //public T Value
        //{
        //    get { return (T)Convert.ChangeType(obj, type); }
        //    set
        //    {
        //        type = value.GetType();
        //        obj = value;
        //    }
        //}


    }

}
