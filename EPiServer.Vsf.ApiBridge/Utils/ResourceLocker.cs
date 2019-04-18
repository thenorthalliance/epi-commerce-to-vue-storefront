using System;
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

            public int ThreadsWaiting { get; private set; }


            public Task Acquire()
            {
                ThreadsWaiting++;
                return _semaphoreSlim.WaitAsync();
            }

            public void Release()
            {
                _semaphoreSlim.Release();
                ThreadsWaiting--;
            }
        }
        
        private static readonly object Locker = new object();
        private static readonly Dictionary<T, ResourceSemaphore> Semaphores = new Dictionary<T, ResourceSemaphore>();

        private readonly Task _semapthoreTask;
        private readonly T _resourceId;

        public ResourceLocker(T resourceId)
        {
            _resourceId = resourceId;
            lock (Locker)
            {
                if (!Semaphores.ContainsKey(_resourceId))
                    Semaphores.Add(_resourceId, new ResourceSemaphore());

                _semapthoreTask = Semaphores[_resourceId].Acquire();
            }
        }

        public void Dispose()
        {
            lock (Locker)
            {
                Semaphores[_resourceId].Release();
                if (Semaphores[_resourceId].ThreadsWaiting == 0)
                    Semaphores.Remove(_resourceId);
            }
        }

        public async Task Wait()
        {
            await _semapthoreTask;
        }

        public static async Task<ResourceLocker<T>> LockAsync(T resourceId)
        {
            var resourceLocker = new ResourceLocker<T>(resourceId);
            await resourceLocker.Wait();
            return resourceLocker;
        }

        public static ResourceLocker<T> Lock(T resourceId)
        {
            return LockAsync(resourceId).Result;
        }
    }
}