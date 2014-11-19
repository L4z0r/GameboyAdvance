using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserToolStripRenderer : ToolStripProfessionalRenderer
	{
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			base.OnRenderToolStripBackground(e);
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(e.AffectedBounds, UserMenuColors.clrVerBG_White, UserMenuColors.clrVerBG_GrayBlue, LinearGradientMode.Vertical))
			{
				using (SolidBrush solidBrush = new SolidBrush(UserMenuColors.clrVerBG_Shadow))
				{
					Rectangle rect = new Rectangle(0, e.ToolStrip.Height - 2, e.ToolStrip.Width, 1);
					e.Graphics.FillRectangle(linearGradientBrush, e.AffectedBounds);
					e.Graphics.FillRectangle(solidBrush, rect);
				}
			}
		}
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			Rectangle rectangle = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
			Rectangle rect = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2);
			if (e.Item.Selected || (e.Item as ToolStripButton).Checked)
			{
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, UserMenuColors.clrSelectedBG_White, UserMenuColors.clrSelectedBG_Gray, LinearGradientMode.Vertical))
				{
					e.Graphics.FillRectangle(linearGradientBrush, rect);
					UserMenuColors.DrawRoundedRectangle(e.Graphics, 0, 0, rectangle.Width - 1, rectangle.Height - 1, 2, Color.FromArgb(160, 160, 160));
				}
			}
			if (e.Item.Pressed)
			{
				using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rect, UserMenuColors.clrSelectedBG_Gray, Color.FromArgb(160, 174, 174, 174), LinearGradientMode.Vertical))
				{
					e.Graphics.FillRectangle(linearGradientBrush2, rect);
					UserMenuColors.DrawRoundedRectangle(e.Graphics, 0, 0, rectangle.Width - 1, rectangle.Height - 1, 2, Color.FromArgb(140, 140, 140));
				}
			}
		}
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			using (Pen pen = new Pen(Color.FromArgb(189, 189, 189)))
			{
				e.Graphics.DrawLine(pen, 2, 5, 2, e.Item.Height - 6);
				pen.Color = Color.White;
				e.Graphics.DrawLine(pen, 3, 6, 3, e.Item.Height - 5);
			}
		}
	}
}
