namespace Log5
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Internal;

    using Newtonsoft.Json;


    /// <summary>
    /// Instances of this class represent a collection of tags.
    /// </summary>
    /// <remarks>
    /// The interface is the same as that of the DOMTokenList found here:
    /// http://dvcs.w3.org/hg/domcore/raw-file/tip/Overview.html#domtokenlist
    /// </remarks>
    [Serializable]
    public class TagList : IEquatable<TagList>, IList<string>
    {

        /// <summary>
        /// This regular expression is used to argument check tags, and also
        /// used to get a list of tags from a single string
        /// </summary>
        private readonly static Regex ReWhitespace = new Regex(@"\s+", RegexOptions.Compiled);


        // These two fields represent the state of the Tag List. Either field
        // could be used to uniquely identify the Tag List, but both exist to
        // speed up operations

        #region Internal State

        [JsonIgnore]
        private readonly Dictionary<string, int> _dict;

        private readonly List<string> _list;

        #endregion


        // An empty constructor, and two initializing constructors which
        // load the TagList from either an enumerable list of strings, or
        // a string of whitespace-separated tags (like an HTML class value)

        #region Constructors

        /// <summary>
        /// Create a tag list
        /// </summary>
        public TagList()
        {
            _dict = new Dictionary<string, int>();
            _list = new List<string>();
        }


        /// <summary>
        /// Create a tag list, loading it from a collection of strings
        /// </summary>
        /// <param name="tags">An enumerable collection of tags</param>
        public TagList(IEnumerable<string> tags)
            : this()
        {
            AddMultiple(tags);
        }


        /// <summary>
        /// Create a tag list, initializing it from a whitespace-separated
        /// string list of tags
        /// </summary>
        /// <param name="tags">A string containing tags separated by whitespace</param>
        public TagList(string tags) : this()
        {
            AddMultiple(tags);
        }

        #endregion


        /// <summary>
        /// Returns the number of tags
        /// </summary>
        public long Length { get { return _list.Count; } }



        #region Implementation of IList<string>

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        int IList<string>.IndexOf(string item)
        {
            if (!_dict.ContainsKey(item))
            {
                return -1;
            }

            return _dict[item];
        }


        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        void IList<string>.Insert(int index, string item)
        {
            if (index < 0 || index >= _list.Count)
            {
                throw new ArgumentException("Index is out of bounds", "index");
            }

            _list.Insert(index, item);

            for (var i = index + 1; i < _list.Count; i++)
            {
                var sibling = _list[i];
                _dict[sibling]++;
            }
        }


        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        void IList<string>.RemoveAt(int index)
        {
            if (index < 0 || index >= _list.Count)
            {
                throw new ArgumentException("Index is out of bounds", "index");
            }

            var item = _list[index];
            _list.RemoveAt(index);
            _dict.Remove(item);
        }


        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <summary>
        /// Returns the tag with index <paramref name="index"/>
        /// </summary>
        /// <param name="index">The zero-based index of the tag to get</param>
        /// <returns>The requested tag</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is out of bounds</exception>
        public string this[int index]
        {
            get { return _list[index]; }
            set
            {
                if (index < 0 || index >= _list.Count)
                {
                    throw new ArgumentException("Index is out of bounds", "index");
                }

                var oldItem = _list[index];
                _dict.Remove(oldItem);
                _list[index] = value;
                _dict[value] = index;
            }
        }


        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        public void Clear()
        {
            _list.Clear();
            _dict.Clear();
        }


        /// <summary>
        /// Returns <code>true</code> if the <paramref name="tag"/> is present; <code>false</code> otherwise
        /// </summary>
        /// <param name="tag">The tag to request</param>
        /// <returns><code>true</code> if <paramref name="tag"/> is present; <code>false</code> otherwise</returns>
        /// <remarks>
        /// Throws an <code>ArgumentException</code> if <paramref name="tag"/> is empty or contains
        /// any whitespace characters
        /// </remarks>
        public bool Contains(string tag)
        {
            ArgumentCheck(tag);
            return _dict.ContainsKey(tag);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination
        /// of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The
        /// <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The
        /// zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        public void CopyTo(string[] array, int arrayIndex)
        {
            if (arrayIndex < 0 || arrayIndex + _list.Count > array.Length)
            {
                throw new ArgumentException("arrayIndex is out of bounds", "arrayIndex");
            }

            for (int i = 0, j = arrayIndex; i < _list.Count; i++)
            {
                array[j] = _list[i];
            }
        }


        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get
            {
                return _list.Count;
            }
        }


        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// Adds <paramref name="tag"/> unless it is already present
        /// </summary>
        /// <param name="tag">The tag to add</param>
        /// <remarks>
        /// Throws an <code>ArgumentException</code> if <paramref name="tag"/> is empty or contains
        /// any whitespace characters
        /// </remarks>
        public void Add(string tag)
        {
            ArgumentCheck(tag);

            if (_dict.ContainsKey(tag))
            {
                return;
            }

            _dict[tag] = _list.Count;
            _list.Add(tag);
        }

        #endregion



        /// <summary>
        /// Adds a number of tags from a single string of whitespace-separated tags
        /// </summary>
        /// <param name="tags">A whitespace-separated string of tags</param>
        /// <remarks>
        /// Throws an <code>ArgumentException</code> if <paramref name="tags"/> is null or empty
        /// </remarks>
        public void AddMultiple(string tags)
        {
            if (String.IsNullOrEmpty(tags))
            {
                throw new ArgumentException("Tags cannot be null or empty", "tags");
            }

            var tagList = ReWhitespace.Split(tags);
            foreach (var tag in tagList)
            {
                // There is no need to argument check these, so use
                // the raw adding code
                _dict[tag] = _list.Count;
                _list.Add(tag);
            }
        }


        /// <summary>
        /// Adds a number of tags from a list of strings
        /// </summary>
        /// <param name="tags">A list of tag strings</param>
        /// <remarks>
        /// Throws an <code>ArgumentException</code> if any of the strings in <paramref name="tags"/>
        /// are null or empty
        /// </remarks>
        public void AddMultiple(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                ArgumentCheck(tag);
                InternalAdd(tag);
            }
        }


        /// <summary>
        /// Removes a number of tags from a single string of whitespace-separated tags
        /// </summary>
        /// <param name="tags">A whitespace-separated string of tags</param>
        /// <remarks>
        /// Throws an <code>ArgumentException</code> if <paramref name="tags"/> is null or empty
        /// </remarks>
        public void RemoveMultiple(string tags)
        {
            if (String.IsNullOrEmpty(tags))
            {
                throw new ArgumentException("Tags cannot be null or empty", "tags");
            }

            var tagList = ReWhitespace.Split(tags);
            foreach (var tag in tagList)
            {
                // There is no need to argument check these, so use
                // the raw removal code
                InternalRemove(tag);
            }
        }


        /// <summary>
        /// Adds a number of tags from a list of strings
        /// </summary>
        /// <param name="tags">A list of tag strings</param>
        /// <remarks>
        /// Throws an <code>ArgumentException</code> if any of the strings in <paramref name="tags"/>
        /// are null or empty
        /// </remarks>
        public void RemoveMultiple(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                ArgumentCheck(tag);
                InternalRemove(tag);
            }
        }



        /// <summary>
        /// Removes <paramref name="tag"/> if it is present
        /// </summary>
        /// <param name="tag">The tag to remove</param>
        /// <remarks>
        /// Throws an <code>ArgumentException</code> if <paramref name="tag"/> is empty or contains
        /// any whitespace characters
        /// </remarks>
        public bool Remove(string tag)
        {
            ArgumentCheck(tag);
            return InternalRemove(tag);
        }


        /// <summary>
        /// Adds <paramref name="tag"/> if it is not present, or removes it if it is. Returns
        /// <code>true</code> if <paramref name="tag"/> is now present (it was added);
        /// returns <code>false</code> if it is not (it was removed).
        /// </summary>
        /// <param name="tag">The tag to toggle</param>
        /// <remarks>
        /// Throws an <code>ArgumentException</code> if <paramref name="tag"/> is empty or contains
        /// any whitespace characters
        /// </remarks>
        public bool Toggle(string tag)
        {
            ArgumentCheck(tag);
            if (Contains(tag))
            {
                InternalRemove(tag);
                return false;
            }

            InternalAdd(tag);
            return true;
        }


        // These functions are used to add tags, remove tags, and
        // check arguments

        #region Internal Functions

        /// <summary>
        /// Adds <paramref name="tag"/> if it is present
        /// </summary>
        /// <param name="tag">The tag to add</param>
        private void InternalAdd(string tag)
        {
            if (_dict.ContainsKey(tag))
            {
                return;
            }

            _dict[tag] = _list.Count;
            _list.Add(tag);
        }


        /// <summary>
        /// Removes <paramref name="tag"/> if it is present
        /// </summary>
        /// <param name="tag">The tag to remove</param>
        private bool InternalRemove(string tag)
        {
            if (!_dict.ContainsKey(tag))
            {
                return false;
            }

            var index = _dict[tag];
            _dict.Remove(tag);

            while (_list[index] != tag)
            {
                index--;
            }

            _list.RemoveAt(index);

            return true;
        }


        /// <summary>
        /// Checks that the tag is valid. It must not be null, empty, or
        /// contain any whitespace characters. If the tag is invalid, this
        /// method throws an <code>ArgumentException</code>
        /// </summary>
        /// <param name="tag">The tag to check</param>
        private static void ArgumentCheck(string tag)
        {
            if (String.IsNullOrEmpty(tag))
            {
                throw new ArgumentException("Tag cannot be null or empty", "tag");
            }

            if (ReWhitespace.IsMatch(tag))
            {
                throw new ArgumentException("Tag cannot contain whitespace", "tag");
            }
        }

        #endregion


        #region Implementation of IEquatable<TagList>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(TagList other)
        {
            return other != null && Helpers.Equals(_list, other._list);
        }

        #endregion


        #region Implementation of IEnumerable<string>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<string> GetEnumerator()
        {
            foreach (var tag in _list)
            {
                yield return tag;
            }
        }


        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


        #region Overrides of Object

        /// <summary>
        /// Determines whether the specified <code>System.Object</code> is equal to the current
        /// <code>System.Object</code>
        /// </summary>
        /// <param name="obj">The <code>Object</code> to compare with the current <code>Object</code></param>
        /// <returns><code>true</code> if the specified object is equal to the current object;
        /// otherwise <code>false</code></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TagList);
        }


        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            return String.Join(" ", _list);
        }


        /// <summary>
        /// Serves as a hash function for a particular type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion
    }
}
