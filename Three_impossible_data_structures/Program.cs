using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;


namespace Three_impossible_data_structures
{

    public class Program
    {
        public static bool Compare(object obj, object another)
        {
            if (ReferenceEquals(obj, another))
                return true;
            if ((obj == null) || (another == null))
                return false;
            if (obj.GetType() != another.GetType())
                return false;
            if (!obj.GetType().IsClass)
                return obj.Equals(another);

            var objJson = JsonConvert.SerializeObject(obj);
            var anotherJson = JsonConvert.SerializeObject(another);

            return objJson == anotherJson;
        }

        public class Floor
        {
            public Object Item
            {
                get;
                set;
            }

            public int Index
            {
                get;
                set;
            }
        }

        public class Tower
        {
            private ArrayList _list = new ArrayList();
            private int _index = 0;
            public void Add(Object val)
            {
                _list.Add(new Floor()
                { Item = val, Index = _index });
                _index++;
            }

            public void Reset(int index = 0)
            {
                _index = index;
                foreach (Floor f in _list)
                {
                    f.Index = _index;
                    _index++;
                }
            }

            public ArrayList Items()
            {
                return _list;
            }

            public int Count()
            {
                return _list.Count;
            }

            public bool Remove(Object val)
            {
                var result = false;
                var found = (
                    from Floor f in _list
                    where Compare(f.Item, val)
                    select f).FirstOrDefault();
                if (found != null)
                {
                    _list.Remove(found);
                    Reset();
                    result = true;
                }

                return result;
            }

            public bool HasValue(Object val)
            {
                return (
                    from Floor f in _list
                    where Compare(f.Item, val) == true
                    select true).DefaultIfEmpty(false).FirstOrDefault();
            }

            public bool HasIndex(int index)
            {
                return (
                    from Floor f in _list
                    where f.Index == index
                    select true).DefaultIfEmpty(false).FirstOrDefault();
            }

            private void _add(int index, Object item)
            {
                if (index == 0)
                {
                    _list.Insert(0, new Floor()
                    { Item = item, Index = 1 });
                }
                else if (index == _list.Count)
                {
                    Reset(1);
                    _list.Insert(0, new Floor()
                    { Item = item, Index = 0 });
                }
                else
                {
                    Add(item);
                }
            }

            public int? GetIndex(Object val)
            {
                int? index = -1;
                Floor found = (
                    from Floor f in _list
                    where Compare(f.Item, val)
                    select f).FirstOrDefault();
                if (found == null)
                    return null;
                if (Remove(found.Item))
                {
                    _add(found.Index, found.Item);
                    index = (
                        from Floor f in _list
                        where Compare(f, found)
                        select f.Index).DefaultIfEmpty(-1).FirstOrDefault();
                    return (index == -1 ? null : index);
                }

                return null;
            }

            public Object GetValue(int index)
            {
                Object val = null;
                Floor found = (
                    from Floor f in _list
                    where f.Index == index
                    select f).FirstOrDefault();
                if (Remove(found.Item))
                {
                    _add(found.Index, found.Item);
                    val = (
                        from Floor f in _list
                        where Compare(f.Item, found)
                        select f.Item).DefaultIfEmpty(null).FirstOrDefault();
                }

                return val;
            }
        }

        public static void Main()
        {
            var t = new Tower();
            t.Add("stringa");
            //t.Add(2);
            //t.Add(true);
            t.Remove(2);
            Console.WriteLine("Value exists: " + t.HasValue("stringa"));
            Console.WriteLine("Value index: " + t.GetIndex("stringa"));
            Console.WriteLine(JsonConvert.SerializeObject(t.Items()));
            Console.ReadKey(true);
        }
    }
}
