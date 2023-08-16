﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Assistant
{
    public partial class MainForm : System.Windows.Forms.Form
    {

        #region Class Variables

        private System.Windows.Forms.NotifyIcon m_NotifyIcon;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.ColumnHeader skillHDRName;
        private System.Windows.Forms.ColumnHeader skillHDRvalue;
        private System.Windows.Forms.ColumnHeader skillHDRbase;
        private System.Windows.Forms.ColumnHeader skillHDRdelta;
        private System.Windows.Forms.Button resetDelta;
        private System.Windows.Forms.Button setlocks;
        private System.Windows.Forms.ComboBox locks;
        private System.Windows.Forms.ListView skillList;
        private System.Windows.Forms.ColumnHeader skillHDRcap;
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
        private System.Windows.Forms.TextBox hkKey;
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
        private System.Windows.Forms.CheckBox dispDelta;
        private System.Windows.Forms.ComboBox agentList;
        private System.Windows.Forms.TabPage macrosTab;
        private System.Windows.Forms.TreeView hotkeyTree;
        private System.Windows.Forms.TabPage screenshotTab;
        private System.Windows.Forms.TabPage aboutTab;
        private System.Windows.Forms.Button dressNow;
        private System.Windows.Forms.Button undressList;
        private System.Windows.Forms.PictureBox screenPrev;
        private System.Windows.Forms.ListBox screensList;
        private System.Windows.Forms.Button setScnPath;
        private System.Windows.Forms.RadioButton radioFull;
        private System.Windows.Forms.RadioButton radioUO;
        private System.Windows.Forms.CheckBox captureOthersDeath;
        private System.Windows.Forms.TextBox screenPath;
        private System.Windows.Forms.Button capNow;
        private System.Windows.Forms.CheckBox dispTime;
        private System.Windows.Forms.Button agentB5;
        private System.Windows.Forms.Button agentB6;
        private System.Windows.Forms.CheckBox undressConflicts;
        private System.Windows.Forms.ColumnHeader skillHDRlock;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button undressBag;
        private System.Windows.Forms.Button dressDelSel;
        private System.Windows.Forms.Label hkStatus;
        private System.Windows.Forms.Button clearDress;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox imgFmt;
        private ToolTip m_Tip;

        #endregion

        private int m_LastKV = 0;
        private bool m_ProfileConfirmLoad;
        private LinkLabel linkMain;
        private Label label21;
        private Label aboutVer;
        private TextBox filterHotkeys;
        private Label label22;
        private bool m_CanClose = true;
        private TabPage advancedTab;
        private Label aboutSubInfo;
        private Label lblCredits1;
        private Label label20;
        private CheckBox logSkillChanges;
        private TreeView _hotkeyTreeViewCache = new TreeView();
        private LinkLabel linkHelp;
        private TabControl tabControl2;
        private TabPage subMacrosTab;
        private TabPage subMacrosOptionsTab;
        private CheckBox rangeCheckDoubleClick;
        private CheckBox rangeCheckTargetByType;
        private Button nextMacroAction;
        private CheckBox stepThroughMacro;
        private CheckBox targetByTypeDifferent;
        private System.Windows.Forms.GroupBox macroVariableGroup;
        private ComboBox macroVariableTypeList;
        private Button retargetMacroVariable;
        private Button insertMacroVariable;
        private Button removeMacroVariable;
        private Button addMacroVariable;
        private ListBox macroVariables;
        private TextBox filterMacros;
        private Label filterLabel;
        private TreeView macroTree;
        private Button delMacro;
        private Button newMacro;
        private GroupBox macroActGroup;
        private Button playMacro;
        private Label waitDisp;
        private CheckBox loopMacro;
        private Button recMacro;
        private ListBox actionList;
        private TabControl optionsTabCtrl;
        private System.Windows.Forms.TabPage subOptionsSpeechTab;
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
        private TabPage subOptionsTargetTab;
        private CheckBox incomingMob;
        private CheckBox incomingCorpse;
        private CheckBox showAttackTargetNewOnly;
        private CheckBox showTextTargetIndicator;
        private CheckBox showAttackTarget;
        private CheckBox showTargetMessagesOverChar;
        private TextBox txtObjDelay;
        private CheckBox objectDelay;
        private TextBox ltRange;
        private CheckBox QueueActions;
        private CheckBox rangeCheckLT;
        private CheckBox actionStatusMsg;
        private Label label8;
        private Label label6;
        private CheckBox queueTargets;
        private CheckBox showtargtext;
        private Button containerLabels;
        private CheckBox showContainerLabels;
        private System.Windows.Forms.CheckBox showBuffDebuffOverhead;
        private TextBox healthFmt;
        private Label label10;
        private CheckBox showHealthOH;
        private CheckBox chkPartyOverhead;
        private TabPage subOptionsMiscTab;
        private Button setMinLightLevel;
        private Button setMaxLightLevel;
        private ComboBox seasonList;
        private Label lblSeason;
        private Label lightLevel;
        private TrackBar lightLevelBar;
        private CheckBox minMaxLightLevel;
        private CheckBox blockPartyInvites;
        private CheckBox blockTradeRequests;
        private CheckBox blockOpenCorpsesTwice;
        private CheckBox preAOSstatbar;
        private TextBox corpseRange;
        private CheckBox autoStackRes;
        private Label label4;
        private CheckBox openCorpses;
        private CheckBox blockDis;
        private CheckBox showStaticWalls;
        private CheckBox showStaticWallLabels;
        private CheckBox stealthOverhead;
        private TextBox forceSizeX;
        private TextBox forceSizeY;
        private CheckBox blockHealPoison;
        private CheckBox potionEquip;
        private CheckBox spellUnequip;
        private CheckBox autoOpenDoors;
        private CheckBox chkStealth;
        private Label label18;
        private CheckBox gameSize;
        private CheckBox damageTakenOverhead;
        private CheckBox showDamageTaken;
        private CheckBox damageDealtOverhead;
        private CheckBox showDamageDealt;
        private CheckBox rememberPwds;
        private System.Windows.Forms.TabControl displayCountersTabCtrl;
        private TabPage subDisplayTab;
        private GroupBox groupBox11;
        private Button razorTitleBarKey;
        private System.Windows.Forms.CheckBox showInRazorTitleBar;
        private TextBox razorTitleBar;
        private System.Windows.Forms.CheckBox trackDps;
        private System.Windows.Forms.CheckBox trackIncomingGold;
        private CheckBox showNotoHue;
        private CheckBox highlightSpellReags;
        private GroupBox groupBox3;
        private ComboBox titleBarParams;
        private TextBox titleStr;
        private CheckBox showInBar;
        private TabPage subCountersTab;
        private TextBox warnNum;
        private CheckBox warnCount;
        private CheckBox excludePouches;
        private CheckBox titlebarImages;
        private CheckBox checkNewConts;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView counters;
        private ColumnHeader cntName;
        private ColumnHeader cntCount;
        private System.Windows.Forms.Button delCounter;
        private System.Windows.Forms.Button addCounter;
        private System.Windows.Forms.Button recount;
        private TabControl subGeneralTab;
        private TabPage subGenTab;
        private RadioButton systray;
        private RadioButton taskbar;
        private ComboBox langSel;
        private Label label7;
        private Label label11;
        private CheckBox showWelcome;
        private TrackBar opacity;
        private CheckBox alwaysTop;
        private Label opacityLabel;
        private GroupBox groupBox4;
        private Button saveProfile;
        private Button cloneProfile;
        private Button delProfile;
        private Button newProfile;
        private ComboBox profiles;
        private TextBox targetIndicatorFormat;
        private Label lblTargetFormat;
        private Label lblStealthFormat;
        private TextBox stealthStepsFormat;
        private CheckBox dispDeltaOverhead;
        private Label lblCredits2;
        private LinkLabel linkGitHub;
        private Label lblCredits3;
        private GroupBox groupBox15;
        private Button btnMap;
        private Button boatControl;
        private CheckBox captureMibs;
        private CheckBox macroActionDelay;
        private CheckBox autoOpenDoorWhenHidden;
        private CheckBox disableMacroPlayFinish;
        private TabPage subBandageTimerTab;
        private CheckBox onlyShowBandageTimerSeconds;
        private TextBox bandageTimerFormat;
        private CheckBox showBandageTimer;
        private ComboBox bandageTimerLocation;
        private TextBox bandageTimerSeconds;
        private Label lblBandageCountFormat;
        private Button setBandageHue;
        private Button setTargetIndicatorHue;
        private GroupBox groupSmartTarget;
        private CheckBox nonFriendlyHarmfulOnly;
        private CheckBox friendBeneficialOnly;
        private CheckBox onlyNextPrevBeneficial;
        private CheckBox smartLT;
        private TextBox bandageEndMessage;
        private TextBox bandageStartMessage;
        private System.Windows.Forms.CheckBox showBandageEnd;
        private System.Windows.Forms.CheckBox showBandageStart;
        private System.Windows.Forms.Button buffDebuffOptions;
        private TextBox captureOthersDeathDelay;
        private Label lblCaptureOthersDeathMs;
        private TextBox captureOwnDeathDelay;
        private Label lblOwnDeathMs;
        private CheckBox captureOwnDeath;
        private Button setMacroHotKey;
        private TabPage subFilterSoundMusic;
        private CheckedListBox soundFilterList;
        private CheckBox soundFilterEnabled;
        private Button playSound;
        private CheckBox showFilteredSound;
        private CheckBox showPlayingSoundInfo;
        private CheckBox showPlayingMusic;
        private CheckBox playInClient;
        private Button playMusic;
        private ComboBox playableMusicList;
        private TreeView _macroTreeViewCache = new TreeView();
        private TreeView _scriptTreeViewCache = new TreeView();
        private TreeView _itemTreeCache = new TreeView();


        public Label WaitDisplay
        {
            get { return waitDisp; }
        }

        public MainForm()
        {
            m_ProfileConfirmLoad = true;
            m_Tip = new ToolTip();
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            m_NotifyIcon.ContextMenuStrip = new ContextMenuStrip();
            m_NotifyIcon.ContextMenuStrip.Items.Add("Show Razor", null, new EventHandler(DoShowMe));
            m_NotifyIcon.ContextMenuStrip.Items.Add("Hide Razor", null, new EventHandler(HideMe));
            m_NotifyIcon.ContextMenuStrip.Items.Add("-");
            m_NotifyIcon.ContextMenuStrip.Items.Add("Toggle Razor Visibility", null, new EventHandler(ToggleVisible));
            m_NotifyIcon.ContextMenuStrip.Items.Add("-");
            m_NotifyIcon.ContextMenuStrip.Items.Add("Close Razor && UO", null, new EventHandler(OnClose));

            m_NotifyIcon.ContextMenuStrip.Items[0].Select();
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
            this.subGeneralTab = new System.Windows.Forms.TabControl();
            this.subGenTab = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.boatControl = new System.Windows.Forms.Button();
            this.btnMap = new System.Windows.Forms.Button();
            this.systray = new System.Windows.Forms.RadioButton();
            this.taskbar = new System.Windows.Forms.RadioButton();
            this.langSel = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.showWelcome = new System.Windows.Forms.CheckBox();
            this.opacity = new System.Windows.Forms.TrackBar();
            this.alwaysTop = new System.Windows.Forms.CheckBox();
            this.opacityLabel = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.saveProfile = new System.Windows.Forms.Button();
            this.cloneProfile = new System.Windows.Forms.Button();
            this.delProfile = new System.Windows.Forms.Button();
            this.newProfile = new System.Windows.Forms.Button();
            this.profiles = new System.Windows.Forms.ComboBox();
            this.moreOptTab = new System.Windows.Forms.TabPage();
            this.optionsTabCtrl = new System.Windows.Forms.TabControl();
            this.subOptionsSpeechTab = new System.Windows.Forms.TabPage();
            this.playEmoteSound = new System.Windows.Forms.CheckBox();
            this.overrideSpellFormat = new System.Windows.Forms.CheckBox();
            this.damageTakenOverhead = new System.Windows.Forms.CheckBox();
            this.showDamageTaken = new System.Windows.Forms.CheckBox();
            this.damageDealtOverhead = new System.Windows.Forms.CheckBox();
            this.showDamageDealt = new System.Windows.Forms.CheckBox();
            this.healthFmt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.showHealthOH = new System.Windows.Forms.CheckBox();
            this.chkPartyOverhead = new System.Windows.Forms.CheckBox();
            this.containerLabels = new System.Windows.Forms.Button();
            this.showContainerLabels = new System.Windows.Forms.CheckBox();
            this.incomingMob = new System.Windows.Forms.CheckBox();
            this.incomingCorpse = new System.Windows.Forms.CheckBox();
            this.setLTHilight = new System.Windows.Forms.Button();
            this.lthilight = new System.Windows.Forms.CheckBox();
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
            this.lblWarnHue = new System.Windows.Forms.Label();
            this.lblMsgHue = new System.Windows.Forms.Label();
            this.lblExHue = new System.Windows.Forms.Label();
            this.txtSpellFormat = new System.Windows.Forms.TextBox();
            this.chkForceSpellHue = new System.Windows.Forms.CheckBox();
            this.chkForceSpeechHue = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.subOptionsTargetTab = new System.Windows.Forms.TabPage();
            this.groupSmartTarget = new System.Windows.Forms.GroupBox();
            this.nextPrevAbcOrder = new System.Windows.Forms.CheckBox();
            this.nonFriendlyHarmfulOnly = new System.Windows.Forms.CheckBox();
            this.friendBeneficialOnly = new System.Windows.Forms.CheckBox();
            this.onlyNextPrevBeneficial = new System.Windows.Forms.CheckBox();
            this.smartLT = new System.Windows.Forms.CheckBox();
            this.setTargetIndicatorHue = new System.Windows.Forms.Button();
            this.targetIndicatorFormat = new System.Windows.Forms.TextBox();
            this.showtargtext = new System.Windows.Forms.CheckBox();
            this.showAttackTargetNewOnly = new System.Windows.Forms.CheckBox();
            this.showTextTargetIndicator = new System.Windows.Forms.CheckBox();
            this.showAttackTarget = new System.Windows.Forms.CheckBox();
            this.showTargetMessagesOverChar = new System.Windows.Forms.CheckBox();
            this.txtObjDelay = new System.Windows.Forms.TextBox();
            this.objectDelay = new System.Windows.Forms.CheckBox();
            this.ltRange = new System.Windows.Forms.TextBox();
            this.QueueActions = new System.Windows.Forms.CheckBox();
            this.rangeCheckLT = new System.Windows.Forms.CheckBox();
            this.actionStatusMsg = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.queueTargets = new System.Windows.Forms.CheckBox();
            this.lblTargetFormat = new System.Windows.Forms.Label();
            this.subOptionsMiscTab = new System.Windows.Forms.TabPage();
            this.buyAgentIgnoreGold = new System.Windows.Forms.CheckBox();
            this.reequipHandsPotion = new System.Windows.Forms.CheckBox();
            this.autoOpenDoorWhenHidden = new System.Windows.Forms.CheckBox();
            this.lblStealthFormat = new System.Windows.Forms.Label();
            this.stealthStepsFormat = new System.Windows.Forms.TextBox();
            this.rememberPwds = new System.Windows.Forms.CheckBox();
            this.showStaticWalls = new System.Windows.Forms.CheckBox();
            this.showStaticWallLabels = new System.Windows.Forms.CheckBox();
            this.stealthOverhead = new System.Windows.Forms.CheckBox();
            this.forceSizeX = new System.Windows.Forms.TextBox();
            this.forceSizeY = new System.Windows.Forms.TextBox();
            this.blockHealPoison = new System.Windows.Forms.CheckBox();
            this.potionEquip = new System.Windows.Forms.CheckBox();
            this.spellUnequip = new System.Windows.Forms.CheckBox();
            this.autoOpenDoors = new System.Windows.Forms.CheckBox();
            this.chkStealth = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.gameSize = new System.Windows.Forms.CheckBox();
            this.setMinLightLevel = new System.Windows.Forms.Button();
            this.setMaxLightLevel = new System.Windows.Forms.Button();
            this.seasonList = new System.Windows.Forms.ComboBox();
            this.lblSeason = new System.Windows.Forms.Label();
            this.lightLevel = new System.Windows.Forms.Label();
            this.lightLevelBar = new System.Windows.Forms.TrackBar();
            this.minMaxLightLevel = new System.Windows.Forms.CheckBox();
            this.blockPartyInvites = new System.Windows.Forms.CheckBox();
            this.blockTradeRequests = new System.Windows.Forms.CheckBox();
            this.blockOpenCorpsesTwice = new System.Windows.Forms.CheckBox();
            this.preAOSstatbar = new System.Windows.Forms.CheckBox();
            this.corpseRange = new System.Windows.Forms.TextBox();
            this.autoStackRes = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.openCorpses = new System.Windows.Forms.CheckBox();
            this.blockDis = new System.Windows.Forms.CheckBox();
            this.displayTab = new System.Windows.Forms.TabPage();
            this.displayCountersTabCtrl = new System.Windows.Forms.TabControl();
            this.subDisplayTab = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.razorTitleBarKey = new System.Windows.Forms.Button();
            this.showInRazorTitleBar = new System.Windows.Forms.CheckBox();
            this.razorTitleBar = new System.Windows.Forms.TextBox();
            this.trackDps = new System.Windows.Forms.CheckBox();
            this.trackIncomingGold = new System.Windows.Forms.CheckBox();
            this.showNotoHue = new System.Windows.Forms.CheckBox();
            this.highlightSpellReags = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.titleBarParams = new System.Windows.Forms.ComboBox();
            this.titleStr = new System.Windows.Forms.TextBox();
            this.showInBar = new System.Windows.Forms.CheckBox();
            this.subCountersTab = new System.Windows.Forms.TabPage();
            this.warnNum = new System.Windows.Forms.TextBox();
            this.warnCount = new System.Windows.Forms.CheckBox();
            this.excludePouches = new System.Windows.Forms.CheckBox();
            this.titlebarImages = new System.Windows.Forms.CheckBox();
            this.checkNewConts = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.counters = new System.Windows.Forms.ListView();
            this.cntName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cntCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.delCounter = new System.Windows.Forms.Button();
            this.addCounter = new System.Windows.Forms.Button();
            this.recount = new System.Windows.Forms.Button();
            this.subBandageTimerTab = new System.Windows.Forms.TabPage();
            this.bandageEndMessage = new System.Windows.Forms.TextBox();
            this.bandageStartMessage = new System.Windows.Forms.TextBox();
            this.showBandageEnd = new System.Windows.Forms.CheckBox();
            this.showBandageStart = new System.Windows.Forms.CheckBox();
            this.setBandageHue = new System.Windows.Forms.Button();
            this.bandageTimerLocation = new System.Windows.Forms.ComboBox();
            this.bandageTimerSeconds = new System.Windows.Forms.TextBox();
            this.onlyShowBandageTimerSeconds = new System.Windows.Forms.CheckBox();
            this.bandageTimerFormat = new System.Windows.Forms.TextBox();
            this.showBandageTimer = new System.Windows.Forms.CheckBox();
            this.lblBandageCountFormat = new System.Windows.Forms.Label();
            this.subOverheadTab = new System.Windows.Forms.TabPage();
            this.setSound = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.unicodeStyle = new System.Windows.Forms.RadioButton();
            this.asciiStyle = new System.Windows.Forms.RadioButton();
            this.editOverheadMessage = new System.Windows.Forms.Button();
            this.setColorHue = new System.Windows.Forms.Button();
            this.removeOverheadMessage = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.overheadFormat = new System.Windows.Forms.TextBox();
            this.setOverheadMessage = new System.Windows.Forms.Button();
            this.cliLocOverheadView = new System.Windows.Forms.ListView();
            this.cliLocOriginal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cliLocOverheadMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cliLocSound = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cliLocSearch = new System.Windows.Forms.Button();
            this.cliLocTextSearch = new System.Windows.Forms.TextBox();
            this.lblOhSearch = new System.Windows.Forms.Label();
            this.cliLocSearchView = new System.Windows.Forms.ListView();
            this.cliLocSearchNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cliLocSearchText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.showOverheadMessages = new System.Windows.Forms.CheckBox();
            this.subWaypoints = new System.Windows.Forms.TabPage();
            this.waypointOnDeath = new System.Windows.Forms.CheckBox();
            this.lblWaypointTiles = new System.Windows.Forms.Label();
            this.hideWaypointDist = new System.Windows.Forms.TextBox();
            this.hideWaypointWithin = new System.Windows.Forms.CheckBox();
            this.txtWaypointDistanceSec = new System.Windows.Forms.TextBox();
            this.lblWaypointSeconds = new System.Windows.Forms.Label();
            this.showWaypointDistance = new System.Windows.Forms.CheckBox();
            this.showWaypointOverhead = new System.Windows.Forms.CheckBox();
            this.btnRemoveSelectedWaypoint = new System.Windows.Forms.Button();
            this.btnHideWaypoint = new System.Windows.Forms.Button();
            this.btnUseCurrentLoc = new System.Windows.Forms.Button();
            this.txtWaypointName = new System.Windows.Forms.TextBox();
            this.lblWaypointY = new System.Windows.Forms.Label();
            this.lblWaypointX = new System.Windows.Forms.Label();
            this.txtWaypointX = new System.Windows.Forms.TextBox();
            this.txtWaypointY = new System.Windows.Forms.TextBox();
            this.lblWaypoint = new System.Windows.Forms.Label();
            this.btnAddWaypoint = new System.Windows.Forms.Button();
            this.waypointList = new System.Windows.Forms.ListBox();
            this.subBuffsDebuffs = new System.Windows.Forms.TabPage();
            this.cooldownGumpBox = new System.Windows.Forms.GroupBox();
            this.cooldownHeight = new System.Windows.Forms.TextBox();
            this.lblCooldownHeight = new System.Windows.Forms.Label();
            this.cooldownWidth = new System.Windows.Forms.TextBox();
            this.lblCooldownWidth = new System.Windows.Forms.Label();
            this.buffBarGroupBox = new System.Windows.Forms.GroupBox();
            this.showBuffDebuffTimeType = new System.Windows.Forms.ComboBox();
            this.lblShowBuffTime = new System.Windows.Forms.Label();
            this.useBlackBuffDebuffBg = new System.Windows.Forms.CheckBox();
            this.buffBarHeight = new System.Windows.Forms.TextBox();
            this.lblBuffBarHeight = new System.Windows.Forms.Label();
            this.buffBarSort = new System.Windows.Forms.ComboBox();
            this.lblBuffSortBy = new System.Windows.Forms.Label();
            this.buffBarWidth = new System.Windows.Forms.TextBox();
            this.lblBuffBarWidth = new System.Windows.Forms.Label();
            this.showBuffIcons = new System.Windows.Forms.CheckBox();
            this.showBuffDebuffGump = new System.Windows.Forms.CheckBox();
            this.buffDebuffOptions = new System.Windows.Forms.Button();
            this.showBuffDebuffOverhead = new System.Windows.Forms.CheckBox();
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
            this.captureMibs = new System.Windows.Forms.CheckBox();
            this.dispDeltaOverhead = new System.Windows.Forms.CheckBox();
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
            this.agentSetHotKey = new System.Windows.Forms.Button();
            this.agentB6 = new System.Windows.Forms.Button();
            this.agentB5 = new System.Windows.Forms.Button();
            this.agentList = new System.Windows.Forms.ComboBox();
            this.agentGroup = new System.Windows.Forms.GroupBox();
            this.agentSubList = new System.Windows.Forms.ListBox();
            this.agentB4 = new System.Windows.Forms.Button();
            this.agentB1 = new System.Windows.Forms.Button();
            this.agentB2 = new System.Windows.Forms.Button();
            this.agentB3 = new System.Windows.Forms.Button();
            this.filtersTab = new System.Windows.Forms.TabPage();
            this.filterTabs = new System.Windows.Forms.TabControl();
            this.subFilterTab = new System.Windows.Forms.TabPage();
            this.wyrmAnimationList = new System.Windows.Forms.ComboBox();
            this.filterWhiteWyrm = new System.Windows.Forms.CheckBox();
            this.daemonAnimationList = new System.Windows.Forms.ComboBox();
            this.filterDaemonGraphics = new System.Windows.Forms.CheckBox();
            this.drakeAnimationList = new System.Windows.Forms.ComboBox();
            this.filterDrakeGraphics = new System.Windows.Forms.CheckBox();
            this.dragonAnimationList = new System.Windows.Forms.ComboBox();
            this.filterDragonGraphics = new System.Windows.Forms.CheckBox();
            this.filters = new System.Windows.Forms.CheckedListBox();
            this.subFilterText = new System.Windows.Forms.TabPage();
            this.gbFilterText = new System.Windows.Forms.GroupBox();
            this.removeFilterText = new System.Windows.Forms.Button();
            this.addFilterText = new System.Windows.Forms.Button();
            this.textFilterList = new System.Windows.Forms.ListBox();
            this.enableTextFilter = new System.Windows.Forms.CheckBox();
            this.gbFilterMessages = new System.Windows.Forms.GroupBox();
            this.filterOverheadMessages = new System.Windows.Forms.CheckBox();
            this.lblFilterDelaySeconds = new System.Windows.Forms.Label();
            this.lblFilterDelay = new System.Windows.Forms.Label();
            this.filterDelaySeconds = new System.Windows.Forms.TextBox();
            this.filterRazorMessages = new System.Windows.Forms.CheckBox();
            this.filterSystemMessages = new System.Windows.Forms.CheckBox();
            this.filterSnoop = new System.Windows.Forms.CheckBox();
            this.subFilterSoundMusic = new System.Windows.Forms.TabPage();
            this.playableMusicList = new System.Windows.Forms.ComboBox();
            this.playMusic = new System.Windows.Forms.Button();
            this.showPlayingMusic = new System.Windows.Forms.CheckBox();
            this.showPlayingSoundInfo = new System.Windows.Forms.CheckBox();
            this.showFilteredSound = new System.Windows.Forms.CheckBox();
            this.playInClient = new System.Windows.Forms.CheckBox();
            this.playSound = new System.Windows.Forms.Button();
            this.soundFilterEnabled = new System.Windows.Forms.CheckBox();
            this.soundFilterList = new System.Windows.Forms.CheckedListBox();
            this.subFilterTargets = new System.Windows.Forms.TabPage();
            this.lblTargetFilter = new System.Windows.Forms.Label();
            this.targetFilterClear = new System.Windows.Forms.Button();
            this.targetFilterRemove = new System.Windows.Forms.Button();
            this.targetFilterAdd = new System.Windows.Forms.Button();
            this.targetFilter = new System.Windows.Forms.ListBox();
            this.targetFilterEnabled = new System.Windows.Forms.CheckBox();
            this.hotkeysTab = new System.Windows.Forms.TabPage();
            this.filterHotkeys = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.hkStatus = new System.Windows.Forms.Label();
            this.hotkeyTree = new System.Windows.Forms.TreeView();
            this.dohotkey = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.hkCommand = new System.Windows.Forms.TextBox();
            this.hkCmdLabel = new System.Windows.Forms.Label();
            this.chkAlt = new System.Windows.Forms.CheckBox();
            this.chkPass = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.unsetHK = new System.Windows.Forms.Button();
            this.setHK = new System.Windows.Forms.Button();
            this.hkKey = new System.Windows.Forms.TextBox();
            this.chkCtrl = new System.Windows.Forms.CheckBox();
            this.chkShift = new System.Windows.Forms.CheckBox();
            this.macrosTab = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.subMacrosTab = new System.Windows.Forms.TabPage();
            this.macroActGroup = new System.Windows.Forms.GroupBox();
            this.setMacroHotKey = new System.Windows.Forms.Button();
            this.playMacro = new System.Windows.Forms.Button();
            this.waitDisp = new System.Windows.Forms.Label();
            this.loopMacro = new System.Windows.Forms.CheckBox();
            this.recMacro = new System.Windows.Forms.Button();
            this.actionList = new System.Windows.Forms.ListBox();
            this.filterMacros = new System.Windows.Forms.TextBox();
            this.filterLabel = new System.Windows.Forms.Label();
            this.macroTree = new System.Windows.Forms.TreeView();
            this.delMacro = new System.Windows.Forms.Button();
            this.newMacro = new System.Windows.Forms.Button();
            this.subMacrosOptionsTab = new System.Windows.Forms.TabPage();
            this.disableMacroPlayFinish = new System.Windows.Forms.CheckBox();
            this.macroActionDelay = new System.Windows.Forms.CheckBox();
            this.rangeCheckDoubleClick = new System.Windows.Forms.CheckBox();
            this.rangeCheckTargetByType = new System.Windows.Forms.CheckBox();
            this.nextMacroAction = new System.Windows.Forms.Button();
            this.stepThroughMacro = new System.Windows.Forms.CheckBox();
            this.targetByTypeDifferent = new System.Windows.Forms.CheckBox();
            this.macroVariableGroup = new System.Windows.Forms.GroupBox();
            this.macroVariableTypeList = new System.Windows.Forms.ComboBox();
            this.retargetMacroVariable = new System.Windows.Forms.Button();
            this.insertMacroVariable = new System.Windows.Forms.Button();
            this.removeMacroVariable = new System.Windows.Forms.Button();
            this.addMacroVariable = new System.Windows.Forms.Button();
            this.macroVariables = new System.Windows.Forms.ListBox();
            this.scriptsTab = new System.Windows.Forms.TabPage();
            this.subTabScripts = new System.Windows.Forms.TabControl();
            this.subScripts = new System.Windows.Forms.TabPage();
            this.scriptHotkey = new System.Windows.Forms.Label();
            this.scriptSplitContainer = new System.Windows.Forms.SplitContainer();
            this.scriptTree = new System.Windows.Forms.TreeView();
            this.scriptFilter = new System.Windows.Forms.TextBox();
            this.scriptDocMap = new FastColoredTextBoxNS.DocumentMap();
            this.scriptEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.scriptGuide = new System.Windows.Forms.Button();
            this.saveScript = new System.Windows.Forms.Button();
            this.newScript = new System.Windows.Forms.Button();
            this.setScriptHotkey = new System.Windows.Forms.Button();
            this.recordScript = new System.Windows.Forms.Button();
            this.playScript = new System.Windows.Forms.Button();
            this.subScriptOptions = new System.Windows.Forms.TabPage();
            this.disableScriptStopwatch = new System.Windows.Forms.CheckBox();
            this.enableHighlight = new System.Windows.Forms.CheckBox();
            this.defaultScriptDelay = new System.Windows.Forms.CheckBox();
            this.disableScriptTooltips = new System.Windows.Forms.CheckBox();
            this.scriptDisablePlayFinish = new System.Windows.Forms.CheckBox();
            this.autoSaveScriptPlay = new System.Windows.Forms.CheckBox();
            this.autoSaveScript = new System.Windows.Forms.CheckBox();
            this.scriptVariablesBox = new System.Windows.Forms.GroupBox();
            this.changeScriptVariable = new System.Windows.Forms.Button();
            this.removeScriptVariable = new System.Windows.Forms.Button();
            this.addScriptVariable = new System.Windows.Forms.Button();
            this.scriptVariables = new System.Windows.Forms.ListBox();
            this.friendsTab = new System.Windows.Forms.TabPage();
            this.showPartyFriendOverhead = new System.Windows.Forms.CheckBox();
            this.highlightFriend = new System.Windows.Forms.CheckBox();
            this.autoAcceptParty = new System.Windows.Forms.CheckBox();
            this.nextPrevIgnoresFriends = new System.Windows.Forms.CheckBox();
            this.autoFriend = new System.Windows.Forms.CheckBox();
            this.friendsGroupBox = new System.Windows.Forms.GroupBox();
            this.showFriendOverhead = new System.Windows.Forms.CheckBox();
            this.setFriendsFormatHue = new System.Windows.Forms.Button();
            this.friendOverheadFormat = new System.Windows.Forms.TextBox();
            this.friendFormat = new System.Windows.Forms.Label();
            this.friendsGroupRemove = new System.Windows.Forms.Button();
            this.friendsGroupAdd = new System.Windows.Forms.Button();
            this.friendClearList = new System.Windows.Forms.Button();
            this.friendRemoveSelected = new System.Windows.Forms.Button();
            this.friendAddTarget = new System.Windows.Forms.Button();
            this.friendsList = new System.Windows.Forms.ListBox();
            this.friendsGroup = new System.Windows.Forms.ComboBox();
            this.friendsListEnabled = new System.Windows.Forms.CheckBox();
            this.screenshotTab = new System.Windows.Forms.TabPage();
            this.openScreenshotFolder = new System.Windows.Forms.Button();
            this.captureOwnDeathDelay = new System.Windows.Forms.TextBox();
            this.lblOwnDeathMs = new System.Windows.Forms.Label();
            this.captureOwnDeath = new System.Windows.Forms.CheckBox();
            this.captureOthersDeathDelay = new System.Windows.Forms.TextBox();
            this.lblCaptureOthersDeathMs = new System.Windows.Forms.Label();
            this.imgFmt = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.capNow = new System.Windows.Forms.Button();
            this.screenPath = new System.Windows.Forms.TextBox();
            this.radioUO = new System.Windows.Forms.RadioButton();
            this.radioFull = new System.Windows.Forms.RadioButton();
            this.captureOthersDeath = new System.Windows.Forms.CheckBox();
            this.setScnPath = new System.Windows.Forms.Button();
            this.screensList = new System.Windows.Forms.ListBox();
            this.screenPrev = new System.Windows.Forms.PictureBox();
            this.dispTime = new System.Windows.Forms.CheckBox();
            this.advancedTab = new System.Windows.Forms.TabPage();
            this.subAdvancedTab = new System.Windows.Forms.TabControl();
            this.advancedInfoTab = new System.Windows.Forms.TabPage();
            this.fontDecrease = new System.Windows.Forms.Button();
            this.fontIncrease = new System.Windows.Forms.Button();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.lastBackup = new System.Windows.Forms.Label();
            this.openBackupFolder = new System.Windows.Forms.Button();
            this.setBackupFolder = new System.Windows.Forms.Button();
            this.createBackup = new System.Windows.Forms.Button();
            this.enableUOAAPI = new System.Windows.Forms.CheckBox();
            this.disableSmartCPU = new System.Windows.Forms.Button();
            this.negotiate = new System.Windows.Forms.CheckBox();
            this.openRazorDataDir = new System.Windows.Forms.Button();
            this.msglvl = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.logPackets = new System.Windows.Forms.CheckBox();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.features = new System.Windows.Forms.TextBox();
            this.advancedStaffDeco = new System.Windows.Forms.TabPage();
            this.itemAppendM = new System.Windows.Forms.CheckBox();
            this.itemCopyToClipboard = new System.Windows.Forms.CheckBox();
            this.itemSearch = new System.Windows.Forms.TextBox();
            this.itemMovable = new System.Windows.Forms.CheckBox();
            this.itemRandomNumber = new System.Windows.Forms.NumericUpDown();
            this.itemRandom = new System.Windows.Forms.CheckBox();
            this.itemTileCount = new System.Windows.Forms.NumericUpDown();
            this.itemTile = new System.Windows.Forms.Button();
            this.itemAdd = new System.Windows.Forms.Button();
            this.itemTree = new System.Windows.Forms.TreeView();
            this.artViewer = new Assistant.UI.ArtViewer();
            this.advancedStaffDoors = new System.Windows.Forms.TabPage();
            this.doorViewer = new Assistant.UI.ArtViewer();
            this.doorTree = new System.Windows.Forms.TreeView();
            this.doorSouthCW = new System.Windows.Forms.Button();
            this.doorSouthCCW = new System.Windows.Forms.Button();
            this.doorEastCW = new System.Windows.Forms.Button();
            this.doorEastCCW = new System.Windows.Forms.Button();
            this.doorNorthCCW = new System.Windows.Forms.Button();
            this.doorNorthCW = new System.Windows.Forms.Button();
            this.doorWestCCW = new System.Windows.Forms.Button();
            this.doorWestCW = new System.Windows.Forms.Button();
            this.aboutTab = new System.Windows.Forms.TabPage();
            this.linkGitHub = new System.Windows.Forms.LinkLabel();
            this.lblCredits3 = new System.Windows.Forms.Label();
            this.linkHelp = new System.Windows.Forms.LinkLabel();
            this.lblCredits2 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblCredits1 = new System.Windows.Forms.Label();
            this.aboutSubInfo = new System.Windows.Forms.Label();
            this.linkMain = new System.Windows.Forms.LinkLabel();
            this.label21 = new System.Windows.Forms.Label();
            this.aboutVer = new System.Windows.Forms.Label();
            this.tabs.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.subGeneralTab.SuspendLayout();
            this.subGenTab.SuspendLayout();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.moreOptTab.SuspendLayout();
            this.optionsTabCtrl.SuspendLayout();
            this.subOptionsSpeechTab.SuspendLayout();
            this.subOptionsTargetTab.SuspendLayout();
            this.groupSmartTarget.SuspendLayout();
            this.subOptionsMiscTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lightLevelBar)).BeginInit();
            this.displayTab.SuspendLayout();
            this.displayCountersTabCtrl.SuspendLayout();
            this.subDisplayTab.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.subCountersTab.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.subBandageTimerTab.SuspendLayout();
            this.subOverheadTab.SuspendLayout();
            this.subWaypoints.SuspendLayout();
            this.subBuffsDebuffs.SuspendLayout();
            this.cooldownGumpBox.SuspendLayout();
            this.buffBarGroupBox.SuspendLayout();
            this.dressTab.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.skillsTab.SuspendLayout();
            this.agentsTab.SuspendLayout();
            this.agentGroup.SuspendLayout();
            this.filtersTab.SuspendLayout();
            this.filterTabs.SuspendLayout();
            this.subFilterTab.SuspendLayout();
            this.subFilterText.SuspendLayout();
            this.gbFilterText.SuspendLayout();
            this.gbFilterMessages.SuspendLayout();
            this.subFilterSoundMusic.SuspendLayout();
            this.subFilterTargets.SuspendLayout();
            this.hotkeysTab.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.macrosTab.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.subMacrosTab.SuspendLayout();
            this.macroActGroup.SuspendLayout();
            this.subMacrosOptionsTab.SuspendLayout();
            this.macroVariableGroup.SuspendLayout();
            this.scriptsTab.SuspendLayout();
            this.subTabScripts.SuspendLayout();
            this.subScripts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scriptSplitContainer)).BeginInit();
            this.scriptSplitContainer.Panel1.SuspendLayout();
            this.scriptSplitContainer.Panel2.SuspendLayout();
            this.scriptSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scriptEditor)).BeginInit();
            this.subScriptOptions.SuspendLayout();
            this.scriptVariablesBox.SuspendLayout();
            this.friendsTab.SuspendLayout();
            this.friendsGroupBox.SuspendLayout();
            this.screenshotTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenPrev)).BeginInit();
            this.advancedTab.SuspendLayout();
            this.subAdvancedTab.SuspendLayout();
            this.advancedInfoTab.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.advancedStaffDeco.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemRandomNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemTileCount)).BeginInit();
            this.advancedStaffDoors.SuspendLayout();
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
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.generalTab);
            this.tabs.Controls.Add(this.moreOptTab);
            this.tabs.Controls.Add(this.displayTab);
            this.tabs.Controls.Add(this.dressTab);
            this.tabs.Controls.Add(this.skillsTab);
            this.tabs.Controls.Add(this.agentsTab);
            this.tabs.Controls.Add(this.filtersTab);
            this.tabs.Controls.Add(this.hotkeysTab);
            this.tabs.Controls.Add(this.macrosTab);
            this.tabs.Controls.Add(this.scriptsTab);
            this.tabs.Controls.Add(this.friendsTab);
            this.tabs.Controls.Add(this.screenshotTab);
            this.tabs.Controls.Add(this.advancedTab);
            this.tabs.Controls.Add(this.aboutTab);
            this.tabs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabs.Location = new System.Drawing.Point(5, 0);
            this.tabs.Multiline = true;
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1325, 935);
            this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabs.TabIndex = 0;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_IndexChanged);
            this.tabs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabs_KeyDown);
            // 
            // generalTab
            // 
            this.generalTab.Controls.Add(this.subGeneralTab);
            this.generalTab.Location = new System.Drawing.Point(10, 106);
            this.generalTab.Name = "generalTab";
            this.generalTab.Size = new System.Drawing.Size(1305, 819);
            this.generalTab.TabIndex = 0;
            this.generalTab.Text = "General";
            // 
            // subGeneralTab
            // 
            this.subGeneralTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subGeneralTab.Controls.Add(this.subGenTab);
            this.subGeneralTab.Location = new System.Drawing.Point(15, 8);
            this.subGeneralTab.Name = "subGeneralTab";
            this.subGeneralTab.SelectedIndex = 0;
            this.subGeneralTab.Size = new System.Drawing.Size(1253, 480);
            this.subGeneralTab.TabIndex = 63;
            this.subGeneralTab.SelectedIndexChanged += new System.EventHandler(this.subGeneralTab_IndexChanged);
            // 
            // subGenTab
            // 
            this.subGenTab.BackColor = System.Drawing.SystemColors.Control;
            this.subGenTab.Controls.Add(this.groupBox15);
            this.subGenTab.Controls.Add(this.systray);
            this.subGenTab.Controls.Add(this.taskbar);
            this.subGenTab.Controls.Add(this.langSel);
            this.subGenTab.Controls.Add(this.label7);
            this.subGenTab.Controls.Add(this.label11);
            this.subGenTab.Controls.Add(this.showWelcome);
            this.subGenTab.Controls.Add(this.opacity);
            this.subGenTab.Controls.Add(this.alwaysTop);
            this.subGenTab.Controls.Add(this.opacityLabel);
            this.subGenTab.Controls.Add(this.groupBox4);
            this.subGenTab.Location = new System.Drawing.Point(10, 58);
            this.subGenTab.Name = "subGenTab";
            this.subGenTab.Padding = new System.Windows.Forms.Padding(3);
            this.subGenTab.Size = new System.Drawing.Size(1233, 412);
            this.subGenTab.TabIndex = 0;
            this.subGenTab.Text = "General";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.boatControl);
            this.groupBox15.Controls.Add(this.btnMap);
            this.groupBox15.Location = new System.Drawing.Point(35, 580);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(557, 120);
            this.groupBox15.TabIndex = 77;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Map / Boat";
            // 
            // boatControl
            // 
            this.boatControl.Location = new System.Drawing.Point(298, 40);
            this.boatControl.Name = "boatControl";
            this.boatControl.Size = new System.Drawing.Size(240, 65);
            this.boatControl.TabIndex = 70;
            this.boatControl.Text = "Boat Control";
            this.boatControl.UseVisualStyleBackColor = true;
            this.boatControl.Click += new System.EventHandler(this.boatControl_Click);
            // 
            // btnMap
            // 
            this.btnMap.Location = new System.Drawing.Point(15, 40);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(267, 65);
            this.btnMap.TabIndex = 67;
            this.btnMap.Text = "UOPS";
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // systray
            // 
            this.systray.Location = new System.Drawing.Point(410, 295);
            this.systray.Name = "systray";
            this.systray.Size = new System.Drawing.Size(220, 57);
            this.systray.TabIndex = 69;
            this.systray.Text = "System Tray";
            this.systray.CheckedChanged += new System.EventHandler(this.systray_CheckedChanged);
            // 
            // taskbar
            // 
            this.taskbar.Location = new System.Drawing.Point(230, 295);
            this.taskbar.Name = "taskbar";
            this.taskbar.Size = new System.Drawing.Size(198, 57);
            this.taskbar.TabIndex = 68;
            this.taskbar.Text = "Taskbar";
            this.taskbar.CheckedChanged += new System.EventHandler(this.taskbar_CheckedChanged);
            // 
            // langSel
            // 
            this.langSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.langSel.Location = new System.Drawing.Point(852, 422);
            this.langSel.Name = "langSel";
            this.langSel.Size = new System.Drawing.Size(340, 49);
            this.langSel.TabIndex = 71;
            this.langSel.SelectedIndexChanged += new System.EventHandler(this.langSel_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(668, 430);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(170, 45);
            this.label7.TabIndex = 70;
            this.label7.Text = "Language:";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(28, 305);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(134, 40);
            this.label11.TabIndex = 67;
            this.label11.Text = "Show in:";
            // 
            // showWelcome
            // 
            this.showWelcome.Location = new System.Drawing.Point(675, 268);
            this.showWelcome.Name = "showWelcome";
            this.showWelcome.Size = new System.Drawing.Size(380, 57);
            this.showWelcome.TabIndex = 66;
            this.showWelcome.Text = "Show Welcome Screen";
            this.showWelcome.CheckedChanged += new System.EventHandler(this.showWelcome_CheckedChanged);
            // 
            // opacity
            // 
            this.opacity.AutoSize = false;
            this.opacity.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.opacity.Location = new System.Drawing.Point(242, 375);
            this.opacity.Maximum = 100;
            this.opacity.Minimum = 10;
            this.opacity.Name = "opacity";
            this.opacity.Size = new System.Drawing.Size(388, 53);
            this.opacity.TabIndex = 64;
            this.opacity.TickFrequency = 0;
            this.opacity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.opacity.Value = 100;
            this.opacity.Scroll += new System.EventHandler(this.opacity_Scroll);
            // 
            // alwaysTop
            // 
            this.alwaysTop.Location = new System.Drawing.Point(675, 340);
            this.alwaysTop.Name = "alwaysTop";
            this.alwaysTop.Size = new System.Drawing.Size(405, 58);
            this.alwaysTop.TabIndex = 63;
            this.alwaysTop.Text = "Use Smart Always on Top";
            this.alwaysTop.CheckedChanged += new System.EventHandler(this.alwaysTop_CheckedChanged);
            // 
            // opacityLabel
            // 
            this.opacityLabel.Location = new System.Drawing.Point(28, 380);
            this.opacityLabel.Name = "opacityLabel";
            this.opacityLabel.Size = new System.Drawing.Size(144, 48);
            this.opacityLabel.TabIndex = 65;
            this.opacityLabel.Text = "Opacity: 100%";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.saveProfile);
            this.groupBox4.Controls.Add(this.cloneProfile);
            this.groupBox4.Controls.Add(this.delProfile);
            this.groupBox4.Controls.Add(this.newProfile);
            this.groupBox4.Controls.Add(this.profiles);
            this.groupBox4.Location = new System.Drawing.Point(15, 15);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1173, 237);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Profiles";
            // 
            // saveProfile
            // 
            this.saveProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveProfile.AutoSize = true;
            this.saveProfile.Location = new System.Drawing.Point(650, 140);
            this.saveProfile.Name = "saveProfile";
            this.saveProfile.Size = new System.Drawing.Size(223, 128);
            this.saveProfile.TabIndex = 4;
            this.saveProfile.Text = "Save";
            this.saveProfile.Click += new System.EventHandler(this.saveProfile_Click);
            // 
            // cloneProfile
            // 
            this.cloneProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cloneProfile.AutoSize = true;
            this.cloneProfile.Location = new System.Drawing.Point(750, 140);
            this.cloneProfile.Name = "cloneProfile";
            this.cloneProfile.Size = new System.Drawing.Size(263, 128);
            this.cloneProfile.TabIndex = 3;
            this.cloneProfile.Text = "Clone";
            this.cloneProfile.Click += new System.EventHandler(this.cloneProfile_Click);
            // 
            // delProfile
            // 
            this.delProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delProfile.AutoSize = true;
            this.delProfile.Location = new System.Drawing.Point(868, 140);
            this.delProfile.Name = "delProfile";
            this.delProfile.Size = new System.Drawing.Size(285, 128);
            this.delProfile.TabIndex = 2;
            this.delProfile.Text = "Delete";
            this.delProfile.Click += new System.EventHandler(this.delProfile_Click);
            // 
            // newProfile
            // 
            this.newProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newProfile.AutoSize = true;
            this.newProfile.Location = new System.Drawing.Point(513, 140);
            this.newProfile.Name = "newProfile";
            this.newProfile.Size = new System.Drawing.Size(220, 128);
            this.newProfile.TabIndex = 1;
            this.newProfile.Text = "New";
            this.newProfile.Click += new System.EventHandler(this.newProfile_Click);
            // 
            // profiles
            // 
            this.profiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profiles.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profiles.ItemHeight = 50;
            this.profiles.Location = new System.Drawing.Point(15, 55);
            this.profiles.MaxDropDownItems = 5;
            this.profiles.Name = "profiles";
            this.profiles.Size = new System.Drawing.Size(1138, 58);
            this.profiles.TabIndex = 0;
            this.profiles.SelectedIndexChanged += new System.EventHandler(this.profiles_SelectedIndexChanged);
            // 
            // moreOptTab
            // 
            this.moreOptTab.Controls.Add(this.optionsTabCtrl);
            this.moreOptTab.Location = new System.Drawing.Point(10, 106);
            this.moreOptTab.Name = "moreOptTab";
            this.moreOptTab.Size = new System.Drawing.Size(1305, 819);
            this.moreOptTab.TabIndex = 5;
            this.moreOptTab.Text = "Options";
            // 
            // optionsTabCtrl
            // 
            this.optionsTabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsTabCtrl.Controls.Add(this.subOptionsSpeechTab);
            this.optionsTabCtrl.Controls.Add(this.subOptionsTargetTab);
            this.optionsTabCtrl.Controls.Add(this.subOptionsMiscTab);
            this.optionsTabCtrl.Location = new System.Drawing.Point(15, 8);
            this.optionsTabCtrl.Name = "optionsTabCtrl";
            this.optionsTabCtrl.SelectedIndex = 0;
            this.optionsTabCtrl.Size = new System.Drawing.Size(1253, 480);
            this.optionsTabCtrl.TabIndex = 93;
            // 
            // subOptionsSpeechTab
            // 
            this.subOptionsSpeechTab.BackColor = System.Drawing.SystemColors.Control;
            this.subOptionsSpeechTab.Controls.Add(this.playEmoteSound);
            this.subOptionsSpeechTab.Controls.Add(this.overrideSpellFormat);
            this.subOptionsSpeechTab.Controls.Add(this.damageTakenOverhead);
            this.subOptionsSpeechTab.Controls.Add(this.showDamageTaken);
            this.subOptionsSpeechTab.Controls.Add(this.damageDealtOverhead);
            this.subOptionsSpeechTab.Controls.Add(this.showDamageDealt);
            this.subOptionsSpeechTab.Controls.Add(this.healthFmt);
            this.subOptionsSpeechTab.Controls.Add(this.label10);
            this.subOptionsSpeechTab.Controls.Add(this.showHealthOH);
            this.subOptionsSpeechTab.Controls.Add(this.chkPartyOverhead);
            this.subOptionsSpeechTab.Controls.Add(this.containerLabels);
            this.subOptionsSpeechTab.Controls.Add(this.showContainerLabels);
            this.subOptionsSpeechTab.Controls.Add(this.incomingMob);
            this.subOptionsSpeechTab.Controls.Add(this.incomingCorpse);
            this.subOptionsSpeechTab.Controls.Add(this.setLTHilight);
            this.subOptionsSpeechTab.Controls.Add(this.lthilight);
            this.subOptionsSpeechTab.Controls.Add(this.setHarmHue);
            this.subOptionsSpeechTab.Controls.Add(this.setNeuHue);
            this.subOptionsSpeechTab.Controls.Add(this.lblHarmHue);
            this.subOptionsSpeechTab.Controls.Add(this.lblNeuHue);
            this.subOptionsSpeechTab.Controls.Add(this.lblBeneHue);
            this.subOptionsSpeechTab.Controls.Add(this.setBeneHue);
            this.subOptionsSpeechTab.Controls.Add(this.setSpeechHue);
            this.subOptionsSpeechTab.Controls.Add(this.setWarnHue);
            this.subOptionsSpeechTab.Controls.Add(this.setMsgHue);
            this.subOptionsSpeechTab.Controls.Add(this.setExHue);
            this.subOptionsSpeechTab.Controls.Add(this.lblWarnHue);
            this.subOptionsSpeechTab.Controls.Add(this.lblMsgHue);
            this.subOptionsSpeechTab.Controls.Add(this.lblExHue);
            this.subOptionsSpeechTab.Controls.Add(this.txtSpellFormat);
            this.subOptionsSpeechTab.Controls.Add(this.chkForceSpellHue);
            this.subOptionsSpeechTab.Controls.Add(this.chkForceSpeechHue);
            this.subOptionsSpeechTab.Controls.Add(this.label3);
            this.subOptionsSpeechTab.Location = new System.Drawing.Point(10, 58);
            this.subOptionsSpeechTab.Name = "subOptionsSpeechTab";
            this.subOptionsSpeechTab.Padding = new System.Windows.Forms.Padding(3);
            this.subOptionsSpeechTab.Size = new System.Drawing.Size(1233, 412);
            this.subOptionsSpeechTab.TabIndex = 0;
            this.subOptionsSpeechTab.Text = "Speech & Messages  ";
            // 
            // playEmoteSound
            // 
            this.playEmoteSound.AutoSize = true;
            this.playEmoteSound.Location = new System.Drawing.Point(650, 525);
            this.playEmoteSound.Name = "playEmoteSound";
            this.playEmoteSound.Size = new System.Drawing.Size(334, 45);
            this.playEmoteSound.TabIndex = 131;
            this.playEmoteSound.Text = "Play *emote* sounds";
            this.playEmoteSound.UseVisualStyleBackColor = true;
            this.playEmoteSound.CheckedChanged += new System.EventHandler(this.playEmoteSound_CheckedChanged);
            // 
            // overrideSpellFormat
            // 
            this.overrideSpellFormat.Location = new System.Drawing.Point(22, 512);
            this.overrideSpellFormat.Name = "overrideSpellFormat";
            this.overrideSpellFormat.Size = new System.Drawing.Size(380, 50);
            this.overrideSpellFormat.TabIndex = 130;
            this.overrideSpellFormat.Text = "Override spell format";
            this.overrideSpellFormat.CheckedChanged += new System.EventHandler(this.overrideSpellFormat_CheckedChanged);
            // 
            // damageTakenOverhead
            // 
            this.damageTakenOverhead.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.damageTakenOverhead.Location = new System.Drawing.Point(985, 462);
            this.damageTakenOverhead.Name = "damageTakenOverhead";
            this.damageTakenOverhead.Size = new System.Drawing.Size(193, 48);
            this.damageTakenOverhead.TabIndex = 128;
            this.damageTakenOverhead.Text = "Overhead";
            this.damageTakenOverhead.UseVisualStyleBackColor = true;
            this.damageTakenOverhead.CheckedChanged += new System.EventHandler(this.damageTakenOverhead_CheckedChanged);
            // 
            // showDamageTaken
            // 
            this.showDamageTaken.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.showDamageTaken.Location = new System.Drawing.Point(650, 462);
            this.showDamageTaken.Name = "showDamageTaken";
            this.showDamageTaken.Size = new System.Drawing.Size(348, 48);
            this.showDamageTaken.TabIndex = 127;
            this.showDamageTaken.Text = "Show damage taken";
            this.showDamageTaken.UseVisualStyleBackColor = true;
            this.showDamageTaken.CheckedChanged += new System.EventHandler(this.showDamageTaken_CheckedChanged);
            // 
            // damageDealtOverhead
            // 
            this.damageDealtOverhead.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.damageDealtOverhead.Location = new System.Drawing.Point(985, 400);
            this.damageDealtOverhead.Name = "damageDealtOverhead";
            this.damageDealtOverhead.Size = new System.Drawing.Size(193, 48);
            this.damageDealtOverhead.TabIndex = 126;
            this.damageDealtOverhead.Text = "Overhead";
            this.damageDealtOverhead.UseVisualStyleBackColor = true;
            this.damageDealtOverhead.CheckedChanged += new System.EventHandler(this.damageDealtOverhead_CheckedChanged);
            // 
            // showDamageDealt
            // 
            this.showDamageDealt.AutoSize = true;
            this.showDamageDealt.Location = new System.Drawing.Point(650, 400);
            this.showDamageDealt.Name = "showDamageDealt";
            this.showDamageDealt.Size = new System.Drawing.Size(319, 45);
            this.showDamageDealt.TabIndex = 125;
            this.showDamageDealt.Text = "Show damage dealt";
            this.showDamageDealt.UseVisualStyleBackColor = true;
            this.showDamageDealt.CheckedChanged += new System.EventHandler(this.showDamageDealt_CheckedChanged);
            // 
            // healthFmt
            // 
            this.healthFmt.Location = new System.Drawing.Point(942, 275);
            this.healthFmt.Name = "healthFmt";
            this.healthFmt.Size = new System.Drawing.Size(133, 47);
            this.healthFmt.TabIndex = 89;
            this.healthFmt.Text = "[{0}%]";
            this.healthFmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.healthFmt.TextChanged += new System.EventHandler(this.healthFmt_TextChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(708, 278);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(287, 44);
            this.label10.TabIndex = 88;
            this.label10.Text = "Health Format:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // showHealthOH
            // 
            this.showHealthOH.Location = new System.Drawing.Point(650, 220);
            this.showHealthOH.Name = "showHealthOH";
            this.showHealthOH.Size = new System.Drawing.Size(578, 50);
            this.showHealthOH.TabIndex = 87;
            this.showHealthOH.Text = "Show health above people/creatures";
            this.showHealthOH.CheckedChanged += new System.EventHandler(this.showHealthOH_CheckedChanged);
            // 
            // chkPartyOverhead
            // 
            this.chkPartyOverhead.Location = new System.Drawing.Point(650, 335);
            this.chkPartyOverhead.Name = "chkPartyOverhead";
            this.chkPartyOverhead.Size = new System.Drawing.Size(595, 50);
            this.chkPartyOverhead.TabIndex = 90;
            this.chkPartyOverhead.Text = "Show mana/stam above party members";
            this.chkPartyOverhead.CheckedChanged += new System.EventHandler(this.chkPartyOverhead_CheckedChanged);
            // 
            // containerLabels
            // 
            this.containerLabels.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerLabels.Location = new System.Drawing.Point(1095, 158);
            this.containerLabels.Name = "containerLabels";
            this.containerLabels.Size = new System.Drawing.Size(83, 47);
            this.containerLabels.TabIndex = 86;
            this.containerLabels.Text = "...";
            this.containerLabels.UseVisualStyleBackColor = true;
            this.containerLabels.Click += new System.EventHandler(this.containerLabels_Click);
            // 
            // showContainerLabels
            // 
            this.showContainerLabels.AutoSize = true;
            this.showContainerLabels.Location = new System.Drawing.Point(650, 158);
            this.showContainerLabels.Name = "showContainerLabels";
            this.showContainerLabels.Size = new System.Drawing.Size(345, 45);
            this.showContainerLabels.TabIndex = 85;
            this.showContainerLabels.Text = "Show container labels";
            this.showContainerLabels.UseVisualStyleBackColor = true;
            this.showContainerLabels.CheckedChanged += new System.EventHandler(this.showContainerLabels_CheckedChanged);
            // 
            // incomingMob
            // 
            this.incomingMob.Location = new System.Drawing.Point(650, 25);
            this.incomingMob.Name = "incomingMob";
            this.incomingMob.Size = new System.Drawing.Size(528, 50);
            this.incomingMob.TabIndex = 71;
            this.incomingMob.Text = "Show Names of Incoming People/Creatures";
            this.incomingMob.CheckedChanged += new System.EventHandler(this.incomingMob_CheckedChanged);
            // 
            // incomingCorpse
            // 
            this.incomingCorpse.Location = new System.Drawing.Point(650, 90);
            this.incomingCorpse.Name = "incomingCorpse";
            this.incomingCorpse.Size = new System.Drawing.Size(595, 50);
            this.incomingCorpse.TabIndex = 72;
            this.incomingCorpse.Text = "Show Names of New/Incoming Corpses";
            this.incomingCorpse.CheckedChanged += new System.EventHandler(this.incomingCorpse_CheckedChanged);
            // 
            // setLTHilight
            // 
            this.setLTHilight.Location = new System.Drawing.Point(420, 290);
            this.setLTHilight.Name = "setLTHilight";
            this.setLTHilight.Size = new System.Drawing.Size(118, 50);
            this.setLTHilight.TabIndex = 70;
            this.setLTHilight.Text = "Set";
            this.setLTHilight.Click += new System.EventHandler(this.setLTHilight_Click);
            // 
            // lthilight
            // 
            this.lthilight.Location = new System.Drawing.Point(22, 290);
            this.lthilight.Name = "lthilight";
            this.lthilight.Size = new System.Drawing.Size(516, 48);
            this.lthilight.TabIndex = 69;
            this.lthilight.Text = "Last Target Highlight:";
            this.lthilight.CheckedChanged += new System.EventHandler(this.lthilight_CheckedChanged);
            // 
            // setHarmHue
            // 
            this.setHarmHue.Enabled = false;
            this.setHarmHue.Location = new System.Drawing.Point(248, 448);
            this.setHarmHue.Name = "setHarmHue";
            this.setHarmHue.Size = new System.Drawing.Size(80, 50);
            this.setHarmHue.TabIndex = 64;
            this.setHarmHue.Text = "Set";
            this.setHarmHue.Click += new System.EventHandler(this.setHarmHue_Click);
            // 
            // setNeuHue
            // 
            this.setNeuHue.Enabled = false;
            this.setNeuHue.Location = new System.Drawing.Point(430, 448);
            this.setNeuHue.Name = "setNeuHue";
            this.setNeuHue.Size = new System.Drawing.Size(80, 50);
            this.setNeuHue.TabIndex = 65;
            this.setNeuHue.Text = "Set";
            this.setNeuHue.Click += new System.EventHandler(this.setNeuHue_Click);
            // 
            // lblHarmHue
            // 
            this.lblHarmHue.Location = new System.Drawing.Point(215, 412);
            this.lblHarmHue.Name = "lblHarmHue";
            this.lblHarmHue.Size = new System.Drawing.Size(147, 36);
            this.lblHarmHue.TabIndex = 68;
            this.lblHarmHue.Text = "Harmful";
            this.lblHarmHue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNeuHue
            // 
            this.lblNeuHue.Location = new System.Drawing.Point(408, 412);
            this.lblNeuHue.Name = "lblNeuHue";
            this.lblNeuHue.Size = new System.Drawing.Size(130, 36);
            this.lblNeuHue.TabIndex = 67;
            this.lblNeuHue.Text = "Neutral";
            this.lblNeuHue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBeneHue
            // 
            this.lblBeneHue.Location = new System.Drawing.Point(22, 412);
            this.lblBeneHue.Name = "lblBeneHue";
            this.lblBeneHue.Size = new System.Drawing.Size(166, 36);
            this.lblBeneHue.TabIndex = 66;
            this.lblBeneHue.Text = "Beneficial";
            this.lblBeneHue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // setBeneHue
            // 
            this.setBeneHue.Enabled = false;
            this.setBeneHue.Location = new System.Drawing.Point(68, 448);
            this.setBeneHue.Name = "setBeneHue";
            this.setBeneHue.Size = new System.Drawing.Size(80, 50);
            this.setBeneHue.TabIndex = 63;
            this.setBeneHue.Text = "Set";
            this.setBeneHue.Click += new System.EventHandler(this.setBeneHue_Click);
            // 
            // setSpeechHue
            // 
            this.setSpeechHue.Location = new System.Drawing.Point(420, 225);
            this.setSpeechHue.Name = "setSpeechHue";
            this.setSpeechHue.Size = new System.Drawing.Size(118, 50);
            this.setSpeechHue.TabIndex = 62;
            this.setSpeechHue.Text = "Set";
            this.setSpeechHue.Click += new System.EventHandler(this.setSpeechHue_Click);
            // 
            // setWarnHue
            // 
            this.setWarnHue.Location = new System.Drawing.Point(420, 160);
            this.setWarnHue.Name = "setWarnHue";
            this.setWarnHue.Size = new System.Drawing.Size(118, 50);
            this.setWarnHue.TabIndex = 61;
            this.setWarnHue.Text = "Set";
            this.setWarnHue.Click += new System.EventHandler(this.setWarnHue_Click);
            // 
            // setMsgHue
            // 
            this.setMsgHue.Location = new System.Drawing.Point(420, 95);
            this.setMsgHue.Name = "setMsgHue";
            this.setMsgHue.Size = new System.Drawing.Size(118, 50);
            this.setMsgHue.TabIndex = 60;
            this.setMsgHue.Text = "Set";
            this.setMsgHue.Click += new System.EventHandler(this.setMsgHue_Click);
            // 
            // setExHue
            // 
            this.setExHue.Location = new System.Drawing.Point(420, 30);
            this.setExHue.Name = "setExHue";
            this.setExHue.Size = new System.Drawing.Size(118, 50);
            this.setExHue.TabIndex = 59;
            this.setExHue.Text = "Set";
            this.setExHue.Click += new System.EventHandler(this.setExHue_Click);
            // 
            // lblWarnHue
            // 
            this.lblWarnHue.Location = new System.Drawing.Point(22, 160);
            this.lblWarnHue.Name = "lblWarnHue";
            this.lblWarnHue.Size = new System.Drawing.Size(516, 45);
            this.lblWarnHue.TabIndex = 58;
            this.lblWarnHue.Text = "Warning Message Hue:";
            // 
            // lblMsgHue
            // 
            this.lblMsgHue.Location = new System.Drawing.Point(22, 95);
            this.lblMsgHue.Name = "lblMsgHue";
            this.lblMsgHue.Size = new System.Drawing.Size(516, 45);
            this.lblMsgHue.TabIndex = 57;
            this.lblMsgHue.Text = "Razor Message Hue:";
            // 
            // lblExHue
            // 
            this.lblExHue.Location = new System.Drawing.Point(22, 30);
            this.lblExHue.Name = "lblExHue";
            this.lblExHue.Size = new System.Drawing.Size(516, 45);
            this.lblExHue.TabIndex = 56;
            this.lblExHue.Text = "Search Exemption Hue:";
            // 
            // txtSpellFormat
            // 
            this.txtSpellFormat.Location = new System.Drawing.Point(222, 570);
            this.txtSpellFormat.Name = "txtSpellFormat";
            this.txtSpellFormat.Size = new System.Drawing.Size(316, 47);
            this.txtSpellFormat.TabIndex = 55;
            this.txtSpellFormat.TextChanged += new System.EventHandler(this.txtSpellFormat_TextChanged);
            // 
            // chkForceSpellHue
            // 
            this.chkForceSpellHue.Location = new System.Drawing.Point(22, 355);
            this.chkForceSpellHue.Name = "chkForceSpellHue";
            this.chkForceSpellHue.Size = new System.Drawing.Size(380, 50);
            this.chkForceSpellHue.TabIndex = 53;
            this.chkForceSpellHue.Text = "Override Spell Hues:";
            this.chkForceSpellHue.CheckedChanged += new System.EventHandler(this.chkForceSpellHue_CheckedChanged);
            // 
            // chkForceSpeechHue
            // 
            this.chkForceSpeechHue.Location = new System.Drawing.Point(22, 225);
            this.chkForceSpeechHue.Name = "chkForceSpeechHue";
            this.chkForceSpeechHue.Size = new System.Drawing.Size(516, 47);
            this.chkForceSpeechHue.TabIndex = 52;
            this.chkForceSpeechHue.Text = "Override Speech Hue:";
            this.chkForceSpeechHue.CheckedChanged += new System.EventHandler(this.chkForceSpeechHue_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(28, 570);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 50);
            this.label3.TabIndex = 54;
            this.label3.Text = "Spell Format:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // subOptionsTargetTab
            // 
            this.subOptionsTargetTab.BackColor = System.Drawing.SystemColors.Control;
            this.subOptionsTargetTab.Controls.Add(this.groupSmartTarget);
            this.subOptionsTargetTab.Controls.Add(this.setTargetIndicatorHue);
            this.subOptionsTargetTab.Controls.Add(this.targetIndicatorFormat);
            this.subOptionsTargetTab.Controls.Add(this.showtargtext);
            this.subOptionsTargetTab.Controls.Add(this.showAttackTargetNewOnly);
            this.subOptionsTargetTab.Controls.Add(this.showTextTargetIndicator);
            this.subOptionsTargetTab.Controls.Add(this.showAttackTarget);
            this.subOptionsTargetTab.Controls.Add(this.showTargetMessagesOverChar);
            this.subOptionsTargetTab.Controls.Add(this.txtObjDelay);
            this.subOptionsTargetTab.Controls.Add(this.objectDelay);
            this.subOptionsTargetTab.Controls.Add(this.ltRange);
            this.subOptionsTargetTab.Controls.Add(this.QueueActions);
            this.subOptionsTargetTab.Controls.Add(this.rangeCheckLT);
            this.subOptionsTargetTab.Controls.Add(this.actionStatusMsg);
            this.subOptionsTargetTab.Controls.Add(this.label8);
            this.subOptionsTargetTab.Controls.Add(this.label6);
            this.subOptionsTargetTab.Controls.Add(this.queueTargets);
            this.subOptionsTargetTab.Controls.Add(this.lblTargetFormat);
            this.subOptionsTargetTab.Location = new System.Drawing.Point(10, 58);
            this.subOptionsTargetTab.Name = "subOptionsTargetTab";
            this.subOptionsTargetTab.Padding = new System.Windows.Forms.Padding(3);
            this.subOptionsTargetTab.Size = new System.Drawing.Size(1225, 306);
            this.subOptionsTargetTab.TabIndex = 1;
            this.subOptionsTargetTab.Text = "Targeting & Queues  ";
            // 
            // groupSmartTarget
            // 
            this.groupSmartTarget.Controls.Add(this.nextPrevAbcOrder);
            this.groupSmartTarget.Controls.Add(this.nonFriendlyHarmfulOnly);
            this.groupSmartTarget.Controls.Add(this.friendBeneficialOnly);
            this.groupSmartTarget.Controls.Add(this.onlyNextPrevBeneficial);
            this.groupSmartTarget.Controls.Add(this.smartLT);
            this.groupSmartTarget.Location = new System.Drawing.Point(608, 28);
            this.groupSmartTarget.Name = "groupSmartTarget";
            this.groupSmartTarget.Size = new System.Drawing.Size(632, 382);
            this.groupSmartTarget.TabIndex = 138;
            this.groupSmartTarget.TabStop = false;
            this.groupSmartTarget.Text = "Smart Targeting:";
            // 
            // nextPrevAbcOrder
            // 
            this.nextPrevAbcOrder.Location = new System.Drawing.Point(15, 308);
            this.nextPrevAbcOrder.Name = "nextPrevAbcOrder";
            this.nextPrevAbcOrder.Size = new System.Drawing.Size(580, 47);
            this.nextPrevAbcOrder.TabIndex = 142;
            this.nextPrevAbcOrder.Text = "\'Next/Prev\' by alphabetical order";
            this.nextPrevAbcOrder.UseVisualStyleBackColor = true;
            this.nextPrevAbcOrder.CheckedChanged += new System.EventHandler(this.nextPrevAbcOrder_CheckedChanged);
            // 
            // nonFriendlyHarmfulOnly
            // 
            this.nonFriendlyHarmfulOnly.Location = new System.Drawing.Point(15, 245);
            this.nonFriendlyHarmfulOnly.Name = "nonFriendlyHarmfulOnly";
            this.nonFriendlyHarmfulOnly.Size = new System.Drawing.Size(580, 47);
            this.nonFriendlyHarmfulOnly.TabIndex = 141;
            this.nonFriendlyHarmfulOnly.Text = "\'Next/Prev Non-Friendly\' harmful only";
            this.nonFriendlyHarmfulOnly.UseVisualStyleBackColor = true;
            this.nonFriendlyHarmfulOnly.CheckedChanged += new System.EventHandler(this.nonFriendlyHarmfulOnly_CheckedChanged);
            // 
            // friendBeneficialOnly
            // 
            this.friendBeneficialOnly.AutoSize = true;
            this.friendBeneficialOnly.Location = new System.Drawing.Point(15, 182);
            this.friendBeneficialOnly.Name = "friendBeneficialOnly";
            this.friendBeneficialOnly.Size = new System.Drawing.Size(573, 45);
            this.friendBeneficialOnly.TabIndex = 140;
            this.friendBeneficialOnly.Text = "\'Next/Prev Friendly\' sets beneficial only";
            this.friendBeneficialOnly.UseVisualStyleBackColor = true;
            this.friendBeneficialOnly.CheckedChanged += new System.EventHandler(this.friendBeneficialOnly_CheckedChanged);
            // 
            // onlyNextPrevBeneficial
            // 
            this.onlyNextPrevBeneficial.AutoSize = true;
            this.onlyNextPrevBeneficial.Location = new System.Drawing.Point(15, 120);
            this.onlyNextPrevBeneficial.Name = "onlyNextPrevBeneficial";
            this.onlyNextPrevBeneficial.Size = new System.Drawing.Size(551, 45);
            this.onlyNextPrevBeneficial.TabIndex = 139;
            this.onlyNextPrevBeneficial.Text = "\'Next/Prev Friend\' sets beneficial only";
            this.onlyNextPrevBeneficial.UseVisualStyleBackColor = true;
            this.onlyNextPrevBeneficial.CheckedChanged += new System.EventHandler(this.onlyNextPrevBeneficial_CheckedChanged);
            // 
            // smartLT
            // 
            this.smartLT.Location = new System.Drawing.Point(15, 55);
            this.smartLT.Name = "smartLT";
            this.smartLT.Size = new System.Drawing.Size(530, 50);
            this.smartLT.TabIndex = 138;
            this.smartLT.Text = "Use Smart Last Target";
            this.smartLT.CheckedChanged += new System.EventHandler(this.smartLT_CheckedChanged);
            // 
            // setTargetIndicatorHue
            // 
            this.setTargetIndicatorHue.Location = new System.Drawing.Point(442, 615);
            this.setTargetIndicatorHue.Name = "setTargetIndicatorHue";
            this.setTargetIndicatorHue.Size = new System.Drawing.Size(148, 57);
            this.setTargetIndicatorHue.TabIndex = 134;
            this.setTargetIndicatorHue.Text = "Set Hue";
            this.setTargetIndicatorHue.UseVisualStyleBackColor = true;
            this.setTargetIndicatorHue.Click += new System.EventHandler(this.setTargetIndicatorHue_Click);
            // 
            // targetIndicatorFormat
            // 
            this.targetIndicatorFormat.Location = new System.Drawing.Point(160, 615);
            this.targetIndicatorFormat.Name = "targetIndicatorFormat";
            this.targetIndicatorFormat.Size = new System.Drawing.Size(268, 47);
            this.targetIndicatorFormat.TabIndex = 93;
            this.targetIndicatorFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.targetIndicatorFormat.TextChanged += new System.EventHandler(this.targetIndicatorFormat_TextChanged);
            // 
            // showtargtext
            // 
            this.showtargtext.Location = new System.Drawing.Point(22, 425);
            this.showtargtext.Name = "showtargtext";
            this.showtargtext.Size = new System.Drawing.Size(530, 50);
            this.showtargtext.TabIndex = 91;
            this.showtargtext.Text = "Show target flag on single click";
            this.showtargtext.CheckedChanged += new System.EventHandler(this.showtargtext_CheckedChanged);
            // 
            // showAttackTargetNewOnly
            // 
            this.showAttackTargetNewOnly.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showAttackTargetNewOnly.Location = new System.Drawing.Point(488, 490);
            this.showAttackTargetNewOnly.Name = "showAttackTargetNewOnly";
            this.showAttackTargetNewOnly.Size = new System.Drawing.Size(302, 110);
            this.showAttackTargetNewOnly.TabIndex = 90;
            this.showAttackTargetNewOnly.Text = "New targets only";
            this.showAttackTargetNewOnly.UseVisualStyleBackColor = true;
            this.showAttackTargetNewOnly.CheckedChanged += new System.EventHandler(this.showAttackTargetNewOnly_CheckedChanged);
            // 
            // showTextTargetIndicator
            // 
            this.showTextTargetIndicator.Location = new System.Drawing.Point(22, 552);
            this.showTextTargetIndicator.Name = "showTextTargetIndicator";
            this.showTextTargetIndicator.Size = new System.Drawing.Size(580, 48);
            this.showTextTargetIndicator.TabIndex = 89;
            this.showTextTargetIndicator.Text = "Show text target indicator";
            this.showTextTargetIndicator.UseVisualStyleBackColor = true;
            this.showTextTargetIndicator.CheckedChanged += new System.EventHandler(this.showTextTargetIndicator_CheckedChanged);
            // 
            // showAttackTarget
            // 
            this.showAttackTarget.Location = new System.Drawing.Point(22, 490);
            this.showAttackTarget.Name = "showAttackTarget";
            this.showAttackTarget.Size = new System.Drawing.Size(580, 48);
            this.showAttackTarget.TabIndex = 88;
            this.showAttackTarget.Text = "Attack/target name overhead";
            this.showAttackTarget.UseVisualStyleBackColor = true;
            this.showAttackTarget.CheckedChanged += new System.EventHandler(this.showAttackTarget_CheckedChanged);
            // 
            // showTargetMessagesOverChar
            // 
            this.showTargetMessagesOverChar.AutoSize = true;
            this.showTargetMessagesOverChar.Location = new System.Drawing.Point(22, 295);
            this.showTargetMessagesOverChar.Name = "showTargetMessagesOverChar";
            this.showTargetMessagesOverChar.Size = new System.Drawing.Size(555, 45);
            this.showTargetMessagesOverChar.TabIndex = 74;
            this.showTargetMessagesOverChar.Text = "Show Target Self/Last/Clear Overhead";
            this.showTargetMessagesOverChar.UseVisualStyleBackColor = true;
            this.showTargetMessagesOverChar.CheckedChanged += new System.EventHandler(this.showTargetMessagesOverChar_CheckedChanged);
            // 
            // txtObjDelay
            // 
            this.txtObjDelay.Location = new System.Drawing.Point(272, 222);
            this.txtObjDelay.Name = "txtObjDelay";
            this.txtObjDelay.Size = new System.Drawing.Size(80, 47);
            this.txtObjDelay.TabIndex = 56;
            this.txtObjDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtObjDelay.TextChanged += new System.EventHandler(this.txtObjDelay_TextChanged);
            // 
            // objectDelay
            // 
            this.objectDelay.Location = new System.Drawing.Point(22, 220);
            this.objectDelay.Name = "objectDelay";
            this.objectDelay.Size = new System.Drawing.Size(260, 60);
            this.objectDelay.TabIndex = 53;
            this.objectDelay.Text = "Object Delay:";
            this.objectDelay.CheckedChanged += new System.EventHandler(this.objectDelay_CheckedChanged);
            // 
            // ltRange
            // 
            this.ltRange.Location = new System.Drawing.Point(412, 352);
            this.ltRange.Name = "ltRange";
            this.ltRange.Size = new System.Drawing.Size(76, 47);
            this.ltRange.TabIndex = 59;
            this.ltRange.TextChanged += new System.EventHandler(this.ltRange_TextChanged);
            // 
            // QueueActions
            // 
            this.QueueActions.Location = new System.Drawing.Point(22, 160);
            this.QueueActions.Name = "QueueActions";
            this.QueueActions.Size = new System.Drawing.Size(506, 50);
            this.QueueActions.TabIndex = 54;
            this.QueueActions.Text = "Auto-Queue Object Delay actions ";
            this.QueueActions.CheckedChanged += new System.EventHandler(this.QueueActions_CheckedChanged);
            // 
            // rangeCheckLT
            // 
            this.rangeCheckLT.Location = new System.Drawing.Point(22, 355);
            this.rangeCheckLT.Name = "rangeCheckLT";
            this.rangeCheckLT.Size = new System.Drawing.Size(406, 50);
            this.rangeCheckLT.TabIndex = 58;
            this.rangeCheckLT.Text = "Range check Last Target:";
            this.rangeCheckLT.CheckedChanged += new System.EventHandler(this.rangeCheckLT_CheckedChanged);
            // 
            // actionStatusMsg
            // 
            this.actionStatusMsg.Location = new System.Drawing.Point(22, 95);
            this.actionStatusMsg.Name = "actionStatusMsg";
            this.actionStatusMsg.Size = new System.Drawing.Size(530, 50);
            this.actionStatusMsg.TabIndex = 57;
            this.actionStatusMsg.Text = "Show Action-Queue status messages";
            this.actionStatusMsg.CheckedChanged += new System.EventHandler(this.actionStatusMsg_CheckedChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(502, 360);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 45);
            this.label8.TabIndex = 60;
            this.label8.Text = "tiles";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(368, 232);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 46);
            this.label6.TabIndex = 55;
            this.label6.Text = "ms";
            // 
            // queueTargets
            // 
            this.queueTargets.Location = new System.Drawing.Point(22, 30);
            this.queueTargets.Name = "queueTargets";
            this.queueTargets.Size = new System.Drawing.Size(570, 50);
            this.queueTargets.TabIndex = 35;
            this.queueTargets.Text = "Queue LastTarget and TargetSelf";
            this.queueTargets.CheckedChanged += new System.EventHandler(this.queueTargets_CheckedChanged);
            // 
            // lblTargetFormat
            // 
            this.lblTargetFormat.Location = new System.Drawing.Point(25, 615);
            this.lblTargetFormat.Name = "lblTargetFormat";
            this.lblTargetFormat.Size = new System.Drawing.Size(350, 57);
            this.lblTargetFormat.TabIndex = 94;
            this.lblTargetFormat.Text = "Format:";
            this.lblTargetFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // subOptionsMiscTab
            // 
            this.subOptionsMiscTab.BackColor = System.Drawing.SystemColors.Control;
            this.subOptionsMiscTab.Controls.Add(this.buyAgentIgnoreGold);
            this.subOptionsMiscTab.Controls.Add(this.reequipHandsPotion);
            this.subOptionsMiscTab.Controls.Add(this.autoOpenDoorWhenHidden);
            this.subOptionsMiscTab.Controls.Add(this.lblStealthFormat);
            this.subOptionsMiscTab.Controls.Add(this.stealthStepsFormat);
            this.subOptionsMiscTab.Controls.Add(this.rememberPwds);
            this.subOptionsMiscTab.Controls.Add(this.showStaticWalls);
            this.subOptionsMiscTab.Controls.Add(this.showStaticWallLabels);
            this.subOptionsMiscTab.Controls.Add(this.stealthOverhead);
            this.subOptionsMiscTab.Controls.Add(this.forceSizeX);
            this.subOptionsMiscTab.Controls.Add(this.forceSizeY);
            this.subOptionsMiscTab.Controls.Add(this.blockHealPoison);
            this.subOptionsMiscTab.Controls.Add(this.potionEquip);
            this.subOptionsMiscTab.Controls.Add(this.spellUnequip);
            this.subOptionsMiscTab.Controls.Add(this.autoOpenDoors);
            this.subOptionsMiscTab.Controls.Add(this.chkStealth);
            this.subOptionsMiscTab.Controls.Add(this.label18);
            this.subOptionsMiscTab.Controls.Add(this.gameSize);
            this.subOptionsMiscTab.Controls.Add(this.setMinLightLevel);
            this.subOptionsMiscTab.Controls.Add(this.setMaxLightLevel);
            this.subOptionsMiscTab.Controls.Add(this.seasonList);
            this.subOptionsMiscTab.Controls.Add(this.lblSeason);
            this.subOptionsMiscTab.Controls.Add(this.lightLevel);
            this.subOptionsMiscTab.Controls.Add(this.lightLevelBar);
            this.subOptionsMiscTab.Controls.Add(this.minMaxLightLevel);
            this.subOptionsMiscTab.Controls.Add(this.blockPartyInvites);
            this.subOptionsMiscTab.Controls.Add(this.blockTradeRequests);
            this.subOptionsMiscTab.Controls.Add(this.blockOpenCorpsesTwice);
            this.subOptionsMiscTab.Controls.Add(this.preAOSstatbar);
            this.subOptionsMiscTab.Controls.Add(this.corpseRange);
            this.subOptionsMiscTab.Controls.Add(this.autoStackRes);
            this.subOptionsMiscTab.Controls.Add(this.label4);
            this.subOptionsMiscTab.Controls.Add(this.openCorpses);
            this.subOptionsMiscTab.Controls.Add(this.blockDis);
            this.subOptionsMiscTab.Location = new System.Drawing.Point(10, 58);
            this.subOptionsMiscTab.Name = "subOptionsMiscTab";
            this.subOptionsMiscTab.Size = new System.Drawing.Size(1225, 306);
            this.subOptionsMiscTab.TabIndex = 2;
            this.subOptionsMiscTab.Text = "Additional Options  ";
            // 
            // buyAgentIgnoreGold
            // 
            this.buyAgentIgnoreGold.AutoSize = true;
            this.buyAgentIgnoreGold.Location = new System.Drawing.Point(650, 610);
            this.buyAgentIgnoreGold.Name = "buyAgentIgnoreGold";
            this.buyAgentIgnoreGold.Size = new System.Drawing.Size(458, 45);
            this.buyAgentIgnoreGold.TabIndex = 126;
            this.buyAgentIgnoreGold.Text = "Buy Agents ignore player gold";
            this.buyAgentIgnoreGold.UseVisualStyleBackColor = true;
            this.buyAgentIgnoreGold.CheckedChanged += new System.EventHandler(this.buyAgentIgnoreGold_CheckedChanged);
            // 
            // reequipHandsPotion
            // 
            this.reequipHandsPotion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reequipHandsPotion.Location = new System.Drawing.Point(1058, 350);
            this.reequipHandsPotion.Name = "reequipHandsPotion";
            this.reequipHandsPotion.Size = new System.Drawing.Size(172, 50);
            this.reequipHandsPotion.TabIndex = 125;
            this.reequipHandsPotion.Text = "Re-equip";
            this.reequipHandsPotion.CheckedChanged += new System.EventHandler(this.reequipHandsPotion_CheckedChanged);
            // 
            // autoOpenDoorWhenHidden
            // 
            this.autoOpenDoorWhenHidden.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoOpenDoorWhenHidden.Location = new System.Drawing.Point(982, 220);
            this.autoOpenDoorWhenHidden.Name = "autoOpenDoorWhenHidden";
            this.autoOpenDoorWhenHidden.Size = new System.Drawing.Size(238, 50);
            this.autoOpenDoorWhenHidden.TabIndex = 124;
            this.autoOpenDoorWhenHidden.Text = "When hidden";
            this.autoOpenDoorWhenHidden.CheckedChanged += new System.EventHandler(this.autoDoorWhenHidden_CheckedChanged);
            // 
            // lblStealthFormat
            // 
            this.lblStealthFormat.AutoSize = true;
            this.lblStealthFormat.Location = new System.Drawing.Point(700, 155);
            this.lblStealthFormat.Name = "lblStealthFormat";
            this.lblStealthFormat.Size = new System.Drawing.Size(119, 41);
            this.lblStealthFormat.TabIndex = 123;
            this.lblStealthFormat.Text = "Format:";
            // 
            // stealthStepsFormat
            // 
            this.stealthStepsFormat.Location = new System.Drawing.Point(835, 148);
            this.stealthStepsFormat.Name = "stealthStepsFormat";
            this.stealthStepsFormat.Size = new System.Drawing.Size(285, 47);
            this.stealthStepsFormat.TabIndex = 122;
            this.stealthStepsFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.stealthStepsFormat.TextChanged += new System.EventHandler(this.stealthStepsFormat_TextChanged);
            // 
            // rememberPwds
            // 
            this.rememberPwds.Location = new System.Drawing.Point(650, 30);
            this.rememberPwds.Name = "rememberPwds";
            this.rememberPwds.Size = new System.Drawing.Size(370, 50);
            this.rememberPwds.TabIndex = 121;
            this.rememberPwds.Text = "Remember passwords ";
            this.rememberPwds.CheckedChanged += new System.EventHandler(this.rememberPwds_CheckedChanged);
            // 
            // showStaticWalls
            // 
            this.showStaticWalls.AutoSize = true;
            this.showStaticWalls.Location = new System.Drawing.Point(650, 548);
            this.showStaticWalls.Name = "showStaticWalls";
            this.showStaticWalls.Size = new System.Drawing.Size(369, 45);
            this.showStaticWalls.TabIndex = 119;
            this.showStaticWalls.Text = "Static magic fields/walls";
            this.showStaticWalls.UseVisualStyleBackColor = true;
            this.showStaticWalls.CheckedChanged += new System.EventHandler(this.showStaticWalls_CheckedChanged);
            // 
            // showStaticWallLabels
            // 
            this.showStaticWallLabels.AutoSize = true;
            this.showStaticWallLabels.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showStaticWallLabels.Location = new System.Drawing.Point(1052, 550);
            this.showStaticWallLabels.Name = "showStaticWallLabels";
            this.showStaticWallLabels.Size = new System.Drawing.Size(130, 42);
            this.showStaticWallLabels.TabIndex = 120;
            this.showStaticWallLabels.Text = "Labels";
            this.showStaticWallLabels.UseVisualStyleBackColor = true;
            this.showStaticWallLabels.CheckedChanged += new System.EventHandler(this.showStaticWallLabels_CheckedChanged);
            // 
            // stealthOverhead
            // 
            this.stealthOverhead.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stealthOverhead.Location = new System.Drawing.Point(982, 92);
            this.stealthOverhead.Name = "stealthOverhead";
            this.stealthOverhead.Size = new System.Drawing.Size(248, 50);
            this.stealthOverhead.TabIndex = 117;
            this.stealthOverhead.Text = "Overhead";
            this.stealthOverhead.UseVisualStyleBackColor = true;
            this.stealthOverhead.CheckedChanged += new System.EventHandler(this.stealthOverhead_CheckedChanged);
            // 
            // forceSizeX
            // 
            this.forceSizeX.Location = new System.Drawing.Point(968, 475);
            this.forceSizeX.Name = "forceSizeX";
            this.forceSizeX.Size = new System.Drawing.Size(84, 47);
            this.forceSizeX.TabIndex = 111;
            this.forceSizeX.TextChanged += new System.EventHandler(this.forceSizeX_TextChanged);
            // 
            // forceSizeY
            // 
            this.forceSizeY.Location = new System.Drawing.Point(1108, 478);
            this.forceSizeY.Name = "forceSizeY";
            this.forceSizeY.Size = new System.Drawing.Size(82, 47);
            this.forceSizeY.TabIndex = 112;
            this.forceSizeY.TextChanged += new System.EventHandler(this.forceSizeY_TextChanged);
            // 
            // blockHealPoison
            // 
            this.blockHealPoison.Location = new System.Drawing.Point(650, 415);
            this.blockHealPoison.Name = "blockHealPoison";
            this.blockHealPoison.Size = new System.Drawing.Size(502, 50);
            this.blockHealPoison.TabIndex = 116;
            this.blockHealPoison.Text = "Block heal if target is poisoned";
            this.blockHealPoison.CheckedChanged += new System.EventHandler(this.blockHealPoison_CheckedChanged);
            // 
            // potionEquip
            // 
            this.potionEquip.Location = new System.Drawing.Point(650, 350);
            this.potionEquip.Name = "potionEquip";
            this.potionEquip.Size = new System.Drawing.Size(580, 50);
            this.potionEquip.TabIndex = 115;
            this.potionEquip.Text = "Auto Unequip for potions";
            this.potionEquip.CheckedChanged += new System.EventHandler(this.potionEquip_CheckedChanged);
            // 
            // spellUnequip
            // 
            this.spellUnequip.Location = new System.Drawing.Point(650, 285);
            this.spellUnequip.Name = "spellUnequip";
            this.spellUnequip.Size = new System.Drawing.Size(532, 50);
            this.spellUnequip.TabIndex = 108;
            this.spellUnequip.Text = "Auto Unequip hands before casting";
            this.spellUnequip.CheckedChanged += new System.EventHandler(this.spellUnequip_CheckedChanged);
            // 
            // autoOpenDoors
            // 
            this.autoOpenDoors.Location = new System.Drawing.Point(650, 220);
            this.autoOpenDoors.Name = "autoOpenDoors";
            this.autoOpenDoors.Size = new System.Drawing.Size(295, 50);
            this.autoOpenDoors.TabIndex = 110;
            this.autoOpenDoors.Text = "Auto-open doors";
            this.autoOpenDoors.CheckedChanged += new System.EventHandler(this.autoOpenDoors_CheckedChanged);
            // 
            // chkStealth
            // 
            this.chkStealth.Location = new System.Drawing.Point(650, 92);
            this.chkStealth.Name = "chkStealth";
            this.chkStealth.Size = new System.Drawing.Size(475, 50);
            this.chkStealth.TabIndex = 107;
            this.chkStealth.Text = "Count stealth steps";
            this.chkStealth.CheckedChanged += new System.EventHandler(this.chkStealth_CheckedChanged);
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(1068, 482);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(24, 46);
            this.label18.TabIndex = 114;
            this.label18.Text = "x";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gameSize
            // 
            this.gameSize.Location = new System.Drawing.Point(650, 480);
            this.gameSize.Name = "gameSize";
            this.gameSize.Size = new System.Drawing.Size(295, 45);
            this.gameSize.TabIndex = 113;
            this.gameSize.Text = "Force Game Size:";
            this.gameSize.CheckedChanged += new System.EventHandler(this.gameSize_CheckedChanged);
            // 
            // setMinLightLevel
            // 
            this.setMinLightLevel.Location = new System.Drawing.Point(292, 618);
            this.setMinLightLevel.Name = "setMinLightLevel";
            this.setMinLightLevel.Size = new System.Drawing.Size(146, 62);
            this.setMinLightLevel.TabIndex = 105;
            this.setMinLightLevel.Text = "Set Min";
            this.setMinLightLevel.Click += new System.EventHandler(this.setMinLightLevel_Click);
            // 
            // setMaxLightLevel
            // 
            this.setMaxLightLevel.Location = new System.Drawing.Point(452, 618);
            this.setMaxLightLevel.Name = "setMaxLightLevel";
            this.setMaxLightLevel.Size = new System.Drawing.Size(146, 62);
            this.setMaxLightLevel.TabIndex = 104;
            this.setMaxLightLevel.Text = "Set Max";
            this.setMaxLightLevel.Click += new System.EventHandler(this.setMaxLightLevel_Click);
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
            this.seasonList.Location = new System.Drawing.Point(148, 488);
            this.seasonList.Name = "seasonList";
            this.seasonList.Size = new System.Drawing.Size(277, 49);
            this.seasonList.TabIndex = 102;
            this.seasonList.SelectedIndexChanged += new System.EventHandler(this.seasonList_SelectedIndexChanged);
            // 
            // lblSeason
            // 
            this.lblSeason.AutoSize = true;
            this.lblSeason.Location = new System.Drawing.Point(15, 495);
            this.lblSeason.Name = "lblSeason";
            this.lblSeason.Size = new System.Drawing.Size(120, 41);
            this.lblSeason.TabIndex = 101;
            this.lblSeason.Text = "Season:";
            // 
            // lightLevel
            // 
            this.lightLevel.AutoSize = true;
            this.lightLevel.Location = new System.Drawing.Point(15, 558);
            this.lightLevel.Name = "lightLevel";
            this.lightLevel.Size = new System.Drawing.Size(166, 41);
            this.lightLevel.TabIndex = 100;
            this.lightLevel.Text = "Light Level:";
            // 
            // lightLevelBar
            // 
            this.lightLevelBar.AutoSize = false;
            this.lightLevelBar.Location = new System.Drawing.Point(198, 558);
            this.lightLevelBar.Maximum = 31;
            this.lightLevelBar.Name = "lightLevelBar";
            this.lightLevelBar.Size = new System.Drawing.Size(402, 52);
            this.lightLevelBar.TabIndex = 99;
            this.lightLevelBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.lightLevelBar.Value = 15;
            this.lightLevelBar.Scroll += new System.EventHandler(this.lightLevelBar_Scroll);
            // 
            // minMaxLightLevel
            // 
            this.minMaxLightLevel.Location = new System.Drawing.Point(22, 625);
            this.minMaxLightLevel.Name = "minMaxLightLevel";
            this.minMaxLightLevel.Size = new System.Drawing.Size(286, 50);
            this.minMaxLightLevel.TabIndex = 106;
            this.minMaxLightLevel.Text = "Enable Min/Max";
            this.minMaxLightLevel.CheckedChanged += new System.EventHandler(this.minMaxLightLevel_CheckedChanged);
            // 
            // blockPartyInvites
            // 
            this.blockPartyInvites.Location = new System.Drawing.Point(22, 422);
            this.blockPartyInvites.Name = "blockPartyInvites";
            this.blockPartyInvites.Size = new System.Drawing.Size(460, 50);
            this.blockPartyInvites.TabIndex = 98;
            this.blockPartyInvites.Text = "Block party invites";
            this.blockPartyInvites.CheckedChanged += new System.EventHandler(this.blockPartyInvites_CheckedChanged);
            // 
            // blockTradeRequests
            // 
            this.blockTradeRequests.Location = new System.Drawing.Point(22, 358);
            this.blockTradeRequests.Name = "blockTradeRequests";
            this.blockTradeRequests.Size = new System.Drawing.Size(460, 50);
            this.blockTradeRequests.TabIndex = 97;
            this.blockTradeRequests.Text = "Block trade requests";
            this.blockTradeRequests.CheckedChanged += new System.EventHandler(this.blockTradeRequests_CheckedChanged);
            // 
            // blockOpenCorpsesTwice
            // 
            this.blockOpenCorpsesTwice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blockOpenCorpsesTwice.Location = new System.Drawing.Point(22, 228);
            this.blockOpenCorpsesTwice.Name = "blockOpenCorpsesTwice";
            this.blockOpenCorpsesTwice.Size = new System.Drawing.Size(523, 50);
            this.blockOpenCorpsesTwice.TabIndex = 96;
            this.blockOpenCorpsesTwice.Text = "Block opening corpses twice";
            this.blockOpenCorpsesTwice.CheckedChanged += new System.EventHandler(this.blockOpenCorpsesTwice_CheckedChanged);
            // 
            // preAOSstatbar
            // 
            this.preAOSstatbar.Location = new System.Drawing.Point(22, 30);
            this.preAOSstatbar.Name = "preAOSstatbar";
            this.preAOSstatbar.Size = new System.Drawing.Size(476, 50);
            this.preAOSstatbar.TabIndex = 95;
            this.preAOSstatbar.Text = "Use Pre-AOS status window";
            this.preAOSstatbar.CheckedChanged += new System.EventHandler(this.preAOSstatbar_CheckedChanged);
            // 
            // corpseRange
            // 
            this.corpseRange.Location = new System.Drawing.Point(422, 158);
            this.corpseRange.Name = "corpseRange";
            this.corpseRange.Size = new System.Drawing.Size(60, 47);
            this.corpseRange.TabIndex = 91;
            this.corpseRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.corpseRange.TextChanged += new System.EventHandler(this.corpseRange_TextChanged);
            // 
            // autoStackRes
            // 
            this.autoStackRes.Location = new System.Drawing.Point(22, 92);
            this.autoStackRes.Name = "autoStackRes";
            this.autoStackRes.Size = new System.Drawing.Size(570, 50);
            this.autoStackRes.TabIndex = 93;
            this.autoStackRes.Text = "Auto-Stack Ore/Fish/Logs at Feet";
            this.autoStackRes.CheckedChanged += new System.EventHandler(this.autoStackRes_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(502, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 40);
            this.label4.TabIndex = 92;
            this.label4.Text = "tiles";
            // 
            // openCorpses
            // 
            this.openCorpses.Location = new System.Drawing.Point(22, 162);
            this.openCorpses.Name = "openCorpses";
            this.openCorpses.Size = new System.Drawing.Size(413, 50);
            this.openCorpses.TabIndex = 90;
            this.openCorpses.Text = "Open new corpses within";
            this.openCorpses.CheckedChanged += new System.EventHandler(this.openCorpses_CheckedChanged);
            // 
            // blockDis
            // 
            this.blockDis.Location = new System.Drawing.Point(22, 292);
            this.blockDis.Name = "blockDis";
            this.blockDis.Size = new System.Drawing.Size(460, 50);
            this.blockDis.TabIndex = 94;
            this.blockDis.Text = "Block dismount in war mode";
            this.blockDis.CheckedChanged += new System.EventHandler(this.blockDis_CheckedChanged);
            // 
            // displayTab
            // 
            this.displayTab.Controls.Add(this.displayCountersTabCtrl);
            this.displayTab.Location = new System.Drawing.Point(10, 106);
            this.displayTab.Name = "displayTab";
            this.displayTab.Size = new System.Drawing.Size(1305, 819);
            this.displayTab.TabIndex = 1;
            this.displayTab.Text = "Display/Counters";
            // 
            // displayCountersTabCtrl
            // 
            this.displayCountersTabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.displayCountersTabCtrl.Controls.Add(this.subDisplayTab);
            this.displayCountersTabCtrl.Controls.Add(this.subCountersTab);
            this.displayCountersTabCtrl.Controls.Add(this.subBandageTimerTab);
            this.displayCountersTabCtrl.Controls.Add(this.subOverheadTab);
            this.displayCountersTabCtrl.Controls.Add(this.subWaypoints);
            this.displayCountersTabCtrl.Controls.Add(this.subBuffsDebuffs);
            this.displayCountersTabCtrl.Location = new System.Drawing.Point(15, 8);
            this.displayCountersTabCtrl.Name = "displayCountersTabCtrl";
            this.displayCountersTabCtrl.SelectedIndex = 0;
            this.displayCountersTabCtrl.Size = new System.Drawing.Size(1253, 480);
            this.displayCountersTabCtrl.TabIndex = 51;
            this.displayCountersTabCtrl.SelectedIndexChanged += new System.EventHandler(this.displayCountersTabCtrl_SelectedIndexChanged);
            // 
            // subDisplayTab
            // 
            this.subDisplayTab.BackColor = System.Drawing.SystemColors.Control;
            this.subDisplayTab.Controls.Add(this.groupBox11);
            this.subDisplayTab.Controls.Add(this.trackDps);
            this.subDisplayTab.Controls.Add(this.trackIncomingGold);
            this.subDisplayTab.Controls.Add(this.showNotoHue);
            this.subDisplayTab.Controls.Add(this.highlightSpellReags);
            this.subDisplayTab.Controls.Add(this.groupBox3);
            this.subDisplayTab.Location = new System.Drawing.Point(10, 58);
            this.subDisplayTab.Name = "subDisplayTab";
            this.subDisplayTab.Padding = new System.Windows.Forms.Padding(3);
            this.subDisplayTab.Size = new System.Drawing.Size(1233, 412);
            this.subDisplayTab.TabIndex = 0;
            this.subDisplayTab.Text = "Display";
            // 
            // groupBox11
            // 
            this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox11.Controls.Add(this.razorTitleBarKey);
            this.groupBox11.Controls.Add(this.showInRazorTitleBar);
            this.groupBox11.Controls.Add(this.razorTitleBar);
            this.groupBox11.Location = new System.Drawing.Point(15, 34);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(1173, 177);
            this.groupBox11.TabIndex = 51;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Razor Title Bar";
            // 
            // razorTitleBarKey
            // 
            this.razorTitleBarKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.razorTitleBarKey.Location = new System.Drawing.Point(1118, 42);
            this.razorTitleBarKey.Name = "razorTitleBarKey";
            this.razorTitleBarKey.Size = new System.Drawing.Size(40, 50);
            this.razorTitleBarKey.TabIndex = 2;
            this.razorTitleBarKey.Text = "?";
            this.razorTitleBarKey.UseVisualStyleBackColor = true;
            this.razorTitleBarKey.Click += new System.EventHandler(this.razorTitleBarKey_Click);
            // 
            // showInRazorTitleBar
            // 
            this.showInRazorTitleBar.AutoSize = true;
            this.showInRazorTitleBar.Location = new System.Drawing.Point(15, 42);
            this.showInRazorTitleBar.Name = "showInRazorTitleBar";
            this.showInRazorTitleBar.Size = new System.Drawing.Size(360, 45);
            this.showInRazorTitleBar.TabIndex = 1;
            this.showInRazorTitleBar.Text = "Show in Razor title bar:";
            this.showInRazorTitleBar.UseVisualStyleBackColor = true;
            this.showInRazorTitleBar.CheckedChanged += new System.EventHandler(this.showInRazorTitleBar_CheckedChanged);
            // 
            // razorTitleBar
            // 
            this.razorTitleBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.razorTitleBar.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.razorTitleBar.Location = new System.Drawing.Point(15, 105);
            this.razorTitleBar.Name = "razorTitleBar";
            this.razorTitleBar.Size = new System.Drawing.Size(1143, 43);
            this.razorTitleBar.TabIndex = 0;
            this.razorTitleBar.Text = "{name} ({profile} - {shard}) - Razor v{version}";
            this.razorTitleBar.TextChanged += new System.EventHandler(this.razorTitleBar_TextChanged);
            // 
            // trackDps
            // 
            this.trackDps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackDps.AutoSize = true;
            this.trackDps.Location = new System.Drawing.Point(682, -37);
            this.trackDps.Name = "trackDps";
            this.trackDps.Size = new System.Drawing.Size(358, 45);
            this.trackDps.TabIndex = 53;
            this.trackDps.Text = "Enable damage tracker";
            this.trackDps.UseVisualStyleBackColor = true;
            this.trackDps.CheckedChanged += new System.EventHandler(this.trackDps_CheckedChanged);
            // 
            // trackIncomingGold
            // 
            this.trackIncomingGold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackIncomingGold.AutoSize = true;
            this.trackIncomingGold.Location = new System.Drawing.Point(682, -101);
            this.trackIncomingGold.Name = "trackIncomingGold";
            this.trackIncomingGold.Size = new System.Drawing.Size(550, 45);
            this.trackIncomingGold.TabIndex = 52;
            this.trackIncomingGold.Text = "Enable gold per sec/min/hour tracker";
            this.trackIncomingGold.UseVisualStyleBackColor = true;
            this.trackIncomingGold.CheckedChanged += new System.EventHandler(this.trackIncomingGold_CheckedChanged);
            // 
            // showNotoHue
            // 
            this.showNotoHue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showNotoHue.Location = new System.Drawing.Point(15, -42);
            this.showNotoHue.Name = "showNotoHue";
            this.showNotoHue.Size = new System.Drawing.Size(553, 50);
            this.showNotoHue.TabIndex = 51;
            this.showNotoHue.Text = "Show noto hue on {char} in TitleBar";
            this.showNotoHue.CheckedChanged += new System.EventHandler(this.showNotoHue_CheckedChanged);
            // 
            // highlightSpellReags
            // 
            this.highlightSpellReags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.highlightSpellReags.Location = new System.Drawing.Point(15, -106);
            this.highlightSpellReags.Name = "highlightSpellReags";
            this.highlightSpellReags.Size = new System.Drawing.Size(513, 50);
            this.highlightSpellReags.TabIndex = 50;
            this.highlightSpellReags.Text = "Highlight Spell Reagents on Cast";
            this.highlightSpellReags.CheckedChanged += new System.EventHandler(this.highlightSpellReags_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.titleBarParams);
            this.groupBox3.Controls.Add(this.titleStr);
            this.groupBox3.Controls.Add(this.showInBar);
            this.groupBox3.Location = new System.Drawing.Point(15, 15);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1173, 106);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Title Bar Display";
            // 
            // titleBarParams
            // 
            this.titleBarParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.titleBarParams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.titleBarParams.FormattingEnabled = true;
            this.titleBarParams.Items.AddRange(new object[] {
            "{ar}",
            "{bandage}",
            "{char}",
            "{crimtime}",
            "{buffsdebuffs}",
            "{damage}",
            "-",
            "{dps}",
            "{maxdps}",
            "{maxdamagedealt}",
            "{maxdamagetaken}",
            "{totaldamagedealt}",
            "{totaldamagetaken}",
            "-",
            "{dex}",
            "{followersmax}",
            "{followers}",
            "-",
            "{gold}",
            "{goldtotal}",
            "{goldtotalmin}",
            "{gpm}",
            "{gps}",
            "{gph}",
            "-",
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
            this.titleBarParams.Location = new System.Drawing.Point(898, 48);
            this.titleBarParams.Name = "titleBarParams";
            this.titleBarParams.Size = new System.Drawing.Size(260, 49);
            this.titleBarParams.TabIndex = 5;
            this.titleBarParams.SelectedIndexChanged += new System.EventHandler(this.titleBarParams_SelectedIndexChanged);
            // 
            // titleStr
            // 
            this.titleStr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleStr.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleStr.Location = new System.Drawing.Point(15, 120);
            this.titleStr.Multiline = true;
            this.titleStr.Name = "titleStr";
            this.titleStr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.titleStr.Size = new System.Drawing.Size(1143, 60);
            this.titleStr.TabIndex = 4;
            this.titleStr.TextChanged += new System.EventHandler(this.titleStr_TextChanged);
            // 
            // showInBar
            // 
            this.showInBar.Location = new System.Drawing.Point(28, 55);
            this.showInBar.Name = "showInBar";
            this.showInBar.Size = new System.Drawing.Size(352, 50);
            this.showInBar.TabIndex = 3;
            this.showInBar.Text = "Show in UO title bar:";
            this.showInBar.CheckedChanged += new System.EventHandler(this.showInBar_CheckedChanged);
            // 
            // subCountersTab
            // 
            this.subCountersTab.BackColor = System.Drawing.SystemColors.Control;
            this.subCountersTab.Controls.Add(this.warnNum);
            this.subCountersTab.Controls.Add(this.warnCount);
            this.subCountersTab.Controls.Add(this.excludePouches);
            this.subCountersTab.Controls.Add(this.titlebarImages);
            this.subCountersTab.Controls.Add(this.checkNewConts);
            this.subCountersTab.Controls.Add(this.groupBox2);
            this.subCountersTab.Location = new System.Drawing.Point(10, 58);
            this.subCountersTab.Name = "subCountersTab";
            this.subCountersTab.Padding = new System.Windows.Forms.Padding(3);
            this.subCountersTab.Size = new System.Drawing.Size(1225, 306);
            this.subCountersTab.TabIndex = 1;
            this.subCountersTab.Text = "Counters";
            // 
            // warnNum
            // 
            this.warnNum.Location = new System.Drawing.Point(1075, 248);
            this.warnNum.Name = "warnNum";
            this.warnNum.Size = new System.Drawing.Size(75, 47);
            this.warnNum.TabIndex = 51;
            this.warnNum.Text = "3";
            this.warnNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.warnNum.TextChanged += new System.EventHandler(this.warnNum_TextChanged);
            // 
            // warnCount
            // 
            this.warnCount.Location = new System.Drawing.Point(748, 252);
            this.warnCount.Name = "warnCount";
            this.warnCount.Size = new System.Drawing.Size(480, 48);
            this.warnCount.TabIndex = 50;
            this.warnCount.Text = "Warn when below:";
            this.warnCount.CheckedChanged += new System.EventHandler(this.warnCount_CheckedChanged);
            // 
            // excludePouches
            // 
            this.excludePouches.Location = new System.Drawing.Point(748, 120);
            this.excludePouches.Name = "excludePouches";
            this.excludePouches.Size = new System.Drawing.Size(480, 52);
            this.excludePouches.TabIndex = 49;
            this.excludePouches.Text = "Never auto-search pouches";
            this.excludePouches.CheckedChanged += new System.EventHandler(this.excludePouches_CheckedChanged);
            // 
            // titlebarImages
            // 
            this.titlebarImages.Location = new System.Drawing.Point(748, 188);
            this.titlebarImages.Name = "titlebarImages";
            this.titlebarImages.Size = new System.Drawing.Size(450, 50);
            this.titlebarImages.TabIndex = 48;
            this.titlebarImages.Text = "Show Images with Counters";
            this.titlebarImages.CheckedChanged += new System.EventHandler(this.titlebarImages_CheckedChanged);
            // 
            // checkNewConts
            // 
            this.checkNewConts.Location = new System.Drawing.Point(748, 60);
            this.checkNewConts.Name = "checkNewConts";
            this.checkNewConts.Size = new System.Drawing.Size(500, 50);
            this.checkNewConts.TabIndex = 47;
            this.checkNewConts.Text = "Auto search new containers";
            this.checkNewConts.CheckedChanged += new System.EventHandler(this.checkNewConts_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.counters);
            this.groupBox2.Controls.Add(this.delCounter);
            this.groupBox2.Controls.Add(this.addCounter);
            this.groupBox2.Controls.Add(this.recount);
            this.groupBox2.Location = new System.Drawing.Point(15, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(710, 555);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Counters";
            // 
            // counters
            // 
            this.counters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.counters.CheckBoxes = true;
            this.counters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cntName,
            this.cntCount});
            this.counters.GridLines = true;
            this.counters.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.counters.HideSelection = false;
            this.counters.LabelWrap = false;
            this.counters.Location = new System.Drawing.Point(15, 45);
            this.counters.MultiSelect = false;
            this.counters.Name = "counters";
            this.counters.Size = new System.Drawing.Size(680, 387);
            this.counters.TabIndex = 11;
            this.counters.UseCompatibleStateImageBehavior = false;
            this.counters.View = System.Windows.Forms.View.Details;
            this.counters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.counters_ItemCheck);
            // 
            // cntName
            // 
            this.cntName.Text = "Name (Format)";
            this.cntName.Width = 216;
            // 
            // cntCount
            // 
            this.cntCount.Text = "Count";
            this.cntCount.Width = 51;
            // 
            // delCounter
            // 
            this.delCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.delCounter.Location = new System.Drawing.Point(270, 448);
            this.delCounter.Name = "delCounter";
            this.delCounter.Size = new System.Drawing.Size(178, 92);
            this.delCounter.TabIndex = 4;
            this.delCounter.Text = "Del/Edit";
            this.delCounter.Click += new System.EventHandler(this.delCounter_Click);
            // 
            // addCounter
            // 
            this.addCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addCounter.Location = new System.Drawing.Point(15, 448);
            this.addCounter.Name = "addCounter";
            this.addCounter.Size = new System.Drawing.Size(175, 92);
            this.addCounter.TabIndex = 3;
            this.addCounter.Text = "Add...";
            this.addCounter.Click += new System.EventHandler(this.addCounter_Click);
            // 
            // recount
            // 
            this.recount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.recount.Location = new System.Drawing.Point(520, 448);
            this.recount.Name = "recount";
            this.recount.Size = new System.Drawing.Size(175, 92);
            this.recount.TabIndex = 2;
            this.recount.Text = "Recount";
            this.recount.Click += new System.EventHandler(this.recount_Click);
            // 
            // subBandageTimerTab
            // 
            this.subBandageTimerTab.BackColor = System.Drawing.SystemColors.Control;
            this.subBandageTimerTab.Controls.Add(this.bandageEndMessage);
            this.subBandageTimerTab.Controls.Add(this.bandageStartMessage);
            this.subBandageTimerTab.Controls.Add(this.showBandageEnd);
            this.subBandageTimerTab.Controls.Add(this.showBandageStart);
            this.subBandageTimerTab.Controls.Add(this.setBandageHue);
            this.subBandageTimerTab.Controls.Add(this.bandageTimerLocation);
            this.subBandageTimerTab.Controls.Add(this.bandageTimerSeconds);
            this.subBandageTimerTab.Controls.Add(this.onlyShowBandageTimerSeconds);
            this.subBandageTimerTab.Controls.Add(this.bandageTimerFormat);
            this.subBandageTimerTab.Controls.Add(this.showBandageTimer);
            this.subBandageTimerTab.Controls.Add(this.lblBandageCountFormat);
            this.subBandageTimerTab.Location = new System.Drawing.Point(10, 58);
            this.subBandageTimerTab.Name = "subBandageTimerTab";
            this.subBandageTimerTab.Size = new System.Drawing.Size(1225, 306);
            this.subBandageTimerTab.TabIndex = 2;
            this.subBandageTimerTab.Text = "Bandage Timer";
            // 
            // bandageEndMessage
            // 
            this.bandageEndMessage.Location = new System.Drawing.Point(512, 308);
            this.bandageEndMessage.Name = "bandageEndMessage";
            this.bandageEndMessage.Size = new System.Drawing.Size(366, 47);
            this.bandageEndMessage.TabIndex = 62;
            this.bandageEndMessage.Text = "Bandage: Ending";
            this.bandageEndMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bandageEndMessage.TextChanged += new System.EventHandler(this.BandageEndMessage_TextChanged);
            // 
            // bandageStartMessage
            // 
            this.bandageStartMessage.Location = new System.Drawing.Point(512, 245);
            this.bandageStartMessage.Name = "bandageStartMessage";
            this.bandageStartMessage.Size = new System.Drawing.Size(366, 47);
            this.bandageStartMessage.TabIndex = 61;
            this.bandageStartMessage.Text = "Bandage: Starting";
            this.bandageStartMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bandageStartMessage.TextChanged += new System.EventHandler(this.BandageStartMessage_TextChanged);
            // 
            // showBandageEnd
            // 
            this.showBandageEnd.AutoSize = true;
            this.showBandageEnd.Location = new System.Drawing.Point(22, 312);
            this.showBandageEnd.Name = "showBandageEnd";
            this.showBandageEnd.Size = new System.Drawing.Size(464, 45);
            this.showBandageEnd.TabIndex = 60;
            this.showBandageEnd.Text = "Show bandaging end message";
            this.showBandageEnd.UseVisualStyleBackColor = true;
            this.showBandageEnd.CheckedChanged += new System.EventHandler(this.ShowBandageEnd_CheckedChanged);
            // 
            // showBandageStart
            // 
            this.showBandageStart.AutoSize = true;
            this.showBandageStart.Location = new System.Drawing.Point(22, 250);
            this.showBandageStart.Name = "showBandageStart";
            this.showBandageStart.Size = new System.Drawing.Size(471, 45);
            this.showBandageStart.TabIndex = 58;
            this.showBandageStart.Text = "Show bandaging start message";
            this.showBandageStart.UseVisualStyleBackColor = true;
            this.showBandageStart.CheckedChanged += new System.EventHandler(this.ShowBandageStart_CheckedChanged);
            // 
            // setBandageHue
            // 
            this.setBandageHue.Location = new System.Drawing.Point(672, 102);
            this.setBandageHue.Name = "setBandageHue";
            this.setBandageHue.Size = new System.Drawing.Size(168, 58);
            this.setBandageHue.TabIndex = 57;
            this.setBandageHue.Text = "Set Hue";
            this.setBandageHue.UseVisualStyleBackColor = true;
            this.setBandageHue.Click += new System.EventHandler(this.setBandageHue_Click);
            // 
            // bandageTimerLocation
            // 
            this.bandageTimerLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bandageTimerLocation.FormattingEnabled = true;
            this.bandageTimerLocation.Items.AddRange(new object[] {
            "Overhead",
            "System Messages"});
            this.bandageTimerLocation.Location = new System.Drawing.Point(362, 25);
            this.bandageTimerLocation.Name = "bandageTimerLocation";
            this.bandageTimerLocation.Size = new System.Drawing.Size(296, 49);
            this.bandageTimerLocation.TabIndex = 56;
            this.bandageTimerLocation.SelectedIndexChanged += new System.EventHandler(this.bandageTimerLocation_SelectedIndexChanged);
            // 
            // bandageTimerSeconds
            // 
            this.bandageTimerSeconds.Location = new System.Drawing.Point(450, 178);
            this.bandageTimerSeconds.Name = "bandageTimerSeconds";
            this.bandageTimerSeconds.Size = new System.Drawing.Size(120, 47);
            this.bandageTimerSeconds.TabIndex = 55;
            this.bandageTimerSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bandageTimerSeconds.TextChanged += new System.EventHandler(this.bandageTimerSeconds_TextChanged);
            // 
            // onlyShowBandageTimerSeconds
            // 
            this.onlyShowBandageTimerSeconds.Location = new System.Drawing.Point(22, 180);
            this.onlyShowBandageTimerSeconds.Name = "onlyShowBandageTimerSeconds";
            this.onlyShowBandageTimerSeconds.Size = new System.Drawing.Size(513, 50);
            this.onlyShowBandageTimerSeconds.TabIndex = 53;
            this.onlyShowBandageTimerSeconds.Text = "Only show every X seconds:";
            this.onlyShowBandageTimerSeconds.CheckedChanged += new System.EventHandler(this.onlyShowBandageTimerSeconds_CheckedChanged);
            // 
            // bandageTimerFormat
            // 
            this.bandageTimerFormat.Location = new System.Drawing.Point(282, 102);
            this.bandageTimerFormat.Name = "bandageTimerFormat";
            this.bandageTimerFormat.Size = new System.Drawing.Size(376, 47);
            this.bandageTimerFormat.TabIndex = 52;
            this.bandageTimerFormat.Text = "Bandage: {count}s";
            this.bandageTimerFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bandageTimerFormat.TextChanged += new System.EventHandler(this.bandageTimerFormat_TextChanged);
            // 
            // showBandageTimer
            // 
            this.showBandageTimer.Location = new System.Drawing.Point(22, 28);
            this.showBandageTimer.Name = "showBandageTimer";
            this.showBandageTimer.Size = new System.Drawing.Size(513, 50);
            this.showBandageTimer.TabIndex = 51;
            this.showBandageTimer.Text = "Show bandage timer";
            this.showBandageTimer.CheckedChanged += new System.EventHandler(this.showBandageTimer_CheckedChanged);
            // 
            // lblBandageCountFormat
            // 
            this.lblBandageCountFormat.Location = new System.Drawing.Point(52, 102);
            this.lblBandageCountFormat.Name = "lblBandageCountFormat";
            this.lblBandageCountFormat.Size = new System.Drawing.Size(398, 58);
            this.lblBandageCountFormat.TabIndex = 54;
            this.lblBandageCountFormat.Text = "Format && Hue:";
            this.lblBandageCountFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // subOverheadTab
            // 
            this.subOverheadTab.BackColor = System.Drawing.SystemColors.Control;
            this.subOverheadTab.Controls.Add(this.setSound);
            this.subOverheadTab.Controls.Add(this.label14);
            this.subOverheadTab.Controls.Add(this.unicodeStyle);
            this.subOverheadTab.Controls.Add(this.asciiStyle);
            this.subOverheadTab.Controls.Add(this.editOverheadMessage);
            this.subOverheadTab.Controls.Add(this.setColorHue);
            this.subOverheadTab.Controls.Add(this.removeOverheadMessage);
            this.subOverheadTab.Controls.Add(this.label13);
            this.subOverheadTab.Controls.Add(this.overheadFormat);
            this.subOverheadTab.Controls.Add(this.setOverheadMessage);
            this.subOverheadTab.Controls.Add(this.cliLocOverheadView);
            this.subOverheadTab.Controls.Add(this.cliLocSearch);
            this.subOverheadTab.Controls.Add(this.cliLocTextSearch);
            this.subOverheadTab.Controls.Add(this.lblOhSearch);
            this.subOverheadTab.Controls.Add(this.cliLocSearchView);
            this.subOverheadTab.Controls.Add(this.showOverheadMessages);
            this.subOverheadTab.Location = new System.Drawing.Point(10, 58);
            this.subOverheadTab.Name = "subOverheadTab";
            this.subOverheadTab.Size = new System.Drawing.Size(1225, 306);
            this.subOverheadTab.TabIndex = 3;
            this.subOverheadTab.Text = "Overhead Messages";
            // 
            // setSound
            // 
            this.setSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setSound.Location = new System.Drawing.Point(20, 512);
            this.setSound.Name = "setSound";
            this.setSound.Size = new System.Drawing.Size(210, 70);
            this.setSound.TabIndex = 103;
            this.setSound.Text = "Set Sound";
            this.setSound.UseVisualStyleBackColor = true;
            this.setSound.Click += new System.EventHandler(this.setSound_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 208);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(214, 41);
            this.label14.TabIndex = 102;
            this.label14.Text = "Message Style:";
            // 
            // unicodeStyle
            // 
            this.unicodeStyle.AutoSize = true;
            this.unicodeStyle.Location = new System.Drawing.Point(20, 252);
            this.unicodeStyle.Name = "unicodeStyle";
            this.unicodeStyle.Size = new System.Drawing.Size(166, 45);
            this.unicodeStyle.TabIndex = 101;
            this.unicodeStyle.TabStop = true;
            this.unicodeStyle.Text = "Unicode";
            this.unicodeStyle.UseVisualStyleBackColor = true;
            this.unicodeStyle.CheckedChanged += new System.EventHandler(this.unicodeStyle_CheckedChanged);
            // 
            // asciiStyle
            // 
            this.asciiStyle.AutoSize = true;
            this.asciiStyle.Location = new System.Drawing.Point(20, 315);
            this.asciiStyle.Name = "asciiStyle";
            this.asciiStyle.Size = new System.Drawing.Size(125, 45);
            this.asciiStyle.TabIndex = 100;
            this.asciiStyle.TabStop = true;
            this.asciiStyle.Text = "ASCII";
            this.asciiStyle.UseVisualStyleBackColor = true;
            this.asciiStyle.CheckedChanged += new System.EventHandler(this.asciiStyle_CheckedChanged);
            // 
            // editOverheadMessage
            // 
            this.editOverheadMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editOverheadMessage.Location = new System.Drawing.Point(20, 258);
            this.editOverheadMessage.Name = "editOverheadMessage";
            this.editOverheadMessage.Size = new System.Drawing.Size(210, 70);
            this.editOverheadMessage.TabIndex = 97;
            this.editOverheadMessage.Text = "Edit";
            this.editOverheadMessage.UseVisualStyleBackColor = true;
            this.editOverheadMessage.Click += new System.EventHandler(this.editOverheadMessage_Click);
            // 
            // setColorHue
            // 
            this.setColorHue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.setColorHue.Location = new System.Drawing.Point(20, 428);
            this.setColorHue.Name = "setColorHue";
            this.setColorHue.Size = new System.Drawing.Size(210, 70);
            this.setColorHue.TabIndex = 96;
            this.setColorHue.Text = "Set Hue";
            this.setColorHue.UseVisualStyleBackColor = true;
            this.setColorHue.Click += new System.EventHandler(this.setColorHue_Click);
            // 
            // removeOverheadMessage
            // 
            this.removeOverheadMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeOverheadMessage.Location = new System.Drawing.Point(20, 342);
            this.removeOverheadMessage.Name = "removeOverheadMessage";
            this.removeOverheadMessage.Size = new System.Drawing.Size(210, 70);
            this.removeOverheadMessage.TabIndex = 95;
            this.removeOverheadMessage.Text = "Remove";
            this.removeOverheadMessage.UseVisualStyleBackColor = true;
            this.removeOverheadMessage.Click += new System.EventHandler(this.removeOverheadMessage_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 98);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(119, 41);
            this.label13.TabIndex = 94;
            this.label13.Text = "Format:";
            // 
            // overheadFormat
            // 
            this.overheadFormat.Location = new System.Drawing.Point(20, 142);
            this.overheadFormat.Name = "overheadFormat";
            this.overheadFormat.Size = new System.Drawing.Size(210, 47);
            this.overheadFormat.TabIndex = 93;
            this.overheadFormat.Text = "[{msg}]";
            this.overheadFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.overheadFormat.TextChanged += new System.EventHandler(this.overheadFormat_TextChanged);
            // 
            // setOverheadMessage
            // 
            this.setOverheadMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setOverheadMessage.Location = new System.Drawing.Point(1078, 22);
            this.setOverheadMessage.Name = "setOverheadMessage";
            this.setOverheadMessage.Size = new System.Drawing.Size(140, 60);
            this.setOverheadMessage.TabIndex = 92;
            this.setOverheadMessage.Text = "Add";
            this.setOverheadMessage.UseVisualStyleBackColor = true;
            this.setOverheadMessage.Click += new System.EventHandler(this.setOverheadMessage_Click);
            // 
            // cliLocOverheadView
            // 
            this.cliLocOverheadView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cliLocOverheadView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cliLocOriginal,
            this.cliLocOverheadMessage,
            this.cliLocSound});
            this.cliLocOverheadView.HideSelection = false;
            this.cliLocOverheadView.Location = new System.Drawing.Point(245, 390);
            this.cliLocOverheadView.Name = "cliLocOverheadView";
            this.cliLocOverheadView.Size = new System.Drawing.Size(973, 192);
            this.cliLocOverheadView.TabIndex = 91;
            this.cliLocOverheadView.UseCompatibleStateImageBehavior = false;
            this.cliLocOverheadView.View = System.Windows.Forms.View.Details;
            // 
            // cliLocOriginal
            // 
            this.cliLocOriginal.Text = "Original Text";
            this.cliLocOriginal.Width = 168;
            // 
            // cliLocOverheadMessage
            // 
            this.cliLocOverheadMessage.Text = "Overhead Message";
            this.cliLocOverheadMessage.Width = 181;
            // 
            // cliLocSound
            // 
            this.cliLocSound.Text = "Sound";
            this.cliLocSound.Width = 48;
            // 
            // cliLocSearch
            // 
            this.cliLocSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cliLocSearch.Location = new System.Drawing.Point(912, 22);
            this.cliLocSearch.Name = "cliLocSearch";
            this.cliLocSearch.Size = new System.Drawing.Size(150, 60);
            this.cliLocSearch.TabIndex = 90;
            this.cliLocSearch.Text = "Search";
            this.cliLocSearch.UseVisualStyleBackColor = true;
            this.cliLocSearch.Click += new System.EventHandler(this.cliLocSearch_Click);
            // 
            // cliLocTextSearch
            // 
            this.cliLocTextSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cliLocTextSearch.Location = new System.Drawing.Point(365, 22);
            this.cliLocTextSearch.Name = "cliLocTextSearch";
            this.cliLocTextSearch.Size = new System.Drawing.Size(533, 47);
            this.cliLocTextSearch.TabIndex = 89;
            // 
            // lblOhSearch
            // 
            this.lblOhSearch.AutoSize = true;
            this.lblOhSearch.Location = new System.Drawing.Point(238, 30);
            this.lblOhSearch.Name = "lblOhSearch";
            this.lblOhSearch.Size = new System.Drawing.Size(113, 41);
            this.lblOhSearch.TabIndex = 88;
            this.lblOhSearch.Text = "Search:";
            // 
            // cliLocSearchView
            // 
            this.cliLocSearchView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cliLocSearchView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cliLocSearchNumber,
            this.cliLocSearchText});
            this.cliLocSearchView.HideSelection = false;
            this.cliLocSearchView.Location = new System.Drawing.Point(245, 98);
            this.cliLocSearchView.Name = "cliLocSearchView";
            this.cliLocSearchView.Size = new System.Drawing.Size(973, 277);
            this.cliLocSearchView.TabIndex = 87;
            this.cliLocSearchView.UseCompatibleStateImageBehavior = false;
            this.cliLocSearchView.View = System.Windows.Forms.View.Details;
            // 
            // cliLocSearchNumber
            // 
            this.cliLocSearchNumber.Text = "Number";
            this.cliLocSearchNumber.Width = 75;
            // 
            // cliLocSearchText
            // 
            this.cliLocSearchText.Text = "Text";
            this.cliLocSearchText.Width = 313;
            // 
            // showOverheadMessages
            // 
            this.showOverheadMessages.Location = new System.Drawing.Point(20, 22);
            this.showOverheadMessages.Name = "showOverheadMessages";
            this.showOverheadMessages.Size = new System.Drawing.Size(178, 60);
            this.showOverheadMessages.TabIndex = 85;
            this.showOverheadMessages.Text = "Enabled";
            this.showOverheadMessages.UseVisualStyleBackColor = true;
            this.showOverheadMessages.CheckedChanged += new System.EventHandler(this.showOverheadMessages_CheckedChanged);
            // 
            // subWaypoints
            // 
            this.subWaypoints.BackColor = System.Drawing.SystemColors.Control;
            this.subWaypoints.Controls.Add(this.waypointOnDeath);
            this.subWaypoints.Controls.Add(this.lblWaypointTiles);
            this.subWaypoints.Controls.Add(this.hideWaypointDist);
            this.subWaypoints.Controls.Add(this.hideWaypointWithin);
            this.subWaypoints.Controls.Add(this.txtWaypointDistanceSec);
            this.subWaypoints.Controls.Add(this.lblWaypointSeconds);
            this.subWaypoints.Controls.Add(this.showWaypointDistance);
            this.subWaypoints.Controls.Add(this.showWaypointOverhead);
            this.subWaypoints.Controls.Add(this.btnRemoveSelectedWaypoint);
            this.subWaypoints.Controls.Add(this.btnHideWaypoint);
            this.subWaypoints.Controls.Add(this.btnUseCurrentLoc);
            this.subWaypoints.Controls.Add(this.txtWaypointName);
            this.subWaypoints.Controls.Add(this.lblWaypointY);
            this.subWaypoints.Controls.Add(this.lblWaypointX);
            this.subWaypoints.Controls.Add(this.txtWaypointX);
            this.subWaypoints.Controls.Add(this.txtWaypointY);
            this.subWaypoints.Controls.Add(this.lblWaypoint);
            this.subWaypoints.Controls.Add(this.btnAddWaypoint);
            this.subWaypoints.Controls.Add(this.waypointList);
            this.subWaypoints.Location = new System.Drawing.Point(10, 58);
            this.subWaypoints.Name = "subWaypoints";
            this.subWaypoints.Size = new System.Drawing.Size(1225, 306);
            this.subWaypoints.TabIndex = 4;
            this.subWaypoints.Text = "Waypoints";
            // 
            // waypointOnDeath
            // 
            this.waypointOnDeath.AutoSize = true;
            this.waypointOnDeath.Location = new System.Drawing.Point(460, 218);
            this.waypointOnDeath.Name = "waypointOnDeath";
            this.waypointOnDeath.Size = new System.Drawing.Size(399, 45);
            this.waypointOnDeath.TabIndex = 66;
            this.waypointOnDeath.Text = "Create waypoint on death";
            this.waypointOnDeath.UseVisualStyleBackColor = true;
            this.waypointOnDeath.CheckedChanged += new System.EventHandler(this.waypointOnDeath_CheckedChanged);
            // 
            // lblWaypointTiles
            // 
            this.lblWaypointTiles.Location = new System.Drawing.Point(935, 158);
            this.lblWaypointTiles.Name = "lblWaypointTiles";
            this.lblWaypointTiles.Size = new System.Drawing.Size(145, 44);
            this.lblWaypointTiles.TabIndex = 65;
            this.lblWaypointTiles.Text = "tiles";
            // 
            // hideWaypointDist
            // 
            this.hideWaypointDist.Location = new System.Drawing.Point(840, 150);
            this.hideWaypointDist.Name = "hideWaypointDist";
            this.hideWaypointDist.Size = new System.Drawing.Size(80, 47);
            this.hideWaypointDist.TabIndex = 64;
            this.hideWaypointDist.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hideWaypointDist.TextChanged += new System.EventHandler(this.hideWaypointDist_TextChanged);
            // 
            // hideWaypointWithin
            // 
            this.hideWaypointWithin.AutoSize = true;
            this.hideWaypointWithin.Location = new System.Drawing.Point(460, 155);
            this.hideWaypointWithin.Name = "hideWaypointWithin";
            this.hideWaypointWithin.Size = new System.Drawing.Size(336, 45);
            this.hideWaypointWithin.TabIndex = 63;
            this.hideWaypointWithin.Text = "Hide waypoint within";
            this.hideWaypointWithin.UseVisualStyleBackColor = true;
            this.hideWaypointWithin.CheckedChanged += new System.EventHandler(this.hideWaypointWithin_CheckedChanged);
            // 
            // txtWaypointDistanceSec
            // 
            this.txtWaypointDistanceSec.Location = new System.Drawing.Point(840, 88);
            this.txtWaypointDistanceSec.Name = "txtWaypointDistanceSec";
            this.txtWaypointDistanceSec.Size = new System.Drawing.Size(80, 47);
            this.txtWaypointDistanceSec.TabIndex = 62;
            this.txtWaypointDistanceSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWaypointDistanceSec.TextChanged += new System.EventHandler(this.txtWaypointDistanceSec_TextChanged);
            // 
            // lblWaypointSeconds
            // 
            this.lblWaypointSeconds.Location = new System.Drawing.Point(935, 92);
            this.lblWaypointSeconds.Name = "lblWaypointSeconds";
            this.lblWaypointSeconds.Size = new System.Drawing.Size(145, 46);
            this.lblWaypointSeconds.TabIndex = 61;
            this.lblWaypointSeconds.Text = "secs";
            // 
            // showWaypointDistance
            // 
            this.showWaypointDistance.AutoSize = true;
            this.showWaypointDistance.Location = new System.Drawing.Point(460, 92);
            this.showWaypointDistance.Name = "showWaypointDistance";
            this.showWaypointDistance.Size = new System.Drawing.Size(374, 45);
            this.showWaypointDistance.TabIndex = 60;
            this.showWaypointDistance.Text = "Show tile distance every";
            this.showWaypointDistance.UseVisualStyleBackColor = true;
            this.showWaypointDistance.CheckedChanged += new System.EventHandler(this.showWaypointDistance_CheckedChanged);
            // 
            // showWaypointOverhead
            // 
            this.showWaypointOverhead.AutoSize = true;
            this.showWaypointOverhead.Location = new System.Drawing.Point(460, 30);
            this.showWaypointOverhead.Name = "showWaypointOverhead";
            this.showWaypointOverhead.Size = new System.Drawing.Size(529, 45);
            this.showWaypointOverhead.TabIndex = 59;
            this.showWaypointOverhead.Text = "Show waypoint messages overhead";
            this.showWaypointOverhead.UseVisualStyleBackColor = true;
            this.showWaypointOverhead.CheckedChanged += new System.EventHandler(this.showWaypointOverhead_CheckedChanged);
            // 
            // btnRemoveSelectedWaypoint
            // 
            this.btnRemoveSelectedWaypoint.Location = new System.Drawing.Point(460, 605);
            this.btnRemoveSelectedWaypoint.Name = "btnRemoveSelectedWaypoint";
            this.btnRemoveSelectedWaypoint.Size = new System.Drawing.Size(338, 73);
            this.btnRemoveSelectedWaypoint.TabIndex = 10;
            this.btnRemoveSelectedWaypoint.Text = "Remove Selected";
            this.btnRemoveSelectedWaypoint.UseVisualStyleBackColor = true;
            this.btnRemoveSelectedWaypoint.Click += new System.EventHandler(this.btnRemoveSelectedWaypoint_Click);
            // 
            // btnHideWaypoint
            // 
            this.btnHideWaypoint.Location = new System.Drawing.Point(910, 605);
            this.btnHideWaypoint.Name = "btnHideWaypoint";
            this.btnHideWaypoint.Size = new System.Drawing.Size(322, 73);
            this.btnHideWaypoint.TabIndex = 9;
            this.btnHideWaypoint.Text = "Clear Waypoint";
            this.btnHideWaypoint.UseVisualStyleBackColor = true;
            this.btnHideWaypoint.Click += new System.EventHandler(this.btnHideWaypoint_Click);
            // 
            // btnUseCurrentLoc
            // 
            this.btnUseCurrentLoc.Location = new System.Drawing.Point(910, 378);
            this.btnUseCurrentLoc.Name = "btnUseCurrentLoc";
            this.btnUseCurrentLoc.Size = new System.Drawing.Size(322, 57);
            this.btnUseCurrentLoc.TabIndex = 8;
            this.btnUseCurrentLoc.Text = "Use Current Position";
            this.btnUseCurrentLoc.UseVisualStyleBackColor = true;
            this.btnUseCurrentLoc.Click += new System.EventHandler(this.btnUseCurrentLoc_Click);
            // 
            // txtWaypointName
            // 
            this.txtWaypointName.Location = new System.Drawing.Point(572, 302);
            this.txtWaypointName.Name = "txtWaypointName";
            this.txtWaypointName.Size = new System.Drawing.Size(323, 47);
            this.txtWaypointName.TabIndex = 7;
            // 
            // lblWaypointY
            // 
            this.lblWaypointY.AutoSize = true;
            this.lblWaypointY.Location = new System.Drawing.Point(712, 385);
            this.lblWaypointY.Name = "lblWaypointY";
            this.lblWaypointY.Size = new System.Drawing.Size(42, 41);
            this.lblWaypointY.TabIndex = 6;
            this.lblWaypointY.Text = "Y:";
            // 
            // lblWaypointX
            // 
            this.lblWaypointX.AutoSize = true;
            this.lblWaypointX.Location = new System.Drawing.Point(515, 385);
            this.lblWaypointX.Name = "lblWaypointX";
            this.lblWaypointX.Size = new System.Drawing.Size(43, 41);
            this.lblWaypointX.TabIndex = 5;
            this.lblWaypointX.Text = "X:";
            // 
            // txtWaypointX
            // 
            this.txtWaypointX.Location = new System.Drawing.Point(572, 378);
            this.txtWaypointX.Name = "txtWaypointX";
            this.txtWaypointX.Size = new System.Drawing.Size(126, 47);
            this.txtWaypointX.TabIndex = 4;
            // 
            // txtWaypointY
            // 
            this.txtWaypointY.Location = new System.Drawing.Point(770, 378);
            this.txtWaypointY.Name = "txtWaypointY";
            this.txtWaypointY.Size = new System.Drawing.Size(125, 47);
            this.txtWaypointY.TabIndex = 3;
            // 
            // lblWaypoint
            // 
            this.lblWaypoint.AutoSize = true;
            this.lblWaypoint.Location = new System.Drawing.Point(452, 310);
            this.lblWaypoint.Name = "lblWaypoint";
            this.lblWaypoint.Size = new System.Drawing.Size(104, 41);
            this.lblWaypoint.TabIndex = 2;
            this.lblWaypoint.Text = "Name:";
            // 
            // btnAddWaypoint
            // 
            this.btnAddWaypoint.Location = new System.Drawing.Point(910, 302);
            this.btnAddWaypoint.Name = "btnAddWaypoint";
            this.btnAddWaypoint.Size = new System.Drawing.Size(322, 60);
            this.btnAddWaypoint.TabIndex = 1;
            this.btnAddWaypoint.Text = "Add Waypoint";
            this.btnAddWaypoint.UseVisualStyleBackColor = true;
            this.btnAddWaypoint.Click += new System.EventHandler(this.btnAddWaypoint_Click);
            // 
            // waypointList
            // 
            this.waypointList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.waypointList.FormattingEnabled = true;
            this.waypointList.ItemHeight = 41;
            this.waypointList.Location = new System.Drawing.Point(28, 30);
            this.waypointList.Name = "waypointList";
            this.waypointList.Size = new System.Drawing.Size(410, 209);
            this.waypointList.TabIndex = 0;
            this.waypointList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listWaypoints_MouseDoubleClick);
            this.waypointList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listWaypoints_MouseDown);
            // 
            // subBuffsDebuffs
            // 
            this.subBuffsDebuffs.BackColor = System.Drawing.SystemColors.Control;
            this.subBuffsDebuffs.Controls.Add(this.cooldownGumpBox);
            this.subBuffsDebuffs.Controls.Add(this.buffBarGroupBox);
            this.subBuffsDebuffs.Controls.Add(this.buffDebuffOptions);
            this.subBuffsDebuffs.Controls.Add(this.showBuffDebuffOverhead);
            this.subBuffsDebuffs.Location = new System.Drawing.Point(10, 58);
            this.subBuffsDebuffs.Name = "subBuffsDebuffs";
            this.subBuffsDebuffs.Size = new System.Drawing.Size(1225, 306);
            this.subBuffsDebuffs.TabIndex = 5;
            this.subBuffsDebuffs.Text = "Buffs / Cooldowns";
            // 
            // cooldownGumpBox
            // 
            this.cooldownGumpBox.Controls.Add(this.cooldownHeight);
            this.cooldownGumpBox.Controls.Add(this.lblCooldownHeight);
            this.cooldownGumpBox.Controls.Add(this.cooldownWidth);
            this.cooldownGumpBox.Controls.Add(this.lblCooldownWidth);
            this.cooldownGumpBox.Location = new System.Drawing.Point(35, 310);
            this.cooldownGumpBox.Name = "cooldownGumpBox";
            this.cooldownGumpBox.Size = new System.Drawing.Size(580, 378);
            this.cooldownGumpBox.TabIndex = 133;
            this.cooldownGumpBox.TabStop = false;
            this.cooldownGumpBox.Text = "Cooldowns:";
            // 
            // cooldownHeight
            // 
            this.cooldownHeight.Location = new System.Drawing.Point(188, 65);
            this.cooldownHeight.Name = "cooldownHeight";
            this.cooldownHeight.Size = new System.Drawing.Size(90, 47);
            this.cooldownHeight.TabIndex = 12;
            this.cooldownHeight.TextChanged += new System.EventHandler(this.cooldownHeight_TextChanged);
            // 
            // lblCooldownHeight
            // 
            this.lblCooldownHeight.AutoSize = true;
            this.lblCooldownHeight.Location = new System.Drawing.Point(22, 72);
            this.lblCooldownHeight.Name = "lblCooldownHeight";
            this.lblCooldownHeight.Size = new System.Drawing.Size(160, 41);
            this.lblCooldownHeight.TabIndex = 11;
            this.lblCooldownHeight.Text = "Bar height:";
            // 
            // cooldownWidth
            // 
            this.cooldownWidth.Location = new System.Drawing.Point(188, 135);
            this.cooldownWidth.Name = "cooldownWidth";
            this.cooldownWidth.Size = new System.Drawing.Size(90, 47);
            this.cooldownWidth.TabIndex = 10;
            this.cooldownWidth.TextChanged += new System.EventHandler(this.cooldownWidth_TextChanged);
            // 
            // lblCooldownWidth
            // 
            this.lblCooldownWidth.AutoSize = true;
            this.lblCooldownWidth.Location = new System.Drawing.Point(22, 142);
            this.lblCooldownWidth.Name = "lblCooldownWidth";
            this.lblCooldownWidth.Size = new System.Drawing.Size(149, 41);
            this.lblCooldownWidth.TabIndex = 9;
            this.lblCooldownWidth.Text = "Bar width:";
            // 
            // buffBarGroupBox
            // 
            this.buffBarGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buffBarGroupBox.Controls.Add(this.showBuffDebuffTimeType);
            this.buffBarGroupBox.Controls.Add(this.lblShowBuffTime);
            this.buffBarGroupBox.Controls.Add(this.useBlackBuffDebuffBg);
            this.buffBarGroupBox.Controls.Add(this.buffBarHeight);
            this.buffBarGroupBox.Controls.Add(this.lblBuffBarHeight);
            this.buffBarGroupBox.Controls.Add(this.buffBarSort);
            this.buffBarGroupBox.Controls.Add(this.lblBuffSortBy);
            this.buffBarGroupBox.Controls.Add(this.buffBarWidth);
            this.buffBarGroupBox.Controls.Add(this.lblBuffBarWidth);
            this.buffBarGroupBox.Controls.Add(this.showBuffIcons);
            this.buffBarGroupBox.Controls.Add(this.showBuffDebuffGump);
            this.buffBarGroupBox.Location = new System.Drawing.Point(600, 30);
            this.buffBarGroupBox.Name = "buffBarGroupBox";
            this.buffBarGroupBox.Size = new System.Drawing.Size(588, 658);
            this.buffBarGroupBox.TabIndex = 132;
            this.buffBarGroupBox.TabStop = false;
            this.buffBarGroupBox.Text = "Buff/Debuff Gump:";
            // 
            // showBuffDebuffTimeType
            // 
            this.showBuffDebuffTimeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.showBuffDebuffTimeType.FormattingEnabled = true;
            this.showBuffDebuffTimeType.Items.AddRange(new object[] {
            "Next to name",
            "Outside of bar",
            "Right side of bar",
            "Under Icon",
            "None"});
            this.showBuffDebuffTimeType.Location = new System.Drawing.Point(210, 280);
            this.showBuffDebuffTimeType.Name = "showBuffDebuffTimeType";
            this.showBuffDebuffTimeType.Size = new System.Drawing.Size(315, 49);
            this.showBuffDebuffTimeType.TabIndex = 12;
            this.showBuffDebuffTimeType.SelectedIndexChanged += new System.EventHandler(this.showBuffDebuffTimeType_SelectedIndexChanged);
            // 
            // lblShowBuffTime
            // 
            this.lblShowBuffTime.AutoSize = true;
            this.lblShowBuffTime.Location = new System.Drawing.Point(30, 288);
            this.lblShowBuffTime.Name = "lblShowBuffTime";
            this.lblShowBuffTime.Size = new System.Drawing.Size(165, 41);
            this.lblShowBuffTime.TabIndex = 11;
            this.lblShowBuffTime.Text = "Show time:";
            // 
            // useBlackBuffDebuffBg
            // 
            this.useBlackBuffDebuffBg.AutoSize = true;
            this.useBlackBuffDebuffBg.Location = new System.Drawing.Point(38, 218);
            this.useBlackBuffDebuffBg.Name = "useBlackBuffDebuffBg";
            this.useBlackBuffDebuffBg.Size = new System.Drawing.Size(351, 45);
            this.useBlackBuffDebuffBg.TabIndex = 9;
            this.useBlackBuffDebuffBg.Text = "Use black background";
            this.useBlackBuffDebuffBg.UseVisualStyleBackColor = true;
            this.useBlackBuffDebuffBg.CheckedChanged += new System.EventHandler(this.useBlackBuffDebuffBg_CheckedChanged);
            // 
            // buffBarHeight
            // 
            this.buffBarHeight.Location = new System.Drawing.Point(195, 398);
            this.buffBarHeight.Name = "buffBarHeight";
            this.buffBarHeight.Size = new System.Drawing.Size(90, 47);
            this.buffBarHeight.TabIndex = 8;
            this.buffBarHeight.TextChanged += new System.EventHandler(this.buffBarHeight_TextChanged);
            // 
            // lblBuffBarHeight
            // 
            this.lblBuffBarHeight.AutoSize = true;
            this.lblBuffBarHeight.Location = new System.Drawing.Point(30, 405);
            this.lblBuffBarHeight.Name = "lblBuffBarHeight";
            this.lblBuffBarHeight.Size = new System.Drawing.Size(160, 41);
            this.lblBuffBarHeight.TabIndex = 7;
            this.lblBuffBarHeight.Text = "Bar height:";
            // 
            // buffBarSort
            // 
            this.buffBarSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.buffBarSort.FormattingEnabled = true;
            this.buffBarSort.Items.AddRange(new object[] {
            "Alphabetical (A-Z)",
            "Ascending by time",
            "Descending by time"});
            this.buffBarSort.Location = new System.Drawing.Point(162, 558);
            this.buffBarSort.Name = "buffBarSort";
            this.buffBarSort.Size = new System.Drawing.Size(363, 49);
            this.buffBarSort.TabIndex = 6;
            this.buffBarSort.SelectedIndexChanged += new System.EventHandler(this.buffBarSort_SelectedIndexChanged);
            // 
            // lblBuffSortBy
            // 
            this.lblBuffSortBy.AutoSize = true;
            this.lblBuffSortBy.Location = new System.Drawing.Point(30, 565);
            this.lblBuffSortBy.Name = "lblBuffSortBy";
            this.lblBuffSortBy.Size = new System.Drawing.Size(120, 41);
            this.lblBuffSortBy.TabIndex = 5;
            this.lblBuffSortBy.Text = "Sort by:";
            // 
            // buffBarWidth
            // 
            this.buffBarWidth.Location = new System.Drawing.Point(195, 468);
            this.buffBarWidth.Name = "buffBarWidth";
            this.buffBarWidth.Size = new System.Drawing.Size(90, 47);
            this.buffBarWidth.TabIndex = 4;
            this.buffBarWidth.TextChanged += new System.EventHandler(this.buffBarWidth_TextChanged);
            // 
            // lblBuffBarWidth
            // 
            this.lblBuffBarWidth.AutoSize = true;
            this.lblBuffBarWidth.Location = new System.Drawing.Point(30, 475);
            this.lblBuffBarWidth.Name = "lblBuffBarWidth";
            this.lblBuffBarWidth.Size = new System.Drawing.Size(149, 41);
            this.lblBuffBarWidth.TabIndex = 3;
            this.lblBuffBarWidth.Text = "Bar width:";
            // 
            // showBuffIcons
            // 
            this.showBuffIcons.AutoSize = true;
            this.showBuffIcons.Location = new System.Drawing.Point(38, 155);
            this.showBuffIcons.Name = "showBuffIcons";
            this.showBuffIcons.Size = new System.Drawing.Size(366, 45);
            this.showBuffIcons.TabIndex = 1;
            this.showBuffIcons.Text = "Show buff/debuff icons";
            this.showBuffIcons.UseVisualStyleBackColor = true;
            this.showBuffIcons.CheckedChanged += new System.EventHandler(this.showBuffIcons_CheckedChanged);
            // 
            // showBuffDebuffGump
            // 
            this.showBuffDebuffGump.AutoSize = true;
            this.showBuffDebuffGump.Location = new System.Drawing.Point(38, 55);
            this.showBuffDebuffGump.Name = "showBuffDebuffGump";
            this.showBuffDebuffGump.Size = new System.Drawing.Size(376, 45);
            this.showBuffDebuffGump.TabIndex = 0;
            this.showBuffDebuffGump.Text = "Show buff/debuff gump";
            this.showBuffDebuffGump.UseVisualStyleBackColor = true;
            this.showBuffDebuffGump.CheckedChanged += new System.EventHandler(this.showBuffDebuffGump_CheckedChanged);
            // 
            // buffDebuffOptions
            // 
            this.buffDebuffOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buffDebuffOptions.Location = new System.Drawing.Point(65, 92);
            this.buffDebuffOptions.Name = "buffDebuffOptions";
            this.buffDebuffOptions.Size = new System.Drawing.Size(350, 76);
            this.buffDebuffOptions.TabIndex = 131;
            this.buffDebuffOptions.Text = "Buff / Debuff Options";
            this.buffDebuffOptions.UseVisualStyleBackColor = true;
            this.buffDebuffOptions.Click += new System.EventHandler(this.buffDebuffOptions_Click);
            // 
            // showBuffDebuffOverhead
            // 
            this.showBuffDebuffOverhead.AutoSize = true;
            this.showBuffDebuffOverhead.Location = new System.Drawing.Point(35, 30);
            this.showBuffDebuffOverhead.Name = "showBuffDebuffOverhead";
            this.showBuffDebuffOverhead.Size = new System.Drawing.Size(421, 45);
            this.showBuffDebuffOverhead.TabIndex = 130;
            this.showBuffDebuffOverhead.Text = "Show buff/debuff overhead";
            this.showBuffDebuffOverhead.UseVisualStyleBackColor = true;
            this.showBuffDebuffOverhead.CheckedChanged += new System.EventHandler(this.showBuffDebuffOverhead_CheckedChanged);
            // 
            // dressTab
            // 
            this.dressTab.Controls.Add(this.groupBox6);
            this.dressTab.Controls.Add(this.groupBox5);
            this.dressTab.Location = new System.Drawing.Point(10, 202);
            this.dressTab.Name = "dressTab";
            this.dressTab.Size = new System.Drawing.Size(1297, 713);
            this.dressTab.TabIndex = 3;
            this.dressTab.Text = "Arm/Dress";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.clearDress);
            this.groupBox6.Controls.Add(this.dressDelSel);
            this.groupBox6.Controls.Add(this.undressBag);
            this.groupBox6.Controls.Add(this.undressList);
            this.groupBox6.Controls.Add(this.dressUseCur);
            this.groupBox6.Controls.Add(this.targItem);
            this.groupBox6.Controls.Add(this.dressItems);
            this.groupBox6.Controls.Add(this.dressNow);
            this.groupBox6.Location = new System.Drawing.Point(502, 8);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(748, 362);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Arm/Dress Items";
            // 
            // clearDress
            // 
            this.clearDress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearDress.Location = new System.Drawing.Point(492, 642);
            this.clearDress.Name = "clearDress";
            this.clearDress.Size = new System.Drawing.Size(240, 80);
            this.clearDress.TabIndex = 13;
            this.clearDress.Text = "Clear List";
            this.clearDress.Click += new System.EventHandler(this.clearDress_Click);
            // 
            // dressDelSel
            // 
            this.dressDelSel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dressDelSel.Location = new System.Drawing.Point(492, 425);
            this.dressDelSel.Name = "dressDelSel";
            this.dressDelSel.Size = new System.Drawing.Size(240, 80);
            this.dressDelSel.TabIndex = 12;
            this.dressDelSel.Text = "Remove";
            this.dressDelSel.Click += new System.EventHandler(this.dressDelSel_Click);
            // 
            // undressBag
            // 
            this.undressBag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.undressBag.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.undressBag.Location = new System.Drawing.Point(492, 520);
            this.undressBag.Name = "undressBag";
            this.undressBag.Size = new System.Drawing.Size(240, 100);
            this.undressBag.TabIndex = 11;
            this.undressBag.Text = "Change Undress Bag";
            this.undressBag.Click += new System.EventHandler(this.undressBag_Click);
            // 
            // undressList
            // 
            this.undressList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.undressList.Location = new System.Drawing.Point(492, 140);
            this.undressList.Name = "undressList";
            this.undressList.Size = new System.Drawing.Size(240, 80);
            this.undressList.TabIndex = 10;
            this.undressList.Text = "Undress";
            this.undressList.Click += new System.EventHandler(this.undressList_Click);
            // 
            // dressUseCur
            // 
            this.dressUseCur.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dressUseCur.Location = new System.Drawing.Point(492, 330);
            this.dressUseCur.Name = "dressUseCur";
            this.dressUseCur.Size = new System.Drawing.Size(240, 80);
            this.dressUseCur.TabIndex = 9;
            this.dressUseCur.Text = "Add Current";
            this.dressUseCur.Click += new System.EventHandler(this.dressUseCur_Click);
            // 
            // targItem
            // 
            this.targItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.targItem.Location = new System.Drawing.Point(492, 235);
            this.targItem.Name = "targItem";
            this.targItem.Size = new System.Drawing.Size(240, 80);
            this.targItem.TabIndex = 7;
            this.targItem.Text = "Add (Target)";
            this.targItem.Click += new System.EventHandler(this.targItem_Click);
            // 
            // dressItems
            // 
            this.dressItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dressItems.IntegralHeight = false;
            this.dressItems.ItemHeight = 41;
            this.dressItems.Location = new System.Drawing.Point(15, 45);
            this.dressItems.Name = "dressItems";
            this.dressItems.Size = new System.Drawing.Size(463, 303);
            this.dressItems.TabIndex = 6;
            this.dressItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dressItems_KeyDown);
            this.dressItems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dressItems_MouseDown);
            // 
            // dressNow
            // 
            this.dressNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dressNow.Location = new System.Drawing.Point(492, 45);
            this.dressNow.Name = "dressNow";
            this.dressNow.Size = new System.Drawing.Size(240, 80);
            this.dressNow.TabIndex = 6;
            this.dressNow.Text = "Dress";
            this.dressNow.Click += new System.EventHandler(this.dressNow_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.removeDress);
            this.groupBox5.Controls.Add(this.addDress);
            this.groupBox5.Controls.Add(this.dressList);
            this.groupBox5.Controls.Add(this.undressConflicts);
            this.groupBox5.Location = new System.Drawing.Point(20, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(468, 362);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Arm/Dress Selection";
            // 
            // removeDress
            // 
            this.removeDress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeDress.Location = new System.Drawing.Point(302, 225);
            this.removeDress.Name = "removeDress";
            this.removeDress.Size = new System.Drawing.Size(150, 63);
            this.removeDress.TabIndex = 5;
            this.removeDress.Text = "Remove";
            this.removeDress.Click += new System.EventHandler(this.removeDress_Click);
            // 
            // addDress
            // 
            this.addDress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addDress.Location = new System.Drawing.Point(15, 225);
            this.addDress.Name = "addDress";
            this.addDress.Size = new System.Drawing.Size(150, 63);
            this.addDress.TabIndex = 4;
            this.addDress.Text = "Add...";
            this.addDress.Click += new System.EventHandler(this.addDress_Click);
            // 
            // dressList
            // 
            this.dressList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dressList.IntegralHeight = false;
            this.dressList.ItemHeight = 41;
            this.dressList.Location = new System.Drawing.Point(15, 45);
            this.dressList.Name = "dressList";
            this.dressList.Size = new System.Drawing.Size(437, 165);
            this.dressList.TabIndex = 3;
            this.dressList.SelectedIndexChanged += new System.EventHandler(this.dressList_SelectedIndexChanged);
            // 
            // undressConflicts
            // 
            this.undressConflicts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.undressConflicts.Location = new System.Drawing.Point(15, 302);
            this.undressConflicts.Name = "undressConflicts";
            this.undressConflicts.Size = new System.Drawing.Size(343, 46);
            this.undressConflicts.TabIndex = 6;
            this.undressConflicts.Text = "Auto-move conflicts";
            this.undressConflicts.CheckedChanged += new System.EventHandler(this.undressConflicts_CheckedChanged);
            // 
            // skillsTab
            // 
            this.skillsTab.Controls.Add(this.captureMibs);
            this.skillsTab.Controls.Add(this.dispDeltaOverhead);
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
            this.skillsTab.Location = new System.Drawing.Point(10, 202);
            this.skillsTab.Name = "skillsTab";
            this.skillsTab.Size = new System.Drawing.Size(1297, 713);
            this.skillsTab.TabIndex = 2;
            this.skillsTab.Text = "Skills";
            // 
            // captureMibs
            // 
            this.captureMibs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.captureMibs.Location = new System.Drawing.Point(860, 575);
            this.captureMibs.Name = "captureMibs";
            this.captureMibs.Size = new System.Drawing.Size(382, 47);
            this.captureMibs.TabIndex = 70;
            this.captureMibs.Text = "Capture MIBs to file";
            this.captureMibs.UseVisualStyleBackColor = true;
            this.captureMibs.CheckedChanged += new System.EventHandler(this.captureMibs_CheckedChanged);
            // 
            // dispDeltaOverhead
            // 
            this.dispDeltaOverhead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dispDeltaOverhead.AutoSize = true;
            this.dispDeltaOverhead.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dispDeltaOverhead.Location = new System.Drawing.Point(1077, 518);
            this.dispDeltaOverhead.Name = "dispDeltaOverhead";
            this.dispDeltaOverhead.Size = new System.Drawing.Size(173, 42);
            this.dispDeltaOverhead.TabIndex = 14;
            this.dispDeltaOverhead.Text = "Overhead";
            this.dispDeltaOverhead.UseVisualStyleBackColor = true;
            this.dispDeltaOverhead.CheckedChanged += new System.EventHandler(this.dispDeltaOverhead_CheckedChanged);
            // 
            // logSkillChanges
            // 
            this.logSkillChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logSkillChanges.Location = new System.Drawing.Point(860, 392);
            this.logSkillChanges.Name = "logSkillChanges";
            this.logSkillChanges.Size = new System.Drawing.Size(390, 48);
            this.logSkillChanges.TabIndex = 13;
            this.logSkillChanges.Text = "Log skill changes";
            this.logSkillChanges.UseVisualStyleBackColor = true;
            this.logSkillChanges.CheckedChanged += new System.EventHandler(this.logSkillChanges_CheckedChanged);
            // 
            // dispDelta
            // 
            this.dispDelta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dispDelta.Location = new System.Drawing.Point(860, 455);
            this.dispDelta.Name = "dispDelta";
            this.dispDelta.Size = new System.Drawing.Size(390, 47);
            this.dispDelta.TabIndex = 11;
            this.dispDelta.Text = "Show skill/stat changes";
            this.dispDelta.CheckedChanged += new System.EventHandler(this.dispDelta_CheckedChanged);
            // 
            // skillCopyAll
            // 
            this.skillCopyAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.skillCopyAll.Location = new System.Drawing.Point(860, 298);
            this.skillCopyAll.Name = "skillCopyAll";
            this.skillCopyAll.Size = new System.Drawing.Size(390, 80);
            this.skillCopyAll.TabIndex = 9;
            this.skillCopyAll.Text = "Copy All";
            this.skillCopyAll.Click += new System.EventHandler(this.skillCopyAll_Click);
            // 
            // skillCopySel
            // 
            this.skillCopySel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.skillCopySel.Location = new System.Drawing.Point(860, 202);
            this.skillCopySel.Name = "skillCopySel";
            this.skillCopySel.Size = new System.Drawing.Size(390, 80);
            this.skillCopySel.TabIndex = 8;
            this.skillCopySel.Text = "Copy Selected";
            this.skillCopySel.Click += new System.EventHandler(this.skillCopySel_Click);
            // 
            // baseTotal
            // 
            this.baseTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.baseTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTotal.Location = new System.Drawing.Point(1040, 315);
            this.baseTotal.Name = "baseTotal";
            this.baseTotal.ReadOnly = true;
            this.baseTotal.Size = new System.Drawing.Size(210, 47);
            this.baseTotal.TabIndex = 7;
            this.baseTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(860, 312);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 58);
            this.label1.TabIndex = 6;
            this.label1.Text = "Base Total:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // locks
            // 
            this.locks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.locks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.locks.Items.AddRange(new object[] {
            "Up",
            "Down",
            "Locked"});
            this.locks.Location = new System.Drawing.Point(1140, 122);
            this.locks.Name = "locks";
            this.locks.Size = new System.Drawing.Size(110, 49);
            this.locks.TabIndex = 5;
            // 
            // setlocks
            // 
            this.setlocks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setlocks.Location = new System.Drawing.Point(860, 108);
            this.setlocks.Name = "setlocks";
            this.setlocks.Size = new System.Drawing.Size(265, 80);
            this.setlocks.TabIndex = 4;
            this.setlocks.Text = "Set all locks:";
            this.setlocks.Click += new System.EventHandler(this.OnSetSkillLocks);
            // 
            // resetDelta
            // 
            this.resetDelta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resetDelta.Location = new System.Drawing.Point(860, 12);
            this.resetDelta.Name = "resetDelta";
            this.resetDelta.Size = new System.Drawing.Size(390, 80);
            this.resetDelta.TabIndex = 3;
            this.resetDelta.Text = "Reset  +/-";
            this.resetDelta.Click += new System.EventHandler(this.OnResetSkillDelta);
            // 
            // skillList
            // 
            this.skillList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skillList.AutoArrange = false;
            this.skillList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.skillHDRName,
            this.skillHDRvalue,
            this.skillHDRbase,
            this.skillHDRdelta,
            this.skillHDRcap,
            this.skillHDRlock});
            this.skillList.FullRowSelect = true;
            this.skillList.HideSelection = false;
            this.skillList.Location = new System.Drawing.Point(20, 12);
            this.skillList.Name = "skillList";
            this.skillList.Size = new System.Drawing.Size(825, 358);
            this.skillList.TabIndex = 1;
            this.skillList.UseCompatibleStateImageBehavior = false;
            this.skillList.View = System.Windows.Forms.View.Details;
            this.skillList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnSkillColClick);
            this.skillList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skillList_MouseDown);
            // 
            // skillHDRName
            // 
            this.skillHDRName.Text = "Skill Name";
            this.skillHDRName.Width = 112;
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
            this.agentsTab.Controls.Add(this.agentSetHotKey);
            this.agentsTab.Controls.Add(this.agentB6);
            this.agentsTab.Controls.Add(this.agentB5);
            this.agentsTab.Controls.Add(this.agentList);
            this.agentsTab.Controls.Add(this.agentGroup);
            this.agentsTab.Controls.Add(this.agentB4);
            this.agentsTab.Controls.Add(this.agentB1);
            this.agentsTab.Controls.Add(this.agentB2);
            this.agentsTab.Controls.Add(this.agentB3);
            this.agentsTab.Location = new System.Drawing.Point(10, 202);
            this.agentsTab.Name = "agentsTab";
            this.agentsTab.Size = new System.Drawing.Size(1297, 713);
            this.agentsTab.TabIndex = 6;
            this.agentsTab.Text = "Agents";
            // 
            // agentSetHotKey
            // 
            this.agentSetHotKey.Location = new System.Drawing.Point(20, 685);
            this.agentSetHotKey.Name = "agentSetHotKey";
            this.agentSetHotKey.Size = new System.Drawing.Size(325, 80);
            this.agentSetHotKey.TabIndex = 7;
            this.agentSetHotKey.Text = "Set Hot Key";
            this.agentSetHotKey.Visible = false;
            this.agentSetHotKey.Click += new System.EventHandler(this.agentSetHotKey_Click);
            // 
            // agentB6
            // 
            this.agentB6.Location = new System.Drawing.Point(20, 578);
            this.agentB6.Name = "agentB6";
            this.agentB6.Size = new System.Drawing.Size(325, 80);
            this.agentB6.TabIndex = 6;
            this.agentB6.Click += new System.EventHandler(this.agentB6_Click);
            // 
            // agentB5
            // 
            this.agentB5.Location = new System.Drawing.Point(20, 482);
            this.agentB5.Name = "agentB5";
            this.agentB5.Size = new System.Drawing.Size(325, 80);
            this.agentB5.TabIndex = 5;
            this.agentB5.Click += new System.EventHandler(this.agentB5_Click);
            // 
            // agentList
            // 
            this.agentList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.agentList.Location = new System.Drawing.Point(20, 35);
            this.agentList.Name = "agentList";
            this.agentList.Size = new System.Drawing.Size(325, 49);
            this.agentList.TabIndex = 2;
            this.agentList.SelectedIndexChanged += new System.EventHandler(this.agentList_SelectedIndexChanged);
            this.agentList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.agentList_MouseDown);
            // 
            // agentGroup
            // 
            this.agentGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.agentGroup.Controls.Add(this.agentSubList);
            this.agentGroup.Location = new System.Drawing.Point(360, 8);
            this.agentGroup.Name = "agentGroup";
            this.agentGroup.Size = new System.Drawing.Size(890, 362);
            this.agentGroup.TabIndex = 1;
            this.agentGroup.TabStop = false;
            // 
            // agentSubList
            // 
            this.agentSubList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.agentSubList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agentSubList.IntegralHeight = false;
            this.agentSubList.ItemHeight = 41;
            this.agentSubList.Location = new System.Drawing.Point(15, 55);
            this.agentSubList.Name = "agentSubList";
            this.agentSubList.Size = new System.Drawing.Size(860, 293);
            this.agentSubList.TabIndex = 0;
            this.agentSubList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.agentSubList_MouseDown);
            // 
            // agentB4
            // 
            this.agentB4.Location = new System.Drawing.Point(20, 388);
            this.agentB4.Name = "agentB4";
            this.agentB4.Size = new System.Drawing.Size(325, 80);
            this.agentB4.TabIndex = 4;
            this.agentB4.Click += new System.EventHandler(this.agentB4_Click);
            // 
            // agentB1
            // 
            this.agentB1.Location = new System.Drawing.Point(20, 102);
            this.agentB1.Name = "agentB1";
            this.agentB1.Size = new System.Drawing.Size(325, 80);
            this.agentB1.TabIndex = 1;
            this.agentB1.Click += new System.EventHandler(this.agentB1_Click);
            // 
            // agentB2
            // 
            this.agentB2.Location = new System.Drawing.Point(20, 198);
            this.agentB2.Name = "agentB2";
            this.agentB2.Size = new System.Drawing.Size(325, 80);
            this.agentB2.TabIndex = 2;
            this.agentB2.Click += new System.EventHandler(this.agentB2_Click);
            // 
            // agentB3
            // 
            this.agentB3.Location = new System.Drawing.Point(20, 292);
            this.agentB3.Name = "agentB3";
            this.agentB3.Size = new System.Drawing.Size(325, 80);
            this.agentB3.TabIndex = 3;
            this.agentB3.Click += new System.EventHandler(this.agentB3_Click);
            // 
            // filtersTab
            // 
            this.filtersTab.BackColor = System.Drawing.SystemColors.Control;
            this.filtersTab.Controls.Add(this.filterTabs);
            this.filtersTab.Location = new System.Drawing.Point(10, 106);
            this.filtersTab.Name = "filtersTab";
            this.filtersTab.Size = new System.Drawing.Size(1305, 819);
            this.filtersTab.TabIndex = 15;
            this.filtersTab.Text = "Filters";
            // 
            // filterTabs
            // 
            this.filterTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterTabs.Controls.Add(this.subFilterTab);
            this.filterTabs.Controls.Add(this.subFilterText);
            this.filterTabs.Controls.Add(this.subFilterSoundMusic);
            this.filterTabs.Controls.Add(this.subFilterTargets);
            this.filterTabs.Location = new System.Drawing.Point(15, 8);
            this.filterTabs.Name = "filterTabs";
            this.filterTabs.SelectedIndex = 0;
            this.filterTabs.Size = new System.Drawing.Size(1243, 478);
            this.filterTabs.TabIndex = 1;
            this.filterTabs.SelectedIndexChanged += new System.EventHandler(this.filterTabs_IndexChanged);
            // 
            // subFilterTab
            // 
            this.subFilterTab.BackColor = System.Drawing.SystemColors.Control;
            this.subFilterTab.Controls.Add(this.wyrmAnimationList);
            this.subFilterTab.Controls.Add(this.filterWhiteWyrm);
            this.subFilterTab.Controls.Add(this.daemonAnimationList);
            this.subFilterTab.Controls.Add(this.filterDaemonGraphics);
            this.subFilterTab.Controls.Add(this.drakeAnimationList);
            this.subFilterTab.Controls.Add(this.filterDrakeGraphics);
            this.subFilterTab.Controls.Add(this.dragonAnimationList);
            this.subFilterTab.Controls.Add(this.filterDragonGraphics);
            this.subFilterTab.Controls.Add(this.filters);
            this.subFilterTab.Location = new System.Drawing.Point(10, 58);
            this.subFilterTab.Name = "subFilterTab";
            this.subFilterTab.Padding = new System.Windows.Forms.Padding(3);
            this.subFilterTab.Size = new System.Drawing.Size(1223, 410);
            this.subFilterTab.TabIndex = 0;
            this.subFilterTab.Text = "General";
            // 
            // wyrmAnimationList
            // 
            this.wyrmAnimationList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wyrmAnimationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wyrmAnimationList.DropDownWidth = 250;
            this.wyrmAnimationList.FormattingEnabled = true;
            this.wyrmAnimationList.Location = new System.Drawing.Point(782, 232);
            this.wyrmAnimationList.Name = "wyrmAnimationList";
            this.wyrmAnimationList.Size = new System.Drawing.Size(356, 49);
            this.wyrmAnimationList.TabIndex = 129;
            this.wyrmAnimationList.SelectedIndexChanged += new System.EventHandler(this.wyrmAnimationList_SelectedIndexChanged);
            // 
            // filterWhiteWyrm
            // 
            this.filterWhiteWyrm.AutoSize = true;
            this.filterWhiteWyrm.Location = new System.Drawing.Point(522, 238);
            this.filterWhiteWyrm.Name = "filterWhiteWyrm";
            this.filterWhiteWyrm.Size = new System.Drawing.Size(215, 45);
            this.filterWhiteWyrm.TabIndex = 128;
            this.filterWhiteWyrm.Text = "Filter wyrms";
            this.filterWhiteWyrm.UseVisualStyleBackColor = true;
            this.filterWhiteWyrm.CheckedChanged += new System.EventHandler(this.filterWhiteWyrm_CheckedChanged);
            // 
            // daemonAnimationList
            // 
            this.daemonAnimationList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.daemonAnimationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.daemonAnimationList.DropDownWidth = 250;
            this.daemonAnimationList.FormattingEnabled = true;
            this.daemonAnimationList.Location = new System.Drawing.Point(782, 160);
            this.daemonAnimationList.Name = "daemonAnimationList";
            this.daemonAnimationList.Size = new System.Drawing.Size(356, 49);
            this.daemonAnimationList.TabIndex = 127;
            this.daemonAnimationList.SelectedIndexChanged += new System.EventHandler(this.daemonAnimationList_SelectedIndexChanged);
            // 
            // filterDaemonGraphics
            // 
            this.filterDaemonGraphics.AutoSize = true;
            this.filterDaemonGraphics.Location = new System.Drawing.Point(522, 165);
            this.filterDaemonGraphics.Name = "filterDaemonGraphics";
            this.filterDaemonGraphics.Size = new System.Drawing.Size(252, 45);
            this.filterDaemonGraphics.TabIndex = 126;
            this.filterDaemonGraphics.Text = "Filter daemons";
            this.filterDaemonGraphics.UseVisualStyleBackColor = true;
            this.filterDaemonGraphics.CheckedChanged += new System.EventHandler(this.filterDaemonGraphics_CheckedChanged);
            // 
            // drakeAnimationList
            // 
            this.drakeAnimationList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drakeAnimationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drakeAnimationList.DropDownWidth = 250;
            this.drakeAnimationList.FormattingEnabled = true;
            this.drakeAnimationList.Location = new System.Drawing.Point(782, 88);
            this.drakeAnimationList.Name = "drakeAnimationList";
            this.drakeAnimationList.Size = new System.Drawing.Size(356, 49);
            this.drakeAnimationList.TabIndex = 118;
            this.drakeAnimationList.SelectedIndexChanged += new System.EventHandler(this.drakeAnimationList_SelectedIndexChanged);
            // 
            // filterDrakeGraphics
            // 
            this.filterDrakeGraphics.AutoSize = true;
            this.filterDrakeGraphics.Location = new System.Drawing.Point(522, 92);
            this.filterDrakeGraphics.Name = "filterDrakeGraphics";
            this.filterDrakeGraphics.Size = new System.Drawing.Size(216, 45);
            this.filterDrakeGraphics.TabIndex = 117;
            this.filterDrakeGraphics.Text = "Filter drakes";
            this.filterDrakeGraphics.UseVisualStyleBackColor = true;
            this.filterDrakeGraphics.CheckedChanged += new System.EventHandler(this.filterDrakeGraphics_CheckedChanged);
            // 
            // dragonAnimationList
            // 
            this.dragonAnimationList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dragonAnimationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dragonAnimationList.DropDownWidth = 250;
            this.dragonAnimationList.FormattingEnabled = true;
            this.dragonAnimationList.Location = new System.Drawing.Point(782, 15);
            this.dragonAnimationList.Name = "dragonAnimationList";
            this.dragonAnimationList.Size = new System.Drawing.Size(356, 49);
            this.dragonAnimationList.TabIndex = 116;
            this.dragonAnimationList.SelectedIndexChanged += new System.EventHandler(this.dragonAnimationList_SelectedIndexChanged);
            // 
            // filterDragonGraphics
            // 
            this.filterDragonGraphics.AutoSize = true;
            this.filterDragonGraphics.Location = new System.Drawing.Point(522, 20);
            this.filterDragonGraphics.Name = "filterDragonGraphics";
            this.filterDragonGraphics.Size = new System.Drawing.Size(238, 45);
            this.filterDragonGraphics.TabIndex = 115;
            this.filterDragonGraphics.Text = "Filter dragons";
            this.filterDragonGraphics.UseVisualStyleBackColor = true;
            this.filterDragonGraphics.CheckedChanged += new System.EventHandler(this.filterDragonGraphics_CheckedChanged);
            // 
            // filters
            // 
            this.filters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.filters.CheckOnClick = true;
            this.filters.IntegralHeight = false;
            this.filters.Location = new System.Drawing.Point(15, 15);
            this.filters.Name = "filters";
            this.filters.Size = new System.Drawing.Size(493, 199);
            this.filters.TabIndex = 114;
            this.filters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnFilterCheck);
            // 
            // subFilterText
            // 
            this.subFilterText.BackColor = System.Drawing.SystemColors.Control;
            this.subFilterText.Controls.Add(this.gbFilterText);
            this.subFilterText.Controls.Add(this.gbFilterMessages);
            this.subFilterText.Location = new System.Drawing.Point(10, 58);
            this.subFilterText.Name = "subFilterText";
            this.subFilterText.Size = new System.Drawing.Size(1223, 410);
            this.subFilterText.TabIndex = 4;
            this.subFilterText.Text = "Text && Messages  ";
            // 
            // gbFilterText
            // 
            this.gbFilterText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbFilterText.Controls.Add(this.removeFilterText);
            this.gbFilterText.Controls.Add(this.addFilterText);
            this.gbFilterText.Controls.Add(this.textFilterList);
            this.gbFilterText.Controls.Add(this.enableTextFilter);
            this.gbFilterText.Location = new System.Drawing.Point(8, 8);
            this.gbFilterText.Name = "gbFilterText";
            this.gbFilterText.Size = new System.Drawing.Size(572, 628);
            this.gbFilterText.TabIndex = 134;
            this.gbFilterText.TabStop = false;
            this.gbFilterText.Text = "Text Filter";
            // 
            // removeFilterText
            // 
            this.removeFilterText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeFilterText.Location = new System.Drawing.Point(370, 541);
            this.removeFilterText.Name = "removeFilterText";
            this.removeFilterText.Size = new System.Drawing.Size(188, 73);
            this.removeFilterText.TabIndex = 2;
            this.removeFilterText.Text = "Remove";
            this.removeFilterText.UseVisualStyleBackColor = true;
            this.removeFilterText.Click += new System.EventHandler(this.removeFilterText_Click);
            // 
            // addFilterText
            // 
            this.addFilterText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addFilterText.Location = new System.Drawing.Point(168, 541);
            this.addFilterText.Name = "addFilterText";
            this.addFilterText.Size = new System.Drawing.Size(187, 73);
            this.addFilterText.TabIndex = 1;
            this.addFilterText.Text = "Add";
            this.addFilterText.UseVisualStyleBackColor = true;
            this.addFilterText.Click += new System.EventHandler(this.addFilterText_Click);
            // 
            // textFilterList
            // 
            this.textFilterList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textFilterList.FormattingEnabled = true;
            this.textFilterList.ItemHeight = 41;
            this.textFilterList.Location = new System.Drawing.Point(15, 120);
            this.textFilterList.Name = "textFilterList";
            this.textFilterList.Size = new System.Drawing.Size(543, 168);
            this.textFilterList.TabIndex = 0;
            // 
            // enableTextFilter
            // 
            this.enableTextFilter.AutoSize = true;
            this.enableTextFilter.Location = new System.Drawing.Point(15, 55);
            this.enableTextFilter.Name = "enableTextFilter";
            this.enableTextFilter.Size = new System.Drawing.Size(269, 45);
            this.enableTextFilter.TabIndex = 3;
            this.enableTextFilter.Text = "Enable text filter";
            this.enableTextFilter.UseVisualStyleBackColor = true;
            this.enableTextFilter.CheckedChanged += new System.EventHandler(this.enableTextFilter_CheckedChanged);
            // 
            // gbFilterMessages
            // 
            this.gbFilterMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFilterMessages.Controls.Add(this.filterOverheadMessages);
            this.gbFilterMessages.Controls.Add(this.lblFilterDelaySeconds);
            this.gbFilterMessages.Controls.Add(this.lblFilterDelay);
            this.gbFilterMessages.Controls.Add(this.filterDelaySeconds);
            this.gbFilterMessages.Controls.Add(this.filterRazorMessages);
            this.gbFilterMessages.Controls.Add(this.filterSystemMessages);
            this.gbFilterMessages.Controls.Add(this.filterSnoop);
            this.gbFilterMessages.Location = new System.Drawing.Point(595, 8);
            this.gbFilterMessages.Name = "gbFilterMessages";
            this.gbFilterMessages.Size = new System.Drawing.Size(621, 628);
            this.gbFilterMessages.TabIndex = 133;
            this.gbFilterMessages.TabStop = false;
            this.gbFilterMessages.Text = "Filter Messages";
            // 
            // filterOverheadMessages
            // 
            this.filterOverheadMessages.Location = new System.Drawing.Point(15, 250);
            this.filterOverheadMessages.Name = "filterOverheadMessages";
            this.filterOverheadMessages.Size = new System.Drawing.Size(550, 50);
            this.filterOverheadMessages.TabIndex = 139;
            this.filterOverheadMessages.Text = "Filter repeating overhead messages";
            this.filterOverheadMessages.CheckedChanged += new System.EventHandler(this.filterOverheadMessages_CheckedChanged);
            // 
            // lblFilterDelaySeconds
            // 
            this.lblFilterDelaySeconds.Location = new System.Drawing.Point(358, 325);
            this.lblFilterDelaySeconds.Name = "lblFilterDelaySeconds";
            this.lblFilterDelaySeconds.Size = new System.Drawing.Size(147, 45);
            this.lblFilterDelaySeconds.TabIndex = 138;
            this.lblFilterDelaySeconds.Text = "seconds";
            // 
            // lblFilterDelay
            // 
            this.lblFilterDelay.AutoSize = true;
            this.lblFilterDelay.Location = new System.Drawing.Point(68, 325);
            this.lblFilterDelay.Name = "lblFilterDelay";
            this.lblFilterDelay.Size = new System.Drawing.Size(172, 41);
            this.lblFilterDelay.TabIndex = 137;
            this.lblFilterDelay.Text = "Filter Delay:";
            // 
            // filterDelaySeconds
            // 
            this.filterDelaySeconds.Location = new System.Drawing.Point(252, 318);
            this.filterDelaySeconds.Name = "filterDelaySeconds";
            this.filterDelaySeconds.Size = new System.Drawing.Size(90, 47);
            this.filterDelaySeconds.TabIndex = 136;
            this.filterDelaySeconds.Text = "3.5";
            this.filterDelaySeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.filterDelaySeconds.TextChanged += new System.EventHandler(this.filterDelaySeconds_TextChanged);
            // 
            // filterRazorMessages
            // 
            this.filterRazorMessages.Location = new System.Drawing.Point(15, 185);
            this.filterRazorMessages.Name = "filterRazorMessages";
            this.filterRazorMessages.Size = new System.Drawing.Size(550, 50);
            this.filterRazorMessages.TabIndex = 135;
            this.filterRazorMessages.Text = "Filter repeating Razor messages";
            this.filterRazorMessages.CheckedChanged += new System.EventHandler(this.filterRazorMessages_CheckedChanged);
            // 
            // filterSystemMessages
            // 
            this.filterSystemMessages.Location = new System.Drawing.Point(15, 120);
            this.filterSystemMessages.Name = "filterSystemMessages";
            this.filterSystemMessages.Size = new System.Drawing.Size(550, 50);
            this.filterSystemMessages.TabIndex = 134;
            this.filterSystemMessages.Text = "Filter repeating system messages";
            this.filterSystemMessages.CheckedChanged += new System.EventHandler(this.filterSystemMessages_CheckedChanged);
            // 
            // filterSnoop
            // 
            this.filterSnoop.Location = new System.Drawing.Point(15, 55);
            this.filterSnoop.Name = "filterSnoop";
            this.filterSnoop.Size = new System.Drawing.Size(550, 50);
            this.filterSnoop.TabIndex = 133;
            this.filterSnoop.Text = "Filter snooping messages";
            this.filterSnoop.CheckedChanged += new System.EventHandler(this.filterSnoop_CheckedChanged);
            // 
            // subFilterSoundMusic
            // 
            this.subFilterSoundMusic.BackColor = System.Drawing.SystemColors.Control;
            this.subFilterSoundMusic.Controls.Add(this.playableMusicList);
            this.subFilterSoundMusic.Controls.Add(this.playMusic);
            this.subFilterSoundMusic.Controls.Add(this.showPlayingMusic);
            this.subFilterSoundMusic.Controls.Add(this.showPlayingSoundInfo);
            this.subFilterSoundMusic.Controls.Add(this.showFilteredSound);
            this.subFilterSoundMusic.Controls.Add(this.playInClient);
            this.subFilterSoundMusic.Controls.Add(this.playSound);
            this.subFilterSoundMusic.Controls.Add(this.soundFilterEnabled);
            this.subFilterSoundMusic.Controls.Add(this.soundFilterList);
            this.subFilterSoundMusic.Location = new System.Drawing.Point(10, 58);
            this.subFilterSoundMusic.Name = "subFilterSoundMusic";
            this.subFilterSoundMusic.Size = new System.Drawing.Size(1223, 410);
            this.subFilterSoundMusic.TabIndex = 3;
            this.subFilterSoundMusic.Text = "Sound & Music  ";
            // 
            // playableMusicList
            // 
            this.playableMusicList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playableMusicList.FormattingEnabled = true;
            this.playableMusicList.Location = new System.Drawing.Point(538, 378);
            this.playableMusicList.Name = "playableMusicList";
            this.playableMusicList.Size = new System.Drawing.Size(362, 49);
            this.playableMusicList.TabIndex = 8;
            // 
            // playMusic
            // 
            this.playMusic.Location = new System.Drawing.Point(538, 450);
            this.playMusic.Name = "playMusic";
            this.playMusic.Size = new System.Drawing.Size(330, 58);
            this.playMusic.TabIndex = 7;
            this.playMusic.Text = "Play Music In Client";
            this.playMusic.UseVisualStyleBackColor = true;
            this.playMusic.Click += new System.EventHandler(this.playMusic_Click);
            // 
            // showPlayingMusic
            // 
            this.showPlayingMusic.AutoSize = true;
            this.showPlayingMusic.Location = new System.Drawing.Point(538, 538);
            this.showPlayingMusic.Name = "showPlayingMusic";
            this.showPlayingMusic.Size = new System.Drawing.Size(378, 45);
            this.showPlayingMusic.TabIndex = 6;
            this.showPlayingMusic.Text = "Show playing music info";
            this.showPlayingMusic.UseVisualStyleBackColor = true;
            this.showPlayingMusic.CheckedChanged += new System.EventHandler(this.showPlayingMusic_CheckedChanged);
            // 
            // showPlayingSoundInfo
            // 
            this.showPlayingSoundInfo.AutoSize = true;
            this.showPlayingSoundInfo.Location = new System.Drawing.Point(538, 265);
            this.showPlayingSoundInfo.Name = "showPlayingSoundInfo";
            this.showPlayingSoundInfo.Size = new System.Drawing.Size(444, 45);
            this.showPlayingSoundInfo.TabIndex = 5;
            this.showPlayingSoundInfo.Text = "Show non-filtered sound info";
            this.showPlayingSoundInfo.UseVisualStyleBackColor = true;
            this.showPlayingSoundInfo.CheckedChanged += new System.EventHandler(this.showPlayingSoundInfo_CheckedChanged);
            // 
            // showFilteredSound
            // 
            this.showFilteredSound.AutoSize = true;
            this.showFilteredSound.Location = new System.Drawing.Point(538, 202);
            this.showFilteredSound.Name = "showFilteredSound";
            this.showFilteredSound.Size = new System.Drawing.Size(380, 45);
            this.showFilteredSound.TabIndex = 4;
            this.showFilteredSound.Text = "Show filtered sound info";
            this.showFilteredSound.UseVisualStyleBackColor = true;
            this.showFilteredSound.CheckedChanged += new System.EventHandler(this.showFilteredSound_CheckedChanged);
            // 
            // playInClient
            // 
            this.playInClient.AutoSize = true;
            this.playInClient.Location = new System.Drawing.Point(870, 105);
            this.playInClient.Name = "playInClient";
            this.playInClient.Size = new System.Drawing.Size(221, 45);
            this.playInClient.TabIndex = 3;
            this.playInClient.Text = "Play in client";
            this.playInClient.UseVisualStyleBackColor = true;
            // 
            // playSound
            // 
            this.playSound.Location = new System.Drawing.Point(538, 98);
            this.playSound.Name = "playSound";
            this.playSound.Size = new System.Drawing.Size(317, 57);
            this.playSound.TabIndex = 2;
            this.playSound.Text = "Play Selected Sound";
            this.playSound.UseVisualStyleBackColor = true;
            this.playSound.Click += new System.EventHandler(this.playSound_Click);
            // 
            // soundFilterEnabled
            // 
            this.soundFilterEnabled.AutoSize = true;
            this.soundFilterEnabled.Location = new System.Drawing.Point(538, 35);
            this.soundFilterEnabled.Name = "soundFilterEnabled";
            this.soundFilterEnabled.Size = new System.Drawing.Size(311, 45);
            this.soundFilterEnabled.TabIndex = 1;
            this.soundFilterEnabled.Text = "Enable Sound Filter";
            this.soundFilterEnabled.UseVisualStyleBackColor = true;
            this.soundFilterEnabled.CheckedChanged += new System.EventHandler(this.soundFilterEnabled_CheckedChanged);
            // 
            // soundFilterList
            // 
            this.soundFilterList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.soundFilterList.FormattingEnabled = true;
            this.soundFilterList.Location = new System.Drawing.Point(20, 35);
            this.soundFilterList.Name = "soundFilterList";
            this.soundFilterList.Size = new System.Drawing.Size(502, 312);
            this.soundFilterList.TabIndex = 0;
            this.soundFilterList.SelectedIndexChanged += new System.EventHandler(this.soundFilterList_SelectedIndexChanged);
            // 
            // subFilterTargets
            // 
            this.subFilterTargets.BackColor = System.Drawing.SystemColors.Control;
            this.subFilterTargets.Controls.Add(this.lblTargetFilter);
            this.subFilterTargets.Controls.Add(this.targetFilterClear);
            this.subFilterTargets.Controls.Add(this.targetFilterRemove);
            this.subFilterTargets.Controls.Add(this.targetFilterAdd);
            this.subFilterTargets.Controls.Add(this.targetFilter);
            this.subFilterTargets.Controls.Add(this.targetFilterEnabled);
            this.subFilterTargets.Location = new System.Drawing.Point(10, 58);
            this.subFilterTargets.Name = "subFilterTargets";
            this.subFilterTargets.Padding = new System.Windows.Forms.Padding(3);
            this.subFilterTargets.Size = new System.Drawing.Size(1223, 410);
            this.subFilterTargets.TabIndex = 1;
            this.subFilterTargets.Text = "Target Filter";
            // 
            // lblTargetFilter
            // 
            this.lblTargetFilter.Location = new System.Drawing.Point(15, 210);
            this.lblTargetFilter.Name = "lblTargetFilter";
            this.lblTargetFilter.Size = new System.Drawing.Size(543, 130);
            this.lblTargetFilter.TabIndex = 18;
            this.lblTargetFilter.Text = "Targets added to this list will be ignored by Razor completely when using any tar" +
    "get hotkeys.";
            // 
            // targetFilterClear
            // 
            this.targetFilterClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.targetFilterClear.Location = new System.Drawing.Point(1016, 534);
            this.targetFilterClear.Name = "targetFilterClear";
            this.targetFilterClear.Size = new System.Drawing.Size(192, 72);
            this.targetFilterClear.TabIndex = 17;
            this.targetFilterClear.Text = "Clear List";
            this.targetFilterClear.UseVisualStyleBackColor = true;
            this.targetFilterClear.Click += new System.EventHandler(this.TargetFilterClear_Click);
            // 
            // targetFilterRemove
            // 
            this.targetFilterRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.targetFilterRemove.Location = new System.Drawing.Point(816, 534);
            this.targetFilterRemove.Name = "targetFilterRemove";
            this.targetFilterRemove.Size = new System.Drawing.Size(184, 72);
            this.targetFilterRemove.TabIndex = 16;
            this.targetFilterRemove.Text = "Remove";
            this.targetFilterRemove.UseVisualStyleBackColor = true;
            this.targetFilterRemove.Click += new System.EventHandler(this.TargetFilterRemove_Click);
            // 
            // targetFilterAdd
            // 
            this.targetFilterAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.targetFilterAdd.Location = new System.Drawing.Point(596, 534);
            this.targetFilterAdd.Name = "targetFilterAdd";
            this.targetFilterAdd.Size = new System.Drawing.Size(204, 72);
            this.targetFilterAdd.TabIndex = 15;
            this.targetFilterAdd.Text = "Add (Target)";
            this.targetFilterAdd.UseVisualStyleBackColor = true;
            this.targetFilterAdd.Click += new System.EventHandler(this.TargetFilterAdd_Click);
            // 
            // targetFilter
            // 
            this.targetFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetFilter.FormattingEnabled = true;
            this.targetFilter.ItemHeight = 41;
            this.targetFilter.Location = new System.Drawing.Point(596, 15);
            this.targetFilter.Name = "targetFilter";
            this.targetFilter.Size = new System.Drawing.Size(612, 291);
            this.targetFilter.TabIndex = 14;
            // 
            // targetFilterEnabled
            // 
            this.targetFilterEnabled.AutoSize = true;
            this.targetFilterEnabled.Location = new System.Drawing.Point(15, 15);
            this.targetFilterEnabled.Name = "targetFilterEnabled";
            this.targetFilterEnabled.Size = new System.Drawing.Size(325, 45);
            this.targetFilterEnabled.TabIndex = 13;
            this.targetFilterEnabled.Text = "Target Filter Enabled";
            this.targetFilterEnabled.UseVisualStyleBackColor = true;
            this.targetFilterEnabled.CheckedChanged += new System.EventHandler(this.TargetFilterEnabled_CheckedChanged);
            // 
            // hotkeysTab
            // 
            this.hotkeysTab.Controls.Add(this.filterHotkeys);
            this.hotkeysTab.Controls.Add(this.label22);
            this.hotkeysTab.Controls.Add(this.hkStatus);
            this.hotkeysTab.Controls.Add(this.hotkeyTree);
            this.hotkeysTab.Controls.Add(this.dohotkey);
            this.hotkeysTab.Controls.Add(this.groupBox8);
            this.hotkeysTab.Location = new System.Drawing.Point(10, 106);
            this.hotkeysTab.Name = "hotkeysTab";
            this.hotkeysTab.Size = new System.Drawing.Size(1305, 819);
            this.hotkeysTab.TabIndex = 4;
            this.hotkeysTab.Text = "Hot Keys";
            // 
            // filterHotkeys
            // 
            this.filterHotkeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterHotkeys.Location = new System.Drawing.Point(125, 20);
            this.filterHotkeys.Name = "filterHotkeys";
            this.filterHotkeys.Size = new System.Drawing.Size(673, 47);
            this.filterHotkeys.TabIndex = 9;
            this.filterHotkeys.TextChanged += new System.EventHandler(this.filterHotkeys_TextChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(20, 28);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(90, 41);
            this.label22.TabIndex = 8;
            this.label22.Text = "Filter:";
            // 
            // hkStatus
            // 
            this.hkStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hkStatus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hkStatus.Location = new System.Drawing.Point(812, 565);
            this.hkStatus.Name = "hkStatus";
            this.hkStatus.Size = new System.Drawing.Size(438, 160);
            this.hkStatus.TabIndex = 7;
            this.hkStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hotkeyTree
            // 
            this.hotkeyTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hotkeyTree.HideSelection = false;
            this.hotkeyTree.Location = new System.Drawing.Point(20, 92);
            this.hotkeyTree.Name = "hotkeyTree";
            this.hotkeyTree.Size = new System.Drawing.Size(778, 278);
            this.hotkeyTree.Sorted = true;
            this.hotkeyTree.TabIndex = 6;
            this.hotkeyTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.hotkeyTree_AfterSelect);
            // 
            // dohotkey
            // 
            this.dohotkey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dohotkey.Location = new System.Drawing.Point(812, 425);
            this.dohotkey.Name = "dohotkey";
            this.dohotkey.Size = new System.Drawing.Size(438, 73);
            this.dohotkey.TabIndex = 5;
            this.dohotkey.Text = "Execute Selected Hot Key";
            this.dohotkey.Click += new System.EventHandler(this.dohotkey_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.hkCommand);
            this.groupBox8.Controls.Add(this.hkCmdLabel);
            this.groupBox8.Controls.Add(this.chkAlt);
            this.groupBox8.Controls.Add(this.chkPass);
            this.groupBox8.Controls.Add(this.label2);
            this.groupBox8.Controls.Add(this.unsetHK);
            this.groupBox8.Controls.Add(this.setHK);
            this.groupBox8.Controls.Add(this.hkKey);
            this.groupBox8.Controls.Add(this.chkCtrl);
            this.groupBox8.Controls.Add(this.chkShift);
            this.groupBox8.Location = new System.Drawing.Point(812, 20);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(438, 390);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Hot Key";
            // 
            // hkCommand
            // 
            this.hkCommand.Location = new System.Drawing.Point(182, 182);
            this.hkCommand.Name = "hkCommand";
            this.hkCommand.Size = new System.Drawing.Size(240, 47);
            this.hkCommand.TabIndex = 11;
            // 
            // hkCmdLabel
            // 
            this.hkCmdLabel.Location = new System.Drawing.Point(20, 190);
            this.hkCmdLabel.Name = "hkCmdLabel";
            this.hkCmdLabel.Size = new System.Drawing.Size(170, 50);
            this.hkCmdLabel.TabIndex = 10;
            this.hkCmdLabel.Text = "Command";
            // 
            // chkAlt
            // 
            this.chkAlt.Location = new System.Drawing.Point(145, 50);
            this.chkAlt.Name = "chkAlt";
            this.chkAlt.Size = new System.Drawing.Size(123, 40);
            this.chkAlt.TabIndex = 2;
            this.chkAlt.Text = "Alt";
            // 
            // chkPass
            // 
            this.chkPass.Location = new System.Drawing.Point(28, 255);
            this.chkPass.Name = "chkPass";
            this.chkPass.Size = new System.Drawing.Size(282, 40);
            this.chkPass.TabIndex = 9;
            this.chkPass.Text = "Pass to UO";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 50);
            this.label2.TabIndex = 8;
            this.label2.Text = "Key:";
            // 
            // unsetHK
            // 
            this.unsetHK.Location = new System.Drawing.Point(28, 310);
            this.unsetHK.Name = "unsetHK";
            this.unsetHK.Size = new System.Drawing.Size(140, 65);
            this.unsetHK.TabIndex = 6;
            this.unsetHK.Text = "Unset";
            this.unsetHK.Click += new System.EventHandler(this.unsetHK_Click);
            // 
            // setHK
            // 
            this.setHK.Location = new System.Drawing.Point(282, 310);
            this.setHK.Name = "setHK";
            this.setHK.Size = new System.Drawing.Size(140, 65);
            this.setHK.TabIndex = 5;
            this.setHK.Text = "Set";
            this.setHK.Click += new System.EventHandler(this.setHK_Click);
            // 
            // hkKey
            // 
            this.hkKey.Location = new System.Drawing.Point(90, 100);
            this.hkKey.Name = "hkKey";
            this.hkKey.ReadOnly = true;
            this.hkKey.Size = new System.Drawing.Size(332, 47);
            this.hkKey.TabIndex = 4;
            this.hkKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hkKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.key_KeyUp);
            this.hkKey.MouseDown += new System.Windows.Forms.MouseEventHandler(this.key_MouseDown);
            this.hkKey.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.key_MouseWheel);
            // 
            // chkCtrl
            // 
            this.chkCtrl.Location = new System.Drawing.Point(20, 50);
            this.chkCtrl.Name = "chkCtrl";
            this.chkCtrl.Size = new System.Drawing.Size(140, 40);
            this.chkCtrl.TabIndex = 1;
            this.chkCtrl.Text = "Ctrl";
            // 
            // chkShift
            // 
            this.chkShift.Location = new System.Drawing.Point(282, 50);
            this.chkShift.Name = "chkShift";
            this.chkShift.Size = new System.Drawing.Size(140, 40);
            this.chkShift.TabIndex = 3;
            this.chkShift.Text = "Shift";
            // 
            // macrosTab
            // 
            this.macrosTab.Controls.Add(this.tabControl2);
            this.macrosTab.Location = new System.Drawing.Point(10, 202);
            this.macrosTab.Name = "macrosTab";
            this.macrosTab.Size = new System.Drawing.Size(1297, 713);
            this.macrosTab.TabIndex = 7;
            this.macrosTab.Text = "Macros";
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.subMacrosTab);
            this.tabControl2.Controls.Add(this.subMacrosOptionsTab);
            this.tabControl2.Location = new System.Drawing.Point(15, 8);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1245, 374);
            this.tabControl2.TabIndex = 13;
            // 
            // subMacrosTab
            // 
            this.subMacrosTab.BackColor = System.Drawing.SystemColors.Control;
            this.subMacrosTab.Controls.Add(this.macroActGroup);
            this.subMacrosTab.Controls.Add(this.filterMacros);
            this.subMacrosTab.Controls.Add(this.filterLabel);
            this.subMacrosTab.Controls.Add(this.macroTree);
            this.subMacrosTab.Controls.Add(this.delMacro);
            this.subMacrosTab.Controls.Add(this.newMacro);
            this.subMacrosTab.Location = new System.Drawing.Point(10, 58);
            this.subMacrosTab.Name = "subMacrosTab";
            this.subMacrosTab.Padding = new System.Windows.Forms.Padding(3);
            this.subMacrosTab.Size = new System.Drawing.Size(1225, 306);
            this.subMacrosTab.TabIndex = 0;
            this.subMacrosTab.Text = "Macros";
            // 
            // macroActGroup
            // 
            this.macroActGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.macroActGroup.Controls.Add(this.setMacroHotKey);
            this.macroActGroup.Controls.Add(this.playMacro);
            this.macroActGroup.Controls.Add(this.waitDisp);
            this.macroActGroup.Controls.Add(this.loopMacro);
            this.macroActGroup.Controls.Add(this.recMacro);
            this.macroActGroup.Controls.Add(this.actionList);
            this.macroActGroup.Location = new System.Drawing.Point(500, 8);
            this.macroActGroup.Name = "macroActGroup";
            this.macroActGroup.Size = new System.Drawing.Size(680, 90);
            this.macroActGroup.TabIndex = 18;
            this.macroActGroup.TabStop = false;
            this.macroActGroup.Text = "Actions";
            this.macroActGroup.Visible = false;
            // 
            // setMacroHotKey
            // 
            this.setMacroHotKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setMacroHotKey.Location = new System.Drawing.Point(515, 238);
            this.setMacroHotKey.Name = "setMacroHotKey";
            this.setMacroHotKey.Size = new System.Drawing.Size(150, 82);
            this.setMacroHotKey.TabIndex = 7;
            this.setMacroHotKey.Text = "Set HK";
            this.setMacroHotKey.Click += new System.EventHandler(this.SetMacroHotKey_Click);
            // 
            // playMacro
            // 
            this.playMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.playMacro.Location = new System.Drawing.Point(515, 42);
            this.playMacro.Name = "playMacro";
            this.playMacro.Size = new System.Drawing.Size(150, 83);
            this.playMacro.TabIndex = 6;
            this.playMacro.Text = "Play";
            this.playMacro.Click += new System.EventHandler(this.playMacro_Click);
            // 
            // waitDisp
            // 
            this.waitDisp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.waitDisp.Location = new System.Drawing.Point(515, 330);
            this.waitDisp.Name = "waitDisp";
            this.waitDisp.Size = new System.Drawing.Size(150, 222);
            this.waitDisp.TabIndex = 5;
            this.waitDisp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loopMacro
            // 
            this.loopMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loopMacro.Location = new System.Drawing.Point(522, 40);
            this.loopMacro.Name = "loopMacro";
            this.loopMacro.Size = new System.Drawing.Size(143, 60);
            this.loopMacro.TabIndex = 4;
            this.loopMacro.Text = "Loop";
            this.loopMacro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.loopMacro.CheckedChanged += new System.EventHandler(this.loopMacro_CheckedChanged);
            // 
            // recMacro
            // 
            this.recMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recMacro.Location = new System.Drawing.Point(515, 140);
            this.recMacro.Name = "recMacro";
            this.recMacro.Size = new System.Drawing.Size(150, 82);
            this.recMacro.TabIndex = 3;
            this.recMacro.Text = "Record";
            this.recMacro.Click += new System.EventHandler(this.recMacro_Click);
            // 
            // actionList
            // 
            this.actionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actionList.BackColor = System.Drawing.SystemColors.Window;
            this.actionList.HorizontalScrollbar = true;
            this.actionList.IntegralHeight = false;
            this.actionList.ItemHeight = 41;
            this.actionList.Location = new System.Drawing.Point(15, 42);
            this.actionList.Name = "actionList";
            this.actionList.Size = new System.Drawing.Size(485, 58);
            this.actionList.TabIndex = 0;
            this.actionList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.actionList_KeyDown);
            this.actionList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.actionList_MouseDown);
            // 
            // filterMacros
            // 
            this.filterMacros.Location = new System.Drawing.Point(118, 18);
            this.filterMacros.Name = "filterMacros";
            this.filterMacros.Size = new System.Drawing.Size(367, 47);
            this.filterMacros.TabIndex = 17;
            this.filterMacros.TextChanged += new System.EventHandler(this.filterMacros_TextChanged);
            // 
            // filterLabel
            // 
            this.filterLabel.AutoSize = true;
            this.filterLabel.Location = new System.Drawing.Point(12, 25);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(90, 41);
            this.filterLabel.TabIndex = 16;
            this.filterLabel.Text = "Filter:";
            // 
            // macroTree
            // 
            this.macroTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.macroTree.FullRowSelect = true;
            this.macroTree.HideSelection = false;
            this.macroTree.Location = new System.Drawing.Point(15, 90);
            this.macroTree.Name = "macroTree";
            this.macroTree.Size = new System.Drawing.Size(470, 0);
            this.macroTree.Sorted = true;
            this.macroTree.TabIndex = 15;
            this.macroTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.macroTree_AfterSelect);
            this.macroTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.macroTree_MouseDown);
            // 
            // delMacro
            // 
            this.delMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.delMacro.Location = new System.Drawing.Point(300, 22);
            this.delMacro.Name = "delMacro";
            this.delMacro.Size = new System.Drawing.Size(185, 76);
            this.delMacro.TabIndex = 14;
            this.delMacro.Text = "Remove";
            this.delMacro.Click += new System.EventHandler(this.delMacro_Click);
            // 
            // newMacro
            // 
            this.newMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newMacro.Location = new System.Drawing.Point(20, 22);
            this.newMacro.Name = "newMacro";
            this.newMacro.Size = new System.Drawing.Size(185, 76);
            this.newMacro.TabIndex = 13;
            this.newMacro.Text = "New...";
            this.newMacro.Click += new System.EventHandler(this.newMacro_Click);
            // 
            // subMacrosOptionsTab
            // 
            this.subMacrosOptionsTab.BackColor = System.Drawing.SystemColors.Control;
            this.subMacrosOptionsTab.Controls.Add(this.disableMacroPlayFinish);
            this.subMacrosOptionsTab.Controls.Add(this.macroActionDelay);
            this.subMacrosOptionsTab.Controls.Add(this.rangeCheckDoubleClick);
            this.subMacrosOptionsTab.Controls.Add(this.rangeCheckTargetByType);
            this.subMacrosOptionsTab.Controls.Add(this.nextMacroAction);
            this.subMacrosOptionsTab.Controls.Add(this.stepThroughMacro);
            this.subMacrosOptionsTab.Controls.Add(this.targetByTypeDifferent);
            this.subMacrosOptionsTab.Controls.Add(this.macroVariableGroup);
            this.subMacrosOptionsTab.Location = new System.Drawing.Point(10, 58);
            this.subMacrosOptionsTab.Name = "subMacrosOptionsTab";
            this.subMacrosOptionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.subMacrosOptionsTab.Size = new System.Drawing.Size(1225, 306);
            this.subMacrosOptionsTab.TabIndex = 1;
            this.subMacrosOptionsTab.Text = "Options";
            // 
            // disableMacroPlayFinish
            // 
            this.disableMacroPlayFinish.AutoSize = true;
            this.disableMacroPlayFinish.Location = new System.Drawing.Point(680, 458);
            this.disableMacroPlayFinish.Name = "disableMacroPlayFinish";
            this.disableMacroPlayFinish.Size = new System.Drawing.Size(505, 45);
            this.disableMacroPlayFinish.TabIndex = 17;
            this.disableMacroPlayFinish.Text = "Disable Playing/Finished Message";
            this.disableMacroPlayFinish.UseVisualStyleBackColor = true;
            this.disableMacroPlayFinish.CheckedChanged += new System.EventHandler(this.disableMacroPlayFinish_CheckedChanged);
            // 
            // macroActionDelay
            // 
            this.macroActionDelay.AutoSize = true;
            this.macroActionDelay.Location = new System.Drawing.Point(680, 395);
            this.macroActionDelay.Name = "macroActionDelay";
            this.macroActionDelay.Size = new System.Drawing.Size(507, 45);
            this.macroActionDelay.TabIndex = 16;
            this.macroActionDelay.Text = "Default macro action delay (50ms)";
            this.macroActionDelay.UseVisualStyleBackColor = true;
            this.macroActionDelay.CheckedChanged += new System.EventHandler(this.macroActionDelay_CheckedChanged);
            // 
            // rangeCheckDoubleClick
            // 
            this.rangeCheckDoubleClick.AutoSize = true;
            this.rangeCheckDoubleClick.Location = new System.Drawing.Point(680, 195);
            this.rangeCheckDoubleClick.Name = "rangeCheckDoubleClick";
            this.rangeCheckDoubleClick.Size = new System.Drawing.Size(511, 45);
            this.rangeCheckDoubleClick.TabIndex = 15;
            this.rangeCheckDoubleClick.Text = "Range check on \'DoubleClickType\'";
            this.rangeCheckDoubleClick.UseVisualStyleBackColor = true;
            this.rangeCheckDoubleClick.CheckedChanged += new System.EventHandler(this.rangeCheckDoubleClick_CheckedChanged);
            // 
            // rangeCheckTargetByType
            // 
            this.rangeCheckTargetByType.AutoSize = true;
            this.rangeCheckTargetByType.Location = new System.Drawing.Point(680, 132);
            this.rangeCheckTargetByType.Name = "rangeCheckTargetByType";
            this.rangeCheckTargetByType.Size = new System.Drawing.Size(466, 45);
            this.rangeCheckTargetByType.TabIndex = 14;
            this.rangeCheckTargetByType.Text = "Range check on \'TargetByType\'";
            this.rangeCheckTargetByType.UseVisualStyleBackColor = true;
            this.rangeCheckTargetByType.CheckedChanged += new System.EventHandler(this.rangeCheckTargetByType_CheckedChanged);
            // 
            // nextMacroAction
            // 
            this.nextMacroAction.Enabled = false;
            this.nextMacroAction.Location = new System.Drawing.Point(1030, 288);
            this.nextMacroAction.Name = "nextMacroAction";
            this.nextMacroAction.Size = new System.Drawing.Size(150, 57);
            this.nextMacroAction.TabIndex = 13;
            this.nextMacroAction.Text = "Next";
            this.nextMacroAction.UseVisualStyleBackColor = true;
            this.nextMacroAction.Click += new System.EventHandler(this.nextMacroAction_Click);
            // 
            // stepThroughMacro
            // 
            this.stepThroughMacro.AutoSize = true;
            this.stepThroughMacro.Location = new System.Drawing.Point(680, 295);
            this.stepThroughMacro.Name = "stepThroughMacro";
            this.stepThroughMacro.Size = new System.Drawing.Size(328, 45);
            this.stepThroughMacro.TabIndex = 12;
            this.stepThroughMacro.Text = "Step Through Macro";
            this.stepThroughMacro.UseVisualStyleBackColor = true;
            this.stepThroughMacro.CheckedChanged += new System.EventHandler(this.stepThroughMacro_CheckedChanged);
            // 
            // targetByTypeDifferent
            // 
            this.targetByTypeDifferent.AutoSize = true;
            this.targetByTypeDifferent.Location = new System.Drawing.Point(680, 70);
            this.targetByTypeDifferent.Name = "targetByTypeDifferent";
            this.targetByTypeDifferent.Size = new System.Drawing.Size(448, 45);
            this.targetByTypeDifferent.TabIndex = 11;
            this.targetByTypeDifferent.Text = "Force different \'TargetByType\'";
            this.targetByTypeDifferent.UseVisualStyleBackColor = true;
            this.targetByTypeDifferent.CheckedChanged += new System.EventHandler(this.targetByTypeDifferent_CheckedChanged);
            // 
            // macroVariableGroup
            // 
            this.macroVariableGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.macroVariableGroup.Controls.Add(this.macroVariableTypeList);
            this.macroVariableGroup.Controls.Add(this.retargetMacroVariable);
            this.macroVariableGroup.Controls.Add(this.insertMacroVariable);
            this.macroVariableGroup.Controls.Add(this.removeMacroVariable);
            this.macroVariableGroup.Controls.Add(this.addMacroVariable);
            this.macroVariableGroup.Controls.Add(this.macroVariables);
            this.macroVariableGroup.Location = new System.Drawing.Point(15, 15);
            this.macroVariableGroup.Name = "macroVariableGroup";
            this.macroVariableGroup.Size = new System.Drawing.Size(600, 597);
            this.macroVariableGroup.TabIndex = 6;
            this.macroVariableGroup.TabStop = false;
            this.macroVariableGroup.Text = "Macro Variables:";
            // 
            // macroVariableTypeList
            // 
            this.macroVariableTypeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.macroVariableTypeList.FormattingEnabled = true;
            this.macroVariableTypeList.Items.AddRange(new object[] {
            "Absolute Target",
            "DoubleClick Target",
            "Set Macro Variable Action"});
            this.macroVariableTypeList.Location = new System.Drawing.Point(198, 55);
            this.macroVariableTypeList.Name = "macroVariableTypeList";
            this.macroVariableTypeList.Size = new System.Drawing.Size(382, 49);
            this.macroVariableTypeList.TabIndex = 6;
            this.macroVariableTypeList.SelectedIndexChanged += new System.EventHandler(this.macroVariableTypeList_SelectedIndexChanged);
            // 
            // retargetMacroVariable
            // 
            this.retargetMacroVariable.Location = new System.Drawing.Point(15, 210);
            this.retargetMacroVariable.Name = "retargetMacroVariable";
            this.retargetMacroVariable.Size = new System.Drawing.Size(167, 62);
            this.retargetMacroVariable.TabIndex = 5;
            this.retargetMacroVariable.Text = "Retarget";
            this.retargetMacroVariable.UseVisualStyleBackColor = true;
            this.retargetMacroVariable.Click += new System.EventHandler(this.retargetMacroVariable_Click);
            // 
            // insertMacroVariable
            // 
            this.insertMacroVariable.Location = new System.Drawing.Point(15, 55);
            this.insertMacroVariable.Name = "insertMacroVariable";
            this.insertMacroVariable.Size = new System.Drawing.Size(167, 63);
            this.insertMacroVariable.TabIndex = 4;
            this.insertMacroVariable.Text = "Insert as...";
            this.insertMacroVariable.UseVisualStyleBackColor = true;
            this.insertMacroVariable.Click += new System.EventHandler(this.insertMacroVariable_Click);
            // 
            // removeMacroVariable
            // 
            this.removeMacroVariable.Location = new System.Drawing.Point(15, 288);
            this.removeMacroVariable.Name = "removeMacroVariable";
            this.removeMacroVariable.Size = new System.Drawing.Size(167, 62);
            this.removeMacroVariable.TabIndex = 3;
            this.removeMacroVariable.Text = "Remove";
            this.removeMacroVariable.UseVisualStyleBackColor = true;
            this.removeMacroVariable.Click += new System.EventHandler(this.removeMacroVariable_Click);
            // 
            // addMacroVariable
            // 
            this.addMacroVariable.Location = new System.Drawing.Point(15, 132);
            this.addMacroVariable.Name = "addMacroVariable";
            this.addMacroVariable.Size = new System.Drawing.Size(167, 63);
            this.addMacroVariable.TabIndex = 2;
            this.addMacroVariable.Text = "Add";
            this.addMacroVariable.UseVisualStyleBackColor = true;
            this.addMacroVariable.Click += new System.EventHandler(this.addMacroVariable_Click);
            // 
            // macroVariables
            // 
            this.macroVariables.FormattingEnabled = true;
            this.macroVariables.ItemHeight = 41;
            this.macroVariables.Location = new System.Drawing.Point(198, 132);
            this.macroVariables.Name = "macroVariables";
            this.macroVariables.Size = new System.Drawing.Size(382, 414);
            this.macroVariables.TabIndex = 1;
            // 
            // scriptsTab
            // 
            this.scriptsTab.BackColor = System.Drawing.SystemColors.Control;
            this.scriptsTab.Controls.Add(this.subTabScripts);
            this.scriptsTab.Location = new System.Drawing.Point(10, 202);
            this.scriptsTab.Name = "scriptsTab";
            this.scriptsTab.Size = new System.Drawing.Size(1297, 713);
            this.scriptsTab.TabIndex = 13;
            this.scriptsTab.Text = "Scripts";
            // 
            // subTabScripts
            // 
            this.subTabScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subTabScripts.Controls.Add(this.subScripts);
            this.subTabScripts.Controls.Add(this.subScriptOptions);
            this.subTabScripts.Location = new System.Drawing.Point(15, 8);
            this.subTabScripts.Name = "subTabScripts";
            this.subTabScripts.SelectedIndex = 0;
            this.subTabScripts.Size = new System.Drawing.Size(1235, 372);
            this.subTabScripts.TabIndex = 14;
            // 
            // subScripts
            // 
            this.subScripts.BackColor = System.Drawing.SystemColors.Control;
            this.subScripts.Controls.Add(this.scriptHotkey);
            this.subScripts.Controls.Add(this.scriptSplitContainer);
            this.subScripts.Controls.Add(this.scriptGuide);
            this.subScripts.Controls.Add(this.saveScript);
            this.subScripts.Controls.Add(this.newScript);
            this.subScripts.Controls.Add(this.setScriptHotkey);
            this.subScripts.Controls.Add(this.recordScript);
            this.subScripts.Controls.Add(this.playScript);
            this.subScripts.Location = new System.Drawing.Point(10, 58);
            this.subScripts.Name = "subScripts";
            this.subScripts.Padding = new System.Windows.Forms.Padding(3);
            this.subScripts.Size = new System.Drawing.Size(1215, 304);
            this.subScripts.TabIndex = 0;
            this.subScripts.Text = "Scripts";
            // 
            // scriptHotkey
            // 
            this.scriptHotkey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptHotkey.ForeColor = System.Drawing.SystemColors.ControlText;
            this.scriptHotkey.Location = new System.Drawing.Point(1020, 420);
            this.scriptHotkey.Name = "scriptHotkey";
            this.scriptHotkey.Size = new System.Drawing.Size(150, 88);
            this.scriptHotkey.TabIndex = 29;
            this.scriptHotkey.Text = "Not Set";
            this.scriptHotkey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scriptSplitContainer
            // 
            this.scriptSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptSplitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.scriptSplitContainer.Location = new System.Drawing.Point(18, 18);
            this.scriptSplitContainer.Name = "scriptSplitContainer";
            // 
            // scriptSplitContainer.Panel1
            // 
            this.scriptSplitContainer.Panel1.Controls.Add(this.scriptTree);
            this.scriptSplitContainer.Panel1.Controls.Add(this.scriptFilter);
            // 
            // scriptSplitContainer.Panel2
            // 
            this.scriptSplitContainer.Panel2.Controls.Add(this.scriptDocMap);
            this.scriptSplitContainer.Panel2.Controls.Add(this.scriptEditor);
            this.scriptSplitContainer.Size = new System.Drawing.Size(987, 144);
            this.scriptSplitContainer.SplitterDistance = 259;
            this.scriptSplitContainer.SplitterWidth = 10;
            this.scriptSplitContainer.TabIndex = 28;
            // 
            // scriptTree
            // 
            this.scriptTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptTree.Location = new System.Drawing.Point(0, 78);
            this.scriptTree.Name = "scriptTree";
            this.scriptTree.Size = new System.Drawing.Size(259, 66);
            this.scriptTree.Sorted = true;
            this.scriptTree.TabIndex = 29;
            this.scriptTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.scriptTree_AfterSelect);
            this.scriptTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scriptTree_MouseDown);
            // 
            // scriptFilter
            // 
            this.scriptFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptFilter.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scriptFilter.Location = new System.Drawing.Point(0, 0);
            this.scriptFilter.Name = "scriptFilter";
            this.scriptFilter.Size = new System.Drawing.Size(259, 51);
            this.scriptFilter.TabIndex = 28;
            this.scriptFilter.TextChanged += new System.EventHandler(this.scriptFilter_TextChanged);
            // 
            // scriptDocMap
            // 
            this.scriptDocMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptDocMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(37)))), ((int)(((byte)(56)))));
            this.scriptDocMap.ForeColor = System.Drawing.Color.Maroon;
            this.scriptDocMap.Location = new System.Drawing.Point(394, -10);
            this.scriptDocMap.Name = "scriptDocMap";
            this.scriptDocMap.Size = new System.Drawing.Size(300, 154);
            this.scriptDocMap.TabIndex = 22;
            this.scriptDocMap.Target = null;
            // 
            // scriptEditor
            // 
            this.scriptEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.scriptEditor.AutoScrollMinSize = new System.Drawing.Size(2, 38);
            this.scriptEditor.BackBrush = null;
            this.scriptEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(37)))), ((int)(((byte)(56)))));
            this.scriptEditor.CaretColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.scriptEditor.CharHeight = 38;
            this.scriptEditor.CharWidth = 18;
            this.scriptEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.scriptEditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.scriptEditor.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.scriptEditor.ForeColor = System.Drawing.Color.White;
            this.scriptEditor.IsReplaceMode = false;
            this.scriptEditor.LeftBracket = '(';
            this.scriptEditor.LeftBracket2 = '[';
            this.scriptEditor.LineNumberColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(144)))), ((int)(((byte)(175)))));
            this.scriptEditor.Location = new System.Drawing.Point(-10, 0);
            this.scriptEditor.Name = "scriptEditor";
            this.scriptEditor.Paddings = new System.Windows.Forms.Padding(0);
            this.scriptEditor.RightBracket = ')';
            this.scriptEditor.RightBracket2 = ']';
            this.scriptEditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.scriptEditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("scriptEditor.ServiceColors")));
            this.scriptEditor.ShowCaretWhenInactive = false;
            this.scriptEditor.Size = new System.Drawing.Size(390, 144);
            this.scriptEditor.TabIndex = 21;
            this.scriptEditor.Zoom = 100;
            this.scriptEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.scriptEditor_KeyDown);
            this.scriptEditor.LostFocus += new System.EventHandler(this.scriptEditor_LostFocus);
            this.scriptEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scriptEditor_MouseDown);
            // 
            // scriptGuide
            // 
            this.scriptGuide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptGuide.AutoSize = true;
            this.scriptGuide.Location = new System.Drawing.Point(945, 35);
            this.scriptGuide.Name = "scriptGuide";
            this.scriptGuide.Size = new System.Drawing.Size(225, 127);
            this.scriptGuide.TabIndex = 26;
            this.scriptGuide.Text = "Help";
            this.scriptGuide.UseVisualStyleBackColor = true;
            this.scriptGuide.Click += new System.EventHandler(this.scriptGuide_Click);
            // 
            // saveScript
            // 
            this.saveScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveScript.AutoSize = true;
            this.saveScript.Location = new System.Drawing.Point(948, 208);
            this.saveScript.Name = "saveScript";
            this.saveScript.Size = new System.Drawing.Size(222, 127);
            this.saveScript.TabIndex = 22;
            this.saveScript.Text = "Save";
            this.saveScript.UseVisualStyleBackColor = true;
            this.saveScript.Click += new System.EventHandler(this.saveScript_Click);
            // 
            // newScript
            // 
            this.newScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newScript.AutoSize = true;
            this.newScript.Location = new System.Drawing.Point(950, 528);
            this.newScript.Name = "newScript";
            this.newScript.Size = new System.Drawing.Size(220, 127);
            this.newScript.TabIndex = 18;
            this.newScript.Text = "New";
            this.newScript.UseVisualStyleBackColor = true;
            this.newScript.Click += new System.EventHandler(this.newScript_Click);
            // 
            // setScriptHotkey
            // 
            this.setScriptHotkey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setScriptHotkey.AutoSize = true;
            this.setScriptHotkey.Location = new System.Drawing.Point(880, 332);
            this.setScriptHotkey.Name = "setScriptHotkey";
            this.setScriptHotkey.Size = new System.Drawing.Size(290, 128);
            this.setScriptHotkey.TabIndex = 16;
            this.setScriptHotkey.Text = "Set HK";
            this.setScriptHotkey.UseVisualStyleBackColor = true;
            this.setScriptHotkey.Click += new System.EventHandler(this.setScriptHotkey_Click);
            // 
            // recordScript
            // 
            this.recordScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recordScript.AutoSize = true;
            this.recordScript.Location = new System.Drawing.Point(868, 112);
            this.recordScript.Name = "recordScript";
            this.recordScript.Size = new System.Drawing.Size(302, 128);
            this.recordScript.TabIndex = 15;
            this.recordScript.Text = "Record";
            this.recordScript.UseVisualStyleBackColor = true;
            this.recordScript.Click += new System.EventHandler(this.recordScript_Click);
            // 
            // playScript
            // 
            this.playScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.playScript.AutoSize = true;
            this.playScript.Location = new System.Drawing.Point(965, 18);
            this.playScript.Name = "playScript";
            this.playScript.Size = new System.Drawing.Size(205, 127);
            this.playScript.TabIndex = 14;
            this.playScript.Text = "Play";
            this.playScript.UseVisualStyleBackColor = true;
            this.playScript.Click += new System.EventHandler(this.playScript_Click);
            // 
            // subScriptOptions
            // 
            this.subScriptOptions.BackColor = System.Drawing.SystemColors.Control;
            this.subScriptOptions.Controls.Add(this.disableScriptStopwatch);
            this.subScriptOptions.Controls.Add(this.enableHighlight);
            this.subScriptOptions.Controls.Add(this.defaultScriptDelay);
            this.subScriptOptions.Controls.Add(this.disableScriptTooltips);
            this.subScriptOptions.Controls.Add(this.scriptDisablePlayFinish);
            this.subScriptOptions.Controls.Add(this.autoSaveScriptPlay);
            this.subScriptOptions.Controls.Add(this.autoSaveScript);
            this.subScriptOptions.Controls.Add(this.scriptVariablesBox);
            this.subScriptOptions.Location = new System.Drawing.Point(10, 58);
            this.subScriptOptions.Name = "subScriptOptions";
            this.subScriptOptions.Padding = new System.Windows.Forms.Padding(3);
            this.subScriptOptions.Size = new System.Drawing.Size(1215, 304);
            this.subScriptOptions.TabIndex = 1;
            this.subScriptOptions.Text = "Options";
            // 
            // disableScriptStopwatch
            // 
            this.disableScriptStopwatch.AutoSize = true;
            this.disableScriptStopwatch.Location = new System.Drawing.Point(702, 288);
            this.disableScriptStopwatch.Name = "disableScriptStopwatch";
            this.disableScriptStopwatch.Size = new System.Drawing.Size(455, 45);
            this.disableScriptStopwatch.TabIndex = 23;
            this.disableScriptStopwatch.Text = "Disable execution time output";
            this.disableScriptStopwatch.UseVisualStyleBackColor = true;
            this.disableScriptStopwatch.CheckedChanged += new System.EventHandler(this.disableScriptStopwatch_CheckedChanged);
            // 
            // enableHighlight
            // 
            this.enableHighlight.AutoSize = true;
            this.enableHighlight.Location = new System.Drawing.Point(630, 475);
            this.enableHighlight.Name = "enableHighlight";
            this.enableHighlight.Size = new System.Drawing.Size(419, 45);
            this.enableHighlight.TabIndex = 22;
            this.enableHighlight.Text = "Enable highlighting per line";
            this.enableHighlight.UseVisualStyleBackColor = true;
            this.enableHighlight.CheckedChanged += new System.EventHandler(this.enableHighlight_CheckedChanged);
            // 
            // defaultScriptDelay
            // 
            this.defaultScriptDelay.AutoSize = true;
            this.defaultScriptDelay.Location = new System.Drawing.Point(630, 412);
            this.defaultScriptDelay.Name = "defaultScriptDelay";
            this.defaultScriptDelay.Size = new System.Drawing.Size(496, 45);
            this.defaultScriptDelay.TabIndex = 21;
            this.defaultScriptDelay.Text = "Default script action delay (25ms)";
            this.defaultScriptDelay.UseVisualStyleBackColor = true;
            this.defaultScriptDelay.CheckedChanged += new System.EventHandler(this.defaultScriptDelay_CheckedChanged);
            // 
            // disableScriptTooltips
            // 
            this.disableScriptTooltips.AutoSize = true;
            this.disableScriptTooltips.Location = new System.Drawing.Point(630, 350);
            this.disableScriptTooltips.Name = "disableScriptTooltips";
            this.disableScriptTooltips.Size = new System.Drawing.Size(262, 45);
            this.disableScriptTooltips.TabIndex = 20;
            this.disableScriptTooltips.Text = "Disable tooltips";
            this.disableScriptTooltips.UseVisualStyleBackColor = true;
            this.disableScriptTooltips.CheckedChanged += new System.EventHandler(this.disableScriptTooltips_CheckedChanged);
            // 
            // scriptDisablePlayFinish
            // 
            this.scriptDisablePlayFinish.AutoSize = true;
            this.scriptDisablePlayFinish.Location = new System.Drawing.Point(630, 225);
            this.scriptDisablePlayFinish.Name = "scriptDisablePlayFinish";
            this.scriptDisablePlayFinish.Size = new System.Drawing.Size(505, 45);
            this.scriptDisablePlayFinish.TabIndex = 19;
            this.scriptDisablePlayFinish.Text = "Disable Playing/Finished Message";
            this.scriptDisablePlayFinish.UseVisualStyleBackColor = true;
            this.scriptDisablePlayFinish.CheckedChanged += new System.EventHandler(this.disableScriptPlayFinish_CheckedChanged);
            // 
            // autoSaveScriptPlay
            // 
            this.autoSaveScriptPlay.AutoSize = true;
            this.autoSaveScriptPlay.Location = new System.Drawing.Point(630, 132);
            this.autoSaveScriptPlay.Name = "autoSaveScriptPlay";
            this.autoSaveScriptPlay.Size = new System.Drawing.Size(469, 45);
            this.autoSaveScriptPlay.TabIndex = 9;
            this.autoSaveScriptPlay.Text = "Auto-save when you click \'Play\'";
            this.autoSaveScriptPlay.UseVisualStyleBackColor = true;
            this.autoSaveScriptPlay.CheckedChanged += new System.EventHandler(this.autoSaveScriptPlay_CheckedChanged);
            // 
            // autoSaveScript
            // 
            this.autoSaveScript.AutoSize = true;
            this.autoSaveScript.Location = new System.Drawing.Point(630, 70);
            this.autoSaveScript.Name = "autoSaveScript";
            this.autoSaveScript.Size = new System.Drawing.Size(591, 45);
            this.autoSaveScript.TabIndex = 8;
            this.autoSaveScript.Text = "Auto-save when script editor loses focus";
            this.autoSaveScript.UseVisualStyleBackColor = true;
            this.autoSaveScript.CheckedChanged += new System.EventHandler(this.autoSaveScript_CheckedChanged);
            // 
            // scriptVariablesBox
            // 
            this.scriptVariablesBox.Controls.Add(this.changeScriptVariable);
            this.scriptVariablesBox.Controls.Add(this.removeScriptVariable);
            this.scriptVariablesBox.Controls.Add(this.addScriptVariable);
            this.scriptVariablesBox.Controls.Add(this.scriptVariables);
            this.scriptVariablesBox.Location = new System.Drawing.Point(15, 15);
            this.scriptVariablesBox.Name = "scriptVariablesBox";
            this.scriptVariablesBox.Size = new System.Drawing.Size(600, 673);
            this.scriptVariablesBox.TabIndex = 7;
            this.scriptVariablesBox.TabStop = false;
            this.scriptVariablesBox.Text = "Script Variables:";
            // 
            // changeScriptVariable
            // 
            this.changeScriptVariable.Location = new System.Drawing.Point(15, 132);
            this.changeScriptVariable.Name = "changeScriptVariable";
            this.changeScriptVariable.Size = new System.Drawing.Size(167, 63);
            this.changeScriptVariable.TabIndex = 5;
            this.changeScriptVariable.Text = "Retarget";
            this.changeScriptVariable.UseVisualStyleBackColor = true;
            this.changeScriptVariable.Click += new System.EventHandler(this.changeScriptVariable_Click);
            // 
            // removeScriptVariable
            // 
            this.removeScriptVariable.Location = new System.Drawing.Point(15, 210);
            this.removeScriptVariable.Name = "removeScriptVariable";
            this.removeScriptVariable.Size = new System.Drawing.Size(167, 62);
            this.removeScriptVariable.TabIndex = 3;
            this.removeScriptVariable.Text = "Remove";
            this.removeScriptVariable.UseVisualStyleBackColor = true;
            this.removeScriptVariable.Click += new System.EventHandler(this.removeScriptVariable_Click);
            // 
            // addScriptVariable
            // 
            this.addScriptVariable.Location = new System.Drawing.Point(15, 55);
            this.addScriptVariable.Name = "addScriptVariable";
            this.addScriptVariable.Size = new System.Drawing.Size(167, 63);
            this.addScriptVariable.TabIndex = 2;
            this.addScriptVariable.Text = "Add";
            this.addScriptVariable.UseVisualStyleBackColor = true;
            this.addScriptVariable.Click += new System.EventHandler(this.addScriptVariable_Click);
            // 
            // scriptVariables
            // 
            this.scriptVariables.FormattingEnabled = true;
            this.scriptVariables.ItemHeight = 41;
            this.scriptVariables.Location = new System.Drawing.Point(198, 55);
            this.scriptVariables.Name = "scriptVariables";
            this.scriptVariables.Size = new System.Drawing.Size(382, 496);
            this.scriptVariables.TabIndex = 1;
            // 
            // friendsTab
            // 
            this.friendsTab.BackColor = System.Drawing.SystemColors.Control;
            this.friendsTab.Controls.Add(this.showPartyFriendOverhead);
            this.friendsTab.Controls.Add(this.highlightFriend);
            this.friendsTab.Controls.Add(this.autoAcceptParty);
            this.friendsTab.Controls.Add(this.nextPrevIgnoresFriends);
            this.friendsTab.Controls.Add(this.autoFriend);
            this.friendsTab.Controls.Add(this.friendsGroupBox);
            this.friendsTab.Location = new System.Drawing.Point(10, 202);
            this.friendsTab.Name = "friendsTab";
            this.friendsTab.Size = new System.Drawing.Size(1297, 713);
            this.friendsTab.TabIndex = 14;
            this.friendsTab.Text = "Friends";
            // 
            // showPartyFriendOverhead
            // 
            this.showPartyFriendOverhead.Location = new System.Drawing.Point(708, 100);
            this.showPartyFriendOverhead.Name = "showPartyFriendOverhead";
            this.showPartyFriendOverhead.Size = new System.Drawing.Size(542, 50);
            this.showPartyFriendOverhead.TabIndex = 144;
            this.showPartyFriendOverhead.Text = "Show [Party-Friend] overhead";
            this.showPartyFriendOverhead.CheckedChanged += new System.EventHandler(this.showPartyFriendOverhead_CheckedChanged);
            // 
            // highlightFriend
            // 
            this.highlightFriend.Location = new System.Drawing.Point(675, 292);
            this.highlightFriend.Name = "highlightFriend";
            this.highlightFriend.Size = new System.Drawing.Size(460, 58);
            this.highlightFriend.TabIndex = 143;
            this.highlightFriend.Text = "Next/Prev highlights \'Friends\'";
            this.highlightFriend.UseVisualStyleBackColor = true;
            this.highlightFriend.CheckedChanged += new System.EventHandler(this.highlightFriend_CheckedChanged);
            // 
            // autoAcceptParty
            // 
            this.autoAcceptParty.Location = new System.Drawing.Point(675, 228);
            this.autoAcceptParty.Name = "autoAcceptParty";
            this.autoAcceptParty.Size = new System.Drawing.Size(575, 50);
            this.autoAcceptParty.TabIndex = 138;
            this.autoAcceptParty.Text = "Auto-accept party invites from friends";
            this.autoAcceptParty.CheckedChanged += new System.EventHandler(this.autoAcceptParty_CheckedChanged);
            // 
            // nextPrevIgnoresFriends
            // 
            this.nextPrevIgnoresFriends.AutoSize = true;
            this.nextPrevIgnoresFriends.Location = new System.Drawing.Point(675, 165);
            this.nextPrevIgnoresFriends.Name = "nextPrevIgnoresFriends";
            this.nextPrevIgnoresFriends.Size = new System.Drawing.Size(502, 45);
            this.nextPrevIgnoresFriends.TabIndex = 137;
            this.nextPrevIgnoresFriends.Text = "Next/Prev Target ignores \'Friends\'";
            this.nextPrevIgnoresFriends.UseVisualStyleBackColor = true;
            this.nextPrevIgnoresFriends.CheckedChanged += new System.EventHandler(this.nextPrevIgnoresFriends_CheckedChanged);
            // 
            // autoFriend
            // 
            this.autoFriend.Location = new System.Drawing.Point(675, 35);
            this.autoFriend.Name = "autoFriend";
            this.autoFriend.Size = new System.Drawing.Size(543, 50);
            this.autoFriend.TabIndex = 136;
            this.autoFriend.Text = "Treat party members as \'Friends\'";
            this.autoFriend.CheckedChanged += new System.EventHandler(this.autoFriend_CheckedChanged);
            // 
            // friendsGroupBox
            // 
            this.friendsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.friendsGroupBox.Controls.Add(this.showFriendOverhead);
            this.friendsGroupBox.Controls.Add(this.setFriendsFormatHue);
            this.friendsGroupBox.Controls.Add(this.friendOverheadFormat);
            this.friendsGroupBox.Controls.Add(this.friendFormat);
            this.friendsGroupBox.Controls.Add(this.friendsGroupRemove);
            this.friendsGroupBox.Controls.Add(this.friendsGroupAdd);
            this.friendsGroupBox.Controls.Add(this.friendClearList);
            this.friendsGroupBox.Controls.Add(this.friendRemoveSelected);
            this.friendsGroupBox.Controls.Add(this.friendAddTarget);
            this.friendsGroupBox.Controls.Add(this.friendsList);
            this.friendsGroupBox.Controls.Add(this.friendsGroup);
            this.friendsGroupBox.Controls.Add(this.friendsListEnabled);
            this.friendsGroupBox.Location = new System.Drawing.Point(15, 8);
            this.friendsGroupBox.Name = "friendsGroupBox";
            this.friendsGroupBox.Size = new System.Drawing.Size(645, 372);
            this.friendsGroupBox.TabIndex = 135;
            this.friendsGroupBox.TabStop = false;
            this.friendsGroupBox.Text = "Friends Groups && Lists";
            // 
            // showFriendOverhead
            // 
            this.showFriendOverhead.Location = new System.Drawing.Point(15, 130);
            this.showFriendOverhead.Name = "showFriendOverhead";
            this.showFriendOverhead.Size = new System.Drawing.Size(460, 58);
            this.showFriendOverhead.TabIndex = 146;
            this.showFriendOverhead.Text = "Show overhead tag";
            this.showFriendOverhead.UseVisualStyleBackColor = true;
            this.showFriendOverhead.CheckedChanged += new System.EventHandler(this.showFriendOverhead_CheckedChanged);
            // 
            // setFriendsFormatHue
            // 
            this.setFriendsFormatHue.Location = new System.Drawing.Point(408, 195);
            this.setFriendsFormatHue.Name = "setFriendsFormatHue";
            this.setFriendsFormatHue.Size = new System.Drawing.Size(147, 57);
            this.setFriendsFormatHue.TabIndex = 145;
            this.setFriendsFormatHue.Text = "Set Hue";
            this.setFriendsFormatHue.UseVisualStyleBackColor = true;
            this.setFriendsFormatHue.Click += new System.EventHandler(this.setFriendsFormatHue_Click);
            // 
            // friendOverheadFormat
            // 
            this.friendOverheadFormat.Location = new System.Drawing.Point(138, 195);
            this.friendOverheadFormat.Name = "friendOverheadFormat";
            this.friendOverheadFormat.Size = new System.Drawing.Size(254, 47);
            this.friendOverheadFormat.TabIndex = 143;
            this.friendOverheadFormat.Text = "[Friend]";
            this.friendOverheadFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.friendOverheadFormat.TextChanged += new System.EventHandler(this.friendOverheadFormat_TextChanged);
            // 
            // friendFormat
            // 
            this.friendFormat.Location = new System.Drawing.Point(15, 195);
            this.friendFormat.Name = "friendFormat";
            this.friendFormat.Size = new System.Drawing.Size(377, 57);
            this.friendFormat.TabIndex = 144;
            this.friendFormat.Text = "Format:";
            this.friendFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // friendsGroupRemove
            // 
            this.friendsGroupRemove.Location = new System.Drawing.Point(392, 52);
            this.friendsGroupRemove.Name = "friendsGroupRemove";
            this.friendsGroupRemove.Size = new System.Drawing.Size(53, 63);
            this.friendsGroupRemove.TabIndex = 11;
            this.friendsGroupRemove.Text = "-";
            this.friendsGroupRemove.UseVisualStyleBackColor = true;
            this.friendsGroupRemove.Click += new System.EventHandler(this.friendsGroupRemove_Click);
            // 
            // friendsGroupAdd
            // 
            this.friendsGroupAdd.Location = new System.Drawing.Point(325, 52);
            this.friendsGroupAdd.Name = "friendsGroupAdd";
            this.friendsGroupAdd.Size = new System.Drawing.Size(53, 63);
            this.friendsGroupAdd.TabIndex = 10;
            this.friendsGroupAdd.Text = "+";
            this.friendsGroupAdd.UseVisualStyleBackColor = true;
            this.friendsGroupAdd.Click += new System.EventHandler(this.friendsGroupAdd_Click);
            // 
            // friendClearList
            // 
            this.friendClearList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.friendClearList.Location = new System.Drawing.Point(435, 275);
            this.friendClearList.Name = "friendClearList";
            this.friendClearList.Size = new System.Drawing.Size(197, 83);
            this.friendClearList.TabIndex = 8;
            this.friendClearList.Text = "Clear List";
            this.friendClearList.UseVisualStyleBackColor = true;
            this.friendClearList.Click += new System.EventHandler(this.friendClearList_Click);
            // 
            // friendRemoveSelected
            // 
            this.friendRemoveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.friendRemoveSelected.Location = new System.Drawing.Point(235, 275);
            this.friendRemoveSelected.Name = "friendRemoveSelected";
            this.friendRemoveSelected.Size = new System.Drawing.Size(185, 83);
            this.friendRemoveSelected.TabIndex = 7;
            this.friendRemoveSelected.Text = "Remove";
            this.friendRemoveSelected.UseVisualStyleBackColor = true;
            this.friendRemoveSelected.Click += new System.EventHandler(this.friendRemoveSelected_Click);
            // 
            // friendAddTarget
            // 
            this.friendAddTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.friendAddTarget.Location = new System.Drawing.Point(15, 275);
            this.friendAddTarget.Name = "friendAddTarget";
            this.friendAddTarget.Size = new System.Drawing.Size(205, 83);
            this.friendAddTarget.TabIndex = 5;
            this.friendAddTarget.Text = "Add (Target)";
            this.friendAddTarget.UseVisualStyleBackColor = true;
            this.friendAddTarget.Click += new System.EventHandler(this.FriendAddTarget_Click);
            // 
            // friendsList
            // 
            this.friendsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.friendsList.FormattingEnabled = true;
            this.friendsList.ItemHeight = 41;
            this.friendsList.Location = new System.Drawing.Point(12, 285);
            this.friendsList.Name = "friendsList";
            this.friendsList.Size = new System.Drawing.Size(618, 4);
            this.friendsList.TabIndex = 4;
            this.friendsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.friendsList_KeyDown);
            this.friendsList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.friendsList_MouseDown);
            // 
            // friendsGroup
            // 
            this.friendsGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.friendsGroup.FormattingEnabled = true;
            this.friendsGroup.Location = new System.Drawing.Point(15, 55);
            this.friendsGroup.Name = "friendsGroup";
            this.friendsGroup.Size = new System.Drawing.Size(295, 49);
            this.friendsGroup.TabIndex = 3;
            this.friendsGroup.SelectedIndexChanged += new System.EventHandler(this.friendsGroup_SelectedIndexChanged);
            // 
            // friendsListEnabled
            // 
            this.friendsListEnabled.Location = new System.Drawing.Point(460, 58);
            this.friendsListEnabled.Name = "friendsListEnabled";
            this.friendsListEnabled.Size = new System.Drawing.Size(172, 50);
            this.friendsListEnabled.TabIndex = 9;
            this.friendsListEnabled.Text = "Enabled";
            this.friendsListEnabled.UseVisualStyleBackColor = true;
            this.friendsListEnabled.CheckedChanged += new System.EventHandler(this.friendsListEnabled_CheckedChanged);
            // 
            // screenshotTab
            // 
            this.screenshotTab.Controls.Add(this.openScreenshotFolder);
            this.screenshotTab.Controls.Add(this.captureOwnDeathDelay);
            this.screenshotTab.Controls.Add(this.lblOwnDeathMs);
            this.screenshotTab.Controls.Add(this.captureOwnDeath);
            this.screenshotTab.Controls.Add(this.captureOthersDeathDelay);
            this.screenshotTab.Controls.Add(this.lblCaptureOthersDeathMs);
            this.screenshotTab.Controls.Add(this.imgFmt);
            this.screenshotTab.Controls.Add(this.label12);
            this.screenshotTab.Controls.Add(this.capNow);
            this.screenshotTab.Controls.Add(this.screenPath);
            this.screenshotTab.Controls.Add(this.radioUO);
            this.screenshotTab.Controls.Add(this.radioFull);
            this.screenshotTab.Controls.Add(this.captureOthersDeath);
            this.screenshotTab.Controls.Add(this.setScnPath);
            this.screenshotTab.Controls.Add(this.screensList);
            this.screenshotTab.Controls.Add(this.screenPrev);
            this.screenshotTab.Controls.Add(this.dispTime);
            this.screenshotTab.Location = new System.Drawing.Point(10, 202);
            this.screenshotTab.Name = "screenshotTab";
            this.screenshotTab.Size = new System.Drawing.Size(1297, 713);
            this.screenshotTab.TabIndex = 8;
            this.screenshotTab.Text = "Screen Shots";
            // 
            // openScreenshotFolder
            // 
            this.openScreenshotFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openScreenshotFolder.Location = new System.Drawing.Point(635, 92);
            this.openScreenshotFolder.Name = "openScreenshotFolder";
            this.openScreenshotFolder.Size = new System.Drawing.Size(387, 58);
            this.openScreenshotFolder.TabIndex = 62;
            this.openScreenshotFolder.Text = "Open Screen Shot Folder";
            this.openScreenshotFolder.UseVisualStyleBackColor = true;
            this.openScreenshotFolder.Click += new System.EventHandler(this.openScreenshotFolder_Click);
            // 
            // captureOwnDeathDelay
            // 
            this.captureOwnDeathDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.captureOwnDeathDelay.Location = new System.Drawing.Point(1048, 248);
            this.captureOwnDeathDelay.Name = "captureOwnDeathDelay";
            this.captureOwnDeathDelay.Size = new System.Drawing.Size(80, 47);
            this.captureOwnDeathDelay.TabIndex = 61;
            this.captureOwnDeathDelay.Text = "0.5";
            this.captureOwnDeathDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.captureOwnDeathDelay.TextChanged += new System.EventHandler(this.CaptureOwnDeathDelay_TextChanged);
            // 
            // lblOwnDeathMs
            // 
            this.lblOwnDeathMs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOwnDeathMs.Location = new System.Drawing.Point(1132, 258);
            this.lblOwnDeathMs.Name = "lblOwnDeathMs";
            this.lblOwnDeathMs.Size = new System.Drawing.Size(66, 44);
            this.lblOwnDeathMs.TabIndex = 60;
            this.lblOwnDeathMs.Text = "s";
            // 
            // captureOwnDeath
            // 
            this.captureOwnDeath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.captureOwnDeath.Location = new System.Drawing.Point(588, 255);
            this.captureOwnDeath.Name = "captureOwnDeath";
            this.captureOwnDeath.Size = new System.Drawing.Size(394, 50);
            this.captureOwnDeath.TabIndex = 59;
            this.captureOwnDeath.Text = "Capture your own death";
            this.captureOwnDeath.CheckedChanged += new System.EventHandler(this.CaptureOwnDeath_CheckedChanged);
            // 
            // captureOthersDeathDelay
            // 
            this.captureOthersDeathDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.captureOthersDeathDelay.Location = new System.Drawing.Point(1048, 188);
            this.captureOthersDeathDelay.Name = "captureOthersDeathDelay";
            this.captureOthersDeathDelay.Size = new System.Drawing.Size(80, 47);
            this.captureOthersDeathDelay.TabIndex = 58;
            this.captureOthersDeathDelay.Text = "0.5";
            this.captureOthersDeathDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.captureOthersDeathDelay.TextChanged += new System.EventHandler(this.CaptureOthersDeathDelay_TextChanged);
            // 
            // lblCaptureOthersDeathMs
            // 
            this.lblCaptureOthersDeathMs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCaptureOthersDeathMs.Location = new System.Drawing.Point(1132, 195);
            this.lblCaptureOthersDeathMs.Name = "lblCaptureOthersDeathMs";
            this.lblCaptureOthersDeathMs.Size = new System.Drawing.Size(66, 45);
            this.lblCaptureOthersDeathMs.TabIndex = 57;
            this.lblCaptureOthersDeathMs.Text = "s";
            // 
            // imgFmt
            // 
            this.imgFmt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
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
            this.imgFmt.Location = new System.Drawing.Point(258, 182);
            this.imgFmt.Name = "imgFmt";
            this.imgFmt.Size = new System.Drawing.Size(244, 49);
            this.imgFmt.TabIndex = 11;
            this.imgFmt.SelectedIndexChanged += new System.EventHandler(this.imgFmt_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.Location = new System.Drawing.Point(15, 182);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(230, 50);
            this.label12.TabIndex = 10;
            this.label12.Text = "Image Format:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // capNow
            // 
            this.capNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.capNow.Location = new System.Drawing.Point(635, 22);
            this.capNow.Name = "capNow";
            this.capNow.Size = new System.Drawing.Size(387, 56);
            this.capNow.TabIndex = 8;
            this.capNow.Text = "Take Screen Shot Now";
            this.capNow.Click += new System.EventHandler(this.capNow_Click);
            // 
            // screenPath
            // 
            this.screenPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screenPath.Location = new System.Drawing.Point(20, 30);
            this.screenPath.Name = "screenPath";
            this.screenPath.Size = new System.Drawing.Size(1128, 47);
            this.screenPath.TabIndex = 7;
            this.screenPath.TextChanged += new System.EventHandler(this.screenPath_TextChanged);
            // 
            // radioUO
            // 
            this.radioUO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioUO.Location = new System.Drawing.Point(32, 240);
            this.radioUO.Name = "radioUO";
            this.radioUO.Size = new System.Drawing.Size(188, 65);
            this.radioUO.TabIndex = 6;
            this.radioUO.Text = "UO Only";
            this.radioUO.CheckedChanged += new System.EventHandler(this.radioUO_CheckedChanged);
            // 
            // radioFull
            // 
            this.radioFull.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioFull.Location = new System.Drawing.Point(258, 240);
            this.radioFull.Name = "radioFull";
            this.radioFull.Size = new System.Drawing.Size(220, 65);
            this.radioFull.TabIndex = 5;
            this.radioFull.Text = "Full Screen";
            this.radioFull.CheckedChanged += new System.EventHandler(this.radioFull_CheckedChanged);
            // 
            // captureOthersDeath
            // 
            this.captureOthersDeath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.captureOthersDeath.Location = new System.Drawing.Point(588, 190);
            this.captureOthersDeath.Name = "captureOthersDeath";
            this.captureOthersDeath.Size = new System.Drawing.Size(540, 50);
            this.captureOthersDeath.TabIndex = 4;
            this.captureOthersDeath.Text = "Capture death of other players";
            this.captureOthersDeath.CheckedChanged += new System.EventHandler(this.CaptureOthersDeath_CheckedChanged);
            // 
            // setScnPath
            // 
            this.setScnPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setScnPath.Location = new System.Drawing.Point(1162, 30);
            this.setScnPath.Name = "setScnPath";
            this.setScnPath.Size = new System.Drawing.Size(88, 58);
            this.setScnPath.TabIndex = 3;
            this.setScnPath.Text = "...";
            this.setScnPath.Click += new System.EventHandler(this.setScnPath_Click);
            // 
            // screensList
            // 
            this.screensList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screensList.IntegralHeight = false;
            this.screensList.ItemHeight = 41;
            this.screensList.Location = new System.Drawing.Point(20, 102);
            this.screensList.Name = "screensList";
            this.screensList.Size = new System.Drawing.Size(600, 66);
            this.screensList.Sorted = true;
            this.screensList.TabIndex = 1;
            this.screensList.SelectedIndexChanged += new System.EventHandler(this.screensList_SelectedIndexChanged);
            this.screensList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screensList_MouseDown);
            // 
            // screenPrev
            // 
            this.screenPrev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screenPrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.screenPrev.Location = new System.Drawing.Point(635, 102);
            this.screenPrev.Name = "screenPrev";
            this.screenPrev.Size = new System.Drawing.Size(615, 0);
            this.screenPrev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.screenPrev.TabIndex = 0;
            this.screenPrev.TabStop = false;
            this.screenPrev.Click += new System.EventHandler(this.screenPrev_Click);
            // 
            // dispTime
            // 
            this.dispTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dispTime.Location = new System.Drawing.Point(32, 312);
            this.dispTime.Name = "dispTime";
            this.dispTime.Size = new System.Drawing.Size(516, 50);
            this.dispTime.TabIndex = 9;
            this.dispTime.Text = "Include Timestamp on images";
            this.dispTime.CheckedChanged += new System.EventHandler(this.dispTime_CheckedChanged);
            // 
            // advancedTab
            // 
            this.advancedTab.BackColor = System.Drawing.SystemColors.Control;
            this.advancedTab.Controls.Add(this.subAdvancedTab);
            this.advancedTab.Location = new System.Drawing.Point(10, 106);
            this.advancedTab.Name = "advancedTab";
            this.advancedTab.Size = new System.Drawing.Size(1305, 819);
            this.advancedTab.TabIndex = 12;
            this.advancedTab.Text = "Advanced";
            // 
            // subAdvancedTab
            // 
            this.subAdvancedTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subAdvancedTab.Controls.Add(this.advancedInfoTab);
            this.subAdvancedTab.Controls.Add(this.advancedStaffDeco);
            this.subAdvancedTab.Controls.Add(this.advancedStaffDoors);
            this.subAdvancedTab.Location = new System.Drawing.Point(15, 8);
            this.subAdvancedTab.Name = "subAdvancedTab";
            this.subAdvancedTab.SelectedIndex = 0;
            this.subAdvancedTab.Size = new System.Drawing.Size(1243, 478);
            this.subAdvancedTab.TabIndex = 0;
            this.subAdvancedTab.SelectedIndexChanged += new System.EventHandler(this.subAdvancedTab_SelectedIndexChanged);
            // 
            // advancedInfoTab
            // 
            this.advancedInfoTab.BackColor = System.Drawing.SystemColors.Control;
            this.advancedInfoTab.Controls.Add(this.fontDecrease);
            this.advancedInfoTab.Controls.Add(this.fontIncrease);
            this.advancedInfoTab.Controls.Add(this.groupBox16);
            this.advancedInfoTab.Controls.Add(this.enableUOAAPI);
            this.advancedInfoTab.Controls.Add(this.disableSmartCPU);
            this.advancedInfoTab.Controls.Add(this.negotiate);
            this.advancedInfoTab.Controls.Add(this.openRazorDataDir);
            this.advancedInfoTab.Controls.Add(this.msglvl);
            this.advancedInfoTab.Controls.Add(this.label17);
            this.advancedInfoTab.Controls.Add(this.logPackets);
            this.advancedInfoTab.Controls.Add(this.statusBox);
            this.advancedInfoTab.Controls.Add(this.features);
            this.advancedInfoTab.Location = new System.Drawing.Point(10, 58);
            this.advancedInfoTab.Name = "advancedInfoTab";
            this.advancedInfoTab.Padding = new System.Windows.Forms.Padding(3);
            this.advancedInfoTab.Size = new System.Drawing.Size(1223, 410);
            this.advancedInfoTab.TabIndex = 0;
            this.advancedInfoTab.Text = "Information";
            // 
            // fontDecrease
            // 
            this.fontDecrease.Location = new System.Drawing.Point(592, 340);
            this.fontDecrease.Name = "fontDecrease";
            this.fontDecrease.Size = new System.Drawing.Size(90, 58);
            this.fontDecrease.TabIndex = 94;
            this.fontDecrease.Text = "-";
            this.fontDecrease.UseVisualStyleBackColor = true;
            this.fontDecrease.Click += new System.EventHandler(this.fontDecrease_Click);
            // 
            // fontIncrease
            // 
            this.fontIncrease.Location = new System.Drawing.Point(698, 340);
            this.fontIncrease.Name = "fontIncrease";
            this.fontIncrease.Size = new System.Drawing.Size(90, 58);
            this.fontIncrease.TabIndex = 93;
            this.fontIncrease.Text = "+";
            this.fontIncrease.UseVisualStyleBackColor = true;
            this.fontIncrease.Click += new System.EventHandler(this.fontIncrease_Click);
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.lastBackup);
            this.groupBox16.Controls.Add(this.openBackupFolder);
            this.groupBox16.Controls.Add(this.setBackupFolder);
            this.groupBox16.Controls.Add(this.createBackup);
            this.groupBox16.Location = new System.Drawing.Point(15, 455);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(563, 247);
            this.groupBox16.TabIndex = 92;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Backup";
            // 
            // lastBackup
            // 
            this.lastBackup.Location = new System.Drawing.Point(250, 138);
            this.lastBackup.Name = "lastBackup";
            this.lastBackup.Size = new System.Drawing.Size(298, 92);
            this.lastBackup.TabIndex = 82;
            this.lastBackup.Text = "Last Backup: 01/01/2019 5:54PM";
            this.lastBackup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openBackupFolder
            // 
            this.openBackupFolder.Location = new System.Drawing.Point(15, 138);
            this.openBackupFolder.Name = "openBackupFolder";
            this.openBackupFolder.Size = new System.Drawing.Size(220, 74);
            this.openBackupFolder.TabIndex = 81;
            this.openBackupFolder.Text = "Open Folder";
            this.openBackupFolder.UseVisualStyleBackColor = true;
            this.openBackupFolder.Click += new System.EventHandler(this.openBackupFolder_Click);
            // 
            // setBackupFolder
            // 
            this.setBackupFolder.Location = new System.Drawing.Point(335, 55);
            this.setBackupFolder.Name = "setBackupFolder";
            this.setBackupFolder.Size = new System.Drawing.Size(213, 75);
            this.setBackupFolder.TabIndex = 80;
            this.setBackupFolder.Text = "Set Folder";
            this.setBackupFolder.UseVisualStyleBackColor = true;
            this.setBackupFolder.Click += new System.EventHandler(this.setBackupFolder_Click);
            // 
            // createBackup
            // 
            this.createBackup.Location = new System.Drawing.Point(15, 55);
            this.createBackup.Name = "createBackup";
            this.createBackup.Size = new System.Drawing.Size(305, 75);
            this.createBackup.TabIndex = 79;
            this.createBackup.Text = "Create Backup";
            this.createBackup.UseVisualStyleBackColor = true;
            this.createBackup.Click += new System.EventHandler(this.createBackup_Click);
            // 
            // enableUOAAPI
            // 
            this.enableUOAAPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.enableUOAAPI.Checked = true;
            this.enableUOAAPI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableUOAAPI.Location = new System.Drawing.Point(548, 205);
            this.enableUOAAPI.Name = "enableUOAAPI";
            this.enableUOAAPI.Size = new System.Drawing.Size(365, 47);
            this.enableUOAAPI.TabIndex = 91;
            this.enableUOAAPI.Text = "Enable UOA API";
            this.enableUOAAPI.CheckedChanged += new System.EventHandler(this.enableUOAAPI_CheckedChanged);
            // 
            // disableSmartCPU
            // 
            this.disableSmartCPU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.disableSmartCPU.Location = new System.Drawing.Point(880, 340);
            this.disableSmartCPU.Name = "disableSmartCPU";
            this.disableSmartCPU.Size = new System.Drawing.Size(298, 55);
            this.disableSmartCPU.TabIndex = 90;
            this.disableSmartCPU.Text = "Disable SmartCPU";
            this.disableSmartCPU.UseVisualStyleBackColor = true;
            // 
            // negotiate
            // 
            this.negotiate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.negotiate.Location = new System.Drawing.Point(548, 140);
            this.negotiate.Name = "negotiate";
            this.negotiate.Size = new System.Drawing.Size(492, 50);
            this.negotiate.TabIndex = 89;
            this.negotiate.Text = "Negotiate features with server";
            this.negotiate.CheckedChanged += new System.EventHandler(this.negotiate_CheckedChanged);
            // 
            // openRazorDataDir
            // 
            this.openRazorDataDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openRazorDataDir.Location = new System.Drawing.Point(928, 15);
            this.openRazorDataDir.Name = "openRazorDataDir";
            this.openRazorDataDir.Size = new System.Drawing.Size(250, 110);
            this.openRazorDataDir.TabIndex = 88;
            this.openRazorDataDir.Text = "Open Data Directory";
            this.openRazorDataDir.UseVisualStyleBackColor = true;
            this.openRazorDataDir.Click += new System.EventHandler(this.openRazorDataDir_Click);
            // 
            // msglvl
            // 
            this.msglvl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.msglvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.msglvl.Items.AddRange(new object[] {
            "Show All",
            "Warnings & Errors",
            "Errors Only",
            "None"});
            this.msglvl.Location = new System.Drawing.Point(776, 268);
            this.msglvl.Name = "msglvl";
            this.msglvl.Size = new System.Drawing.Size(402, 49);
            this.msglvl.TabIndex = 87;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.Location = new System.Drawing.Point(540, 270);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(248, 45);
            this.label17.TabIndex = 86;
            this.label17.Text = "Razor messages:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logPackets
            // 
            this.logPackets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logPackets.Location = new System.Drawing.Point(548, 68);
            this.logPackets.Name = "logPackets";
            this.logPackets.Size = new System.Drawing.Size(365, 57);
            this.logPackets.TabIndex = 85;
            this.logPackets.Text = "Enable packet logging";
            this.logPackets.CheckedChanged += new System.EventHandler(this.logPackets_CheckedChanged);
            // 
            // statusBox
            // 
            this.statusBox.BackColor = System.Drawing.SystemColors.Control;
            this.statusBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.HideSelection = false;
            this.statusBox.Location = new System.Drawing.Point(15, 15);
            this.statusBox.Multiline = true;
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            this.statusBox.Size = new System.Drawing.Size(563, 425);
            this.statusBox.TabIndex = 84;
            // 
            // features
            // 
            this.features.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.features.Cursor = System.Windows.Forms.Cursors.No;
            this.features.Location = new System.Drawing.Point(592, 410);
            this.features.Multiline = true;
            this.features.Name = "features";
            this.features.ReadOnly = true;
            this.features.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.features.Size = new System.Drawing.Size(586, 106);
            this.features.TabIndex = 83;
            this.features.Visible = false;
            // 
            // advancedStaffDeco
            // 
            this.advancedStaffDeco.BackColor = System.Drawing.SystemColors.Control;
            this.advancedStaffDeco.Controls.Add(this.itemAppendM);
            this.advancedStaffDeco.Controls.Add(this.itemCopyToClipboard);
            this.advancedStaffDeco.Controls.Add(this.itemSearch);
            this.advancedStaffDeco.Controls.Add(this.itemMovable);
            this.advancedStaffDeco.Controls.Add(this.itemRandomNumber);
            this.advancedStaffDeco.Controls.Add(this.itemRandom);
            this.advancedStaffDeco.Controls.Add(this.itemTileCount);
            this.advancedStaffDeco.Controls.Add(this.itemTile);
            this.advancedStaffDeco.Controls.Add(this.itemAdd);
            this.advancedStaffDeco.Controls.Add(this.itemTree);
            this.advancedStaffDeco.Controls.Add(this.artViewer);
            this.advancedStaffDeco.Location = new System.Drawing.Point(10, 58);
            this.advancedStaffDeco.Name = "advancedStaffDeco";
            this.advancedStaffDeco.Padding = new System.Windows.Forms.Padding(3);
            this.advancedStaffDeco.Size = new System.Drawing.Size(1215, 304);
            this.advancedStaffDeco.TabIndex = 1;
            this.advancedStaffDeco.Text = "Decoration";
            // 
            // itemAppendM
            // 
            this.itemAppendM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itemAppendM.AutoSize = true;
            this.itemAppendM.Location = new System.Drawing.Point(714, 538);
            this.itemAppendM.Name = "itemAppendM";
            this.itemAppendM.Size = new System.Drawing.Size(434, 45);
            this.itemAppendM.TabIndex = 11;
            this.itemAppendM.Text = "Append [m to the command";
            this.itemAppendM.UseVisualStyleBackColor = true;
            // 
            // itemCopyToClipboard
            // 
            this.itemCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itemCopyToClipboard.AutoSize = true;
            this.itemCopyToClipboard.Location = new System.Drawing.Point(712, 475);
            this.itemCopyToClipboard.Name = "itemCopyToClipboard";
            this.itemCopyToClipboard.Size = new System.Drawing.Size(488, 45);
            this.itemCopyToClipboard.TabIndex = 10;
            this.itemCopyToClipboard.Text = "Copy command to the clipboard";
            this.itemCopyToClipboard.UseVisualStyleBackColor = true;
            // 
            // itemSearch
            // 
            this.itemSearch.AcceptsReturn = true;
            this.itemSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemSearch.Location = new System.Drawing.Point(15, 15);
            this.itemSearch.Name = "itemSearch";
            this.itemSearch.Size = new System.Drawing.Size(493, 47);
            this.itemSearch.TabIndex = 9;
            this.itemSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.itemSearch_KeyDown);
            this.itemSearch.LostFocus += new System.EventHandler(this.itemSearch_LostFocus);
            // 
            // itemMovable
            // 
            this.itemMovable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itemMovable.AutoSize = true;
            this.itemMovable.Location = new System.Drawing.Point(531, 272);
            this.itemMovable.Name = "itemMovable";
            this.itemMovable.Size = new System.Drawing.Size(171, 45);
            this.itemMovable.TabIndex = 8;
            this.itemMovable.Text = "Movable";
            this.itemMovable.UseVisualStyleBackColor = true;
            // 
            // itemRandomNumber
            // 
            this.itemRandomNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itemRandomNumber.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemRandomNumber.Location = new System.Drawing.Point(520, 398);
            this.itemRandomNumber.Name = "itemRandomNumber";
            this.itemRandomNumber.Size = new System.Drawing.Size(188, 51);
            this.itemRandomNumber.TabIndex = 7;
            // 
            // itemRandom
            // 
            this.itemRandom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itemRandom.AutoSize = true;
            this.itemRandom.Location = new System.Drawing.Point(530, 335);
            this.itemRandom.Name = "itemRandom";
            this.itemRandom.Size = new System.Drawing.Size(168, 45);
            this.itemRandom.TabIndex = 6;
            this.itemRandom.Text = "Random";
            this.itemRandom.UseVisualStyleBackColor = true;
            // 
            // itemTileCount
            // 
            this.itemTileCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itemTileCount.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemTileCount.Location = new System.Drawing.Point(522, 195);
            this.itemTileCount.Name = "itemTileCount";
            this.itemTileCount.Size = new System.Drawing.Size(188, 51);
            this.itemTileCount.TabIndex = 4;
            // 
            // itemTile
            // 
            this.itemTile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itemTile.Location = new System.Drawing.Point(522, 105);
            this.itemTile.Name = "itemTile";
            this.itemTile.Size = new System.Drawing.Size(188, 75);
            this.itemTile.TabIndex = 3;
            this.itemTile.Text = "Tile";
            this.itemTile.UseVisualStyleBackColor = true;
            this.itemTile.Click += new System.EventHandler(this.itemTile_Click);
            // 
            // itemAdd
            // 
            this.itemAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itemAdd.Location = new System.Drawing.Point(522, 15);
            this.itemAdd.Name = "itemAdd";
            this.itemAdd.Size = new System.Drawing.Size(188, 75);
            this.itemAdd.TabIndex = 2;
            this.itemAdd.Text = "Add";
            this.itemAdd.UseVisualStyleBackColor = true;
            this.itemAdd.Click += new System.EventHandler(this.itemAdd_Click);
            this.itemAdd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.itemAdd_MouseDown);
            // 
            // itemTree
            // 
            this.itemTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemTree.Location = new System.Drawing.Point(15, 88);
            this.itemTree.Name = "itemTree";
            this.itemTree.Size = new System.Drawing.Size(493, 534);
            this.itemTree.TabIndex = 1;
            this.itemTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.itemTree_AfterSelect);
            this.itemTree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.itemTree_MouseDoubleClick);
            // 
            // artViewer
            // 
            this.artViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.artViewer.Animate = false;
            this.artViewer.Art = Assistant.UI.Art.Items;
            this.artViewer.ArtIndex = 0;
            this.artViewer.Hue = 0;
            this.artViewer.Location = new System.Drawing.Point(725, 15);
            this.artViewer.Name = "artViewer";
            this.artViewer.ResizeTallItems = false;
            this.artViewer.RoomView = true;
            this.artViewer.ShowHexID = true;
            this.artViewer.ShowID = true;
            this.artViewer.Size = new System.Drawing.Size(475, 445);
            this.artViewer.TabIndex = 0;
            // 
            // advancedStaffDoors
            // 
            this.advancedStaffDoors.BackColor = System.Drawing.SystemColors.Control;
            this.advancedStaffDoors.Controls.Add(this.doorViewer);
            this.advancedStaffDoors.Controls.Add(this.doorTree);
            this.advancedStaffDoors.Controls.Add(this.doorSouthCW);
            this.advancedStaffDoors.Controls.Add(this.doorSouthCCW);
            this.advancedStaffDoors.Controls.Add(this.doorEastCW);
            this.advancedStaffDoors.Controls.Add(this.doorEastCCW);
            this.advancedStaffDoors.Controls.Add(this.doorNorthCCW);
            this.advancedStaffDoors.Controls.Add(this.doorNorthCW);
            this.advancedStaffDoors.Controls.Add(this.doorWestCCW);
            this.advancedStaffDoors.Controls.Add(this.doorWestCW);
            this.advancedStaffDoors.Location = new System.Drawing.Point(10, 58);
            this.advancedStaffDoors.Name = "advancedStaffDoors";
            this.advancedStaffDoors.Size = new System.Drawing.Size(1215, 304);
            this.advancedStaffDoors.TabIndex = 2;
            this.advancedStaffDoors.Text = "Doors";
            // 
            // doorViewer
            // 
            this.doorViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.doorViewer.Animate = false;
            this.doorViewer.Art = Assistant.UI.Art.Items;
            this.doorViewer.ArtIndex = 0;
            this.doorViewer.Hue = 0;
            this.doorViewer.Location = new System.Drawing.Point(412, 412);
            this.doorViewer.MaximumSize = new System.Drawing.Size(318, 318);
            this.doorViewer.Name = "doorViewer";
            this.doorViewer.ResizeTallItems = false;
            this.doorViewer.RoomView = true;
            this.doorViewer.ShowHexID = true;
            this.doorViewer.ShowID = true;
            this.doorViewer.Size = new System.Drawing.Size(318, 218);
            this.doorViewer.TabIndex = 33;
            // 
            // doorTree
            // 
            this.doorTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.doorTree.Location = new System.Drawing.Point(8, 8);
            this.doorTree.Name = "doorTree";
            this.doorTree.Size = new System.Drawing.Size(420, 622);
            this.doorTree.TabIndex = 32;
            this.doorTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.doorTree_AfterSelect);
            // 
            // doorSouthCW
            // 
            this.doorSouthCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doorSouthCW.Image = ((System.Drawing.Image)(resources.GetObject("doorSouthCW.Image")));
            this.doorSouthCW.Location = new System.Drawing.Point(1020, 210);
            this.doorSouthCW.Name = "doorSouthCW";
            this.doorSouthCW.Size = new System.Drawing.Size(188, 188);
            this.doorSouthCW.TabIndex = 31;
            this.doorSouthCW.UseVisualStyleBackColor = true;
            this.doorSouthCW.Click += new System.EventHandler(this.doorSouthCW_Click);
            this.doorSouthCW.MouseEnter += new System.EventHandler(this.DoorFacingEnter);
            // 
            // doorSouthCCW
            // 
            this.doorSouthCCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doorSouthCCW.Image = ((System.Drawing.Image)(resources.GetObject("doorSouthCCW.Image")));
            this.doorSouthCCW.Location = new System.Drawing.Point(818, 210);
            this.doorSouthCCW.Name = "doorSouthCCW";
            this.doorSouthCCW.Size = new System.Drawing.Size(187, 188);
            this.doorSouthCCW.TabIndex = 30;
            this.doorSouthCCW.UseVisualStyleBackColor = true;
            this.doorSouthCCW.Click += new System.EventHandler(this.doorSouthCCW_Click);
            this.doorSouthCCW.MouseEnter += new System.EventHandler(this.DoorFacingEnter);
            // 
            // doorEastCW
            // 
            this.doorEastCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doorEastCW.Image = ((System.Drawing.Image)(resources.GetObject("doorEastCW.Image")));
            this.doorEastCW.Location = new System.Drawing.Point(615, 210);
            this.doorEastCW.Name = "doorEastCW";
            this.doorEastCW.Size = new System.Drawing.Size(187, 188);
            this.doorEastCW.TabIndex = 29;
            this.doorEastCW.UseVisualStyleBackColor = true;
            this.doorEastCW.Click += new System.EventHandler(this.doorEastCW_Click);
            this.doorEastCW.MouseEnter += new System.EventHandler(this.DoorFacingEnter);
            // 
            // doorEastCCW
            // 
            this.doorEastCCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doorEastCCW.Image = ((System.Drawing.Image)(resources.GetObject("doorEastCCW.Image")));
            this.doorEastCCW.Location = new System.Drawing.Point(412, 210);
            this.doorEastCCW.Name = "doorEastCCW";
            this.doorEastCCW.Size = new System.Drawing.Size(188, 188);
            this.doorEastCCW.TabIndex = 28;
            this.doorEastCCW.UseVisualStyleBackColor = true;
            this.doorEastCCW.Click += new System.EventHandler(this.doorEastCCW_Click);
            this.doorEastCCW.MouseEnter += new System.EventHandler(this.DoorFacingEnter);
            // 
            // doorNorthCCW
            // 
            this.doorNorthCCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doorNorthCCW.Image = ((System.Drawing.Image)(resources.GetObject("doorNorthCCW.Image")));
            this.doorNorthCCW.Location = new System.Drawing.Point(1020, 8);
            this.doorNorthCCW.Name = "doorNorthCCW";
            this.doorNorthCCW.Size = new System.Drawing.Size(188, 187);
            this.doorNorthCCW.TabIndex = 27;
            this.doorNorthCCW.UseVisualStyleBackColor = true;
            this.doorNorthCCW.Click += new System.EventHandler(this.doorNorthCCW_Click);
            this.doorNorthCCW.MouseEnter += new System.EventHandler(this.DoorFacingEnter);
            // 
            // doorNorthCW
            // 
            this.doorNorthCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doorNorthCW.Image = ((System.Drawing.Image)(resources.GetObject("doorNorthCW.Image")));
            this.doorNorthCW.Location = new System.Drawing.Point(818, 8);
            this.doorNorthCW.Name = "doorNorthCW";
            this.doorNorthCW.Size = new System.Drawing.Size(187, 187);
            this.doorNorthCW.TabIndex = 26;
            this.doorNorthCW.UseVisualStyleBackColor = true;
            this.doorNorthCW.Click += new System.EventHandler(this.doorNorthCW_Click);
            this.doorNorthCW.MouseEnter += new System.EventHandler(this.DoorFacingEnter);
            // 
            // doorWestCCW
            // 
            this.doorWestCCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doorWestCCW.Image = ((System.Drawing.Image)(resources.GetObject("doorWestCCW.Image")));
            this.doorWestCCW.Location = new System.Drawing.Point(615, 8);
            this.doorWestCCW.Name = "doorWestCCW";
            this.doorWestCCW.Size = new System.Drawing.Size(187, 187);
            this.doorWestCCW.TabIndex = 25;
            this.doorWestCCW.UseVisualStyleBackColor = true;
            this.doorWestCCW.Click += new System.EventHandler(this.doorWestCCW_Click);
            this.doorWestCCW.MouseEnter += new System.EventHandler(this.DoorFacingEnter);
            // 
            // doorWestCW
            // 
            this.doorWestCW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doorWestCW.Image = ((System.Drawing.Image)(resources.GetObject("doorWestCW.Image")));
            this.doorWestCW.Location = new System.Drawing.Point(412, 8);
            this.doorWestCW.Name = "doorWestCW";
            this.doorWestCW.Size = new System.Drawing.Size(188, 187);
            this.doorWestCW.TabIndex = 24;
            this.doorWestCW.UseVisualStyleBackColor = true;
            this.doorWestCW.Click += new System.EventHandler(this.doorWestCW_Click);
            this.doorWestCW.MouseEnter += new System.EventHandler(this.DoorFacingEnter);
            // 
            // aboutTab
            // 
            this.aboutTab.Controls.Add(this.linkGitHub);
            this.aboutTab.Controls.Add(this.lblCredits3);
            this.aboutTab.Controls.Add(this.linkHelp);
            this.aboutTab.Controls.Add(this.lblCredits2);
            this.aboutTab.Controls.Add(this.label20);
            this.aboutTab.Controls.Add(this.lblCredits1);
            this.aboutTab.Controls.Add(this.aboutSubInfo);
            this.aboutTab.Controls.Add(this.linkMain);
            this.aboutTab.Controls.Add(this.label21);
            this.aboutTab.Controls.Add(this.aboutVer);
            this.aboutTab.Location = new System.Drawing.Point(10, 106);
            this.aboutTab.Name = "aboutTab";
            this.aboutTab.Size = new System.Drawing.Size(1305, 819);
            this.aboutTab.TabIndex = 9;
            this.aboutTab.Text = "About";
            // 
            // linkGitHub
            // 
            this.linkGitHub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linkGitHub.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkGitHub.Location = new System.Drawing.Point(22, 241);
            this.linkGitHub.Name = "linkGitHub";
            this.linkGitHub.Size = new System.Drawing.Size(1244, 50);
            this.linkGitHub.TabIndex = 25;
            this.linkGitHub.TabStop = true;
            this.linkGitHub.Text = "https://github.com/markdwags/Razor";
            this.linkGitHub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGitHub_LinkClicked);
            // 
            // lblCredits3
            // 
            this.lblCredits3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCredits3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits3.Location = new System.Drawing.Point(20, 442);
            this.lblCredits3.Name = "lblCredits3";
            this.lblCredits3.Size = new System.Drawing.Size(1228, 50);
            this.lblCredits3.TabIndex = 24;
            this.lblCredits3.Text = "Cross-platform implementation by DarkLotus";
            this.lblCredits3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkHelp
            // 
            this.linkHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkHelp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkHelp.Location = new System.Drawing.Point(1063, 25);
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.Size = new System.Drawing.Size(195, 50);
            this.linkHelp.TabIndex = 23;
            this.linkHelp.TabStop = true;
            this.linkHelp.Text = "Need Help?";
            this.linkHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHelp_LinkClicked);
            // 
            // lblCredits2
            // 
            this.lblCredits2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCredits2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits2.Location = new System.Drawing.Point(22, 445);
            this.lblCredits2.Name = "lblCredits2";
            this.lblCredits2.Size = new System.Drawing.Size(1236, 50);
            this.lblCredits2.TabIndex = 22;
            this.lblCredits2.Text = "Major design changes including ClassicUO integration by Jaedan";
            this.lblCredits2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(30, 148);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(1226, 43);
            this.label20.TabIndex = 21;
            this.label20.Text = "For feedback, support and the latest releases please visit:\r\n";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCredits1
            // 
            this.lblCredits1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCredits1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits1.Location = new System.Drawing.Point(15, 342);
            this.lblCredits1.Name = "lblCredits1";
            this.lblCredits1.Size = new System.Drawing.Size(1235, 50);
            this.lblCredits1.TabIndex = 19;
            this.lblCredits1.Text = "Razor was designed by Zippy, modified and maintained by Quick";
            this.lblCredits1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // aboutSubInfo
            // 
            this.aboutSubInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutSubInfo.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutSubInfo.Location = new System.Drawing.Point(15, -18);
            this.aboutSubInfo.Name = "aboutSubInfo";
            this.aboutSubInfo.Size = new System.Drawing.Size(1235, 48);
            this.aboutSubInfo.TabIndex = 17;
            this.aboutSubInfo.Text = "Community Edition";
            this.aboutSubInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkMain
            // 
            this.linkMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.linkMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkMain.Location = new System.Drawing.Point(22, 191);
            this.linkMain.Name = "linkMain";
            this.linkMain.Size = new System.Drawing.Size(1244, 50);
            this.linkMain.TabIndex = 16;
            this.linkMain.TabStop = true;
            this.linkMain.Text = "https://www.razorce.com";
            this.linkMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkMain.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(130, 245);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(0, 41);
            this.label21.TabIndex = 15;
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // aboutVer
            // 
            this.aboutVer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutVer.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutVer.Location = new System.Drawing.Point(15, -105);
            this.aboutVer.Name = "aboutVer";
            this.aboutVer.Size = new System.Drawing.Size(1235, 87);
            this.aboutVer.TabIndex = 14;
            this.aboutVer.Text = "Razor v{0}";
            this.aboutVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(15, 40);
            this.ClientSize = new System.Drawing.Size(1333, 940);
            this.Controls.Add(this.tabs);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1365, 1028);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Razor v{0}";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.Load += new System.EventHandler(this.MainForm_StartLoad);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabs.ResumeLayout(false);
            this.generalTab.ResumeLayout(false);
            this.subGeneralTab.ResumeLayout(false);
            this.subGenTab.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.moreOptTab.ResumeLayout(false);
            this.optionsTabCtrl.ResumeLayout(false);
            this.subOptionsSpeechTab.ResumeLayout(false);
            this.subOptionsSpeechTab.PerformLayout();
            this.subOptionsTargetTab.ResumeLayout(false);
            this.subOptionsTargetTab.PerformLayout();
            this.groupSmartTarget.ResumeLayout(false);
            this.groupSmartTarget.PerformLayout();
            this.subOptionsMiscTab.ResumeLayout(false);
            this.subOptionsMiscTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lightLevelBar)).EndInit();
            this.displayTab.ResumeLayout(false);
            this.displayCountersTabCtrl.ResumeLayout(false);
            this.subDisplayTab.ResumeLayout(false);
            this.subDisplayTab.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.subCountersTab.ResumeLayout(false);
            this.subCountersTab.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.subBandageTimerTab.ResumeLayout(false);
            this.subBandageTimerTab.PerformLayout();
            this.subOverheadTab.ResumeLayout(false);
            this.subOverheadTab.PerformLayout();
            this.subWaypoints.ResumeLayout(false);
            this.subWaypoints.PerformLayout();
            this.subBuffsDebuffs.ResumeLayout(false);
            this.subBuffsDebuffs.PerformLayout();
            this.cooldownGumpBox.ResumeLayout(false);
            this.cooldownGumpBox.PerformLayout();
            this.buffBarGroupBox.ResumeLayout(false);
            this.buffBarGroupBox.PerformLayout();
            this.dressTab.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.skillsTab.ResumeLayout(false);
            this.skillsTab.PerformLayout();
            this.agentsTab.ResumeLayout(false);
            this.agentGroup.ResumeLayout(false);
            this.filtersTab.ResumeLayout(false);
            this.filterTabs.ResumeLayout(false);
            this.subFilterTab.ResumeLayout(false);
            this.subFilterTab.PerformLayout();
            this.subFilterText.ResumeLayout(false);
            this.gbFilterText.ResumeLayout(false);
            this.gbFilterText.PerformLayout();
            this.gbFilterMessages.ResumeLayout(false);
            this.gbFilterMessages.PerformLayout();
            this.subFilterSoundMusic.ResumeLayout(false);
            this.subFilterSoundMusic.PerformLayout();
            this.subFilterTargets.ResumeLayout(false);
            this.subFilterTargets.PerformLayout();
            this.hotkeysTab.ResumeLayout(false);
            this.hotkeysTab.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.macrosTab.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.subMacrosTab.ResumeLayout(false);
            this.subMacrosTab.PerformLayout();
            this.macroActGroup.ResumeLayout(false);
            this.subMacrosOptionsTab.ResumeLayout(false);
            this.subMacrosOptionsTab.PerformLayout();
            this.macroVariableGroup.ResumeLayout(false);
            this.scriptsTab.ResumeLayout(false);
            this.subTabScripts.ResumeLayout(false);
            this.subScripts.ResumeLayout(false);
            this.subScripts.PerformLayout();
            this.scriptSplitContainer.Panel1.ResumeLayout(false);
            this.scriptSplitContainer.Panel1.PerformLayout();
            this.scriptSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scriptSplitContainer)).EndInit();
            this.scriptSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scriptEditor)).EndInit();
            this.subScriptOptions.ResumeLayout(false);
            this.subScriptOptions.PerformLayout();
            this.scriptVariablesBox.ResumeLayout(false);
            this.friendsTab.ResumeLayout(false);
            this.friendsTab.PerformLayout();
            this.friendsGroupBox.ResumeLayout(false);
            this.friendsGroupBox.PerformLayout();
            this.screenshotTab.ResumeLayout(false);
            this.screenshotTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.screenPrev)).EndInit();
            this.advancedTab.ResumeLayout(false);
            this.subAdvancedTab.ResumeLayout(false);
            this.advancedInfoTab.ResumeLayout(false);
            this.advancedInfoTab.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.advancedStaffDeco.ResumeLayout(false);
            this.advancedStaffDeco.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemRandomNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemTileCount)).EndInit();
            this.advancedStaffDoors.ResumeLayout(false);
            this.aboutTab.ResumeLayout(false);
            this.aboutTab.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.CheckBox disableScriptStopwatch;

        private System.Windows.Forms.CheckBox enableHighlight;

        private System.Windows.Forms.CheckBox defaultScriptDelay;

        private System.Windows.Forms.TabPage subBuffsDebuffs;

        #endregion

        private TabPage scriptsTab;
        private TabControl subTabScripts;
        private TabPage subScripts;
        private Button saveScript;
        private Button newScript;
        private Button setScriptHotkey;
        private Button recordScript;
        private Button playScript;
        private System.Windows.Forms.TabPage subScriptOptions;
        private GroupBox scriptVariablesBox;
        private Button changeScriptVariable;
        private Button removeScriptVariable;
        private Button addScriptVariable;
        private ListBox scriptVariables;
        private CheckBox autoSaveScript;
        private CheckBox autoSaveScriptPlay;
        private CheckBox scriptDisablePlayFinish;
        private TabPage friendsTab;
        private CheckBox highlightFriend;
        private CheckBox autoAcceptParty;
        private CheckBox nextPrevIgnoresFriends;
        private CheckBox autoFriend;
        private GroupBox friendsGroupBox;
        private Button friendsGroupRemove;
        private Button friendsGroupAdd;
        private Button friendClearList;
        private Button friendRemoveSelected;
        private Button friendAddTarget;
        private ListBox friendsList;
        private ComboBox friendsGroup;
        private CheckBox friendsListEnabled;
        private CheckBox showFriendOverhead;
        private Button setFriendsFormatHue;
        private TextBox friendOverheadFormat;
        private Label friendFormat;
        private Button scriptGuide;
        private CheckBox showPartyFriendOverhead;
        private TabPage subWaypoints;
        private System.Windows.Forms.CheckBox waypointOnDeath;
        private Label lblWaypointTiles;
        private TextBox hideWaypointDist;
        private System.Windows.Forms.CheckBox hideWaypointWithin;
        private TextBox txtWaypointDistanceSec;
        private Label lblWaypointSeconds;
        private System.Windows.Forms.CheckBox showWaypointDistance;
        private System.Windows.Forms.CheckBox showWaypointOverhead;
        private Button btnRemoveSelectedWaypoint;
        private Button btnHideWaypoint;
        private Button btnUseCurrentLoc;
        private TextBox txtWaypointName;
        private Label lblWaypointY;
        private Label lblWaypointX;
        private TextBox txtWaypointX;
        private TextBox txtWaypointY;
        private Label lblWaypoint;
        private Button btnAddWaypoint;
        private System.Windows.Forms.ListBox waypointList;
        private TabPage filtersTab;
        private TabControl filterTabs;
        private TabPage subFilterTab;
        private ComboBox daemonAnimationList;
        private CheckBox filterDaemonGraphics;
        private ComboBox drakeAnimationList;
        private CheckBox filterDrakeGraphics;
        private ComboBox dragonAnimationList;
        private CheckBox filterDragonGraphics;
        private CheckedListBox filters;
        private TabPage subFilterTargets;
        private Label lblTargetFilter;
        private Button targetFilterClear;
        private Button targetFilterRemove;
        private Button targetFilterAdd;
        private ListBox targetFilter;
        private CheckBox targetFilterEnabled;
        private TabPage subOverheadTab;
        private CheckBox showOverheadMessages;
        private Button cliLocSearch;
        private TextBox cliLocTextSearch;
        private Label lblOhSearch;
        private ListView cliLocSearchView;
        private ColumnHeader cliLocSearchNumber;
        private ColumnHeader cliLocSearchText;
        private Label label13;
        private TextBox overheadFormat;
        private Button setOverheadMessage;
        private System.Windows.Forms.ListView cliLocOverheadView;
        private ColumnHeader cliLocOriginal;
        private ColumnHeader cliLocOverheadMessage;
        private System.Windows.Forms.Button editOverheadMessage;
        private System.Windows.Forms.Button setColorHue;
        private System.Windows.Forms.Button removeOverheadMessage;
        private Label label14;
        private System.Windows.Forms.RadioButton unicodeStyle;
        private System.Windows.Forms.RadioButton asciiStyle;
        private CheckBox overrideSpellFormat;
        private CheckBox reequipHandsPotion;
        private Button agentSetHotKey;
        private TabPage subFilterText;
        private GroupBox gbFilterText;
        private Button removeFilterText;
        private Button addFilterText;
        private ListBox textFilterList;
        private GroupBox gbFilterMessages;
        private CheckBox filterOverheadMessages;
        private Label lblFilterDelaySeconds;
        private Label lblFilterDelay;
        private TextBox filterDelaySeconds;
        private CheckBox filterRazorMessages;
        private CheckBox filterSystemMessages;
        private CheckBox filterSnoop;
        private CheckBox enableTextFilter;
        private System.Windows.Forms.CheckBox disableScriptTooltips;
        private SplitContainer scriptSplitContainer;
        private TreeView scriptTree;
        private TextBox scriptFilter;
        private FastColoredTextBoxNS.FastColoredTextBox scriptEditor;
        private Label scriptHotkey;
        private Button openScreenshotFolder;
        private CheckBox buyAgentIgnoreGold;
        private CheckBox nextPrevAbcOrder;
        private ComboBox wyrmAnimationList;
        private CheckBox filterWhiteWyrm;
        private TextBox hkCommand;
        private Label hkCmdLabel;
        private ColumnHeader cliLocSound;
        private System.Windows.Forms.Button setSound;
        private TabControl subAdvancedTab;
        private TabPage advancedInfoTab;
        private Button fontDecrease;
        private Button fontIncrease;
        private GroupBox groupBox16;
        private Label lastBackup;
        private Button openBackupFolder;
        private Button setBackupFolder;
        private Button createBackup;
        private CheckBox enableUOAAPI;
        private Button disableSmartCPU;
        private CheckBox negotiate;
        private Button openRazorDataDir;
        private ComboBox msglvl;
        private Label label17;
        private CheckBox logPackets;
        private TextBox statusBox;
        private TextBox features;
        private TabPage advancedStaffDeco;
        private UI.ArtViewer artViewer;
        private TreeView itemTree;
        private NumericUpDown itemRandomNumber;
        private CheckBox itemRandom;
        private NumericUpDown itemTileCount;
        private Button itemTile;
        private Button itemAdd;
        private CheckBox itemMovable;
        private TabPage advancedStaffDoors;
        private Button doorSouthCW;
        private Button doorSouthCCW;
        private Button doorEastCW;
        private Button doorEastCCW;
        private Button doorNorthCCW;
        private Button doorNorthCW;
        private Button doorWestCCW;
        private Button doorWestCW;
        private TreeView doorTree;
        private UI.ArtViewer doorViewer;
        private TextBox itemSearch;
        private CheckBox itemCopyToClipboard;
        private CheckBox itemAppendM;
        private FastColoredTextBoxNS.DocumentMap scriptDocMap;
        private System.Windows.Forms.CheckBox playEmoteSound;
        private GroupBox buffBarGroupBox;
        private ComboBox buffBarSort;
        private Label lblBuffSortBy;
        private TextBox buffBarWidth;
        private Label lblBuffBarWidth;
        private CheckBox showBuffIcons;
        private CheckBox showBuffDebuffGump;
        private TextBox buffBarHeight;
        private Label lblBuffBarHeight;
        private CheckBox useBlackBuffDebuffBg;
        private ComboBox showBuffDebuffTimeType;
        private Label lblShowBuffTime;
        private GroupBox cooldownGumpBox;
        private TextBox cooldownHeight;
        private Label lblCooldownHeight;
        private TextBox cooldownWidth;
        private Label lblCooldownWidth;
    }
}
