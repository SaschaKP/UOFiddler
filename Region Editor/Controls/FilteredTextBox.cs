using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace Region_Editor
{
    public class FilteredTextBox : TextBox
    {
        private string _AllowedChars = "";

        [Description("Controls which characters are allowed to be entered in the edit control. Supercedes the DeniedChars list."), Category("Special"), DefaultValue("")]
        public string AllowedChars { get { return _AllowedChars; } set { _AllowedChars = value; } }

        protected override void OnTextChanged(EventArgs e)
        {
            // Perform standard functions
            base.OnTextChanged(e);

            if (_AllowedChars == "")
                return;

            List<char> invalid = new List<char>();

            // Determine if invalid characters are present
            foreach (char c in Text)
                if (_AllowedChars.IndexOf(c) < 0 && !invalid.Contains(c))
                    invalid.Add(c);

            StringBuilder sb = new StringBuilder(Text);

            // Filter out the invalid characters from the text
            foreach (char c in invalid)
                sb.Replace(c.ToString(), "");

            Text = sb.ToString();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            // Perform standard functions
            base.OnKeyPress(e);

            if (e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else if (_AllowedChars != "")
            {
                e.Handled = true;

                // If the key pressed is in the allowed list, then process the keypress
                foreach (char c in _AllowedChars)
                    if (e.KeyChar == c)
                        e.Handled = false;
            }
        }
    }
}