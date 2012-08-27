namespace Log5
{
    using System;
    using System.Collections.Generic;

    internal static class Helpers
    {

        public static bool Equals<T>(this List<T> list, List<T> other) where T : IEquatable<T>
        {
            if (list.Count != other.Count)
            {
                return false;
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (!list[i].Equals(other[i]))
                {
                    return false;
                }
            }

            return true;
        }


        public static bool Equals(this object[] array, object[] other)
        {
            if (array.Length != other.Length)
            {
                return false;
            }

            for (var i = 0; i < array.Length; i++)
            {
                if (!array[i].Equals(other[i]))
                {
                    return false;
                }
            }

            return true;
        }



        public static bool Equals(this Dictionary<string, object> dict, Dictionary<string, object> other)
        {
            if (dict.Count != other.Count)
            {
                return false;
            }

            var enum1 = dict.GetEnumerator();
            var enum2 = other.GetEnumerator();

            while (enum1.MoveNext() && enum2.MoveNext())
            {
                if (enum1.Current.Key != enum2.Current.Key)
                {
                    return false;
                }

                if (!enum1.Current.Value.Equals(enum2.Current.Value))
                {
                    // Check, just to be sure

                    if (enum1.Current.Value is Int32)
                    {
                        var v1 = (long) ((int) enum1.Current.Value);
                        var v2 = (long) enum2.Current.Value;

                        return v1 == v2;
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
