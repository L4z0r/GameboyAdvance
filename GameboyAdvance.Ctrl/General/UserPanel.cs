using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserPanel : Panel
	{
		private Color borderColor;
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

        private RenderStyle renderStyle;
        [Browsable(true), Category("Content")]
        public RenderStyle RenderStyle
        {
            get
            {
                return this.renderStyle;
            }
            set
            {
                this.renderStyle = value;
                base.Invalidate();
            }
        }

		public UserPanel()
		{
			this.DoubleBuffered = true;
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.borderColor = Color.FromArgb(160, 160, 160);
		}
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			using (SolidBrush solidBrush = new SolidBrush(base.Parent.BackColor))
			{
				e.Graphics.FillRectangle(solidBrush, 0, 0, base.Width, base.Height);
			}
			using (SolidBrush solidBrush2 = new SolidBrush(this.BackColor))
			{
				e.Graphics.FillRectangle(solidBrush2, 1, 1, base.Width - 1, base.Height - 1);
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
            if (renderStyle == RenderStyle.Rounded)
            {
                UserMenuColors.DrawRoundedRectangle(e.Graphics, 0, 0, base.Width - 1, base.Height - 1, 14, this.borderColor);
            }
                else
            {
                using (var b = new Pen(borderColor))
                    e.Graphics.DrawRectangle(b, 0, 0, base.Width - 1, base.Height - 1);
            }
        }
		protected override void OnResize(EventArgs eventargs)
		{
			base.Invalidate();
			base.OnResize(eventargs);
		}
	}
}
