using System.Collections.Generic;
using System.Globalization;
using DataMigration.Input.Model;
using EPiServer.Core;

namespace DataMigration.Input
{
    public interface IContentService<T> where T : ICmsObject
    {
        IEnumerable<T> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level = 2); 
    }
}