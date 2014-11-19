using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserContextMenuStrip : ContextMenuStrip
	{
		public UserContextMenuStrip()
		{
			base.Renderer = new UserMenuRenderer();
		}
	}
}
