using System.Collections.Generic;
using System.Linq;

namespace MarkdownMerge.Commands
{
    public abstract class StringPart
    {
        private List<StringPart> parts;

        public string RealString { get; private set; }

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

        public string GetBeforeString()
        {
            var before = Before;
            if (before != null && before is RealStringPart)
            {
                return before.ToString();
            }

            return null;
        }

        public string GetNextString()
        {
            var next = Next;
            if (next != null && next is RealStringPart)
            {
                return next.ToString();
            }

            return null;
        }

        public abstract void Process();

        public override string ToString()
        {
            return this.RealString;
        }
    }
}
