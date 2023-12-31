using System.Collections.Generic;
using System.Text;

namespace JsonChecker
{
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

            var isShift = string.IsNullOrEmpty(name);
            name = isShift ? this.register.Last() : name;
            var builder = new StringBuilder(this.length + this.register.Count + name.Length);

            for (var i = 0; i < this.register.Count - (isShift ? 1 : 0); i++)
            {
                builder.Append(this.register[i]).Append(separator);
            }

            builder.Append(name);
            return builder.ToString();
        }
    }
}
