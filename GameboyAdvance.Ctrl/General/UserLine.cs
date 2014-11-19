using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserLine : UserControl
	{
		private Color lineColor;
		private int lineWidth;
		[Browsable(true), Category("Content")]
		public Color LineColor
		{
			get
			{
				return this.lineColor;
			}
			set
			{
				this.lineColor = value;
				base.Invalidate();
			}
		}
		[Browsable(true), Category("Content")]
		public int LineWidth
		{
			get
			{
				return this.lineWidth;
			}
			set
			{
				this.lineWidth = value;
				base.Invalidate();
			}
		}
		public UserLine()
		{
			this.DoubleBuffered = true;
			this.LineColor = Color.FromArgb(160, 160, 160);
			this.LineWidth = 1;
			base.Width = 1;
			this.BackColor = Color.Transparent;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			Pen pen = new Pen(this.lineColor, (float)this.lineWidth);
			pen.DashStyle = DashStyle.Dash;
			e.Graphics.DrawLine(pen, 0, 0, 0, base.Height);
			pen.Dispose();
		}
	}
}
