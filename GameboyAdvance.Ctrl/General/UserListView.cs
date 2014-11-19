using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace GameboyAdvance.Controls
{
	public class UserListView : ListView
	{
		private ListViewItem selectedItem;
		private int selectedIndex = -1;
		private bool isInitialized;
		public event EventHandler IndexChanged;
		public int Index
		{
			get
			{
				return this.selectedIndex;
			}
			set
			{
				if (this.isInitialized)
				{
					bool flag = this.selectedIndex != value;
					this.selectedIndex = value;
					if (flag)
					{
						this.OnSelectedIndexChanged();
						base.Invalidate();
					}
				}
			}
		}
		public UserListView()
		{
			this.DoubleBuffered = true;
			base.OwnerDraw = true;
			base.View = View.Details;
			base.HeaderStyle = ColumnHeaderStyle.None;
			base.Scrollable = true;
			base.HideSelection = false;
			base.MultiSelect = false;
		}
		protected override void InitLayout()
		{
			base.Columns.Add("DUMMY COLUMN", base.Width - 21);
			base.InitLayout();
			this.isInitialized = true;
		}
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			if (e.Item == this.selectedItem)
			{
				Rectangle bounds = e.Bounds;
				bounds.Width = base.Width;
                bounds.Width -= 21;
				bounds.Height -= 2;
				bounds.X += 2;
				bounds.Y += 2;
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, UserMenuColors.clrSelectedBG_White, UserMenuColors.clrSelectedBG_Gray, LinearGradientMode.Vertical))
				{
					using (new SolidBrush(UserMenuColors.clrSelectedBG_Border))
					{
						e.Graphics.FillRectangle(linearGradientBrush, bounds);
						UserMenuColors.DrawRoundedRectangle(e.Graphics, bounds.X, bounds.Y, bounds.Width, bounds.Height, 2, UserMenuColors.clrSelectedBG_Border);
					}
				}
			}
			if (e.Item.ImageIndex != -1)
			{
				Image image = base.SmallImageList.Images[e.Item.ImageIndex];
				e.Graphics.DrawImage(image, e.Item.Position.X + 1, e.Item.Position.Y + 2);
				e.Graphics.DrawString(e.Item.Text, this.Font, SystemBrushes.ControlText, (float)(e.Item.Position.X + image.Width + 3), (float)(e.Item.Position.Y + e.Item.Bounds.Height / 2 - TextRenderer.MeasureText(e.Item.Text, this.Font).Height / 2 + 1));
				return;
			}
			e.Graphics.DrawString(e.Item.Text, this.Font, SystemBrushes.ControlText, (float)(e.Item.Position.X + 3), (float)(e.Item.Position.Y + e.Item.Bounds.Height / 2 - TextRenderer.MeasureText(e.Item.Text, this.Font).Height / 2 + 1));
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			int num = -1;
			for (int i = 0; i < base.Items.Count; i++)
			{
				if (base.GetItemRect(i).Contains(e.X, e.Y))
				{
					num = i;
					break;
				}
			}
			if (this.Index != num)
			{
				this.Index = num;
				if (num != -1)
				{
					this.selectedItem = base.Items[num];
				}
			}
			base.OnMouseDown(e);
		}
		private void OnSelectedIndexChanged()
		{
			if (this.IndexChanged != null)
			{
				this.IndexChanged(this, EventArgs.Empty);
			}
		}
		protected override void OnResize(EventArgs e)
		{
			base.Invalidate();
		}
	}
}
