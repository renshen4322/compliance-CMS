using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SEACompliance.Core.Common
{
    public static class Singleton<T>
    {
        private static object LockKey = new object();

        /// <summary>
        /// Get an instance of T
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            return GetInstance(null);
        }

        /// <summary>
        /// Get an instance of T
        /// </summary>
        /// <param name="onCreateInstance"></param>
        /// <returns></returns>
        public static T GetInstance(CreateInstanceHandler<T> onCreateInstance)
        {
            if (_Instance == null)
            {
				
                lock (LockKey)
                {
                    if (_Instance == null)
                    {
                        try
                        {
                            if (onCreateInstance == null)
                                _Instance = Activator.CreateInstance<T>();
                            else
                                _Instance = onCreateInstance();
                        }
                        catch
                        {
                            _Instance = default(T);
                        }
                    }
                }
            }
            return _Instance;
        }
        private static T _Instance;


        /// <summary>
        /// Get an instance of T and set to instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="onCreateInstance"></param>
        /// <returns></returns>
        public static T GetInstance(object lockKey, ref T instance, CreateInstanceHandler<T> onCreateInstance)
        {
            if (instance == null)
            {
                if (lockKey == null)
                    lockKey = LockKey;
                lock (lockKey)
                {
                    if (instance == null)
                    {
                        try
                        {
                            if (onCreateInstance == null)
                                instance = Activator.CreateInstance<T>();
                            else
                                instance = onCreateInstance();
                        }
                        catch
                        {
                            instance = default(T);
                        }
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// Release the instance of T
        /// </summary>
        public static void ReleaseInstance()
        {
            lock (LockKey)
            {
                IDisposable id = _Instance as IDisposable;
                if (id != null)
                    id.Dispose();

                _Instance = default(T);
            }
        }

        /// <summary>
        /// Get an instance of T for current thread
        /// </summary>
        /// <param name="appendedkey"></param>
        /// <param name="onCreateInstance"></param>
        /// <returns></returns>
        public static T GetThreadInstance(string appendedkey, CreateInstanceHandler<T> onCreateInstance)
        {
            LocalDataStoreSlot slot = Thread.GetNamedDataSlot("__ThreadInstanceTable");
            Dictionary<string, object> instances = Thread.GetData(slot) as Dictionary<string, object>;
            if (instances == null)
            {
                lock (LockKey)
                {
                    instances = Thread.GetData(slot) as Dictionary<string, object>;
                    if (instances == null)
                    {
                        instances = new Dictionary<string, object>();
                        Thread.SetData(slot, instances);
                    }
                }
            }

            string key = string.Format("{0}:{1}", typeof(T).AssemblyQualifiedName, appendedkey);
            object obj;
            if (instances.TryGetValue(key, out obj))
            {
                return (T)obj;
            }
            else
            {
                lock (LockKey)
                {
                    if (instances.TryGetValue(key, out obj))
                    {
                        return (T)obj;
                    }
                    else
                    {
                        T t;
                        try
                        {
                            t = (onCreateInstance == null ? Activator.CreateInstance<T>() : onCreateInstance());
                        }
                        catch
                        {
                            t = default(T);
                        }
                        instances[key] = t;
                        return t;
                    }
                }
            }
        }

        /// <summary>
        /// Get an instance of T for current thread
        /// </summary>
        /// <param name="appendedkey"></param>
        /// <returns></returns>
        public static T GetThreadInstance(string appendedkey)
        {
            return GetThreadInstance(appendedkey, null);
        }

        /// <summary>
        /// Get an instance of T for current thread
        /// </summary>
        /// <returns></returns>
        public static T GetThreadInstance()
        {
            return GetThreadInstance(null, null);
        }

        /// <summary>
        /// Release the instance of T for current thread
        /// </summary>
        public static void ReleaseThreadInstance()
        {
            LocalDataStoreSlot slot = Thread.GetNamedDataSlot("__ThreadInstanceTable");
            lock (LockKey)
            {
                Dictionary<int, object> instances = Thread.GetData(slot) as Dictionary<int, object>;
                if (instances != null)
                {
                    int threadId = Thread.CurrentThread.ManagedThreadId;
                    instances.Remove(threadId);
                }
            }
        }
    }

    public delegate T CreateInstanceHandler<T>();
}
