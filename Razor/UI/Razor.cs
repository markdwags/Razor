using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Assistant.Filters;
using Assistant.Macros;
using System.Diagnostics;
using Assistant.Boat;
using Assistant.UI;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Assistant
{
    public class MainForm : System.Windows.Forms.Form
    {
        #region Class Variables
        private System.Windows.Forms.NotifyIcon m_NotifyIcon;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader skillHDRName;
        private System.Windows.Forms.ColumnHeader skillHDRvalue;
        private System.Windows.Forms.ColumnHeader skillHDRbase;
        private System.Windows.Forms.ColumnHeader skillHDRdelta;
        private System.Windows.Forms.Button resetDelta;
        private System.Windows.Forms.Button setlocks;
        private System.Windows.Forms.ComboBox locks;
        private System.Windows.Forms.ListView skillList;
        private System.Windows.Forms.ColumnHeader skillHDRcap;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button addCounter;
        private System.Windows.Forms.Button delCounter;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox showInBar;
        private System.Windows.Forms.TextBox titleStr;
        private System.Windows.Forms.CheckBox checkNewConts;
        private System.Windows.Forms.Timer timerTimer;
        private System.Windows.Forms.CheckBox alwaysTop;
        private System.Windows.Forms.ColumnHeader cntName;
        private System.Windows.Forms.ColumnHeader cntCount;
        private System.Windows.Forms.ListView counters;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button newProfile;
        private System.Windows.Forms.Button delProfile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox baseTotal;
        private System.Windows.Forms.TabPage dressTab;
        private System.Windows.Forms.Button skillCopySel;
        private System.Windows.Forms.Button skillCopyAll;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button removeDress;
        private System.Windows.Forms.Button addDress;
        private System.Windows.Forms.ListBox dressList;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button targItem;
        private System.Windows.Forms.ListBox dressItems;
        private System.Windows.Forms.Button dressUseCur;
        private System.Windows.Forms.TabPage generalTab;
        private System.Windows.Forms.TabPage displayTab;
        private System.Windows.Forms.TabPage skillsTab;
        private System.Windows.Forms.TabPage hotkeysTab;
        private System.Windows.Forms.CheckBox chkCtrl;
        private System.Windows.Forms.CheckBox chkAlt;
        private System.Windows.Forms.CheckBox chkShift;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox key;
        private System.Windows.Forms.Button setHK;
        private System.Windows.Forms.Button unsetHK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkPass;
        private System.Windows.Forms.TabPage moreOptTab;
        private System.Windows.Forms.TabPage agentsTab;
        private System.Windows.Forms.GroupBox agentGroup;
        private System.Windows.Forms.ListBox agentSubList;
        private System.Windows.Forms.Button agentB1;
        private System.Windows.Forms.Button agentB2;
        private System.Windows.Forms.Button agentB3;
        private System.Windows.Forms.Button dohotkey;
        private System.Windows.Forms.Button agentB4;
        private System.Windows.Forms.Label opacityLabel;
        private System.Windows.Forms.TrackBar opacity;
        private System.Windows.Forms.CheckBox dispDelta;
        private System.Windows.Forms.ComboBox agentList;
        private System.Windows.Forms.Button recount;
        private System.Windows.Forms.CheckBox openCorpses;
        private System.Windows.Forms.TextBox corpseRange;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage macrosTab;
        private System.Windows.Forms.TreeView hotkeyTree;
        private System.Windows.Forms.TabPage screenshotTab;
        private System.Windows.Forms.TabPage aboutTab;
        private System.Windows.Forms.Button newMacro;
        private System.Windows.Forms.Button delMacro;
        private System.Windows.Forms.GroupBox macroActGroup;
        private System.Windows.Forms.ListBox actionList;
        private System.Windows.Forms.Button playMacro;
        private System.Windows.Forms.Button recMacro;
        private System.Windows.Forms.CheckBox loopMacro;
        private System.Windows.Forms.Button dressNow;
        private System.Windows.Forms.Button undressList;
        private System.Windows.Forms.CheckBox spamFilter;
        private System.Windows.Forms.PictureBox screenPrev;
        private System.Windows.Forms.ListBox screensList;
        private System.Windows.Forms.Button setScnPath;
        private System.Windows.Forms.RadioButton radioFull;
        private System.Windows.Forms.RadioButton radioUO;
        private System.Windows.Forms.CheckBox screenAutoCap;
        private System.Windows.Forms.TextBox screenPath;
        private System.Windows.Forms.Button capNow;
        private System.Windows.Forms.CheckBox dispTime;
        private System.Windows.Forms.Button agentB5;
        private System.Windows.Forms.Button agentB6;
        private System.Windows.Forms.CheckBox undressConflicts;
        private System.Windows.Forms.CheckBox titlebarImages;
        private System.Windows.Forms.CheckBox showWelcome;
        private System.Windows.Forms.CheckBox highlightSpellReags;
        private System.Windows.Forms.ColumnHeader skillHDRlock;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.CheckBox queueTargets;
        private System.Windows.Forms.RadioButton systray;
        private System.Windows.Forms.RadioButton taskbar;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox autoStackRes;
        private System.Windows.Forms.Button undressBag;
        private System.Windows.Forms.Button dressDelSel;
        private System.Windows.Forms.CheckBox incomingCorpse;
        private System.Windows.Forms.CheckBox incomingMob;
        private System.Windows.Forms.ComboBox langSel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox profiles;
        private System.Windows.Forms.Label hkStatus;
        private System.Windows.Forms.Button clearDress;
        private System.Windows.Forms.TabPage moreMoreOptTab;
        private System.Windows.Forms.CheckBox excludePouches;
        private System.Windows.Forms.CheckBox filterSnoop;
        private System.Windows.Forms.Label waitDisp;
        private System.Windows.Forms.CheckBox blockDis;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox imgFmt;
        private System.Windows.Forms.TabPage videoTab;
        private System.Windows.Forms.Button vidRec;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button vidOpen;
        private System.Windows.Forms.Button vidPlay;
        private System.Windows.Forms.Button vidPlayStop;
        private System.Windows.Forms.Label vidPlayInfo;
        private System.Windows.Forms.TrackBar playPos;
        private System.Windows.Forms.Button vidClose;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox playSpeed;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox aviFPS;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox aviRes;
        private System.Windows.Forms.Button recAVI;
        private System.Windows.Forms.Button recFolder;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtRecFolder;
        private System.Windows.Forms.TreeView macroTree;
        private ToolTip m_Tip;
        #endregion

        private int m_LastKV = 0;
        private bool m_ProfileConfirmLoad;
        private System.Windows.Forms.CheckBox flipVidHoriz;
        private System.Windows.Forms.CheckBox flipVidVert;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox warnNum;
        private System.Windows.Forms.CheckBox warnCount;
        private System.Windows.Forms.Label rpvTime;
        private System.Windows.Forms.CheckBox showNotoHue;
        private System.Windows.Forms.CheckBox preAOSstatbar;
        private System.Windows.Forms.ComboBox clientPrio;
        private System.Windows.Forms.Label label9;
        private LinkLabel linkGithub;
        private Label label21;
        private Label aboutVer;
        private Button cloneProfile;
        private Button setLTHilight;
        private CheckBox lthilight;
        private Button setHarmHue;
        private Button setNeuHue;
        private Label lblHarmHue;
        private Label lblNeuHue;
        private Label lblBeneHue;
        private Button setBeneHue;
        private Button setSpeechHue;
        private Button setWarnHue;
        private Button setMsgHue;
        private Button setExHue;
        private Label lblWarnHue;
        private Label lblMsgHue;
        private Label lblExHue;
        private TextBox txtSpellFormat;
        private CheckBox chkForceSpellHue;
        private CheckBox chkForceSpeechHue;
        private Label label3;
        private CheckedListBox filters;
        private TextBox filterHotkeys;
        private Label label22;
        private bool m_CanClose = true;
        private TabPage advancedTab;
        private Button backupDataDir;
        private Button openRazorDataDir;
        private ComboBox msglvl;
        private Label label17;
        private CheckBox logPackets;
        private TextBox statusBox;
        private TextBox features;
        private CheckBox negotiate;
        private Label aboutSubInfo;
        private ComboBox titleBarParams;
        private Button expandAdvancedMacros;
        private Label label23;
        private LinkLabel linkLabel1;
        private Label label20;
        private GroupBox absoluteTargetGroup;
        private Button retargetAbsoluteTarget;
        private Button insertAbsoluteTarget;
        private Button removeAbsoluteTarget;
        private Button addAbsoluteTarget;
        private ListBox absoluteTargets;
        private CheckBox targetByTypeDifferent;
        private CheckBox stepThroughMacro;
        private Button nextMacroAction;
        private Button disableSmartCPU;
        private TabPage mapTab;
        private CheckBox trackPlayerPosition;
        private GroupBox groupMapPoints;
        private ListBox mapPoints;
        private Button openUOPS;
        private CheckBox tiltMap;
        private CheckBox showPartyMemberPositions;
        private Button removeMapPoint;
        private Button addMapPoint;
        private CheckBox showPlayerPosition;
        private TextBox forceSizeX;
        private TextBox forceSizeY;
        private TextBox healthFmt;
        private Label label10;
        private CheckBox showHealthOH;
        private CheckBox blockHealPoison;
        private TextBox ltRange;
        private CheckBox potionEquip;
        private TextBox txtObjDelay;
        private CheckBox QueueActions;
        private CheckBox spellUnequip;
        private CheckBox autoOpenDoors;
        private CheckBox alwaysStealth;
        private CheckBox autoFriend;
        private CheckBox chkStealth;
        private CheckBox rememberPwds;
        private CheckBox showtargtext;
        private CheckBox rangeCheckLT;
        private CheckBox actionStatusMsg;
        private Label label8;
        private Label label6;
        private Label label18;
        private CheckBox smartLT;
        private CheckBox gameSize;
        private CheckBox chkPartyOverhead;
        private Button btnMap;
        private Button boatControl;
        private CheckBox showTargetMessagesOverChar;
        private CheckBox showOverheadMessages;
        private Label label24;
        private CheckBox logSkillChanges;
        private Button overHeadMessages;
        private Button saveProfile;
        private Label lightLevel;
        private TrackBar lightLevelBar;
        private CheckBox stealthOverhead;
        private CheckBox captureMibs;
        private CheckBox trackIncomingGold;
        private CheckBox objectDelay;
        private CheckBox showBuffDebuffOverhead;
        private Label lblBuffDebuff;
        private TextBox buffDebuffFormat;
        private CheckBox blockOpenCorpsesTwice;
        private CheckBox screenShotOpenBrowser;
        private CheckBox screenShotNotification;
        private ListBox imgurUploads;
        private CheckBox screenShotClipboard;
        private CheckBox showAttackTarget;
        private CheckBox rangeCheckTargetByType;
        private CheckBox rangeCheckDoubleClick;
        private Button containerLabels;
        private CheckBox showContainerLabels;
        private CheckBox realSeason;
        private ComboBox seasonList;
        private Label lblSeason;
        private TreeView _hotkeyTreeViewCache = new TreeView();

        [DllImport("User32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr wnd, bool reset);
        [DllImport("User32.dll")]
        private static extern IntPtr EnableMenuItem(IntPtr menu, uint item, uint options);

        public Label WaitDisplay { get { return waitDisp; } }

        public MainForm()
        {
            m_ProfileConfirmLoad = true;
            m_Tip = new ToolTip();
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            m_NotifyIcon.ContextMenu =
                 new ContextMenu(new MenuItem[]
                 {
                         new MenuItem( "Show Razor", new EventHandler( DoShowMe ) ),
                         new MenuItem( "Hide Razor", new EventHandler( HideMe ) ),
                         new MenuItem( "-" ),
                         new MenuItem( "Toggle Razor Visibility", new EventHandler( ToggleVisible ) ),
                         new MenuItem( "-" ),
                         new MenuItem( "Close Razor && UO", new EventHandler( OnClose ) )
                 });
            m_NotifyIcon.ContextMenu.MenuItems[0].DefaultItem = true;
        }

        public void SwitchToVidTab()
        {
            tabs.SelectedTab = videoTab;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.m_NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabs = new System.Windows.Forms.TabControl();
            this.generalTab = new System.Windows.Forms.TabPage();
            this.clientPrio = new System.Windows.Forms.ComboBox();
            this.systray = new System.Windows.Forms.RadioButton();
            this.taskbar = new System.Windows.Forms.RadioButton();
            this.langSel = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.saveProfile = new System.Windows.Forms.Button();
            this.cloneProfile = new System.Windows.Forms.Button();
            this.delProfile = new System.Windows.Forms.Button();
            this.newProfile = new System.Windows.Forms.Button();
            this.profiles = new System.Windows.Forms.ComboBox();
            this.showWelcome = new System.Windows.Forms.CheckBox();
            this.opacity = new System.Windows.Forms.TrackBar();
            this.alwaysTop = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.filters = new System.Windows.Forms.CheckedListBox();
            this.opacityLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.moreOptTab = new System.Windows.Forms.TabPage();
            this.realSeason = new System.Windows.Forms.CheckBox();
            this.seasonList = new System.Windows.Forms.ComboBox();
            this.lblSeason = new System.Windows.Forms.Label();
            this.blockOpenCorpsesTwice = new System.Windows.Forms.CheckBox();
            this.lightLevel = new System.Windows.Forms.Label();
            this.lightLevelBar = new System.Windows.Forms.TrackBar();
            this.preAOSstatbar = new System.Windows.Forms.CheckBox();
            this.setLTHilight = new System.Windows.Forms.Button();
            this.lthilight = new System.Windows.Forms.CheckBox();
            this.filterSnoop = new System.Windows.Forms.CheckBox();
            this.corpseRange = new System.Windows.Forms.TextBox();
            this.incomingMob = new System.Windows.Forms.CheckBox();
            this.setHarmHue = new System.Windows.Forms.Button();
            this.setNeuHue = new System.Windows.Forms.Button();
            this.lblHarmHue = new System.Windows.Forms.Label();
            this.lblNeuHue = new System.Windows.Forms.Label();
            this.lblBeneHue = new System.Windows.Forms.Label();
            this.setBeneHue = new System.Windows.Forms.Button();
            this.setSpeechHue = new System.Windows.Forms.Button();
            this.setWarnHue = new System.Windows.Forms.Button();
            this.setMsgHue = new System.Windows.Forms.Button();
            this.setExHue = new System.Windows.Forms.Button();
            this.autoStackRes = new System.Windows.Forms.CheckBox();
            this.queueTargets = new System.Windows.Forms.CheckBox();
            this.spamFilter = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.openCorpses = new System.Windows.Forms.CheckBox();
            this.lblWarnHue = new System.Windows.Forms.Label();
            this.lblMsgHue = new System.Windows.Forms.Label();
            this.lblExHue = new System.Windows.Forms.Label();
            this.blockDis = new System.Windows.Forms.CheckBox();
            this.txtSpellFormat = new System.Windows.Forms.TextBox();
            this.chkForceSpellHue = new System.Windows.Forms.CheckBox();
            this.chkForceSpeechHue = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.incomingCorpse = new System.Windows.Forms.CheckBox();
            this.moreMoreOptTab = new System.Windows.Forms.TabPage();
            this.containerLabels = new System.Windows.Forms.Button();
            this.showContainerLabels = new System.Windows.Forms.CheckBox();
            this.showAttackTarget = new System.Windows.Forms.CheckBox();
            this.lblBuffDebuff = new System.Windows.Forms.Label();
            this.buffDebuffFormat = new System.Windows.Forms.TextBox();
            this.showBuffDebuffOverhead = new System.Windows.Forms.CheckBox();
            this.txtObjDelay = new System.Windows.Forms.TextBox();
            this.objectDelay = new System.Windows.Forms.CheckBox();
            this.stealthOverhead = new System.Windows.Forms.CheckBox();
            this.overHeadMessages = new System.Windows.Forms.Button();
            this.showOverheadMessages = new System.Windows.Forms.CheckBox();
            this.showTargetMessagesOverChar = new System.Windows.Forms.CheckBox();
            this.forceSizeX = new System.Windows.Forms.TextBox();
            this.forceSizeY = new System.Windows.Forms.TextBox();
            this.healthFmt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.showHealthOH = new System.Windows.Forms.CheckBox();
            this.blockHealPoison = new System.Windows.Forms.CheckBox();
            this.ltRange = new System.Windows.Forms.TextBox();
            this.potionEquip = new System.Windows.Forms.CheckBox();
            this.QueueActions = new System.Windows.Forms.CheckBox();
            this.spellUnequip = new System.Windows.Forms.CheckBox();
            this.autoOpenDoors = new System.Windows.Forms.CheckBox();
            this.alwaysStealth = new System.Windows.Forms.CheckBox();
            this.autoFriend = new System.Windows.Forms.CheckBox();
            this.chkStealth = new System.Windows.Forms.CheckBox();
            this.rememberPwds = new System.Windows.Forms.CheckBox();
            this.showtargtext = new System.Windows.Forms.CheckBox();
            this.rangeCheckLT = new System.Windows.Forms.CheckBox();
            this.actionStatusMsg = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.smartLT = new System.Windows.Forms.CheckBox();
            this.gameSize = new System.Windows.Forms.CheckBox();
            this.chkPartyOverhead = new System.Windows.Forms.CheckBox();
            this.displayTab = new System.Windows.Forms.TabPage();
            this.trackIncomingGold = new System.Windows.Forms.CheckBox();
            this.showNotoHue = new System.Windows.Forms.CheckBox();
            this.warnNum = new System.Windows.Forms.TextBox();
            this.warnCount = new System.Windows.Forms.CheckBox();
            this.excludePouches = new System.Windows.Forms.CheckBox();
            this.highlightSpellReags = new System.Windows.Forms.CheckBox();
            this.titlebarImages = new System.Windows.Forms.CheckBox();
            this.checkNewConts = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.titleBarParams = new System.Windows.Forms.ComboBox();
            this.titleStr = new System.Windows.Forms.TextBox();
            this.showInBar = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.counters = new System.Windows.Forms.ListView();
            this.cntName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cntCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.delCounter = new System.Windows.Forms.Button();
            this.addCounter = new System.Windows.Forms.Button();
            this.recount = new System.Windows.Forms.Button();
            this.dressTab = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.clearDress = new System.Windows.Forms.Button();
            this.dressDelSel = new System.Windows.Forms.Button();
            this.undressBag = new System.Windows.Forms.Button();
            this.undressList = new System.Windows.Forms.Button();
            this.dressUseCur = new System.Windows.Forms.Button();
            this.targItem = new System.Windows.Forms.Button();
            this.dressItems = new System.Windows.Forms.ListBox();
            this.dressNow = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.removeDress = new System.Windows.Forms.Button();
            this.addDress = new System.Windows.Forms.Button();
            this.dressList = new System.Windows.Forms.ListBox();
            this.undressConflicts = new System.Windows.Forms.CheckBox();
            this.skillsTab = new System.Windows.Forms.TabPage();
            this.logSkillChanges = new System.Windows.Forms.CheckBox();
            this.dispDelta = new System.Windows.Forms.CheckBox();
            this.skillCopyAll = new System.Windows.Forms.Button();
            this.skillCopySel = new System.Windows.Forms.Button();
            this.baseTotal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.locks = new System.Windows.Forms.ComboBox();
            this.setlocks = new System.Windows.Forms.Button();
            this.resetDelta = new System.Windows.Forms.Button();
            this.skillList = new System.Windows.Forms.ListView();
            this.skillHDRName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillHDRvalue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillHDRbase = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillHDRdelta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillHDRcap = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.skillHDRlock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.agentsTab = new System.Windows.Forms.TabPage();
            this.agentB6 = new System.Windows.Forms.Button();
            this.agentB5 = new System.Windows.Forms.Button();
            this.agentList = new System.Windows.Forms.ComboBox();
            this.agentGroup = new System.Windows.Forms.GroupBox();
            this.agentSubList = new System.Windows.Forms.ListBox();
            this.agentB4 = new System.Windows.Forms.Button();
            this.agentB1 = new System.Windows.Forms.Button();
            this.agentB2 = new System.Windows.Forms.Button();
            this.agentB3 = new System.Windows.Forms.Button();
            this.hotkeysTab = new System.Windows.Forms.TabPage();
            this.filterHotkeys = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.hkStatus = new System.Windows.Forms.Label();
            this.hotkeyTree = new System.Windows.Forms.TreeView();
            this.dohotkey = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.chkPass = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.unsetHK = new System.Windows.Forms.Button();
            this.setHK = new System.Windows.Forms.Button();
            this.key = new System.Windows.Forms.TextBox();
            this.chkCtrl = new System.Windows.Forms.CheckBox();
            this.chkAlt = new System.Windows.Forms.CheckBox();
            this.chkShift = new System.Windows.Forms.CheckBox();
            this.macrosTab = new System.Windows.Forms.TabPage();
            this.rangeCheckDoubleClick = new System.Windows.Forms.CheckBox();
            this.rangeCheckTargetByType = new System.Windows.Forms.CheckBox();
            this.nextMacroAction = new System.Windows.Forms.Button();
            this.stepThroughMacro = new System.Windows.Forms.CheckBox();
            this.expandAdvancedMacros = new System.Windows.Forms.Button();
            this.targetByTypeDifferent = new System.Windows.Forms.CheckBox();
            this.absoluteTargetGroup = new System.Windows.Forms.GroupBox();
            this.retargetAbsoluteTarget = new System.Windows.Forms.Button();
            this.insertAbsoluteTarget = new System.Windows.Forms.Button();
            this.removeAbsoluteTarget = new System.Windows.Forms.Button();
            this.addAbsoluteTarget = new System.Windows.Forms.Button();
            this.absoluteTargets = new System.Windows.Forms.ListBox();
            this.macroTree = new System.Windows.Forms.TreeView();
            this.macroActGroup = new System.Windows.Forms.GroupBox();
            this.waitDisp = new System.Windows.Forms.Label();
            this.loopMacro = new System.Windows.Forms.CheckBox();
            this.recMacro = new System.Windows.Forms.Button();
            this.playMacro = new System.Windows.Forms.Button();
            this.actionList = new System.Windows.Forms.ListBox();
            this.delMacro = new System.Windows.Forms.Button();
            this.newMacro = new System.Windows.Forms.Button();
            this.mapTab = new System.Windows.Forms.TabPage();
            this.captureMibs = new System.Windows.Forms.CheckBox();
            this.boatControl = new System.Windows.Forms.Button();
            this.btnMap = new System.Windows.Forms.Button();
            this.showPlayerPosition = new System.Windows.Forms.CheckBox();
            this.tiltMap = new System.Windows.Forms.CheckBox();
            this.showPartyMemberPositions = new System.Windows.Forms.CheckBox();
            this.openUOPS = new System.Windows.Forms.Button();
            this.groupMapPoints = new System.Windows.Forms.GroupBox();
            this.removeMapPoint = new System.Windows.Forms.Button();
            this.addMapPoint = new System.Windows.Forms.Button();
            this.mapPoints = new System.Windows.Forms.ListBox();
            this.trackPlayerPosition = new System.Windows.Forms.CheckBox();
            this.videoTab = new System.Windows.Forms.TabPage();
            this.txtRecFolder = new System.Windows.Forms.TextBox();
            this.recFolder = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.vidRec = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.flipVidVert = new System.Windows.Forms.CheckBox();
            this.flipVidHoriz = new System.Windows.Forms.CheckBox();
            this.recAVI = new System.Windows.Forms.Button();
            this.aviRes = new System.Windows.Forms.ComboBox();
            this.aviFPS = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.rpvTime = new System.Windows.Forms.Label();
            this.playSpeed = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.vidClose = new System.Windows.Forms.Button();
            this.playPos = new System.Windows.Forms.TrackBar();
            this.vidPlayStop = new System.Windows.Forms.Button();
            this.vidPlay = new System.Windows.Forms.Button();
            this.vidPlayInfo = new System.Windows.Forms.Label();
            this.vidOpen = new System.Windows.Forms.Button();
            this.screenshotTab = new System.Windows.Forms.TabPage();
            this.imgurUploads = new System.Windows.Forms.ListBox();
            this.screenShotClipboard = new System.Windows.Forms.CheckBox();
            this.screenShotNotification = new System.Windows.Forms.CheckBox();
            this.screenShotOpenBrowser = new System.Windows.Forms.CheckBox();
            this.imgFmt = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.capNow = new System.Windows.Forms.Button();
            this.screenPath = new System.Windows.Forms.TextBox();
            this.radioUO = new System.Windows.Forms.RadioButton();
            this.radioFull = new System.Windows.Forms.RadioButton();
            this.screenAutoCap = new System.Windows.Forms.CheckBox();
            this.setScnPath = new System.Windows.Forms.Button();
            this.screensList = new System.Windows.Forms.ListBox();
            this.screenPrev = new System.Windows.Forms.PictureBox();
            this.dispTime = new System.Windows.Forms.CheckBox();
            this.advancedTab = new System.Windows.Forms.TabPage();
            this.disableSmartCPU = new System.Windows.Forms.Button();
            this.negotiate = new System.Windows.Forms.CheckBox();
            this.backupDataDir = new System.Windows.Forms.Button();
            this.openRazorDataDir = new System.Windows.Forms.Button();
            this.msglvl = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.logPackets = new System.Windows.Forms.CheckBox();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.features = new System.Windows.Forms.TextBox();
            this.aboutTab = new System.Windows.Forms.TabPage();
            this.label24 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label23 = new System.Windows.Forms.Label();
            this.aboutSubInfo = new System.Windows.Forms.Label();
            this.linkGithub = new System.Windows.Forms.LinkLabel();
            this.label21 = new System.Windows.Forms.Label();
            this.aboutVer = new System.Windows.Forms.Label();
            this.timerTimer = new System.Windows.Forms.Timer(this.components);
            this.tabs.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.moreOptTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lightLevelBar)).BeginInit();
            this.moreMoreOptTab.SuspendLayout();
            this.displayTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.dressTab.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.skillsTab.SuspendLayout();
            this.agentsTab.SuspendLayout();
            this.agentGroup.SuspendLayout();
            this.hotkeysTab.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.macrosTab.SuspendLayout();
            this.absoluteTargetGroup.SuspendLayout();
            this.macroActGroup.SuspendLayout();
            this.mapTab.SuspendLayout();
            this.groupMapPoints.SuspendLayout();
            this.videoTab.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playPos)).BeginInit();
            this.screenshotTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenPrev)).BeginInit();
            this.advancedTab.SuspendLayout();
            this.aboutTab.SuspendLayout();
            this.SuspendLayout();
            //
            // m_NotifyIcon
            //
            this.m_NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("m_NotifyIcon.Icon")));
            this.m_NotifyIcon.Text = "Razor";
            this.m_NotifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
            //
            // tabs
            //
            this.tabs.Controls.Add(this.generalTab);
            this.tabs.Controls.Add(this.moreOptTab);
            this.tabs.Controls.Add(this.moreMoreOptTab);
            this.tabs.Controls.Add(this.displayTab);
            this.tabs.Controls.Add(this.dressTab);
            this.tabs.Controls.Add(this.skillsTab);
            this.tabs.Controls.Add(this.agentsTab);
            this.tabs.Controls.Add(this.hotkeysTab);
            this.tabs.Controls.Add(this.macrosTab);
            this.tabs.Controls.Add(this.mapTab);
            this.tabs.Controls.Add(this.videoTab);
            this.tabs.Controls.Add(this.screenshotTab);
            this.tabs.Controls.Add(this.advancedTab);
            this.tabs.Controls.Add(this.aboutTab);
            this.tabs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabs.Location = new System.Drawing.Point(2, 0);
            this.tabs.Multiline = true;
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(490, 497);
            this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabs.TabIndex = 0;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_IndexChanged);
            this.tabs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabs_KeyDown);
            //
            // generalTab
            //
            this.generalTab.Controls.Add(this.clientPrio);
            this.generalTab.Controls.Add(this.systray);
            this.generalTab.Controls.Add(this.taskbar);
            this.generalTab.Controls.Add(this.langSel);
            this.generalTab.Controls.Add(this.label7);
            this.generalTab.Controls.Add(this.label11);
            this.generalTab.Controls.Add(this.groupBox4);
            this.generalTab.Controls.Add(this.showWelcome);
            this.generalTab.Controls.Add(this.opacity);
            this.generalTab.Controls.Add(this.alwaysTop);
            this.generalTab.Controls.Add(this.groupBox1);
            this.generalTab.Controls.Add(this.opacityLabel);
            this.generalTab.Controls.Add(this.label9);
            this.generalTab.Location = new System.Drawing.Point(4, 44);
            this.generalTab.Name = "generalTab";
            this.generalTab.Size = new System.Drawing.Size(482, 449);
            this.generalTab.TabIndex = 0;
            this.generalTab.Text = "General";
            //
            // clientPrio
            //
            this.clientPrio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clientPrio.Items.AddRange(new object[] {
            "Idle",
            "BelowNormal",
            "Normal",
            "AboveNormal",
            "High",
            "Realtime"});
            this.clientPrio.Location = new System.Drawing.Point(319, 159);
            this.clientPrio.Name = "clientPrio";
            this.clientPrio.Size = new System.Drawing.Size(154, 23);
            this.clientPrio.TabIndex = 60;
            this.clientPrio.SelectedIndexChanged += new System.EventHandler(this.clientPrio_SelectedIndexChanged);
            //
            // systray
            //
            this.systray.Location = new System.Drawing.Point(319, 130);
            this.systray.Name = "systray";
            this.systray.Size = new System.Drawing.Size(88, 23);
            this.systray.TabIndex = 35;
            this.systray.Text = "System Tray";
            this.systray.CheckedChanged += new System.EventHandler(this.systray_CheckedChanged);
            //
            // taskbar
            //
            this.taskbar.Location = new System.Drawing.Point(241, 130);
            this.taskbar.Name = "taskbar";
            this.taskbar.Size = new System.Drawing.Size(79, 23);
            this.taskbar.TabIndex = 34;
            this.taskbar.Text = "Taskbar";
            this.taskbar.CheckedChanged += new System.EventHandler(this.taskbar_CheckedChanged);
            //
            // langSel
            //
            this.langSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.langSel.Location = new System.Drawing.Point(319, 186);
            this.langSel.Name = "langSel";
            this.langSel.Size = new System.Drawing.Size(154, 23);
            this.langSel.TabIndex = 52;
            this.langSel.SelectedIndexChanged += new System.EventHandler(this.langSel_SelectedIndexChanged);
            //
            // label7
            //
            this.label7.Location = new System.Drawing.Point(181, 189);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 18);
            this.label7.TabIndex = 51;
            this.label7.Text = "Language:";
            //
            // label11
            //
            this.label11.Location = new System.Drawing.Point(181, 134);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 16);
            this.label11.TabIndex = 33;
            this.label11.Text = "Show in:";
            //
            // groupBox4
            //
            this.groupBox4.Controls.Add(this.saveProfile);
            this.groupBox4.Controls.Add(this.cloneProfile);
            this.groupBox4.Controls.Add(this.delProfile);
            this.groupBox4.Controls.Add(this.newProfile);
            this.groupBox4.Controls.Add(this.profiles);
            this.groupBox4.Location = new System.Drawing.Point(181, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(292, 75);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Profiles";
            //
            // saveProfile
            //
            this.saveProfile.Location = new System.Drawing.Point(89, 45);
            this.saveProfile.Name = "saveProfile";
            this.saveProfile.Size = new System.Drawing.Size(50, 23);
            this.saveProfile.TabIndex = 4;
            this.saveProfile.Text = "Save";
            this.saveProfile.Click += new System.EventHandler(this.saveProfile_Click);
            //
            // cloneProfile
            //
            this.cloneProfile.Location = new System.Drawing.Point(145, 45);
            this.cloneProfile.Name = "cloneProfile";
            this.cloneProfile.Size = new System.Drawing.Size(50, 23);
            this.cloneProfile.TabIndex = 3;
            this.cloneProfile.Text = "Clone";
            this.cloneProfile.Click += new System.EventHandler(this.cloneProfile_Click);
            //
            // delProfile
            //
            this.delProfile.Location = new System.Drawing.Point(201, 45);
            this.delProfile.Name = "delProfile";
            this.delProfile.Size = new System.Drawing.Size(50, 23);
            this.delProfile.TabIndex = 2;
            this.delProfile.Text = "Delete";
            this.delProfile.Click += new System.EventHandler(this.delProfile_Click);
            //
            // newProfile
            //
            this.newProfile.Location = new System.Drawing.Point(33, 45);
            this.newProfile.Name = "newProfile";
            this.newProfile.Size = new System.Drawing.Size(50, 23);
            this.newProfile.TabIndex = 1;
            this.newProfile.Text = "New";
            this.newProfile.Click += new System.EventHandler(this.newProfile_Click);
            //
            // profiles
            //
            this.profiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profiles.ItemHeight = 15;
            this.profiles.Location = new System.Drawing.Point(33, 18);
            this.profiles.MaxDropDownItems = 5;
            this.profiles.Name = "profiles";
            this.profiles.Size = new System.Drawing.Size(218, 23);
            this.profiles.TabIndex = 0;
            this.profiles.SelectedIndexChanged += new System.EventHandler(this.profiles_SelectedIndexChanged);
            //
            // showWelcome
            //
            this.showWelcome.Location = new System.Drawing.Point(184, 77);
            this.showWelcome.Name = "showWelcome";
            this.showWelcome.Size = new System.Drawing.Size(152, 23);
            this.showWelcome.TabIndex = 26;
            this.showWelcome.Text = "Show Welcome Screen";
            this.showWelcome.CheckedChanged += new System.EventHandler(this.showWelcome_CheckedChanged);
            //
            // opacity
            //
            this.opacity.AutoSize = false;
            this.opacity.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.opacity.Location = new System.Drawing.Point(279, 214);
            this.opacity.Maximum = 100;
            this.opacity.Minimum = 10;
            this.opacity.Name = "opacity";
            this.opacity.Size = new System.Drawing.Size(197, 21);
            this.opacity.TabIndex = 22;
            this.opacity.TickFrequency = 0;
            this.opacity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.opacity.Value = 100;
            this.opacity.Scroll += new System.EventHandler(this.opacity_Scroll);
            //
            // alwaysTop
            //
            this.alwaysTop.Location = new System.Drawing.Point(184, 101);
            this.alwaysTop.Name = "alwaysTop";
            this.alwaysTop.Size = new System.Drawing.Size(162, 23);
            this.alwaysTop.TabIndex = 3;
            this.alwaysTop.Text = "Use Smart Always on Top";
            this.alwaysTop.CheckedChanged += new System.EventHandler(this.alwaysTop_CheckedChanged);
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.filters);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(167, 232);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            //
            // filters
            //
            this.filters.CheckOnClick = true;
            this.filters.IntegralHeight = false;
            this.filters.Location = new System.Drawing.Point(6, 18);
            this.filters.Name = "filters";
            this.filters.Size = new System.Drawing.Size(155, 208);
            this.filters.TabIndex = 1;
            this.filters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnFilterCheck);
            //
            // opacityLabel
            //
            this.opacityLabel.Location = new System.Drawing.Point(181, 216);
            this.opacityLabel.Name = "opacityLabel";
            this.opacityLabel.Size = new System.Drawing.Size(89, 19);
            this.opacityLabel.TabIndex = 23;
            this.opacityLabel.Text = "Opacity: 100%";
            //
            // label9
            //
            this.label9.Location = new System.Drawing.Point(181, 162);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(129, 18);
            this.label9.TabIndex = 59;
            this.label9.Text = "Default Client Priority:";
            //
            // moreOptTab
            //
            this.moreOptTab.Controls.Add(this.realSeason);
            this.moreOptTab.Controls.Add(this.seasonList);
            this.moreOptTab.Controls.Add(this.lblSeason);
            this.moreOptTab.Controls.Add(this.blockOpenCorpsesTwice);
            this.moreOptTab.Controls.Add(this.lightLevel);
            this.moreOptTab.Controls.Add(this.lightLevelBar);
            this.moreOptTab.Controls.Add(this.preAOSstatbar);
            this.moreOptTab.Controls.Add(this.setLTHilight);
            this.moreOptTab.Controls.Add(this.lthilight);
            this.moreOptTab.Controls.Add(this.filterSnoop);
            this.moreOptTab.Controls.Add(this.corpseRange);
            this.moreOptTab.Controls.Add(this.incomingMob);
            this.moreOptTab.Controls.Add(this.setHarmHue);
            this.moreOptTab.Controls.Add(this.setNeuHue);
            this.moreOptTab.Controls.Add(this.lblHarmHue);
            this.moreOptTab.Controls.Add(this.lblNeuHue);
            this.moreOptTab.Controls.Add(this.lblBeneHue);
            this.moreOptTab.Controls.Add(this.setBeneHue);
            this.moreOptTab.Controls.Add(this.setSpeechHue);
            this.moreOptTab.Controls.Add(this.setWarnHue);
            this.moreOptTab.Controls.Add(this.setMsgHue);
            this.moreOptTab.Controls.Add(this.setExHue);
            this.moreOptTab.Controls.Add(this.autoStackRes);
            this.moreOptTab.Controls.Add(this.queueTargets);
            this.moreOptTab.Controls.Add(this.spamFilter);
            this.moreOptTab.Controls.Add(this.label4);
            this.moreOptTab.Controls.Add(this.openCorpses);
            this.moreOptTab.Controls.Add(this.lblWarnHue);
            this.moreOptTab.Controls.Add(this.lblMsgHue);
            this.moreOptTab.Controls.Add(this.lblExHue);
            this.moreOptTab.Controls.Add(this.blockDis);
            this.moreOptTab.Controls.Add(this.txtSpellFormat);
            this.moreOptTab.Controls.Add(this.chkForceSpellHue);
            this.moreOptTab.Controls.Add(this.chkForceSpeechHue);
            this.moreOptTab.Controls.Add(this.label3);
            this.moreOptTab.Controls.Add(this.incomingCorpse);
            this.moreOptTab.Location = new System.Drawing.Point(4, 44);
            this.moreOptTab.Name = "moreOptTab";
            this.moreOptTab.Size = new System.Drawing.Size(482, 449);
            this.moreOptTab.TabIndex = 5;
            this.moreOptTab.Text = "Options";
            //
            // realSeason
            //
            this.realSeason.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.realSeason.Location = new System.Drawing.Point(61, 240);
            this.realSeason.Name = "realSeason";
            this.realSeason.Size = new System.Drawing.Size(49, 20);
            this.realSeason.TabIndex = 87;
            this.realSeason.Text = "Real";
            this.realSeason.CheckedChanged += new System.EventHandler(this.realSeason_CheckedChanged);
            //
            // seasonList
            //
            this.seasonList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.seasonList.FormattingEnabled = true;
            this.seasonList.Items.AddRange(new object[] {
            "Spring",
            "Summer",
            "Fall",
            "Winter",
            "Desolation",
            "(Server Default)"});
            this.seasonList.Location = new System.Drawing.Point(116, 239);
            this.seasonList.Name = "seasonList";
            this.seasonList.Size = new System.Drawing.Size(111, 23);
            this.seasonList.TabIndex = 86;
            this.seasonList.SelectedIndexChanged += new System.EventHandler(this.seasonList_SelectedIndexChanged);
            //
            // lblSeason
            //
            this.lblSeason.AutoSize = true;
            this.lblSeason.Location = new System.Drawing.Point(8, 242);
            this.lblSeason.Name = "lblSeason";
            this.lblSeason.Size = new System.Drawing.Size(47, 15);
            this.lblSeason.TabIndex = 85;
            this.lblSeason.Text = "Season:";
            //
            // blockOpenCorpsesTwice
            //
            this.blockOpenCorpsesTwice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blockOpenCorpsesTwice.Location = new System.Drawing.Point(245, 109);
            this.blockOpenCorpsesTwice.Name = "blockOpenCorpsesTwice";
            this.blockOpenCorpsesTwice.Size = new System.Drawing.Size(209, 20);
            this.blockOpenCorpsesTwice.TabIndex = 84;
            this.blockOpenCorpsesTwice.Text = "Block opening corpses twice";
            this.blockOpenCorpsesTwice.CheckedChanged += new System.EventHandler(this.blockOpenCorpsesTwice_CheckedChanged);
            //
            // lightLevel
            //
            this.lightLevel.AutoSize = true;
            this.lightLevel.Location = new System.Drawing.Point(8, 268);
            this.lightLevel.Name = "lightLevel";
            this.lightLevel.Size = new System.Drawing.Size(67, 15);
            this.lightLevel.TabIndex = 83;
            this.lightLevel.Text = "Light Level:";
            //
            // lightLevelBar
            //
            this.lightLevelBar.AutoSize = false;
            this.lightLevelBar.Location = new System.Drawing.Point(91, 267);
            this.lightLevelBar.Maximum = 31;
            this.lightLevelBar.Name = "lightLevelBar";
            this.lightLevelBar.Size = new System.Drawing.Size(148, 21);
            this.lightLevelBar.TabIndex = 82;
            this.lightLevelBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.lightLevelBar.Value = 15;
            this.lightLevelBar.Scroll += new System.EventHandler(this.lightLevelBar_Scroll);
            //
            // preAOSstatbar
            //
            this.preAOSstatbar.Location = new System.Drawing.Point(245, 5);
            this.preAOSstatbar.Name = "preAOSstatbar";
            this.preAOSstatbar.Size = new System.Drawing.Size(190, 20);
            this.preAOSstatbar.TabIndex = 57;
            this.preAOSstatbar.Text = "Use Pre-AOS status window";
            this.preAOSstatbar.CheckedChanged += new System.EventHandler(this.preAOSstatbar_CheckedChanged);
            //
            // setLTHilight
            //
            this.setLTHilight.Location = new System.Drawing.Point(167, 111);
            this.setLTHilight.Name = "setLTHilight";
            this.setLTHilight.Size = new System.Drawing.Size(47, 20);
            this.setLTHilight.TabIndex = 51;
            this.setLTHilight.Text = "Set";
            this.setLTHilight.Click += new System.EventHandler(this.setLTHilight_Click);
            //
            // lthilight
            //
            this.lthilight.Location = new System.Drawing.Point(8, 111);
            this.lthilight.Name = "lthilight";
            this.lthilight.Size = new System.Drawing.Size(206, 19);
            this.lthilight.TabIndex = 50;
            this.lthilight.Text = "Last Target Highlight:";
            this.lthilight.CheckedChanged += new System.EventHandler(this.lthilight_CheckedChanged);
            //
            // filterSnoop
            //
            this.filterSnoop.Location = new System.Drawing.Point(245, 188);
            this.filterSnoop.Name = "filterSnoop";
            this.filterSnoop.Size = new System.Drawing.Size(220, 20);
            this.filterSnoop.TabIndex = 49;
            this.filterSnoop.Text = "Filter Snooping Messages";
            this.filterSnoop.CheckedChanged += new System.EventHandler(this.filterSnoop_CheckedChanged);
            //
            // corpseRange
            //
            this.corpseRange.Location = new System.Drawing.Point(405, 81);
            this.corpseRange.Name = "corpseRange";
            this.corpseRange.Size = new System.Drawing.Size(24, 23);
            this.corpseRange.TabIndex = 23;
            this.corpseRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.corpseRange.TextChanged += new System.EventHandler(this.corpseRange_TextChanged);
            //
            // incomingMob
            //
            this.incomingMob.Location = new System.Drawing.Point(245, 214);
            this.incomingMob.Name = "incomingMob";
            this.incomingMob.Size = new System.Drawing.Size(228, 20);
            this.incomingMob.TabIndex = 47;
            this.incomingMob.Text = "Show Names of Incoming People/Creatures";
            this.incomingMob.CheckedChanged += new System.EventHandler(this.incomingMob_CheckedChanged);
            //
            // setHarmHue
            //
            this.setHarmHue.Enabled = false;
            this.setHarmHue.Location = new System.Drawing.Point(101, 179);
            this.setHarmHue.Name = "setHarmHue";
            this.setHarmHue.Size = new System.Drawing.Size(32, 20);
            this.setHarmHue.TabIndex = 42;
            this.setHarmHue.Text = "Set";
            this.setHarmHue.Click += new System.EventHandler(this.setHarmHue_Click);
            //
            // setNeuHue
            //
            this.setNeuHue.Enabled = false;
            this.setNeuHue.Location = new System.Drawing.Point(174, 179);
            this.setNeuHue.Name = "setNeuHue";
            this.setNeuHue.Size = new System.Drawing.Size(32, 20);
            this.setNeuHue.TabIndex = 43;
            this.setNeuHue.Text = "Set";
            this.setNeuHue.Click += new System.EventHandler(this.setNeuHue_Click);
            //
            // lblHarmHue
            //
            this.lblHarmHue.Location = new System.Drawing.Point(88, 165);
            this.lblHarmHue.Name = "lblHarmHue";
            this.lblHarmHue.Size = new System.Drawing.Size(59, 14);
            this.lblHarmHue.TabIndex = 46;
            this.lblHarmHue.Text = "Harmful";
            this.lblHarmHue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblNeuHue
            //
            this.lblNeuHue.Location = new System.Drawing.Point(165, 165);
            this.lblNeuHue.Name = "lblNeuHue";
            this.lblNeuHue.Size = new System.Drawing.Size(52, 14);
            this.lblNeuHue.TabIndex = 45;
            this.lblNeuHue.Text = "Neutral";
            this.lblNeuHue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblBeneHue
            //
            this.lblBeneHue.Location = new System.Drawing.Point(11, 165);
            this.lblBeneHue.Name = "lblBeneHue";
            this.lblBeneHue.Size = new System.Drawing.Size(66, 14);
            this.lblBeneHue.TabIndex = 44;
            this.lblBeneHue.Text = "Beneficial";
            this.lblBeneHue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // setBeneHue
            //
            this.setBeneHue.Enabled = false;
            this.setBeneHue.Location = new System.Drawing.Point(29, 179);
            this.setBeneHue.Name = "setBeneHue";
            this.setBeneHue.Size = new System.Drawing.Size(32, 20);
            this.setBeneHue.TabIndex = 41;
            this.setBeneHue.Text = "Set";
            this.setBeneHue.Click += new System.EventHandler(this.setBeneHue_Click);
            //
            // setSpeechHue
            //
            this.setSpeechHue.Location = new System.Drawing.Point(167, 85);
            this.setSpeechHue.Name = "setSpeechHue";
            this.setSpeechHue.Size = new System.Drawing.Size(47, 20);
            this.setSpeechHue.TabIndex = 40;
            this.setSpeechHue.Text = "Set";
            this.setSpeechHue.Click += new System.EventHandler(this.setSpeechHue_Click);
            //
            // setWarnHue
            //
            this.setWarnHue.Location = new System.Drawing.Point(167, 59);
            this.setWarnHue.Name = "setWarnHue";
            this.setWarnHue.Size = new System.Drawing.Size(47, 20);
            this.setWarnHue.TabIndex = 39;
            this.setWarnHue.Text = "Set";
            this.setWarnHue.Click += new System.EventHandler(this.setWarnHue_Click);
            //
            // setMsgHue
            //
            this.setMsgHue.Location = new System.Drawing.Point(167, 33);
            this.setMsgHue.Name = "setMsgHue";
            this.setMsgHue.Size = new System.Drawing.Size(47, 20);
            this.setMsgHue.TabIndex = 38;
            this.setMsgHue.Text = "Set";
            this.setMsgHue.Click += new System.EventHandler(this.setMsgHue_Click);
            //
            // setExHue
            //
            this.setExHue.Location = new System.Drawing.Point(167, 7);
            this.setExHue.Name = "setExHue";
            this.setExHue.Size = new System.Drawing.Size(47, 20);
            this.setExHue.TabIndex = 37;
            this.setExHue.Text = "Set";
            this.setExHue.Click += new System.EventHandler(this.setExHue_Click);
            //
            // autoStackRes
            //
            this.autoStackRes.Location = new System.Drawing.Point(245, 57);
            this.autoStackRes.Name = "autoStackRes";
            this.autoStackRes.Size = new System.Drawing.Size(228, 20);
            this.autoStackRes.TabIndex = 35;
            this.autoStackRes.Text = "Auto-Stack Ore/Fish/Logs at Feet";
            this.autoStackRes.CheckedChanged += new System.EventHandler(this.autoStackRes_CheckedChanged);
            //
            // queueTargets
            //
            this.queueTargets.Location = new System.Drawing.Point(245, 31);
            this.queueTargets.Name = "queueTargets";
            this.queueTargets.Size = new System.Drawing.Size(228, 20);
            this.queueTargets.TabIndex = 34;
            this.queueTargets.Text = "Queue LastTarget and TargetSelf";
            this.queueTargets.CheckedChanged += new System.EventHandler(this.queueTargets_CheckedChanged);
            //
            // spamFilter
            //
            this.spamFilter.Location = new System.Drawing.Point(245, 162);
            this.spamFilter.Name = "spamFilter";
            this.spamFilter.Size = new System.Drawing.Size(220, 20);
            this.spamFilter.TabIndex = 26;
            this.spamFilter.Text = "Filter repeating system messages";
            this.spamFilter.CheckedChanged += new System.EventHandler(this.spamFilter_CheckedChanged);
            //
            // label4
            //
            this.label4.Location = new System.Drawing.Point(437, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 24;
            this.label4.Text = "tiles";
            //
            // openCorpses
            //
            this.openCorpses.Location = new System.Drawing.Point(245, 83);
            this.openCorpses.Name = "openCorpses";
            this.openCorpses.Size = new System.Drawing.Size(165, 20);
            this.openCorpses.TabIndex = 22;
            this.openCorpses.Text = "Open new corpses within";
            this.openCorpses.CheckedChanged += new System.EventHandler(this.openCorpses_CheckedChanged);
            //
            // lblWarnHue
            //
            this.lblWarnHue.Location = new System.Drawing.Point(8, 59);
            this.lblWarnHue.Name = "lblWarnHue";
            this.lblWarnHue.Size = new System.Drawing.Size(206, 18);
            this.lblWarnHue.TabIndex = 16;
            this.lblWarnHue.Text = "Warning Message Hue:";
            //
            // lblMsgHue
            //
            this.lblMsgHue.Location = new System.Drawing.Point(8, 33);
            this.lblMsgHue.Name = "lblMsgHue";
            this.lblMsgHue.Size = new System.Drawing.Size(206, 18);
            this.lblMsgHue.TabIndex = 15;
            this.lblMsgHue.Text = "Razor Message Hue:";
            //
            // lblExHue
            //
            this.lblExHue.Location = new System.Drawing.Point(8, 7);
            this.lblExHue.Name = "lblExHue";
            this.lblExHue.Size = new System.Drawing.Size(206, 18);
            this.lblExHue.TabIndex = 14;
            this.lblExHue.Text = "Search Exemption Hue:";
            //
            // blockDis
            //
            this.blockDis.Location = new System.Drawing.Point(245, 135);
            this.blockDis.Name = "blockDis";
            this.blockDis.Size = new System.Drawing.Size(184, 20);
            this.blockDis.TabIndex = 55;
            this.blockDis.Text = "Block dismount in war mode";
            this.blockDis.CheckedChanged += new System.EventHandler(this.blockDis_CheckedChanged);
            //
            // txtSpellFormat
            //
            this.txtSpellFormat.Location = new System.Drawing.Point(91, 209);
            this.txtSpellFormat.Name = "txtSpellFormat";
            this.txtSpellFormat.Size = new System.Drawing.Size(136, 23);
            this.txtSpellFormat.TabIndex = 5;
            this.txtSpellFormat.TextChanged += new System.EventHandler(this.txtSpellFormat_TextChanged);
            //
            // chkForceSpellHue
            //
            this.chkForceSpellHue.Location = new System.Drawing.Point(8, 137);
            this.chkForceSpellHue.Name = "chkForceSpellHue";
            this.chkForceSpellHue.Size = new System.Drawing.Size(152, 20);
            this.chkForceSpellHue.TabIndex = 2;
            this.chkForceSpellHue.Text = "Override Spell Hues:";
            this.chkForceSpellHue.CheckedChanged += new System.EventHandler(this.chkForceSpellHue_CheckedChanged);
            //
            // chkForceSpeechHue
            //
            this.chkForceSpeechHue.Location = new System.Drawing.Point(8, 85);
            this.chkForceSpeechHue.Name = "chkForceSpeechHue";
            this.chkForceSpeechHue.Size = new System.Drawing.Size(206, 19);
            this.chkForceSpeechHue.TabIndex = 0;
            this.chkForceSpeechHue.Text = "Override Speech Hue:";
            this.chkForceSpeechHue.CheckedChanged += new System.EventHandler(this.chkForceSpeechHue_CheckedChanged);
            //
            // label3
            //
            this.label3.Location = new System.Drawing.Point(8, 209);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Spell Format:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // incomingCorpse
            //
            this.incomingCorpse.Location = new System.Drawing.Point(245, 240);
            this.incomingCorpse.Name = "incomingCorpse";
            this.incomingCorpse.Size = new System.Drawing.Size(228, 20);
            this.incomingCorpse.TabIndex = 48;
            this.incomingCorpse.Text = "Show Names of New/Incoming Corpses";
            this.incomingCorpse.CheckedChanged += new System.EventHandler(this.incomingCorpse_CheckedChanged);
            //
            // moreMoreOptTab
            //
            this.moreMoreOptTab.Controls.Add(this.containerLabels);
            this.moreMoreOptTab.Controls.Add(this.showContainerLabels);
            this.moreMoreOptTab.Controls.Add(this.showAttackTarget);
            this.moreMoreOptTab.Controls.Add(this.lblBuffDebuff);
            this.moreMoreOptTab.Controls.Add(this.buffDebuffFormat);
            this.moreMoreOptTab.Controls.Add(this.showBuffDebuffOverhead);
            this.moreMoreOptTab.Controls.Add(this.txtObjDelay);
            this.moreMoreOptTab.Controls.Add(this.objectDelay);
            this.moreMoreOptTab.Controls.Add(this.stealthOverhead);
            this.moreMoreOptTab.Controls.Add(this.overHeadMessages);
            this.moreMoreOptTab.Controls.Add(this.showOverheadMessages);
            this.moreMoreOptTab.Controls.Add(this.showTargetMessagesOverChar);
            this.moreMoreOptTab.Controls.Add(this.forceSizeX);
            this.moreMoreOptTab.Controls.Add(this.forceSizeY);
            this.moreMoreOptTab.Controls.Add(this.healthFmt);
            this.moreMoreOptTab.Controls.Add(this.label10);
            this.moreMoreOptTab.Controls.Add(this.showHealthOH);
            this.moreMoreOptTab.Controls.Add(this.blockHealPoison);
            this.moreMoreOptTab.Controls.Add(this.ltRange);
            this.moreMoreOptTab.Controls.Add(this.potionEquip);
            this.moreMoreOptTab.Controls.Add(this.QueueActions);
            this.moreMoreOptTab.Controls.Add(this.spellUnequip);
            this.moreMoreOptTab.Controls.Add(this.autoOpenDoors);
            this.moreMoreOptTab.Controls.Add(this.alwaysStealth);
            this.moreMoreOptTab.Controls.Add(this.autoFriend);
            this.moreMoreOptTab.Controls.Add(this.chkStealth);
            this.moreMoreOptTab.Controls.Add(this.rememberPwds);
            this.moreMoreOptTab.Controls.Add(this.showtargtext);
            this.moreMoreOptTab.Controls.Add(this.rangeCheckLT);
            this.moreMoreOptTab.Controls.Add(this.actionStatusMsg);
            this.moreMoreOptTab.Controls.Add(this.label8);
            this.moreMoreOptTab.Controls.Add(this.label6);
            this.moreMoreOptTab.Controls.Add(this.label18);
            this.moreMoreOptTab.Controls.Add(this.smartLT);
            this.moreMoreOptTab.Controls.Add(this.gameSize);
            this.moreMoreOptTab.Controls.Add(this.chkPartyOverhead);
            this.moreMoreOptTab.Location = new System.Drawing.Point(4, 44);
            this.moreMoreOptTab.Name = "moreMoreOptTab";
            this.moreMoreOptTab.Size = new System.Drawing.Size(482, 449);
            this.moreMoreOptTab.TabIndex = 10;
            this.moreMoreOptTab.Text = "More Options";
            //
            // containerLabels
            //
            this.containerLabels.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerLabels.Location = new System.Drawing.Point(425, 287);
            this.containerLabels.Name = "containerLabels";
            this.containerLabels.Size = new System.Drawing.Size(33, 19);
            this.containerLabels.TabIndex = 82;
            this.containerLabels.Text = "...";
            this.containerLabels.UseVisualStyleBackColor = true;
            this.containerLabels.Click += new System.EventHandler(this.containerLabels_Click);
            //
            // showContainerLabels
            //
            this.showContainerLabels.AutoSize = true;
            this.showContainerLabels.Location = new System.Drawing.Point(245, 288);
            this.showContainerLabels.Name = "showContainerLabels";
            this.showContainerLabels.Size = new System.Drawing.Size(141, 19);
            this.showContainerLabels.TabIndex = 81;
            this.showContainerLabels.Text = "Show container labels";
            this.showContainerLabels.UseVisualStyleBackColor = true;
            this.showContainerLabels.CheckedChanged += new System.EventHandler(this.showContainerLabels_CheckedChanged);
            //
            // showAttackTarget
            //
            this.showAttackTarget.Location = new System.Drawing.Point(245, 263);
            this.showAttackTarget.Name = "showAttackTarget";
            this.showAttackTarget.Size = new System.Drawing.Size(232, 19);
            this.showAttackTarget.TabIndex = 80;
            this.showAttackTarget.Text = "Show attack target name overhead";
            this.showAttackTarget.UseVisualStyleBackColor = true;
            this.showAttackTarget.CheckedChanged += new System.EventHandler(this.showAttackTarget_CheckedChanged);
            //
            // lblBuffDebuff
            //
            this.lblBuffDebuff.Location = new System.Drawing.Point(5, 285);
            this.lblBuffDebuff.Name = "lblBuffDebuff";
            this.lblBuffDebuff.Size = new System.Drawing.Size(115, 18);
            this.lblBuffDebuff.TabIndex = 79;
            this.lblBuffDebuff.Text = "Buff/Debuff Format:";
            this.lblBuffDebuff.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // buffDebuffFormat
            //
            this.buffDebuffFormat.Location = new System.Drawing.Point(124, 284);
            this.buffDebuffFormat.Name = "buffDebuffFormat";
            this.buffDebuffFormat.Size = new System.Drawing.Size(109, 23);
            this.buffDebuffFormat.TabIndex = 78;
            this.buffDebuffFormat.Text = "[{action}{name}]";
            this.buffDebuffFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.buffDebuffFormat.TextChanged += new System.EventHandler(this.buffDebuffFormat_TextChanged);
            //
            // showBuffDebuffOverhead
            //
            this.showBuffDebuffOverhead.AutoSize = true;
            this.showBuffDebuffOverhead.Location = new System.Drawing.Point(8, 263);
            this.showBuffDebuffOverhead.Name = "showBuffDebuffOverhead";
            this.showBuffDebuffOverhead.Size = new System.Drawing.Size(172, 19);
            this.showBuffDebuffOverhead.TabIndex = 77;
            this.showBuffDebuffOverhead.Text = "Show buff/debuff overhead";
            this.showBuffDebuffOverhead.UseVisualStyleBackColor = true;
            this.showBuffDebuffOverhead.CheckedChanged += new System.EventHandler(this.showBuffDebuffOverhead_CheckedChanged);
            //
            // txtObjDelay
            //
            this.txtObjDelay.Location = new System.Drawing.Point(108, 56);
            this.txtObjDelay.Name = "txtObjDelay";
            this.txtObjDelay.Size = new System.Drawing.Size(32, 23);
            this.txtObjDelay.TabIndex = 37;
            this.txtObjDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtObjDelay.TextChanged += new System.EventHandler(this.txtObjDelay_TextChanged);
            //
            // objectDelay
            //
            this.objectDelay.Location = new System.Drawing.Point(8, 55);
            this.objectDelay.Name = "objectDelay";
            this.objectDelay.Size = new System.Drawing.Size(104, 24);
            this.objectDelay.TabIndex = 0;
            this.objectDelay.Text = "Object Delay:";
            this.objectDelay.CheckedChanged += new System.EventHandler(this.objectDelay_CheckedChanged);
            //
            // stealthOverhead
            //
            this.stealthOverhead.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stealthOverhead.Location = new System.Drawing.Point(387, 49);
            this.stealthOverhead.Name = "stealthOverhead";
            this.stealthOverhead.Size = new System.Drawing.Size(92, 36);
            this.stealthOverhead.TabIndex = 76;
            this.stealthOverhead.Text = "Show overhead";
            this.stealthOverhead.UseVisualStyleBackColor = true;
            this.stealthOverhead.CheckedChanged += new System.EventHandler(this.stealthOverhead_CheckedChanged);
            //
            // overHeadMessages
            //
            this.overHeadMessages.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overHeadMessages.Location = new System.Drawing.Point(425, 237);
            this.overHeadMessages.Name = "overHeadMessages";
            this.overHeadMessages.Size = new System.Drawing.Size(33, 19);
            this.overHeadMessages.TabIndex = 75;
            this.overHeadMessages.Text = "...";
            this.overHeadMessages.UseVisualStyleBackColor = true;
            this.overHeadMessages.Click += new System.EventHandler(this.overHeadMessages_Click);
            //
            // showOverheadMessages
            //
            this.showOverheadMessages.AutoSize = true;
            this.showOverheadMessages.Location = new System.Drawing.Point(245, 238);
            this.showOverheadMessages.Name = "showOverheadMessages";
            this.showOverheadMessages.Size = new System.Drawing.Size(161, 19);
            this.showOverheadMessages.TabIndex = 74;
            this.showOverheadMessages.Text = "Show overhead messages";
            this.showOverheadMessages.UseVisualStyleBackColor = true;
            this.showOverheadMessages.CheckedChanged += new System.EventHandler(this.showStunMessagesOverhead_CheckedChanged);
            //
            // showTargetMessagesOverChar
            //
            this.showTargetMessagesOverChar.AutoSize = true;
            this.showTargetMessagesOverChar.Location = new System.Drawing.Point(8, 238);
            this.showTargetMessagesOverChar.Name = "showTargetMessagesOverChar";
            this.showTargetMessagesOverChar.Size = new System.Drawing.Size(225, 19);
            this.showTargetMessagesOverChar.TabIndex = 73;
            this.showTargetMessagesOverChar.Text = "Show Target Self/Last/Clear Overhead";
            this.showTargetMessagesOverChar.UseVisualStyleBackColor = true;
            this.showTargetMessagesOverChar.CheckedChanged += new System.EventHandler(this.showTargetMessagesOverChar_CheckedChanged);
            //
            // forceSizeX
            //
            this.forceSizeX.Location = new System.Drawing.Point(369, 211);
            this.forceSizeX.Name = "forceSizeX";
            this.forceSizeX.Size = new System.Drawing.Size(34, 23);
            this.forceSizeX.TabIndex = 63;
            this.forceSizeX.TextChanged += new System.EventHandler(this.forceSizeX_TextChanged);
            //
            // forceSizeY
            //
            this.forceSizeY.Location = new System.Drawing.Point(425, 211);
            this.forceSizeY.Name = "forceSizeY";
            this.forceSizeY.Size = new System.Drawing.Size(33, 23);
            this.forceSizeY.TabIndex = 64;
            this.forceSizeY.TextChanged += new System.EventHandler(this.forceSizeY_TextChanged);
            //
            // healthFmt
            //
            this.healthFmt.Location = new System.Drawing.Point(102, 183);
            this.healthFmt.Name = "healthFmt";
            this.healthFmt.Size = new System.Drawing.Size(53, 23);
            this.healthFmt.TabIndex = 71;
            this.healthFmt.Text = "[{0}%]";
            this.healthFmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.healthFmt.TextChanged += new System.EventHandler(this.healthFmt_TextChanged);
            //
            // label10
            //
            this.label10.Location = new System.Drawing.Point(8, 184);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 18);
            this.label10.TabIndex = 70;
            this.label10.Text = "Health Format:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // showHealthOH
            //
            this.showHealthOH.Location = new System.Drawing.Point(8, 161);
            this.showHealthOH.Name = "showHealthOH";
            this.showHealthOH.Size = new System.Drawing.Size(231, 20);
            this.showHealthOH.TabIndex = 69;
            this.showHealthOH.Text = "Show health above people/creatures";
            this.showHealthOH.CheckedChanged += new System.EventHandler(this.showHealthOH_CheckedChanged);
            //
            // blockHealPoison
            //
            this.blockHealPoison.Location = new System.Drawing.Point(245, 187);
            this.blockHealPoison.Name = "blockHealPoison";
            this.blockHealPoison.Size = new System.Drawing.Size(201, 20);
            this.blockHealPoison.TabIndex = 68;
            this.blockHealPoison.Text = "Block heal if target is poisoned";
            this.blockHealPoison.CheckedChanged += new System.EventHandler(this.blockHealPoison_CheckedChanged);
            //
            // ltRange
            //
            this.ltRange.Location = new System.Drawing.Point(164, 108);
            this.ltRange.Name = "ltRange";
            this.ltRange.Size = new System.Drawing.Size(30, 23);
            this.ltRange.TabIndex = 41;
            this.ltRange.TextChanged += new System.EventHandler(this.ltRange_TextChanged);
            //
            // potionEquip
            //
            this.potionEquip.Location = new System.Drawing.Point(245, 161);
            this.potionEquip.Name = "potionEquip";
            this.potionEquip.Size = new System.Drawing.Size(232, 20);
            this.potionEquip.TabIndex = 67;
            this.potionEquip.Text = "Auto Un/Re-equip hands for potions";
            this.potionEquip.CheckedChanged += new System.EventHandler(this.potionEquip_CheckedChanged);
            //
            // QueueActions
            //
            this.QueueActions.Location = new System.Drawing.Point(8, 31);
            this.QueueActions.Name = "QueueActions";
            this.QueueActions.Size = new System.Drawing.Size(202, 20);
            this.QueueActions.TabIndex = 34;
            this.QueueActions.Text = "Auto-Queue Object Delay actions ";
            this.QueueActions.CheckedChanged += new System.EventHandler(this.QueueActions_CheckedChanged);
            //
            // spellUnequip
            //
            this.spellUnequip.Location = new System.Drawing.Point(245, 135);
            this.spellUnequip.Name = "spellUnequip";
            this.spellUnequip.Size = new System.Drawing.Size(213, 20);
            this.spellUnequip.TabIndex = 39;
            this.spellUnequip.Text = "Auto Unequip hands before casting";
            this.spellUnequip.CheckedChanged += new System.EventHandler(this.spellUnequip_CheckedChanged);
            //
            // autoOpenDoors
            //
            this.autoOpenDoors.Location = new System.Drawing.Point(245, 109);
            this.autoOpenDoors.Name = "autoOpenDoors";
            this.autoOpenDoors.Size = new System.Drawing.Size(190, 20);
            this.autoOpenDoors.TabIndex = 58;
            this.autoOpenDoors.Text = "Automatically open doors";
            this.autoOpenDoors.CheckedChanged += new System.EventHandler(this.autoOpenDoors_CheckedChanged);
            //
            // alwaysStealth
            //
            this.alwaysStealth.Location = new System.Drawing.Point(245, 83);
            this.alwaysStealth.Name = "alwaysStealth";
            this.alwaysStealth.Size = new System.Drawing.Size(190, 20);
            this.alwaysStealth.TabIndex = 57;
            this.alwaysStealth.Text = "Always show stealth steps ";
            this.alwaysStealth.CheckedChanged += new System.EventHandler(this.alwaysStealth_CheckedChanged);
            //
            // autoFriend
            //
            this.autoFriend.Location = new System.Drawing.Point(245, 31);
            this.autoFriend.Name = "autoFriend";
            this.autoFriend.Size = new System.Drawing.Size(217, 20);
            this.autoFriend.TabIndex = 56;
            this.autoFriend.Text = "Treat party members as \'Friends\'";
            this.autoFriend.CheckedChanged += new System.EventHandler(this.autoFriend_CheckedChanged);
            //
            // chkStealth
            //
            this.chkStealth.Location = new System.Drawing.Point(245, 57);
            this.chkStealth.Name = "chkStealth";
            this.chkStealth.Size = new System.Drawing.Size(190, 20);
            this.chkStealth.TabIndex = 12;
            this.chkStealth.Text = "Count stealth steps";
            this.chkStealth.CheckedChanged += new System.EventHandler(this.chkStealth_CheckedChanged);
            //
            // rememberPwds
            //
            this.rememberPwds.Location = new System.Drawing.Point(245, 5);
            this.rememberPwds.Name = "rememberPwds";
            this.rememberPwds.Size = new System.Drawing.Size(190, 20);
            this.rememberPwds.TabIndex = 54;
            this.rememberPwds.Text = "Remember passwords ";
            this.rememberPwds.CheckedChanged += new System.EventHandler(this.rememberPwds_CheckedChanged);
            //
            // showtargtext
            //
            this.showtargtext.Location = new System.Drawing.Point(8, 135);
            this.showtargtext.Name = "showtargtext";
            this.showtargtext.Size = new System.Drawing.Size(212, 20);
            this.showtargtext.TabIndex = 53;
            this.showtargtext.Text = "Show target flag on single click";
            this.showtargtext.CheckedChanged += new System.EventHandler(this.showtargtext_CheckedChanged);
            //
            // rangeCheckLT
            //
            this.rangeCheckLT.Location = new System.Drawing.Point(8, 109);
            this.rangeCheckLT.Name = "rangeCheckLT";
            this.rangeCheckLT.Size = new System.Drawing.Size(162, 20);
            this.rangeCheckLT.TabIndex = 40;
            this.rangeCheckLT.Text = "Range check Last Target:";
            this.rangeCheckLT.CheckedChanged += new System.EventHandler(this.rangeCheckLT_CheckedChanged);
            //
            // actionStatusMsg
            //
            this.actionStatusMsg.Location = new System.Drawing.Point(8, 5);
            this.actionStatusMsg.Name = "actionStatusMsg";
            this.actionStatusMsg.Size = new System.Drawing.Size(212, 20);
            this.actionStatusMsg.TabIndex = 38;
            this.actionStatusMsg.Text = "Show Action-Queue status messages";
            this.actionStatusMsg.CheckedChanged += new System.EventHandler(this.actionStatusMsg_CheckedChanged);
            //
            // label8
            //
            this.label8.Location = new System.Drawing.Point(200, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 18);
            this.label8.TabIndex = 42;
            this.label8.Text = "tiles";
            //
            // label6
            //
            this.label6.Location = new System.Drawing.Point(146, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 18);
            this.label6.TabIndex = 36;
            this.label6.Text = "ms";
            //
            // label18
            //
            this.label18.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(409, 214);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(10, 18);
            this.label18.TabIndex = 66;
            this.label18.Text = "x";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // smartLT
            //
            this.smartLT.Location = new System.Drawing.Point(8, 83);
            this.smartLT.Name = "smartLT";
            this.smartLT.Size = new System.Drawing.Size(212, 20);
            this.smartLT.TabIndex = 52;
            this.smartLT.Text = "Use smart last target";
            this.smartLT.CheckedChanged += new System.EventHandler(this.smartLT_CheckedChanged);
            //
            // gameSize
            //
            this.gameSize.Location = new System.Drawing.Point(245, 213);
            this.gameSize.Name = "gameSize";
            this.gameSize.Size = new System.Drawing.Size(118, 18);
            this.gameSize.TabIndex = 65;
            this.gameSize.Text = "Force Game Size:";
            this.gameSize.CheckedChanged += new System.EventHandler(this.gameSize_CheckedChanged);
            //
            // chkPartyOverhead
            //
            this.chkPartyOverhead.Location = new System.Drawing.Point(8, 212);
            this.chkPartyOverhead.Name = "chkPartyOverhead";
            this.chkPartyOverhead.Size = new System.Drawing.Size(238, 20);
            this.chkPartyOverhead.TabIndex = 72;
            this.chkPartyOverhead.Text = "Show mana/stam above party members";
            this.chkPartyOverhead.CheckedChanged += new System.EventHandler(this.chkPartyOverhead_CheckedChanged);
            //
            // displayTab
            //
            this.displayTab.Controls.Add(this.trackIncomingGold);
            this.displayTab.Controls.Add(this.showNotoHue);
            this.displayTab.Controls.Add(this.warnNum);
            this.displayTab.Controls.Add(this.warnCount);
            this.displayTab.Controls.Add(this.excludePouches);
            this.displayTab.Controls.Add(this.highlightSpellReags);
            this.displayTab.Controls.Add(this.titlebarImages);
            this.displayTab.Controls.Add(this.checkNewConts);
            this.displayTab.Controls.Add(this.groupBox3);
            this.displayTab.Controls.Add(this.groupBox2);
            this.displayTab.Location = new System.Drawing.Point(4, 44);
            this.displayTab.Name = "displayTab";
            this.displayTab.Size = new System.Drawing.Size(482, 449);
            this.displayTab.TabIndex = 1;
            this.displayTab.Text = "Display/Counters";
            //
            // trackIncomingGold
            //
            this.trackIncomingGold.AutoSize = true;
            this.trackIncomingGold.Location = new System.Drawing.Point(216, 245);
            this.trackIncomingGold.Name = "trackIncomingGold";
            this.trackIncomingGold.Size = new System.Drawing.Size(177, 19);
            this.trackIncomingGold.TabIndex = 48;
            this.trackIncomingGold.Text = "Track gold per sec/min/hour";
            this.trackIncomingGold.UseVisualStyleBackColor = true;
            this.trackIncomingGold.CheckedChanged += new System.EventHandler(this.trackIncomingGold_CheckedChanged);
            //
            // showNotoHue
            //
            this.showNotoHue.Location = new System.Drawing.Point(216, 168);
            this.showNotoHue.Name = "showNotoHue";
            this.showNotoHue.Size = new System.Drawing.Size(251, 20);
            this.showNotoHue.TabIndex = 47;
            this.showNotoHue.Text = "Show noto hue on {char} in TitleBar";
            this.showNotoHue.CheckedChanged += new System.EventHandler(this.showNotoHue_CheckedChanged);
            //
            // warnNum
            //
            this.warnNum.Location = new System.Drawing.Point(405, 218);
            this.warnNum.Name = "warnNum";
            this.warnNum.Size = new System.Drawing.Size(20, 23);
            this.warnNum.TabIndex = 46;
            this.warnNum.Text = "3";
            this.warnNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.warnNum.TextChanged += new System.EventHandler(this.warnNum_TextChanged);
            //
            // warnCount
            //
            this.warnCount.Location = new System.Drawing.Point(216, 220);
            this.warnCount.Name = "warnCount";
            this.warnCount.Size = new System.Drawing.Size(192, 19);
            this.warnCount.TabIndex = 45;
            this.warnCount.Text = "Warn when a counter is below:";
            this.warnCount.CheckedChanged += new System.EventHandler(this.warnCount_CheckedChanged);
            //
            // excludePouches
            //
            this.excludePouches.Location = new System.Drawing.Point(14, 243);
            this.excludePouches.Name = "excludePouches";
            this.excludePouches.Size = new System.Drawing.Size(192, 21);
            this.excludePouches.TabIndex = 14;
            this.excludePouches.Text = "Never auto-search pouches";
            this.excludePouches.CheckedChanged += new System.EventHandler(this.excludePouches_CheckedChanged);
            //
            // highlightSpellReags
            //
            this.highlightSpellReags.Location = new System.Drawing.Point(216, 142);
            this.highlightSpellReags.Name = "highlightSpellReags";
            this.highlightSpellReags.Size = new System.Drawing.Size(251, 20);
            this.highlightSpellReags.TabIndex = 13;
            this.highlightSpellReags.Text = "Highlight Spell Reagents on Cast";
            this.highlightSpellReags.CheckedChanged += new System.EventHandler(this.highlightSpellReags_CheckedChanged);
            //
            // titlebarImages
            //
            this.titlebarImages.Location = new System.Drawing.Point(216, 194);
            this.titlebarImages.Name = "titlebarImages";
            this.titlebarImages.Size = new System.Drawing.Size(251, 20);
            this.titlebarImages.TabIndex = 12;
            this.titlebarImages.Text = "Show Images with Counters";
            this.titlebarImages.CheckedChanged += new System.EventHandler(this.titlebarImages_CheckedChanged);
            //
            // checkNewConts
            //
            this.checkNewConts.Location = new System.Drawing.Point(14, 219);
            this.checkNewConts.Name = "checkNewConts";
            this.checkNewConts.Size = new System.Drawing.Size(200, 20);
            this.checkNewConts.TabIndex = 9;
            this.checkNewConts.Text = "Auto search new containers";
            this.checkNewConts.CheckedChanged += new System.EventHandler(this.checkNewConts_CheckedChanged);
            //
            // groupBox3
            //
            this.groupBox3.Controls.Add(this.titleBarParams);
            this.groupBox3.Controls.Add(this.titleStr);
            this.groupBox3.Controls.Add(this.showInBar);
            this.groupBox3.Location = new System.Drawing.Point(210, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(263, 133);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Title Bar Display";
            //
            // titleBarParams
            //
            this.titleBarParams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.titleBarParams.FormattingEnabled = true;
            this.titleBarParams.Items.AddRange(new object[] {
            "{ar}",
            "{bandage}",
            "{char}",
            "{crimtime}",
            "{buffsdebuffs}",
            "{damage}",
            "{dex}",
            "{followersmax}",
            "{followers}",
            "{gold}",
            "{goldtotal}",
            "{goldtotalmin}",
            "{gpm}",
            "{gps}",
            "{gph}",
            "{gate}",
            "{hpmax}",
            "{hp}",
            "{int}",
            "{largestatbar}",
            "{manamax}",
            "{mana}",
            "{maxweight}",
            "{mediumstatbar}",
            "{shard}",
            "{skill}",
            "{stammax}",
            "{stam}",
            "{statbar}",
            "{stealthsteps}",
            "{str}",
            "{uptime}",
            "{weight}",
            "-",
            "{coldresist}",
            "{energyresist}",
            "{fireresist}",
            "{luck}",
            "{physresist}",
            "{poisonresist}",
            "{tithe}"});
            this.titleBarParams.Location = new System.Drawing.Point(153, 12);
            this.titleBarParams.Name = "titleBarParams";
            this.titleBarParams.Size = new System.Drawing.Size(104, 23);
            this.titleBarParams.TabIndex = 5;
            this.titleBarParams.SelectedIndexChanged += new System.EventHandler(this.titleBarParams_SelectedIndexChanged);
            //
            // titleStr
            //
            this.titleStr.Location = new System.Drawing.Point(6, 41);
            this.titleStr.Multiline = true;
            this.titleStr.Name = "titleStr";
            this.titleStr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.titleStr.Size = new System.Drawing.Size(251, 81);
            this.titleStr.TabIndex = 4;
            this.titleStr.TextChanged += new System.EventHandler(this.titleStr_TextChanged);
            //
            // showInBar
            //
            this.showInBar.Location = new System.Drawing.Point(6, 15);
            this.showInBar.Name = "showInBar";
            this.showInBar.Size = new System.Drawing.Size(141, 20);
            this.showInBar.TabIndex = 3;
            this.showInBar.Text = "Show in UO title bar:";
            this.showInBar.CheckedChanged += new System.EventHandler(this.showInBar_CheckedChanged);
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.counters);
            this.groupBox2.Controls.Add(this.delCounter);
            this.groupBox2.Controls.Add(this.addCounter);
            this.groupBox2.Controls.Add(this.recount);
            this.groupBox2.Location = new System.Drawing.Point(8, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(196, 210);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Counters";
            //
            // counters
            //
            this.counters.CheckBoxes = true;
            this.counters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cntName,
            this.cntCount});
            this.counters.GridLines = true;
            this.counters.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.counters.LabelWrap = false;
            this.counters.Location = new System.Drawing.Point(6, 18);
            this.counters.MultiSelect = false;
            this.counters.Name = "counters";
            this.counters.Size = new System.Drawing.Size(180, 160);
            this.counters.TabIndex = 11;
            this.counters.UseCompatibleStateImageBehavior = false;
            this.counters.View = System.Windows.Forms.View.Details;
            this.counters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.counters_ItemCheck);
            //
            // cntName
            //
            this.cntName.Text = "Name (Format)";
            this.cntName.Width = 121;
            //
            // cntCount
            //
            this.cntCount.Text = "Count";
            this.cntCount.Width = 51;
            //
            // delCounter
            //
            this.delCounter.Location = new System.Drawing.Point(64, 184);
            this.delCounter.Name = "delCounter";
            this.delCounter.Size = new System.Drawing.Size(56, 20);
            this.delCounter.TabIndex = 4;
            this.delCounter.Text = "Del/Edit";
            this.delCounter.Click += new System.EventHandler(this.delCounter_Click);
            //
            // addCounter
            //
            this.addCounter.Location = new System.Drawing.Point(6, 184);
            this.addCounter.Name = "addCounter";
            this.addCounter.Size = new System.Drawing.Size(52, 20);
            this.addCounter.TabIndex = 3;
            this.addCounter.Text = "Add...";
            this.addCounter.Click += new System.EventHandler(this.addCounter_Click);
            //
            // recount
            //
            this.recount.Location = new System.Drawing.Point(126, 184);
            this.recount.Name = "recount";
            this.recount.Size = new System.Drawing.Size(60, 20);
            this.recount.TabIndex = 2;
            this.recount.Text = "Recount";
            this.recount.Click += new System.EventHandler(this.recount_Click);
            //
            // dressTab
            //
            this.dressTab.Controls.Add(this.groupBox6);
            this.dressTab.Controls.Add(this.groupBox5);
            this.dressTab.Location = new System.Drawing.Point(4, 44);
            this.dressTab.Name = "dressTab";
            this.dressTab.Size = new System.Drawing.Size(482, 449);
            this.dressTab.TabIndex = 3;
            this.dressTab.Text = "Arm/Dress";
            //
            // groupBox6
            //
            this.groupBox6.Controls.Add(this.clearDress);
            this.groupBox6.Controls.Add(this.dressDelSel);
            this.groupBox6.Controls.Add(this.undressBag);
            this.groupBox6.Controls.Add(this.undressList);
            this.groupBox6.Controls.Add(this.dressUseCur);
            this.groupBox6.Controls.Add(this.targItem);
            this.groupBox6.Controls.Add(this.dressItems);
            this.groupBox6.Controls.Add(this.dressNow);
            this.groupBox6.Location = new System.Drawing.Point(162, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(311, 238);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Arm/Dress Items";
            //
            // clearDress
            //
            this.clearDress.Location = new System.Drawing.Point(207, 209);
            this.clearDress.Name = "clearDress";
            this.clearDress.Size = new System.Drawing.Size(96, 23);
            this.clearDress.TabIndex = 13;
            this.clearDress.Text = "Clear List";
            this.clearDress.Click += new System.EventHandler(this.clearDress_Click);
            //
            // dressDelSel
            //
            this.dressDelSel.Location = new System.Drawing.Point(207, 134);
            this.dressDelSel.Name = "dressDelSel";
            this.dressDelSel.Size = new System.Drawing.Size(96, 23);
            this.dressDelSel.TabIndex = 12;
            this.dressDelSel.Text = "Remove";
            this.dressDelSel.Click += new System.EventHandler(this.dressDelSel_Click);
            //
            // undressBag
            //
            this.undressBag.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.undressBag.Location = new System.Drawing.Point(207, 163);
            this.undressBag.Name = "undressBag";
            this.undressBag.Size = new System.Drawing.Size(96, 40);
            this.undressBag.TabIndex = 11;
            this.undressBag.Text = "Change Undress Bag";
            this.undressBag.Click += new System.EventHandler(this.undressBag_Click);
            //
            // undressList
            //
            this.undressList.Location = new System.Drawing.Point(207, 47);
            this.undressList.Name = "undressList";
            this.undressList.Size = new System.Drawing.Size(96, 23);
            this.undressList.TabIndex = 10;
            this.undressList.Text = "Undress";
            this.undressList.Click += new System.EventHandler(this.undressList_Click);
            //
            // dressUseCur
            //
            this.dressUseCur.Location = new System.Drawing.Point(207, 105);
            this.dressUseCur.Name = "dressUseCur";
            this.dressUseCur.Size = new System.Drawing.Size(96, 23);
            this.dressUseCur.TabIndex = 9;
            this.dressUseCur.Text = "Add Current";
            this.dressUseCur.Click += new System.EventHandler(this.dressUseCur_Click);
            //
            // targItem
            //
            this.targItem.Location = new System.Drawing.Point(207, 76);
            this.targItem.Name = "targItem";
            this.targItem.Size = new System.Drawing.Size(96, 23);
            this.targItem.TabIndex = 7;
            this.targItem.Text = "Add (Target)";
            this.targItem.Click += new System.EventHandler(this.targItem_Click);
            //
            // dressItems
            //
            this.dressItems.IntegralHeight = false;
            this.dressItems.ItemHeight = 15;
            this.dressItems.Location = new System.Drawing.Point(6, 18);
            this.dressItems.Name = "dressItems";
            this.dressItems.Size = new System.Drawing.Size(195, 214);
            this.dressItems.TabIndex = 6;
            this.dressItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dressItems_KeyDown);
            this.dressItems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dressItems_MouseDown);
            //
            // dressNow
            //
            this.dressNow.Location = new System.Drawing.Point(207, 18);
            this.dressNow.Name = "dressNow";
            this.dressNow.Size = new System.Drawing.Size(98, 23);
            this.dressNow.TabIndex = 6;
            this.dressNow.Text = "Dress";
            this.dressNow.Click += new System.EventHandler(this.dressNow_Click);
            //
            // groupBox5
            //
            this.groupBox5.Controls.Add(this.removeDress);
            this.groupBox5.Controls.Add(this.addDress);
            this.groupBox5.Controls.Add(this.dressList);
            this.groupBox5.Controls.Add(this.undressConflicts);
            this.groupBox5.Location = new System.Drawing.Point(8, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(148, 238);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Arm/Dress Selection";
            //
            // removeDress
            //
            this.removeDress.Location = new System.Drawing.Point(72, 156);
            this.removeDress.Name = "removeDress";
            this.removeDress.Size = new System.Drawing.Size(60, 33);
            this.removeDress.TabIndex = 5;
            this.removeDress.Text = "Remove";
            this.removeDress.Click += new System.EventHandler(this.removeDress_Click);
            //
            // addDress
            //
            this.addDress.Location = new System.Drawing.Point(6, 156);
            this.addDress.Name = "addDress";
            this.addDress.Size = new System.Drawing.Size(60, 33);
            this.addDress.TabIndex = 4;
            this.addDress.Text = "Add...";
            this.addDress.Click += new System.EventHandler(this.addDress_Click);
            //
            // dressList
            //
            this.dressList.IntegralHeight = false;
            this.dressList.ItemHeight = 15;
            this.dressList.Location = new System.Drawing.Point(6, 18);
            this.dressList.Name = "dressList";
            this.dressList.Size = new System.Drawing.Size(126, 132);
            this.dressList.TabIndex = 3;
            this.dressList.SelectedIndexChanged += new System.EventHandler(this.dressList_SelectedIndexChanged);
            //
            // undressConflicts
            //
            this.undressConflicts.Location = new System.Drawing.Point(6, 195);
            this.undressConflicts.Name = "undressConflicts";
            this.undressConflicts.Size = new System.Drawing.Size(136, 38);
            this.undressConflicts.TabIndex = 6;
            this.undressConflicts.Text = "Automatically move conflicting items";
            this.undressConflicts.CheckedChanged += new System.EventHandler(this.undressConflicts_CheckedChanged);
            //
            // skillsTab
            //
            this.skillsTab.Controls.Add(this.logSkillChanges);
            this.skillsTab.Controls.Add(this.dispDelta);
            this.skillsTab.Controls.Add(this.skillCopyAll);
            this.skillsTab.Controls.Add(this.skillCopySel);
            this.skillsTab.Controls.Add(this.baseTotal);
            this.skillsTab.Controls.Add(this.label1);
            this.skillsTab.Controls.Add(this.locks);
            this.skillsTab.Controls.Add(this.setlocks);
            this.skillsTab.Controls.Add(this.resetDelta);
            this.skillsTab.Controls.Add(this.skillList);
            this.skillsTab.Location = new System.Drawing.Point(4, 44);
            this.skillsTab.Name = "skillsTab";
            this.skillsTab.Size = new System.Drawing.Size(482, 449);
            this.skillsTab.TabIndex = 2;
            this.skillsTab.Text = "Skills";
            //
            // logSkillChanges
            //
            this.logSkillChanges.AutoSize = true;
            this.logSkillChanges.Location = new System.Drawing.Point(358, 172);
            this.logSkillChanges.Name = "logSkillChanges";
            this.logSkillChanges.Size = new System.Drawing.Size(116, 19);
            this.logSkillChanges.TabIndex = 13;
            this.logSkillChanges.Text = "Log skill changes";
            this.logSkillChanges.UseVisualStyleBackColor = true;
            this.logSkillChanges.CheckedChanged += new System.EventHandler(this.logSkillChanges_CheckedChanged);
            //
            // dispDelta
            //
            this.dispDelta.Location = new System.Drawing.Point(358, 132);
            this.dispDelta.Name = "dispDelta";
            this.dispDelta.Size = new System.Drawing.Size(116, 42);
            this.dispDelta.TabIndex = 11;
            this.dispDelta.Text = "Display skill and stat changes";
            this.dispDelta.CheckedChanged += new System.EventHandler(this.dispDelta_CheckedChanged);
            //
            // skillCopyAll
            //
            this.skillCopyAll.Location = new System.Drawing.Point(358, 100);
            this.skillCopyAll.Name = "skillCopyAll";
            this.skillCopyAll.Size = new System.Drawing.Size(115, 26);
            this.skillCopyAll.TabIndex = 9;
            this.skillCopyAll.Text = "Copy All";
            this.skillCopyAll.Click += new System.EventHandler(this.skillCopyAll_Click);
            //
            // skillCopySel
            //
            this.skillCopySel.Location = new System.Drawing.Point(358, 67);
            this.skillCopySel.Name = "skillCopySel";
            this.skillCopySel.Size = new System.Drawing.Size(115, 27);
            this.skillCopySel.TabIndex = 8;
            this.skillCopySel.Text = "Copy Selected";
            this.skillCopySel.Click += new System.EventHandler(this.skillCopySel_Click);
            //
            // baseTotal
            //
            this.baseTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTotal.Location = new System.Drawing.Point(382, 213);
            this.baseTotal.Name = "baseTotal";
            this.baseTotal.ReadOnly = true;
            this.baseTotal.Size = new System.Drawing.Size(62, 23);
            this.baseTotal.TabIndex = 7;
            this.baseTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(380, 191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Base Total:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // locks
            //
            this.locks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.locks.Items.AddRange(new object[] {
            "Up",
            "Down",
            "Locked"});
            this.locks.Location = new System.Drawing.Point(429, 40);
            this.locks.Name = "locks";
            this.locks.Size = new System.Drawing.Size(44, 23);
            this.locks.TabIndex = 5;
            //
            // setlocks
            //
            this.setlocks.Location = new System.Drawing.Point(358, 40);
            this.setlocks.Name = "setlocks";
            this.setlocks.Size = new System.Drawing.Size(68, 23);
            this.setlocks.TabIndex = 4;
            this.setlocks.Text = "Set all locks:";
            this.setlocks.Click += new System.EventHandler(this.OnSetSkillLocks);
            //
            // resetDelta
            //
            this.resetDelta.Location = new System.Drawing.Point(358, 5);
            this.resetDelta.Name = "resetDelta";
            this.resetDelta.Size = new System.Drawing.Size(115, 29);
            this.resetDelta.TabIndex = 3;
            this.resetDelta.Text = "Reset  +/-";
            this.resetDelta.Click += new System.EventHandler(this.OnResetSkillDelta);
            //
            // skillList
            //
            this.skillList.AutoArrange = false;
            this.skillList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.skillHDRName,
            this.skillHDRvalue,
            this.skillHDRbase,
            this.skillHDRdelta,
            this.skillHDRcap,
            this.skillHDRlock});
            this.skillList.FullRowSelect = true;
            this.skillList.Location = new System.Drawing.Point(8, 5);
            this.skillList.Name = "skillList";
            this.skillList.Size = new System.Drawing.Size(344, 231);
            this.skillList.TabIndex = 1;
            this.skillList.UseCompatibleStateImageBehavior = false;
            this.skillList.View = System.Windows.Forms.View.Details;
            this.skillList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnSkillColClick);
            this.skillList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skillList_MouseDown);
            //
            // skillHDRName
            //
            this.skillHDRName.Text = "Skill Name";
            this.skillHDRName.Width = 93;
            //
            // skillHDRvalue
            //
            this.skillHDRvalue.Text = "Value";
            this.skillHDRvalue.Width = 46;
            //
            // skillHDRbase
            //
            this.skillHDRbase.Text = "Base";
            this.skillHDRbase.Width = 43;
            //
            // skillHDRdelta
            //
            this.skillHDRdelta.Text = "+/-";
            this.skillHDRdelta.Width = 40;
            //
            // skillHDRcap
            //
            this.skillHDRcap.Text = "Cap";
            this.skillHDRcap.Width = 40;
            //
            // skillHDRlock
            //
            this.skillHDRlock.Text = "Lock";
            this.skillHDRlock.Width = 40;
            //
            // agentsTab
            //
            this.agentsTab.Controls.Add(this.agentB6);
            this.agentsTab.Controls.Add(this.agentB5);
            this.agentsTab.Controls.Add(this.agentList);
            this.agentsTab.Controls.Add(this.agentGroup);
            this.agentsTab.Controls.Add(this.agentB4);
            this.agentsTab.Controls.Add(this.agentB1);
            this.agentsTab.Controls.Add(this.agentB2);
            this.agentsTab.Controls.Add(this.agentB3);
            this.agentsTab.Location = new System.Drawing.Point(4, 44);
            this.agentsTab.Name = "agentsTab";
            this.agentsTab.Size = new System.Drawing.Size(482, 449);
            this.agentsTab.TabIndex = 6;
            this.agentsTab.Text = "Agents";
            //
            // agentB6
            //
            this.agentB6.Location = new System.Drawing.Point(8, 206);
            this.agentB6.Name = "agentB6";
            this.agentB6.Size = new System.Drawing.Size(130, 24);
            this.agentB6.TabIndex = 6;
            this.agentB6.Click += new System.EventHandler(this.agentB6_Click);
            //
            // agentB5
            //
            this.agentB5.Location = new System.Drawing.Point(8, 173);
            this.agentB5.Name = "agentB5";
            this.agentB5.Size = new System.Drawing.Size(130, 24);
            this.agentB5.TabIndex = 5;
            this.agentB5.Click += new System.EventHandler(this.agentB5_Click);
            //
            // agentList
            //
            this.agentList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.agentList.Location = new System.Drawing.Point(8, 14);
            this.agentList.Name = "agentList";
            this.agentList.Size = new System.Drawing.Size(130, 23);
            this.agentList.TabIndex = 2;
            this.agentList.SelectedIndexChanged += new System.EventHandler(this.agentList_SelectedIndexChanged);
            //
            // agentGroup
            //
            this.agentGroup.Controls.Add(this.agentSubList);
            this.agentGroup.Location = new System.Drawing.Point(144, 3);
            this.agentGroup.Name = "agentGroup";
            this.agentGroup.Size = new System.Drawing.Size(329, 233);
            this.agentGroup.TabIndex = 1;
            this.agentGroup.TabStop = false;
            //
            // agentSubList
            //
            this.agentSubList.IntegralHeight = false;
            this.agentSubList.ItemHeight = 15;
            this.agentSubList.Location = new System.Drawing.Point(8, 16);
            this.agentSubList.Name = "agentSubList";
            this.agentSubList.Size = new System.Drawing.Size(315, 211);
            this.agentSubList.TabIndex = 0;
            this.agentSubList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.agentSubList_MouseDown);
            //
            // agentB4
            //
            this.agentB4.Location = new System.Drawing.Point(8, 140);
            this.agentB4.Name = "agentB4";
            this.agentB4.Size = new System.Drawing.Size(130, 24);
            this.agentB4.TabIndex = 4;
            this.agentB4.Click += new System.EventHandler(this.agentB4_Click);
            //
            // agentB1
            //
            this.agentB1.Location = new System.Drawing.Point(8, 41);
            this.agentB1.Name = "agentB1";
            this.agentB1.Size = new System.Drawing.Size(130, 24);
            this.agentB1.TabIndex = 1;
            this.agentB1.Click += new System.EventHandler(this.agentB1_Click);
            //
            // agentB2
            //
            this.agentB2.Location = new System.Drawing.Point(8, 74);
            this.agentB2.Name = "agentB2";
            this.agentB2.Size = new System.Drawing.Size(130, 24);
            this.agentB2.TabIndex = 2;
            this.agentB2.Click += new System.EventHandler(this.agentB2_Click);
            //
            // agentB3
            //
            this.agentB3.Location = new System.Drawing.Point(8, 107);
            this.agentB3.Name = "agentB3";
            this.agentB3.Size = new System.Drawing.Size(130, 24);
            this.agentB3.TabIndex = 3;
            this.agentB3.Click += new System.EventHandler(this.agentB3_Click);
            //
            // hotkeysTab
            //
            this.hotkeysTab.Controls.Add(this.filterHotkeys);
            this.hotkeysTab.Controls.Add(this.label22);
            this.hotkeysTab.Controls.Add(this.hkStatus);
            this.hotkeysTab.Controls.Add(this.hotkeyTree);
            this.hotkeysTab.Controls.Add(this.dohotkey);
            this.hotkeysTab.Controls.Add(this.groupBox8);
            this.hotkeysTab.Location = new System.Drawing.Point(4, 44);
            this.hotkeysTab.Name = "hotkeysTab";
            this.hotkeysTab.Size = new System.Drawing.Size(482, 449);
            this.hotkeysTab.TabIndex = 4;
            this.hotkeysTab.Text = "Hot Keys";
            //
            // filterHotkeys
            //
            this.filterHotkeys.Location = new System.Drawing.Point(50, 8);
            this.filterHotkeys.Name = "filterHotkeys";
            this.filterHotkeys.Size = new System.Drawing.Size(148, 23);
            this.filterHotkeys.TabIndex = 9;
            this.filterHotkeys.TextChanged += new System.EventHandler(this.filterHotkeys_TextChanged);
            //
            // label22
            //
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(8, 11);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(36, 15);
            this.label22.TabIndex = 8;
            this.label22.Text = "Filter:";
            //
            // hkStatus
            //
            this.hkStatus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hkStatus.Location = new System.Drawing.Point(298, 172);
            this.hkStatus.Name = "hkStatus";
            this.hkStatus.Size = new System.Drawing.Size(175, 64);
            this.hkStatus.TabIndex = 7;
            this.hkStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // hotkeyTree
            //
            this.hotkeyTree.HideSelection = false;
            this.hotkeyTree.Location = new System.Drawing.Point(8, 37);
            this.hotkeyTree.Name = "hotkeyTree";
            this.hotkeyTree.Size = new System.Drawing.Size(284, 199);
            this.hotkeyTree.Sorted = true;
            this.hotkeyTree.TabIndex = 6;
            this.hotkeyTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.hotkeyTree_AfterSelect);
            //
            // dohotkey
            //
            this.dohotkey.Location = new System.Drawing.Point(298, 140);
            this.dohotkey.Name = "dohotkey";
            this.dohotkey.Size = new System.Drawing.Size(175, 29);
            this.dohotkey.TabIndex = 5;
            this.dohotkey.Text = "Execute Selected Hot Key";
            this.dohotkey.Click += new System.EventHandler(this.dohotkey_Click);
            //
            // groupBox8
            //
            this.groupBox8.Controls.Add(this.chkPass);
            this.groupBox8.Controls.Add(this.label2);
            this.groupBox8.Controls.Add(this.unsetHK);
            this.groupBox8.Controls.Add(this.setHK);
            this.groupBox8.Controls.Add(this.key);
            this.groupBox8.Controls.Add(this.chkCtrl);
            this.groupBox8.Controls.Add(this.chkAlt);
            this.groupBox8.Controls.Add(this.chkShift);
            this.groupBox8.Location = new System.Drawing.Point(298, 8);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(175, 124);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Hot Key";
            //
            // chkPass
            //
            this.chkPass.Location = new System.Drawing.Point(11, 69);
            this.chkPass.Name = "chkPass";
            this.chkPass.Size = new System.Drawing.Size(113, 16);
            this.chkPass.TabIndex = 9;
            this.chkPass.Text = "Pass to UO";
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(8, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Key:";
            //
            // unsetHK
            //
            this.unsetHK.Location = new System.Drawing.Point(8, 90);
            this.unsetHK.Name = "unsetHK";
            this.unsetHK.Size = new System.Drawing.Size(56, 26);
            this.unsetHK.TabIndex = 6;
            this.unsetHK.Text = "Unset";
            this.unsetHK.Click += new System.EventHandler(this.unsetHK_Click);
            //
            // setHK
            //
            this.setHK.Location = new System.Drawing.Point(113, 90);
            this.setHK.Name = "setHK";
            this.setHK.Size = new System.Drawing.Size(56, 26);
            this.setHK.TabIndex = 5;
            this.setHK.Text = "Set";
            this.setHK.Click += new System.EventHandler(this.setHK_Click);
            //
            // key
            //
            this.key.Location = new System.Drawing.Point(36, 40);
            this.key.Name = "key";
            this.key.ReadOnly = true;
            this.key.Size = new System.Drawing.Size(133, 23);
            this.key.TabIndex = 4;
            this.key.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.key.KeyUp += new System.Windows.Forms.KeyEventHandler(this.key_KeyUp);
            this.key.MouseDown += new System.Windows.Forms.MouseEventHandler(this.key_MouseDown);
            this.key.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.key_MouseWheel);
            //
            // chkCtrl
            //
            this.chkCtrl.Location = new System.Drawing.Point(8, 20);
            this.chkCtrl.Name = "chkCtrl";
            this.chkCtrl.Size = new System.Drawing.Size(44, 16);
            this.chkCtrl.TabIndex = 1;
            this.chkCtrl.Text = "Ctrl";
            //
            // chkAlt
            //
            this.chkAlt.Location = new System.Drawing.Point(58, 20);
            this.chkAlt.Name = "chkAlt";
            this.chkAlt.Size = new System.Drawing.Size(49, 16);
            this.chkAlt.TabIndex = 2;
            this.chkAlt.Text = "Alt";
            //
            // chkShift
            //
            this.chkShift.Location = new System.Drawing.Point(113, 20);
            this.chkShift.Name = "chkShift";
            this.chkShift.Size = new System.Drawing.Size(56, 16);
            this.chkShift.TabIndex = 3;
            this.chkShift.Text = "Shift";
            //
            // macrosTab
            //
            this.macrosTab.Controls.Add(this.rangeCheckDoubleClick);
            this.macrosTab.Controls.Add(this.rangeCheckTargetByType);
            this.macrosTab.Controls.Add(this.nextMacroAction);
            this.macrosTab.Controls.Add(this.stepThroughMacro);
            this.macrosTab.Controls.Add(this.expandAdvancedMacros);
            this.macrosTab.Controls.Add(this.targetByTypeDifferent);
            this.macrosTab.Controls.Add(this.absoluteTargetGroup);
            this.macrosTab.Controls.Add(this.macroTree);
            this.macrosTab.Controls.Add(this.macroActGroup);
            this.macrosTab.Controls.Add(this.delMacro);
            this.macrosTab.Controls.Add(this.newMacro);
            this.macrosTab.Location = new System.Drawing.Point(4, 44);
            this.macrosTab.Name = "macrosTab";
            this.macrosTab.Size = new System.Drawing.Size(482, 449);
            this.macrosTab.TabIndex = 7;
            this.macrosTab.Text = "Macros";
            //
            // rangeCheckDoubleClick
            //
            this.rangeCheckDoubleClick.AutoSize = true;
            this.rangeCheckDoubleClick.Location = new System.Drawing.Point(267, 315);
            this.rangeCheckDoubleClick.Name = "rangeCheckDoubleClick";
            this.rangeCheckDoubleClick.Size = new System.Drawing.Size(208, 19);
            this.rangeCheckDoubleClick.TabIndex = 10;
            this.rangeCheckDoubleClick.Text = "Range check on \'DoubleClickType\'";
            this.rangeCheckDoubleClick.UseVisualStyleBackColor = true;
            this.rangeCheckDoubleClick.CheckedChanged += new System.EventHandler(this.rangeCheckDoubleClick_CheckedChanged);
            //
            // rangeCheckTargetByType
            //
            this.rangeCheckTargetByType.AutoSize = true;
            this.rangeCheckTargetByType.Location = new System.Drawing.Point(267, 290);
            this.rangeCheckTargetByType.Name = "rangeCheckTargetByType";
            this.rangeCheckTargetByType.Size = new System.Drawing.Size(190, 19);
            this.rangeCheckTargetByType.TabIndex = 9;
            this.rangeCheckTargetByType.Text = "Range check on \'TargetByType\'";
            this.rangeCheckTargetByType.UseVisualStyleBackColor = true;
            this.rangeCheckTargetByType.CheckedChanged += new System.EventHandler(this.rangeCheckTargetByType_CheckedChanged);
            //
            // nextMacroAction
            //
            this.nextMacroAction.Enabled = false;
            this.nextMacroAction.Location = new System.Drawing.Point(407, 352);
            this.nextMacroAction.Name = "nextMacroAction";
            this.nextMacroAction.Size = new System.Drawing.Size(60, 23);
            this.nextMacroAction.TabIndex = 8;
            this.nextMacroAction.Text = "Next";
            this.nextMacroAction.UseVisualStyleBackColor = true;
            this.nextMacroAction.Click += new System.EventHandler(this.nextMacroAction_Click);
            //
            // stepThroughMacro
            //
            this.stepThroughMacro.AutoSize = true;
            this.stepThroughMacro.Location = new System.Drawing.Point(267, 355);
            this.stepThroughMacro.Name = "stepThroughMacro";
            this.stepThroughMacro.Size = new System.Drawing.Size(135, 19);
            this.stepThroughMacro.TabIndex = 7;
            this.stepThroughMacro.Text = "Step Through Macro";
            this.stepThroughMacro.UseVisualStyleBackColor = true;
            this.stepThroughMacro.CheckedChanged += new System.EventHandler(this.stepThroughMacro_CheckedChanged);
            //
            // expandAdvancedMacros
            //
            this.expandAdvancedMacros.Location = new System.Drawing.Point(153, 213);
            this.expandAdvancedMacros.Name = "expandAdvancedMacros";
            this.expandAdvancedMacros.Size = new System.Drawing.Size(21, 20);
            this.expandAdvancedMacros.TabIndex = 6;
            this.expandAdvancedMacros.UseVisualStyleBackColor = true;
            this.expandAdvancedMacros.Click += new System.EventHandler(this.expandAdvancedMacros_Click);
            //
            // targetByTypeDifferent
            //
            this.targetByTypeDifferent.AutoSize = true;
            this.targetByTypeDifferent.Location = new System.Drawing.Point(267, 265);
            this.targetByTypeDifferent.Name = "targetByTypeDifferent";
            this.targetByTypeDifferent.Size = new System.Drawing.Size(183, 19);
            this.targetByTypeDifferent.TabIndex = 6;
            this.targetByTypeDifferent.Text = "Force different \'TargetByType\'";
            this.targetByTypeDifferent.UseVisualStyleBackColor = true;
            this.targetByTypeDifferent.CheckedChanged += new System.EventHandler(this.targetByTypeDifferent_CheckedChanged);
            //
            // absoluteTargetGroup
            //
            this.absoluteTargetGroup.Controls.Add(this.retargetAbsoluteTarget);
            this.absoluteTargetGroup.Controls.Add(this.insertAbsoluteTarget);
            this.absoluteTargetGroup.Controls.Add(this.removeAbsoluteTarget);
            this.absoluteTargetGroup.Controls.Add(this.addAbsoluteTarget);
            this.absoluteTargetGroup.Controls.Add(this.absoluteTargets);
            this.absoluteTargetGroup.Location = new System.Drawing.Point(8, 256);
            this.absoluteTargetGroup.Name = "absoluteTargetGroup";
            this.absoluteTargetGroup.Size = new System.Drawing.Size(240, 185);
            this.absoluteTargetGroup.TabIndex = 5;
            this.absoluteTargetGroup.TabStop = false;
            this.absoluteTargetGroup.Text = "Absolute Target Variables:";
            this.absoluteTargetGroup.Visible = false;
            //
            // retargetAbsoluteTarget
            //
            this.retargetAbsoluteTarget.Location = new System.Drawing.Point(167, 84);
            this.retargetAbsoluteTarget.Name = "retargetAbsoluteTarget";
            this.retargetAbsoluteTarget.Size = new System.Drawing.Size(67, 25);
            this.retargetAbsoluteTarget.TabIndex = 5;
            this.retargetAbsoluteTarget.Text = "Retarget";
            this.retargetAbsoluteTarget.UseVisualStyleBackColor = true;
            this.retargetAbsoluteTarget.Click += new System.EventHandler(this.retargetAbsoluteTarget_Click);
            //
            // insertAbsoluteTarget
            //
            this.insertAbsoluteTarget.Location = new System.Drawing.Point(167, 53);
            this.insertAbsoluteTarget.Name = "insertAbsoluteTarget";
            this.insertAbsoluteTarget.Size = new System.Drawing.Size(67, 25);
            this.insertAbsoluteTarget.TabIndex = 4;
            this.insertAbsoluteTarget.Text = "Insert";
            this.insertAbsoluteTarget.UseVisualStyleBackColor = true;
            this.insertAbsoluteTarget.Click += new System.EventHandler(this.insertAbsoluteTarget_Click);
            //
            // removeAbsoluteTarget
            //
            this.removeAbsoluteTarget.Location = new System.Drawing.Point(167, 115);
            this.removeAbsoluteTarget.Name = "removeAbsoluteTarget";
            this.removeAbsoluteTarget.Size = new System.Drawing.Size(67, 25);
            this.removeAbsoluteTarget.TabIndex = 3;
            this.removeAbsoluteTarget.Text = "Remove";
            this.removeAbsoluteTarget.UseVisualStyleBackColor = true;
            this.removeAbsoluteTarget.Click += new System.EventHandler(this.removeAbsoluteTarget_Click);
            //
            // addAbsoluteTarget
            //
            this.addAbsoluteTarget.Location = new System.Drawing.Point(167, 22);
            this.addAbsoluteTarget.Name = "addAbsoluteTarget";
            this.addAbsoluteTarget.Size = new System.Drawing.Size(67, 25);
            this.addAbsoluteTarget.TabIndex = 2;
            this.addAbsoluteTarget.Text = "Add";
            this.addAbsoluteTarget.UseVisualStyleBackColor = true;
            this.addAbsoluteTarget.Click += new System.EventHandler(this.addAbsoluteTarget_Click);
            //
            // absoluteTargets
            //
            this.absoluteTargets.FormattingEnabled = true;
            this.absoluteTargets.ItemHeight = 15;
            this.absoluteTargets.Location = new System.Drawing.Point(8, 22);
            this.absoluteTargets.Name = "absoluteTargets";
            this.absoluteTargets.Size = new System.Drawing.Size(153, 154);
            this.absoluteTargets.TabIndex = 1;
            //
            // macroTree
            //
            this.macroTree.FullRowSelect = true;
            this.macroTree.HideSelection = false;
            this.macroTree.Location = new System.Drawing.Point(8, 8);
            this.macroTree.Name = "macroTree";
            this.macroTree.Size = new System.Drawing.Size(166, 196);
            this.macroTree.Sorted = true;
            this.macroTree.TabIndex = 4;
            this.macroTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.macroTree_AfterSelect);
            this.macroTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.macroTree_MouseDown);
            //
            // macroActGroup
            //
            this.macroActGroup.Controls.Add(this.waitDisp);
            this.macroActGroup.Controls.Add(this.loopMacro);
            this.macroActGroup.Controls.Add(this.recMacro);
            this.macroActGroup.Controls.Add(this.playMacro);
            this.macroActGroup.Controls.Add(this.actionList);
            this.macroActGroup.Location = new System.Drawing.Point(180, 4);
            this.macroActGroup.Name = "macroActGroup";
            this.macroActGroup.Size = new System.Drawing.Size(293, 232);
            this.macroActGroup.TabIndex = 3;
            this.macroActGroup.TabStop = false;
            this.macroActGroup.Text = "Actions";
            this.macroActGroup.Visible = false;
            //
            // waitDisp
            //
            this.waitDisp.Location = new System.Drawing.Point(227, 110);
            this.waitDisp.Name = "waitDisp";
            this.waitDisp.Size = new System.Drawing.Size(60, 89);
            this.waitDisp.TabIndex = 5;
            this.waitDisp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // loopMacro
            //
            this.loopMacro.Location = new System.Drawing.Point(230, 202);
            this.loopMacro.Name = "loopMacro";
            this.loopMacro.Size = new System.Drawing.Size(57, 24);
            this.loopMacro.TabIndex = 4;
            this.loopMacro.Text = "Loop";
            this.loopMacro.CheckedChanged += new System.EventHandler(this.loopMacro_CheckedChanged);
            //
            // recMacro
            //
            this.recMacro.Location = new System.Drawing.Point(227, 55);
            this.recMacro.Name = "recMacro";
            this.recMacro.Size = new System.Drawing.Size(60, 33);
            this.recMacro.TabIndex = 3;
            this.recMacro.Text = "Record";
            this.recMacro.Click += new System.EventHandler(this.recMacro_Click);
            //
            // playMacro
            //
            this.playMacro.Location = new System.Drawing.Point(227, 16);
            this.playMacro.Name = "playMacro";
            this.playMacro.Size = new System.Drawing.Size(60, 33);
            this.playMacro.TabIndex = 1;
            this.playMacro.Text = "Play";
            this.playMacro.Click += new System.EventHandler(this.playMacro_Click);
            //
            // actionList
            //
            this.actionList.BackColor = System.Drawing.SystemColors.Window;
            this.actionList.HorizontalScrollbar = true;
            this.actionList.IntegralHeight = false;
            this.actionList.ItemHeight = 15;
            this.actionList.Location = new System.Drawing.Point(8, 16);
            this.actionList.Name = "actionList";
            this.actionList.Size = new System.Drawing.Size(213, 210);
            this.actionList.TabIndex = 0;
            this.actionList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.actionList_KeyDown);
            this.actionList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.actionList_MouseDown);
            //
            // delMacro
            //
            this.delMacro.Location = new System.Drawing.Point(74, 210);
            this.delMacro.Name = "delMacro";
            this.delMacro.Size = new System.Drawing.Size(60, 26);
            this.delMacro.TabIndex = 2;
            this.delMacro.Text = "Remove";
            this.delMacro.Click += new System.EventHandler(this.delMacro_Click);
            //
            // newMacro
            //
            this.newMacro.Location = new System.Drawing.Point(8, 210);
            this.newMacro.Name = "newMacro";
            this.newMacro.Size = new System.Drawing.Size(60, 26);
            this.newMacro.TabIndex = 1;
            this.newMacro.Text = "New...";
            this.newMacro.Click += new System.EventHandler(this.newMacro_Click);
            //
            // mapTab
            //
            this.mapTab.BackColor = System.Drawing.SystemColors.Control;
            this.mapTab.Controls.Add(this.captureMibs);
            this.mapTab.Controls.Add(this.boatControl);
            this.mapTab.Controls.Add(this.btnMap);
            this.mapTab.Controls.Add(this.showPlayerPosition);
            this.mapTab.Controls.Add(this.tiltMap);
            this.mapTab.Controls.Add(this.showPartyMemberPositions);
            this.mapTab.Controls.Add(this.openUOPS);
            this.mapTab.Controls.Add(this.groupMapPoints);
            this.mapTab.Controls.Add(this.trackPlayerPosition);
            this.mapTab.Location = new System.Drawing.Point(4, 44);
            this.mapTab.Name = "mapTab";
            this.mapTab.Size = new System.Drawing.Size(482, 449);
            this.mapTab.TabIndex = 13;
            this.mapTab.Text = "Map";
            //
            // captureMibs
            //
            this.captureMibs.AutoSize = true;
            this.captureMibs.Location = new System.Drawing.Point(9, 77);
            this.captureMibs.Name = "captureMibs";
            this.captureMibs.Size = new System.Drawing.Size(236, 19);
            this.captureMibs.TabIndex = 62;
            this.captureMibs.Text = "Capture MIB coordinates when opening";
            this.captureMibs.UseVisualStyleBackColor = true;
            this.captureMibs.CheckedChanged += new System.EventHandler(this.captureMibs_CheckedChanged);
            //
            // boatControl
            //
            this.boatControl.Location = new System.Drawing.Point(145, 14);
            this.boatControl.Name = "boatControl";
            this.boatControl.Size = new System.Drawing.Size(128, 32);
            this.boatControl.TabIndex = 60;
            this.boatControl.Text = "Open Boat Control";
            this.boatControl.UseVisualStyleBackColor = true;
            this.boatControl.Click += new System.EventHandler(this.boatControl_Click);
            //
            // btnMap
            //
            this.btnMap.Location = new System.Drawing.Point(21, 198);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(237, 29);
            this.btnMap.TabIndex = 59;
            this.btnMap.Text = "Open UOPS";
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // showPlayerPosition
            //
            this.showPlayerPosition.AutoSize = true;
            this.showPlayerPosition.Location = new System.Drawing.Point(6, 52);
            this.showPlayerPosition.Name = "showPlayerPosition";
            this.showPlayerPosition.Size = new System.Drawing.Size(172, 19);
            this.showPlayerPosition.TabIndex = 5;
            this.showPlayerPosition.Text = "Show your position on map";
            this.showPlayerPosition.UseVisualStyleBackColor = true;
            this.showPlayerPosition.CheckedChanged += new System.EventHandler(this.showPlayerPosition_CheckedChanged);
            //
            // tiltMap
            //
            this.tiltMap.AutoSize = true;
            this.tiltMap.Location = new System.Drawing.Point(6, 127);
            this.tiltMap.Name = "tiltMap";
            this.tiltMap.Size = new System.Drawing.Size(93, 19);
            this.tiltMap.TabIndex = 4;
            this.tiltMap.Text = "Tilt map 45° ";
            this.tiltMap.UseVisualStyleBackColor = true;
            this.tiltMap.CheckedChanged += new System.EventHandler(this.tiltMap_CheckedChanged);
            //
            // showPartyMemberPositions
            //
            this.showPartyMemberPositions.AutoSize = true;
            this.showPartyMemberPositions.Location = new System.Drawing.Point(6, 102);
            this.showPartyMemberPositions.Name = "showPartyMemberPositions";
            this.showPartyMemberPositions.Size = new System.Drawing.Size(184, 19);
            this.showPartyMemberPositions.TabIndex = 3;
            this.showPartyMemberPositions.Text = "Show party member positions";
            this.showPartyMemberPositions.UseVisualStyleBackColor = true;
            this.showPartyMemberPositions.CheckedChanged += new System.EventHandler(this.showPartyMemberPositions_CheckedChanged);
            //
            // openUOPS
            //
            this.openUOPS.Location = new System.Drawing.Point(5, 14);
            this.openUOPS.Name = "openUOPS";
            this.openUOPS.Size = new System.Drawing.Size(134, 32);
            this.openUOPS.TabIndex = 2;
            this.openUOPS.Text = "Open UOPS 2.0";
            this.openUOPS.UseVisualStyleBackColor = true;
            this.openUOPS.Click += new System.EventHandler(this.openUOPS_Click);
            //
            // groupMapPoints
            //
            this.groupMapPoints.Controls.Add(this.removeMapPoint);
            this.groupMapPoints.Controls.Add(this.addMapPoint);
            this.groupMapPoints.Controls.Add(this.mapPoints);
            this.groupMapPoints.Location = new System.Drawing.Point(279, 3);
            this.groupMapPoints.Name = "groupMapPoints";
            this.groupMapPoints.Size = new System.Drawing.Size(200, 232);
            this.groupMapPoints.TabIndex = 1;
            this.groupMapPoints.TabStop = false;
            this.groupMapPoints.Text = "Map Points";
            //
            // removeMapPoint
            //
            this.removeMapPoint.Location = new System.Drawing.Point(123, 191);
            this.removeMapPoint.Name = "removeMapPoint";
            this.removeMapPoint.Size = new System.Drawing.Size(71, 35);
            this.removeMapPoint.TabIndex = 2;
            this.removeMapPoint.Text = "Remove";
            this.removeMapPoint.UseVisualStyleBackColor = true;
            //
            // addMapPoint
            //
            this.addMapPoint.Location = new System.Drawing.Point(6, 191);
            this.addMapPoint.Name = "addMapPoint";
            this.addMapPoint.Size = new System.Drawing.Size(71, 35);
            this.addMapPoint.TabIndex = 1;
            this.addMapPoint.Text = "Add";
            this.addMapPoint.UseVisualStyleBackColor = true;
            //
            // mapPoints
            //
            this.mapPoints.FormattingEnabled = true;
            this.mapPoints.ItemHeight = 15;
            this.mapPoints.Location = new System.Drawing.Point(6, 16);
            this.mapPoints.Name = "mapPoints";
            this.mapPoints.Size = new System.Drawing.Size(188, 169);
            this.mapPoints.TabIndex = 0;
            //
            // trackPlayerPosition
            //
            this.trackPlayerPosition.AutoSize = true;
            this.trackPlayerPosition.Location = new System.Drawing.Point(6, 77);
            this.trackPlayerPosition.Name = "trackPlayerPosition";
            this.trackPlayerPosition.Size = new System.Drawing.Size(220, 19);
            this.trackPlayerPosition.TabIndex = 0;
            this.trackPlayerPosition.Text = "Auto-track your position on the map";
            this.trackPlayerPosition.UseVisualStyleBackColor = true;
            this.trackPlayerPosition.CheckedChanged += new System.EventHandler(this.trackPlayerPosition_CheckedChanged);
            //
            // videoTab
            //
            this.videoTab.Controls.Add(this.txtRecFolder);
            this.videoTab.Controls.Add(this.recFolder);
            this.videoTab.Controls.Add(this.label13);
            this.videoTab.Controls.Add(this.groupBox7);
            this.videoTab.Controls.Add(this.groupBox10);
            this.videoTab.Controls.Add(this.groupBox9);
            this.videoTab.Location = new System.Drawing.Point(4, 44);
            this.videoTab.Name = "videoTab";
            this.videoTab.Size = new System.Drawing.Size(482, 449);
            this.videoTab.TabIndex = 11;
            this.videoTab.Text = "Video Capture";
            //
            // txtRecFolder
            //
            this.txtRecFolder.Location = new System.Drawing.Point(8, 20);
            this.txtRecFolder.Name = "txtRecFolder";
            this.txtRecFolder.Size = new System.Drawing.Size(198, 23);
            this.txtRecFolder.TabIndex = 16;
            this.txtRecFolder.TextChanged += new System.EventHandler(this.txtRecFolder_TextChanged);
            //
            // recFolder
            //
            this.recFolder.Location = new System.Drawing.Point(212, 20);
            this.recFolder.Name = "recFolder";
            this.recFolder.Size = new System.Drawing.Size(33, 22);
            this.recFolder.TabIndex = 15;
            this.recFolder.Text = "...";
            this.recFolder.Click += new System.EventHandler(this.recFolder_Click);
            //
            // label13
            //
            this.label13.Location = new System.Drawing.Point(8, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 16);
            this.label13.TabIndex = 17;
            this.label13.Text = "Recordings Folder:";
            //
            // groupBox7
            //
            this.groupBox7.Controls.Add(this.vidRec);
            this.groupBox7.Location = new System.Drawing.Point(8, 49);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(237, 58);
            this.groupBox7.TabIndex = 12;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "PacketVideo Recording";
            //
            // vidRec
            //
            this.vidRec.Location = new System.Drawing.Point(10, 20);
            this.vidRec.Name = "vidRec";
            this.vidRec.Size = new System.Drawing.Size(221, 28);
            this.vidRec.TabIndex = 1;
            this.vidRec.Text = "Record PacketVideo";
            this.vidRec.Click += new System.EventHandler(this.vidRec_Click);
            //
            // groupBox10
            //
            this.groupBox10.Controls.Add(this.flipVidVert);
            this.groupBox10.Controls.Add(this.flipVidHoriz);
            this.groupBox10.Controls.Add(this.recAVI);
            this.groupBox10.Controls.Add(this.aviRes);
            this.groupBox10.Controls.Add(this.aviFPS);
            this.groupBox10.Controls.Add(this.label16);
            this.groupBox10.Controls.Add(this.label15);
            this.groupBox10.Controls.Add(this.label19);
            this.groupBox10.Location = new System.Drawing.Point(8, 113);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(237, 123);
            this.groupBox10.TabIndex = 14;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "AVI Video Recording";
            //
            // flipVidVert
            //
            this.flipVidVert.Location = new System.Drawing.Point(134, 54);
            this.flipVidVert.Name = "flipVidVert";
            this.flipVidVert.Size = new System.Drawing.Size(74, 16);
            this.flipVidVert.TabIndex = 6;
            this.flipVidVert.Text = "Vertical";
            this.flipVidVert.CheckedChanged += new System.EventHandler(this.flipVidVert_CheckedChanged);
            //
            // flipVidHoriz
            //
            this.flipVidHoriz.Location = new System.Drawing.Point(46, 54);
            this.flipVidHoriz.Name = "flipVidHoriz";
            this.flipVidHoriz.Size = new System.Drawing.Size(82, 16);
            this.flipVidHoriz.TabIndex = 5;
            this.flipVidHoriz.Text = "Horizontal";
            this.flipVidHoriz.CheckedChanged += new System.EventHandler(this.flipVidHoriz_CheckedChanged);
            //
            // recAVI
            //
            this.recAVI.Location = new System.Drawing.Point(6, 75);
            this.recAVI.Name = "recAVI";
            this.recAVI.Size = new System.Drawing.Size(225, 33);
            this.recAVI.TabIndex = 4;
            this.recAVI.Text = "Record AVI Video...";
            this.recAVI.Click += new System.EventHandler(this.recAVI_Click);
            //
            // aviRes
            //
            this.aviRes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.aviRes.Items.AddRange(new object[] {
            "Full Size",
            "3/4",
            "1/2",
            "1/4"});
            this.aviRes.Location = new System.Drawing.Point(150, 18);
            this.aviRes.Name = "aviRes";
            this.aviRes.Size = new System.Drawing.Size(81, 23);
            this.aviRes.TabIndex = 3;
            this.aviRes.SelectedIndexChanged += new System.EventHandler(this.aviRes_SelectedIndexChanged);
            //
            // aviFPS
            //
            this.aviFPS.Location = new System.Drawing.Point(38, 19);
            this.aviFPS.Name = "aviFPS";
            this.aviFPS.Size = new System.Drawing.Size(26, 23);
            this.aviFPS.TabIndex = 1;
            this.aviFPS.TextChanged += new System.EventHandler(this.aviFPS_TextChanged);
            //
            // label16
            //
            this.label16.Location = new System.Drawing.Point(70, 18);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(74, 21);
            this.label16.TabIndex = 2;
            this.label16.Text = "Resolution:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label15
            //
            this.label15.Location = new System.Drawing.Point(7, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(30, 16);
            this.label15.TabIndex = 0;
            this.label15.Text = "FPS:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label19
            //
            this.label19.Location = new System.Drawing.Point(7, 52);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(50, 18);
            this.label19.TabIndex = 7;
            this.label19.Text = "Flip:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // groupBox9
            //
            this.groupBox9.Controls.Add(this.rpvTime);
            this.groupBox9.Controls.Add(this.playSpeed);
            this.groupBox9.Controls.Add(this.label14);
            this.groupBox9.Controls.Add(this.vidClose);
            this.groupBox9.Controls.Add(this.playPos);
            this.groupBox9.Controls.Add(this.vidPlayStop);
            this.groupBox9.Controls.Add(this.vidPlay);
            this.groupBox9.Controls.Add(this.vidPlayInfo);
            this.groupBox9.Controls.Add(this.vidOpen);
            this.groupBox9.Location = new System.Drawing.Point(251, 3);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(228, 233);
            this.groupBox9.TabIndex = 13;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "PacketVideo Playback";
            //
            // rpvTime
            //
            this.rpvTime.Location = new System.Drawing.Point(10, 96);
            this.rpvTime.Name = "rpvTime";
            this.rpvTime.Size = new System.Drawing.Size(212, 21);
            this.rpvTime.TabIndex = 8;
            this.rpvTime.Text = "00:00/00:00";
            //
            // playSpeed
            //
            this.playSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playSpeed.Items.AddRange(new object[] {
            "1/4",
            "1/2",
            "Reg",
            "2x",
            "4x"});
            this.playSpeed.Location = new System.Drawing.Point(178, 46);
            this.playSpeed.Name = "playSpeed";
            this.playSpeed.Size = new System.Drawing.Size(44, 23);
            this.playSpeed.TabIndex = 7;
            this.playSpeed.SelectedIndexChanged += new System.EventHandler(this.playSpeed_SelectedIndexChanged);
            //
            // label14
            //
            this.label14.Location = new System.Drawing.Point(129, 49);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 16);
            this.label14.TabIndex = 6;
            this.label14.Text = "Speed:";
            //
            // vidClose
            //
            this.vidClose.Enabled = false;
            this.vidClose.Location = new System.Drawing.Point(132, 18);
            this.vidClose.Name = "vidClose";
            this.vidClose.Size = new System.Drawing.Size(90, 20);
            this.vidClose.TabIndex = 5;
            this.vidClose.Text = "Close";
            this.vidClose.Click += new System.EventHandler(this.vidClose_Click);
            //
            // playPos
            //
            this.playPos.AutoSize = false;
            this.playPos.Location = new System.Drawing.Point(8, 73);
            this.playPos.Maximum = 1;
            this.playPos.Name = "playPos";
            this.playPos.Size = new System.Drawing.Size(214, 20);
            this.playPos.TabIndex = 4;
            this.playPos.TickFrequency = 5;
            this.playPos.TickStyle = System.Windows.Forms.TickStyle.None;
            this.playPos.Scroll += new System.EventHandler(this.playPos_Scroll);
            //
            // vidPlayStop
            //
            this.vidPlayStop.Enabled = false;
            this.vidPlayStop.Location = new System.Drawing.Point(63, 46);
            this.vidPlayStop.Name = "vidPlayStop";
            this.vidPlayStop.Size = new System.Drawing.Size(46, 21);
            this.vidPlayStop.TabIndex = 3;
            this.vidPlayStop.Text = "Stop";
            this.vidPlayStop.Click += new System.EventHandler(this.vidPlayStop_Click);
            //
            // vidPlay
            //
            this.vidPlay.Enabled = false;
            this.vidPlay.Location = new System.Drawing.Point(10, 46);
            this.vidPlay.Name = "vidPlay";
            this.vidPlay.Size = new System.Drawing.Size(46, 21);
            this.vidPlay.TabIndex = 2;
            this.vidPlay.Text = "Play";
            this.vidPlay.Click += new System.EventHandler(this.vidPlay_Click);
            //
            // vidPlayInfo
            //
            this.vidPlayInfo.Location = new System.Drawing.Point(10, 112);
            this.vidPlayInfo.Name = "vidPlayInfo";
            this.vidPlayInfo.Size = new System.Drawing.Size(212, 106);
            this.vidPlayInfo.TabIndex = 1;
            //
            // vidOpen
            //
            this.vidOpen.Location = new System.Drawing.Point(10, 18);
            this.vidOpen.Name = "vidOpen";
            this.vidOpen.Size = new System.Drawing.Size(99, 22);
            this.vidOpen.TabIndex = 0;
            this.vidOpen.Text = "Open...";
            this.vidOpen.Click += new System.EventHandler(this.vidOpen_Click);
            //
            // screenshotTab
            //
            this.screenshotTab.Controls.Add(this.imgurUploads);
            this.screenshotTab.Controls.Add(this.screenShotClipboard);
            this.screenshotTab.Controls.Add(this.screenShotNotification);
            this.screenshotTab.Controls.Add(this.screenShotOpenBrowser);
            this.screenshotTab.Controls.Add(this.imgFmt);
            this.screenshotTab.Controls.Add(this.label12);
            this.screenshotTab.Controls.Add(this.capNow);
            this.screenshotTab.Controls.Add(this.screenPath);
            this.screenshotTab.Controls.Add(this.radioUO);
            this.screenshotTab.Controls.Add(this.radioFull);
            this.screenshotTab.Controls.Add(this.screenAutoCap);
            this.screenshotTab.Controls.Add(this.setScnPath);
            this.screenshotTab.Controls.Add(this.screensList);
            this.screenshotTab.Controls.Add(this.screenPrev);
            this.screenshotTab.Controls.Add(this.dispTime);
            this.screenshotTab.Location = new System.Drawing.Point(4, 44);
            this.screenshotTab.Name = "screenshotTab";
            this.screenshotTab.Size = new System.Drawing.Size(482, 449);
            this.screenshotTab.TabIndex = 8;
            this.screenshotTab.Text = "Screen Shots";
            //
            // imgurUploads
            //
            this.imgurUploads.FormattingEnabled = true;
            this.imgurUploads.ItemHeight = 15;
            this.imgurUploads.Location = new System.Drawing.Point(224, 244);
            this.imgurUploads.Name = "imgurUploads";
            this.imgurUploads.Size = new System.Drawing.Size(246, 94);
            this.imgurUploads.TabIndex = 15;
            this.imgurUploads.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgurUploads_MouseDown);
            //
            // screenShotClipboard
            //
            this.screenShotClipboard.Location = new System.Drawing.Point(12, 283);
            this.screenShotClipboard.Name = "screenShotClipboard";
            this.screenShotClipboard.Size = new System.Drawing.Size(239, 20);
            this.screenShotClipboard.TabIndex = 14;
            this.screenShotClipboard.Text = "Copy link to clipboard after upload";
            this.screenShotClipboard.CheckedChanged += new System.EventHandler(this.screenShotClipboard_CheckedChanged);
            //
            // screenShotNotification
            //
            this.screenShotNotification.Location = new System.Drawing.Point(12, 257);
            this.screenShotNotification.Name = "screenShotNotification";
            this.screenShotNotification.Size = new System.Drawing.Size(216, 20);
            this.screenShotNotification.TabIndex = 13;
            this.screenShotNotification.Text = "Show upload notification";
            this.screenShotNotification.CheckedChanged += new System.EventHandler(this.screenShotNotification_CheckedChanged);
            //
            // screenShotOpenBrowser
            //
            this.screenShotOpenBrowser.Location = new System.Drawing.Point(12, 309);
            this.screenShotOpenBrowser.Name = "screenShotOpenBrowser";
            this.screenShotOpenBrowser.Size = new System.Drawing.Size(216, 20);
            this.screenShotOpenBrowser.TabIndex = 12;
            this.screenShotOpenBrowser.Text = "Open link in browser when shared";
            this.screenShotOpenBrowser.CheckedChanged += new System.EventHandler(this.screenShotOpenBrowser_CheckedChanged);
            //
            // imgFmt
            //
            this.imgFmt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imgFmt.Items.AddRange(new object[] {
            "jpg",
            "png",
            "bmp",
            "gif",
            "tif",
            "wmf",
            "exif",
            "emf"});
            this.imgFmt.Location = new System.Drawing.Point(102, 134);
            this.imgFmt.Name = "imgFmt";
            this.imgFmt.Size = new System.Drawing.Size(98, 23);
            this.imgFmt.TabIndex = 11;
            this.imgFmt.SelectedIndexChanged += new System.EventHandler(this.imgFmt_SelectedIndexChanged);
            //
            // label12
            //
            this.label12.Location = new System.Drawing.Point(5, 134);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(92, 20);
            this.label12.TabIndex = 10;
            this.label12.Text = "Image Format:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // capNow
            //
            this.capNow.Location = new System.Drawing.Point(329, 9);
            this.capNow.Name = "capNow";
            this.capNow.Size = new System.Drawing.Size(141, 22);
            this.capNow.TabIndex = 8;
            this.capNow.Text = "Take Screen Shot Now";
            this.capNow.Click += new System.EventHandler(this.capNow_Click);
            //
            // screenPath
            //
            this.screenPath.Location = new System.Drawing.Point(8, 8);
            this.screenPath.Name = "screenPath";
            this.screenPath.Size = new System.Drawing.Size(274, 23);
            this.screenPath.TabIndex = 7;
            this.screenPath.TextChanged += new System.EventHandler(this.screenPath_TextChanged);
            //
            // radioUO
            //
            this.radioUO.Location = new System.Drawing.Point(12, 163);
            this.radioUO.Name = "radioUO";
            this.radioUO.Size = new System.Drawing.Size(75, 26);
            this.radioUO.TabIndex = 6;
            this.radioUO.Text = "UO Only";
            this.radioUO.CheckedChanged += new System.EventHandler(this.radioUO_CheckedChanged);
            //
            // radioFull
            //
            this.radioFull.Location = new System.Drawing.Point(102, 163);
            this.radioFull.Name = "radioFull";
            this.radioFull.Size = new System.Drawing.Size(88, 26);
            this.radioFull.TabIndex = 5;
            this.radioFull.Text = "Full Screen";
            this.radioFull.CheckedChanged += new System.EventHandler(this.radioFull_CheckedChanged);
            //
            // screenAutoCap
            //
            this.screenAutoCap.Location = new System.Drawing.Point(12, 221);
            this.screenAutoCap.Name = "screenAutoCap";
            this.screenAutoCap.Size = new System.Drawing.Size(180, 20);
            this.screenAutoCap.TabIndex = 4;
            this.screenAutoCap.Text = "Auto Death Screen Capture";
            this.screenAutoCap.CheckedChanged += new System.EventHandler(this.screenAutoCap_CheckedChanged);
            //
            // setScnPath
            //
            this.setScnPath.Location = new System.Drawing.Point(288, 9);
            this.setScnPath.Name = "setScnPath";
            this.setScnPath.Size = new System.Drawing.Size(35, 22);
            this.setScnPath.TabIndex = 3;
            this.setScnPath.Text = "...";
            this.setScnPath.Click += new System.EventHandler(this.setScnPath_Click);
            //
            // screensList
            //
            this.screensList.IntegralHeight = false;
            this.screensList.ItemHeight = 15;
            this.screensList.Location = new System.Drawing.Point(8, 36);
            this.screensList.Name = "screensList";
            this.screensList.Size = new System.Drawing.Size(210, 92);
            this.screensList.Sorted = true;
            this.screensList.TabIndex = 1;
            this.screensList.SelectedIndexChanged += new System.EventHandler(this.screensList_SelectedIndexChanged);
            this.screensList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screensList_MouseDown);
            //
            // screenPrev
            //
            this.screenPrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.screenPrev.Location = new System.Drawing.Point(224, 37);
            this.screenPrev.Name = "screenPrev";
            this.screenPrev.Size = new System.Drawing.Size(246, 199);
            this.screenPrev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.screenPrev.TabIndex = 0;
            this.screenPrev.TabStop = false;
            this.screenPrev.Click += new System.EventHandler(this.screenPrev_Click);
            //
            // dispTime
            //
            this.dispTime.Location = new System.Drawing.Point(12, 195);
            this.dispTime.Name = "dispTime";
            this.dispTime.Size = new System.Drawing.Size(206, 20);
            this.dispTime.TabIndex = 9;
            this.dispTime.Text = "Include Timestamp on images";
            this.dispTime.CheckedChanged += new System.EventHandler(this.dispTime_CheckedChanged);
            //
            // advancedTab
            //
            this.advancedTab.BackColor = System.Drawing.SystemColors.Control;
            this.advancedTab.Controls.Add(this.disableSmartCPU);
            this.advancedTab.Controls.Add(this.negotiate);
            this.advancedTab.Controls.Add(this.backupDataDir);
            this.advancedTab.Controls.Add(this.openRazorDataDir);
            this.advancedTab.Controls.Add(this.msglvl);
            this.advancedTab.Controls.Add(this.label17);
            this.advancedTab.Controls.Add(this.logPackets);
            this.advancedTab.Controls.Add(this.statusBox);
            this.advancedTab.Controls.Add(this.features);
            this.advancedTab.Location = new System.Drawing.Point(4, 44);
            this.advancedTab.Name = "advancedTab";
            this.advancedTab.Size = new System.Drawing.Size(482, 449);
            this.advancedTab.TabIndex = 12;
            this.advancedTab.Text = "Advanced";
            //
            // disableSmartCPU
            //
            this.disableSmartCPU.Location = new System.Drawing.Point(353, 145);
            this.disableSmartCPU.Name = "disableSmartCPU";
            this.disableSmartCPU.Size = new System.Drawing.Size(119, 21);
            this.disableSmartCPU.TabIndex = 74;
            this.disableSmartCPU.Text = "Disable SmartCPU";
            this.disableSmartCPU.UseVisualStyleBackColor = true;
            this.disableSmartCPU.Click += new System.EventHandler(this.disableSmartCPU_Click);
            //
            // negotiate
            //
            this.negotiate.Location = new System.Drawing.Point(202, 94);
            this.negotiate.Name = "negotiate";
            this.negotiate.Size = new System.Drawing.Size(197, 20);
            this.negotiate.TabIndex = 72;
            this.negotiate.Text = "Negotiate features with server";
            this.negotiate.CheckedChanged += new System.EventHandler(this.negotiate_CheckedChanged);
            //
            // backupDataDir
            //
            this.backupDataDir.Location = new System.Drawing.Point(202, 39);
            this.backupDataDir.Name = "backupDataDir";
            this.backupDataDir.Size = new System.Drawing.Size(271, 25);
            this.backupDataDir.TabIndex = 71;
            this.backupDataDir.Text = "Backup Profiles and Macros";
            this.backupDataDir.UseVisualStyleBackColor = true;
            this.backupDataDir.Click += new System.EventHandler(this.backupDataDir_Click);
            //
            // openRazorDataDir
            //
            this.openRazorDataDir.Location = new System.Drawing.Point(202, 8);
            this.openRazorDataDir.Name = "openRazorDataDir";
            this.openRazorDataDir.Size = new System.Drawing.Size(271, 25);
            this.openRazorDataDir.TabIndex = 70;
            this.openRazorDataDir.Text = "Open Data Directory";
            this.openRazorDataDir.UseVisualStyleBackColor = true;
            this.openRazorDataDir.Click += new System.EventHandler(this.openRazorDataDir_Click);
            //
            // msglvl
            //
            this.msglvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.msglvl.Items.AddRange(new object[] {
            "Show All",
            "Warnings & Errors",
            "Errors Only",
            "None"});
            this.msglvl.Location = new System.Drawing.Point(304, 116);
            this.msglvl.Name = "msglvl";
            this.msglvl.Size = new System.Drawing.Size(144, 23);
            this.msglvl.TabIndex = 69;
            this.msglvl.SelectedIndexChanged += new System.EventHandler(this.msglvl_SelectedIndexChanged);
            //
            // label17
            //
            this.label17.Location = new System.Drawing.Point(199, 117);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(99, 18);
            this.label17.TabIndex = 68;
            this.label17.Text = "Razor messages:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // logPackets
            //
            this.logPackets.Location = new System.Drawing.Point(202, 70);
            this.logPackets.Name = "logPackets";
            this.logPackets.Size = new System.Drawing.Size(146, 23);
            this.logPackets.TabIndex = 67;
            this.logPackets.Text = "Enable packet logging";
            this.logPackets.CheckedChanged += new System.EventHandler(this.logPackets_CheckedChanged);
            //
            // statusBox
            //
            this.statusBox.BackColor = System.Drawing.SystemColors.Control;
            this.statusBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.HideSelection = false;
            this.statusBox.Location = new System.Drawing.Point(7, 8);
            this.statusBox.Multiline = true;
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            this.statusBox.Size = new System.Drawing.Size(185, 228);
            this.statusBox.TabIndex = 66;
            //
            // features
            //
            this.features.Cursor = System.Windows.Forms.Cursors.No;
            this.features.Location = new System.Drawing.Point(202, 173);
            this.features.Multiline = true;
            this.features.Name = "features";
            this.features.ReadOnly = true;
            this.features.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.features.Size = new System.Drawing.Size(274, 63);
            this.features.TabIndex = 65;
            this.features.Visible = false;
            //
            // aboutTab
            //
            this.aboutTab.Controls.Add(this.label24);
            this.aboutTab.Controls.Add(this.label20);
            this.aboutTab.Controls.Add(this.linkLabel1);
            this.aboutTab.Controls.Add(this.label23);
            this.aboutTab.Controls.Add(this.aboutSubInfo);
            this.aboutTab.Controls.Add(this.linkGithub);
            this.aboutTab.Controls.Add(this.label21);
            this.aboutTab.Controls.Add(this.aboutVer);
            this.aboutTab.Location = new System.Drawing.Point(4, 44);
            this.aboutTab.Name = "aboutTab";
            this.aboutTab.Size = new System.Drawing.Size(482, 449);
            this.aboutTab.TabIndex = 9;
            this.aboutTab.Text = "About";
            //
            // label24
            //
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(55, 155);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(384, 17);
            this.label24.TabIndex = 22;
            this.label24.Text = "Razor mapping tool designed and written by Jimmy the Hand";
            //
            // label20
            //
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(77, 193);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(343, 17);
            this.label20.TabIndex = 21;
            this.label20.Text = "For feedback, support and the latest releases please visit:\r\n";
            //
            // linkLabel1
            //
            this.linkLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(12, 76);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(459, 20);
            this.linkLabel1.TabIndex = 20;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.uorenaissance.com";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            //
            // label23
            //
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(33, 135);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(422, 17);
            this.label23.TabIndex = 19;
            this.label23.Text = "Razor was designed by Zippy, modified for UO Renaissance by Quick";
            //
            // aboutSubInfo
            //
            this.aboutSubInfo.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutSubInfo.Location = new System.Drawing.Point(8, 45);
            this.aboutSubInfo.Name = "aboutSubInfo";
            this.aboutSubInfo.Size = new System.Drawing.Size(463, 19);
            this.aboutSubInfo.TabIndex = 17;
            this.aboutSubInfo.Text = "UO Renaissance Community Edition";
            this.aboutSubInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // linkGithub
            //
            this.linkGithub.AutoSize = true;
            this.linkGithub.Location = new System.Drawing.Point(162, 216);
            this.linkGithub.Name = "linkGithub";
            this.linkGithub.Size = new System.Drawing.Size(148, 15);
            this.linkGithub.TabIndex = 16;
            this.linkGithub.TabStop = true;
            this.linkGithub.Text = "http://www.uor-razor.com";
            this.linkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            //
            // label21
            //
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(52, 98);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(0, 15);
            this.label21.TabIndex = 15;
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // aboutVer
            //
            this.aboutVer.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutVer.Location = new System.Drawing.Point(12, 10);
            this.aboutVer.Name = "aboutVer";
            this.aboutVer.Size = new System.Drawing.Size(459, 35);
            this.aboutVer.TabIndex = 14;
            this.aboutVer.Text = "Razor v{0}";
            this.aboutVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // timerTimer
            //
            this.timerTimer.Enabled = true;
            this.timerTimer.Interval = 5;
            this.timerTimer.Tick += new System.EventHandler(this.timerTimer_Tick);
            //
            // MainForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(541, 450);
            this.Controls.Add(this.tabs);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Razor v{0}";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabs.ResumeLayout(false);
            this.generalTab.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.moreOptTab.ResumeLayout(false);
            this.moreOptTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lightLevelBar)).EndInit();
            this.moreMoreOptTab.ResumeLayout(false);
            this.moreMoreOptTab.PerformLayout();
            this.displayTab.ResumeLayout(false);
            this.displayTab.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.dressTab.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.skillsTab.ResumeLayout(false);
            this.skillsTab.PerformLayout();
            this.agentsTab.ResumeLayout(false);
            this.agentGroup.ResumeLayout(false);
            this.hotkeysTab.ResumeLayout(false);
            this.hotkeysTab.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.macrosTab.ResumeLayout(false);
            this.macrosTab.PerformLayout();
            this.absoluteTargetGroup.ResumeLayout(false);
            this.macroActGroup.ResumeLayout(false);
            this.mapTab.ResumeLayout(false);
            this.mapTab.PerformLayout();
            this.groupMapPoints.ResumeLayout(false);
            this.videoTab.ResumeLayout(false);
            this.videoTab.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playPos)).EndInit();
            this.screenshotTab.ResumeLayout(false);
            this.screenshotTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenPrev)).EndInit();
            this.advancedTab.ResumeLayout(false);
            this.advancedTab.PerformLayout();
            this.aboutTab.ResumeLayout(false);
            this.aboutTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == ClientCommunication.WM_UONETEVENT)
                msg.Result = (IntPtr)(ClientCommunication.OnMessage(this, (uint)msg.WParam.ToInt32(), msg.LParam.ToInt32()) ? 1 : 0);
            else if (msg.Msg >= (int)ClientCommunication.UOAMessage.First && msg.Msg <= (int)ClientCommunication.UOAMessage.Last)
                msg.Result = (IntPtr)ClientCommunication.OnUOAMessage(this, msg.Msg, msg.WParam.ToInt32(), msg.LParam.ToInt32());
            else
                base.WndProc(ref msg);
        }

        private void DisableCloseButton()
        {
            IntPtr menu = GetSystemMenu(this.Handle, false);
            EnableMenuItem(menu, 0xF060, 0x00000002); //menu, SC_CLOSE, MF_BYCOMMAND|MF_GRAYED
            m_CanClose = false;
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            //ClientCommunication.SetCustomNotoHue( 0x2 );

            Timer.Control = timerTimer;

            new StatsTimer(this).Start();

            this.Hide();
            Language.LoadControlNames(this);

            bool st = Config.GetBool("Systray");
            taskbar.Checked = this.ShowInTaskbar = !st;
            systray.Checked = m_NotifyIcon.Visible = st;

            //this.Text = String.Format( this.Text, Engine.Version );
            UpdateTitle();

            if (!ClientCommunication.InstallHooks(this.Handle)) // WaitForInputIdle done here
            {
                m_CanClose = true;
                SplashScreen.End();
                this.Close();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            SplashScreen.Message = LocString.Welcome;
            InitConfig();

            this.Show();
            this.BringToFront();

            Engine.ActiveWindow = this;

            DisableCloseButton();

            tabs_IndexChanged(this, null); // load first tab

            m_ProfileConfirmLoad = false;
            Config.SetupProfilesList(profiles, Config.CurrentProfile.Name);
            m_ProfileConfirmLoad = true;

            showWelcome.Checked = Config.GetAppSetting<int>("ShowWelcome") == 1;

            m_Tip.Active = true;
            m_Tip.SetToolTip(titleStr, Language.GetString(LocString.TitleBarTip));

            SplashScreen.End();
	    }

        private bool m_Initializing = false;

        public void InitConfig()
        {
            m_Initializing = true;

            this.opacity.AutoSize = false;
            //this.opacity.Size = new System.Drawing.Size(156, 16);

            opacity.Value = Config.GetInt("Opacity");
            this.Opacity = ((float)opacity.Value) / 100.0;
            opacityLabel.Text = Language.Format(LocString.OpacityA1, opacity.Value);

            this.TopMost = alwaysTop.Checked = Config.GetBool("AlwaysOnTop");
            this.Location = new System.Drawing.Point(Config.GetInt("WindowX"), Config.GetInt("WindowY"));
            this.TopLevel = true;

            bool onScreen = false;
            foreach (Screen s in Screen.AllScreens)
            {
                if (s.Bounds.Contains(this.Location.X + this.Width, this.Location.Y + this.Height) ||
                    s.Bounds.Contains(this.Location.X - this.Width, this.Location.Y - this.Height))
                {
                    onScreen = true;
                    break;
                }
            }

            if (!onScreen)
                this.Location = Point.Empty;

            spellUnequip.Checked = Config.GetBool("SpellUnequip");
            ltRange.Enabled = rangeCheckLT.Checked = Config.GetBool("RangeCheckLT");
            ltRange.Text = Config.GetInt("LTRange").ToString();

            counters.BeginUpdate();
            if (Config.GetBool("SortCounters"))
            {
                counters.Sorting = SortOrder.None;
                counters.ListViewItemSorter = CounterLVIComparer.Instance;
                counters.Sort();
            }
            else
            {
                counters.ListViewItemSorter = null;
                counters.Sorting = SortOrder.Ascending;
            }

            counters.EndUpdate();
            counters.Refresh();

            incomingMob.Checked = Config.GetBool("ShowMobNames");
            incomingCorpse.Checked = Config.GetBool("ShowCorpseNames");
            checkNewConts.Checked = Config.GetBool("AutoSearch");
            excludePouches.Checked = Config.GetBool("NoSearchPouches");
            excludePouches.Enabled = checkNewConts.Checked;
            warnNum.Enabled = warnCount.Checked = Config.GetBool("CounterWarn");
            warnNum.Text = Config.GetInt("CounterWarnAmount").ToString();
            QueueActions.Checked = Config.GetBool("QueueActions");
            queueTargets.Checked = Config.GetBool("QueueTargets");
            chkForceSpeechHue.Checked = setSpeechHue.Enabled = Config.GetBool("ForceSpeechHue");
            chkForceSpellHue.Checked = setBeneHue.Enabled =
                setNeuHue.Enabled = setHarmHue.Enabled = Config.GetBool("ForceSpellHue");
            if (Config.GetInt("LTHilight") != 0)
            {
                InitPreviewHue(lthilight, "LTHilight");
                //ClientCommunication.SetCustomNotoHue( Config.GetInt( "LTHilight" ) );
                lthilight.Checked = setLTHilight.Enabled = true;
            }
            else
            {
                //ClientCommunication.SetCustomNotoHue( 0 );
                lthilight.Checked = setLTHilight.Enabled = false;
            }

            txtSpellFormat.Text = Config.GetString("SpellFormat");
            txtObjDelay.Text = Config.GetInt("ObjectDelay").ToString();
            chkStealth.Checked = Config.GetBool("CountStealthSteps");

            spamFilter.Checked = Config.GetBool("FilterSpam");
            screenAutoCap.Checked = Config.GetBool("AutoCap");
            radioUO.Checked = !(radioFull.Checked = Config.GetBool("CapFullScreen"));
            screenPath.Text = Config.GetString("CapPath");
            dispTime.Checked = Config.GetBool("CapTimeStamp");
            blockDis.Checked = Config.GetBool("BlockDismount");
            alwaysStealth.Checked = Config.GetBool("AlwaysStealth");
            autoOpenDoors.Checked = Config.GetBool("AutoOpenDoors");

            objectDelay.Checked = Config.GetBool("ObjectDelayEnabled");
            txtObjDelay.Enabled = Config.GetBool("ObjectDelayEnabled");

            msglvl.SelectedIndex = Config.GetInt("MessageLevel");

            try
            {
                imgFmt.SelectedItem = Config.GetString("ImageFormat");
            }
            catch
            {
                imgFmt.SelectedIndex = 0;
                Config.SetProperty("ImageFormat", "jpg");
            }

            PacketPlayer.SetControls(vidPlayInfo, vidRec, vidPlay, vidPlayStop, vidClose, playPos, rpvTime);
            txtRecFolder.Text = Config.GetString("RecFolder");
            aviFPS.Text = Config.GetInt("AviFPS").ToString();
            aviRes.SelectedIndex = Config.GetInt("AviRes");
            playSpeed.SelectedIndex = 2;

            InitPreviewHue(lblExHue, "ExemptColor");
            InitPreviewHue(lblMsgHue, "SysColor");
            InitPreviewHue(lblWarnHue, "WarningColor");
            InitPreviewHue(chkForceSpeechHue, "SpeechHue");
            InitPreviewHue(lblBeneHue, "BeneficialSpellHue");
            InitPreviewHue(lblHarmHue, "HarmfulSpellHue");
            InitPreviewHue(lblNeuHue, "NeutralSpellHue");

            undressConflicts.Checked = Config.GetBool("UndressConflicts");
            taskbar.Checked = !(systray.Checked = Config.GetBool("Systray"));
            titlebarImages.Checked = Config.GetBool("TitlebarImages");
            highlightSpellReags.Checked = Config.GetBool("HighlightReagents");

            dispDelta.Checked = Config.GetBool("DisplaySkillChanges");
            titleStr.Enabled = showInBar.Checked = Config.GetBool("TitleBarDisplay");
            titleStr.Text = Config.GetString("TitleBarText");

            showNotoHue.Checked = Config.GetBool("ShowNotoHue");

            corpseRange.Enabled = openCorpses.Checked = Config.GetBool("AutoOpenCorpses");
            corpseRange.Text = Config.GetInt("CorpseRange").ToString();

            actionStatusMsg.Checked = Config.GetBool("ActionStatusMsg");
            autoStackRes.Checked = Config.GetBool("AutoStack");

            rememberPwds.Checked = Config.GetBool("RememberPwds");
            filterSnoop.Checked = Config.GetBool("FilterSnoopMsg");

            preAOSstatbar.Checked = Config.GetBool("OldStatBar");
            showtargtext.Checked = Config.GetBool("LastTargTextFlags");
            smartLT.Checked = Config.GetBool("SmartLastTarget");

            autoFriend.Checked = Config.GetBool("AutoFriend");

            flipVidHoriz.Checked = Config.GetBool("FlipVidH");
            flipVidVert.Checked = Config.GetBool("FlipVidV");

            try
            {
                clientPrio.SelectedItem = Config.GetString("ClientPrio");
            }
            catch
            {
                clientPrio.SelectedItem = "Normal";
            }

            forceSizeX.Text = Config.GetInt("ForceSizeX").ToString();
            forceSizeY.Text = Config.GetInt("ForceSizeY").ToString();

            gameSize.Checked = Config.GetBool("ForceSizeEnabled");

            potionEquip.Checked = Config.GetBool("PotionEquip");
            blockHealPoison.Checked = Config.GetBool("BlockHealPoison");

            negotiate.Checked = Config.GetBool("Negotiate");

            logPackets.Checked = Config.GetBool("LogPacketsByDefault");

            healthFmt.Enabled = showHealthOH.Checked = Config.GetBool("ShowHealth");
            healthFmt.Text = Config.GetString("HealthFmt");
            chkPartyOverhead.Checked = Config.GetBool("ShowPartyStats");

            dressList.SelectedIndex = -1;
            hotkeyTree.SelectedNode = null;

            targetByTypeDifferent.Checked = Config.GetBool("DiffTargetByType");
            stepThroughMacro.Checked = Config.GetBool("StepThroughMacro");

            //hotKeyStop.Checked = Config.GetBool("HotKeyStop");

	        showPartyMemberPositions.Checked = Config.GetBool("ShowPartyMemberPositions");
            trackPlayerPosition.Checked = Config.GetBool("TrackPlayerPosition");
            showPlayerPosition.Checked = Config.GetBool("ShowPlayerPosition");
            tiltMap.Checked = Config.GetBool("TiltMap");

            showTargetMessagesOverChar.Checked = Config.GetBool("ShowTargetSelfLastClearOverhead");
            showOverheadMessages.Checked = Config.GetBool("ShowOverheadMessages");

            logSkillChanges.Checked = Config.GetBool("LogSkillChanges");

            lightLevelBar.Value = Config.GetInt("LightLevel");
            double percent = Math.Round((lightLevelBar.Value / (double)lightLevelBar.Maximum) * 100.0);
            lightLevel.Text = $"Light: {percent}%";

            captureMibs.Checked = Config.GetBool("CaptureMibs");

            stealthOverhead.Checked = Config.GetBool("StealthOverhead");

            blockOpenCorpsesTwice.Checked = Config.GetBool("BlockOpenCorpsesTwice");

            screenShotOpenBrowser.Checked = Config.GetBool("ScreenshotUploadOpenBrowser");
            screenShotClipboard.Checked = Config.GetBool("ScreenshotUploadClipboard");
            screenShotNotification.Checked = Config.GetBool("ScreenshotUploadNotifications");

            showContainerLabels.Checked = Config.GetBool("ShowContainerLabels");

            realSeason.Checked = Config.GetBool("RealSeason");
            seasonList.SelectedIndex = Config.GetInt("Season");

            if (screenShotNotification.Checked)
            {
                m_NotifyIcon.Visible = true;
            }
            else
            {
                bool st = Config.GetBool("Systray");
                taskbar.Checked = ShowInTaskbar = !st;
                systray.Checked = m_NotifyIcon.Visible = st;
            }

            showAttackTarget.Checked = Config.GetBool("ShowAttackTargetOverhead");
            showBuffDebuffOverhead.Checked = Config.GetBool("ShowBuffDebuffOverhead");

            buffDebuffFormat.Text = Config.GetString("BuffDebuffFormat");

            rangeCheckTargetByType.Checked = Config.GetBool("RangeCheckTargetByType");
            rangeCheckDoubleClick.Checked = Config.GetBool("RangeCheckDoubleClick");

            // Disable SmartCPU in case it was enabled before the feature was removed
            ClientCommunication.SetSmartCPU(false);

            m_Initializing = false;
        }

        private void tabs_IndexChanged(object sender, System.EventArgs e)
        {
            if (tabs == null)
                return;

            tabs.Size = new Size(tabs.Size.Width, 290);
            Size = new Size(tabs.Size.Width + 10, tabs.Size.Height + 30);

            if (tabs.SelectedTab == generalTab)
            {
                Filters.Filter.Draw(filters);
                langSel.BeginUpdate();
                langSel.Items.Clear();
                langSel.Items.AddRange(Language.GetPackNames());
                langSel.SelectedItem = Language.Current;
                langSel.EndUpdate();
            }
            else if (tabs.SelectedTab == skillsTab)
            {
                RedrawSkills();
            }
            else if (tabs.SelectedTab == displayTab)
            {
                Counter.Redraw(counters);

                titleBarParams.SelectedIndex = 0;

                tabs.Size = new Size(tabs.Size.Width, 313);
                Size = new Size(tabs.Size.Width + 10, tabs.Size.Height + 33);
            }
            else if (tabs.SelectedTab == dressTab)
            {
                int sel = dressList.SelectedIndex;
                dressItems.Items.Clear();
                DressList.Redraw(dressList);
                if (sel >= 0 && sel < dressList.Items.Count)
                    dressList.SelectedIndex = sel;
            }
            else if (tabs.SelectedTab == hotkeysTab)
            {
                filterHotkeys.Text = string.Empty;

                hotkeyTree.SelectedNode = null;
                HotKey.Status = hkStatus;
                if (hotkeyTree.TopNode != null)
                    hotkeyTree.TopNode.Expand();
                else
                    HotKey.RebuildList(hotkeyTree);

                RebuildHotKeyCache();
            }
            else if (tabs.SelectedTab == agentsTab)
            {
                int sel = agentList.SelectedIndex;
                Agent.Redraw(agentList, agentGroup, agentB1, agentB2, agentB3, agentB4, agentB5, agentB6);
                if (sel >= 0 && sel < agentList.Items.Count)
                    agentList.SelectedIndex = sel;
            }
            else if (tabs.SelectedTab == advancedTab)
            {
                UpdateRazorStatus();
            }
            else if (tabs.SelectedTab == macrosTab)
            {
                RedrawMacros();

                if (MacroManager.Playing || MacroManager.Recording)
                    OnMacroStart(MacroManager.Current);
                else
                    OnMacroStop();

                if (MacroManager.Current != null)
                    MacroManager.Current.DisplayTo(actionList);

                macroActGroup.Visible = macroTree.SelectedNode != null;

                expandAdvancedMacros.Text = "\u2193";
            }
            else if (tabs.SelectedTab == screenshotTab)
            {
                tabs.Size = new Size(tabs.Size.Width, 397);
                Size = new Size(tabs.Size.Width + 10, tabs.Size.Height + 33);

                ReloadScreenShotsList();
            }
            else if (tabs.SelectedTab == moreMoreOptTab)
            {
                tabs.Size = new Size(tabs.Size.Width, 358);
                Size = new Size(tabs.Size.Width + 10, tabs.Size.Height + 33);
            }
            else if (tabs.SelectedTab == moreOptTab)
            {
                tabs.Size = new Size(tabs.Size.Width, 346);
                Size = new Size(tabs.Size.Width + 10, tabs.Size.Height + 33);
            }
        }

        private void RebuildHotKeyCache()
        {
            _hotkeyTreeViewCache = new TreeView();

            foreach (TreeNode node in hotkeyTree.Nodes)
            {
                _hotkeyTreeViewCache.Nodes.Add((TreeNode)node.Clone());
            }
        }

        private Version m_Ver = System.Reflection.Assembly.GetCallingAssembly().GetName().Version;

        private uint m_OutPrev;
        private uint m_InPrev;

        private class StatsTimer : Timer
        {
            MainForm m_Form;
            public StatsTimer(MainForm form) : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5))
            {
                m_Form = form;
            }

            protected override void OnTick()
            {
                m_Form.UpdateRazorStatus();
            }
        }

        private void UpdateRazorStatus()
        {
            if (!ClientCommunication.ClientRunning)
                Close();

            uint ps = m_OutPrev;
            uint pr = m_InPrev;
            m_OutPrev = ClientCommunication.TotalOut();
            m_InPrev = ClientCommunication.TotalIn();

            if (tabs.SelectedTab != advancedTab)
                return;

            int time = 0;
            if (ClientCommunication.ConnectionStart != DateTime.MinValue)
                time = (int)((DateTime.UtcNow - ClientCommunication.ConnectionStart).TotalSeconds);

            if (String.IsNullOrEmpty(statusBox.SelectedText))
            {
                statusBox.Lines = Language.Format(LocString.RazorStatus1,
                     m_Ver,
                     Utility.FormatSize(System.GC.GetTotalMemory(false)),
                     Utility.FormatSize(m_OutPrev), Utility.FormatSize((long)((m_OutPrev - ps))),
                     Utility.FormatSize(m_InPrev), Utility.FormatSize((long)((m_InPrev - pr))),
                     Utility.FormatTime(time),
                     (World.Player != null ? (uint)World.Player.Serial : 0),
                     (World.Player != null && World.Player.Backpack != null ? (uint)World.Player.Backpack.Serial : 0),
                     World.Items.Count,
                     World.Mobiles.Count).Split('\n');

                if (World.Player != null)
                    statusBox.AppendText(
                            $"\r\nCoordinates: {World.Player.Position.X} {World.Player.Position.Y} {World.Player.Position.Z}");
            }

            if (PacketHandlers.PlayCharTime < DateTime.UtcNow && PacketHandlers.PlayCharTime + TimeSpan.FromSeconds(30) < DateTime.UtcNow)
            {
                if (Config.GetBool("Negotiate"))
                {
                    bool allAllowed = true;
                    StringBuilder text = new StringBuilder();

                    text.Append(Language.GetString(LocString.NegotiateTitle));
                    text.Append("\r\n");

                    for (uint i = 0; i < FeatureBit.MaxBit; i++)
                    {
                        if (!ClientCommunication.AllowBit(i))
                        {
                            allAllowed = false;

                            text.Append(Language.GetString((LocString)(((int)LocString.FeatureDescBase) + i)));
                            text.Append(' ');
                            text.Append(Language.GetString(LocString.NotAllowed));
                            text.Append("\r\n");
                        }
                    }

                    if (allAllowed)
                        text.Append(Language.GetString(LocString.AllFeaturesEnabled));

                    text.Append("\r\n");

                    features.Visible = true;
                    features.Text = text.ToString();
                }
                else
                {
                    features.Visible = false;
                }
            }
        }

        public void UpdateSkill(Skill skill)
        {
            double total = 0;
            for (int i = 0; i < Skill.Count; i++)
            {
                total += World.Player.Skills[i].Base;
            }

            baseTotal.Text = $"{total:F1}";

            if (Config.GetBool("LogSkillChanges"))
            {
                string skillLog =
                    $"{Config.GetInstallDirectory()}\\SkillLog\\{World.Player.Name}_{World.Player.Serial}_SkillLog.csv";

                if (!Directory.Exists($"{Config.GetInstallDirectory()}\\SkillLog"))
                {
                    Directory.CreateDirectory($"{Config.GetInstallDirectory()}\\SkillLog");
                }

                if (!File.Exists(skillLog))
                {
                    using (StreamWriter sr = File.CreateText(skillLog))
                    {
                        sr.WriteLine("Timestamp,SkillName,Value,Base,Gain,Cap");
                    }
                }

                using (StreamWriter sw = File.AppendText(skillLog))
                {
                    sw.WriteLine(
                        $"{DateTime.Now},{(SkillName)skill.Index},{skill.Value},{skill.Base},{skill.Delta},{skill.Cap}");
                }
            }

            for (int i = 0; i < skillList.Items.Count; i++)
            {
                ListViewItem cur = skillList.Items[i];
                if (cur.Tag == skill)
                {
                    cur.SubItems[1].Text = String.Format("{0:F1}", skill.Value);
                    cur.SubItems[2].Text = String.Format("{0:F1}", skill.Base);
                    cur.SubItems[3].Text = String.Format("{0}{1:F1}", (skill.Delta > 0 ? "+" : ""), skill.Delta);
                    cur.SubItems[4].Text = String.Format("{0:F1}", skill.Cap);
                    cur.SubItems[5].Text = skill.Lock.ToString()[0].ToString();
                    SortSkills();
                    return;
                }
            }
        }

        public void RedrawSkills()
        {
            skillList.BeginUpdate();
            skillList.Items.Clear();
            double Total = 0;
            if (World.Player != null && World.Player.SkillsSent)
            {
                string[] items = new string[6];
                for (int i = 0; i < Skill.Count; i++)
                {
                    Skill sk = World.Player.Skills[i];
                    Total += sk.Base;
                    items[0] = Language.Skill2Str(i);//((SkillName)i).ToString();
                    items[1] = String.Format("{0:F1}", sk.Value);
                    items[2] = String.Format("{0:F1}", sk.Base);
                    items[3] = String.Format("{0}{1:F1}", (sk.Delta > 0 ? "+" : ""), sk.Delta);
                    items[4] = String.Format("{0:F1}", sk.Cap);
                    items[5] = sk.Lock.ToString()[0].ToString();

                    ListViewItem lvi = new ListViewItem(items);
                    lvi.Tag = sk;
                    skillList.Items.Add(lvi);
                }

                //Config.SetProperty( "SkillListAsc", false );
                SortSkills();
            }
            skillList.EndUpdate();
            baseTotal.Text = String.Format("{0:F1}", Total);
        }

        private void OnFilterCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            ((Filter)filters.Items[e.Index]).OnCheckChanged(e.NewValue);
        }

        private void incomingMob_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("ShowMobNames", incomingMob.Checked);
        }

        private void incomingCorpse_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("ShowCorpseNames", incomingCorpse.Checked);
        }

        private ContextMenu m_SkillMenu;
        private void skillList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListView.SelectedListViewItemCollection items = skillList.SelectedItems;
                if (items.Count <= 0)
                    return;
                Skill s = items[0].Tag as Skill;
                if (s == null)
                    return;

                if (m_SkillMenu == null)
                {
                    m_SkillMenu = new ContextMenu(new MenuItem[]
                    {
                              new MenuItem( Language.GetString( LocString.SetSLUp ), new EventHandler( onSetSkillLockUP ) ),
                              new MenuItem( Language.GetString( LocString.SetSLDown ), new EventHandler( onSetSkillLockDOWN ) ),
                              new MenuItem( Language.GetString( LocString.SetSLLocked ), new EventHandler( onSetSkillLockLOCKED ) )
                    });
                }

                for (int i = 0; i < 3; i++)
                    m_SkillMenu.MenuItems[i].Checked = ((int)s.Lock) == i;

                m_SkillMenu.Show(skillList, new Point(e.X, e.Y));
            }
        }

        private void onSetSkillLockUP(object sender, EventArgs e)
        {
            SetLock(LockType.Up);
        }

        private void onSetSkillLockDOWN(object sender, EventArgs e)
        {
            SetLock(LockType.Down);
        }

        private void onSetSkillLockLOCKED(object sender, EventArgs e)
        {
            SetLock(LockType.Locked);
        }

        private void SetLock(LockType lockType)
        {
            ListView.SelectedListViewItemCollection items = skillList.SelectedItems;
            if (items.Count <= 0)
                return;
            Skill s = items[0].Tag as Skill;
            if (s == null)
                return;

            try
            {
                ClientCommunication.SendToServer(new SetSkillLock(s.Index, lockType));

                s.Lock = lockType;
                UpdateSkill(s);

                ClientCommunication.SendToClient(new SkillUpdate(s));
            }
            catch
            {
            }

        }

        private void OnSkillColClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            if (e.Column == Config.GetInt("SkillListCol"))
                Config.SetProperty("SkillListAsc", !Config.GetBool("SkillListAsc"));
            else
                Config.SetProperty("SkillListCol", e.Column);
            SortSkills();
        }

        private void SortSkills()
        {
            int col = Config.GetInt("SkillListCol");
            bool asc = Config.GetBool("SkillListAsc");

            if (col < 0 || col > 5)
                col = 0;

            skillList.BeginUpdate();
            if (col == 0 || col == 5)
            {
                skillList.ListViewItemSorter = null;
                skillList.Sorting = asc ? SortOrder.Ascending : SortOrder.Descending;
            }
            else
            {
                LVDoubleComparer.Column = col;
                LVDoubleComparer.Asc = asc;

                skillList.ListViewItemSorter = LVDoubleComparer.Instance;

                skillList.Sorting = SortOrder.None;
                skillList.Sort();
            }
            skillList.EndUpdate();
            skillList.Refresh();
        }

        private class LVDoubleComparer : IComparer
        {
            public static readonly LVDoubleComparer Instance = new LVDoubleComparer();
            public static int Column { set { Instance.m_Col = value; } }
            public static bool Asc { set { Instance.m_Asc = value; } }

            private int m_Col;
            private bool m_Asc;

            private LVDoubleComparer()
            {
            }

            public int Compare(object x, object y)
            {
                if (x == null || !(x is ListViewItem))
                    return m_Asc ? 1 : -1;
                else if (y == null || !(y is ListViewItem))
                    return m_Asc ? -1 : 1;

                try
                {
                    double dx = Convert.ToDouble(((ListViewItem)x).SubItems[m_Col].Text);
                    double dy = Convert.ToDouble(((ListViewItem)y).SubItems[m_Col].Text);

                    if (dx > dy)
                        return m_Asc ? -1 : 1;
                    else if (dx == dy)
                        return 0;
                    else //if ( dx > dy )
                        return m_Asc ? 1 : -1;
                }
                catch
                {
                    return ((ListViewItem)x).Text.CompareTo(((ListViewItem)y).Text) * (m_Asc ? 1 : -1);
                }
            }
        }

        private void OnResetSkillDelta(object sender, System.EventArgs e)
        {
            if (World.Player == null)
                return;

            for (int i = 0; i < Skill.Count; i++)
                World.Player.Skills[i].Delta = 0;

            RedrawSkills();
        }

        private void OnSetSkillLocks(object sender, System.EventArgs e)
        {
            if (locks.SelectedIndex == -1 || World.Player == null)
                return;

            LockType type = (LockType)locks.SelectedIndex;

            for (short i = 0; i < Skill.Count; i++)
            {
                World.Player.Skills[i].Lock = type;
                ClientCommunication.SendToServer(new SetSkillLock(i, type));
            }
            ClientCommunication.SendToClient(new SkillsList());
            RedrawSkills();
        }

        private void OnDispSkillCheck(object sender, System.EventArgs e)
        {
            Config.SetProperty("DispSkillChanges", dispDelta.Checked);
        }

        private void delCounter_Click(object sender, System.EventArgs e)
        {
            if (counters.SelectedItems.Count <= 0)
                return;

            Counter c = counters.SelectedItems[0].Tag as Counter;

            if (c != null)
            {
                AddCounter ac = new AddCounter(c);
                switch (ac.ShowDialog(this))
                {
                    case DialogResult.Abort:
                        counters.Items.Remove(c.ViewItem);
                        Counter.List.Remove(c);
                        break;

                    case DialogResult.OK:
                        c.Set((ushort)ac.ItemID, ac.Hue, ac.NameStr, ac.FmtStr, ac.DisplayImage);
                        break;
                }
            }
        }

        private void addCounter_Click(object sender, System.EventArgs e)
        {
            AddCounter dlg = new AddCounter();

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Counter.Register(new Counter(dlg.NameStr, dlg.FmtStr, (ushort)dlg.ItemID, (int)dlg.Hue, dlg.DisplayImage));
                Counter.Redraw(counters);
            }
        }

        private void showInBar_CheckedChanged(object sender, System.EventArgs e)
        {
            titleStr.Enabled = showInBar.Checked;
            Config.SetProperty("TitleBarDisplay", showInBar.Checked);
            ClientCommunication.RequestTitlebarUpdate();
        }

        private void titleStr_TextChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("TitleBarText", titleStr.Text.TrimEnd());
            if (Config.GetBool("TitleBarDisplay"))
                ClientCommunication.RequestTitlebarUpdate();
        }

        private void counters_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (e.Index >= 0 && e.Index < Counter.List.Count && !Counter.SupressChecks)
            {
                ((Counter)(counters.Items[e.Index].Tag)).SetEnabled(e.NewValue == CheckState.Checked);
                ClientCommunication.RequestTitlebarUpdate();
                counters.Sort();
                //counters.Refresh();
            }
        }

        public void RedrawCounters()
        {
            Counter.Redraw(counters);
        }

        private void checkNewConts_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("AutoSearch", checkNewConts.Checked);
            excludePouches.Enabled = checkNewConts.Checked;
        }

        private void warnCount_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("CounterWarn", warnCount.Checked);
            warnNum.Enabled = warnCount.Checked;
        }

        private void timerTimer_Tick(object sender, System.EventArgs e)
        {
            Timer.Control = timerTimer;
            Timer.Slice();
        }

        private void warnNum_TextChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("CounterWarnAmount", Utility.ToInt32(warnNum.Text.Trim(), 3));
        }

        private void alwaysTop_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("AlwaysOnTop", this.TopMost = alwaysTop.Checked);
        }

        private void profiles_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (profiles.SelectedIndex < 0 || !m_ProfileConfirmLoad)
                return;

            string name = (string)profiles.Items[profiles.SelectedIndex];
            if (MessageBox.Show(this, Language.Format(LocString.ProfLoadQ, name), "Load?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Config.Save();
                if (!Config.LoadProfile(name))
                {
                    MessageBox.Show(this, Language.GetString(LocString.ProfLoadE), "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    InitConfig();
                    if (World.Player != null)
                        Config.SetProfileFor(World.Player);
                }
                ClientCommunication.RequestTitlebarUpdate();
            }
            else
            {
                m_ProfileConfirmLoad = false;
                for (int i = 0; i < profiles.Items.Count; i++)
                {
                    if ((string)profiles.Items[i] == Config.CurrentProfile.Name)
                    {
                        profiles.SelectedIndex = i;
                        break;
                    }
                }
                m_ProfileConfirmLoad = true;
            }
        }

        private void delProfile_Click(object sender, System.EventArgs e)
        {
            if (profiles.SelectedIndex < 0)
                return;

            if (MessageBox.Show(this, "Are you sure you want to delete this profile?", "Delete Profile?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            string remove = (string)profiles.Items[profiles.SelectedIndex];

            if (remove == "default")
            {
                MessageBox.Show(this, Language.GetString(LocString.NoDelete), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string file = String.Format("Profiles/{0}.xml", remove);
            if (File.Exists(file))
                File.Delete(file);

            profiles.Items.Remove(remove);
            if (!Config.LoadProfile("default"))
            {
                Config.CurrentProfile.MakeDefault();
                Config.CurrentProfile.Name = "default";
            }
            InitConfig();

            m_ProfileConfirmLoad = false;
            for (int i = 0; i < profiles.Items.Count; i++)
            {
                if ((string)profiles.Items[i] == "default")
                {
                    profiles.SelectedIndex = i;
                    m_ProfileConfirmLoad = true;
                    return;
                }
            }

            int sel = profiles.Items.Count;
            profiles.Items.Add("default");
            profiles.SelectedIndex = sel;
            m_ProfileConfirmLoad = true;
        }

        public void SelectProfile(string name)
        {
            m_ProfileConfirmLoad = false;
            profiles.SelectedItem = name;
            m_ProfileConfirmLoad = true;
        }

        private void newProfile_Click(object sender, System.EventArgs e)
        {
            if (InputBox.Show(this, Language.GetString(LocString.EnterProfileName),
                Language.GetString(LocString.EnterAName)))
            {
                string str = InputBox.GetString();
                if (str == null || str == "" || str.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                    str.IndexOfAny(m_InvalidNameChars) != -1)
                {
                    MessageBox.Show(this, Language.GetString(LocString.InvalidChars),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                m_ProfileConfirmLoad = false;
                int sel = profiles.Items.Count;
                string lwr = str.ToLower();
                for (int i = 0; i < profiles.Items.Count; i++)
                {
                    if (lwr == ((string)profiles.Items[i]).ToLower())
                    {
                        if (MessageBox.Show(this, Language.GetString(LocString.ProfExists), "Load Profile?",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Config.Save();
                            profiles.SelectedIndex = i;
                            if (!Config.LoadProfile(str))
                            {
                                MessageBox.Show(this, Language.GetString(LocString.ProfLoadE), "Load Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                InitConfig();
                                if (World.Player != null)
                                    Config.SetProfileFor(World.Player);
                            }

                            ClientCommunication.RequestTitlebarUpdate();
                        }

                        m_ProfileConfirmLoad = true;
                        return;
                    }
                }

                Config.Save();
                Config.NewProfile(str);
                profiles.Items.Add(str);
                profiles.SelectedIndex = sel;
                InitConfig();
                if (World.Player != null)
                    Config.SetProfileFor(World.Player);
                m_ProfileConfirmLoad = true;
            }
        }

        public bool CanClose
        {
            get
            {
                return m_CanClose;
            }
            set
            {
                m_CanClose = value;
            }
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!m_CanClose && ClientCommunication.ClientRunning)
            {
                DisableCloseButton();
                e.Cancel = true;
            }
            else
            {
                PacketPlayer.Stop();
                AVIRec.Stop();
            }
            //if ( Engine.NoPatch )
            //	e.Cancel = MessageBox.Show( this, "Are you sure you want to close Razor?\n(This will not close the UO client.)", "Close Razor?", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.No;
        }

        private void skillCopySel_Click(object sender, System.EventArgs e)
        {
            if (skillList.SelectedItems == null || skillList.SelectedItems.Count <= 0)
                return;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < skillList.SelectedItems.Count; i++)
            {
                ListViewItem vi = skillList.SelectedItems[i];
                if (vi != null && vi.SubItems != null && vi.SubItems.Count > 4)
                {
                    string name = vi.SubItems[0].Text;
                    if (name != null && name.Length > 20)
                        name = name.Substring(0, 16) + "...";

                    sb.AppendFormat("{0,-20} {1,5:F1} {2,5:F1} {4:F1} {5,5:F1}\n",
                         name,
                         vi.SubItems[1].Text,
                         vi.SubItems[2].Text,
                         Utility.ToInt32(vi.SubItems[3].Text, 0) < 0 ? "" : "+",
                         vi.SubItems[3].Text,
                         vi.SubItems[4].Text);
                }
            }

            if (sb.Length > 0)
                Clipboard.SetDataObject(sb.ToString(), true);
        }

        private void skillCopyAll_Click(object sender, System.EventArgs e)
        {
            if (World.Player == null)
                return;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Skill.Count; i++)
            {
                Skill sk = World.Player.Skills[i];
                sb.AppendFormat("{0,-20} {1,-5:F1} {2,-5:F1} {3}{4,-5:F1} {5,-5:F1}\n", (SkillName)i, sk.Value, sk.Base, sk.Delta > 0 ? "+" : "", sk.Delta, sk.Cap);
            }

            if (sb.Length > 0)
                Clipboard.SetDataObject(sb.ToString(), true);
        }

        private void addDress_Click(object sender, System.EventArgs e)
        {
            if (InputBox.Show(this, Language.GetString(LocString.DressName), Language.GetString(LocString.EnterAName)))
            {
                string str = InputBox.GetString();
                if (str == null || str == "")
                    return;
                DressList list = new DressList(str);
                DressList.Add(list);
                dressList.Items.Add(list);
                dressList.SelectedItem = list;
            }
        }

        private void removeDress_Click(object sender, System.EventArgs e)
        {
            DressList dress = (DressList)dressList.SelectedItem;

            if (dress != null && MessageBox.Show(this, Language.GetString(LocString.DelDressQ), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dress.Items.Clear();
                dressList.Items.Remove(dress);
                dressList.SelectedIndex = -1;
                dressItems.Items.Clear();
                DressList.Remove(dress);
            }
        }

        private void dressNow_Click(object sender, System.EventArgs e)
        {
            DressList dress = (DressList)dressList.SelectedItem;
            if (dress != null && World.Player != null)
                dress.Dress();
        }

        private void undressList_Click(object sender, System.EventArgs e)
        {
            DressList dress = (DressList)dressList.SelectedItem;
            if (dress != null && World.Player != null && World.Player.Backpack != null)
                dress.Undress();
        }

        private void targItem_Click(object sender, System.EventArgs e)
        {
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(OnDressItemTarget));
        }

        private void OnDressItemTarget(bool loc, Serial serial, Point3D pt, ushort itemid)
        {
            if (loc)
                return;

            ShowMe();
            if (serial.IsItem)
            {
                DressList list = (DressList)dressList.SelectedItem;

                if (list == null)
                    return;

                list.Items.Add(serial);
                Item item = World.FindItem(serial);

                if (item == null)
                    dressItems.Items.Add(Language.Format(LocString.OutOfRangeA1, serial));
                else
                    dressItems.Items.Add(item.ToString());
            }
        }

        private void dressDelSel_Click(object sender, System.EventArgs e)
        {
            DressList list = (DressList)dressList.SelectedItem;
            if (list == null)
                return;

            int sel = dressItems.SelectedIndex;
            if (sel < 0 || sel >= list.Items.Count)
                return;

            if (MessageBox.Show(this, Language.GetString(LocString.DelDressItemQ), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    list.Items.RemoveAt(sel);
                    dressItems.Items.RemoveAt(sel);
                }
                catch
                {
                }
            }
        }

        private void clearDress_Click(object sender, System.EventArgs e)
        {
            DressList list = (DressList)dressList.SelectedItem;
            if (list == null)
                return;

            if (MessageBox.Show(this, Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                list.Items.Clear();
                dressItems.Items.Clear();
            }
        }

        private DressList undressBagList = null;
        private void undressBag_Click(object sender, System.EventArgs e)
        {
            if (World.Player == null)
                return;

            DressList list = (DressList)dressList.SelectedItem;
            if (list == null)
                return;

            undressBagList = list;
            Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(onDressBagTarget));
            World.Player.SendMessage(MsgLevel.Force, LocString.TargUndressBag, list.Name);
        }

        void onDressBagTarget(bool location, Serial serial, Point3D p, ushort gfxid)
        {
            if (undressBagList == null)
                return;

            ShowMe();
            if (serial.IsItem)
            {
                Item item = World.FindItem(serial);
                if (item != null)
                {
                    undressBagList.SetUndressBag(item.Serial);
                    World.Player.SendMessage(MsgLevel.Force, LocString.UB_Set);
                }
                else
                {
                    undressBagList.SetUndressBag(Serial.Zero);
                    World.Player.SendMessage(MsgLevel.Force, LocString.ItemNotFound);
                }
            }
            else
            {
                World.Player.SendMessage(MsgLevel.Force, LocString.ItemNotFound);
            }

            undressBagList = null;
        }

        private void dressList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DressList list = (DressList)dressList.SelectedItem;

            dressItems.BeginUpdate();
            dressItems.Items.Clear();
            if (list != null)
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    if (list.Items[i] is Serial)
                    {
                        Serial serial = (Serial)list.Items[i];
                        Item item = World.FindItem(serial);

                        if (item != null)
                            dressItems.Items.Add(item.ToString());
                        else
                            dressItems.Items.Add(Language.Format(LocString.OutOfRangeA1, serial));
                    }
                    else if (list.Items[i] is ItemID)
                    {
                        dressItems.Items.Add(list.Items[i].ToString());
                    }
                }
            }
            dressItems.EndUpdate();
        }

        private void dressUseCur_Click(object sender, System.EventArgs e)
        {
            DressList list = (DressList)dressList.SelectedItem;
            if (World.Player == null)
                return;
            if (list == null)
                return;

            for (int i = 0; i < World.Player.Contains.Count; i++)
            {
                Item item = (Item)World.Player.Contains[i];
                if (item.Layer <= Layer.LastUserValid && item.Layer != Layer.Backpack && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair)
                    list.Items.Add(item.Serial);
            }
            dressList.SelectedItem = null;
            dressList.SelectedItem = list;
        }

        private void hotkeyTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            ClearHKCtrls();

            if (e.Node == null || !(e.Node.Tag is KeyData))
                return;
            KeyData hk = (KeyData)e.Node.Tag;

            try
            {
                m_LastKV = hk.Key;
                switch (hk.Key)
                {
                    case -1:
                        key.Text = ("MouseWheel UP");
                        break;
                    case -2:
                        key.Text = ("MouseWheel DOWN");
                        break;
                    case -3:
                        key.Text = ("Mouse MID Button");
                        break;
                    case -4:
                        key.Text = ("Mouse XButton 1");
                        break;
                    case -5:
                        key.Text = ("Mouse XButton 2");
                        break;
                    default:
                        if (hk.Key > 0 && hk.Key < 256)
                            key.Text = (((Keys)hk.Key).ToString());
                        else
                            key.Text = ("");
                        break;
                }
            }
            catch
            {
                key.Text = ">>ERROR<<";
            }

            chkCtrl.Checked = (hk.Mod & ModKeys.Control) != 0;
            chkAlt.Checked = (hk.Mod & ModKeys.Alt) != 0;
            chkShift.Checked = (hk.Mod & ModKeys.Shift) != 0;
            chkPass.Checked = hk.SendToUO;

            if ((hk.LocName >= (int)LocString.DrinkHeal && hk.LocName <= (int)LocString.DrinkAg && !ClientCommunication.AllowBit(FeatureBit.PotionHotkeys)) ||
                 (hk.LocName >= (int)LocString.TargCloseRed && hk.LocName <= (int)LocString.TargCloseCriminal && !ClientCommunication.AllowBit(FeatureBit.ClosestTargets)) ||
                 (((hk.LocName >= (int)LocString.TargRandRed && hk.LocName <= (int)LocString.TargRandNFriend) ||
                 (hk.LocName >= (int)LocString.TargRandEnemyHuman && hk.LocName <= (int)LocString.TargRandCriminal)) && !ClientCommunication.AllowBit(FeatureBit.RandomTargets)))
            {
                LockControl(chkCtrl);
                LockControl(chkAlt);
                LockControl(chkShift);
                LockControl(chkPass);
                LockControl(key);
                LockControl(unsetHK);
                LockControl(setHK);
                LockControl(dohotkey);
            }
        }

        private KeyData GetSelectedHK()
        {
            if (hotkeyTree != null && hotkeyTree.SelectedNode != null)
                return hotkeyTree.SelectedNode.Tag as KeyData;
            else
                return null;
        }

        private void ClearHKCtrls()
        {
            m_LastKV = 0;
            key.Text = "";
            chkCtrl.Checked = false;
            chkAlt.Checked = false;
            chkShift.Checked = false;
            chkPass.Checked = false;

            UnlockControl(chkCtrl);
            UnlockControl(chkAlt);
            UnlockControl(chkShift);
            UnlockControl(chkPass);
            UnlockControl(key);
            UnlockControl(unsetHK);
            UnlockControl(setHK);
            UnlockControl(dohotkey);
        }

        private void setHK_Click(object sender, System.EventArgs e)
        {
            KeyData hk = GetSelectedHK();
            if (hk == null || m_LastKV == 0)
                return;

            ModKeys mod = ModKeys.None;
            if (chkCtrl.Checked)
                mod |= ModKeys.Control;
            if (chkAlt.Checked)
                mod |= ModKeys.Alt;
            if (chkShift.Checked)
                mod |= ModKeys.Shift;

            KeyData g = HotKey.Get(m_LastKV, mod);
            bool block = false;
            if (g != null && g != hk)
            {
                if (MessageBox.Show(this, Language.Format(LocString.KeyUsed, g.DispName, hk.DispName), "Hot Key Conflict", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    g.Key = 0;
                    g.Mod = ModKeys.None;
                    g.SendToUO = false;
                }
                else
                {
                    block = true;
                }
            }

            if (!block)
            {
                hk.Key = m_LastKV;
                hk.Mod = mod;

                hk.SendToUO = chkPass.Checked;
            }

            if (!string.IsNullOrEmpty(filterHotkeys.Text))
            {
                TreeNode node = hotkeyTree.SelectedNode;

                HotKey.RebuildList(hotkeyTree);
                RebuildHotKeyCache();

                filterHotkeys_TextChanged(sender, e);

                hotkeyTree.SelectedNode = node;
            }
        }

        private void unsetHK_Click(object sender, System.EventArgs e)
        {
            KeyData hk = GetSelectedHK();
            if (hk == null)
                return;

            hk.Key = 0;
            hk.Mod = 0;
            hk.SendToUO = false;

            ClearHKCtrls();

            if (!string.IsNullOrEmpty(filterHotkeys.Text))
            {
                TreeNode node = hotkeyTree.SelectedNode;

                HotKey.RebuildList(hotkeyTree);
                RebuildHotKeyCache();

                filterHotkeys_TextChanged(sender, e);

                hotkeyTree.SelectedNode = node;
            }
        }

        private void key_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            m_LastKV = (int)e.KeyCode;
            key.Text = e.KeyCode.ToString();

            e.Handled = true;
        }

        private void key_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                m_LastKV = -1;
                key.Text = "MouseWheel UP";
            }
            else if (e.Delta < 0)
            {
                m_LastKV = -2;
                key.Text = "MouseWheel DOWN";
            }
        }

        private void key_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                m_LastKV = -3;
                key.Text = "Mouse MID Button";
            }
            else if (e.Button == MouseButtons.XButton1)
            {
                m_LastKV = -4;
                key.Text = "Mouse XButton 1";
            }
            else if (e.Button == MouseButtons.XButton2)
            {
                m_LastKV = -5;
                key.Text = "Mouse XButton 2";
            }
        }

        private void dohotkey_Click(object sender, System.EventArgs e)
        {
            KeyData hk = GetSelectedHK();
            if (hk != null && World.Player != null)
            {
                if (MacroManager.AcceptActions)
                    MacroManager.Action(new HotKeyAction(hk));
                hk.Callback();
            }
        }

        private void queueTargets_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("QueueTargets", queueTargets.Checked);
        }

        private void chkForceSpeechHue_CheckedChanged(object sender, System.EventArgs e)
        {
            setSpeechHue.Enabled = chkForceSpeechHue.Checked;
            Config.SetProperty("ForceSpeechHue", chkForceSpeechHue.Checked);
        }

        private void lthilight_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!(setLTHilight.Enabled = lthilight.Checked))
            {
                Config.SetProperty("LTHilight", 0);
                ClientCommunication.SetCustomNotoHue(0);
                lthilight.BackColor = SystemColors.Control;
                lthilight.ForeColor = SystemColors.ControlText;
            }
        }

        private void chkForceSpellHue_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkForceSpellHue.Checked)
            {
                setBeneHue.Enabled = setHarmHue.Enabled = setNeuHue.Enabled = true;
                Config.SetProperty("ForceSpellHue", true);
            }
            else
            {
                setBeneHue.Enabled = setHarmHue.Enabled = setNeuHue.Enabled = false;
                Config.SetProperty("ForceSpellHue", false);
            }
        }

        private void txtSpellFormat_TextChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("SpellFormat", txtSpellFormat.Text.Trim());
        }

        private void InitPreviewHue(Control ctrl, string cfg)
        {
            int hueIdx = Config.GetInt(cfg);
            if (hueIdx > 0 && hueIdx < 3000)
                ctrl.BackColor = Ultima.Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
            else
                ctrl.BackColor = SystemColors.Control;
            ctrl.ForeColor = (ctrl.BackColor.GetBrightness() < 0.35 ? Color.White : Color.Black);
        }

        private bool SetHue(Control ctrl, string cfg)
        {
            HueEntry h = new HueEntry(Config.GetInt(cfg));

            if (h.ShowDialog(this) == DialogResult.OK)
            {
                int hueIdx = h.Hue;
                Config.SetProperty(cfg, hueIdx);
                if (hueIdx > 0 && hueIdx < 3000)
                    ctrl.BackColor = Ultima.Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
                else
                    ctrl.BackColor = Color.White;
                ctrl.ForeColor = (ctrl.BackColor.GetBrightness() < 0.35 ? Color.White : Color.Black);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void setExHue_Click(object sender, System.EventArgs e)
        {
            SetHue(lblExHue, "ExemptColor");
        }

        private void setMsgHue_Click(object sender, System.EventArgs e)
        {
            SetHue(lblMsgHue, "SysColor");
        }

        private void setWarnHue_Click(object sender, System.EventArgs e)
        {
            SetHue(lblWarnHue, "WarningColor");
        }

        private void setSpeechHue_Click(object sender, System.EventArgs e)
        {
            SetHue(chkForceSpeechHue, "SpeechHue");
        }

        private void setLTHilight_Click(object sender, System.EventArgs e)
        {
            if (SetHue(lthilight, "LTHilight"))
                ClientCommunication.SetCustomNotoHue(Config.GetInt("LTHilight"));
        }

        private void setBeneHue_Click(object sender, System.EventArgs e)
        {
            SetHue(lblBeneHue, "BeneficialSpellHue");
        }

        private void setHarmHue_Click(object sender, System.EventArgs e)
        {
            SetHue(lblHarmHue, "HarmfulSpellHue");
        }

        private void setNeuHue_Click(object sender, System.EventArgs e)
        {
            SetHue(lblNeuHue, "NeutralSpellHue");
        }

        private void QueueActions_CheckedChanged(object sender, System.EventArgs e)
        {
            //txtObjDelay.Enabled = QueueActions.Checked;
            Config.SetProperty("QueueActions", QueueActions.Checked);
        }

        private void txtObjDelay_TextChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("ObjectDelay", Utility.ToInt32(txtObjDelay.Text.Trim(), 500));
        }

        private void chkStealth_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("CountStealthSteps", chkStealth.Checked);

            stealthOverhead.Enabled = chkStealth.Checked;
        }

        private void agentList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                Agent.Select(agentList.SelectedIndex, agentList, agentSubList, agentGroup, agentB1, agentB2, agentB3, agentB4, agentB5, agentB6);
            }
            catch
            {
            }
        }

        private void Agent_Button(int b)
        {
            if (World.Player == null)
                return;

            Agent a = agentList.SelectedItem as Agent;
            if (a == null)
                agentList.SelectedIndex = -1;
            else
                a.OnButtonPress(b);
        }

        private void agentB1_Click(object sender, System.EventArgs e)
        {
            Agent_Button(1);
        }

        private void agentB2_Click(object sender, System.EventArgs e)
        {
            Agent_Button(2);
        }

        private void agentB3_Click(object sender, System.EventArgs e)
        {
            Agent_Button(3);
        }

        private void agentB4_Click(object sender, System.EventArgs e)
        {
            Agent_Button(4);
        }

        private void agentB5_Click(object sender, System.EventArgs e)
        {
            Agent_Button(5);
        }

        private void agentB6_Click(object sender, System.EventArgs e)
        {
            Agent_Button(6);
        }

        private void MainForm_Activated(object sender, System.EventArgs e)
        {
            DisableCloseButton();
            //this.TopMost = true;
        }

        private void MainForm_Deactivate(object sender, System.EventArgs e)
        {
            if (this.TopMost)
                this.TopMost = false;
        }

        private void MainForm_Resize(object sender, System.EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && !this.ShowInTaskbar)
                this.Hide();
        }

        private bool IsNear(int a, int b)
        {
            return (a <= b + 5 && a >= b - 5);
        }

        private void MainForm_Move(object sender, System.EventArgs e)
        {
            // atempt to dock to the side of the screen.  Also try not to save the X/Y when we are minimized (which is -32000, -32000)
            System.Drawing.Point pt = this.Location;

            Rectangle screen = Screen.GetWorkingArea(this);
            if (this.WindowState != FormWindowState.Minimized && pt.X + this.Width / 2 >= screen.Left && pt.Y + this.Height / 2 >= screen.Top && pt.X <= screen.Right && pt.Y <= screen.Bottom)
            {
                if (IsNear(pt.X + this.Width, screen.Right))
                    pt.X = screen.Right - this.Width;
                else if (IsNear(pt.X, screen.Left))
                    pt.X = screen.Left;

                if (IsNear(pt.Y + this.Height, screen.Bottom))
                    pt.Y = screen.Bottom - this.Height;
                else if (IsNear(pt.Y, screen.Top))
                    pt.Y = screen.Top;

                this.Location = pt;
                Config.SetProperty("WindowX", (int)pt.X);
                Config.SetProperty("WindowY", (int)pt.Y);
            }
        }

        private void opacity_Scroll(object sender, System.EventArgs e)
        {
            int o = opacity.Value;
            Config.SetProperty("Opacity", o);
            opacityLabel.Text = String.Format("Opacity: {0}%", o);
            this.Opacity = ((double)o) / 100.0;
        }

        private void dispDelta_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("DisplaySkillChanges", dispDelta.Checked);
        }

        /*private void saveProfile_Click(object sender, System.EventArgs e)
        {
             Counter.Save();
             Config.Save();
             MacroManager.Save();
             MessageBox.Show( this, Language.GetString( LocString.SaveOK ), "Save OK", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }

        private void edit_Click(object sender, System.EventArgs e)
        {
             if ( counters.SelectedItems.Count <= 0 )
                  return;

             Counter c = counters.SelectedItems[0].Tag as Counter;
             if ( c == null )
                  return;

             AddCounter dlg = new AddCounter( c.Name, c.Format, c.ItemID, c.Hue );

             if ( dlg.ShowDialog( this ) == DialogResult.OK )
             {
                  c.Name = dlg.NameStr;
                  c.Format = dlg.FmtStr;
                  c.ItemID = (ushort)dlg.ItemID;
                  c.Hue = (int)dlg.Hue;
                  Counter.Redraw( counters );
             }
        }*/

        private void logPackets_CheckedChanged(object sender, System.EventArgs e)
        {
            if (logPackets.Checked)
            {
                if (m_Initializing || MessageBox.Show(this, Language.GetString(LocString.PacketLogWarn), "Caution!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    Packet.Logging = true;
                else
                    logPackets.Checked = false;
            }
            else
            {
                Packet.Logging = false;
            }
        }

        private void showNotoHue_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("ShowNotoHue", showNotoHue.Checked);
            if (showNotoHue.Checked)
                ClientCommunication.RequestTitlebarUpdate();
        }

        private void recount_Click(object sender, System.EventArgs e)
        {
            Counter.FullRecount();
        }

        private void openCorpses_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("AutoOpenCorpses", openCorpses.Checked);
            corpseRange.Enabled = openCorpses.Checked;
        }

        private void corpseRange_TextChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("CorpseRange", Utility.ToInt32(corpseRange.Text, 2));
        }

        private void showWelcome_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetAppSetting("ShowWelcome", (showWelcome.Checked ? 1 : 0).ToString());

        }

        private ContextMenu m_DressItemsMenu = null;
        private void dressItems_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_DressItemsMenu = new ContextMenu(new MenuItem[] { new MenuItem(Language.GetString(LocString.Conv2Type), new EventHandler(OnMakeType)) });
                m_DressItemsMenu.Show(dressItems, new Point(e.X, e.Y));
            }
        }

        private void OnMakeType(object sender, System.EventArgs e)
        {
            DressList list = (DressList)dressList.SelectedItem;
            if (list == null)
                return;
            int sel = dressItems.SelectedIndex;
            if (sel < 0 || sel >= list.Items.Count)
                return;

            if (list.Items[sel] is Serial)
            {
                Serial s = (Serial)list.Items[sel];
                Item item = World.FindItem(s);
                if (item != null)
                {
                    list.Items[sel] = item.ItemID;
                    dressItems.BeginUpdate();
                    dressItems.Items[sel] = item.ItemID.ToString();
                    dressItems.EndUpdate();
                }
            }
        }

        private static char[] m_InvalidNameChars = new char[] { '/', '\\', ';', '?', ':', '*' };
        private void newMacro_Click(object sender, System.EventArgs e)
        {
            if (InputBox.Show(this, Language.GetString(LocString.NewMacro), Language.GetString(LocString.EnterAName)))
            {
                string name = InputBox.GetString();
                if (name == null || name == "" || name.IndexOfAny(Path.GetInvalidPathChars()) != -1 || name.IndexOfAny(m_InvalidNameChars) != -1)
                {
                    MessageBox.Show(this, Language.GetString(LocString.InvalidChars), Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                TreeNode node = GetMacroDirNode();
                string path = (node == null || !(node.Tag is string)) ? Config.GetUserDirectory("Macros") : (string)node.Tag;
                path = Path.Combine(path, name + ".macro");
                if (File.Exists(path))
                {
                    MessageBox.Show(this, Language.GetString(LocString.MacroExists), Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    bool createFromClipboard = false;

                    // TODO: Instead of contains, do we want to look at the first X number of characters
                    if (Clipboard.GetText().Contains("Assistant.Macros."))
                    {
                        createFromClipboard = MessageBox.Show(this, Language.GetString(LocString.NewClipboardMacro), "Create new macro from clipboard?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                        if (createFromClipboard)
                        {
                            string[] macroCommands = Clipboard.GetText().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                            File.WriteAllLines(path, macroCommands);

                            Clipboard.Clear();
                        }
                        else
                        {
                            File.CreateText(path).Close();
                        }

                    }

                    // If they didn't create from clipboard, create empty macro
                    if (!createFromClipboard)
                    {
                        File.CreateText(path).Close();
                    }


                }
                catch
                {
                    return;
                }

                Macro m = new Macro(path);
                MacroManager.Add(m);
                TreeNode newNode = new TreeNode(Path.GetFileNameWithoutExtension(m.Filename));
                newNode.Tag = m;
                if (node == null)
                    macroTree.Nodes.Add(newNode);
                else
                    node.Nodes.Add(newNode);
                macroTree.SelectedNode = newNode;
            }

            RedrawMacros();
        }

        public Macro GetMacroSel()
        {
            if (macroTree.SelectedNode == null || !(macroTree.SelectedNode.Tag is Macro))
                return null;
            else
                return (Macro)macroTree.SelectedNode.Tag;
        }

        public void playMacro_Click(object sender, System.EventArgs e)
        {
            if (World.Player == null)
                return;

            // Playing is true if the timer is running which in a step-through scenario isn't true
            if (MacroManager.Playing || MacroManager.StepThrough)
            {
                MacroManager.Stop();
                nextMacroAction.Enabled = false;
            }
            else
            {
                Macro m = GetMacroSel();
                if (m == null || m.Actions.Count <= 0)
                    return;

                // Check if we're going to step through the macro
                nextMacroAction.Enabled = stepThroughMacro.Checked;
                m.StepThrough = stepThroughMacro.Checked;

                actionList.SelectedIndex = 0;
                MacroManager.Play(m);
                playMacro.Text = "Stop";
                recMacro.Enabled = false;
                OnMacroStart(m);
            }
        }

        private void recMacro_Click(object sender, System.EventArgs e)
        {
            if (World.Player == null)
                return;

            if (MacroManager.Recording)
            {
                MacroManager.Stop();
                //OnMacroStop();
            }
            else
            {
                Macro m = GetMacroSel();
                if (m == null)
                    return;

                bool rec = true;
                if (m.Actions.Count > 0)
                    rec = MessageBox.Show(this, Language.GetString(LocString.MacroConfRec), "Overwrite?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                if (rec)
                {
                    MacroManager.Record(m);
                    OnMacroStart(m);
                    recMacro.Text = "Stop";
                    playMacro.Enabled = false;
                }
            }
        }

        public void OnMacroStart(Macro m)
        {
            actionList.SelectedIndex = -1;
            macroTree.Enabled = actionList.Enabled = false;
            newMacro.Enabled = delMacro.Enabled = false;
            //macroList.SelectedItem = m;
            macroTree.SelectedNode = FindNode(macroTree.Nodes, m);
            macroTree.Update();
            macroTree.Refresh();
            m.DisplayTo(actionList);
        }

        public void PlayMacro(Macro m)
        {
            playMacro.Text = "Stop";
            recMacro.Enabled = false;
        }

        public void OnMacroStop()
        {
            recMacro.Text = "Record";
            recMacro.Enabled = true;
            playMacro.Text = "Play";
            playMacro.Enabled = true;
            actionList.SelectedIndex = -1;
            macroTree.Enabled = actionList.Enabled = true;
            newMacro.Enabled = delMacro.Enabled = true;
            nextMacroAction.Enabled = false;
            RedrawMacros();
        }

        private ContextMenu m_MacroContextMenu = null;
        private void macroTree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                if (m_MacroContextMenu == null)
                {
                    m_MacroContextMenu = new ContextMenu(new MenuItem[]
                         {
                                   new MenuItem( "Add Category", new EventHandler( Macro_AddCategory ) ),
                                   new MenuItem( "Delete Category", new EventHandler( Macro_DeleteCategory ) ),
                                   new MenuItem( "Move to Category", new EventHandler( Macro_Move2Category ) ),
                                  new MenuItem( "-" ),
                            new MenuItem( "Copy to Clipboard", new EventHandler( Macro_CopyToClipboard ) ),
                                  new MenuItem( "Rename Macro", new EventHandler( Macro_Rename ) ),
                                  new MenuItem( "Open Externally", new EventHandler( Open_Externally ) ),
                            new MenuItem( "-" ),
                                   new MenuItem( "Refresh Macro List", new EventHandler( Macro_RefreshList ) )
                    });
                }

                Macro sel = GetMacroSel();

                m_MacroContextMenu.MenuItems[1].Enabled = sel == null;
                m_MacroContextMenu.MenuItems[2].Enabled = sel != null;

                m_MacroContextMenu.Show(this, new Point(e.X, e.Y));
            }

            //RedrawMacros();
        }

        private TreeNode GetMacroDirNode()
        {
            if (macroTree.SelectedNode == null)
            {
                return null;
            }
            else
            {
                if (macroTree.SelectedNode.Tag is string)
                    return macroTree.SelectedNode;
                else if (macroTree.SelectedNode.Parent == null || !(macroTree.SelectedNode.Parent.Tag is string))
                    return null;
                else
                    return macroTree.SelectedNode.Parent;
            }
        }

        private void Macro_AddCategory(object sender, EventArgs args)
        {
            if (!InputBox.Show(this, Language.GetString(LocString.CatName)))
                return;

            string path = InputBox.GetString();
            if (path == null || path == "" || path.IndexOfAny(Path.GetInvalidPathChars()) != -1 || path.IndexOfAny(m_InvalidNameChars) != -1)
            {
                MessageBox.Show(this, Language.GetString(LocString.InvalidChars), "Invalid Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TreeNode node = GetMacroDirNode();

            try
            {
                if (node == null || !(node.Tag is string))
                    path = Path.Combine(Config.GetUserDirectory("Macros"), path);
                else
                    path = Path.Combine((string)node.Tag, path);
                Engine.EnsureDirectory(path);
            }
            catch
            {
                MessageBox.Show(this, Language.Format(LocString.CanCreateDir, path), "Unabled to Create Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TreeNode newNode = new TreeNode(String.Format("[{0}]", Path.GetFileName(path)));
            newNode.Tag = path;
            if (node == null)
                macroTree.Nodes.Add(newNode);
            else
                node.Nodes.Add(newNode);
            RedrawMacros();
            macroTree.SelectedNode = newNode;
        }

        private void Macro_DeleteCategory(object sender, EventArgs args)
        {
            string path = null;
            if (macroTree.SelectedNode != null)
                path = macroTree.SelectedNode.Tag as string;

            if (path == null)
                return;

            try
            {
                Directory.Delete(path);
            }
            catch
            {
                MessageBox.Show(this, Language.GetString(LocString.CantDelDir), "Unable to Delete Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TreeNode node = FindNode(macroTree.Nodes, path);
            if (node != null)
                node.Remove();
        }

        private void Macro_Move2Category(object sender, EventArgs args)
        {
            Macro sel = GetMacroSel();
            if (sel == null)
                return;

            List<string> dirNames = new List<string>();
            dirNames.Add("<None>");

            foreach (string dir in Directory.GetDirectories(Config.GetUserDirectory("Macros")))
            {
                dirNames.Add(dir.Substring(dir.LastIndexOf('\\') + 1));
            }

            if (!InputDropdown.Show(this, Language.GetString(LocString.CatName), dirNames.ToArray()))
                return;

            try
            {
                File.Move(sel.Filename, InputDropdown.GetString().Equals("<None>")
                        ? Path.Combine(Config.GetUserDirectory("Macros"), $"{Path.GetFileName(sel.Filename)}")
                       : Path.Combine(Config.GetUserDirectory("Macros"), $"{InputDropdown.GetString()}/{Path.GetFileName(sel.Filename)}"));
            }
            catch
            {
                MessageBox.Show(this, Language.GetString(LocString.CantMoveMacro), "Unable to Move Macro",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            RedrawMacros();
            macroTree.SelectedNode = FindNode(macroTree.Nodes, sel);
        }

        /// <summary>
        /// Copy the selected macro the user's clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Macro_CopyToClipboard(object sender, EventArgs args)
        {
            Macro sel = GetMacroSel();
            if (sel == null)
                return;

            try
            {
                string[] macroLines = File.ReadAllLines(sel.Filename);

                Clipboard.SetText(string.Join(Environment.NewLine, macroLines));
            }
            catch
            {
                MessageBox.Show(this, Language.GetString(LocString.ReadError), "Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Macro_Rename(object sender, EventArgs args)
        {
            Macro sel = GetMacroSel();
            if (sel == null)
                return;

            if (InputBox.Show(this, Language.GetString(LocString.MacroRename), Language.GetString(LocString.EnterAName)))
            {
                string name = InputBox.GetString();
                if (string.IsNullOrEmpty(name) || name.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                    name.IndexOfAny(m_InvalidNameChars) != -1)
                {
                    MessageBox.Show(this, Language.GetString(LocString.InvalidChars),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                TreeNode node = GetMacroDirNode();

                string newMacro = $"{Path.GetDirectoryName(sel.Filename)}/{name}.macro";


                if (File.Exists(newMacro))
                {
                    MessageBox.Show(this, Language.GetString(LocString.MacroExists), Language.GetString(LocString.Invalid),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    File.Move(sel.Filename, newMacro);
                }
                catch
                {
                    return;
                }

                /*Macro m = new Macro(newMacro);
	            MacroManager.Add(m);
	            TreeNode newNode = new TreeNode(Path.GetFileNameWithoutExtension(m.Filename));
	            newNode.Tag = m;
	            if (node == null)
	                macroTree.Nodes.Add(newNode);
	            else
	                node.Nodes.Add(newNode);
	            macroTree.SelectedNode = newNode;*/

                RedrawMacros();
            }


        }

        private void Open_Externally(object sender, EventArgs args)
        {
            Macro sel = GetMacroSel();
            if (sel == null)
                return;

            try
            {
                Process.Start(sel.Filename);
            }
            catch (Exception)
            {
                MessageBox.Show(this, Language.GetString(LocString.UnableToOpenMacro), Language.GetString(LocString.ReadError),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Macro_RefreshList(object sender, EventArgs args)
        {
            RedrawMacros();
        }

        private static TreeNode FindNode(TreeNodeCollection nodes, object tag)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                TreeNode node = nodes[i];

                if (node.Tag == tag)
                {
                    return node;
                }
                else if (node.Nodes.Count > 0)
                {
                    node = FindNode(node.Nodes, tag);
                    if (node != null)
                        return node;
                }
            }

            return null;
        }

        private void RedrawMacros()
        {
            Macro ms = GetMacroSel();
            MacroManager.DisplayTo(macroTree);
            if (ms != null)
                macroTree.SelectedNode = FindNode(macroTree.Nodes, ms);

            MacroManager.DisplayAbsoluteTargetsTo(absoluteTargets);
        }

        private void macroTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (MacroManager.Recording)
                return;

            Macro m = e.Node.Tag as Macro;
            macroActGroup.Visible = m != null;
            MacroManager.Select(m, actionList, playMacro, recMacro, loopMacro);
        }

        private void delMacro_Click(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
            {
                Macro_DeleteCategory(sender, e);
                return;
            }

            if (m == MacroManager.Current)
                return;

            if (m.Actions.Count <= 0 || MessageBox.Show(this, Language.Format(LocString.DelConf, m.ToString()), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    File.Delete(m.Filename);
                }
                catch
                {
                    return;
                }

                MacroManager.Remove(m);

                TreeNode node = FindNode(macroTree.Nodes, m);
                if (node != null)
                    node.Remove();
            }
        }

        private void actionList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                    return;

                ContextMenu menu = new ContextMenu();
                menu.MenuItems.Add(Language.GetString(LocString.Reload), new EventHandler(onMacroReload));
                menu.MenuItems.Add(Language.GetString(LocString.Save), new EventHandler(onMacroSave));

                MacroAction a;
                try
                {
                    a = actionList.SelectedItem as MacroAction;
                }
                catch
                {
                    a = null;
                }

                if (a != null)
                {
                    int pos = actionList.SelectedIndex;

                    menu.MenuItems.Add("-");
                    if (actionList.Items.Count > 1)
                    {
                        menu.MenuItems.Add(Language.GetString(LocString.MoveUp), new EventHandler(OnMacroActionMoveUp));

                        if (pos <= 0)
                        {
                            menu.MenuItems[menu.MenuItems.Count - 1].Enabled = false;
                        }

                        menu.MenuItems.Add(Language.GetString(LocString.MoveDown),
                            new EventHandler(OnMacroActionMoveDown));

                        if (pos >= actionList.Items.Count - 1)
                        {
                            menu.MenuItems[menu.MenuItems.Count - 1].Enabled = false;
                        }

                        menu.MenuItems.Add("-");
                    }

                    menu.MenuItems.Add(Language.GetString(LocString.RemAct), new EventHandler(onMacroActionDelete));
                    menu.MenuItems.Add("-");
                    menu.MenuItems.Add(Language.GetString(LocString.BeginRec), new EventHandler(onMacroBegRecHere));
                    menu.MenuItems.Add(Language.GetString(LocString.PlayFromHere), new EventHandler(onMacroPlayHere));

                    MenuItem[] aMenus = a.GetContextMenuItems();
                    if (aMenus != null && aMenus.Length > 0)
                    {
                        menu.MenuItems.Add("-");
                        menu.MenuItems.AddRange(aMenus);
                    }
                }

                menu.MenuItems.Add("-");
                menu.MenuItems.Add(Language.GetString(LocString.Constructs), new MenuItem[]
                {
                     new MenuItem(Language.GetString(LocString.InsWait), new EventHandler(onMacroInsPause)),
                     new MenuItem(Language.GetString(LocString.InsLT), new EventHandler(onMacroInsertSetLT)),
                     new MenuItem(Language.GetString(LocString.InsComment), new EventHandler(onMacroInsertComment)),
                     new MenuItem("-"),
                     new MenuItem(Language.GetString(LocString.InsIF), new EventHandler(onMacroInsertIf)),
                     new MenuItem(Language.GetString(LocString.InsELSE), new EventHandler(onMacroInsertElse)),
                     new MenuItem(Language.GetString(LocString.InsENDIF), new EventHandler(onMacroInsertEndIf)),
                     new MenuItem("-"),
                     new MenuItem(Language.GetString(LocString.InsFOR), new EventHandler(onMacroInsertFor)),
                     new MenuItem(Language.GetString(LocString.InsENDFOR), new EventHandler(onMacroInsertEndFor))
                });

                menu.Show(actionList, new Point(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                /*if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                    return;*/

                MacroAction a;
                try
                {
                    a = actionList.SelectedItem as MacroAction;
                }
                catch
                {
                    a = null;
                }

                if (a == null)
                    return;

                MenuItem[] aMenus = a.GetContextMenuItems();

                if (aMenus != null && aMenus.Length > 0)
                {
                    if (aMenus[0].Text.Equals(Language.GetString(LocString.Edit)))
                    {
                        if (a.GetType().Name.Equals("IfAction"))
                        {
                            new MacroInsertIf(a).ShowDialog(Engine.MainWindow);
                        }
                        else if (a.GetType().Name.Equals("ForAction"))
                        {
                            aMenus[0].PerformClick();
                        }
                        else if (a.GetType().Name.Equals("MacroComment"))
                        {
                            aMenus[0].PerformClick();
                        }
                        else if (a.GetType().Name.Equals("SpeechAction"))
                        {
                            aMenus[0].PerformClick();
                        }
                        else if (a.GetType().Name.Equals("GumpResponseAction"))
                        {
                            aMenus[0].PerformClick();
                        }
                        else
                        {
                            new MacroInsertWait(a).ShowDialog(Engine.MainWindow);
                        }


                    }
                }
            }

        }

        private void onMacroPlayHere(object sender, EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int sel = actionList.SelectedIndex + 1;
            if (sel < 0 || sel > m.Actions.Count)
                sel = m.Actions.Count;

            MacroManager.PlayAt(m, sel);
            playMacro.Text = "Stop";
            nextMacroAction.Enabled = stepThroughMacro.Checked;
            recMacro.Enabled = false;

            OnMacroStart(m);
        }

        private void onMacroInsertComment(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            if (InputBox.Show(Language.GetString(LocString.InsComment)))
            {
                m.Actions.Insert(a + 1, new MacroComment(InputBox.GetString()));
                RedrawActionList(m);
            }
        }

        private void onMacroInsertIf(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            MacroInsertIf ins = new MacroInsertIf(m, a);
            if (ins.ShowDialog(this) == DialogResult.OK)
                RedrawActionList(m);
        }

        private void onMacroInsertElse(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            m.Actions.Insert(a + 1, new ElseAction());
            RedrawActionList(m);
        }

        private void onMacroInsertEndIf(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            m.Actions.Insert(a + 1, new EndIfAction());
            RedrawActionList(m);
        }

        private void onMacroInsertFor(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            if (InputBox.Show(Language.GetString(LocString.NumIter)))
            {
                m.Actions.Insert(a + 1, new ForAction(InputBox.GetInt(1)));
                RedrawActionList(m);
            }
        }

        private void onMacroInsertEndFor(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            m.Actions.Insert(a + 1, new EndForAction());
            RedrawActionList(m);
        }

        private void OnMacroActionMoveUp(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int move = actionList.SelectedIndex;
            if (move > 0 && move < m.Actions.Count)
            {
                MacroAction a = (MacroAction)m.Actions[move - 1];
                m.Actions[move - 1] = m.Actions[move];
                m.Actions[move] = a;

                RedrawActionList(m);
                actionList.SelectedIndex = move - 1;
            }
        }

        private void OnMacroActionMoveDown(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int move = actionList.SelectedIndex;
            if (move + 1 < m.Actions.Count)
            {
                MacroAction a = (MacroAction)m.Actions[move + 1];
                m.Actions[move + 1] = m.Actions[move];
                m.Actions[move] = a;

                RedrawActionList(m);
                actionList.SelectedIndex = move + 1;
            }
        }


        private void OnMacroKeyMoveUp()
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int move = actionList.SelectedIndex;
            if (move > 0 && move < m.Actions.Count)
            {
                MacroAction a = (MacroAction)m.Actions[move - 1];
                m.Actions[move - 1] = m.Actions[move];
                m.Actions[move] = a;

                RedrawActionList(m);
            }
        }

        private void OnMacroKeyMoveDown()
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int move = actionList.SelectedIndex;
            if (move + 1 < m.Actions.Count)
            {
                MacroAction a = (MacroAction)m.Actions[move + 1];
                m.Actions[move + 1] = m.Actions[move];
                m.Actions[move] = a;

                RedrawActionList(m);
            }
        }

        private void RedrawActionList(Macro m)
        {
            int sel = actionList.SelectedIndex;
            m.DisplayTo(actionList);
            actionList.SelectedIndex = sel;
        }

        private void actionList_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                onMacroActionDelete(sender, e);

                return;
            }

            int origIndex = actionList.SelectedIndex;

            if ((e.KeyCode == Keys.Up && e.Control) && actionList.SelectedIndex > 0)
            {
                OnMacroKeyMoveUp();

                return;
            }

            if ((e.KeyCode == Keys.Down && e.Control) && actionList.SelectedIndex < actionList.Items.Count)
            {
                OnMacroKeyMoveDown();

                return;
            }
        }

        private void onMacroActionDelete(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a < 0 || a >= m.Actions.Count)
                return;

            if (MessageBox.Show(this, Language.Format(LocString.DelConf, m.Actions[a].ToString()), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m.Actions.RemoveAt(a);
                actionList.Items.RemoveAt(a);
            }
        }

        private void onMacroBegRecHere(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int sel = actionList.SelectedIndex + 1;
            if (sel < 0 || sel > m.Actions.Count)
                sel = m.Actions.Count;

            MacroManager.RecordAt(m, sel);
            recMacro.Text = "Stop";
            playMacro.Enabled = false;
            OnMacroStart(m);
        }

        private void onMacroInsPause(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            MacroInsertWait ins = new MacroInsertWait(m, a);
            if (ins.ShowDialog(this) == DialogResult.OK)
                RedrawActionList(m);
        }

        private void onMacroReload(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            m.Load();
            RedrawActionList(m);
        }

        private void onMacroSave(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            m.Save();
            RedrawActionList(m);
        }

        private void onMacroInsertSetLT(object sender, EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            m.Actions.Insert(a + 1, new SetLastTargetAction());
            RedrawActionList(m);
        }

        private void loopMacro_CheckedChanged(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel(); ;
            if (m == null)
                return;
            m.Loop = loopMacro.Checked;
        }

        private void spamFilter_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("FilterSpam", spamFilter.Checked);
        }

        private void jump2SearchEx_Click(object sender, System.EventArgs e)
        {
            tabs.SelectedTab = agentsTab;
            agentList.SelectedItem = SearchExemptionAgent.Instance;
        }

        private void screenAutoCap_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("AutoCap", screenAutoCap.Checked);
        }

        private void setScnPath_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = Language.GetString(LocString.SelSSFolder);
            folder.SelectedPath = Config.GetString("CapPath");
            folder.ShowNewFolderButton = true;

            if (folder.ShowDialog(this) == DialogResult.OK)
            {
                Config.SetProperty("CapPath", folder.SelectedPath);
                screenPath.Text = folder.SelectedPath;

                ReloadScreenShotsList();
            }
        }

        public void ReloadScreenShotsList()
        {
            ScreenCapManager.DisplayTo(screensList);
            if (screenPrev.Image != null)
            {
                screenPrev.Image.Dispose();
                screenPrev.Image = null;
            }

            ReloadImgurUploadList();
        }

        public List<ImgurUpload> m_ImgurUploads = new List<ImgurUpload>();

        public class ImgurUpload
        {
            public string Url { get; set; }
            public string DeleteHash { get; set; }
            public string UploadTime { get; set; }
        }

        public void ReloadImgurUploadList()
        {
            imgurUploads.Items.Clear();
            m_ImgurUploads.Clear();

            if (!File.Exists($"{Config.GetInstallDirectory()}\\ImgurUploads.csv"))
                return;

            string[] lines = File.ReadAllLines($"{Config.GetInstallDirectory()}\\ImgurUploads.csv");

            foreach (string line in lines)
            {
                string[] splitLine = line.Split(',');

                ImgurUpload upload = new ImgurUpload
                {
                    Url = splitLine[0],
                    DeleteHash = splitLine[1],
                    UploadTime = splitLine[2]
                };

                m_ImgurUploads.Add(upload);
            }

            foreach (ImgurUpload upload in m_ImgurUploads)
            {
                imgurUploads.Items.Add(upload.Url);
            }
        }

        private void imgurUploads_MouseDown(object sender, MouseEventArgs e)
        {
            if (imgurUploads.SelectedIndex < 0)
                return;

            if (e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                ContextMenu menu = new ContextMenu();

                m_LastImgurUrl = imgurUploads.Items[imgurUploads.SelectedIndex].ToString();

                menu.MenuItems.Add("Copy URL to clipboard", new EventHandler(CopyImgurUrlToClipboard));
                menu.MenuItems.Add("Open URL in browser", new EventHandler(razorNotify_BalloonTipClicked));
                menu.MenuItems.Add("-");
                menu.MenuItems.Add("Delete from Imgur", new EventHandler(DeleteFromImgur));

                menu.Show(imgurUploads, new Point(e.X, e.Y));
            }
        }

        private void DeleteFromImgur(object sender, System.EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, "Are you sure you want to delete this from Imgur? It will permanently be removed and cannot be undone.", "Delete Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                using (var w = new WebClient()) //HttpClient would be preferred, but I didn't want to add a new package to Razor
                {
                    string clientID = "b241fb37ce1e0e9";
                    w.Headers.Add("Authorization", "Client-ID " + clientID);

                    string deleteHash = m_ImgurUploads[imgurUploads.SelectedIndex].DeleteHash;

                    string response = w.UploadString($"https://api.imgur.com/3/image/{deleteHash}", "DELETE", "");

                    dynamic jsonResponse = JsonConvert.DeserializeObject(response);

                    if (jsonResponse["status"] == 200)
                    {
                        m_ImgurUploads.RemoveAt(imgurUploads.SelectedIndex);

                        SaveImgurUploadsToLog();

                        ReloadImgurUploadList();
                    }
                    else
                    {
                        throw new Exception("Unable to delete image");
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error deleting image on Imgur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void CopyImgurUrlToClipboard(object sender, System.EventArgs e)
        {
            Clipboard.SetText(m_LastImgurUrl);
        }

        private void radioFull_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioFull.Checked)
            {
                radioUO.Checked = false;
                Config.SetProperty("CapFullScreen", true);
            }
        }

        private void radioUO_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioUO.Checked)
            {
                radioFull.Checked = false;
                Config.SetProperty("CapFullScreen", false);
            }
        }

        private void screensList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (screenPrev.Image != null)
            {
                screenPrev.Image.Dispose();
                screenPrev.Image = null;
            }

            if (screensList.SelectedIndex == -1)
                return;

            string file = Path.Combine(Config.GetString("CapPath"), screensList.SelectedItem.ToString());
            if (!File.Exists(file))
            {
                MessageBox.Show(this, Language.Format(LocString.FileNotFoundA1, file), "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                screensList.Items.RemoveAt(screensList.SelectedIndex);
                screensList.SelectedIndex = -1;
                return;
            }

            using (Stream reader = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                screenPrev.Image = Image.FromStream(reader);
            }
        }

        private void screensList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                ContextMenu menu = new ContextMenu();

                menu.MenuItems.Add("Upload to Imgur", new EventHandler(UploadToImgurEvent));
                menu.MenuItems.Add("-");
                menu.MenuItems.Add("Delete", new EventHandler(DeleteScreenCap));

                if (screensList.SelectedIndex == -1)
                    menu.MenuItems[menu.MenuItems.Count - 1].Enabled = false;

                menu.MenuItems.Add("Delete ALL", new EventHandler(ClearScreensDirectory));



                menu.Show(screensList, new Point(e.X, e.Y));
            }
        }

        private void UploadToImgurEvent(object sender, System.EventArgs e)
        {
            int sel = screensList.SelectedIndex;
            if (sel == -1)
                return;

            string file = Path.Combine(Config.GetString("CapPath"), (string)screensList.SelectedItem);


            // This is .NET 4.0, can't use async Task.Run
            Task T = Task.Factory.StartNew(() => { UploadToImgur(file); });
        }

        private void UploadToImgur(string file)
        {
            try
            {
                using (var w = new WebClient())
                {
                    string clientID = "b241fb37ce1e0e9";
                    w.Headers.Add("Authorization", "Client-ID " + clientID);
                    var values = new NameValueCollection
                    {
                        { "image", Convert.ToBase64String(File.ReadAllBytes(file)) }
                    };

                    byte[] response = w.UploadValues("https://api.imgur.com/3/upload", values);

                    dynamic jsonResponse = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(response));

                    if (jsonResponse["status"] == 200)
                    {
                        m_LastImgurUrl = jsonResponse["data"].link.Value;

                        imgurUploads.Invoke((MethodInvoker)delegate
                        {
                            // Running on the UI thread
                            LogImgurUpload(m_LastImgurUrl, jsonResponse["data"].deletehash.Value);

                            if (Config.GetBool("ScreenshotUploadNotifications"))
                            {
                                m_NotifyIcon.ShowBalloonTip(2000, "Screenshot Uploaded", "Click here to view in your browser.", ToolTipIcon.Info);
                                m_NotifyIcon.BalloonTipClicked += new EventHandler(razorNotify_BalloonTipClicked);
                            }

                            if (Config.GetBool("ScreenshotUploadOpenBrowser"))
                            {
                                Process.Start(m_LastImgurUrl);
                            }

                            if (Config.GetBool("ScreenshotUploadClipboard"))
                            {
                                Clipboard.SetText(m_LastImgurUrl);
                            }
                        });
                    }
                    else
                    {
                        throw new Exception("Unable to upload");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error Uploading to Imgur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private string m_LastImgurUrl;

        private void razorNotify_BalloonTipClicked(object sender, EventArgs e)
        {
            Process.Start(m_LastImgurUrl);
        }

        private void LogImgurUpload(string url, string deleteHash)
        {
            try
            {
                string path = $"{Config.GetInstallDirectory()}\\ImgurUploads.csv";
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine($"{url},{deleteHash},{DateTime.Now}");
                }

                ReloadImgurUploadList();
            }
            catch (Exception ex)
            {
            }
        }

        private void SaveImgurUploadsToLog()
        {
            try
            {
                if (File.Exists($"{Config.GetInstallDirectory()}\\ImgurUploads.csv"))
                {
                    File.Delete($"{Config.GetInstallDirectory()}\\ImgurUploads.csv");
                }

                string path = $"{Config.GetInstallDirectory()}\\ImgurUploads.csv";

                using (StreamWriter sw = File.AppendText(path))
                {
                    foreach (ImgurUpload upload in m_ImgurUploads)
                    {
                        sw.WriteLine($"{upload.Url},{upload.DeleteHash},{upload.UploadTime}");
                    }
                }

                ReloadImgurUploadList();

            }
            catch (Exception ex)
            {

            }
        }

        private void DeleteScreenCap(object sender, System.EventArgs e)
        {
            int sel = screensList.SelectedIndex;
            if (sel == -1)
                return;

            string file = Path.Combine(Config.GetString("CapPath"), (string)screensList.SelectedItem);
            if (MessageBox.Show(this, Language.Format(LocString.DelConf, file), "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            screensList.SelectedIndex = -1;
            if (screenPrev.Image != null)
            {
                screenPrev.Image.Dispose();
                screenPrev.Image = null;
            }

            try
            {
                File.Delete(file);
                screensList.Items.RemoveAt(sel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unable to Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ReloadScreenShotsList();
        }

        private void ClearScreensDirectory(object sender, System.EventArgs e)
        {
            string dir = Config.GetString("CapPath");
            if (MessageBox.Show(this, Language.Format(LocString.Confirm, dir), "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            string[] files = Directory.GetFiles(dir, "*.jpg");
            StringBuilder sb = new StringBuilder();
            int failed = 0;
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    File.Delete(files[i]);
                }
                catch
                {
                    sb.AppendFormat("{0}\n", files[i]);
                    failed++;
                }
            }

            if (failed > 0)
                MessageBox.Show(this, Language.Format(LocString.FileDelError, failed, failed != 1 ? "s" : "", sb.ToString()), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ReloadScreenShotsList();
        }

        private void capNow_Click(object sender, System.EventArgs e)
        {
            ScreenCapManager.CaptureNow();
        }

        private void dispTime_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("CapTimeStamp", dispTime.Checked);
        }

        public static void LaunchBrowser(string site)
        {
            try
            {
                System.Diagnostics.Process.Start(site);//"iexplore", site );
            }
            catch
            {
                MessageBox.Show(String.Format("Unable to open browser to '{0}'", site), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void donate_Click(object sender, System.EventArgs e)
        {
            LaunchBrowser("https://www.paypal.com/xclick/business=zippy%40runuo.com&item_name=Razor&no_shipping=1&no_note=1&tax=0&currency_code=USD");
        }

        private void undressConflicts_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("UndressConflicts", undressConflicts.Checked);
        }

        private void taskbar_CheckedChanged(object sender, System.EventArgs e)
        {
            if (taskbar.Checked)
            {
                systray.Checked = false;
                Config.SetProperty("Systray", false);
                if (!this.ShowInTaskbar)
                    MessageBox.Show(this, Language.GetString(LocString.NextRestart), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void systray_CheckedChanged(object sender, System.EventArgs e)
        {
            if (systray.Checked)
            {
                taskbar.Checked = false;
                Config.SetProperty("Systray", true);
                if (this.ShowInTaskbar)
                    MessageBox.Show(this, Language.GetString(LocString.NextRestart), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void UpdateTitle()
        {
            string str = Language.GetControlText(this.Name);
            if (string.IsNullOrEmpty(str))
            {
                str = "Razor v{0}";
            }


            str = string.Format(str, Engine.Version);

            if (World.Player != null)
            {
                string title = $"{World.Player.Name} ({World.ShardName}) - {str}";
                Text = title;
            }
            else
            {
                Text = str;
            }

            aboutVer.Text = str;


            UpdateSystray();
        }

        public void UpdateSystray()
        {
            if (m_NotifyIcon != null && m_NotifyIcon.Visible)
            {
                if (World.Player != null)
                    m_NotifyIcon.Text = String.Format("Razor - {0} ({1})", World.Player.Name, World.ShardName);
                else
                    m_NotifyIcon.Text = "Razor";
            }
        }

        private void DoShowMe(object sender, System.EventArgs e)
        {
            ShowMe();
        }

        public void ShowMe()
        {
            // Fuck windows, seriously.

            ClientCommunication.BringToFront(this.Handle);
            if (Config.GetBool("AlwaysOnTop"))
                this.TopMost = true;
            if (WindowState != FormWindowState.Normal)
                WindowState = FormWindowState.Normal;
        }

        private void HideMe(object sender, System.EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;
            this.TopMost = false;
            this.SendToBack();
            this.Hide();
        }

        private void NotifyIcon_DoubleClick(object sender, System.EventArgs e)
        {
            ShowMe();
        }

        private void ToggleVisible(object sender, System.EventArgs e)
        {
            if (this.Visible)
                HideMe(sender, e);
            else
                ShowMe();
        }

        private void OnClose(object sender, System.EventArgs e)
        {
            m_CanClose = true;
            this.Close();
        }

        private void titlebarImages_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("TitlebarImages", titlebarImages.Checked);
            ClientCommunication.RequestTitlebarUpdate();
        }

        private void highlightSpellReags_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("HighlightReagents", highlightSpellReags.Checked);
            ClientCommunication.RequestTitlebarUpdate();
        }

        private void actionStatusMsg_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("ActionStatusMsg", actionStatusMsg.Checked);
        }

        private void autoStackRes_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("AutoStack", autoStackRes.Checked);
            //setAutoStackDest.Enabled = autoStackRes.Checked;
        }

        private void screenPath_TextChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("CapPath", screenPath.Text);
        }

        private void rememberPwds_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rememberPwds.Checked && !Config.GetBool("RememberPwds"))
            {
                if (MessageBox.Show(this, Language.GetString(LocString.PWWarn), "Security Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    rememberPwds.Checked = false;
                    return;
                }
            }
            Config.SetProperty("RememberPwds", rememberPwds.Checked);
        }

        private void langSel_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string lang = langSel.SelectedItem as String;
            if (lang != null && lang != Language.Current)
            {
                if (!Language.Load(lang))
                {
                    MessageBox.Show(this, "Unable to load that language.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    langSel.SelectedItem = Language.Current;
                }
                else
                {
                    Config.SetAppSetting("DefaultLanguage", Language.Current);
                    Language.LoadControlNames(this);
                    HotKey.RebuildList(hotkeyTree);
                }
            }
        }

        private void tabs_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            HotKey.KeyDown(e.KeyData);
        }

        private void MainForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            HotKey.KeyDown(e.KeyData);
        }

        private void spellUnequip_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("SpellUnequip", spellUnequip.Checked);
        }

        private void rangeCheckLT_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("RangeCheckLT", ltRange.Enabled = rangeCheckLT.Checked);
        }

        private void ltRange_TextChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("LTRange", Utility.ToInt32(ltRange.Text, 11));
        }

        private void excludePouches_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("NoSearchPouches", excludePouches.Checked);
        }

        private void clientPrio_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string str = (string)clientPrio.SelectedItem;
            Config.SetProperty("ClientPrio", str);
            try
            {
                ClientCommunication.ClientProcess.PriorityClass = (System.Diagnostics.ProcessPriorityClass)Enum.Parse(typeof(System.Diagnostics.ProcessPriorityClass), str, true);
            }
            catch
            {
            }
        }

        private void filterSnoop_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("FilterSnoopMsg", filterSnoop.Checked);
        }

        private void preAOSstatbar_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("OldStatBar", preAOSstatbar.Checked);
            ClientCommunication.RequestStatbarPatch(preAOSstatbar.Checked);
            if (World.Player != null && !m_Initializing)
                MessageBox.Show(this, "Close and re-open your status bar for the change to take effect.", "Status Window Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void smartLT_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("SmartLastTarget", smartLT.Checked);
        }

        private void showtargtext_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("LastTargTextFlags", showtargtext.Checked);
        }

        private void dressItems_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DressList list = (DressList)dressList.SelectedItem;
                if (list == null)
                    return;

                int sel = dressItems.SelectedIndex;
                if (sel < 0 || sel >= list.Items.Count)
                    return;

                if (MessageBox.Show(this, Language.GetString(LocString.DelDressItemQ), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        list.Items.RemoveAt(sel);
                        dressItems.Items.RemoveAt(sel);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void blockDis_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("BlockDismount", blockDis.Checked);
        }

        private void imgFmt_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (imgFmt.SelectedIndex != -1)
                Config.SetProperty("ImageFormat", imgFmt.SelectedItem);
            else
                Config.SetProperty("ImageFormat", "jpg");
        }

        private void vidRec_Click(object sender, System.EventArgs e)
        {
            if (!PacketPlayer.Playing)
            {
                if (PacketPlayer.Recording)
                    PacketPlayer.Stop();
                else
                    PacketPlayer.Record();
            }
        }

        private void recFolder_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Select Recording Folder";//Language.GetString( LocString.SelRecFolder );
            folder.SelectedPath = Config.GetString("RecFolder");
            folder.ShowNewFolderButton = true;

            if (folder.ShowDialog(this) == DialogResult.OK)
            {
                Config.SetProperty("RecFolder", folder.SelectedPath);
                txtRecFolder.Text = folder.SelectedPath;
            }
        }

        private void vidPlay_Click(object sender, System.EventArgs e)
        {
            if (!PacketPlayer.Playing)
                PacketPlayer.Play();
            else
                PacketPlayer.Pause();
        }

        private void vidPlayStop_Click(object sender, System.EventArgs e)
        {
            if (PacketPlayer.Playing)
                PacketPlayer.Stop();
        }

        private void vidOpen_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = false;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DefaultExt = "rpv";
            ofd.DereferenceLinks = true;
            ofd.Filter = "Razor PacketVideo (*.rpv)|*.rpv|All Files (*.*)|*.*";
            ofd.FilterIndex = 0;
            ofd.InitialDirectory = Config.GetString("RecFolder");
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.ShowHelp = ofd.ShowReadOnly = false;
            ofd.Title = "Select a Video File...";
            ofd.ValidateNames = true;

            if (ofd.ShowDialog(this) == DialogResult.OK)
                PacketPlayer.Open(ofd.FileName);
        }

        private void playPos_Scroll(object sender, System.EventArgs e)
        {
            PacketPlayer.OnScroll();
        }

        private void txtRecFolder_TextChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("RecFolder", txtRecFolder.Text);
        }

        private void vidClose_Click(object sender, System.EventArgs e)
        {
            PacketPlayer.Close();
        }

        private void playSpeed_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            PacketPlayer.SetSpeed(playSpeed.SelectedIndex - 2);
        }

        private void recAVI_Click(object sender, System.EventArgs e)
        {
            if (!AVIRec.Recording)
            {
                double res = 1.00;
                switch (Config.GetInt("AviRes"))
                {
                    case 1: res = 0.75; break;
                    case 2: res = 0.50; break;
                    case 3: res = 0.25; break;
                }
                if (AVIRec.Record(Config.GetInt("AviFPS"), res))
                {
                    recAVI.Text = "Stop Rec";
                }
            }
            else
            {
                AVIRec.Stop();
                recAVI.Text = "Record AVI Video";
            }
        }

        private void aviFPS_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                int fps = Convert.ToInt32(aviFPS.Text);
                if (fps < 5)
                    fps = 5;
                else if (fps > 30)
                    fps = 30;
                Config.SetProperty("AviFPS", fps);
            }
            catch
            {
            }
        }

        private void aviRes_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("AviRes", aviRes.SelectedIndex);
        }

        private void autoFriend_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("AutoFriend", autoFriend.Checked);
        }

        private void alwaysStealth_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("AlwaysStealth", alwaysStealth.Checked);
        }

        private void autoOpenDoors_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("AutoOpenDoors", autoOpenDoors.Checked);
        }

        private void msglvl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("MessageLevel", msglvl.SelectedIndex);
        }

        private void screenPrev_Click(object sender, System.EventArgs e)
        {
            string file = screensList.SelectedItem as String;
            if (file != null)
                System.Diagnostics.Process.Start(Path.Combine(Config.GetString("CapPath"), file));
        }

        private void flipVidHoriz_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("FlipVidH", flipVidHoriz.Checked);
            AVIRec.UpdateFlip();
        }

        private void flipVidVert_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("FlipVidV", flipVidVert.Checked);
            AVIRec.UpdateFlip();
        }

        private Timer m_ResizeTimer = Timer.DelayedCallback(TimeSpan.FromSeconds(1.0), new TimerCallback(ForceSize));

        private static void ForceSize()
        {
            int x, y;

            if (Config.GetBool("ForceSizeEnabled"))
            {
                x = Config.GetInt("ForceSizeX");
                y = Config.GetInt("ForceSizeY");

                if (x > 100 && x < 2000 && y > 100 && y < 2000)
                    ClientCommunication.SetGameSize(x, y);
                else
                    MessageBox.Show(Engine.MainWindow, Language.GetString(LocString.ForceSizeBad), "Bad Size", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void gameSize_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("ForceSizeEnabled", gameSize.Checked);
            forceSizeX.Enabled = forceSizeY.Enabled = gameSize.Checked;

            if (gameSize.Checked)
            {
                int x = Utility.ToInt32(forceSizeX.Text, 800);
                int y = Utility.ToInt32(forceSizeY.Text, 600);

                if (x < 100 || y < 100 || x > 2000 || y > 2000)
                    MessageBox.Show(this, Language.GetString(LocString.ForceSizeBad), "Bad Size", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else
                    ClientCommunication.SetGameSize(x, y);
            }
            else
            {
                ClientCommunication.SetGameSize(800, 600);
            }

            if (!m_Initializing)
                MessageBox.Show(this, Language.GetString(LocString.ApplyOptionsRequired), "Additional Step", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void forceSizeX_TextChanged(object sender, System.EventArgs e)
        {
            int x = Utility.ToInt32(forceSizeX.Text, 600);
            if (x >= 100 && x <= 2000)
                Config.SetProperty("ForceSizeX", x);

            if (!m_Initializing)
            {
                if (x > 100 && x < 2000)
                {
                    m_ResizeTimer.Stop();
                    m_ResizeTimer.Start();
                }
            }
        }

        private void forceSizeY_TextChanged(object sender, System.EventArgs e)
        {
            int y = Utility.ToInt32(forceSizeY.Text, 600);
            if (y >= 100 && y <= 2000)
                Config.SetProperty("ForceSizeY", y);

            if (!m_Initializing)
            {
                if (y > 100 && y < 2000)
                {
                    m_ResizeTimer.Stop();
                    m_ResizeTimer.Start();
                }
            }
        }

        private void potionEquip_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("PotionEquip", potionEquip.Checked);
        }

        private void blockHealPoison_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("BlockHealPoison", blockHealPoison.Checked);
        }

        private void negotiate_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!m_Initializing)
            {
                Config.SetProperty("Negotiate", negotiate.Checked);
                ClientCommunication.SetNegotiate(negotiate.Checked);
            }
        }

        private void wikiLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            LaunchBrowser("https://github.com/msturgill/razor/wiki");
        }

        private void homeLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            LaunchBrowser("https://github.com/msturgill/razor/wiki");
        }

        private void issuesLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            LaunchBrowser("https://github.com/msturgill/razor/issues");
        }

        private void mleLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LaunchBrowser("http://www.mlewallpapers.com/");
        }

        private void lockBox_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show(this, Language.GetString(LocString.FeatureDisabledText), Language.GetString(LocString.FeatureDisabled), MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private ArrayList m_LockBoxes = new ArrayList();

        public void LockControl(Control locked)
        {
            if (locked != null)
            {
                if (locked.Parent != null && locked.Parent.Controls != null)
                {
                    try
                    {
                        int y_off = (locked.Size.Height - 16) / 2;
                        int x_off = 0;
                        System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
                        PictureBox newLockBox = new PictureBox();

                        if (locked is TextBox)
                            x_off += 5;
                        else if (!(locked is CheckBox || locked is RadioButton))
                            x_off = (locked.Size.Width - 16) / 2;

                        newLockBox.Cursor = System.Windows.Forms.Cursors.Help;
                        //newLockBox.Image = ((System.Drawing.Image)(resources.GetObject("lockBox.Image")));
                        newLockBox.Size = new System.Drawing.Size(16, 16);
                        newLockBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                        newLockBox.Click += new System.EventHandler(this.lockBox_Click);

                        newLockBox.TabIndex = locked.TabIndex;
                        newLockBox.TabStop = locked.TabStop;
                        newLockBox.Location = new System.Drawing.Point(locked.Location.X + x_off, locked.Location.Y + y_off);
                        newLockBox.Name = locked.Name + "LockBox";
                        newLockBox.Tag = locked;
                        newLockBox.Visible = true;

                        locked.Parent.Controls.Add(newLockBox);
                        locked.Parent.Controls.SetChildIndex(newLockBox, 0);
                        m_LockBoxes.Add(newLockBox);
                    }
                    catch
                    {
                    }
                }

                locked.Enabled = false;
            }
        }

        public void UnlockControl(Control unlock)
        {
            if (unlock != null)
            {
                for (int i = 0; i < m_LockBoxes.Count; i++)
                {
                    PictureBox box = m_LockBoxes[i] as PictureBox;
                    if (box == null)
                        continue;

                    if (box.Tag == unlock)
                    {
                        unlock.Enabled = true;
                        if (box.Parent != null && box.Parent.Controls != null)
                            box.Parent.Controls.Remove(box);

                        m_LockBoxes.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void OnLogout()
        {
            OnMacroStop();

            features.Visible = false;

            for (int i = 0; i < m_LockBoxes.Count; i++)
            {
                PictureBox box = m_LockBoxes[i] as PictureBox;
                if (box == null)
                    continue;

                box.Parent.Controls.Remove(box);
                if (box.Tag is Control)
                    ((Control)box.Tag).Enabled = true;
            }
            m_LockBoxes.Clear();
        }

        public void UpdateControlLocks()
        {
            for (int i = 0; i < m_LockBoxes.Count; i++)
            {
                PictureBox box = m_LockBoxes[i] as PictureBox;
                if (box == null)
                    continue;

                box.Parent.Controls.Remove(box);
                if (box.Tag is Control)
                    ((Control)box.Tag).Enabled = true;
            }
            m_LockBoxes.Clear();

            if (!ClientCommunication.AllowBit(FeatureBit.SmartLT))
                LockControl(this.smartLT);

            if (!ClientCommunication.AllowBit(FeatureBit.RangeCheckLT))
                LockControl(this.rangeCheckLT);

            if (!ClientCommunication.AllowBit(FeatureBit.AutoOpenDoors))
                LockControl(this.autoOpenDoors);

            if (!ClientCommunication.AllowBit(FeatureBit.UnequipBeforeCast))
                LockControl(this.spellUnequip);

            if (!ClientCommunication.AllowBit(FeatureBit.AutoPotionEquip))
                LockControl(this.potionEquip);

            if (!ClientCommunication.AllowBit(FeatureBit.BlockHealPoisoned))
                LockControl(this.blockHealPoison);

            if (!ClientCommunication.AllowBit(FeatureBit.LoopingMacros))
                LockControl(this.loopMacro);

            if (!ClientCommunication.AllowBit(FeatureBit.OverheadHealth))
            {
                LockControl(this.showHealthOH);
                LockControl(this.healthFmt);
                LockControl(this.chkPartyOverhead);
            }
        }

        public Assistant.MapUO.MapWindow MapWindow;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

        private void btnMap_Click(object sender, System.EventArgs e)
        {
            if (World.Player != null)
            {
                if (MapWindow == null)
                    MapWindow = new Assistant.MapUO.MapWindow();
                //SetParent( MapWindow.Handle, ClientCommunication.FindUOWindow() );
                //MapWindow.Owner = (Form)Form.FromHandle( ClientCommunication.FindUOWindow() );
                MapWindow.Show();
                MapWindow.BringToFront();
            }
        }

        private void showHealthOH_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("ShowHealth", healthFmt.Enabled = showHealthOH.Checked);
        }

        private void healthFmt_TextChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("HealthFmt", healthFmt.Text);
        }

        private void chkPartyOverhead_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("ShowPartyStats", chkPartyOverhead.Checked);
        }

        private void btcLabel_Click(object sender, EventArgs e)
        {

        }

        private void cloneProfile_Click(object sender, EventArgs e)
        {
            if (profiles.SelectedIndex < 0)
                return;

            string profileToClone = (string)profiles.Items[profiles.SelectedIndex];

            if (InputBox.Show(this, Language.GetString(LocString.EnterProfileName),
                Language.GetString(LocString.EnterAName)))
            {
                string newProfileName = InputBox.GetString();
                if (string.IsNullOrEmpty(newProfileName) || newProfileName.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                    newProfileName.IndexOfAny(m_InvalidNameChars) != -1)
                {
                    MessageBox.Show(this, Language.GetString(LocString.InvalidChars),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                File.Copy(Path.Combine(Config.GetUserDirectory("Profiles"), $"{profileToClone}.xml"), Path.Combine(Config.GetUserDirectory("Profiles"), $"{newProfileName}.xml"));

                profiles.Items.Add(newProfileName);

            }
        }

        private void filterHotkeys_TextChanged(object sender, EventArgs e)
        {
            this.hotkeyTree.BeginUpdate();
            this.hotkeyTree.Nodes.Clear();

            if (this.filterHotkeys.Text != string.Empty)
            {
                foreach (TreeNode _parentNode in _hotkeyTreeViewCache.Nodes) // We won't filter on the top parent domain
                {
                    foreach (TreeNode _childNode in _parentNode.Nodes) // lets not filter on the sub-ones either
                    {

                        foreach (TreeNode _subChildNode in _childNode.Nodes) // we want all these sub child nodes to be filters
                        {
                            if (_subChildNode.Text.ToLower().Contains(filterHotkeys.Text.ToLower()))
                            {
                                this.hotkeyTree.Nodes.Add((TreeNode)_subChildNode.Clone());
                            }

                            if (_subChildNode.Nodes.Count > 0) // Just in case
                            {
                                foreach (TreeNode _subSubChildNode in _subChildNode.Nodes)
                                {
                                    if (_subSubChildNode.Text.ToLower().Contains(filterHotkeys.Text.ToLower()))
                                    {
                                        this.hotkeyTree.Nodes.Add((TreeNode)_subSubChildNode.Clone());
                                    }
                                }
                            }
                        }


                    }
                }
            }
            else
            {
                HotKey.RebuildList(hotkeyTree);
            }
            //enables redrawing tree after all objects have been added
            this.hotkeyTree.EndUpdate();
        }

        private void openRazorDataDir_Click(object sender, EventArgs e)
        {
            //C:\Users\Quick\AppData\Roaming\Razor

            try
            {
                Process.Start(Config.GetUserDirectory());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unable to open directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void backupDataDir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists($"{Config.GetUserDirectory()}\\Backup"))
                {
                    Directory.CreateDirectory($"{Config.GetUserDirectory()}\\Backup");
                }

                string backupTime = $"{DateTime.Now:yyMMddHHmmss}";

                Directory.CreateDirectory($"{Config.GetUserDirectory()}\\Backup\\{backupTime}");

                // Backup the macros
                Directory.CreateDirectory($"{Config.GetUserDirectory()}\\Backup\\{backupTime}\\Macros");

                foreach (string dirPath in Directory.GetDirectories($"{Config.GetUserDirectory()}\\Macros", "*",
                    SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace($"{Config.GetUserDirectory()}\\Macros",
                        $"{Config.GetUserDirectory()}\\Backup\\{backupTime}\\Macros"));
                }

                foreach (string newPath in Directory.GetFiles($"{Config.GetUserDirectory()}\\Macros", "*.*",
                    SearchOption.AllDirectories))
                {
                    File.Copy(newPath,
                        newPath.Replace($"{Config.GetUserDirectory()}\\Macros",
                            $"{Config.GetUserDirectory()}\\Backup\\{backupTime}\\Macros"), true);
                }

                // Backup the profiles
                Directory.CreateDirectory($"{Config.GetUserDirectory()}\\Backup\\{backupTime}\\Profiles");

                foreach (string newPath in Directory.GetFiles($"{Config.GetUserDirectory()}\\Profiles", "*.*",
                     SearchOption.AllDirectories))
                {
                    File.Copy(newPath,
                        newPath.Replace($"{Config.GetUserDirectory()}\\Profiles",
                            $"{Config.GetUserDirectory()}\\Backup\\{backupTime}\\Profiles"), true);
                }

                MessageBox.Show(this, $"Backup created: {Config.GetUserDirectory()}\\Backup\\{backupTime}\\", "Razor Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unable to create backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void titleBarParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (titleBarParams.SelectedItem.ToString().Contains("{") && titleBarParams.Focused)
            {
                titleStr.AppendText($" {titleBarParams.SelectedItem} ");
            }
        }

        private void addAbsoluteTarget_Click(object sender, EventArgs e)
        {
            if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                return;

            Targeting.OneTimeTarget(OnAbsoluteTargetListAddTarget);
            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void OnAbsoluteTargetListAddTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            TargetInfo t = new TargetInfo
            {
                Gfx = gfx,
                Serial = serial,
                Type = (byte)(ground ? 1 : 0),
                X = pt.X,
                Y = pt.Y,
                Z = pt.Z
            };

            if (InputBox.Show(this, Language.GetString(LocString.NewAbsoluteTargetVar), Language.GetString(LocString.EnterAName)))
            {
                string name = InputBox.GetString();

                foreach (AbsoluteTargets.AbsoluteTarget at in AbsoluteTargets.AbsoluteTargetList)
                {
                    if (at.TargetVariableName.ToLower().Equals(name.ToLower()))
                    {
                        MessageBox.Show(this, "Pick a unique Absolute Target name and try again", "New Absolute Target", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                AbsoluteTargets.AbsoluteTargetList.Add(new AbsoluteTargets.AbsoluteTarget(name, t));

                // Save and reload the macros and vars
                MacroManager.DisplayAbsoluteTargetsTo(absoluteTargets);
            }

            Engine.MainWindow.ShowMe();
        }

        private void expandAdvancedMacros_Click(object sender, EventArgs e)
        {
            if (expandAdvancedMacros.Text.Equals("\u2191"))
            {
                expandAdvancedMacros.Text = "\u2193";
                absoluteTargetGroup.Visible = false;
                tabs.Size = new Size(tabs.Size.Width, 290);

                Size = new Size(tabs.Size.Width + 10, tabs.Size.Height + 30);
            }
            else
            {
                expandAdvancedMacros.Text = "\u2191";
                absoluteTargetGroup.Visible = true;
                tabs.Size = new Size(tabs.Size.Width, 500);
                Size = new Size(tabs.Size.Width + 10, tabs.Size.Height + 30);
            }
        }

        private void insertAbsoluteTarget_Click(object sender, EventArgs e)
        {
            if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                return;

            Macro m = GetMacroSel();

            if (m == null)
                return;

            int a = actionList.SelectedIndex;

            if (absoluteTargets.SelectedIndex < 0)
            {
                return;
            }

            m.Actions.Insert(a + 1,
                new AbsoluteTargetVariableAction(AbsoluteTargets.AbsoluteTargetList[absoluteTargets.SelectedIndex].TargetVariableName));

            RedrawActionList(m);
        }

        private void retargetAbsoluteTarget_Click(object sender, EventArgs e)
        {
            if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                return;

            if (absoluteTargets.SelectedIndex < 0)
                return;

            Targeting.OneTimeTarget(OnAbsoluteTargetListReTarget);
            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void OnAbsoluteTargetListReTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            TargetInfo t = new TargetInfo
            {
                Gfx = gfx,
                Serial = serial,
                Type = (byte)(ground ? 1 : 0),
                X = pt.X,
                Y = pt.Y,
                Z = pt.Z
            };

            AbsoluteTargets.AbsoluteTargetList[absoluteTargets.SelectedIndex].TargetInfo = t;

            // Save and reload the macros and vars
            MacroManager.DisplayAbsoluteTargetsTo(absoluteTargets);

            Engine.MainWindow.ShowMe();
        }

        private void removeAbsoluteTarget_Click(object sender, EventArgs e)
        {
            if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                return;

            if (absoluteTargets.SelectedIndex < 0)
                return;

            AbsoluteTargets.AbsoluteTargetList.RemoveAt(absoluteTargets.SelectedIndex);

            // Save and reload the macros and vars
            MacroManager.Save();
            MacroManager.DisplayAbsoluteTargetsTo(absoluteTargets);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.uor-razor.com");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.uorenaissance.com/");
        }

        private void targetByTypeDifferent_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("DiffTargetByType", targetByTypeDifferent.Checked);
        }

        private void stepThroughMacro_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("StepThroughMacro", stepThroughMacro.Checked);
        }

        private void nextMacroAction_Click(object sender, EventArgs e)
        {
            /*if (actionList.SelectedIndex + 1 > actionList.Items.Count - 1)
            {
                actionList.SelectedIndex = 0;
            }*/

            MacroManager.PlayNext();
        }

        private void disableSmartCPU_Click(object sender, EventArgs e)
        {
            ClientCommunication.SetSmartCPU(false);
        }

        private void lightLevelBar_Scroll(object sender, EventArgs e)
        {
            if (ClientCommunication.AllowBit(FeatureBit.LightFilter) && World.Player != null)
            {
                World.Player.LocalLightLevel = 0;
                World.Player.GlobalLightLevel = Convert.ToByte(lightLevelBar.Value);

                ClientCommunication.SendToClient(new GlobalLightLevel(Convert.ToByte(lightLevelBar.Maximum - lightLevelBar.Value)));
                ClientCommunication.SendToClient(new PersonalLightLevel(World.Player));

                double percent = Math.Round((lightLevelBar.Value / (double)lightLevelBar.Maximum) * 100.0);

                lightLevel.Text = $"Light: {percent}%";

                Config.SetProperty("LightLevel", lightLevelBar.Value);
            }
        }

        private void showPlayerPosition_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowPlayerPosition", showPlayerPosition.Checked);
        }

        private void trackPlayerPosition_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("TrackPlayerPosition", trackPlayerPosition.Checked);
        }

        private void showPartyMemberPositions_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowPartyMemberPositions", showPartyMemberPositions.Checked);
        }

        private void tiltMap_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("TiltMap", showPartyMemberPositions.Checked);
        }

        private void boatControl_Click(object sender, EventArgs e)
        {
            BoatWindow boatWindow = new BoatWindow();
            boatWindow.Show();
            boatWindow.BringToFront();
        }

        private void showTargetMessagesOverChar_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowTargetSelfLastClearOverhead", showTargetMessagesOverChar.Checked);
        }

        private void showStunMessagesOverhead_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowOverheadMessages", showOverheadMessages.Checked);

            overHeadMessages.Enabled = showOverheadMessages.Checked;
        }

        private void openUOPS_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Soon(tm)");

            return;

            /*if (World.Player != null)
            {
                if (MapWindow == null)
                    MapWindow = new Assistant.MapUO.MapWindow();
                //SetParent( MapWindow.Handle, ClientCommunication.FindUOWindow() );
                //MapWindow.Owner = (Form)Form.FromHandle( ClientCommunication.FindUOWindow() );
                MapWindow.Show();
                MapWindow.BringToFront();
            }*/
        }

        private void logSkillChanges_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("LogSkillChanges", logSkillChanges.Checked);
        }

        private void overHeadMessages_Click(object sender, EventArgs e)
        {
            OverheadMessages form = new OverheadMessages();
            form.Show();
            form.BringToFront();
        }

        private void saveProfile_Click(object sender, EventArgs e)
        {
            if (profiles.SelectedIndex < 0)
                return;

            Config.Save();

            string profileToClone = (string)profiles.Items[profiles.SelectedIndex];
            MessageBox.Show(SplashScreen.Instance, $"Saved current settings to profile {Path.Combine(Config.GetUserDirectory("Profiles"), $"{profileToClone}.xml")}", "Save Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void stealthOverhead_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("StealthOverhead", stealthOverhead.Checked);
        }

        private void captureMibs_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("CaptureMibs", captureMibs.Checked);
        }

        private void trackIncomingGold_CheckedChanged(object sender, EventArgs e)
        {
            if (trackIncomingGold.Checked)
            {
                GoldPerHourTimer.Start();
            }
            else
            {
                GoldPerHourTimer.Stop();
            }
        }

        private void objectDelay_CheckedChanged(object sender, EventArgs e)
        {
            txtObjDelay.Enabled = objectDelay.Checked;

            Config.SetProperty("ObjectDelayEnabled", objectDelay.Checked);
        }

        private void showBuffDebuffOverhead_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowBuffDebuffOverhead", showBuffDebuffOverhead.Checked);
        }

        private void buffDebuffFormat_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(buffDebuffFormat.Text))
            {
                Config.SetProperty("BuffDebuffFormat", "[{action}{name}]");
            }
            else
            {
                Config.SetProperty("BuffDebuffFormat", buffDebuffFormat.Text);
            }
        }

        private void blockOpenCorpsesTwice_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("BlockOpenCorpsesTwice", blockOpenCorpsesTwice.Checked);

            if (blockOpenCorpsesTwice.Checked)
            {
                if (World.Player == null)
                    return;

                World.Player.OpenedCorpses.Clear();
            }
        }

        private void screenShotNotification_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ScreenshotUploadNotifications", screenShotNotification.Checked);


            if (screenShotNotification.Checked)
            {
                m_NotifyIcon.Visible = true;
            }
            else
            {
                bool st = Config.GetBool("Systray");
                taskbar.Checked = ShowInTaskbar = !st;
                systray.Checked = m_NotifyIcon.Visible = st;
            }
        }

        private void screenShotClipboard_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ScreenshotUploadClipboard", screenShotClipboard.Checked);
        }

        private void screenShotOpenBrowser_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ScreenshotUploadOpenBrowser", screenShotOpenBrowser.Checked);
        }

        private void showAttackTarget_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowAttackTargetOverhead", showAttackTarget.Checked);
        }

        private void rangeCheckTargetByType_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("RangeCheckTargetByType", rangeCheckTargetByType.Checked);
        }

        private void rangeCheckDoubleClick_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("RangeCheckDoubleClick", rangeCheckDoubleClick.Checked);
        }

        private void agentSubList_MouseDown(object sender, MouseEventArgs e)
        {
            if (agentList.SelectedIndex < 0 || agentSubList.Items.Count == 0)
                return;

            //if (e.Button == MouseButtons.Right && e.Clicks == 1)
            //{
            //    ContextMenu menu = new ContextMenu();
            //    //menu.MenuItems.Add(Language.GetString(LocString.Reload), new EventHandler(onMacroReload));
            //    menu.MenuItems.Add("Import (Copy from clipboard)", new EventHandler(OnAgentImport));
            //    menu.MenuItems.Add("-");
            //    menu.MenuItems.Add("Export (Copy to clipboard)", new EventHandler(OnAgentExport));

            //    menu.Show(agentSubList, new Point(e.X, e.Y));
            //}
        }

        private void OnAgentExport(object sender, System.EventArgs e)
        {
            if (agentList.SelectedIndex < 0 || agentSubList.Items.Count == 0)
                return;

            //Agent.Select(agentList.SelectedIndex, agentList, agentSubList, agentGroup, agentB1, agentB2, agentB3, agentB4, agentB5, agentB6);

            StringBuilder sb = new StringBuilder();

            foreach (var item in agentSubList.Items)
            {
                sb.AppendLine(item.ToString());
            }

            Console.WriteLine(sb.ToString());
        }

        private void OnAgentImport(object sender, System.EventArgs e)
        {

        }

        private void showContainerLabels_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowContainerLabels", showContainerLabels.Checked);

            containerLabels.Enabled = showContainerLabels.Checked;
        }

        private void containerLabels_Click(object sender, EventArgs e)
        {
            //if (World.Player == null)
            //    return;

            ContainerLabels form = new ContainerLabels();
            form.Show();
            form.BringToFront();
        }

        private void realSeason_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("RealSeason", realSeason.Checked);

            if (realSeason.Checked)
            {
                seasonList.Enabled = false;

                if (World.Player != null)
                    seasonList.SelectedIndex = World.Player.WhichSeason();
            }
            else
            {
                seasonList.Enabled = true;
                seasonList.SelectedIndex = Config.GetInt("Season");
            }
        }

        private void seasonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Season Flag:
            //0: Spring
            //1: Summer
            //2: Fall
            //3: Winter
            //4: Desolation
            //Default server

            Config.SetProperty("Season", seasonList.SelectedIndex);

            if (seasonList.SelectedIndex < 5)
            {
                ClientCommunication.ForceSendToClient(new SeasonChange(seasonList.SelectedIndex, true));
            }
        }

        private void actionList_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
