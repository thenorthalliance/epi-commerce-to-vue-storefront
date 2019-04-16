using System.Collections.Generic;

namespace EPiServer.Vsf.DataExport.Model
{
    public class EpiContentProperty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; }
    }
}