namespace Log5.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Common;

    public static class Helpers
    {

        internal static bool Equals<T>(this List<T> list, List<T> other) where T : IEquatable<T>
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


        internal static bool Equals(this object[] array, object[] other)
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


        internal static bool Equals<T>(this Dictionary<string, T> dict, Dictionary<string, T> other) where T : IEquatable<T>
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
                    return false;
                }
            }

            return true;
        }


        internal static bool Equals(this Dictionary<string, object> dict, Dictionary<string, object> other)
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


        public static string AtFormat(string format, IDictionary<string, Json> dict, bool permissive = false)
        {
            var builder = new StringBuilder();

            if (dict == null)
            {
                dict = new Dictionary<string, Json>();
            }

            // x is the last position we have copied over to the string builder
            var x = 0;

            // The position of the formatter, when reading normal text, and when
            // reading a parameter, the position at which that parameter began
            int i;

            // The position of the formatter when reading parameter text
            int j;

            // The position of the end of the parameter name
            int p2;
            
            for (i = 0; i < format.Length; i++)
            {
                if (format[i] == '@')
                {
                    for (j = i + 1; j < format.Length; j++)
                    {
                        if (format[j] != '@')
                        {
                            break;
                        }
                    }

                    // We've reached the end of a chain of at-signs

                    // The number of at-signs
                    var n = j - i;

                    builder.Append(format.Substring(x, i - x));

                    if (n > 1)
                    {
                        builder.Append(new string('@', n >> 1));
                    }

                    if ((n & 1) != 0)
                    {
                        // Get the parameter name
                        var k = j;

                        if (format[k] != '{')
                        {
                            for (; k < format.Length; k++)
                            {
                                switch (format[k])
                                {
                                    case 'a':  case 'b':  case 'c':  case 'd':
                                    case 'e':  case 'f':  case 'g':  case 'h':
                                    case 'i':  case 'j':  case 'k':  case 'l':
                                    case 'm':  case 'n':  case 'o':  case 'p':
                                    case 'q':  case 'r':  case 's':  case 't':
                                    case 'u':  case 'v':  case 'w':  case 'x':
                                    case 'y':  case 'z':  
                                    case 'A':  case 'B':  case 'C':  case 'D':
                                    case 'E':  case 'F':  case 'G':  case 'H':
                                    case 'I':  case 'J':  case 'K':  case 'L':
                                    case 'M':  case 'N':  case 'O':  case 'P':
                                    case 'Q':  case 'R':  case 'S':  case 'T':
                                    case 'U':  case 'V':  case 'W':  case 'X':
                                    case 'Y':  case 'Z':  case '0':  case '1':
                                    case '2':  case '3':  case '4':  case '5':
                                    case '6':  case '7':  case '8':  case '9':
                                        break;

                                    default:
                                        p2 = k;
                                        goto done;
                                }
                            }

                            p2 = k;
                        }
                        else
                        {
                            for (k++; k < format.Length; k++)
                            {
                                switch (format[k])
                                {
                                    case '}':
                                        j++;
                                        p2 = k++;
                                        goto done;

                                    case '{':
                                    {
                                        if (!permissive)
                                        {
                                            throw new FormatException(
                                                String.Format(
                                                    "Parameter group beginning at index {0} not closed properly",
                                                    i
                                                )
                                            );
                                        }

                                        // Just return the rest

                                        var parameter = format.Substring(j, k - j);
                                        builder.Append("@");
                                        builder.Append(parameter);
                                        builder.Append(format.Substring(k));
                                        return builder.ToString();
                                    }
                                }
                            }

                            // End of string while looking for right curly brace

                            if (!permissive)
                            {
                                throw new FormatException(
                                    String.Format(
                                        "Parameter group beginning at index {0} not closed properly",
                                        i
                                    )
                                );
                            }
                            // Just return the rest
                            builder.Append('@');
                            builder.Append(format.Substring(j));
                            return builder.ToString();
                        }

                        done:
                        {
                            var parameter = format.Substring(j, p2 - j);
                            if (!dict.ContainsKey(parameter))
                            {
                                if (!permissive)
                                {
                                    throw new FormatException("No parameter named '" + parameter + "'");
                                }

                                builder.Append('@');
                                builder.Append(parameter);
                            }
                            else
                            {
                                builder.Append(dict[parameter].ToString());
                            }

                            i = k - 1;
                            x = k;
                        }
                    }
                    else
                    {
                        i = j;
                        x = j;
                    }
                }
            }

            builder.Append(format.Substring(x, format.Length - x));

            return builder.ToString();
        }


        internal static string[] ToStringArray(object[] objs)
        {
            if (objs == null)
            {
                return null;
            }

            var strArgs = new string[objs.Length];

            for (var i = 0; i < objs.Length; i++)
            {
                strArgs[i] = objs[i].ToString();
            }

            return strArgs;
        }
    }
}
