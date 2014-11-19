using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserMenuStrip : MenuStrip
	{
		public UserMenuStrip()
		{
			base.Renderer = new UserMenuRenderer();
		}
	}
}
