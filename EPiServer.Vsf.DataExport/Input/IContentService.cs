using System.Collections.Generic;
using System.Globalization;
using EPiServer.Core;
using EPiServer.Vsf.DataExport.Input.Model;

namespace EPiServer.Vsf.DataExport.Input
{
    public interface IContentService<T> where T : ICmsObject
    {
        IEnumerable<T> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level = 2); 
    }
}