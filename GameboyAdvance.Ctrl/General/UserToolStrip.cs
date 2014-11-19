using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserToolStrip : ToolStrip
	{
		public UserToolStrip()
		{
			base.Renderer = new UserToolStripRenderer();
		}
	}
}
