using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserGroupBox : GroupBox
	{
		private Color borderColor;
		private Color shadowColor;
		private TextAlignment textAlign;
		[Browsable(true), Category("Content")]
		public Color BorderColor
		{
			get
			{
				return this.borderColor;
			}
			set
			{
				this.borderColor = value;
				base.Invalidate();
			}
		}
		[Browsable(true), Category("Content")]
		public Color ShadowColor
		{
			get
			{
				return this.shadowColor;
			}
			set
			{
				this.shadowColor = value;
				base.Invalidate();
			}
		}
		[Browsable(true), Category("Content")]
		public TextAlignment TextAlign
		{
			get
			{
				return this.textAlign;
			}
			set
			{
				this.textAlign = value;
				base.Invalidate();
			}
		}
		public UserGroupBox()
		{
			this.DoubleBuffered = true;
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.borderColor = Color.FromArgb(160, 160, 160);
			this.shadowColor = Color.FromArgb(248, 248, 248);
			this.textAlign = TextAlignment.Left;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			Size size = TextRenderer.MeasureText(" " + this.Text + " ", this.Font);
			UserMenuColors.DrawRoundedRectangle(e.Graphics, 0, size.Height / 2, base.Width - 1, base.Height - size.Height / 2 - 1, 14, this.borderColor);
			UserMenuColors.DrawRoundedRectangle(e.Graphics, 1, size.Height / 2 + 1, base.Width - 3, base.Height - size.Height / 2 - 3, 14, this.shadowColor);
			if (this.Text != string.Empty)
			{
				Rectangle rect = new Rectangle(0, 0, size.Width, size.Height);
				switch (this.textAlign)
				{
				case TextAlignment.Left:
					rect.X = 8;
					break;
				case TextAlignment.Middle:
					rect.X = base.Width / 2 - size.Width / 2;
					break;
				case TextAlignment.Right:
					rect.X = base.Width - size.Width - 9;
					break;
				}
				using (SolidBrush solidBrush = new SolidBrush(base.Parent.BackColor))
				{
					e.Graphics.FillRectangle(solidBrush, rect);
				}
				using (SolidBrush solidBrush2 = new SolidBrush(this.ForeColor))
				{
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
					e.Graphics.DrawString(" " + this.Text + " ", this.Font, solidBrush2, (float)rect.X, (float)rect.Y);
				}
			}
		}
		protected override void OnResize(EventArgs e)
		{
			this.Invalidate();
			base.OnResize(e);
		}
	}
}
