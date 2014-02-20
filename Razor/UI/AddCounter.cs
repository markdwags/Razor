using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Assistant
{
	/// <summary>
	/// Summary description for AddCounter.
	/// </summary>
	public class AddCounter : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox name;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox format;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox itemid;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox hue;
		private System.Windows.Forms.Button Add;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button target;
		private System.Windows.Forms.Button delete;
		private System.Windows.Forms.CheckBox dispImg;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddCounter()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public AddCounter( Counter c ) : this()
		{
			name.Text = c.Name;
			format.Text = c.Format;
			itemid.Text = c.ItemID.ToString();
			hue.Text = c.Hue.ToString();
			dispImg.Checked = c.DisplayImage;

			delete.Visible = true;
			this.Text = "Edit Counter";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.name = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.format = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.itemid = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.hue = new System.Windows.Forms.TextBox();
			this.Add = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.target = new System.Windows.Forms.Button();
			this.delete = new System.Windows.Forms.Button();
			this.dispImg = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			// 
			// name
			// 
			this.name.Location = new System.Drawing.Point(48, 8);
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(68, 20);
			this.name.TabIndex = 1;
			this.name.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(148, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Format:";
			// 
			// format
			// 
			this.format.Location = new System.Drawing.Point(196, 8);
			this.format.Name = "format";
			this.format.Size = new System.Drawing.Size(44, 20);
			this.format.TabIndex = 3;
			this.format.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 36);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Item ID:";
			// 
			// itemid
			// 
			this.itemid.Location = new System.Drawing.Point(48, 32);
			this.itemid.Name = "itemid";
			this.itemid.Size = new System.Drawing.Size(44, 20);
			this.itemid.TabIndex = 5;
			this.itemid.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(116, 36);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(84, 16);
			this.label4.TabIndex = 6;
			this.label4.Text = "Color (Any: -1):";
			// 
			// hue
			// 
			this.hue.Location = new System.Drawing.Point(196, 32);
			this.hue.Name = "hue";
			this.hue.Size = new System.Drawing.Size(44, 20);
			this.hue.TabIndex = 7;
			this.hue.Text = "";
			// 
			// Add
			// 
			this.Add.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Add.Location = new System.Drawing.Point(8, 84);
			this.Add.Name = "Add";
			this.Add.Size = new System.Drawing.Size(56, 20);
			this.Add.TabIndex = 8;
			this.Add.Text = "&Okay";
			this.Add.Click += new System.EventHandler(this.Add_Click);
			// 
			// cancel
			// 
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(72, 84);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(56, 20);
			this.cancel.TabIndex = 9;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// target
			// 
			this.target.Location = new System.Drawing.Point(200, 84);
			this.target.Name = "target";
			this.target.Size = new System.Drawing.Size(56, 20);
			this.target.TabIndex = 10;
			this.target.Text = "Target ";
			this.target.Click += new System.EventHandler(this.target_Click);
			// 
			// delete
			// 
			this.delete.Location = new System.Drawing.Point(136, 84);
			this.delete.Name = "delete";
			this.delete.Size = new System.Drawing.Size(56, 20);
			this.delete.TabIndex = 11;
			this.delete.Text = "Delete";
			this.delete.Visible = false;
			this.delete.Click += new System.EventHandler(this.delete_Click);
			// 
			// dispImg
			// 
			this.dispImg.Checked = true;
			this.dispImg.CheckState = System.Windows.Forms.CheckState.Checked;
			this.dispImg.Location = new System.Drawing.Point(8, 56);
			this.dispImg.Name = "dispImg";
			this.dispImg.Size = new System.Drawing.Size(160, 20);
			this.dispImg.TabIndex = 12;
			this.dispImg.Text = "Display image in titlebar";
			// 
			// AddCounter
			// 
			this.AcceptButton = this.Add;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(264, 112);
			this.ControlBox = false;
			this.Controls.Add(this.dispImg);
			this.Controls.Add(this.hue);
			this.Controls.Add(this.itemid);
			this.Controls.Add(this.format);
			this.Controls.Add(this.name);
			this.Controls.Add(this.delete);
			this.Controls.Add(this.target);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.Add);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AddCounter";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add a Counter";
			this.Load += new System.EventHandler(this.AddCounter_Load);
			this.ResumeLayout(false);

		}
		#endregion

		public string NameStr, FmtStr;
		public int ItemID, Hue;
		public bool DisplayImage;
		private void Add_Click(object sender, System.EventArgs e)
		{
			if ( name.Text.Trim().Length > 0 && format.Text.Trim().Length > 0 )
			{
				NameStr = name.Text.Trim();
				FmtStr = format.Text.Trim();
			}
			else
			{
				MessageBox.Show( this, Language.GetString( LocString.InvalidAbrev ), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				if ( itemid.Text.StartsWith( "0x" ) )
					ItemID = Convert.ToUInt16( itemid.Text.Substring( 2 ).Trim(), 16 );
				else
					ItemID = Convert.ToUInt16( itemid.Text.Trim() );
			}
			catch
			{
				ItemID = 0;
			}

			if ( ItemID == 0 )
			{
				MessageBox.Show( this, Language.GetString( LocString.InvalidIID ), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			Hue = Utility.ToInt32( hue.Text, -1 );

			if ( Hue < -1 || Hue > 0xFFFF )
			{
				MessageBox.Show( this, Language.GetString( LocString.InvalidHue ), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information );
				Hue = 0;
				return;
			}

			DisplayImage = dispImg.Checked;
		}

		private void cancel_Click(object sender, System.EventArgs e)
		{
			Targeting.CancelOneTimeTarget();
		}

		private void target_Click(object sender, System.EventArgs e)
		{
			if ( World.Player != null )
			{
				Targeting.OneTimeTarget( new Targeting.TargetResponseCallback( OnTarget ) );
				World.Player.SendMessage( MsgLevel.Force, LocString.SelItem2Count );
			}
		}

		private void OnTarget( bool loc, Serial serial, Point3D p, ushort graphic )
		{
			Engine.MainWindow.ShowMe();
			this.BringToFront();
			this.Show();
			this.Focus();
			if ( loc )
				return;

			Item item = World.FindItem( serial );
			if ( item != null )
			{
				itemid.Text = item.ItemID.Value.ToString();
				hue.Text = item.Hue == 0 ? "-1" : item.Hue.ToString();
			}
			else
			{
				itemid.Text = graphic.ToString();
				hue.Text = "-1";
			}
		}

		private void AddCounter_Load(object sender, System.EventArgs e)
		{
			Language.LoadControlNames( this );
		}

		private void delete_Click(object sender, System.EventArgs e)
		{
			if ( MessageBox.Show( this, "Are you sure you want to delete this counter?", "Delete Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
			{
				this.DialogResult = DialogResult.Abort;
				this.Close();
			}
		}
	}
}
