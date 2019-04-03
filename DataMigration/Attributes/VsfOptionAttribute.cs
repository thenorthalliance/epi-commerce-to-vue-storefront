using System;

namespace DataMigration.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class VsfOptionAttribute : Attribute
    {
    }
}
