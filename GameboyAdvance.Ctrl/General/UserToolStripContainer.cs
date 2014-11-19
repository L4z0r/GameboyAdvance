using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserToolStripContainer : ToolStripContainer
	{
		public UserToolStripContainer()
		{
			base.TopToolStripPanel.Paint += new PaintEventHandler(this.TopToolStripPanel_Paint);
			base.TopToolStripPanel.SizeChanged += new EventHandler(this.TopToolStripPanel_SizeChanged);
		}
		private void TopToolStripPanel_SizeChanged(object sender, EventArgs e)
		{
			base.Invalidate();
		}
		private void TopToolStripPanel_Paint(object sender, PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle rect = new Rectangle(0, 0, base.Width, base.FindForm().Height);
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, UserMenuColors.clrHorBG_GrayBlue, UserMenuColors.clrHorBG_White, LinearGradientMode.Horizontal))
			{
				graphics.FillRectangle(linearGradientBrush, rect);
			}
		}
	}
}
