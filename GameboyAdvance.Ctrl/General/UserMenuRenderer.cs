using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserMenuRenderer : ToolStripRenderer
	{
		protected override void InitializeItem(ToolStripItem item)
		{
			base.InitializeItem(item);
			item.ForeColor = Color.Black;
		}
		protected override void Initialize(ToolStrip toolStrip)
		{
			base.Initialize(toolStrip);
			toolStrip.ForeColor = Color.Black;
		}
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			base.OnRenderToolStripBackground(e);
			using (Pen pen = new Pen(Color.White)
			{
				Width = 3f
			})
			{
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(e.AffectedBounds, Color.FromArgb(212, 219, 237), Color.FromArgb(225, 230, 246), 90f))
				{
					e.Graphics.DrawLine(pen, 0, 0, e.AffectedBounds.Width, 0);
					pen.Width = 1f;
					pen.Color = Color.FromArgb(248, 250, 250);
					e.Graphics.DrawLine(pen, 0, 2, e.AffectedBounds.Width, 2);
					pen.Width = 2f;
					e.Graphics.DrawLine(pen, 0, 3, e.AffectedBounds.Width, 3);
					pen.Color = Color.FromArgb(239, 242, 249);
					e.Graphics.DrawLine(pen, 0, 5, e.AffectedBounds.Width, 5);
					pen.Width = 1f;
					pen.Color = Color.FromArgb(232, 236, 246);
					e.Graphics.DrawLine(pen, 0, 7, e.AffectedBounds.Width, 7);
					e.Graphics.FillRectangle(linearGradientBrush, 0, 8, e.AffectedBounds.Width, e.AffectedBounds.Height);
					pen.Width = 1f;
					pen.Color = Color.FromArgb(182, 188, 204);
					e.Graphics.DrawLine(pen, 0, e.AffectedBounds.Height - 1, e.AffectedBounds.Width, e.AffectedBounds.Height - 1);
				}
			}
		}
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
			base.OnRenderImageMargin(e);
			LinearGradientBrush brush = new LinearGradientBrush(e.AffectedBounds, UserMenuColors.clrImageMarginWhite, UserMenuColors.clrImageMarginBlue, LinearGradientMode.Horizontal);
			SolidBrush solidBrush = new SolidBrush(UserMenuColors.clrImageMarginLine);
			SolidBrush solidBrush2 = new SolidBrush(Color.White);
			Rectangle rect = new Rectangle(e.AffectedBounds.Width, 2, 1, e.AffectedBounds.Height);
			Rectangle rect2 = new Rectangle(e.AffectedBounds.Width + 1, 2, 1, e.AffectedBounds.Height);
			SolidBrush solidBrush3 = new SolidBrush(UserMenuColors.clrSubmenuBG);
			Rectangle rect3 = new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height);
			Pen pen = new Pen(UserMenuColors.clrMenuBorder);
			Rectangle rect4 = new Rectangle(0, 1, e.ToolStrip.Width - 1, e.ToolStrip.Height - 2);
			e.Graphics.FillRectangle(solidBrush3, rect3);
			e.Graphics.FillRectangle(brush, e.AffectedBounds);
			e.Graphics.FillRectangle(solidBrush, rect);
			e.Graphics.FillRectangle(solidBrush2, rect2);
			e.Graphics.DrawRectangle(pen, rect4);
			solidBrush.Dispose();
			solidBrush2.Dispose();
			solidBrush3.Dispose();
			pen.Dispose();
		}

		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
            base.OnRenderItemCheck(e);
			if (e.Item.Selected)
			{
				Rectangle rect2 = new Rectangle(4, 2, 18, 18);
				SolidBrush solidBrush2 = new SolidBrush(UserMenuColors.clrSelectedBG_White);
				e.Graphics.FillRectangle(solidBrush2, rect2);
				e.Graphics.DrawImage(e.Image, new Point(5, 3));
				solidBrush2.Dispose();
				return;
			}
            Rectangle rect3 = new Rectangle(3, 1, 19, 19);
			Rectangle rect4 = new Rectangle(4, 2, 18, 18);
            Pen solidBrush3 = new Pen(UserMenuColors.clrMenuBorder);
            SolidBrush solidBrush4 = new SolidBrush(UserMenuColors.clrSelectedBG_White);
			e.Graphics.DrawRectangle(solidBrush3, rect3);
			e.Graphics.FillRectangle(solidBrush4, rect4);
			e.Graphics.DrawImage(e.Image, new Point(5, 3));
			solidBrush3.Dispose();
			solidBrush4.Dispose();
		}
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			base.OnRenderSeparator(e);
			SolidBrush solidBrush = new SolidBrush(UserMenuColors.clrImageMarginLine);
			SolidBrush solidBrush2 = new SolidBrush(Color.White);
			Rectangle rect = new Rectangle(24, 3, e.Item.Width - 32, 1);
			e.Graphics.FillRectangle(solidBrush, rect);
			solidBrush.Dispose();
			solidBrush2.Dispose();
		}
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			e.ArrowColor = Color.Black;
			base.OnRenderArrow(e);
		}
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			base.OnRenderMenuItemBackground(e);
			if (e.Item.Enabled)
			{
				if (!e.Item.IsOnDropDown && e.Item.Selected)
				{
					using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Rectangle(0, 0, e.Item.Width, e.Item.Height), Color.FromArgb(180, 186, 198), Color.FromArgb(224, 228, 243), 90f))
					{
						e.Graphics.FillRectangle(linearGradientBrush, 3, e.Item.Height / 2, e.Item.Width - 6, e.Item.Height / 2 - 3);
						using (Pen pen = new Pen(Color.FromArgb(233, 236, 244)))
						{
							e.Graphics.DrawLine(pen, 2, e.Item.Height / 2, 2, e.Item.Height - 5);
							e.Graphics.DrawLine(pen, 3, e.Item.Height - 5, 4, e.Item.Height - 3);
							e.Graphics.DrawLine(pen, 4, e.Item.Height - 3, e.Item.Width - 5, e.Item.Height - 3);
							e.Graphics.DrawLine(pen, e.Item.Width - 4, e.Item.Height - 4, e.Item.Width - 3, e.Item.Height - 4);
							e.Graphics.DrawLine(pen, e.Item.Width - 3, e.Item.Height - 5, e.Item.Width - 3, e.Item.Height / 2);
							pen.Color = Color.FromArgb(227, 229, 238);
							e.Graphics.DrawLine(pen, 2, e.Item.Height - 4, 3, e.Item.Height - 2);
							e.Graphics.DrawLine(pen, e.Item.Width - 3, e.Item.Height - 4, e.Item.Width - 4, e.Item.Height - 3);
							pen.Color = Color.FromArgb(244, 246, 251);
							e.Graphics.DrawLine(pen, 2, e.Item.Height / 2 - 1, 2, 4);
							e.Graphics.DrawLine(pen, e.Item.Width - 3, e.Item.Height / 2 - 1, e.Item.Width - 3, 4);
							e.Graphics.DrawLine(pen, 3, 3, 4, 2);
							e.Graphics.DrawLine(pen, e.Item.Width - 4, 3, e.Item.Width - 5, 2);
							e.Graphics.DrawLine(pen, 4, 2, e.Item.Width - 4, 2);
							pen.Color = Color.FromArgb(240, 241, 243);
							e.Graphics.DrawLine(pen, 2, 3, 3, 2);
							e.Graphics.DrawLine(pen, e.Item.Width - 3, 3, e.Item.Width - 4, 2);
							pen.Color = Color.FromArgb(148, 153, 165);
							e.Graphics.DrawLine(pen, 1, e.Item.Height / 2 - 3, 1, e.Item.Height - 4);
							e.Graphics.DrawLine(pen, e.Item.Width - 2, e.Item.Height / 2 - 3, e.Item.Width - 2, e.Item.Height - 4);
							e.Graphics.DrawLine(pen, 1, e.Item.Height - 4, 2, e.Item.Height - 3);
							e.Graphics.DrawLine(pen, e.Item.Width - 2, e.Item.Height - 4, e.Item.Width - 3, e.Item.Height - 3);
							e.Graphics.DrawLine(pen, 3, e.Item.Height - 2, e.Item.Width - 4, e.Item.Height - 2);
							pen.Color = Color.FromArgb(194, 198, 212);
							e.Graphics.DrawLine(pen, 1, e.Item.Height - 3, 2, e.Item.Height - 2);
							e.Graphics.DrawLine(pen, e.Item.Width - 2, e.Item.Height - 3, e.Item.Width - 3, e.Item.Height - 2);
							pen.Color = Color.FromArgb(162, 165, 172);
							e.Graphics.DrawLine(pen, 1, e.Item.Height / 2 - 4, 1, 3);
							e.Graphics.DrawLine(pen, e.Item.Width - 2, e.Item.Height / 2 - 4, e.Item.Width - 2, 3);
							e.Graphics.DrawLine(pen, 1, 3, 2, 2);
							e.Graphics.DrawLine(pen, e.Item.Width - 2, 3, e.Item.Width - 3, 2);
							e.Graphics.DrawLine(pen, 3, 1, e.Item.Width - 4, 1);
							pen.Color = Color.FromArgb(212, 214, 217);
							e.Graphics.DrawLine(pen, 1, 2, 2, 1);
							e.Graphics.DrawLine(pen, e.Item.Width - 2, 2, e.Item.Width - 3, 1);
						}
						goto IL_70F;
					}
				}
				if (e.Item.IsOnDropDown && e.Item.Selected)
				{
					Rectangle rect = new Rectangle(4, 2, e.Item.Width - 6, e.Item.Height - 4);
					using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rect, UserMenuColors.clrSelectedBG_White, UserMenuColors.clrSelectedBG_Gray, LinearGradientMode.Vertical))
					{
						using (new SolidBrush(UserMenuColors.clrSelectedBG_Border))
						{
							e.Graphics.FillRectangle(linearGradientBrush2, rect);
							UserMenuColors.DrawRoundedRectangle(e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width, rect.Height + 1, 2, UserMenuColors.clrSelectedBG_Border);
							e.Item.ForeColor = Color.Black;
						}
					}
				}
				IL_70F:
				if ((e.Item as ToolStripMenuItem).DropDown.Visible && !e.Item.IsOnDropDown)
				{
					using (SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb(194, 200, 216)))
					{
						e.Graphics.FillRectangle(solidBrush2, 3, e.Item.Height / 2 - 2, e.Item.Width - 5, e.Item.Height / 2);
						solidBrush2.Color = Color.FromArgb(202, 205, 214);
						e.Graphics.FillRectangle(solidBrush2, 3, 4, e.Item.Width - 5, 4);
						using (Pen pen2 = new Pen(Color.FromArgb(158, 163, 174)))
						{
							pen2.Color = Color.FromArgb(158, 163, 174);
							e.Graphics.DrawLine(pen2, 2, e.Item.Height / 2, 2, e.Item.Height - 5);
							e.Graphics.DrawLine(pen2, 2, e.Item.Height - 4, 3, e.Item.Height - 3);
							e.Graphics.DrawLine(pen2, e.Item.Width - 3, e.Item.Height - 4, e.Item.Width - 4, e.Item.Height - 3);
							pen2.Color = Color.FromArgb(164, 166, 172);
							e.Graphics.DrawLine(pen2, 2, e.Item.Height / 2 - 1, 2, 4);
							e.Graphics.DrawLine(pen2, 3, 3, 4, 2);
							e.Graphics.DrawLine(pen2, e.Item.Width - 4, 3, e.Item.Width - 5, 2);
							e.Graphics.DrawLine(pen2, 4, 2, e.Item.Width - 4, 2);
							pen2.Color = Color.FromArgb(145, 147, 150);
							e.Graphics.DrawLine(pen2, 2, 3, 3, 2);
							e.Graphics.DrawLine(pen2, e.Item.Width - 3, 3, e.Item.Width - 4, 2);
							pen2.Color = Color.FromArgb(182, 188, 202);
							e.Graphics.DrawLine(pen2, 3, e.Item.Height / 2 - 2, 3, e.Item.Height - 2);
							pen2.Color = Color.FromArgb(191, 195, 204);
							e.Graphics.DrawLine(pen2, 3, 4, 3, e.Item.Height / 2 - 3);
							e.Graphics.DrawLine(pen2, 4, 3, e.Item.Width - 3, 3);
							pen2.Color = Color.FromArgb(74, 76, 83);
							e.Graphics.DrawLine(pen2, 1, e.Item.Height / 2 - 3, 1, e.Item.Height - 4);
							e.Graphics.DrawLine(pen2, e.Item.Width - 2, e.Item.Height / 2 - 3, e.Item.Width - 2, e.Item.Height - 4);
							e.Graphics.DrawLine(pen2, 1, e.Item.Height - 4, 2, e.Item.Height - 3);
							e.Graphics.DrawLine(pen2, e.Item.Width - 2, e.Item.Height - 4, e.Item.Width - 3, e.Item.Height - 3);
							e.Graphics.DrawLine(pen2, 3, e.Item.Height - 2, e.Item.Width - 4, e.Item.Height - 2);
							pen2.Color = Color.FromArgb(158, 161, 173);
							e.Graphics.DrawLine(pen2, 1, e.Item.Height - 3, 2, e.Item.Height - 2);
							e.Graphics.DrawLine(pen2, e.Item.Width - 2, e.Item.Height - 3, e.Item.Width - 3, e.Item.Height - 2);
							pen2.Color = Color.FromArgb(83, 84, 87);
							e.Graphics.DrawLine(pen2, 1, e.Item.Height / 2 - 4, 1, 3);
							e.Graphics.DrawLine(pen2, e.Item.Width - 2, e.Item.Height / 2 - 4, e.Item.Width - 2, 3);
							e.Graphics.DrawLine(pen2, 1, 3, 2, 2);
							e.Graphics.DrawLine(pen2, e.Item.Width - 2, 3, e.Item.Width - 3, 2);
							e.Graphics.DrawLine(pen2, 3, 1, e.Item.Width - 4, 1);
							pen2.Color = Color.FromArgb(173, 174, 177);
							e.Graphics.DrawLine(pen2, 1, 2, 2, 1);
							e.Graphics.DrawLine(pen2, e.Item.Width - 2, 2, e.Item.Width - 3, 1);
						}
					}
				}
			}
		}
	}
}
