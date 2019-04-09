using System.Collections.Generic;

namespace EPiServer.Vsf.DataExport.Input.Model
{
    public class EpiContentProperty : ICmsObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Values { get; set; }
    }
}