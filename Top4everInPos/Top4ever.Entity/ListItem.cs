using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Entity
{
    public class ListItem
    {
        private string text = string.Empty;
        private string value = string.Empty;

        public ListItem(string text, string value)
        {
            this.text = text;
            this.value = value;
        }

        public override string ToString()
        {
            return this.text;
        }

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}
