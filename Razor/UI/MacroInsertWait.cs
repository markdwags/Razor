using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Assistant.Macros;

namespace Assistant
{
	/// <summary>
	/// Summary description for MacroInsertWait.
	/// </summary>
	public class MacroInsertWait : System.Windows.Forms.Form
	{
		private Macro m_Macro;
		private int m_Idx;
		private MacroAction m_Action;

		private System.Windows.Forms.Button insert;
		private System.Windows.Forms.RadioButton radioPause;
		private System.Windows.Forms.TextBox pause;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton radioGump;
		private System.Windows.Forms.RadioButton radioStat;
		private System.Windows.Forms.TextBox statAmount;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.RadioButton radioTarg;
		private System.Windows.Forms.ComboBox statList;
		private System.Windows.Forms.ComboBox statOpList;
		private System.Windows.Forms.RadioButton radioMenu;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MacroInsertWait( Macro m, int idx )
		{
			m_Macro = m;
			m_Idx = idx;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public MacroInsertWait( MacroAction a )
		{
			m_Action = a;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.insert = new System.Windows.Forms.Button();
			this.radioPause = new System.Windows.Forms.RadioButton();
			this.pause = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.radioGump = new System.Windows.Forms.RadioButton();
			this.radioTarg = new System.Windows.Forms.RadioButton();
			this.radioStat = new System.Windows.Forms.RadioButton();
			this.statAmount = new System.Windows.Forms.TextBox();
			this.statList = new System.Windows.Forms.ComboBox();
			this.cancel = new System.Windows.Forms.Button();
			this.statOpList = new System.Windows.Forms.ComboBox();
			this.radioMenu = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// insert
			// 
			this.insert.Location = new System.Drawing.Point(8, 116);
			this.insert.Name = "insert";
			this.insert.Size = new System.Drawing.Size(64, 24);
			this.insert.TabIndex = 0;
			this.insert.Text = "&Insert";
			this.insert.Click += new System.EventHandler(this.insert_Click);
			// 
			// radioPause
			// 
			this.radioPause.Location = new System.Drawing.Point(8, 4);
			this.radioPause.Name = "radioPause";
			this.radioPause.Size = new System.Drawing.Size(76, 20);
			this.radioPause.TabIndex = 1;
			this.radioPause.Text = "Pause for:";
			this.radioPause.CheckedChanged += new System.EventHandler(this.radioPause_CheckedChanged);
			// 
			// pause
			// 
			this.pause.Enabled = false;
			this.pause.Location = new System.Drawing.Point(84, 4);
			this.pause.Name = "pause";
			this.pause.Size = new System.Drawing.Size(40, 20);
			this.pause.TabIndex = 2;
			this.pause.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(128, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "milliseconds";
			// 
			// radioGump
			// 
			this.radioGump.Location = new System.Drawing.Point(8, 24);
			this.radioGump.Name = "radioGump";
			this.radioGump.Size = new System.Drawing.Size(96, 20);
			this.radioGump.TabIndex = 4;
			this.radioGump.Text = "Wait for Gump";
			// 
			// radioTarg
			// 
			this.radioTarg.Location = new System.Drawing.Point(8, 64);
			this.radioTarg.Name = "radioTarg";
			this.radioTarg.Size = new System.Drawing.Size(100, 20);
			this.radioTarg.TabIndex = 5;
			this.radioTarg.Text = "Wait for Target";
			// 
			// radioStat
			// 
			this.radioStat.Location = new System.Drawing.Point(8, 84);
			this.radioStat.Name = "radioStat";
			this.radioStat.Size = new System.Drawing.Size(64, 20);
			this.radioStat.TabIndex = 6;
			this.radioStat.Text = "Wait for ";
			this.radioStat.CheckedChanged += new System.EventHandler(this.radioStat_CheckedChanged);
			// 
			// statAmount
			// 
			this.statAmount.Enabled = false;
			this.statAmount.Location = new System.Drawing.Point(180, 84);
			this.statAmount.Name = "statAmount";
			this.statAmount.Size = new System.Drawing.Size(40, 20);
			this.statAmount.TabIndex = 7;
			this.statAmount.Text = "";
			// 
			// statList
			// 
			this.statList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.statList.Enabled = false;
			this.statList.Items.AddRange(new object[] {
														  "Hits",
														  "Mana",
														  "Stamina"});
			this.statList.Location = new System.Drawing.Point(72, 84);
			this.statList.Name = "statList";
			this.statList.Size = new System.Drawing.Size(64, 21);
			this.statList.TabIndex = 8;
			// 
			// cancel
			// 
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(80, 116);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(64, 24);
			this.cancel.TabIndex = 10;
			this.cancel.Text = "&Cancel";
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// statOpList
			// 
			this.statOpList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.statOpList.Enabled = false;
			this.statOpList.Items.AddRange(new object[] {
															"<=",
															">="});
			this.statOpList.Location = new System.Drawing.Point(136, 84);
			this.statOpList.Name = "statOpList";
			this.statOpList.Size = new System.Drawing.Size(44, 21);
			this.statOpList.TabIndex = 11;
			// 
			// radioMenu
			// 
			this.radioMenu.Location = new System.Drawing.Point(8, 44);
			this.radioMenu.Name = "radioMenu";
			this.radioMenu.Size = new System.Drawing.Size(176, 20);
			this.radioMenu.TabIndex = 12;
			this.radioMenu.Text = "Wait for old-style Menu/Dialog";
			// 
			// MacroInsertWait
			// 
			this.AcceptButton = this.insert;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(226, 148);
			this.ControlBox = false;
			this.Controls.Add(this.radioMenu);
			this.Controls.Add(this.statOpList);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.statList);
			this.Controls.Add(this.statAmount);
			this.Controls.Add(this.radioStat);
			this.Controls.Add(this.radioTarg);
			this.Controls.Add(this.radioGump);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pause);
			this.Controls.Add(this.radioPause);
			this.Controls.Add(this.insert);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "MacroInsertWait";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Insert Wait/Pause...";
			this.Load += new System.EventHandler(this.MacroInsertWait_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void insert_Click(object sender, System.EventArgs e)
		{
			MacroAction a = null;

			if ( radioPause.Checked )
				a = new PauseAction( TimeSpan.FromMilliseconds( Utility.ToInt32( pause.Text, 1000 ) ) );
			else if ( radioGump.Checked )
				a = new WaitForGumpAction( 0 );
			else if ( radioMenu.Checked )
				a = new WaitForMenuAction( 0 );
			else if ( radioTarg.Checked )
				a = new WaitForTargetAction();
			else if ( radioStat.Checked && statList.SelectedIndex >= 0 && statList.SelectedIndex < 3 && statOpList.SelectedIndex >= 0 && statOpList.SelectedIndex < statOpList.Items.Count )
				a = new WaitForStatAction( (IfAction.IfVarType)statList.SelectedIndex, (byte)statOpList.SelectedIndex, Utility.ToInt32( statAmount.Text, 0 ) );
			
			if ( a != null )
			{
				if ( m_Action == null )
					m_Macro.Insert( m_Idx+1, a );
				else
					m_Action.Parent.Convert( m_Action, a );
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void radioStat_CheckedChanged(object sender, System.EventArgs e)
		{
			statList.Enabled = statAmount.Enabled = statOpList.Enabled = radioStat.Checked;
		}

		private void radioPause_CheckedChanged(object sender, System.EventArgs e)
		{
			pause.Enabled = radioPause.Checked;
		}

		private void MacroInsertWait_Load(object sender, System.EventArgs e)
		{
			Language.LoadControlNames( this );

			radioPause.Checked = m_Action is PauseAction;
			radioGump.Checked = m_Action is WaitForGumpAction;
			radioMenu.Checked = m_Action is WaitForMenuAction;
			radioTarg.Checked = m_Action is WaitForTargetAction;
			radioStat.Checked = m_Action is WaitForStatAction;

			if ( radioPause.Checked )
			{
				pause.Text = ((int)((PauseAction)m_Action).Timeout.TotalMilliseconds).ToString();
			}
			else if ( radioStat.Checked )
			{
				statList.SelectedIndex = (int)((WaitForStatAction)m_Action).Stat;
				statOpList.SelectedIndex = (int)((WaitForStatAction)m_Action).Op;
				statAmount.Text = ((WaitForStatAction)m_Action).Amount.ToString();
			}
		}
	}
}
