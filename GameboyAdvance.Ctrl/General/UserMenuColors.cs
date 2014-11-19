using System;
using System.Drawing;
namespace GameboyAdvance.Controls
{
	public class UserMenuColors
	{
		public static Color clrHorBG_GrayBlue = Color.FromArgb(255, 233, 236, 250);
		public static Color clrHorBG_White = Color.FromArgb(255, 244, 247, 252);
		public static Color clrSubmenuBG = Color.FromArgb(255, 240, 240, 240);
		public static Color clrImageMarginBlue = Color.FromArgb(255, 212, 216, 230);
		public static Color clrImageMarginWhite = Color.FromArgb(255, 244, 247, 252);
		public static Color clrImageMarginLine = Color.FromArgb(255, 160, 160, 180);
		public static Color clrSelectedBG_Gray = Color.FromArgb(140, 194, 200, 216);
		public static Color clrSelectedBG_Header_Blue = Color.FromArgb(255, 146, 202, 230);
		public static Color clrSelectedBG_White = Color.FromArgb(140, 202, 205, 214);
		public static Color clrSelectedBG_Border = Color.FromArgb(255, 160, 160, 160);
		public static Color clrSelectedBG_Drop_Blue = Color.FromArgb(255, 139, 195, 225);
		public static Color clrSelectedBG_Drop_Border = Color.FromArgb(255, 48, 127, 177);
		public static Color clrMenuBorder = Color.FromArgb(255, 160, 160, 160);
		public static Color clrCheckBG = Color.FromArgb(255, 206, 237, 250);
		public static Color clrVerBG_GrayBlue = Color.FromArgb(255, 196, 203, 219);
		public static Color clrVerBG_White = Color.FromArgb(255, 250, 250, 253);
		public static Color clrVerBG_Shadow = Color.FromArgb(255, 181, 190, 206);
		public static Color clrToolstripBtnGrad_Blue = Color.FromArgb(255, 129, 192, 224);
		public static Color clrToolstripBtnGrad_White = Color.FromArgb(255, 237, 248, 253);
		public static Color clrToolstripBtn_Border = Color.FromArgb(255, 41, 153, 255);
		public static Color clrToolstripBtnGrad_Blue_Pressed = Color.FromArgb(255, 124, 177, 204);
		public static Color clrToolstripBtnGrad_White_Pressed = Color.FromArgb(255, 228, 245, 252);
		public static void DrawRoundedRectangle(Graphics g, int x, int y, int width, int height, int m_diameter, Color color)
		{
			using (Pen pen = new Pen(color))
			{
				RectangleF rectangleF = new RectangleF((float)x, (float)y, (float)width, (float)height);
				RectangleF rect = new RectangleF(rectangleF.Location, new SizeF((float)m_diameter, (float)m_diameter));
				g.DrawArc(pen, rect, 180f, 90f);
				g.DrawLine(pen, x + Convert.ToInt32(m_diameter / 2), y, x + width - Convert.ToInt32(m_diameter / 2), y);
				rect.X = rectangleF.Right - (float)m_diameter;
				g.DrawArc(pen, rect, 270f, 90f);
				g.DrawLine(pen, x + width, y + Convert.ToInt32(m_diameter / 2), x + width, y + height - Convert.ToInt32(m_diameter / 2));
				rect.Y = rectangleF.Bottom - (float)m_diameter;
				g.DrawArc(pen, rect, 0f, 90f);
				g.DrawLine(pen, x + Convert.ToInt32(m_diameter / 2), y + height, x + width - Convert.ToInt32(m_diameter / 2), y + height);
				rect.X = rectangleF.Left;
				g.DrawArc(pen, rect, 90f, 90f);
				g.DrawLine(pen, x, y + Convert.ToInt32(m_diameter / 2), x, y + height - Convert.ToInt32(m_diameter / 2));
			}
		}
	}
}
