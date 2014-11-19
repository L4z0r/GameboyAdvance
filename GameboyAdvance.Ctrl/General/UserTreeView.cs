using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserTreeView : TreeView
	{
		private Size? selectedIndex;
		public Size? SelectedIndex
		{
			get
			{
				return this.selectedIndex;
			}
			set
			{
				this.selectedIndex = value;
			}
		}
		public UserTreeView()
		{
			this.Font = new Font("Segoe UI", 8.25f);
			base.BorderStyle = BorderStyle.None;
			base.DrawMode = TreeViewDrawMode.OwnerDrawText;
			base.HideSelection = false;
			base.Indent = 24;
		}
		protected override void OnDrawNode(DrawTreeNodeEventArgs e)
		{
			TreeNode node = e.Node;
			using (Brush brush = new SolidBrush(this.BackColor))
			{
				e.Graphics.FillRectangle(brush, e.Bounds);
			}
			if (node == base.SelectedNode && node.Bounds.X != 0 && node.Bounds.Y != 0)
			{
                if (node.Text != "Konfiguration")
                {
                    if (node.Nodes.Count != 0)
                    {
                        selectedIndex = new Size(node.Index, -1);
                    }
                    else
                    {
                        selectedIndex = new Size(node.Parent.Index, node.Index);
                    }
                }
                else
                {
                    selectedIndex = null;
                }

				Rectangle rect = new Rectangle(node.Bounds.X, node.Bounds.Y + 1, node.Bounds.Width - 1, node.Bounds.Height - 2);
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, UserMenuColors.clrSelectedBG_White, UserMenuColors.clrSelectedBG_Gray, LinearGradientMode.Vertical))
				{
					using (new SolidBrush(UserMenuColors.clrSelectedBG_Border))
					{
						e.Graphics.FillRectangle(linearGradientBrush, rect);
						UserMenuColors.DrawRoundedRectangle(e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width, rect.Height + 1, 2, UserMenuColors.clrSelectedBG_Border);
					}
				}
			}
			if (node.Bounds.X != 0 || node.Bounds.Y != 0)
			{
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                if (node.NodeFont == null)
				    e.Graphics.DrawString(node.Text, this.Font, SystemBrushes.ControlText, (float)node.Bounds.X, (float)node.Bounds.Y);
			    else
                    e.Graphics.DrawString(node.Text, node.NodeFont, SystemBrushes.ControlText, (float)node.Bounds.X - 20, (float)node.Bounds.Y);
            }
		}
		protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
		{
			if (selectedIndex != null && e.Node.Bounds.Contains(e.X, e.Y))
			{
				base.OnNodeMouseClick(e);
			}
		}
	}
}
