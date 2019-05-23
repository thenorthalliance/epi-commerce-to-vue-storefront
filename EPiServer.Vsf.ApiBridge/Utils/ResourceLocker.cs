using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EPiServer.Vsf.ApiBridge.Utils
{
    public class UserLocker : ResourceLocker<string>
    {
        public UserLocker(string resourceId) : base(resourceId)
        {}
    }

    public class CartLocker : ResourceLocker<Guid>
    {
        public CartLocker(Guid contact) : base(contact)
        { }
    }
    public class ResourceLocker<T> : IDisposable
    {
        public class ResourceSemaphore
        {
            private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);

            private int _threadsWaiting;

            public Task Acquire()
            {
                _threadsWaiting++;
                return _semaphoreSlim.WaitAsync();
            }

            public void Release()
            {
                _semaphoreSlim.Release();
                _threadsWaiting--;
            }

            public bool IsFree()
            {
                return _threadsWaiting == 0;
            }
        }

        private static readonly object locker = new object();
        private static readonly Dictionary<T, ResourceSemaphore> Semaphores = new Dictionary<T, ResourceSemaphore>();

        private readonly Task _semapthoreTask;
        private readonly T _resourceId;

        public ResourceLocker(T resourceId)
        {
            _resourceId = resourceId;
            lock (locker)
            {
                if (!Semaphores.ContainsKey(_resourceId))
                    Semaphores.Add(_resourceId, new ResourceSemaphore());

                _semapthoreTask = Semaphores[_resourceId].Acquire();
            }
        }

        public void Dispose()
        {
            lock (locker)
            {
                var semaphoreResurce = Semaphores[_resourceId];
                semaphoreResurce.Release();
                if (semaphoreResurce.IsFree())
                {
                    Semaphores.Remove(_resourceId);
                }
            }
        }

        public async Task WaitAsync()
        {
            await _semapthoreTask;
        }

        public static async Task<ResourceLocker<T>> LockAsync(T resourceId)
        {
            var resourceLocker = new ResourceLocker<T>(resourceId);
            await resourceLocker.WaitAsync();
            return resourceLocker;
        }

        public static ResourceLocker<T> Lock(T resourceId)
        {
            return Task.Run(() => LockAsync(resourceId)).Result;
        }
    }
}