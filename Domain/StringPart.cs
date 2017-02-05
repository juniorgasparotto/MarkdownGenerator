using System.Collections.Generic;
using System.Linq;

namespace MarkdownMerge.Commands
{
    public abstract class StringPart
    {
        private List<StringPart> parts;

        public string RealString { get; private set; }
        public string NewString { get; set; }

        public StringPart Before
        {
            get
            {
                return this.parts.ElementAtOrDefault(parts.IndexOf(this) - 1);
            }
        }

        public StringPart Next
        {
            get
            {
                return this.parts.ElementAtOrDefault(parts.IndexOf(this) + 1);
            }
        }

        public StringPart(List<StringPart> parts, string realString)
        {
            this.parts = parts;
            this.parts.Add(this);

            this.RealString = realString;
        }

        public RealStringPart GetBeforeString()
        {
            var before = Before;
            if (before != null && before is RealStringPart)
            {
                return (RealStringPart)before;
            }

            return null;
        }

        public RealStringPart GetNextString()
        {
            var next = Next;
            if (next != null && next is RealStringPart)
            {
                return (RealStringPart)next;
            }

            return null;
        }

        public abstract void Process();

        public override string ToString()
        {
            return NewString ?? RealString;
        }
    }
}
