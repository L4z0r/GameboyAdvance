using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace GameboyAdvance.Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
	public class UserToolStripHexPanel : ToolStripControlHost
	{
		public UserHexPanel HexPanel
		{
			get
			{
				return base.Control as UserHexPanel;
			}
		}
		public UserToolStripHexPanel() : base(UserToolStripHexPanel.CreateHexPanel())
		{
			this.Font = this.HexPanel.Font;
			this.BackColor = Color.Transparent;
		}
		private static UserHexPanel CreateHexPanel()
		{
			return new UserHexPanel
			{
				Height = 22,
				isToolItem = true
			};
		}
	}
}
