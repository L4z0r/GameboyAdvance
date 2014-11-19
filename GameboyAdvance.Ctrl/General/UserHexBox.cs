using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	[DesignTimeVisible(false)]
	public class UserHexBox : TextBox
	{
		private bool mouseHover;
		public new bool MouseHover
		{
			get
			{
				return this.mouseHover;
			}
		}
		public UserHexBox()
		{
			this.Font = new Font("Segoe UI", 8.25f);
			base.CharacterCasing = CharacterCasing.Upper;
			base.BorderStyle = BorderStyle.None;
			this.Multiline = true;
			base.Height = 16;
			this.MaxLength = 7;
			base.Location = new Point(26, 2);
		}
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			char keyChar = e.KeyChar;
			if (keyChar != '\b' && !Uri.IsHexDigit(keyChar))
			{
				e.Handled = true;
				return;
			}
			e.Handled = false;
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.mouseHover = true;
			base.Parent.Invalidate();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.mouseHover = false;
			base.Parent.Invalidate();
		}
	}
}
