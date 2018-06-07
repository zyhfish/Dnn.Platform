#region Copyright
// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2018
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion
#region Usings

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

#endregion

namespace DotNetNuke.Collections.Internal
{
    [Serializable]
    public class SharedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private ConcurrentDictionary<TKey, TValue> _dict;

        private bool _isDisposed;
        private ILockStrategy _lockController;

        public SharedDictionary() : this(LockingStrategy.ReaderWriter)
        {
        }

        public SharedDictionary(ILockStrategy lockStrategy)
        {
            _dict = new ConcurrentDictionary<TKey, TValue>();
            _lockController = lockStrategy;
        }

        public SharedDictionary(LockingStrategy strategy) : this(LockingStrategyFactory.Create(strategy))
        {
        }

        internal IDictionary<TKey, TValue> BackingDictionary
        {
            get
            {
                return _dict;
            }
        }

        #region IDictionary<TKey,TValue> Members

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return IEnumerable_GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return IEnumerable_GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dict.TryAdd(item.Key, item.Value);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            
            
            return _dict.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            
            
            _dict.ToArray().CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            return _dict.TryRemove(item.Key, out value);
        }

        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                /**
                 * Kept only for compatibility
                 * even if it was uselss.
                 * Actually we don't have control over 
                 * readability of the _dict instance, since
                 * the Dictionary class is instatiated in the 
                 * constructor as ReadWrite.
                 * Actually it is alwyas false
                 */
                return false; // _dict.IsReadOnly;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            _dict.TryAdd(key, value);
        }

        public bool Remove(TKey key)
        {
            TValue value;
            return _dict.TryRemove(key, out value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                _dict.TryGetValue(key, out value);
                return value;
            }
            set
            {
                _dict.TryAdd(key, value);
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return _dict.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return _dict.Values;
            }
        }

        #endregion
        
        public ISharedCollectionLock GetReadLock()
        {
            return GetReadLock(TimeSpan.FromMilliseconds(-1));
        }

        public ISharedCollectionLock GetReadLock(TimeSpan timeOut)
        {
            
            return _lockController.GetReadLock(timeOut);
        }

        public ISharedCollectionLock GetReadLock(int millisecondTimeout)
        {
            return GetReadLock(TimeSpan.FromMilliseconds(millisecondTimeout));
        }

        public ISharedCollectionLock GetWriteLock()
        {
            return GetWriteLock(TimeSpan.FromMilliseconds(-1));
        }

        public ISharedCollectionLock GetWriteLock(TimeSpan timeOut)
        {
            
            return _lockController.GetWriteLock(timeOut);
        }

        public ISharedCollectionLock GetWriteLock(int millisecondTimeout)
        {
            return GetWriteLock(TimeSpan.FromMilliseconds(millisecondTimeout));
        }
        
        public IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable_GetEnumerator()
        {
            //todo nothing ensures read lock is held for life of enumerator
            return _dict.GetEnumerator();
        }

    }
}