using System.Collections.Generic;
using System.Text;

namespace JsonChecker
{
    using Extensions;
    using System.Linq;

    public class NameRegister
    {
        private int length = 0;
        private readonly List<string> register = new();


        public NameRegister()
        {
        }

        public NameRegister(string value)
        {
            this.Regist(value);
        }

        public NameRegister(NameRegister other)
        {
            this.length = other.length;
            this.register.AddRange(other.register);
        }

        public NameRegister(NameRegister other, string value) : this(other)
        {
            this.Regist(value);
        }

        public void Regist(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            this.register.Add(value);
            this.length += value.Length;
        }

        public string Combine(string name, char separator)
        {
            if (this.register.Count == 0)
            {
                return name;
            }

            name = string.IsNullOrEmpty(name) ? this.register.Last() : name;
            var builder = new StringBuilder(this.length + this.register.Count + name.Length);

            foreach (var e in this.register)
            {
                builder.Append(e).Append(separator);
            }

            builder.Append(name);
            return builder.ToString();
        }
    }
}
