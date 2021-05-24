﻿
using System;
using System.ComponentModel;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace Assistant.UI
{
    partial class RazorScriptEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RazorScriptEditor));
            this.scriptToolStrip = new System.Windows.Forms.ToolStrip();
            this.scriptSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.scriptPlay = new System.Windows.Forms.ToolStripButton();
            this.scriptRecord = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.scriptSettings = new System.Windows.Forms.ToolStripDropDownButton();
            this.alwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptHelp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.scriptInfo = new System.Windows.Forms.ToolStripButton();
            this.scriptLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.scriptEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.scriptDocMap = new FastColoredTextBoxNS.DocumentMap();
            this.scriptStatusStrip = new System.Windows.Forms.StatusStrip();
            this.scriptToolStrip.SuspendLayout();
            this.scriptLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scriptEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // scriptToolStrip
            // 
            this.scriptToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scriptSave,
            this.toolStripSeparator1,
            this.scriptPlay,
            this.scriptRecord,
            this.toolStripSeparator2,
            this.scriptSettings,
            this.scriptHelp,
            this.toolStripSeparator3,
            this.scriptInfo});
            this.scriptToolStrip.Location = new System.Drawing.Point(0, 0);
            this.scriptToolStrip.Name = "scriptToolStrip";
            this.scriptToolStrip.Size = new System.Drawing.Size(801, 25);
            this.scriptToolStrip.TabIndex = 23;
            this.scriptToolStrip.Text = "toolStrip1";
            // 
            // scriptSave
            // 
            this.scriptSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.scriptSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scriptSave.Name = "scriptSave";
            this.scriptSave.Size = new System.Drawing.Size(35, 22);
            this.scriptSave.Text = "Save";
            this.scriptSave.Click += new System.EventHandler(this.scriptSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // scriptPlay
            // 
            this.scriptPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.scriptPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scriptPlay.Name = "scriptPlay";
            this.scriptPlay.Size = new System.Drawing.Size(33, 22);
            this.scriptPlay.Text = "Play";
            this.scriptPlay.Click += new System.EventHandler(this.scriptPlay_Click);
            // 
            // scriptRecord
            // 
            this.scriptRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.scriptRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scriptRecord.Name = "scriptRecord";
            this.scriptRecord.Size = new System.Drawing.Size(48, 22);
            this.scriptRecord.Text = "Record";
            this.scriptRecord.Click += new System.EventHandler(this.scriptRecord_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // scriptSettings
            // 
            this.scriptSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.scriptSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alwaysOnTop});
            this.scriptSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scriptSettings.Name = "scriptSettings";
            this.scriptSettings.Size = new System.Drawing.Size(62, 22);
            this.scriptSettings.Text = "Settings";
            // 
            // alwaysOnTop
            // 
            this.alwaysOnTop.Name = "alwaysOnTop";
            this.alwaysOnTop.Size = new System.Drawing.Size(180, 22);
            this.alwaysOnTop.Text = "Always On Top";
            this.alwaysOnTop.Click += new System.EventHandler(this.alwaysOnTop_Click);
            // 
            // scriptHelp
            // 
            this.scriptHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.scriptHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.scriptHelp.Image = ((System.Drawing.Image)(resources.GetObject("scriptHelp.Image")));
            this.scriptHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scriptHelp.Name = "scriptHelp";
            this.scriptHelp.Size = new System.Drawing.Size(72, 22);
            this.scriptHelp.Text = "Help/Guide";
            this.scriptHelp.Click += new System.EventHandler(this.scriptHelp_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // scriptInfo
            // 
            this.scriptInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.scriptInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scriptInfo.Name = "scriptInfo";
            this.scriptInfo.Size = new System.Drawing.Size(32, 22);
            this.scriptInfo.Text = "Info";
            this.scriptInfo.Click += new System.EventHandler(this.scriptInfo_Click);
            // 
            // scriptLayoutPanel
            // 
            this.scriptLayoutPanel.ColumnCount = 2;
            this.scriptLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.14357F));
            this.scriptLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.85643F));
            this.scriptLayoutPanel.Controls.Add(this.scriptEditor, 0, 0);
            this.scriptLayoutPanel.Controls.Add(this.scriptDocMap, 1, 0);
            this.scriptLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptLayoutPanel.Location = new System.Drawing.Point(0, 25);
            this.scriptLayoutPanel.Name = "scriptLayoutPanel";
            this.scriptLayoutPanel.RowCount = 1;
            this.scriptLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.scriptLayoutPanel.Size = new System.Drawing.Size(801, 511);
            this.scriptLayoutPanel.TabIndex = 24;
            // 
            // scriptEditor
            // 
            this.scriptEditor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.scriptEditor.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.scriptEditor.BackBrush = null;
            this.scriptEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(37)))), ((int)(((byte)(56)))));
            this.scriptEditor.CaretColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.scriptEditor.CharHeight = 15;
            this.scriptEditor.CharWidth = 7;
            this.scriptEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.scriptEditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.scriptEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptEditor.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.scriptEditor.ForeColor = System.Drawing.Color.White;
            this.scriptEditor.IsReplaceMode = false;
            this.scriptEditor.LeftBracket = '(';
            this.scriptEditor.LeftBracket2 = '[';
            this.scriptEditor.LineNumberColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(144)))), ((int)(((byte)(175)))));
            this.scriptEditor.Location = new System.Drawing.Point(3, 3);
            this.scriptEditor.Name = "scriptEditor";
            this.scriptEditor.Paddings = new System.Windows.Forms.Padding(0);
            this.scriptEditor.RightBracket = ')';
            this.scriptEditor.RightBracket2 = ']';
            this.scriptEditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.scriptEditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("scriptEditor.ServiceColors")));
            this.scriptEditor.ShowCaretWhenInactive = false;
            this.scriptEditor.Size = new System.Drawing.Size(675, 505);
            this.scriptEditor.TabIndex = 23;
            this.scriptEditor.Zoom = 100;
            this.scriptEditor.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.scriptEditor_TextChanged);
            this.scriptEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.scriptEditor_KeyDown);
            this.scriptEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scriptEditor_MouseDown);
            // 
            // scriptDocMap
            // 
            this.scriptDocMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(37)))), ((int)(((byte)(56)))));
            this.scriptDocMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptDocMap.ForeColor = System.Drawing.Color.Maroon;
            this.scriptDocMap.Location = new System.Drawing.Point(684, 3);
            this.scriptDocMap.Name = "scriptDocMap";
            this.scriptDocMap.Size = new System.Drawing.Size(114, 505);
            this.scriptDocMap.TabIndex = 24;
            this.scriptDocMap.Target = null;
            // 
            // scriptStatusStrip
            // 
            this.scriptStatusStrip.Location = new System.Drawing.Point(0, 514);
            this.scriptStatusStrip.Name = "scriptStatusStrip";
            this.scriptStatusStrip.Size = new System.Drawing.Size(801, 22);
            this.scriptStatusStrip.TabIndex = 25;
            this.scriptStatusStrip.Text = "statusStrip1";
            // 
            // RazorScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 536);
            this.Controls.Add(this.scriptStatusStrip);
            this.Controls.Add(this.scriptLayoutPanel);
            this.Controls.Add(this.scriptToolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RazorScriptEditor";
            this.Text = "Razor Script Editor (RSE)";
            this.Load += new System.EventHandler(this.RazorScriptEditor_Load);
            this.VisibleChanged += new System.EventHandler(this.RazorScriptEditor_VisibleChanged);
            this.scriptToolStrip.ResumeLayout(false);
            this.scriptToolStrip.PerformLayout();
            this.scriptLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scriptEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip scriptToolStrip;
        private System.Windows.Forms.ToolStripButton scriptSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton scriptRecord;
        private System.Windows.Forms.ToolStripButton scriptPlay;
        private System.Windows.Forms.TableLayoutPanel scriptLayoutPanel;
        private FastColoredTextBoxNS.FastColoredTextBox scriptEditor;
        private FastColoredTextBoxNS.DocumentMap scriptDocMap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton scriptSettings;
        private System.Windows.Forms.ToolStripMenuItem alwaysOnTop;
        private ToolStripButton scriptHelp;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton scriptInfo;
        private StatusStrip scriptStatusStrip;
    }
}