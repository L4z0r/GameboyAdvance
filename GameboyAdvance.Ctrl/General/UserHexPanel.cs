using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserHexPanel : UserControl
	{
		public bool isToolItem;
		private UserHexBox hexBox;
		private bool mouseHover;
		[Browsable(false)]
		public UserHexBox HexBox
		{
			get
			{
				base.Invalidate();
				return this.hexBox;
			}
		}
		public UserHexPanel()
		{
			this.DoubleBuffered = true;
			this.BackColor = Color.White;
			this.hexBox = new UserHexBox();
			this.hexBox.Visible = true;
			this.hexBox.Width = base.Width - base.Width / 2 -2;
            this.hexBox.Height = 14;
			base.Controls.Add(this.hexBox);
			base.Height = 20;
			this.isToolItem = false;
		}
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			Brush brush;
			if (this.isToolItem)
			{
				brush = new LinearGradientBrush(new Rectangle(base.Parent.ClientRectangle.X, base.Parent.ClientRectangle.Y, base.Parent.ClientRectangle.Width - 1, base.Parent.ClientRectangle.Height - 2), UserMenuColors.clrVerBG_White, UserMenuColors.clrVerBG_GrayBlue, LinearGradientMode.Vertical);
			}
			else
			{
				brush = new SolidBrush(base.Parent.BackColor);
			}
			e.Graphics.FillRectangle(brush, base.ClientRectangle);
			using (Brush brush2 = new SolidBrush(base.Enabled ? this.BackColor : SystemColors.Control))
			{
				e.Graphics.FillRectangle(brush2, 1, 1, base.Width - 3, base.Height - 3);
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.mouseHover || this.hexBox.MouseHover)
			{
				UserMenuColors.DrawRoundedRectangle(e.Graphics, 0, 0, base.Width - 1, base.Height - 2, 4, UserMenuColors.clrSelectedBG_Drop_Blue);
			}
			else
			{
				UserMenuColors.DrawRoundedRectangle(e.Graphics, 0, 0, base.Width - 1, base.Height - 2, 4, UserMenuColors.clrMenuBorder);
			}
			using (Pen pen = new Pen((this.mouseHover || this.hexBox.MouseHover) ? UserMenuColors.clrSelectedBG_Drop_Blue : UserMenuColors.clrMenuBorder))
			{
				e.Graphics.DrawLine(pen, 21, 0, 21, base.Height - 2);
			}

            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            using (Brush b = new SolidBrush(Color.Black))
			    e.Graphics.DrawString("0x", hexBox.Font, b, 2f, 1f);
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.mouseHover = true;
			base.Invalidate();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.mouseHover = false;
			base.Invalidate();
		}
	}
}
