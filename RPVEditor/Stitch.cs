using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RPVEditor
{
	/// <summary>
	/// Summary description for Stitch.
	/// </summary>
	public class Stitch : System.Windows.Forms.Form
	{
		public static void Main( string[] args )
		{
			Application.Run( new Stitch() );
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Stitch()
		{
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
			// 
			// Stitch
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Name = "Stitch";
			this.Text = "Stitch";
			this.Load += new System.EventHandler(this.Stitch_Load);

		}
		#endregion

		private void Stitch_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
