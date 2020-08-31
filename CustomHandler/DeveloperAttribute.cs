using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.CustomHandler
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple =true)]
    public class DeveloperAttribute : Attribute
    {
        // Private fields.
        private string name;
        private string level;
        private bool reviewed;

        // This constructor defines two required parameters: name and level.

        public DeveloperAttribute(string name, string level)
        {
            this.name = name;
            this.level = level;
            this.reviewed = false;
        }

        // Define Name property.
        // This is a read-only attribute.

        public virtual string Name
        {
            get { return name; }
        }

        // Define Level property.
        // This is a read-only attribute.

        public virtual string Level
        {
            get { return level; }
        }

        // Define Reviewed property.
        // This is a read/write attribute.

        public virtual bool Reviewed
        {
            get { return reviewed; }
            set { reviewed = value; }
        }
    }
}
