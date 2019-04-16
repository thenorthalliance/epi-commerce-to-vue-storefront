using System.Collections.Generic;

namespace EPiServer.Vsf.Core.Exporting
{
    public interface IIndexingService
    {
        bool IsServiceAvailable();
        void CreateIndex();

        void AddForIndexing<T>(IEnumerable<T> items) where T : class;
        void AddForIndexing<T>(T items) where T : class;
        void ApplyChanges();
    }
}