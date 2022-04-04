using System;
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
        private Button openRazorDataDir;
        private ComboBox msglvl;
        private Label label17;
        private CheckBox logPackets;
        private TextBox statusBox;
        private TextBox features;
        private CheckBox negotiate;
        private Label aboutSubInfo;
        private Label lblCredits1;
        private Label label20;
        private Button disableSmartCPU;
        private CheckBox logSkillChanges;
        private TreeView _hotkeyTreeViewCache = new TreeView();
        private LinkLabel linkHelp;
        private CheckBox enableUOAAPI;
        private TabControl tabControl2;
        private TabPage subMacrosTab;
        private TabPage subMacrosOptionsTab;
        private CheckBox rangeCheckDoubleClick;
        private CheckBox rangeCheckTargetByType;
        private Button nextMacroAction;
        private CheckBox stepThroughMacro;
        private CheckBox targetByTypeDifferent;
        private GroupBox macroVariableGroup;
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
        private TabPage subOptionsSpeechTab;
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
        private CheckBox showBuffDebuffOverhead;
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
        private TabControl displayCountersTabCtrl;
        private TabPage subDisplayTab;
        private GroupBox groupBox11;
        private Button razorTitleBarKey;
        private CheckBox showInRazorTitleBar;
        private TextBox razorTitleBar;
        private CheckBox trackDps;
        private CheckBox trackIncomingGold;
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
        private GroupBox groupBox2;
        private ListView counters;
        private ColumnHeader cntName;
        private ColumnHeader cntCount;
        private Button delCounter;
        private Button addCounter;
        private Button recount;
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
        private CheckBox showBandageEnd;
        private CheckBox showBandageStart;
        private Button buffDebuffOptions;
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
            this.overrideSpellFormat = new System.Windows.Forms.CheckBox();
            this.buffDebuffOptions = new System.Windows.Forms.Button();
            this.damageTakenOverhead = new System.Windows.Forms.CheckBox();
            this.showDamageTaken = new System.Windows.Forms.CheckBox();
            this.damageDealtOverhead = new System.Windows.Forms.CheckBox();
            this.showDamageDealt = new System.Windows.Forms.CheckBox();
            this.showBuffDebuffOverhead = new System.Windows.Forms.CheckBox();
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
            this.editFilterText = new System.Windows.Forms.Button();
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
            this.chkAlt = new System.Windows.Forms.CheckBox();
            this.chkPass = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.unsetHK = new System.Windows.Forms.Button();
            this.setHK = new System.Windows.Forms.Button();
            this.key = new System.Windows.Forms.TextBox();
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
            this.scriptEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.scriptGuide = new System.Windows.Forms.Button();
            this.saveScript = new System.Windows.Forms.Button();
            this.newScript = new System.Windows.Forms.Button();
            this.setScriptHotkey = new System.Windows.Forms.Button();
            this.recordScript = new System.Windows.Forms.Button();
            this.playScript = new System.Windows.Forms.Button();
            this.subScriptOptions = new System.Windows.Forms.TabPage();
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
            this.groupBox16.SuspendLayout();
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
            this.tabs.Location = new System.Drawing.Point(2, 0);
            this.tabs.Multiline = true;
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(527, 370);
            this.tabs.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabs.TabIndex = 0;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_IndexChanged);
            this.tabs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabs_KeyDown);
            // 
            // generalTab
            // 
            this.generalTab.Controls.Add(this.subGeneralTab);
            this.generalTab.Location = new System.Drawing.Point(4, 44);
            this.generalTab.Name = "generalTab";
            this.generalTab.Size = new System.Drawing.Size(519, 322);
            this.generalTab.TabIndex = 0;
            this.generalTab.Text = "General";
            // 
            // subGeneralTab
            // 
            this.subGeneralTab.Controls.Add(this.subGenTab);
            this.subGeneralTab.Location = new System.Drawing.Point(6, 3);
            this.subGeneralTab.Name = "subGeneralTab";
            this.subGeneralTab.SelectedIndex = 0;
            this.subGeneralTab.Size = new System.Drawing.Size(510, 314);
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
            this.subGenTab.Location = new System.Drawing.Point(4, 24);
            this.subGenTab.Name = "subGenTab";
            this.subGenTab.Padding = new System.Windows.Forms.Padding(3);
            this.subGenTab.Size = new System.Drawing.Size(502, 286);
            this.subGenTab.TabIndex = 0;
            this.subGenTab.Text = "General";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.boatControl);
            this.groupBox15.Controls.Add(this.btnMap);
            this.groupBox15.Location = new System.Drawing.Point(14, 232);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(223, 48);
            this.groupBox15.TabIndex = 77;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Map / Boat";
            // 
            // boatControl
            // 
            this.boatControl.Location = new System.Drawing.Point(119, 16);
            this.boatControl.Name = "boatControl";
            this.boatControl.Size = new System.Drawing.Size(96, 26);
            this.boatControl.TabIndex = 70;
            this.boatControl.Text = "Boat Control";
            this.boatControl.UseVisualStyleBackColor = true;
            this.boatControl.Click += new System.EventHandler(this.boatControl_Click);
            // 
            // btnMap
            // 
            this.btnMap.Location = new System.Drawing.Point(6, 16);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(107, 26);
            this.btnMap.TabIndex = 67;
            this.btnMap.Text = "UOPS";
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // systray
            // 
            this.systray.Location = new System.Drawing.Point(164, 118);
            this.systray.Name = "systray";
            this.systray.Size = new System.Drawing.Size(88, 23);
            this.systray.TabIndex = 69;
            this.systray.Text = "System Tray";
            this.systray.CheckedChanged += new System.EventHandler(this.systray_CheckedChanged);
            // 
            // taskbar
            // 
            this.taskbar.Location = new System.Drawing.Point(92, 118);
            this.taskbar.Name = "taskbar";
            this.taskbar.Size = new System.Drawing.Size(79, 23);
            this.taskbar.TabIndex = 68;
            this.taskbar.Text = "Taskbar";
            this.taskbar.CheckedChanged += new System.EventHandler(this.taskbar_CheckedChanged);
            // 
            // langSel
            // 
            this.langSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.langSel.Location = new System.Drawing.Point(341, 169);
            this.langSel.Name = "langSel";
            this.langSel.Size = new System.Drawing.Size(136, 23);
            this.langSel.TabIndex = 71;
            this.langSel.SelectedIndexChanged += new System.EventHandler(this.langSel_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(267, 172);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 18);
            this.label7.TabIndex = 70;
            this.label7.Text = "Language:";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(11, 122);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 16);
            this.label11.TabIndex = 67;
            this.label11.Text = "Show in:";
            // 
            // showWelcome
            // 
            this.showWelcome.Location = new System.Drawing.Point(270, 107);
            this.showWelcome.Name = "showWelcome";
            this.showWelcome.Size = new System.Drawing.Size(152, 23);
            this.showWelcome.TabIndex = 66;
            this.showWelcome.Text = "Show Welcome Screen";
            this.showWelcome.CheckedChanged += new System.EventHandler(this.showWelcome_CheckedChanged);
            // 
            // opacity
            // 
            this.opacity.AutoSize = false;
            this.opacity.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.opacity.Location = new System.Drawing.Point(97, 150);
            this.opacity.Maximum = 100;
            this.opacity.Minimum = 10;
            this.opacity.Name = "opacity";
            this.opacity.Size = new System.Drawing.Size(155, 21);
            this.opacity.TabIndex = 64;
            this.opacity.TickFrequency = 0;
            this.opacity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.opacity.Value = 100;
            this.opacity.Scroll += new System.EventHandler(this.opacity_Scroll);
            // 
            // alwaysTop
            // 
            this.alwaysTop.Location = new System.Drawing.Point(270, 136);
            this.alwaysTop.Name = "alwaysTop";
            this.alwaysTop.Size = new System.Drawing.Size(162, 23);
            this.alwaysTop.TabIndex = 63;
            this.alwaysTop.Text = "Use Smart Always on Top";
            this.alwaysTop.CheckedChanged += new System.EventHandler(this.alwaysTop_CheckedChanged);
            // 
            // opacityLabel
            // 
            this.opacityLabel.Location = new System.Drawing.Point(11, 152);
            this.opacityLabel.Name = "opacityLabel";
            this.opacityLabel.Size = new System.Drawing.Size(58, 19);
            this.opacityLabel.TabIndex = 65;
            this.opacityLabel.Text = "Opacity: 100%";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.saveProfile);
            this.groupBox4.Controls.Add(this.cloneProfile);
            this.groupBox4.Controls.Add(this.delProfile);
            this.groupBox4.Controls.Add(this.newProfile);
            this.groupBox4.Controls.Add(this.profiles);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(490, 95);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Profiles";
            // 
            // saveProfile
            // 
            this.saveProfile.Location = new System.Drawing.Point(320, 56);
            this.saveProfile.Name = "saveProfile";
            this.saveProfile.Size = new System.Drawing.Size(50, 30);
            this.saveProfile.TabIndex = 4;
            this.saveProfile.Text = "Save";
            this.saveProfile.Click += new System.EventHandler(this.saveProfile_Click);
            // 
            // cloneProfile
            // 
            this.cloneProfile.Location = new System.Drawing.Point(376, 56);
            this.cloneProfile.Name = "cloneProfile";
            this.cloneProfile.Size = new System.Drawing.Size(50, 30);
            this.cloneProfile.TabIndex = 3;
            this.cloneProfile.Text = "Clone";
            this.cloneProfile.Click += new System.EventHandler(this.cloneProfile_Click);
            // 
            // delProfile
            // 
            this.delProfile.Location = new System.Drawing.Point(432, 56);
            this.delProfile.Name = "delProfile";
            this.delProfile.Size = new System.Drawing.Size(50, 30);
            this.delProfile.TabIndex = 2;
            this.delProfile.Text = "Delete";
            this.delProfile.Click += new System.EventHandler(this.delProfile_Click);
            // 
            // newProfile
            // 
            this.newProfile.Location = new System.Drawing.Point(264, 56);
            this.newProfile.Name = "newProfile";
            this.newProfile.Size = new System.Drawing.Size(50, 30);
            this.newProfile.TabIndex = 1;
            this.newProfile.Text = "New";
            this.newProfile.Click += new System.EventHandler(this.newProfile_Click);
            // 
            // profiles
            // 
            this.profiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profiles.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profiles.ItemHeight = 20;
            this.profiles.Location = new System.Drawing.Point(6, 22);
            this.profiles.MaxDropDownItems = 5;
            this.profiles.Name = "profiles";
            this.profiles.Size = new System.Drawing.Size(476, 28);
            this.profiles.TabIndex = 0;
            this.profiles.SelectedIndexChanged += new System.EventHandler(this.profiles_SelectedIndexChanged);
            // 
            // moreOptTab
            // 
            this.moreOptTab.Controls.Add(this.optionsTabCtrl);
            this.moreOptTab.Location = new System.Drawing.Point(4, 44);
            this.moreOptTab.Name = "moreOptTab";
            this.moreOptTab.Size = new System.Drawing.Size(519, 322);
            this.moreOptTab.TabIndex = 5;
            this.moreOptTab.Text = "Options";
            // 
            // optionsTabCtrl
            // 
            this.optionsTabCtrl.Controls.Add(this.subOptionsSpeechTab);
            this.optionsTabCtrl.Controls.Add(this.subOptionsTargetTab);
            this.optionsTabCtrl.Controls.Add(this.subOptionsMiscTab);
            this.optionsTabCtrl.Location = new System.Drawing.Point(6, 3);
            this.optionsTabCtrl.Name = "optionsTabCtrl";
            this.optionsTabCtrl.SelectedIndex = 0;
            this.optionsTabCtrl.Size = new System.Drawing.Size(510, 314);
            this.optionsTabCtrl.TabIndex = 93;
            // 
            // subOptionsSpeechTab
            // 
            this.subOptionsSpeechTab.BackColor = System.Drawing.SystemColors.Control;
            this.subOptionsSpeechTab.Controls.Add(this.overrideSpellFormat);
            this.subOptionsSpeechTab.Controls.Add(this.buffDebuffOptions);
            this.subOptionsSpeechTab.Controls.Add(this.damageTakenOverhead);
            this.subOptionsSpeechTab.Controls.Add(this.showDamageTaken);
            this.subOptionsSpeechTab.Controls.Add(this.damageDealtOverhead);
            this.subOptionsSpeechTab.Controls.Add(this.showDamageDealt);
            this.subOptionsSpeechTab.Controls.Add(this.showBuffDebuffOverhead);
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
            this.subOptionsSpeechTab.Location = new System.Drawing.Point(4, 24);
            this.subOptionsSpeechTab.Name = "subOptionsSpeechTab";
            this.subOptionsSpeechTab.Padding = new System.Windows.Forms.Padding(3);
            this.subOptionsSpeechTab.Size = new System.Drawing.Size(502, 286);
            this.subOptionsSpeechTab.TabIndex = 0;
            this.subOptionsSpeechTab.Text = "Speech & Messages  ";
            // 
            // overrideSpellFormat
            // 
            this.overrideSpellFormat.Location = new System.Drawing.Point(9, 205);
            this.overrideSpellFormat.Name = "overrideSpellFormat";
            this.overrideSpellFormat.Size = new System.Drawing.Size(152, 20);
            this.overrideSpellFormat.TabIndex = 130;
            this.overrideSpellFormat.Text = "Override spell format";
            this.overrideSpellFormat.CheckedChanged += new System.EventHandler(this.overrideSpellFormat_CheckedChanged);
            // 
            // buffDebuffOptions
            // 
            this.buffDebuffOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buffDebuffOptions.Location = new System.Drawing.Point(438, 209);
            this.buffDebuffOptions.Name = "buffDebuffOptions";
            this.buffDebuffOptions.Size = new System.Drawing.Size(33, 19);
            this.buffDebuffOptions.TabIndex = 129;
            this.buffDebuffOptions.Text = "...";
            this.buffDebuffOptions.UseVisualStyleBackColor = true;
            this.buffDebuffOptions.Click += new System.EventHandler(this.BuffDebuffOptions_Click);
            // 
            // damageTakenOverhead
            // 
            this.damageTakenOverhead.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.damageTakenOverhead.Location = new System.Drawing.Point(394, 185);
            this.damageTakenOverhead.Name = "damageTakenOverhead";
            this.damageTakenOverhead.Size = new System.Drawing.Size(77, 19);
            this.damageTakenOverhead.TabIndex = 128;
            this.damageTakenOverhead.Text = "Overhead";
            this.damageTakenOverhead.UseVisualStyleBackColor = true;
            this.damageTakenOverhead.CheckedChanged += new System.EventHandler(this.damageTakenOverhead_CheckedChanged);
            // 
            // showDamageTaken
            // 
            this.showDamageTaken.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.showDamageTaken.Location = new System.Drawing.Point(260, 185);
            this.showDamageTaken.Name = "showDamageTaken";
            this.showDamageTaken.Size = new System.Drawing.Size(139, 19);
            this.showDamageTaken.TabIndex = 127;
            this.showDamageTaken.Text = "Show damage taken";
            this.showDamageTaken.UseVisualStyleBackColor = true;
            this.showDamageTaken.CheckedChanged += new System.EventHandler(this.showDamageTaken_CheckedChanged);
            // 
            // damageDealtOverhead
            // 
            this.damageDealtOverhead.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.damageDealtOverhead.Location = new System.Drawing.Point(394, 160);
            this.damageDealtOverhead.Name = "damageDealtOverhead";
            this.damageDealtOverhead.Size = new System.Drawing.Size(77, 19);
            this.damageDealtOverhead.TabIndex = 126;
            this.damageDealtOverhead.Text = "Overhead";
            this.damageDealtOverhead.UseVisualStyleBackColor = true;
            this.damageDealtOverhead.CheckedChanged += new System.EventHandler(this.damageDealtOverhead_CheckedChanged);
            // 
            // showDamageDealt
            // 
            this.showDamageDealt.AutoSize = true;
            this.showDamageDealt.Location = new System.Drawing.Point(260, 160);
            this.showDamageDealt.Name = "showDamageDealt";
            this.showDamageDealt.Size = new System.Drawing.Size(130, 19);
            this.showDamageDealt.TabIndex = 125;
            this.showDamageDealt.Text = "Show damage dealt";
            this.showDamageDealt.UseVisualStyleBackColor = true;
            this.showDamageDealt.CheckedChanged += new System.EventHandler(this.showDamageDealt_CheckedChanged);
            // 
            // showBuffDebuffOverhead
            // 
            this.showBuffDebuffOverhead.AutoSize = true;
            this.showBuffDebuffOverhead.Location = new System.Drawing.Point(260, 210);
            this.showBuffDebuffOverhead.Name = "showBuffDebuffOverhead";
            this.showBuffDebuffOverhead.Size = new System.Drawing.Size(172, 19);
            this.showBuffDebuffOverhead.TabIndex = 91;
            this.showBuffDebuffOverhead.Text = "Show buff/debuff overhead";
            this.showBuffDebuffOverhead.UseVisualStyleBackColor = true;
            this.showBuffDebuffOverhead.CheckedChanged += new System.EventHandler(this.showBuffDebuffOverhead_CheckedChanged);
            // 
            // healthFmt
            // 
            this.healthFmt.Location = new System.Drawing.Point(377, 110);
            this.healthFmt.Name = "healthFmt";
            this.healthFmt.Size = new System.Drawing.Size(53, 23);
            this.healthFmt.TabIndex = 89;
            this.healthFmt.Text = "[{0}%]";
            this.healthFmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.healthFmt.TextChanged += new System.EventHandler(this.healthFmt_TextChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(283, 111);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 18);
            this.label10.TabIndex = 88;
            this.label10.Text = "Health Format:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // showHealthOH
            // 
            this.showHealthOH.Location = new System.Drawing.Point(260, 88);
            this.showHealthOH.Name = "showHealthOH";
            this.showHealthOH.Size = new System.Drawing.Size(231, 20);
            this.showHealthOH.TabIndex = 87;
            this.showHealthOH.Text = "Show health above people/creatures";
            this.showHealthOH.CheckedChanged += new System.EventHandler(this.showHealthOH_CheckedChanged);
            // 
            // chkPartyOverhead
            // 
            this.chkPartyOverhead.Location = new System.Drawing.Point(260, 134);
            this.chkPartyOverhead.Name = "chkPartyOverhead";
            this.chkPartyOverhead.Size = new System.Drawing.Size(238, 20);
            this.chkPartyOverhead.TabIndex = 90;
            this.chkPartyOverhead.Text = "Show mana/stam above party members";
            this.chkPartyOverhead.CheckedChanged += new System.EventHandler(this.chkPartyOverhead_CheckedChanged);
            // 
            // containerLabels
            // 
            this.containerLabels.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerLabels.Location = new System.Drawing.Point(438, 63);
            this.containerLabels.Name = "containerLabels";
            this.containerLabels.Size = new System.Drawing.Size(33, 19);
            this.containerLabels.TabIndex = 86;
            this.containerLabels.Text = "...";
            this.containerLabels.UseVisualStyleBackColor = true;
            this.containerLabels.Click += new System.EventHandler(this.containerLabels_Click);
            // 
            // showContainerLabels
            // 
            this.showContainerLabels.AutoSize = true;
            this.showContainerLabels.Location = new System.Drawing.Point(260, 63);
            this.showContainerLabels.Name = "showContainerLabels";
            this.showContainerLabels.Size = new System.Drawing.Size(141, 19);
            this.showContainerLabels.TabIndex = 85;
            this.showContainerLabels.Text = "Show container labels";
            this.showContainerLabels.UseVisualStyleBackColor = true;
            this.showContainerLabels.CheckedChanged += new System.EventHandler(this.showContainerLabels_CheckedChanged);
            // 
            // incomingMob
            // 
            this.incomingMob.Location = new System.Drawing.Point(260, 10);
            this.incomingMob.Name = "incomingMob";
            this.incomingMob.Size = new System.Drawing.Size(211, 20);
            this.incomingMob.TabIndex = 71;
            this.incomingMob.Text = "Show Names of Incoming People/Creatures";
            this.incomingMob.CheckedChanged += new System.EventHandler(this.incomingMob_CheckedChanged);
            // 
            // incomingCorpse
            // 
            this.incomingCorpse.Location = new System.Drawing.Point(260, 36);
            this.incomingCorpse.Name = "incomingCorpse";
            this.incomingCorpse.Size = new System.Drawing.Size(238, 20);
            this.incomingCorpse.TabIndex = 72;
            this.incomingCorpse.Text = "Show Names of New/Incoming Corpses";
            this.incomingCorpse.CheckedChanged += new System.EventHandler(this.incomingCorpse_CheckedChanged);
            // 
            // setLTHilight
            // 
            this.setLTHilight.Location = new System.Drawing.Point(168, 116);
            this.setLTHilight.Name = "setLTHilight";
            this.setLTHilight.Size = new System.Drawing.Size(47, 20);
            this.setLTHilight.TabIndex = 70;
            this.setLTHilight.Text = "Set";
            this.setLTHilight.Click += new System.EventHandler(this.setLTHilight_Click);
            // 
            // lthilight
            // 
            this.lthilight.Location = new System.Drawing.Point(9, 116);
            this.lthilight.Name = "lthilight";
            this.lthilight.Size = new System.Drawing.Size(206, 19);
            this.lthilight.TabIndex = 69;
            this.lthilight.Text = "Last Target Highlight:";
            this.lthilight.CheckedChanged += new System.EventHandler(this.lthilight_CheckedChanged);
            // 
            // setHarmHue
            // 
            this.setHarmHue.Enabled = false;
            this.setHarmHue.Location = new System.Drawing.Point(99, 179);
            this.setHarmHue.Name = "setHarmHue";
            this.setHarmHue.Size = new System.Drawing.Size(32, 20);
            this.setHarmHue.TabIndex = 64;
            this.setHarmHue.Text = "Set";
            this.setHarmHue.Click += new System.EventHandler(this.setHarmHue_Click);
            // 
            // setNeuHue
            // 
            this.setNeuHue.Enabled = false;
            this.setNeuHue.Location = new System.Drawing.Point(172, 179);
            this.setNeuHue.Name = "setNeuHue";
            this.setNeuHue.Size = new System.Drawing.Size(32, 20);
            this.setNeuHue.TabIndex = 65;
            this.setNeuHue.Text = "Set";
            this.setNeuHue.Click += new System.EventHandler(this.setNeuHue_Click);
            // 
            // lblHarmHue
            // 
            this.lblHarmHue.Location = new System.Drawing.Point(86, 165);
            this.lblHarmHue.Name = "lblHarmHue";
            this.lblHarmHue.Size = new System.Drawing.Size(59, 14);
            this.lblHarmHue.TabIndex = 68;
            this.lblHarmHue.Text = "Harmful";
            this.lblHarmHue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNeuHue
            // 
            this.lblNeuHue.Location = new System.Drawing.Point(163, 165);
            this.lblNeuHue.Name = "lblNeuHue";
            this.lblNeuHue.Size = new System.Drawing.Size(52, 14);
            this.lblNeuHue.TabIndex = 67;
            this.lblNeuHue.Text = "Neutral";
            this.lblNeuHue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBeneHue
            // 
            this.lblBeneHue.Location = new System.Drawing.Point(9, 165);
            this.lblBeneHue.Name = "lblBeneHue";
            this.lblBeneHue.Size = new System.Drawing.Size(66, 14);
            this.lblBeneHue.TabIndex = 66;
            this.lblBeneHue.Text = "Beneficial";
            this.lblBeneHue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // setBeneHue
            // 
            this.setBeneHue.Enabled = false;
            this.setBeneHue.Location = new System.Drawing.Point(27, 179);
            this.setBeneHue.Name = "setBeneHue";
            this.setBeneHue.Size = new System.Drawing.Size(32, 20);
            this.setBeneHue.TabIndex = 63;
            this.setBeneHue.Text = "Set";
            this.setBeneHue.Click += new System.EventHandler(this.setBeneHue_Click);
            // 
            // setSpeechHue
            // 
            this.setSpeechHue.Location = new System.Drawing.Point(168, 90);
            this.setSpeechHue.Name = "setSpeechHue";
            this.setSpeechHue.Size = new System.Drawing.Size(47, 20);
            this.setSpeechHue.TabIndex = 62;
            this.setSpeechHue.Text = "Set";
            this.setSpeechHue.Click += new System.EventHandler(this.setSpeechHue_Click);
            // 
            // setWarnHue
            // 
            this.setWarnHue.Location = new System.Drawing.Point(168, 64);
            this.setWarnHue.Name = "setWarnHue";
            this.setWarnHue.Size = new System.Drawing.Size(47, 20);
            this.setWarnHue.TabIndex = 61;
            this.setWarnHue.Text = "Set";
            this.setWarnHue.Click += new System.EventHandler(this.setWarnHue_Click);
            // 
            // setMsgHue
            // 
            this.setMsgHue.Location = new System.Drawing.Point(168, 38);
            this.setMsgHue.Name = "setMsgHue";
            this.setMsgHue.Size = new System.Drawing.Size(47, 20);
            this.setMsgHue.TabIndex = 60;
            this.setMsgHue.Text = "Set";
            this.setMsgHue.Click += new System.EventHandler(this.setMsgHue_Click);
            // 
            // setExHue
            // 
            this.setExHue.Location = new System.Drawing.Point(168, 12);
            this.setExHue.Name = "setExHue";
            this.setExHue.Size = new System.Drawing.Size(47, 20);
            this.setExHue.TabIndex = 59;
            this.setExHue.Text = "Set";
            this.setExHue.Click += new System.EventHandler(this.setExHue_Click);
            // 
            // lblWarnHue
            // 
            this.lblWarnHue.Location = new System.Drawing.Point(9, 64);
            this.lblWarnHue.Name = "lblWarnHue";
            this.lblWarnHue.Size = new System.Drawing.Size(206, 18);
            this.lblWarnHue.TabIndex = 58;
            this.lblWarnHue.Text = "Warning Message Hue:";
            // 
            // lblMsgHue
            // 
            this.lblMsgHue.Location = new System.Drawing.Point(9, 38);
            this.lblMsgHue.Name = "lblMsgHue";
            this.lblMsgHue.Size = new System.Drawing.Size(206, 18);
            this.lblMsgHue.TabIndex = 57;
            this.lblMsgHue.Text = "Razor Message Hue:";
            // 
            // lblExHue
            // 
            this.lblExHue.Location = new System.Drawing.Point(9, 12);
            this.lblExHue.Name = "lblExHue";
            this.lblExHue.Size = new System.Drawing.Size(206, 18);
            this.lblExHue.TabIndex = 56;
            this.lblExHue.Text = "Search Exemption Hue:";
            // 
            // txtSpellFormat
            // 
            this.txtSpellFormat.Location = new System.Drawing.Point(89, 228);
            this.txtSpellFormat.Name = "txtSpellFormat";
            this.txtSpellFormat.Size = new System.Drawing.Size(126, 23);
            this.txtSpellFormat.TabIndex = 55;
            this.txtSpellFormat.TextChanged += new System.EventHandler(this.txtSpellFormat_TextChanged);
            // 
            // chkForceSpellHue
            // 
            this.chkForceSpellHue.Location = new System.Drawing.Point(9, 142);
            this.chkForceSpellHue.Name = "chkForceSpellHue";
            this.chkForceSpellHue.Size = new System.Drawing.Size(152, 20);
            this.chkForceSpellHue.TabIndex = 53;
            this.chkForceSpellHue.Text = "Override Spell Hues:";
            this.chkForceSpellHue.CheckedChanged += new System.EventHandler(this.chkForceSpellHue_CheckedChanged);
            // 
            // chkForceSpeechHue
            // 
            this.chkForceSpeechHue.Location = new System.Drawing.Point(9, 90);
            this.chkForceSpeechHue.Name = "chkForceSpeechHue";
            this.chkForceSpeechHue.Size = new System.Drawing.Size(206, 19);
            this.chkForceSpeechHue.TabIndex = 52;
            this.chkForceSpeechHue.Text = "Override Speech Hue:";
            this.chkForceSpeechHue.CheckedChanged += new System.EventHandler(this.chkForceSpeechHue_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 20);
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
            this.subOptionsTargetTab.Location = new System.Drawing.Point(4, 22);
            this.subOptionsTargetTab.Name = "subOptionsTargetTab";
            this.subOptionsTargetTab.Padding = new System.Windows.Forms.Padding(3);
            this.subOptionsTargetTab.Size = new System.Drawing.Size(502, 288);
            this.subOptionsTargetTab.TabIndex = 1;
            this.subOptionsTargetTab.Text = "Targeting & Queues  ";
            // 
            // groupSmartTarget
            // 
            this.groupSmartTarget.Controls.Add(this.nonFriendlyHarmfulOnly);
            this.groupSmartTarget.Controls.Add(this.friendBeneficialOnly);
            this.groupSmartTarget.Controls.Add(this.onlyNextPrevBeneficial);
            this.groupSmartTarget.Controls.Add(this.smartLT);
            this.groupSmartTarget.Location = new System.Drawing.Point(257, 11);
            this.groupSmartTarget.Name = "groupSmartTarget";
            this.groupSmartTarget.Size = new System.Drawing.Size(239, 126);
            this.groupSmartTarget.TabIndex = 138;
            this.groupSmartTarget.TabStop = false;
            this.groupSmartTarget.Text = "Smart Targeting:";
            // 
            // nonFriendlyHarmfulOnly
            // 
            this.nonFriendlyHarmfulOnly.Location = new System.Drawing.Point(6, 98);
            this.nonFriendlyHarmfulOnly.Name = "nonFriendlyHarmfulOnly";
            this.nonFriendlyHarmfulOnly.Size = new System.Drawing.Size(232, 19);
            this.nonFriendlyHarmfulOnly.TabIndex = 141;
            this.nonFriendlyHarmfulOnly.Text = "\'Next/Prev Non-Friendly\' harmful only";
            this.nonFriendlyHarmfulOnly.UseVisualStyleBackColor = true;
            this.nonFriendlyHarmfulOnly.CheckedChanged += new System.EventHandler(this.nonFriendlyHarmfulOnly_CheckedChanged);
            // 
            // friendBeneficialOnly
            // 
            this.friendBeneficialOnly.AutoSize = true;
            this.friendBeneficialOnly.Location = new System.Drawing.Point(6, 73);
            this.friendBeneficialOnly.Name = "friendBeneficialOnly";
            this.friendBeneficialOnly.Size = new System.Drawing.Size(233, 19);
            this.friendBeneficialOnly.TabIndex = 140;
            this.friendBeneficialOnly.Text = "\'Next/Prev Friendly\' sets beneficial only";
            this.friendBeneficialOnly.UseVisualStyleBackColor = true;
            this.friendBeneficialOnly.CheckedChanged += new System.EventHandler(this.friendBeneficialOnly_CheckedChanged);
            // 
            // onlyNextPrevBeneficial
            // 
            this.onlyNextPrevBeneficial.AutoSize = true;
            this.onlyNextPrevBeneficial.Location = new System.Drawing.Point(6, 48);
            this.onlyNextPrevBeneficial.Name = "onlyNextPrevBeneficial";
            this.onlyNextPrevBeneficial.Size = new System.Drawing.Size(224, 19);
            this.onlyNextPrevBeneficial.TabIndex = 139;
            this.onlyNextPrevBeneficial.Text = "\'Next/Prev Friend\' sets beneficial only";
            this.onlyNextPrevBeneficial.UseVisualStyleBackColor = true;
            this.onlyNextPrevBeneficial.CheckedChanged += new System.EventHandler(this.onlyNextPrevBeneficial_CheckedChanged);
            // 
            // smartLT
            // 
            this.smartLT.Location = new System.Drawing.Point(6, 22);
            this.smartLT.Name = "smartLT";
            this.smartLT.Size = new System.Drawing.Size(212, 20);
            this.smartLT.TabIndex = 138;
            this.smartLT.Text = "Use Smart Last Target";
            this.smartLT.CheckedChanged += new System.EventHandler(this.smartLT_CheckedChanged);
            // 
            // setTargetIndicatorHue
            // 
            this.setTargetIndicatorHue.Location = new System.Drawing.Point(177, 246);
            this.setTargetIndicatorHue.Name = "setTargetIndicatorHue";
            this.setTargetIndicatorHue.Size = new System.Drawing.Size(59, 23);
            this.setTargetIndicatorHue.TabIndex = 134;
            this.setTargetIndicatorHue.Text = "Set Hue";
            this.setTargetIndicatorHue.UseVisualStyleBackColor = true;
            this.setTargetIndicatorHue.Click += new System.EventHandler(this.setTargetIndicatorHue_Click);
            // 
            // targetIndicatorFormat
            // 
            this.targetIndicatorFormat.Location = new System.Drawing.Point(64, 246);
            this.targetIndicatorFormat.Name = "targetIndicatorFormat";
            this.targetIndicatorFormat.Size = new System.Drawing.Size(107, 23);
            this.targetIndicatorFormat.TabIndex = 93;
            this.targetIndicatorFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.targetIndicatorFormat.TextChanged += new System.EventHandler(this.targetIndicatorFormat_TextChanged);
            // 
            // showtargtext
            // 
            this.showtargtext.Location = new System.Drawing.Point(9, 170);
            this.showtargtext.Name = "showtargtext";
            this.showtargtext.Size = new System.Drawing.Size(212, 20);
            this.showtargtext.TabIndex = 91;
            this.showtargtext.Text = "Show target flag on single click";
            this.showtargtext.CheckedChanged += new System.EventHandler(this.showtargtext_CheckedChanged);
            // 
            // showAttackTargetNewOnly
            // 
            this.showAttackTargetNewOnly.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showAttackTargetNewOnly.Location = new System.Drawing.Point(195, 196);
            this.showAttackTargetNewOnly.Name = "showAttackTargetNewOnly";
            this.showAttackTargetNewOnly.Size = new System.Drawing.Size(121, 44);
            this.showAttackTargetNewOnly.TabIndex = 90;
            this.showAttackTargetNewOnly.Text = "New targets only";
            this.showAttackTargetNewOnly.UseVisualStyleBackColor = true;
            this.showAttackTargetNewOnly.CheckedChanged += new System.EventHandler(this.showAttackTargetNewOnly_CheckedChanged);
            // 
            // showTextTargetIndicator
            // 
            this.showTextTargetIndicator.Location = new System.Drawing.Point(9, 221);
            this.showTextTargetIndicator.Name = "showTextTargetIndicator";
            this.showTextTargetIndicator.Size = new System.Drawing.Size(232, 19);
            this.showTextTargetIndicator.TabIndex = 89;
            this.showTextTargetIndicator.Text = "Show text target indicator";
            this.showTextTargetIndicator.UseVisualStyleBackColor = true;
            this.showTextTargetIndicator.CheckedChanged += new System.EventHandler(this.showTextTargetIndicator_CheckedChanged);
            // 
            // showAttackTarget
            // 
            this.showAttackTarget.Location = new System.Drawing.Point(9, 196);
            this.showAttackTarget.Name = "showAttackTarget";
            this.showAttackTarget.Size = new System.Drawing.Size(232, 19);
            this.showAttackTarget.TabIndex = 88;
            this.showAttackTarget.Text = "Attack/target name overhead";
            this.showAttackTarget.UseVisualStyleBackColor = true;
            this.showAttackTarget.CheckedChanged += new System.EventHandler(this.showAttackTarget_CheckedChanged);
            // 
            // showTargetMessagesOverChar
            // 
            this.showTargetMessagesOverChar.AutoSize = true;
            this.showTargetMessagesOverChar.Location = new System.Drawing.Point(9, 118);
            this.showTargetMessagesOverChar.Name = "showTargetMessagesOverChar";
            this.showTargetMessagesOverChar.Size = new System.Drawing.Size(224, 19);
            this.showTargetMessagesOverChar.TabIndex = 74;
            this.showTargetMessagesOverChar.Text = "Show Target Self/Last/Clear Overhead";
            this.showTargetMessagesOverChar.UseVisualStyleBackColor = true;
            this.showTargetMessagesOverChar.CheckedChanged += new System.EventHandler(this.showTargetMessagesOverChar_CheckedChanged);
            // 
            // txtObjDelay
            // 
            this.txtObjDelay.Location = new System.Drawing.Point(109, 89);
            this.txtObjDelay.Name = "txtObjDelay";
            this.txtObjDelay.Size = new System.Drawing.Size(32, 23);
            this.txtObjDelay.TabIndex = 56;
            this.txtObjDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtObjDelay.TextChanged += new System.EventHandler(this.txtObjDelay_TextChanged);
            // 
            // objectDelay
            // 
            this.objectDelay.Location = new System.Drawing.Point(9, 88);
            this.objectDelay.Name = "objectDelay";
            this.objectDelay.Size = new System.Drawing.Size(104, 24);
            this.objectDelay.TabIndex = 53;
            this.objectDelay.Text = "Object Delay:";
            this.objectDelay.CheckedChanged += new System.EventHandler(this.objectDelay_CheckedChanged);
            // 
            // ltRange
            // 
            this.ltRange.Location = new System.Drawing.Point(165, 141);
            this.ltRange.Name = "ltRange";
            this.ltRange.Size = new System.Drawing.Size(30, 23);
            this.ltRange.TabIndex = 59;
            this.ltRange.TextChanged += new System.EventHandler(this.ltRange_TextChanged);
            // 
            // QueueActions
            // 
            this.QueueActions.Location = new System.Drawing.Point(9, 64);
            this.QueueActions.Name = "QueueActions";
            this.QueueActions.Size = new System.Drawing.Size(202, 20);
            this.QueueActions.TabIndex = 54;
            this.QueueActions.Text = "Auto-Queue Object Delay actions ";
            this.QueueActions.CheckedChanged += new System.EventHandler(this.QueueActions_CheckedChanged);
            // 
            // rangeCheckLT
            // 
            this.rangeCheckLT.Location = new System.Drawing.Point(9, 142);
            this.rangeCheckLT.Name = "rangeCheckLT";
            this.rangeCheckLT.Size = new System.Drawing.Size(162, 20);
            this.rangeCheckLT.TabIndex = 58;
            this.rangeCheckLT.Text = "Range check Last Target:";
            this.rangeCheckLT.CheckedChanged += new System.EventHandler(this.rangeCheckLT_CheckedChanged);
            // 
            // actionStatusMsg
            // 
            this.actionStatusMsg.Location = new System.Drawing.Point(9, 38);
            this.actionStatusMsg.Name = "actionStatusMsg";
            this.actionStatusMsg.Size = new System.Drawing.Size(212, 20);
            this.actionStatusMsg.TabIndex = 57;
            this.actionStatusMsg.Text = "Show Action-Queue status messages";
            this.actionStatusMsg.CheckedChanged += new System.EventHandler(this.actionStatusMsg_CheckedChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(201, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 18);
            this.label8.TabIndex = 60;
            this.label8.Text = "tiles";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(147, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 18);
            this.label6.TabIndex = 55;
            this.label6.Text = "ms";
            // 
            // queueTargets
            // 
            this.queueTargets.Location = new System.Drawing.Point(9, 12);
            this.queueTargets.Name = "queueTargets";
            this.queueTargets.Size = new System.Drawing.Size(228, 20);
            this.queueTargets.TabIndex = 35;
            this.queueTargets.Text = "Queue LastTarget and TargetSelf";
            this.queueTargets.CheckedChanged += new System.EventHandler(this.queueTargets_CheckedChanged);
            // 
            // lblTargetFormat
            // 
            this.lblTargetFormat.Location = new System.Drawing.Point(10, 246);
            this.lblTargetFormat.Name = "lblTargetFormat";
            this.lblTargetFormat.Size = new System.Drawing.Size(140, 23);
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
            this.subOptionsMiscTab.Location = new System.Drawing.Point(4, 22);
            this.subOptionsMiscTab.Name = "subOptionsMiscTab";
            this.subOptionsMiscTab.Size = new System.Drawing.Size(502, 288);
            this.subOptionsMiscTab.TabIndex = 2;
            this.subOptionsMiscTab.Text = "Additional Options  ";
            // 
            // buyAgentIgnoreGold
            // 
            this.buyAgentIgnoreGold.AutoSize = true;
            this.buyAgentIgnoreGold.Location = new System.Drawing.Point(260, 244);
            this.buyAgentIgnoreGold.Name = "buyAgentIgnoreGold";
            this.buyAgentIgnoreGold.Size = new System.Drawing.Size(185, 19);
            this.buyAgentIgnoreGold.TabIndex = 126;
            this.buyAgentIgnoreGold.Text = "Buy Agents ignore player gold";
            this.buyAgentIgnoreGold.UseVisualStyleBackColor = true;
            this.buyAgentIgnoreGold.CheckedChanged += new System.EventHandler(this.buyAgentIgnoreGold_CheckedChanged);
            // 
            // reequipHandsPotion
            // 
            this.reequipHandsPotion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reequipHandsPotion.Location = new System.Drawing.Point(423, 140);
            this.reequipHandsPotion.Name = "reequipHandsPotion";
            this.reequipHandsPotion.Size = new System.Drawing.Size(69, 20);
            this.reequipHandsPotion.TabIndex = 125;
            this.reequipHandsPotion.Text = "Re-equip";
            this.reequipHandsPotion.CheckedChanged += new System.EventHandler(this.reequipHandsPotion_CheckedChanged);
            // 
            // autoOpenDoorWhenHidden
            // 
            this.autoOpenDoorWhenHidden.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoOpenDoorWhenHidden.Location = new System.Drawing.Point(393, 88);
            this.autoOpenDoorWhenHidden.Name = "autoOpenDoorWhenHidden";
            this.autoOpenDoorWhenHidden.Size = new System.Drawing.Size(95, 20);
            this.autoOpenDoorWhenHidden.TabIndex = 124;
            this.autoOpenDoorWhenHidden.Text = "When hidden";
            this.autoOpenDoorWhenHidden.CheckedChanged += new System.EventHandler(this.autoDoorWhenHidden_CheckedChanged);
            // 
            // lblStealthFormat
            // 
            this.lblStealthFormat.AutoSize = true;
            this.lblStealthFormat.Location = new System.Drawing.Point(280, 62);
            this.lblStealthFormat.Name = "lblStealthFormat";
            this.lblStealthFormat.Size = new System.Drawing.Size(48, 15);
            this.lblStealthFormat.TabIndex = 123;
            this.lblStealthFormat.Text = "Format:";
            // 
            // stealthStepsFormat
            // 
            this.stealthStepsFormat.Location = new System.Drawing.Point(334, 59);
            this.stealthStepsFormat.Name = "stealthStepsFormat";
            this.stealthStepsFormat.Size = new System.Drawing.Size(114, 23);
            this.stealthStepsFormat.TabIndex = 122;
            this.stealthStepsFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.stealthStepsFormat.TextChanged += new System.EventHandler(this.stealthStepsFormat_TextChanged);
            // 
            // rememberPwds
            // 
            this.rememberPwds.Location = new System.Drawing.Point(260, 12);
            this.rememberPwds.Name = "rememberPwds";
            this.rememberPwds.Size = new System.Drawing.Size(148, 20);
            this.rememberPwds.TabIndex = 121;
            this.rememberPwds.Text = "Remember passwords ";
            this.rememberPwds.CheckedChanged += new System.EventHandler(this.rememberPwds_CheckedChanged);
            // 
            // showStaticWalls
            // 
            this.showStaticWalls.AutoSize = true;
            this.showStaticWalls.Location = new System.Drawing.Point(260, 219);
            this.showStaticWalls.Name = "showStaticWalls";
            this.showStaticWalls.Size = new System.Drawing.Size(153, 19);
            this.showStaticWalls.TabIndex = 119;
            this.showStaticWalls.Text = "Static magic fields/walls";
            this.showStaticWalls.UseVisualStyleBackColor = true;
            this.showStaticWalls.CheckedChanged += new System.EventHandler(this.showStaticWalls_CheckedChanged);
            // 
            // showStaticWallLabels
            // 
            this.showStaticWallLabels.AutoSize = true;
            this.showStaticWallLabels.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showStaticWallLabels.Location = new System.Drawing.Point(421, 220);
            this.showStaticWallLabels.Name = "showStaticWallLabels";
            this.showStaticWallLabels.Size = new System.Drawing.Size(55, 17);
            this.showStaticWallLabels.TabIndex = 120;
            this.showStaticWallLabels.Text = "Labels";
            this.showStaticWallLabels.UseVisualStyleBackColor = true;
            this.showStaticWallLabels.CheckedChanged += new System.EventHandler(this.showStaticWallLabels_CheckedChanged);
            // 
            // stealthOverhead
            // 
            this.stealthOverhead.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stealthOverhead.Location = new System.Drawing.Point(393, 37);
            this.stealthOverhead.Name = "stealthOverhead";
            this.stealthOverhead.Size = new System.Drawing.Size(99, 20);
            this.stealthOverhead.TabIndex = 117;
            this.stealthOverhead.Text = "Overhead";
            this.stealthOverhead.UseVisualStyleBackColor = true;
            this.stealthOverhead.CheckedChanged += new System.EventHandler(this.stealthOverhead_CheckedChanged);
            // 
            // forceSizeX
            // 
            this.forceSizeX.Location = new System.Drawing.Point(387, 190);
            this.forceSizeX.Name = "forceSizeX";
            this.forceSizeX.Size = new System.Drawing.Size(34, 23);
            this.forceSizeX.TabIndex = 111;
            this.forceSizeX.TextChanged += new System.EventHandler(this.forceSizeX_TextChanged);
            // 
            // forceSizeY
            // 
            this.forceSizeY.Location = new System.Drawing.Point(443, 191);
            this.forceSizeY.Name = "forceSizeY";
            this.forceSizeY.Size = new System.Drawing.Size(33, 23);
            this.forceSizeY.TabIndex = 112;
            this.forceSizeY.TextChanged += new System.EventHandler(this.forceSizeY_TextChanged);
            // 
            // blockHealPoison
            // 
            this.blockHealPoison.Location = new System.Drawing.Point(260, 166);
            this.blockHealPoison.Name = "blockHealPoison";
            this.blockHealPoison.Size = new System.Drawing.Size(201, 20);
            this.blockHealPoison.TabIndex = 116;
            this.blockHealPoison.Text = "Block heal if target is poisoned";
            this.blockHealPoison.CheckedChanged += new System.EventHandler(this.blockHealPoison_CheckedChanged);
            // 
            // potionEquip
            // 
            this.potionEquip.Location = new System.Drawing.Point(260, 140);
            this.potionEquip.Name = "potionEquip";
            this.potionEquip.Size = new System.Drawing.Size(232, 20);
            this.potionEquip.TabIndex = 115;
            this.potionEquip.Text = "Auto Unequip for potions";
            this.potionEquip.CheckedChanged += new System.EventHandler(this.potionEquip_CheckedChanged);
            // 
            // spellUnequip
            // 
            this.spellUnequip.Location = new System.Drawing.Point(260, 114);
            this.spellUnequip.Name = "spellUnequip";
            this.spellUnequip.Size = new System.Drawing.Size(213, 20);
            this.spellUnequip.TabIndex = 108;
            this.spellUnequip.Text = "Auto Unequip hands before casting";
            this.spellUnequip.CheckedChanged += new System.EventHandler(this.spellUnequip_CheckedChanged);
            // 
            // autoOpenDoors
            // 
            this.autoOpenDoors.Location = new System.Drawing.Point(260, 88);
            this.autoOpenDoors.Name = "autoOpenDoors";
            this.autoOpenDoors.Size = new System.Drawing.Size(118, 20);
            this.autoOpenDoors.TabIndex = 110;
            this.autoOpenDoors.Text = "Auto-open doors";
            this.autoOpenDoors.CheckedChanged += new System.EventHandler(this.autoOpenDoors_CheckedChanged);
            // 
            // chkStealth
            // 
            this.chkStealth.Location = new System.Drawing.Point(260, 37);
            this.chkStealth.Name = "chkStealth";
            this.chkStealth.Size = new System.Drawing.Size(190, 20);
            this.chkStealth.TabIndex = 107;
            this.chkStealth.Text = "Count stealth steps";
            this.chkStealth.CheckedChanged += new System.EventHandler(this.chkStealth_CheckedChanged);
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(427, 193);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(10, 18);
            this.label18.TabIndex = 114;
            this.label18.Text = "x";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gameSize
            // 
            this.gameSize.Location = new System.Drawing.Point(260, 192);
            this.gameSize.Name = "gameSize";
            this.gameSize.Size = new System.Drawing.Size(118, 18);
            this.gameSize.TabIndex = 113;
            this.gameSize.Text = "Force Game Size:";
            this.gameSize.CheckedChanged += new System.EventHandler(this.gameSize_CheckedChanged);
            // 
            // setMinLightLevel
            // 
            this.setMinLightLevel.Location = new System.Drawing.Point(117, 247);
            this.setMinLightLevel.Name = "setMinLightLevel";
            this.setMinLightLevel.Size = new System.Drawing.Size(58, 25);
            this.setMinLightLevel.TabIndex = 105;
            this.setMinLightLevel.Text = "Set Min";
            this.setMinLightLevel.Click += new System.EventHandler(this.setMinLightLevel_Click);
            // 
            // setMaxLightLevel
            // 
            this.setMaxLightLevel.Location = new System.Drawing.Point(181, 247);
            this.setMaxLightLevel.Name = "setMaxLightLevel";
            this.setMaxLightLevel.Size = new System.Drawing.Size(58, 25);
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
            this.seasonList.Location = new System.Drawing.Point(59, 195);
            this.seasonList.Name = "seasonList";
            this.seasonList.Size = new System.Drawing.Size(111, 23);
            this.seasonList.TabIndex = 102;
            this.seasonList.SelectedIndexChanged += new System.EventHandler(this.seasonList_SelectedIndexChanged);
            // 
            // lblSeason
            // 
            this.lblSeason.AutoSize = true;
            this.lblSeason.Location = new System.Drawing.Point(6, 198);
            this.lblSeason.Name = "lblSeason";
            this.lblSeason.Size = new System.Drawing.Size(47, 15);
            this.lblSeason.TabIndex = 101;
            this.lblSeason.Text = "Season:";
            // 
            // lightLevel
            // 
            this.lightLevel.AutoSize = true;
            this.lightLevel.Location = new System.Drawing.Point(6, 223);
            this.lightLevel.Name = "lightLevel";
            this.lightLevel.Size = new System.Drawing.Size(67, 15);
            this.lightLevel.TabIndex = 100;
            this.lightLevel.Text = "Light Level:";
            // 
            // lightLevelBar
            // 
            this.lightLevelBar.AutoSize = false;
            this.lightLevelBar.Location = new System.Drawing.Point(79, 223);
            this.lightLevelBar.Maximum = 31;
            this.lightLevelBar.Name = "lightLevelBar";
            this.lightLevelBar.Size = new System.Drawing.Size(161, 21);
            this.lightLevelBar.TabIndex = 99;
            this.lightLevelBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.lightLevelBar.Value = 15;
            this.lightLevelBar.Scroll += new System.EventHandler(this.lightLevelBar_Scroll);
            // 
            // minMaxLightLevel
            // 
            this.minMaxLightLevel.Location = new System.Drawing.Point(9, 250);
            this.minMaxLightLevel.Name = "minMaxLightLevel";
            this.minMaxLightLevel.Size = new System.Drawing.Size(114, 20);
            this.minMaxLightLevel.TabIndex = 106;
            this.minMaxLightLevel.Text = "Enable Min/Max";
            this.minMaxLightLevel.CheckedChanged += new System.EventHandler(this.minMaxLightLevel_CheckedChanged);
            // 
            // blockPartyInvites
            // 
            this.blockPartyInvites.Location = new System.Drawing.Point(9, 169);
            this.blockPartyInvites.Name = "blockPartyInvites";
            this.blockPartyInvites.Size = new System.Drawing.Size(184, 20);
            this.blockPartyInvites.TabIndex = 98;
            this.blockPartyInvites.Text = "Block party invites";
            this.blockPartyInvites.CheckedChanged += new System.EventHandler(this.blockPartyInvites_CheckedChanged);
            // 
            // blockTradeRequests
            // 
            this.blockTradeRequests.Location = new System.Drawing.Point(9, 143);
            this.blockTradeRequests.Name = "blockTradeRequests";
            this.blockTradeRequests.Size = new System.Drawing.Size(184, 20);
            this.blockTradeRequests.TabIndex = 97;
            this.blockTradeRequests.Text = "Block trade requests";
            this.blockTradeRequests.CheckedChanged += new System.EventHandler(this.blockTradeRequests_CheckedChanged);
            // 
            // blockOpenCorpsesTwice
            // 
            this.blockOpenCorpsesTwice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blockOpenCorpsesTwice.Location = new System.Drawing.Point(9, 91);
            this.blockOpenCorpsesTwice.Name = "blockOpenCorpsesTwice";
            this.blockOpenCorpsesTwice.Size = new System.Drawing.Size(209, 20);
            this.blockOpenCorpsesTwice.TabIndex = 96;
            this.blockOpenCorpsesTwice.Text = "Block opening corpses twice";
            this.blockOpenCorpsesTwice.CheckedChanged += new System.EventHandler(this.blockOpenCorpsesTwice_CheckedChanged);
            // 
            // preAOSstatbar
            // 
            this.preAOSstatbar.Location = new System.Drawing.Point(9, 12);
            this.preAOSstatbar.Name = "preAOSstatbar";
            this.preAOSstatbar.Size = new System.Drawing.Size(190, 20);
            this.preAOSstatbar.TabIndex = 95;
            this.preAOSstatbar.Text = "Use Pre-AOS status window";
            this.preAOSstatbar.CheckedChanged += new System.EventHandler(this.preAOSstatbar_CheckedChanged);
            // 
            // corpseRange
            // 
            this.corpseRange.Location = new System.Drawing.Point(169, 63);
            this.corpseRange.Name = "corpseRange";
            this.corpseRange.Size = new System.Drawing.Size(24, 23);
            this.corpseRange.TabIndex = 91;
            this.corpseRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.corpseRange.TextChanged += new System.EventHandler(this.corpseRange_TextChanged);
            // 
            // autoStackRes
            // 
            this.autoStackRes.Location = new System.Drawing.Point(9, 37);
            this.autoStackRes.Name = "autoStackRes";
            this.autoStackRes.Size = new System.Drawing.Size(228, 20);
            this.autoStackRes.TabIndex = 93;
            this.autoStackRes.Text = "Auto-Stack Ore/Fish/Logs at Feet";
            this.autoStackRes.CheckedChanged += new System.EventHandler(this.autoStackRes_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(201, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 92;
            this.label4.Text = "tiles";
            // 
            // openCorpses
            // 
            this.openCorpses.Location = new System.Drawing.Point(9, 65);
            this.openCorpses.Name = "openCorpses";
            this.openCorpses.Size = new System.Drawing.Size(165, 20);
            this.openCorpses.TabIndex = 90;
            this.openCorpses.Text = "Open new corpses within";
            this.openCorpses.CheckedChanged += new System.EventHandler(this.openCorpses_CheckedChanged);
            // 
            // blockDis
            // 
            this.blockDis.Location = new System.Drawing.Point(9, 117);
            this.blockDis.Name = "blockDis";
            this.blockDis.Size = new System.Drawing.Size(184, 20);
            this.blockDis.TabIndex = 94;
            this.blockDis.Text = "Block dismount in war mode";
            this.blockDis.CheckedChanged += new System.EventHandler(this.blockDis_CheckedChanged);
            // 
            // displayTab
            // 
            this.displayTab.Controls.Add(this.displayCountersTabCtrl);
            this.displayTab.Location = new System.Drawing.Point(4, 44);
            this.displayTab.Name = "displayTab";
            this.displayTab.Size = new System.Drawing.Size(519, 322);
            this.displayTab.TabIndex = 1;
            this.displayTab.Text = "Display/Counters";
            // 
            // displayCountersTabCtrl
            // 
            this.displayCountersTabCtrl.Controls.Add(this.subDisplayTab);
            this.displayCountersTabCtrl.Controls.Add(this.subCountersTab);
            this.displayCountersTabCtrl.Controls.Add(this.subBandageTimerTab);
            this.displayCountersTabCtrl.Controls.Add(this.subOverheadTab);
            this.displayCountersTabCtrl.Controls.Add(this.subWaypoints);
            this.displayCountersTabCtrl.Location = new System.Drawing.Point(6, 3);
            this.displayCountersTabCtrl.Name = "displayCountersTabCtrl";
            this.displayCountersTabCtrl.SelectedIndex = 0;
            this.displayCountersTabCtrl.Size = new System.Drawing.Size(510, 314);
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
            this.subDisplayTab.Location = new System.Drawing.Point(4, 24);
            this.subDisplayTab.Name = "subDisplayTab";
            this.subDisplayTab.Padding = new System.Windows.Forms.Padding(3);
            this.subDisplayTab.Size = new System.Drawing.Size(502, 286);
            this.subDisplayTab.TabIndex = 0;
            this.subDisplayTab.Text = "Display";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.razorTitleBarKey);
            this.groupBox11.Controls.Add(this.showInRazorTitleBar);
            this.groupBox11.Controls.Add(this.razorTitleBar);
            this.groupBox11.Location = new System.Drawing.Point(6, 207);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(490, 71);
            this.groupBox11.TabIndex = 51;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Razor Title Bar";
            // 
            // razorTitleBarKey
            // 
            this.razorTitleBarKey.Location = new System.Drawing.Point(468, 17);
            this.razorTitleBarKey.Name = "razorTitleBarKey";
            this.razorTitleBarKey.Size = new System.Drawing.Size(16, 20);
            this.razorTitleBarKey.TabIndex = 2;
            this.razorTitleBarKey.Text = "?";
            this.razorTitleBarKey.UseVisualStyleBackColor = true;
            this.razorTitleBarKey.Click += new System.EventHandler(this.razorTitleBarKey_Click);
            // 
            // showInRazorTitleBar
            // 
            this.showInRazorTitleBar.AutoSize = true;
            this.showInRazorTitleBar.Location = new System.Drawing.Point(6, 17);
            this.showInRazorTitleBar.Name = "showInRazorTitleBar";
            this.showInRazorTitleBar.Size = new System.Drawing.Size(146, 19);
            this.showInRazorTitleBar.TabIndex = 1;
            this.showInRazorTitleBar.Text = "Show in Razor title bar:";
            this.showInRazorTitleBar.UseVisualStyleBackColor = true;
            this.showInRazorTitleBar.CheckedChanged += new System.EventHandler(this.showInRazorTitleBar_CheckedChanged);
            // 
            // razorTitleBar
            // 
            this.razorTitleBar.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.razorTitleBar.Location = new System.Drawing.Point(6, 42);
            this.razorTitleBar.Name = "razorTitleBar";
            this.razorTitleBar.Size = new System.Drawing.Size(478, 22);
            this.razorTitleBar.TabIndex = 0;
            this.razorTitleBar.Text = "{name} ({profile} - {shard}) - Razor v{version}";
            this.razorTitleBar.TextChanged += new System.EventHandler(this.razorTitleBar_TextChanged);
            // 
            // trackDps
            // 
            this.trackDps.AutoSize = true;
            this.trackDps.Location = new System.Drawing.Point(273, 178);
            this.trackDps.Name = "trackDps";
            this.trackDps.Size = new System.Drawing.Size(146, 19);
            this.trackDps.TabIndex = 53;
            this.trackDps.Text = "Enable damage tracker";
            this.trackDps.UseVisualStyleBackColor = true;
            this.trackDps.CheckedChanged += new System.EventHandler(this.trackDps_CheckedChanged);
            // 
            // trackIncomingGold
            // 
            this.trackIncomingGold.AutoSize = true;
            this.trackIncomingGold.Location = new System.Drawing.Point(273, 152);
            this.trackIncomingGold.Name = "trackIncomingGold";
            this.trackIncomingGold.Size = new System.Drawing.Size(223, 19);
            this.trackIncomingGold.TabIndex = 52;
            this.trackIncomingGold.Text = "Enable gold per sec/min/hour tracker";
            this.trackIncomingGold.UseVisualStyleBackColor = true;
            this.trackIncomingGold.CheckedChanged += new System.EventHandler(this.trackIncomingGold_CheckedChanged);
            // 
            // showNotoHue
            // 
            this.showNotoHue.Location = new System.Drawing.Point(6, 177);
            this.showNotoHue.Name = "showNotoHue";
            this.showNotoHue.Size = new System.Drawing.Size(221, 20);
            this.showNotoHue.TabIndex = 51;
            this.showNotoHue.Text = "Show noto hue on {char} in TitleBar";
            this.showNotoHue.CheckedChanged += new System.EventHandler(this.showNotoHue_CheckedChanged);
            // 
            // highlightSpellReags
            // 
            this.highlightSpellReags.Location = new System.Drawing.Point(6, 151);
            this.highlightSpellReags.Name = "highlightSpellReags";
            this.highlightSpellReags.Size = new System.Drawing.Size(205, 20);
            this.highlightSpellReags.TabIndex = 50;
            this.highlightSpellReags.Text = "Highlight Spell Reagents on Cast";
            this.highlightSpellReags.CheckedChanged += new System.EventHandler(this.highlightSpellReags_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.titleBarParams);
            this.groupBox3.Controls.Add(this.titleStr);
            this.groupBox3.Controls.Add(this.showInBar);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(490, 139);
            this.groupBox3.TabIndex = 4;
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
            this.titleBarParams.Location = new System.Drawing.Point(380, 19);
            this.titleBarParams.Name = "titleBarParams";
            this.titleBarParams.Size = new System.Drawing.Size(104, 23);
            this.titleBarParams.TabIndex = 5;
            this.titleBarParams.SelectedIndexChanged += new System.EventHandler(this.titleBarParams_SelectedIndexChanged);
            // 
            // titleStr
            // 
            this.titleStr.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleStr.Location = new System.Drawing.Point(6, 48);
            this.titleStr.Multiline = true;
            this.titleStr.Name = "titleStr";
            this.titleStr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.titleStr.Size = new System.Drawing.Size(478, 85);
            this.titleStr.TabIndex = 4;
            this.titleStr.TextChanged += new System.EventHandler(this.titleStr_TextChanged);
            // 
            // showInBar
            // 
            this.showInBar.Location = new System.Drawing.Point(11, 22);
            this.showInBar.Name = "showInBar";
            this.showInBar.Size = new System.Drawing.Size(141, 20);
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
            this.subCountersTab.Location = new System.Drawing.Point(4, 22);
            this.subCountersTab.Name = "subCountersTab";
            this.subCountersTab.Padding = new System.Windows.Forms.Padding(3);
            this.subCountersTab.Size = new System.Drawing.Size(502, 288);
            this.subCountersTab.TabIndex = 1;
            this.subCountersTab.Text = "Counters";
            // 
            // warnNum
            // 
            this.warnNum.Location = new System.Drawing.Point(430, 99);
            this.warnNum.Name = "warnNum";
            this.warnNum.Size = new System.Drawing.Size(30, 23);
            this.warnNum.TabIndex = 51;
            this.warnNum.Text = "3";
            this.warnNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.warnNum.TextChanged += new System.EventHandler(this.warnNum_TextChanged);
            // 
            // warnCount
            // 
            this.warnCount.Location = new System.Drawing.Point(299, 101);
            this.warnCount.Name = "warnCount";
            this.warnCount.Size = new System.Drawing.Size(192, 19);
            this.warnCount.TabIndex = 50;
            this.warnCount.Text = "Warn when below:";
            this.warnCount.CheckedChanged += new System.EventHandler(this.warnCount_CheckedChanged);
            // 
            // excludePouches
            // 
            this.excludePouches.Location = new System.Drawing.Point(299, 48);
            this.excludePouches.Name = "excludePouches";
            this.excludePouches.Size = new System.Drawing.Size(192, 21);
            this.excludePouches.TabIndex = 49;
            this.excludePouches.Text = "Never auto-search pouches";
            this.excludePouches.CheckedChanged += new System.EventHandler(this.excludePouches_CheckedChanged);
            // 
            // titlebarImages
            // 
            this.titlebarImages.Location = new System.Drawing.Point(299, 75);
            this.titlebarImages.Name = "titlebarImages";
            this.titlebarImages.Size = new System.Drawing.Size(180, 20);
            this.titlebarImages.TabIndex = 48;
            this.titlebarImages.Text = "Show Images with Counters";
            this.titlebarImages.CheckedChanged += new System.EventHandler(this.titlebarImages_CheckedChanged);
            // 
            // checkNewConts
            // 
            this.checkNewConts.Location = new System.Drawing.Point(299, 24);
            this.checkNewConts.Name = "checkNewConts";
            this.checkNewConts.Size = new System.Drawing.Size(200, 20);
            this.checkNewConts.TabIndex = 47;
            this.checkNewConts.Text = "Auto search new containers";
            this.checkNewConts.CheckedChanged += new System.EventHandler(this.checkNewConts_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.counters);
            this.groupBox2.Controls.Add(this.delCounter);
            this.groupBox2.Controls.Add(this.addCounter);
            this.groupBox2.Controls.Add(this.recount);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(284, 272);
            this.groupBox2.TabIndex = 2;
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
            this.counters.HideSelection = false;
            this.counters.LabelWrap = false;
            this.counters.Location = new System.Drawing.Point(6, 18);
            this.counters.MultiSelect = false;
            this.counters.Name = "counters";
            this.counters.Size = new System.Drawing.Size(272, 205);
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
            this.delCounter.Location = new System.Drawing.Point(108, 229);
            this.delCounter.Name = "delCounter";
            this.delCounter.Size = new System.Drawing.Size(71, 37);
            this.delCounter.TabIndex = 4;
            this.delCounter.Text = "Del/Edit";
            this.delCounter.Click += new System.EventHandler(this.delCounter_Click);
            // 
            // addCounter
            // 
            this.addCounter.Location = new System.Drawing.Point(6, 229);
            this.addCounter.Name = "addCounter";
            this.addCounter.Size = new System.Drawing.Size(70, 37);
            this.addCounter.TabIndex = 3;
            this.addCounter.Text = "Add...";
            this.addCounter.Click += new System.EventHandler(this.addCounter_Click);
            // 
            // recount
            // 
            this.recount.Location = new System.Drawing.Point(208, 229);
            this.recount.Name = "recount";
            this.recount.Size = new System.Drawing.Size(70, 37);
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
            this.subBandageTimerTab.Location = new System.Drawing.Point(4, 22);
            this.subBandageTimerTab.Name = "subBandageTimerTab";
            this.subBandageTimerTab.Size = new System.Drawing.Size(502, 288);
            this.subBandageTimerTab.TabIndex = 2;
            this.subBandageTimerTab.Text = "Bandage Timer";
            // 
            // bandageEndMessage
            // 
            this.bandageEndMessage.Location = new System.Drawing.Point(205, 123);
            this.bandageEndMessage.Name = "bandageEndMessage";
            this.bandageEndMessage.Size = new System.Drawing.Size(146, 23);
            this.bandageEndMessage.TabIndex = 62;
            this.bandageEndMessage.Text = "Bandage: Ending";
            this.bandageEndMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bandageEndMessage.TextChanged += new System.EventHandler(this.BandageEndMessage_TextChanged);
            // 
            // bandageStartMessage
            // 
            this.bandageStartMessage.Location = new System.Drawing.Point(205, 98);
            this.bandageStartMessage.Name = "bandageStartMessage";
            this.bandageStartMessage.Size = new System.Drawing.Size(146, 23);
            this.bandageStartMessage.TabIndex = 61;
            this.bandageStartMessage.Text = "Bandage: Starting";
            this.bandageStartMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bandageStartMessage.TextChanged += new System.EventHandler(this.BandageStartMessage_TextChanged);
            // 
            // showBandageEnd
            // 
            this.showBandageEnd.AutoSize = true;
            this.showBandageEnd.Location = new System.Drawing.Point(9, 125);
            this.showBandageEnd.Name = "showBandageEnd";
            this.showBandageEnd.Size = new System.Drawing.Size(187, 19);
            this.showBandageEnd.TabIndex = 60;
            this.showBandageEnd.Text = "Show bandaging end message";
            this.showBandageEnd.UseVisualStyleBackColor = true;
            this.showBandageEnd.CheckedChanged += new System.EventHandler(this.ShowBandageEnd_CheckedChanged);
            // 
            // showBandageStart
            // 
            this.showBandageStart.AutoSize = true;
            this.showBandageStart.Location = new System.Drawing.Point(9, 100);
            this.showBandageStart.Name = "showBandageStart";
            this.showBandageStart.Size = new System.Drawing.Size(190, 19);
            this.showBandageStart.TabIndex = 58;
            this.showBandageStart.Text = "Show bandaging start message";
            this.showBandageStart.UseVisualStyleBackColor = true;
            this.showBandageStart.CheckedChanged += new System.EventHandler(this.ShowBandageStart_CheckedChanged);
            // 
            // setBandageHue
            // 
            this.setBandageHue.Location = new System.Drawing.Point(269, 41);
            this.setBandageHue.Name = "setBandageHue";
            this.setBandageHue.Size = new System.Drawing.Size(67, 23);
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
            this.bandageTimerLocation.Location = new System.Drawing.Point(145, 10);
            this.bandageTimerLocation.Name = "bandageTimerLocation";
            this.bandageTimerLocation.Size = new System.Drawing.Size(118, 23);
            this.bandageTimerLocation.TabIndex = 56;
            this.bandageTimerLocation.SelectedIndexChanged += new System.EventHandler(this.bandageTimerLocation_SelectedIndexChanged);
            // 
            // bandageTimerSeconds
            // 
            this.bandageTimerSeconds.Location = new System.Drawing.Point(180, 71);
            this.bandageTimerSeconds.Name = "bandageTimerSeconds";
            this.bandageTimerSeconds.Size = new System.Drawing.Size(48, 23);
            this.bandageTimerSeconds.TabIndex = 55;
            this.bandageTimerSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bandageTimerSeconds.TextChanged += new System.EventHandler(this.bandageTimerSeconds_TextChanged);
            // 
            // onlyShowBandageTimerSeconds
            // 
            this.onlyShowBandageTimerSeconds.Location = new System.Drawing.Point(9, 72);
            this.onlyShowBandageTimerSeconds.Name = "onlyShowBandageTimerSeconds";
            this.onlyShowBandageTimerSeconds.Size = new System.Drawing.Size(205, 20);
            this.onlyShowBandageTimerSeconds.TabIndex = 53;
            this.onlyShowBandageTimerSeconds.Text = "Only show every X seconds:";
            this.onlyShowBandageTimerSeconds.CheckedChanged += new System.EventHandler(this.onlyShowBandageTimerSeconds_CheckedChanged);
            // 
            // bandageTimerFormat
            // 
            this.bandageTimerFormat.Location = new System.Drawing.Point(113, 41);
            this.bandageTimerFormat.Name = "bandageTimerFormat";
            this.bandageTimerFormat.Size = new System.Drawing.Size(150, 23);
            this.bandageTimerFormat.TabIndex = 52;
            this.bandageTimerFormat.Text = "Bandage: {count}s";
            this.bandageTimerFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bandageTimerFormat.TextChanged += new System.EventHandler(this.bandageTimerFormat_TextChanged);
            // 
            // showBandageTimer
            // 
            this.showBandageTimer.Location = new System.Drawing.Point(9, 11);
            this.showBandageTimer.Name = "showBandageTimer";
            this.showBandageTimer.Size = new System.Drawing.Size(205, 20);
            this.showBandageTimer.TabIndex = 51;
            this.showBandageTimer.Text = "Show bandage timer";
            this.showBandageTimer.CheckedChanged += new System.EventHandler(this.showBandageTimer_CheckedChanged);
            // 
            // lblBandageCountFormat
            // 
            this.lblBandageCountFormat.Location = new System.Drawing.Point(21, 41);
            this.lblBandageCountFormat.Name = "lblBandageCountFormat";
            this.lblBandageCountFormat.Size = new System.Drawing.Size(159, 23);
            this.lblBandageCountFormat.TabIndex = 54;
            this.lblBandageCountFormat.Text = "Format && Hue:";
            this.lblBandageCountFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // subOverheadTab
            // 
            this.subOverheadTab.BackColor = System.Drawing.SystemColors.Control;
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
            this.subOverheadTab.Location = new System.Drawing.Point(4, 22);
            this.subOverheadTab.Name = "subOverheadTab";
            this.subOverheadTab.Size = new System.Drawing.Size(502, 288);
            this.subOverheadTab.TabIndex = 3;
            this.subOverheadTab.Text = "Overhead Messages";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(5, 101);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(84, 15);
            this.label14.TabIndex = 102;
            this.label14.Text = "Message Style:";
            // 
            // unicodeStyle
            // 
            this.unicodeStyle.AutoSize = true;
            this.unicodeStyle.Location = new System.Drawing.Point(8, 119);
            this.unicodeStyle.Name = "unicodeStyle";
            this.unicodeStyle.Size = new System.Drawing.Size(69, 19);
            this.unicodeStyle.TabIndex = 101;
            this.unicodeStyle.TabStop = true;
            this.unicodeStyle.Text = "Unicode";
            this.unicodeStyle.UseVisualStyleBackColor = true;
            this.unicodeStyle.CheckedChanged += new System.EventHandler(this.unicodeStyle_CheckedChanged);
            // 
            // asciiStyle
            // 
            this.asciiStyle.AutoSize = true;
            this.asciiStyle.Location = new System.Drawing.Point(8, 144);
            this.asciiStyle.Name = "asciiStyle";
            this.asciiStyle.Size = new System.Drawing.Size(53, 19);
            this.asciiStyle.TabIndex = 100;
            this.asciiStyle.TabStop = true;
            this.asciiStyle.Text = "ASCII";
            this.asciiStyle.UseVisualStyleBackColor = true;
            this.asciiStyle.CheckedChanged += new System.EventHandler(this.asciiStyle_CheckedChanged);
            // 
            // editOverheadMessage
            // 
            this.editOverheadMessage.Location = new System.Drawing.Point(8, 179);
            this.editOverheadMessage.Name = "editOverheadMessage";
            this.editOverheadMessage.Size = new System.Drawing.Size(84, 28);
            this.editOverheadMessage.TabIndex = 97;
            this.editOverheadMessage.Text = "Edit";
            this.editOverheadMessage.UseVisualStyleBackColor = true;
            this.editOverheadMessage.Click += new System.EventHandler(this.editOverheadMessage_Click);
            // 
            // setColorHue
            // 
            this.setColorHue.Location = new System.Drawing.Point(8, 247);
            this.setColorHue.Name = "setColorHue";
            this.setColorHue.Size = new System.Drawing.Size(84, 28);
            this.setColorHue.TabIndex = 96;
            this.setColorHue.Text = "Set Hue";
            this.setColorHue.UseVisualStyleBackColor = true;
            this.setColorHue.Click += new System.EventHandler(this.setColorHue_Click);
            // 
            // removeOverheadMessage
            // 
            this.removeOverheadMessage.Location = new System.Drawing.Point(8, 213);
            this.removeOverheadMessage.Name = "removeOverheadMessage";
            this.removeOverheadMessage.Size = new System.Drawing.Size(84, 28);
            this.removeOverheadMessage.TabIndex = 95;
            this.removeOverheadMessage.Text = "Remove";
            this.removeOverheadMessage.UseVisualStyleBackColor = true;
            this.removeOverheadMessage.Click += new System.EventHandler(this.removeOverheadMessage_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 39);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 15);
            this.label13.TabIndex = 94;
            this.label13.Text = "Format:";
            // 
            // overheadFormat
            // 
            this.overheadFormat.Location = new System.Drawing.Point(8, 57);
            this.overheadFormat.Name = "overheadFormat";
            this.overheadFormat.Size = new System.Drawing.Size(84, 23);
            this.overheadFormat.TabIndex = 93;
            this.overheadFormat.Text = "[{msg}]";
            this.overheadFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.overheadFormat.TextChanged += new System.EventHandler(this.overheadFormat_TextChanged);
            // 
            // setOverheadMessage
            // 
            this.setOverheadMessage.Location = new System.Drawing.Point(443, 9);
            this.setOverheadMessage.Name = "setOverheadMessage";
            this.setOverheadMessage.Size = new System.Drawing.Size(56, 24);
            this.setOverheadMessage.TabIndex = 92;
            this.setOverheadMessage.Text = "Add";
            this.setOverheadMessage.UseVisualStyleBackColor = true;
            this.setOverheadMessage.Click += new System.EventHandler(this.setOverheadMessage_Click);
            // 
            // cliLocOverheadView
            // 
            this.cliLocOverheadView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cliLocOriginal,
            this.cliLocOverheadMessage});
            this.cliLocOverheadView.HideSelection = false;
            this.cliLocOverheadView.Location = new System.Drawing.Point(98, 156);
            this.cliLocOverheadView.Name = "cliLocOverheadView";
            this.cliLocOverheadView.Size = new System.Drawing.Size(401, 127);
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
            this.cliLocOverheadMessage.Width = 229;
            // 
            // cliLocSearch
            // 
            this.cliLocSearch.Location = new System.Drawing.Point(377, 9);
            this.cliLocSearch.Name = "cliLocSearch";
            this.cliLocSearch.Size = new System.Drawing.Size(60, 24);
            this.cliLocSearch.TabIndex = 90;
            this.cliLocSearch.Text = "Search";
            this.cliLocSearch.UseVisualStyleBackColor = true;
            this.cliLocSearch.Click += new System.EventHandler(this.cliLocSearch_Click);
            // 
            // cliLocTextSearch
            // 
            this.cliLocTextSearch.Location = new System.Drawing.Point(146, 9);
            this.cliLocTextSearch.Name = "cliLocTextSearch";
            this.cliLocTextSearch.Size = new System.Drawing.Size(225, 23);
            this.cliLocTextSearch.TabIndex = 89;
            // 
            // lblOhSearch
            // 
            this.lblOhSearch.AutoSize = true;
            this.lblOhSearch.Location = new System.Drawing.Point(95, 12);
            this.lblOhSearch.Name = "lblOhSearch";
            this.lblOhSearch.Size = new System.Drawing.Size(45, 15);
            this.lblOhSearch.TabIndex = 88;
            this.lblOhSearch.Text = "Search:";
            // 
            // cliLocSearchView
            // 
            this.cliLocSearchView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cliLocSearchNumber,
            this.cliLocSearchText});
            this.cliLocSearchView.HideSelection = false;
            this.cliLocSearchView.Location = new System.Drawing.Point(98, 39);
            this.cliLocSearchView.Name = "cliLocSearchView";
            this.cliLocSearchView.Size = new System.Drawing.Size(401, 111);
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
            this.showOverheadMessages.Location = new System.Drawing.Point(8, 9);
            this.showOverheadMessages.Name = "showOverheadMessages";
            this.showOverheadMessages.Size = new System.Drawing.Size(71, 24);
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
            this.subWaypoints.Location = new System.Drawing.Point(4, 22);
            this.subWaypoints.Name = "subWaypoints";
            this.subWaypoints.Size = new System.Drawing.Size(502, 288);
            this.subWaypoints.TabIndex = 4;
            this.subWaypoints.Text = "Waypoints";
            // 
            // waypointOnDeath
            // 
            this.waypointOnDeath.AutoSize = true;
            this.waypointOnDeath.Location = new System.Drawing.Point(184, 87);
            this.waypointOnDeath.Name = "waypointOnDeath";
            this.waypointOnDeath.Size = new System.Drawing.Size(162, 19);
            this.waypointOnDeath.TabIndex = 66;
            this.waypointOnDeath.Text = "Create waypoint on death";
            this.waypointOnDeath.UseVisualStyleBackColor = true;
            this.waypointOnDeath.CheckedChanged += new System.EventHandler(this.waypointOnDeath_CheckedChanged);
            // 
            // lblWaypointTiles
            // 
            this.lblWaypointTiles.Location = new System.Drawing.Point(374, 63);
            this.lblWaypointTiles.Name = "lblWaypointTiles";
            this.lblWaypointTiles.Size = new System.Drawing.Size(58, 18);
            this.lblWaypointTiles.TabIndex = 65;
            this.lblWaypointTiles.Text = "tiles";
            // 
            // hideWaypointDist
            // 
            this.hideWaypointDist.Location = new System.Drawing.Point(336, 60);
            this.hideWaypointDist.Name = "hideWaypointDist";
            this.hideWaypointDist.Size = new System.Drawing.Size(32, 23);
            this.hideWaypointDist.TabIndex = 64;
            this.hideWaypointDist.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hideWaypointDist.TextChanged += new System.EventHandler(this.hideWaypointDist_TextChanged);
            // 
            // hideWaypointWithin
            // 
            this.hideWaypointWithin.AutoSize = true;
            this.hideWaypointWithin.Location = new System.Drawing.Point(184, 62);
            this.hideWaypointWithin.Name = "hideWaypointWithin";
            this.hideWaypointWithin.Size = new System.Drawing.Size(139, 19);
            this.hideWaypointWithin.TabIndex = 63;
            this.hideWaypointWithin.Text = "Hide waypoint within";
            this.hideWaypointWithin.UseVisualStyleBackColor = true;
            this.hideWaypointWithin.CheckedChanged += new System.EventHandler(this.hideWaypointWithin_CheckedChanged);
            // 
            // txtWaypointDistanceSec
            // 
            this.txtWaypointDistanceSec.Location = new System.Drawing.Point(336, 35);
            this.txtWaypointDistanceSec.Name = "txtWaypointDistanceSec";
            this.txtWaypointDistanceSec.Size = new System.Drawing.Size(32, 23);
            this.txtWaypointDistanceSec.TabIndex = 62;
            this.txtWaypointDistanceSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWaypointDistanceSec.TextChanged += new System.EventHandler(this.txtWaypointDistanceSec_TextChanged);
            // 
            // lblWaypointSeconds
            // 
            this.lblWaypointSeconds.Location = new System.Drawing.Point(374, 37);
            this.lblWaypointSeconds.Name = "lblWaypointSeconds";
            this.lblWaypointSeconds.Size = new System.Drawing.Size(58, 18);
            this.lblWaypointSeconds.TabIndex = 61;
            this.lblWaypointSeconds.Text = "secs";
            // 
            // showWaypointDistance
            // 
            this.showWaypointDistance.AutoSize = true;
            this.showWaypointDistance.Location = new System.Drawing.Point(184, 37);
            this.showWaypointDistance.Name = "showWaypointDistance";
            this.showWaypointDistance.Size = new System.Drawing.Size(152, 19);
            this.showWaypointDistance.TabIndex = 60;
            this.showWaypointDistance.Text = "Show tile distance every";
            this.showWaypointDistance.UseVisualStyleBackColor = true;
            this.showWaypointDistance.CheckedChanged += new System.EventHandler(this.showWaypointDistance_CheckedChanged);
            // 
            // showWaypointOverhead
            // 
            this.showWaypointOverhead.AutoSize = true;
            this.showWaypointOverhead.Location = new System.Drawing.Point(184, 12);
            this.showWaypointOverhead.Name = "showWaypointOverhead";
            this.showWaypointOverhead.Size = new System.Drawing.Size(213, 19);
            this.showWaypointOverhead.TabIndex = 59;
            this.showWaypointOverhead.Text = "Show waypoint messages overhead";
            this.showWaypointOverhead.UseVisualStyleBackColor = true;
            this.showWaypointOverhead.CheckedChanged += new System.EventHandler(this.showWaypointOverhead_CheckedChanged);
            // 
            // btnRemoveSelectedWaypoint
            // 
            this.btnRemoveSelectedWaypoint.Location = new System.Drawing.Point(184, 242);
            this.btnRemoveSelectedWaypoint.Name = "btnRemoveSelectedWaypoint";
            this.btnRemoveSelectedWaypoint.Size = new System.Drawing.Size(135, 29);
            this.btnRemoveSelectedWaypoint.TabIndex = 10;
            this.btnRemoveSelectedWaypoint.Text = "Remove Selected";
            this.btnRemoveSelectedWaypoint.UseVisualStyleBackColor = true;
            this.btnRemoveSelectedWaypoint.Click += new System.EventHandler(this.btnRemoveSelectedWaypoint_Click);
            // 
            // btnHideWaypoint
            // 
            this.btnHideWaypoint.Location = new System.Drawing.Point(364, 242);
            this.btnHideWaypoint.Name = "btnHideWaypoint";
            this.btnHideWaypoint.Size = new System.Drawing.Size(129, 29);
            this.btnHideWaypoint.TabIndex = 9;
            this.btnHideWaypoint.Text = "Clear Waypoint";
            this.btnHideWaypoint.UseVisualStyleBackColor = true;
            this.btnHideWaypoint.Click += new System.EventHandler(this.btnHideWaypoint_Click);
            // 
            // btnUseCurrentLoc
            // 
            this.btnUseCurrentLoc.Location = new System.Drawing.Point(364, 151);
            this.btnUseCurrentLoc.Name = "btnUseCurrentLoc";
            this.btnUseCurrentLoc.Size = new System.Drawing.Size(129, 23);
            this.btnUseCurrentLoc.TabIndex = 8;
            this.btnUseCurrentLoc.Text = "Use Current Position";
            this.btnUseCurrentLoc.UseVisualStyleBackColor = true;
            this.btnUseCurrentLoc.Click += new System.EventHandler(this.btnUseCurrentLoc_Click);
            // 
            // txtWaypointName
            // 
            this.txtWaypointName.Location = new System.Drawing.Point(229, 121);
            this.txtWaypointName.Name = "txtWaypointName";
            this.txtWaypointName.Size = new System.Drawing.Size(129, 23);
            this.txtWaypointName.TabIndex = 7;
            // 
            // lblWaypointY
            // 
            this.lblWaypointY.AutoSize = true;
            this.lblWaypointY.Location = new System.Drawing.Point(285, 154);
            this.lblWaypointY.Name = "lblWaypointY";
            this.lblWaypointY.Size = new System.Drawing.Size(17, 15);
            this.lblWaypointY.TabIndex = 6;
            this.lblWaypointY.Text = "Y:";
            // 
            // lblWaypointX
            // 
            this.lblWaypointX.AutoSize = true;
            this.lblWaypointX.Location = new System.Drawing.Point(206, 154);
            this.lblWaypointX.Name = "lblWaypointX";
            this.lblWaypointX.Size = new System.Drawing.Size(17, 15);
            this.lblWaypointX.TabIndex = 5;
            this.lblWaypointX.Text = "X:";
            // 
            // txtWaypointX
            // 
            this.txtWaypointX.Location = new System.Drawing.Point(229, 151);
            this.txtWaypointX.Name = "txtWaypointX";
            this.txtWaypointX.Size = new System.Drawing.Size(50, 23);
            this.txtWaypointX.TabIndex = 4;
            // 
            // txtWaypointY
            // 
            this.txtWaypointY.Location = new System.Drawing.Point(308, 151);
            this.txtWaypointY.Name = "txtWaypointY";
            this.txtWaypointY.Size = new System.Drawing.Size(50, 23);
            this.txtWaypointY.TabIndex = 3;
            // 
            // lblWaypoint
            // 
            this.lblWaypoint.AutoSize = true;
            this.lblWaypoint.Location = new System.Drawing.Point(181, 124);
            this.lblWaypoint.Name = "lblWaypoint";
            this.lblWaypoint.Size = new System.Drawing.Size(42, 15);
            this.lblWaypoint.TabIndex = 2;
            this.lblWaypoint.Text = "Name:";
            // 
            // btnAddWaypoint
            // 
            this.btnAddWaypoint.Location = new System.Drawing.Point(364, 121);
            this.btnAddWaypoint.Name = "btnAddWaypoint";
            this.btnAddWaypoint.Size = new System.Drawing.Size(129, 24);
            this.btnAddWaypoint.TabIndex = 1;
            this.btnAddWaypoint.Text = "Add Waypoint";
            this.btnAddWaypoint.UseVisualStyleBackColor = true;
            this.btnAddWaypoint.Click += new System.EventHandler(this.btnAddWaypoint_Click);
            // 
            // waypointList
            // 
            this.waypointList.FormattingEnabled = true;
            this.waypointList.ItemHeight = 15;
            this.waypointList.Location = new System.Drawing.Point(11, 12);
            this.waypointList.Name = "waypointList";
            this.waypointList.Size = new System.Drawing.Size(164, 259);
            this.waypointList.TabIndex = 0;
            this.waypointList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listWaypoints_MouseDoubleClick);
            this.waypointList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listWaypoints_MouseDown);
            // 
            // dressTab
            // 
            this.dressTab.Controls.Add(this.groupBox6);
            this.dressTab.Controls.Add(this.groupBox5);
            this.dressTab.Location = new System.Drawing.Point(4, 44);
            this.dressTab.Name = "dressTab";
            this.dressTab.Size = new System.Drawing.Size(519, 322);
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
            this.groupBox6.Location = new System.Drawing.Point(201, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(311, 309);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Arm/Dress Items";
            // 
            // clearDress
            // 
            this.clearDress.Location = new System.Drawing.Point(209, 257);
            this.clearDress.Name = "clearDress";
            this.clearDress.Size = new System.Drawing.Size(96, 32);
            this.clearDress.TabIndex = 13;
            this.clearDress.Text = "Clear List";
            this.clearDress.Click += new System.EventHandler(this.clearDress_Click);
            // 
            // dressDelSel
            // 
            this.dressDelSel.Location = new System.Drawing.Point(209, 170);
            this.dressDelSel.Name = "dressDelSel";
            this.dressDelSel.Size = new System.Drawing.Size(96, 32);
            this.dressDelSel.TabIndex = 12;
            this.dressDelSel.Text = "Remove";
            this.dressDelSel.Click += new System.EventHandler(this.dressDelSel_Click);
            // 
            // undressBag
            // 
            this.undressBag.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.undressBag.Location = new System.Drawing.Point(209, 208);
            this.undressBag.Name = "undressBag";
            this.undressBag.Size = new System.Drawing.Size(96, 40);
            this.undressBag.TabIndex = 11;
            this.undressBag.Text = "Change Undress Bag";
            this.undressBag.Click += new System.EventHandler(this.undressBag_Click);
            // 
            // undressList
            // 
            this.undressList.Location = new System.Drawing.Point(209, 56);
            this.undressList.Name = "undressList";
            this.undressList.Size = new System.Drawing.Size(96, 32);
            this.undressList.TabIndex = 10;
            this.undressList.Text = "Undress";
            this.undressList.Click += new System.EventHandler(this.undressList_Click);
            // 
            // dressUseCur
            // 
            this.dressUseCur.Location = new System.Drawing.Point(209, 132);
            this.dressUseCur.Name = "dressUseCur";
            this.dressUseCur.Size = new System.Drawing.Size(96, 32);
            this.dressUseCur.TabIndex = 9;
            this.dressUseCur.Text = "Add Current";
            this.dressUseCur.Click += new System.EventHandler(this.dressUseCur_Click);
            // 
            // targItem
            // 
            this.targItem.Location = new System.Drawing.Point(209, 94);
            this.targItem.Name = "targItem";
            this.targItem.Size = new System.Drawing.Size(96, 32);
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
            this.dressItems.Size = new System.Drawing.Size(197, 285);
            this.dressItems.TabIndex = 6;
            this.dressItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dressItems_KeyDown);
            this.dressItems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dressItems_MouseDown);
            // 
            // dressNow
            // 
            this.dressNow.Location = new System.Drawing.Point(209, 18);
            this.dressNow.Name = "dressNow";
            this.dressNow.Size = new System.Drawing.Size(96, 32);
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
            this.groupBox5.Size = new System.Drawing.Size(187, 309);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Arm/Dress Selection";
            // 
            // removeDress
            // 
            this.removeDress.Location = new System.Drawing.Point(121, 254);
            this.removeDress.Name = "removeDress";
            this.removeDress.Size = new System.Drawing.Size(60, 25);
            this.removeDress.TabIndex = 5;
            this.removeDress.Text = "Remove";
            this.removeDress.Click += new System.EventHandler(this.removeDress_Click);
            // 
            // addDress
            // 
            this.addDress.Location = new System.Drawing.Point(6, 254);
            this.addDress.Name = "addDress";
            this.addDress.Size = new System.Drawing.Size(60, 25);
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
            this.dressList.Size = new System.Drawing.Size(175, 230);
            this.dressList.TabIndex = 3;
            this.dressList.SelectedIndexChanged += new System.EventHandler(this.dressList_SelectedIndexChanged);
            // 
            // undressConflicts
            // 
            this.undressConflicts.Location = new System.Drawing.Point(6, 285);
            this.undressConflicts.Name = "undressConflicts";
            this.undressConflicts.Size = new System.Drawing.Size(137, 18);
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
            this.skillsTab.Location = new System.Drawing.Point(4, 44);
            this.skillsTab.Name = "skillsTab";
            this.skillsTab.Size = new System.Drawing.Size(519, 322);
            this.skillsTab.TabIndex = 2;
            this.skillsTab.Text = "Skills";
            // 
            // captureMibs
            // 
            this.captureMibs.Location = new System.Drawing.Point(356, 230);
            this.captureMibs.Name = "captureMibs";
            this.captureMibs.Size = new System.Drawing.Size(153, 19);
            this.captureMibs.TabIndex = 70;
            this.captureMibs.Text = "Capture MIBs to file";
            this.captureMibs.UseVisualStyleBackColor = true;
            this.captureMibs.CheckedChanged += new System.EventHandler(this.captureMibs_CheckedChanged);
            // 
            // dispDeltaOverhead
            // 
            this.dispDeltaOverhead.AutoSize = true;
            this.dispDeltaOverhead.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dispDeltaOverhead.Location = new System.Drawing.Point(441, 207);
            this.dispDeltaOverhead.Name = "dispDeltaOverhead";
            this.dispDeltaOverhead.Size = new System.Drawing.Size(71, 17);
            this.dispDeltaOverhead.TabIndex = 14;
            this.dispDeltaOverhead.Text = "Overhead";
            this.dispDeltaOverhead.UseVisualStyleBackColor = true;
            this.dispDeltaOverhead.CheckedChanged += new System.EventHandler(this.dispDeltaOverhead_CheckedChanged);
            // 
            // logSkillChanges
            // 
            this.logSkillChanges.Location = new System.Drawing.Point(356, 157);
            this.logSkillChanges.Name = "logSkillChanges";
            this.logSkillChanges.Size = new System.Drawing.Size(156, 19);
            this.logSkillChanges.TabIndex = 13;
            this.logSkillChanges.Text = "Log skill changes";
            this.logSkillChanges.UseVisualStyleBackColor = true;
            this.logSkillChanges.CheckedChanged += new System.EventHandler(this.logSkillChanges_CheckedChanged);
            // 
            // dispDelta
            // 
            this.dispDelta.Location = new System.Drawing.Point(356, 182);
            this.dispDelta.Name = "dispDelta";
            this.dispDelta.Size = new System.Drawing.Size(156, 19);
            this.dispDelta.TabIndex = 11;
            this.dispDelta.Text = "Show skill/stat changes";
            this.dispDelta.CheckedChanged += new System.EventHandler(this.dispDelta_CheckedChanged);
            // 
            // skillCopyAll
            // 
            this.skillCopyAll.Location = new System.Drawing.Point(356, 119);
            this.skillCopyAll.Name = "skillCopyAll";
            this.skillCopyAll.Size = new System.Drawing.Size(156, 32);
            this.skillCopyAll.TabIndex = 9;
            this.skillCopyAll.Text = "Copy All";
            this.skillCopyAll.Click += new System.EventHandler(this.skillCopyAll_Click);
            // 
            // skillCopySel
            // 
            this.skillCopySel.Location = new System.Drawing.Point(356, 81);
            this.skillCopySel.Name = "skillCopySel";
            this.skillCopySel.Size = new System.Drawing.Size(156, 32);
            this.skillCopySel.TabIndex = 8;
            this.skillCopySel.Text = "Copy Selected";
            this.skillCopySel.Click += new System.EventHandler(this.skillCopySel_Click);
            // 
            // baseTotal
            // 
            this.baseTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTotal.Location = new System.Drawing.Point(428, 290);
            this.baseTotal.Name = "baseTotal";
            this.baseTotal.ReadOnly = true;
            this.baseTotal.Size = new System.Drawing.Size(84, 23);
            this.baseTotal.TabIndex = 7;
            this.baseTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(356, 289);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Base Total:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // locks
            // 
            this.locks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.locks.Items.AddRange(new object[] {
            "Up",
            "Down",
            "Locked"});
            this.locks.Location = new System.Drawing.Point(468, 49);
            this.locks.Name = "locks";
            this.locks.Size = new System.Drawing.Size(44, 23);
            this.locks.TabIndex = 5;
            // 
            // setlocks
            // 
            this.setlocks.Location = new System.Drawing.Point(356, 43);
            this.setlocks.Name = "setlocks";
            this.setlocks.Size = new System.Drawing.Size(106, 32);
            this.setlocks.TabIndex = 4;
            this.setlocks.Text = "Set all locks:";
            this.setlocks.Click += new System.EventHandler(this.OnSetSkillLocks);
            // 
            // resetDelta
            // 
            this.resetDelta.Location = new System.Drawing.Point(356, 5);
            this.resetDelta.Name = "resetDelta";
            this.resetDelta.Size = new System.Drawing.Size(156, 32);
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
            this.skillList.HideSelection = false;
            this.skillList.Location = new System.Drawing.Point(8, 5);
            this.skillList.Name = "skillList";
            this.skillList.Size = new System.Drawing.Size(342, 307);
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
            this.agentsTab.Location = new System.Drawing.Point(4, 44);
            this.agentsTab.Name = "agentsTab";
            this.agentsTab.Size = new System.Drawing.Size(519, 322);
            this.agentsTab.TabIndex = 6;
            this.agentsTab.Text = "Agents";
            // 
            // agentSetHotKey
            // 
            this.agentSetHotKey.Location = new System.Drawing.Point(8, 274);
            this.agentSetHotKey.Name = "agentSetHotKey";
            this.agentSetHotKey.Size = new System.Drawing.Size(130, 32);
            this.agentSetHotKey.TabIndex = 7;
            this.agentSetHotKey.Text = "Set Hot Key";
            this.agentSetHotKey.Visible = false;
            this.agentSetHotKey.Click += new System.EventHandler(this.agentSetHotKey_Click);
            // 
            // agentB6
            // 
            this.agentB6.Location = new System.Drawing.Point(8, 231);
            this.agentB6.Name = "agentB6";
            this.agentB6.Size = new System.Drawing.Size(130, 32);
            this.agentB6.TabIndex = 6;
            this.agentB6.Click += new System.EventHandler(this.agentB6_Click);
            // 
            // agentB5
            // 
            this.agentB5.Location = new System.Drawing.Point(8, 193);
            this.agentB5.Name = "agentB5";
            this.agentB5.Size = new System.Drawing.Size(130, 32);
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
            this.agentList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.agentList_MouseDown);
            // 
            // agentGroup
            // 
            this.agentGroup.Controls.Add(this.agentSubList);
            this.agentGroup.Location = new System.Drawing.Point(144, 3);
            this.agentGroup.Name = "agentGroup";
            this.agentGroup.Size = new System.Drawing.Size(368, 309);
            this.agentGroup.TabIndex = 1;
            this.agentGroup.TabStop = false;
            // 
            // agentSubList
            // 
            this.agentSubList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.agentSubList.IntegralHeight = false;
            this.agentSubList.ItemHeight = 15;
            this.agentSubList.Location = new System.Drawing.Point(6, 22);
            this.agentSubList.Name = "agentSubList";
            this.agentSubList.Size = new System.Drawing.Size(356, 281);
            this.agentSubList.TabIndex = 0;
            this.agentSubList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.agentSubList_MouseDown);
            // 
            // agentB4
            // 
            this.agentB4.Location = new System.Drawing.Point(8, 155);
            this.agentB4.Name = "agentB4";
            this.agentB4.Size = new System.Drawing.Size(130, 32);
            this.agentB4.TabIndex = 4;
            this.agentB4.Click += new System.EventHandler(this.agentB4_Click);
            // 
            // agentB1
            // 
            this.agentB1.Location = new System.Drawing.Point(8, 41);
            this.agentB1.Name = "agentB1";
            this.agentB1.Size = new System.Drawing.Size(130, 32);
            this.agentB1.TabIndex = 1;
            this.agentB1.Click += new System.EventHandler(this.agentB1_Click);
            // 
            // agentB2
            // 
            this.agentB2.Location = new System.Drawing.Point(8, 79);
            this.agentB2.Name = "agentB2";
            this.agentB2.Size = new System.Drawing.Size(130, 32);
            this.agentB2.TabIndex = 2;
            this.agentB2.Click += new System.EventHandler(this.agentB2_Click);
            // 
            // agentB3
            // 
            this.agentB3.Location = new System.Drawing.Point(8, 117);
            this.agentB3.Name = "agentB3";
            this.agentB3.Size = new System.Drawing.Size(130, 32);
            this.agentB3.TabIndex = 3;
            this.agentB3.Click += new System.EventHandler(this.agentB3_Click);
            // 
            // filtersTab
            // 
            this.filtersTab.BackColor = System.Drawing.SystemColors.Control;
            this.filtersTab.Controls.Add(this.filterTabs);
            this.filtersTab.Location = new System.Drawing.Point(4, 44);
            this.filtersTab.Name = "filtersTab";
            this.filtersTab.Size = new System.Drawing.Size(519, 322);
            this.filtersTab.TabIndex = 15;
            this.filtersTab.Text = "Filters";
            // 
            // filterTabs
            // 
            this.filterTabs.Controls.Add(this.subFilterTab);
            this.filterTabs.Controls.Add(this.subFilterText);
            this.filterTabs.Controls.Add(this.subFilterSoundMusic);
            this.filterTabs.Controls.Add(this.subFilterTargets);
            this.filterTabs.Location = new System.Drawing.Point(6, 3);
            this.filterTabs.Name = "filterTabs";
            this.filterTabs.SelectedIndex = 0;
            this.filterTabs.Size = new System.Drawing.Size(506, 313);
            this.filterTabs.TabIndex = 1;
            this.filterTabs.SelectedIndexChanged += new System.EventHandler(this.filterTabs_IndexChanged);
            // 
            // subFilterTab
            // 
            this.subFilterTab.BackColor = System.Drawing.SystemColors.Control;
            this.subFilterTab.Controls.Add(this.daemonAnimationList);
            this.subFilterTab.Controls.Add(this.filterDaemonGraphics);
            this.subFilterTab.Controls.Add(this.drakeAnimationList);
            this.subFilterTab.Controls.Add(this.filterDrakeGraphics);
            this.subFilterTab.Controls.Add(this.dragonAnimationList);
            this.subFilterTab.Controls.Add(this.filterDragonGraphics);
            this.subFilterTab.Controls.Add(this.filters);
            this.subFilterTab.Location = new System.Drawing.Point(4, 24);
            this.subFilterTab.Name = "subFilterTab";
            this.subFilterTab.Padding = new System.Windows.Forms.Padding(3);
            this.subFilterTab.Size = new System.Drawing.Size(498, 285);
            this.subFilterTab.TabIndex = 0;
            this.subFilterTab.Text = "General";
            // 
            // daemonAnimationList
            // 
            this.daemonAnimationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.daemonAnimationList.DropDownWidth = 250;
            this.daemonAnimationList.FormattingEnabled = true;
            this.daemonAnimationList.Location = new System.Drawing.Point(313, 64);
            this.daemonAnimationList.Name = "daemonAnimationList";
            this.daemonAnimationList.Size = new System.Drawing.Size(163, 23);
            this.daemonAnimationList.TabIndex = 127;
            this.daemonAnimationList.SelectedIndexChanged += new System.EventHandler(this.daemonAnimationList_SelectedIndexChanged);
            // 
            // filterDaemonGraphics
            // 
            this.filterDaemonGraphics.AutoSize = true;
            this.filterDaemonGraphics.Location = new System.Drawing.Point(209, 66);
            this.filterDaemonGraphics.Name = "filterDaemonGraphics";
            this.filterDaemonGraphics.Size = new System.Drawing.Size(104, 19);
            this.filterDaemonGraphics.TabIndex = 126;
            this.filterDaemonGraphics.Text = "Filter daemons";
            this.filterDaemonGraphics.UseVisualStyleBackColor = true;
            this.filterDaemonGraphics.CheckedChanged += new System.EventHandler(this.filterDaemonGraphics_CheckedChanged);
            // 
            // drakeAnimationList
            // 
            this.drakeAnimationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drakeAnimationList.DropDownWidth = 250;
            this.drakeAnimationList.FormattingEnabled = true;
            this.drakeAnimationList.Location = new System.Drawing.Point(313, 35);
            this.drakeAnimationList.Name = "drakeAnimationList";
            this.drakeAnimationList.Size = new System.Drawing.Size(163, 23);
            this.drakeAnimationList.TabIndex = 118;
            this.drakeAnimationList.SelectedIndexChanged += new System.EventHandler(this.drakeAnimationList_SelectedIndexChanged);
            // 
            // filterDrakeGraphics
            // 
            this.filterDrakeGraphics.AutoSize = true;
            this.filterDrakeGraphics.Location = new System.Drawing.Point(209, 37);
            this.filterDrakeGraphics.Name = "filterDrakeGraphics";
            this.filterDrakeGraphics.Size = new System.Drawing.Size(89, 19);
            this.filterDrakeGraphics.TabIndex = 117;
            this.filterDrakeGraphics.Text = "Filter drakes";
            this.filterDrakeGraphics.UseVisualStyleBackColor = true;
            this.filterDrakeGraphics.CheckedChanged += new System.EventHandler(this.filterDrakeGraphics_CheckedChanged);
            // 
            // dragonAnimationList
            // 
            this.dragonAnimationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dragonAnimationList.DropDownWidth = 250;
            this.dragonAnimationList.FormattingEnabled = true;
            this.dragonAnimationList.Location = new System.Drawing.Point(313, 6);
            this.dragonAnimationList.Name = "dragonAnimationList";
            this.dragonAnimationList.Size = new System.Drawing.Size(163, 23);
            this.dragonAnimationList.TabIndex = 116;
            this.dragonAnimationList.SelectedIndexChanged += new System.EventHandler(this.dragonAnimationList_SelectedIndexChanged);
            // 
            // filterDragonGraphics
            // 
            this.filterDragonGraphics.AutoSize = true;
            this.filterDragonGraphics.Location = new System.Drawing.Point(209, 8);
            this.filterDragonGraphics.Name = "filterDragonGraphics";
            this.filterDragonGraphics.Size = new System.Drawing.Size(98, 19);
            this.filterDragonGraphics.TabIndex = 115;
            this.filterDragonGraphics.Text = "Filter dragons";
            this.filterDragonGraphics.UseVisualStyleBackColor = true;
            this.filterDragonGraphics.CheckedChanged += new System.EventHandler(this.filterDragonGraphics_CheckedChanged);
            // 
            // filters
            // 
            this.filters.CheckOnClick = true;
            this.filters.IntegralHeight = false;
            this.filters.Location = new System.Drawing.Point(6, 6);
            this.filters.Name = "filters";
            this.filters.Size = new System.Drawing.Size(197, 273);
            this.filters.TabIndex = 114;
            this.filters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnFilterCheck);
            // 
            // subFilterText
            // 
            this.subFilterText.BackColor = System.Drawing.SystemColors.Control;
            this.subFilterText.Controls.Add(this.gbFilterText);
            this.subFilterText.Controls.Add(this.gbFilterMessages);
            this.subFilterText.Location = new System.Drawing.Point(4, 22);
            this.subFilterText.Name = "subFilterText";
            this.subFilterText.Size = new System.Drawing.Size(498, 287);
            this.subFilterText.TabIndex = 4;
            this.subFilterText.Text = "Text && Messages  ";
            // 
            // gbFilterText
            // 
            this.gbFilterText.Controls.Add(this.removeFilterText);
            this.gbFilterText.Controls.Add(this.addFilterText);
            this.gbFilterText.Controls.Add(this.editFilterText);
            this.gbFilterText.Controls.Add(this.textFilterList);
            this.gbFilterText.Controls.Add(this.enableTextFilter);
            this.gbFilterText.Location = new System.Drawing.Point(3, 3);
            this.gbFilterText.Name = "gbFilterText";
            this.gbFilterText.Size = new System.Drawing.Size(229, 279);
            this.gbFilterText.TabIndex = 134;
            this.gbFilterText.TabStop = false;
            this.gbFilterText.Text = "Text Filter";
            // 
            // removeFilterText
            // 
            this.removeFilterText.Location = new System.Drawing.Point(148, 244);
            this.removeFilterText.Name = "removeFilterText";
            this.removeFilterText.Size = new System.Drawing.Size(75, 29);
            this.removeFilterText.TabIndex = 3;
            this.removeFilterText.Text = "Remove";
            this.removeFilterText.UseVisualStyleBackColor = true;
            this.removeFilterText.Click += new System.EventHandler(this.removeFilterText_Click);
            // 
            // addFilterText
            // 
            this.addFilterText.Location = new System.Drawing.Point(67, 244);
            this.addFilterText.Name = "addFilterText";
            this.addFilterText.Size = new System.Drawing.Size(75, 29);
            this.addFilterText.TabIndex = 2;
            this.addFilterText.Text = "Add";
            this.addFilterText.UseVisualStyleBackColor = true;
            this.addFilterText.Click += new System.EventHandler(this.addFilterText_Click);
            // 
            // editFilterText
            // 
            this.editFilterText.Location = new System.Drawing.Point(5, 244);
            this.editFilterText.Name = "editFilterText";
            this.editFilterText.Size = new System.Drawing.Size(75, 29);
            this.editFilterText.TabIndex = 1;
            this.editFilterText.Text = "Add";
            this.editFilterText.UseVisualStyleBackColor = true;
            this.editFilterText.Click += new System.EventHandler(this.editFilterText_Click);
            // 
            // textFilterList
            // 
            this.textFilterList.FormattingEnabled = true;
            this.textFilterList.ItemHeight = 15;
            this.textFilterList.Location = new System.Drawing.Point(6, 48);
            this.textFilterList.Name = "textFilterList";
            this.textFilterList.Size = new System.Drawing.Size(217, 184);
            this.textFilterList.TabIndex = 0;
            // 
            // enableTextFilter
            // 
            this.enableTextFilter.AutoSize = true;
            this.enableTextFilter.Location = new System.Drawing.Point(6, 22);
            this.enableTextFilter.Name = "enableTextFilter";
            this.enableTextFilter.Size = new System.Drawing.Size(111, 19);
            this.enableTextFilter.TabIndex = 3;
            this.enableTextFilter.Text = "Enable text filter";
            this.enableTextFilter.UseVisualStyleBackColor = true;
            this.enableTextFilter.CheckedChanged += new System.EventHandler(this.enableTextFilter_CheckedChanged);
            // 
            // gbFilterMessages
            // 
            this.gbFilterMessages.Controls.Add(this.filterOverheadMessages);
            this.gbFilterMessages.Controls.Add(this.lblFilterDelaySeconds);
            this.gbFilterMessages.Controls.Add(this.lblFilterDelay);
            this.gbFilterMessages.Controls.Add(this.filterDelaySeconds);
            this.gbFilterMessages.Controls.Add(this.filterRazorMessages);
            this.gbFilterMessages.Controls.Add(this.filterSystemMessages);
            this.gbFilterMessages.Controls.Add(this.filterSnoop);
            this.gbFilterMessages.Location = new System.Drawing.Point(238, 3);
            this.gbFilterMessages.Name = "gbFilterMessages";
            this.gbFilterMessages.Size = new System.Drawing.Size(257, 279);
            this.gbFilterMessages.TabIndex = 133;
            this.gbFilterMessages.TabStop = false;
            this.gbFilterMessages.Text = "Filter Messages";
            // 
            // filterOverheadMessages
            // 
            this.filterOverheadMessages.Location = new System.Drawing.Point(6, 100);
            this.filterOverheadMessages.Name = "filterOverheadMessages";
            this.filterOverheadMessages.Size = new System.Drawing.Size(220, 20);
            this.filterOverheadMessages.TabIndex = 139;
            this.filterOverheadMessages.Text = "Filter repeating overhead messages";
            this.filterOverheadMessages.CheckedChanged += new System.EventHandler(this.filterOverheadMessages_CheckedChanged);
            // 
            // lblFilterDelaySeconds
            // 
            this.lblFilterDelaySeconds.Location = new System.Drawing.Point(143, 130);
            this.lblFilterDelaySeconds.Name = "lblFilterDelaySeconds";
            this.lblFilterDelaySeconds.Size = new System.Drawing.Size(59, 18);
            this.lblFilterDelaySeconds.TabIndex = 138;
            this.lblFilterDelaySeconds.Text = "seconds";
            // 
            // lblFilterDelay
            // 
            this.lblFilterDelay.AutoSize = true;
            this.lblFilterDelay.Location = new System.Drawing.Point(27, 130);
            this.lblFilterDelay.Name = "lblFilterDelay";
            this.lblFilterDelay.Size = new System.Drawing.Size(68, 15);
            this.lblFilterDelay.TabIndex = 137;
            this.lblFilterDelay.Text = "Filter Delay:";
            // 
            // filterDelaySeconds
            // 
            this.filterDelaySeconds.Location = new System.Drawing.Point(101, 127);
            this.filterDelaySeconds.Name = "filterDelaySeconds";
            this.filterDelaySeconds.Size = new System.Drawing.Size(36, 23);
            this.filterDelaySeconds.TabIndex = 136;
            this.filterDelaySeconds.Text = "3.5";
            this.filterDelaySeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.filterDelaySeconds.TextChanged += new System.EventHandler(this.filterDelaySeconds_TextChanged);
            // 
            // filterRazorMessages
            // 
            this.filterRazorMessages.Location = new System.Drawing.Point(6, 74);
            this.filterRazorMessages.Name = "filterRazorMessages";
            this.filterRazorMessages.Size = new System.Drawing.Size(220, 20);
            this.filterRazorMessages.TabIndex = 135;
            this.filterRazorMessages.Text = "Filter repeating Razor messages";
            this.filterRazorMessages.CheckedChanged += new System.EventHandler(this.filterRazorMessages_CheckedChanged);
            // 
            // filterSystemMessages
            // 
            this.filterSystemMessages.Location = new System.Drawing.Point(6, 48);
            this.filterSystemMessages.Name = "filterSystemMessages";
            this.filterSystemMessages.Size = new System.Drawing.Size(220, 20);
            this.filterSystemMessages.TabIndex = 134;
            this.filterSystemMessages.Text = "Filter repeating system messages";
            this.filterSystemMessages.CheckedChanged += new System.EventHandler(this.filterSystemMessages_CheckedChanged);
            // 
            // filterSnoop
            // 
            this.filterSnoop.Location = new System.Drawing.Point(6, 22);
            this.filterSnoop.Name = "filterSnoop";
            this.filterSnoop.Size = new System.Drawing.Size(220, 20);
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
            this.subFilterSoundMusic.Location = new System.Drawing.Point(4, 22);
            this.subFilterSoundMusic.Name = "subFilterSoundMusic";
            this.subFilterSoundMusic.Size = new System.Drawing.Size(498, 287);
            this.subFilterSoundMusic.TabIndex = 3;
            this.subFilterSoundMusic.Text = "Sound & Music  ";
            // 
            // playableMusicList
            // 
            this.playableMusicList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.playableMusicList.FormattingEnabled = true;
            this.playableMusicList.Location = new System.Drawing.Point(215, 151);
            this.playableMusicList.Name = "playableMusicList";
            this.playableMusicList.Size = new System.Drawing.Size(145, 23);
            this.playableMusicList.TabIndex = 8;
            // 
            // playMusic
            // 
            this.playMusic.Location = new System.Drawing.Point(215, 180);
            this.playMusic.Name = "playMusic";
            this.playMusic.Size = new System.Drawing.Size(132, 23);
            this.playMusic.TabIndex = 7;
            this.playMusic.Text = "Play Music In Client";
            this.playMusic.UseVisualStyleBackColor = true;
            this.playMusic.Click += new System.EventHandler(this.playMusic_Click);
            // 
            // showPlayingMusic
            // 
            this.showPlayingMusic.AutoSize = true;
            this.showPlayingMusic.Location = new System.Drawing.Point(215, 215);
            this.showPlayingMusic.Name = "showPlayingMusic";
            this.showPlayingMusic.Size = new System.Drawing.Size(156, 19);
            this.showPlayingMusic.TabIndex = 6;
            this.showPlayingMusic.Text = "Show playing music info";
            this.showPlayingMusic.UseVisualStyleBackColor = true;
            this.showPlayingMusic.CheckedChanged += new System.EventHandler(this.showPlayingMusic_CheckedChanged);
            // 
            // showPlayingSoundInfo
            // 
            this.showPlayingSoundInfo.AutoSize = true;
            this.showPlayingSoundInfo.Location = new System.Drawing.Point(215, 106);
            this.showPlayingSoundInfo.Name = "showPlayingSoundInfo";
            this.showPlayingSoundInfo.Size = new System.Drawing.Size(181, 19);
            this.showPlayingSoundInfo.TabIndex = 5;
            this.showPlayingSoundInfo.Text = "Show non-filtered sound info";
            this.showPlayingSoundInfo.UseVisualStyleBackColor = true;
            this.showPlayingSoundInfo.CheckedChanged += new System.EventHandler(this.showPlayingSoundInfo_CheckedChanged);
            // 
            // showFilteredSound
            // 
            this.showFilteredSound.AutoSize = true;
            this.showFilteredSound.Location = new System.Drawing.Point(215, 81);
            this.showFilteredSound.Name = "showFilteredSound";
            this.showFilteredSound.Size = new System.Drawing.Size(155, 19);
            this.showFilteredSound.TabIndex = 4;
            this.showFilteredSound.Text = "Show filtered sound info";
            this.showFilteredSound.UseVisualStyleBackColor = true;
            this.showFilteredSound.CheckedChanged += new System.EventHandler(this.showFilteredSound_CheckedChanged);
            // 
            // playInClient
            // 
            this.playInClient.AutoSize = true;
            this.playInClient.Location = new System.Drawing.Point(348, 42);
            this.playInClient.Name = "playInClient";
            this.playInClient.Size = new System.Drawing.Size(93, 19);
            this.playInClient.TabIndex = 3;
            this.playInClient.Text = "Play in client";
            this.playInClient.UseVisualStyleBackColor = true;
            // 
            // playSound
            // 
            this.playSound.Location = new System.Drawing.Point(215, 39);
            this.playSound.Name = "playSound";
            this.playSound.Size = new System.Drawing.Size(127, 23);
            this.playSound.TabIndex = 2;
            this.playSound.Text = "Play Selected Sound";
            this.playSound.UseVisualStyleBackColor = true;
            this.playSound.Click += new System.EventHandler(this.playSound_Click);
            // 
            // soundFilterEnabled
            // 
            this.soundFilterEnabled.AutoSize = true;
            this.soundFilterEnabled.Location = new System.Drawing.Point(215, 14);
            this.soundFilterEnabled.Name = "soundFilterEnabled";
            this.soundFilterEnabled.Size = new System.Drawing.Size(127, 19);
            this.soundFilterEnabled.TabIndex = 1;
            this.soundFilterEnabled.Text = "Enable Sound Filter";
            this.soundFilterEnabled.UseVisualStyleBackColor = true;
            this.soundFilterEnabled.CheckedChanged += new System.EventHandler(this.soundFilterEnabled_CheckedChanged);
            // 
            // soundFilterList
            // 
            this.soundFilterList.FormattingEnabled = true;
            this.soundFilterList.Location = new System.Drawing.Point(16, 14);
            this.soundFilterList.Name = "soundFilterList";
            this.soundFilterList.Size = new System.Drawing.Size(193, 256);
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
            this.subFilterTargets.Location = new System.Drawing.Point(4, 22);
            this.subFilterTargets.Name = "subFilterTargets";
            this.subFilterTargets.Padding = new System.Windows.Forms.Padding(3);
            this.subFilterTargets.Size = new System.Drawing.Size(498, 287);
            this.subFilterTargets.TabIndex = 1;
            this.subFilterTargets.Text = "Target Filter";
            // 
            // lblTargetFilter
            // 
            this.lblTargetFilter.Location = new System.Drawing.Point(6, 84);
            this.lblTargetFilter.Name = "lblTargetFilter";
            this.lblTargetFilter.Size = new System.Drawing.Size(217, 52);
            this.lblTargetFilter.TabIndex = 18;
            this.lblTargetFilter.Text = "Targets added to this list will be ignored by Razor completely when using any tar" +
    "get hotkeys.";
            // 
            // targetFilterClear
            // 
            this.targetFilterClear.Location = new System.Drawing.Point(415, 241);
            this.targetFilterClear.Name = "targetFilterClear";
            this.targetFilterClear.Size = new System.Drawing.Size(77, 29);
            this.targetFilterClear.TabIndex = 17;
            this.targetFilterClear.Text = "Clear List";
            this.targetFilterClear.UseVisualStyleBackColor = true;
            this.targetFilterClear.Click += new System.EventHandler(this.TargetFilterClear_Click);
            // 
            // targetFilterRemove
            // 
            this.targetFilterRemove.Location = new System.Drawing.Point(335, 241);
            this.targetFilterRemove.Name = "targetFilterRemove";
            this.targetFilterRemove.Size = new System.Drawing.Size(74, 29);
            this.targetFilterRemove.TabIndex = 16;
            this.targetFilterRemove.Text = "Remove";
            this.targetFilterRemove.UseVisualStyleBackColor = true;
            this.targetFilterRemove.Click += new System.EventHandler(this.TargetFilterRemove_Click);
            // 
            // targetFilterAdd
            // 
            this.targetFilterAdd.Location = new System.Drawing.Point(247, 241);
            this.targetFilterAdd.Name = "targetFilterAdd";
            this.targetFilterAdd.Size = new System.Drawing.Size(82, 29);
            this.targetFilterAdd.TabIndex = 15;
            this.targetFilterAdd.Text = "Add (Target)";
            this.targetFilterAdd.UseVisualStyleBackColor = true;
            this.targetFilterAdd.Click += new System.EventHandler(this.TargetFilterAdd_Click);
            // 
            // targetFilter
            // 
            this.targetFilter.FormattingEnabled = true;
            this.targetFilter.ItemHeight = 15;
            this.targetFilter.Location = new System.Drawing.Point(247, 6);
            this.targetFilter.Name = "targetFilter";
            this.targetFilter.Size = new System.Drawing.Size(245, 229);
            this.targetFilter.TabIndex = 14;
            // 
            // targetFilterEnabled
            // 
            this.targetFilterEnabled.AutoSize = true;
            this.targetFilterEnabled.Location = new System.Drawing.Point(6, 6);
            this.targetFilterEnabled.Name = "targetFilterEnabled";
            this.targetFilterEnabled.Size = new System.Drawing.Size(132, 19);
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
            this.hotkeysTab.Location = new System.Drawing.Point(4, 44);
            this.hotkeysTab.Name = "hotkeysTab";
            this.hotkeysTab.Size = new System.Drawing.Size(519, 322);
            this.hotkeysTab.TabIndex = 4;
            this.hotkeysTab.Text = "Hot Keys";
            // 
            // filterHotkeys
            // 
            this.filterHotkeys.Location = new System.Drawing.Point(50, 8);
            this.filterHotkeys.Name = "filterHotkeys";
            this.filterHotkeys.Size = new System.Drawing.Size(281, 23);
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
            this.hkStatus.Location = new System.Drawing.Point(337, 206);
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
            this.hotkeyTree.Size = new System.Drawing.Size(323, 275);
            this.hotkeyTree.Sorted = true;
            this.hotkeyTree.TabIndex = 6;
            this.hotkeyTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.hotkeyTree_AfterSelect);
            // 
            // dohotkey
            // 
            this.dohotkey.Location = new System.Drawing.Point(337, 140);
            this.dohotkey.Name = "dohotkey";
            this.dohotkey.Size = new System.Drawing.Size(175, 29);
            this.dohotkey.TabIndex = 5;
            this.dohotkey.Text = "Execute Selected Hot Key";
            this.dohotkey.Click += new System.EventHandler(this.dohotkey_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.chkAlt);
            this.groupBox8.Controls.Add(this.chkPass);
            this.groupBox8.Controls.Add(this.label2);
            this.groupBox8.Controls.Add(this.unsetHK);
            this.groupBox8.Controls.Add(this.setHK);
            this.groupBox8.Controls.Add(this.key);
            this.groupBox8.Controls.Add(this.chkCtrl);
            this.groupBox8.Controls.Add(this.chkShift);
            this.groupBox8.Location = new System.Drawing.Point(337, 8);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(175, 124);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Hot Key";
            // 
            // chkAlt
            // 
            this.chkAlt.Location = new System.Drawing.Point(58, 20);
            this.chkAlt.Name = "chkAlt";
            this.chkAlt.Size = new System.Drawing.Size(49, 16);
            this.chkAlt.TabIndex = 2;
            this.chkAlt.Text = "Alt";
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
            this.chkCtrl.Size = new System.Drawing.Size(56, 16);
            this.chkCtrl.TabIndex = 1;
            this.chkCtrl.Text = "Ctrl";
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
            this.macrosTab.Controls.Add(this.tabControl2);
            this.macrosTab.Location = new System.Drawing.Point(4, 44);
            this.macrosTab.Name = "macrosTab";
            this.macrosTab.Size = new System.Drawing.Size(519, 322);
            this.macrosTab.TabIndex = 7;
            this.macrosTab.Text = "Macros";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.subMacrosTab);
            this.tabControl2.Controls.Add(this.subMacrosOptionsTab);
            this.tabControl2.Location = new System.Drawing.Point(6, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(510, 314);
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
            this.subMacrosTab.Location = new System.Drawing.Point(4, 24);
            this.subMacrosTab.Name = "subMacrosTab";
            this.subMacrosTab.Padding = new System.Windows.Forms.Padding(3);
            this.subMacrosTab.Size = new System.Drawing.Size(502, 286);
            this.subMacrosTab.TabIndex = 0;
            this.subMacrosTab.Text = "Macros";
            // 
            // macroActGroup
            // 
            this.macroActGroup.Controls.Add(this.setMacroHotKey);
            this.macroActGroup.Controls.Add(this.playMacro);
            this.macroActGroup.Controls.Add(this.waitDisp);
            this.macroActGroup.Controls.Add(this.loopMacro);
            this.macroActGroup.Controls.Add(this.recMacro);
            this.macroActGroup.Controls.Add(this.actionList);
            this.macroActGroup.Location = new System.Drawing.Point(200, 3);
            this.macroActGroup.Name = "macroActGroup";
            this.macroActGroup.Size = new System.Drawing.Size(296, 272);
            this.macroActGroup.TabIndex = 18;
            this.macroActGroup.TabStop = false;
            this.macroActGroup.Text = "Actions";
            this.macroActGroup.Visible = false;
            // 
            // setMacroHotKey
            // 
            this.setMacroHotKey.Location = new System.Drawing.Point(230, 95);
            this.setMacroHotKey.Name = "setMacroHotKey";
            this.setMacroHotKey.Size = new System.Drawing.Size(60, 33);
            this.setMacroHotKey.TabIndex = 7;
            this.setMacroHotKey.Text = "Set HK";
            this.setMacroHotKey.Click += new System.EventHandler(this.SetMacroHotKey_Click);
            // 
            // playMacro
            // 
            this.playMacro.Location = new System.Drawing.Point(230, 17);
            this.playMacro.Name = "playMacro";
            this.playMacro.Size = new System.Drawing.Size(60, 33);
            this.playMacro.TabIndex = 6;
            this.playMacro.Text = "Play";
            this.playMacro.Click += new System.EventHandler(this.playMacro_Click);
            // 
            // waitDisp
            // 
            this.waitDisp.Location = new System.Drawing.Point(230, 132);
            this.waitDisp.Name = "waitDisp";
            this.waitDisp.Size = new System.Drawing.Size(60, 89);
            this.waitDisp.TabIndex = 5;
            this.waitDisp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loopMacro
            // 
            this.loopMacro.Location = new System.Drawing.Point(233, 242);
            this.loopMacro.Name = "loopMacro";
            this.loopMacro.Size = new System.Drawing.Size(57, 24);
            this.loopMacro.TabIndex = 4;
            this.loopMacro.Text = "Loop";
            this.loopMacro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.loopMacro.CheckedChanged += new System.EventHandler(this.loopMacro_CheckedChanged);
            // 
            // recMacro
            // 
            this.recMacro.Location = new System.Drawing.Point(230, 56);
            this.recMacro.Name = "recMacro";
            this.recMacro.Size = new System.Drawing.Size(60, 33);
            this.recMacro.TabIndex = 3;
            this.recMacro.Text = "Record";
            this.recMacro.Click += new System.EventHandler(this.recMacro_Click);
            // 
            // actionList
            // 
            this.actionList.BackColor = System.Drawing.SystemColors.Window;
            this.actionList.HorizontalScrollbar = true;
            this.actionList.IntegralHeight = false;
            this.actionList.ItemHeight = 15;
            this.actionList.Location = new System.Drawing.Point(6, 17);
            this.actionList.Name = "actionList";
            this.actionList.Size = new System.Drawing.Size(218, 249);
            this.actionList.TabIndex = 0;
            this.actionList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.actionList_KeyDown);
            this.actionList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.actionList_MouseDown);
            // 
            // filterMacros
            // 
            this.filterMacros.Location = new System.Drawing.Point(47, 7);
            this.filterMacros.Name = "filterMacros";
            this.filterMacros.Size = new System.Drawing.Size(147, 23);
            this.filterMacros.TabIndex = 17;
            this.filterMacros.TextChanged += new System.EventHandler(this.filterMacros_TextChanged);
            // 
            // filterLabel
            // 
            this.filterLabel.AutoSize = true;
            this.filterLabel.Location = new System.Drawing.Point(5, 10);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(36, 15);
            this.filterLabel.TabIndex = 16;
            this.filterLabel.Text = "Filter:";
            // 
            // macroTree
            // 
            this.macroTree.FullRowSelect = true;
            this.macroTree.HideSelection = false;
            this.macroTree.Location = new System.Drawing.Point(6, 36);
            this.macroTree.Name = "macroTree";
            this.macroTree.Size = new System.Drawing.Size(188, 203);
            this.macroTree.Sorted = true;
            this.macroTree.TabIndex = 15;
            this.macroTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.macroTree_AfterSelect);
            this.macroTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.macroTree_MouseDown);
            // 
            // delMacro
            // 
            this.delMacro.Location = new System.Drawing.Point(120, 245);
            this.delMacro.Name = "delMacro";
            this.delMacro.Size = new System.Drawing.Size(74, 30);
            this.delMacro.TabIndex = 14;
            this.delMacro.Text = "Remove";
            this.delMacro.Click += new System.EventHandler(this.delMacro_Click);
            // 
            // newMacro
            // 
            this.newMacro.Location = new System.Drawing.Point(8, 245);
            this.newMacro.Name = "newMacro";
            this.newMacro.Size = new System.Drawing.Size(74, 30);
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
            this.subMacrosOptionsTab.Location = new System.Drawing.Point(4, 22);
            this.subMacrosOptionsTab.Name = "subMacrosOptionsTab";
            this.subMacrosOptionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.subMacrosOptionsTab.Size = new System.Drawing.Size(502, 288);
            this.subMacrosOptionsTab.TabIndex = 1;
            this.subMacrosOptionsTab.Text = "Options";
            // 
            // disableMacroPlayFinish
            // 
            this.disableMacroPlayFinish.AutoSize = true;
            this.disableMacroPlayFinish.Location = new System.Drawing.Point(272, 183);
            this.disableMacroPlayFinish.Name = "disableMacroPlayFinish";
            this.disableMacroPlayFinish.Size = new System.Drawing.Size(204, 19);
            this.disableMacroPlayFinish.TabIndex = 17;
            this.disableMacroPlayFinish.Text = "Disable Playing/Finished Message";
            this.disableMacroPlayFinish.UseVisualStyleBackColor = true;
            this.disableMacroPlayFinish.CheckedChanged += new System.EventHandler(this.disableMacroPlayFinish_CheckedChanged);
            // 
            // macroActionDelay
            // 
            this.macroActionDelay.AutoSize = true;
            this.macroActionDelay.Location = new System.Drawing.Point(272, 158);
            this.macroActionDelay.Name = "macroActionDelay";
            this.macroActionDelay.Size = new System.Drawing.Size(207, 19);
            this.macroActionDelay.TabIndex = 16;
            this.macroActionDelay.Text = "Default macro action delay (50ms)";
            this.macroActionDelay.UseVisualStyleBackColor = true;
            this.macroActionDelay.CheckedChanged += new System.EventHandler(this.macroActionDelay_CheckedChanged);
            // 
            // rangeCheckDoubleClick
            // 
            this.rangeCheckDoubleClick.AutoSize = true;
            this.rangeCheckDoubleClick.Location = new System.Drawing.Point(272, 78);
            this.rangeCheckDoubleClick.Name = "rangeCheckDoubleClick";
            this.rangeCheckDoubleClick.Size = new System.Drawing.Size(207, 19);
            this.rangeCheckDoubleClick.TabIndex = 15;
            this.rangeCheckDoubleClick.Text = "Range check on \'DoubleClickType\'";
            this.rangeCheckDoubleClick.UseVisualStyleBackColor = true;
            this.rangeCheckDoubleClick.CheckedChanged += new System.EventHandler(this.rangeCheckDoubleClick_CheckedChanged);
            // 
            // rangeCheckTargetByType
            // 
            this.rangeCheckTargetByType.AutoSize = true;
            this.rangeCheckTargetByType.Location = new System.Drawing.Point(272, 53);
            this.rangeCheckTargetByType.Name = "rangeCheckTargetByType";
            this.rangeCheckTargetByType.Size = new System.Drawing.Size(188, 19);
            this.rangeCheckTargetByType.TabIndex = 14;
            this.rangeCheckTargetByType.Text = "Range check on \'TargetByType\'";
            this.rangeCheckTargetByType.UseVisualStyleBackColor = true;
            this.rangeCheckTargetByType.CheckedChanged += new System.EventHandler(this.rangeCheckTargetByType_CheckedChanged);
            // 
            // nextMacroAction
            // 
            this.nextMacroAction.Enabled = false;
            this.nextMacroAction.Location = new System.Drawing.Point(412, 115);
            this.nextMacroAction.Name = "nextMacroAction";
            this.nextMacroAction.Size = new System.Drawing.Size(60, 23);
            this.nextMacroAction.TabIndex = 13;
            this.nextMacroAction.Text = "Next";
            this.nextMacroAction.UseVisualStyleBackColor = true;
            this.nextMacroAction.Click += new System.EventHandler(this.nextMacroAction_Click);
            // 
            // stepThroughMacro
            // 
            this.stepThroughMacro.AutoSize = true;
            this.stepThroughMacro.Location = new System.Drawing.Point(272, 118);
            this.stepThroughMacro.Name = "stepThroughMacro";
            this.stepThroughMacro.Size = new System.Drawing.Size(134, 19);
            this.stepThroughMacro.TabIndex = 12;
            this.stepThroughMacro.Text = "Step Through Macro";
            this.stepThroughMacro.UseVisualStyleBackColor = true;
            this.stepThroughMacro.CheckedChanged += new System.EventHandler(this.stepThroughMacro_CheckedChanged);
            // 
            // targetByTypeDifferent
            // 
            this.targetByTypeDifferent.AutoSize = true;
            this.targetByTypeDifferent.Location = new System.Drawing.Point(272, 28);
            this.targetByTypeDifferent.Name = "targetByTypeDifferent";
            this.targetByTypeDifferent.Size = new System.Drawing.Size(181, 19);
            this.targetByTypeDifferent.TabIndex = 11;
            this.targetByTypeDifferent.Text = "Force different \'TargetByType\'";
            this.targetByTypeDifferent.UseVisualStyleBackColor = true;
            this.targetByTypeDifferent.CheckedChanged += new System.EventHandler(this.targetByTypeDifferent_CheckedChanged);
            // 
            // macroVariableGroup
            // 
            this.macroVariableGroup.Controls.Add(this.macroVariableTypeList);
            this.macroVariableGroup.Controls.Add(this.retargetMacroVariable);
            this.macroVariableGroup.Controls.Add(this.insertMacroVariable);
            this.macroVariableGroup.Controls.Add(this.removeMacroVariable);
            this.macroVariableGroup.Controls.Add(this.addMacroVariable);
            this.macroVariableGroup.Controls.Add(this.macroVariables);
            this.macroVariableGroup.Location = new System.Drawing.Point(6, 6);
            this.macroVariableGroup.Name = "macroVariableGroup";
            this.macroVariableGroup.Size = new System.Drawing.Size(240, 269);
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
            this.macroVariableTypeList.Location = new System.Drawing.Point(79, 22);
            this.macroVariableTypeList.Name = "macroVariableTypeList";
            this.macroVariableTypeList.Size = new System.Drawing.Size(153, 23);
            this.macroVariableTypeList.TabIndex = 6;
            this.macroVariableTypeList.SelectedIndexChanged += new System.EventHandler(this.macroVariableTypeList_SelectedIndexChanged);
            // 
            // retargetMacroVariable
            // 
            this.retargetMacroVariable.Location = new System.Drawing.Point(6, 84);
            this.retargetMacroVariable.Name = "retargetMacroVariable";
            this.retargetMacroVariable.Size = new System.Drawing.Size(67, 25);
            this.retargetMacroVariable.TabIndex = 5;
            this.retargetMacroVariable.Text = "Retarget";
            this.retargetMacroVariable.UseVisualStyleBackColor = true;
            this.retargetMacroVariable.Click += new System.EventHandler(this.retargetMacroVariable_Click);
            // 
            // insertMacroVariable
            // 
            this.insertMacroVariable.Location = new System.Drawing.Point(6, 22);
            this.insertMacroVariable.Name = "insertMacroVariable";
            this.insertMacroVariable.Size = new System.Drawing.Size(67, 25);
            this.insertMacroVariable.TabIndex = 4;
            this.insertMacroVariable.Text = "Insert as...";
            this.insertMacroVariable.UseVisualStyleBackColor = true;
            this.insertMacroVariable.Click += new System.EventHandler(this.insertMacroVariable_Click);
            // 
            // removeMacroVariable
            // 
            this.removeMacroVariable.Location = new System.Drawing.Point(6, 115);
            this.removeMacroVariable.Name = "removeMacroVariable";
            this.removeMacroVariable.Size = new System.Drawing.Size(67, 25);
            this.removeMacroVariable.TabIndex = 3;
            this.removeMacroVariable.Text = "Remove";
            this.removeMacroVariable.UseVisualStyleBackColor = true;
            this.removeMacroVariable.Click += new System.EventHandler(this.removeMacroVariable_Click);
            // 
            // addMacroVariable
            // 
            this.addMacroVariable.Location = new System.Drawing.Point(6, 53);
            this.addMacroVariable.Name = "addMacroVariable";
            this.addMacroVariable.Size = new System.Drawing.Size(67, 25);
            this.addMacroVariable.TabIndex = 2;
            this.addMacroVariable.Text = "Add";
            this.addMacroVariable.UseVisualStyleBackColor = true;
            this.addMacroVariable.Click += new System.EventHandler(this.addMacroVariable_Click);
            // 
            // macroVariables
            // 
            this.macroVariables.FormattingEnabled = true;
            this.macroVariables.ItemHeight = 15;
            this.macroVariables.Location = new System.Drawing.Point(79, 53);
            this.macroVariables.Name = "macroVariables";
            this.macroVariables.Size = new System.Drawing.Size(153, 199);
            this.macroVariables.TabIndex = 1;
            // 
            // scriptsTab
            // 
            this.scriptsTab.BackColor = System.Drawing.SystemColors.Control;
            this.scriptsTab.Controls.Add(this.subTabScripts);
            this.scriptsTab.Location = new System.Drawing.Point(4, 44);
            this.scriptsTab.Name = "scriptsTab";
            this.scriptsTab.Size = new System.Drawing.Size(519, 322);
            this.scriptsTab.TabIndex = 13;
            this.scriptsTab.Text = "Scripts";
            // 
            // subTabScripts
            // 
            this.subTabScripts.Controls.Add(this.subScripts);
            this.subTabScripts.Controls.Add(this.subScriptOptions);
            this.subTabScripts.Location = new System.Drawing.Point(6, 3);
            this.subTabScripts.Name = "subTabScripts";
            this.subTabScripts.SelectedIndex = 0;
            this.subTabScripts.Size = new System.Drawing.Size(506, 313);
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
            this.subScripts.Location = new System.Drawing.Point(4, 24);
            this.subScripts.Name = "subScripts";
            this.subScripts.Padding = new System.Windows.Forms.Padding(3);
            this.subScripts.Size = new System.Drawing.Size(498, 285);
            this.subScripts.TabIndex = 0;
            this.subScripts.Text = "Scripts";
            // 
            // scriptHotkey
            // 
            this.scriptHotkey.ForeColor = System.Drawing.SystemColors.ControlText;
            this.scriptHotkey.Location = new System.Drawing.Point(432, 168);
            this.scriptHotkey.Name = "scriptHotkey";
            this.scriptHotkey.Size = new System.Drawing.Size(60, 35);
            this.scriptHotkey.TabIndex = 29;
            this.scriptHotkey.Text = "Not Set";
            this.scriptHotkey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scriptSplitContainer
            // 
            this.scriptSplitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.scriptSplitContainer.Location = new System.Drawing.Point(7, 7);
            this.scriptSplitContainer.Name = "scriptSplitContainer";
            // 
            // scriptSplitContainer.Panel1
            // 
            this.scriptSplitContainer.Panel1.Controls.Add(this.scriptTree);
            this.scriptSplitContainer.Panel1.Controls.Add(this.scriptFilter);
            // 
            // scriptSplitContainer.Panel2
            // 
            this.scriptSplitContainer.Panel2.Controls.Add(this.scriptEditor);
            this.scriptSplitContainer.Size = new System.Drawing.Size(419, 272);
            this.scriptSplitContainer.SplitterDistance = 110;
            this.scriptSplitContainer.SplitterWidth = 10;
            this.scriptSplitContainer.TabIndex = 28;
            // 
            // scriptTree
            // 
            this.scriptTree.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scriptTree.Location = new System.Drawing.Point(0, 31);
            this.scriptTree.Name = "scriptTree";
            this.scriptTree.Size = new System.Drawing.Size(110, 241);
            this.scriptTree.Sorted = true;
            this.scriptTree.TabIndex = 29;
            this.scriptTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.scriptTree_AfterSelect);
            this.scriptTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scriptTree_MouseDown);
            // 
            // scriptFilter
            // 
            this.scriptFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.scriptFilter.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scriptFilter.Location = new System.Drawing.Point(0, 0);
            this.scriptFilter.Name = "scriptFilter";
            this.scriptFilter.Size = new System.Drawing.Size(110, 25);
            this.scriptFilter.TabIndex = 28;
            this.scriptFilter.TextChanged += new System.EventHandler(this.scriptFilter_TextChanged);
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
            this.scriptEditor.AutoScrollMinSize = new System.Drawing.Size(2, 15);
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
            this.scriptEditor.Location = new System.Drawing.Point(0, 0);
            this.scriptEditor.Name = "scriptEditor";
            this.scriptEditor.Paddings = new System.Windows.Forms.Padding(0);
            this.scriptEditor.RightBracket = ')';
            this.scriptEditor.RightBracket2 = ']';
            this.scriptEditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.scriptEditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("scriptEditor.ServiceColors")));
            this.scriptEditor.ShowCaretWhenInactive = false;
            this.scriptEditor.Size = new System.Drawing.Size(299, 272);
            this.scriptEditor.TabIndex = 21;
            this.scriptEditor.Zoom = 100;
            this.scriptEditor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.scriptEditor_KeyDown);
            this.scriptEditor.LostFocus += new System.EventHandler(this.scriptEditor_LostFocus);
            this.scriptEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scriptEditor_MouseDown);
            // 
            // scriptGuide
            // 
            this.scriptGuide.Location = new System.Drawing.Point(432, 253);
            this.scriptGuide.Name = "scriptGuide";
            this.scriptGuide.Size = new System.Drawing.Size(60, 26);
            this.scriptGuide.TabIndex = 26;
            this.scriptGuide.Text = "Help";
            this.scriptGuide.UseVisualStyleBackColor = true;
            this.scriptGuide.Click += new System.EventHandler(this.scriptGuide_Click);
            // 
            // saveScript
            // 
            this.saveScript.Location = new System.Drawing.Point(432, 83);
            this.saveScript.Name = "saveScript";
            this.saveScript.Size = new System.Drawing.Size(60, 32);
            this.saveScript.TabIndex = 22;
            this.saveScript.Text = "Save";
            this.saveScript.UseVisualStyleBackColor = true;
            this.saveScript.Click += new System.EventHandler(this.saveScript_Click);
            // 
            // newScript
            // 
            this.newScript.Location = new System.Drawing.Point(432, 221);
            this.newScript.Name = "newScript";
            this.newScript.Size = new System.Drawing.Size(60, 26);
            this.newScript.TabIndex = 18;
            this.newScript.Text = "New";
            this.newScript.UseVisualStyleBackColor = true;
            this.newScript.Click += new System.EventHandler(this.newScript_Click);
            // 
            // setScriptHotkey
            // 
            this.setScriptHotkey.Location = new System.Drawing.Point(432, 133);
            this.setScriptHotkey.Name = "setScriptHotkey";
            this.setScriptHotkey.Size = new System.Drawing.Size(60, 32);
            this.setScriptHotkey.TabIndex = 16;
            this.setScriptHotkey.Text = "Set HK";
            this.setScriptHotkey.UseVisualStyleBackColor = true;
            this.setScriptHotkey.Click += new System.EventHandler(this.setScriptHotkey_Click);
            // 
            // recordScript
            // 
            this.recordScript.Location = new System.Drawing.Point(432, 45);
            this.recordScript.Name = "recordScript";
            this.recordScript.Size = new System.Drawing.Size(60, 32);
            this.recordScript.TabIndex = 15;
            this.recordScript.Text = "Record";
            this.recordScript.UseVisualStyleBackColor = true;
            this.recordScript.Click += new System.EventHandler(this.recordScript_Click);
            // 
            // playScript
            // 
            this.playScript.Location = new System.Drawing.Point(432, 7);
            this.playScript.Name = "playScript";
            this.playScript.Size = new System.Drawing.Size(60, 32);
            this.playScript.TabIndex = 14;
            this.playScript.Text = "Play";
            this.playScript.UseVisualStyleBackColor = true;
            this.playScript.Click += new System.EventHandler(this.playScript_Click);
            // 
            // subScriptOptions
            // 
            this.subScriptOptions.BackColor = System.Drawing.SystemColors.Control;
            this.subScriptOptions.Controls.Add(this.disableScriptTooltips);
            this.subScriptOptions.Controls.Add(this.scriptDisablePlayFinish);
            this.subScriptOptions.Controls.Add(this.autoSaveScriptPlay);
            this.subScriptOptions.Controls.Add(this.autoSaveScript);
            this.subScriptOptions.Controls.Add(this.scriptVariablesBox);
            this.subScriptOptions.Location = new System.Drawing.Point(4, 22);
            this.subScriptOptions.Name = "subScriptOptions";
            this.subScriptOptions.Padding = new System.Windows.Forms.Padding(3);
            this.subScriptOptions.Size = new System.Drawing.Size(498, 287);
            this.subScriptOptions.TabIndex = 1;
            this.subScriptOptions.Text = "Options";
            // 
            // disableScriptTooltips
            // 
            this.disableScriptTooltips.AutoSize = true;
            this.disableScriptTooltips.Location = new System.Drawing.Point(252, 115);
            this.disableScriptTooltips.Name = "disableScriptTooltips";
            this.disableScriptTooltips.Size = new System.Drawing.Size(107, 19);
            this.disableScriptTooltips.TabIndex = 20;
            this.disableScriptTooltips.Text = "Disable tooltips";
            this.disableScriptTooltips.UseVisualStyleBackColor = true;
            this.disableScriptTooltips.CheckedChanged += new System.EventHandler(this.disableScriptTooltips_CheckedChanged);
            // 
            // scriptDisablePlayFinish
            // 
            this.scriptDisablePlayFinish.AutoSize = true;
            this.scriptDisablePlayFinish.Location = new System.Drawing.Point(252, 90);
            this.scriptDisablePlayFinish.Name = "scriptDisablePlayFinish";
            this.scriptDisablePlayFinish.Size = new System.Drawing.Size(204, 19);
            this.scriptDisablePlayFinish.TabIndex = 19;
            this.scriptDisablePlayFinish.Text = "Disable Playing/Finished Message";
            this.scriptDisablePlayFinish.UseVisualStyleBackColor = true;
            this.scriptDisablePlayFinish.CheckedChanged += new System.EventHandler(this.disableScriptPlayFinish_CheckedChanged);
            // 
            // autoSaveScriptPlay
            // 
            this.autoSaveScriptPlay.AutoSize = true;
            this.autoSaveScriptPlay.Location = new System.Drawing.Point(252, 53);
            this.autoSaveScriptPlay.Name = "autoSaveScriptPlay";
            this.autoSaveScriptPlay.Size = new System.Drawing.Size(193, 19);
            this.autoSaveScriptPlay.TabIndex = 9;
            this.autoSaveScriptPlay.Text = "Auto-save when you click \'Play\'";
            this.autoSaveScriptPlay.UseVisualStyleBackColor = true;
            this.autoSaveScriptPlay.CheckedChanged += new System.EventHandler(this.autoSaveScriptPlay_CheckedChanged);
            // 
            // autoSaveScript
            // 
            this.autoSaveScript.AutoSize = true;
            this.autoSaveScript.Location = new System.Drawing.Point(252, 28);
            this.autoSaveScript.Name = "autoSaveScript";
            this.autoSaveScript.Size = new System.Drawing.Size(239, 19);
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
            this.scriptVariablesBox.Location = new System.Drawing.Point(6, 6);
            this.scriptVariablesBox.Name = "scriptVariablesBox";
            this.scriptVariablesBox.Size = new System.Drawing.Size(240, 269);
            this.scriptVariablesBox.TabIndex = 7;
            this.scriptVariablesBox.TabStop = false;
            this.scriptVariablesBox.Text = "Script Variables:";
            // 
            // changeScriptVariable
            // 
            this.changeScriptVariable.Location = new System.Drawing.Point(6, 53);
            this.changeScriptVariable.Name = "changeScriptVariable";
            this.changeScriptVariable.Size = new System.Drawing.Size(67, 25);
            this.changeScriptVariable.TabIndex = 5;
            this.changeScriptVariable.Text = "Retarget";
            this.changeScriptVariable.UseVisualStyleBackColor = true;
            this.changeScriptVariable.Click += new System.EventHandler(this.changeScriptVariable_Click);
            // 
            // removeScriptVariable
            // 
            this.removeScriptVariable.Location = new System.Drawing.Point(6, 84);
            this.removeScriptVariable.Name = "removeScriptVariable";
            this.removeScriptVariable.Size = new System.Drawing.Size(67, 25);
            this.removeScriptVariable.TabIndex = 3;
            this.removeScriptVariable.Text = "Remove";
            this.removeScriptVariable.UseVisualStyleBackColor = true;
            this.removeScriptVariable.Click += new System.EventHandler(this.removeScriptVariable_Click);
            // 
            // addScriptVariable
            // 
            this.addScriptVariable.Location = new System.Drawing.Point(6, 22);
            this.addScriptVariable.Name = "addScriptVariable";
            this.addScriptVariable.Size = new System.Drawing.Size(67, 25);
            this.addScriptVariable.TabIndex = 2;
            this.addScriptVariable.Text = "Add";
            this.addScriptVariable.UseVisualStyleBackColor = true;
            this.addScriptVariable.Click += new System.EventHandler(this.addScriptVariable_Click);
            // 
            // scriptVariables
            // 
            this.scriptVariables.FormattingEnabled = true;
            this.scriptVariables.ItemHeight = 15;
            this.scriptVariables.Location = new System.Drawing.Point(79, 22);
            this.scriptVariables.Name = "scriptVariables";
            this.scriptVariables.Size = new System.Drawing.Size(153, 229);
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
            this.friendsTab.Location = new System.Drawing.Point(4, 44);
            this.friendsTab.Name = "friendsTab";
            this.friendsTab.Size = new System.Drawing.Size(519, 322);
            this.friendsTab.TabIndex = 14;
            this.friendsTab.Text = "Friends";
            // 
            // showPartyFriendOverhead
            // 
            this.showPartyFriendOverhead.Location = new System.Drawing.Point(283, 40);
            this.showPartyFriendOverhead.Name = "showPartyFriendOverhead";
            this.showPartyFriendOverhead.Size = new System.Drawing.Size(217, 20);
            this.showPartyFriendOverhead.TabIndex = 144;
            this.showPartyFriendOverhead.Text = "Show [Party-Friend] overhead";
            this.showPartyFriendOverhead.CheckedChanged += new System.EventHandler(this.showPartyFriendOverhead_CheckedChanged);
            // 
            // highlightFriend
            // 
            this.highlightFriend.Location = new System.Drawing.Point(270, 117);
            this.highlightFriend.Name = "highlightFriend";
            this.highlightFriend.Size = new System.Drawing.Size(184, 23);
            this.highlightFriend.TabIndex = 143;
            this.highlightFriend.Text = "Next/Prev highlights \'Friends\'";
            this.highlightFriend.UseVisualStyleBackColor = true;
            this.highlightFriend.CheckedChanged += new System.EventHandler(this.highlightFriend_CheckedChanged);
            // 
            // autoAcceptParty
            // 
            this.autoAcceptParty.Location = new System.Drawing.Point(270, 91);
            this.autoAcceptParty.Name = "autoAcceptParty";
            this.autoAcceptParty.Size = new System.Drawing.Size(230, 20);
            this.autoAcceptParty.TabIndex = 138;
            this.autoAcceptParty.Text = "Auto-accept party invites from friends";
            this.autoAcceptParty.CheckedChanged += new System.EventHandler(this.autoAcceptParty_CheckedChanged);
            // 
            // nextPrevIgnoresFriends
            // 
            this.nextPrevIgnoresFriends.AutoSize = true;
            this.nextPrevIgnoresFriends.Location = new System.Drawing.Point(270, 66);
            this.nextPrevIgnoresFriends.Name = "nextPrevIgnoresFriends";
            this.nextPrevIgnoresFriends.Size = new System.Drawing.Size(203, 19);
            this.nextPrevIgnoresFriends.TabIndex = 137;
            this.nextPrevIgnoresFriends.Text = "Next/Prev Target ignores \'Friends\'";
            this.nextPrevIgnoresFriends.UseVisualStyleBackColor = true;
            this.nextPrevIgnoresFriends.CheckedChanged += new System.EventHandler(this.nextPrevIgnoresFriends_CheckedChanged);
            // 
            // autoFriend
            // 
            this.autoFriend.Location = new System.Drawing.Point(270, 14);
            this.autoFriend.Name = "autoFriend";
            this.autoFriend.Size = new System.Drawing.Size(217, 20);
            this.autoFriend.TabIndex = 136;
            this.autoFriend.Text = "Treat party members as \'Friends\'";
            this.autoFriend.CheckedChanged += new System.EventHandler(this.autoFriend_CheckedChanged);
            // 
            // friendsGroupBox
            // 
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
            this.friendsGroupBox.Location = new System.Drawing.Point(6, 3);
            this.friendsGroupBox.Name = "friendsGroupBox";
            this.friendsGroupBox.Size = new System.Drawing.Size(258, 313);
            this.friendsGroupBox.TabIndex = 135;
            this.friendsGroupBox.TabStop = false;
            this.friendsGroupBox.Text = "Friends Groups && Lists";
            // 
            // showFriendOverhead
            // 
            this.showFriendOverhead.Location = new System.Drawing.Point(6, 52);
            this.showFriendOverhead.Name = "showFriendOverhead";
            this.showFriendOverhead.Size = new System.Drawing.Size(184, 23);
            this.showFriendOverhead.TabIndex = 146;
            this.showFriendOverhead.Text = "Show overhead tag";
            this.showFriendOverhead.UseVisualStyleBackColor = true;
            this.showFriendOverhead.CheckedChanged += new System.EventHandler(this.showFriendOverhead_CheckedChanged);
            // 
            // setFriendsFormatHue
            // 
            this.setFriendsFormatHue.Location = new System.Drawing.Point(163, 78);
            this.setFriendsFormatHue.Name = "setFriendsFormatHue";
            this.setFriendsFormatHue.Size = new System.Drawing.Size(59, 23);
            this.setFriendsFormatHue.TabIndex = 145;
            this.setFriendsFormatHue.Text = "Set Hue";
            this.setFriendsFormatHue.UseVisualStyleBackColor = true;
            this.setFriendsFormatHue.Click += new System.EventHandler(this.setFriendsFormatHue_Click);
            // 
            // friendOverheadFormat
            // 
            this.friendOverheadFormat.Location = new System.Drawing.Point(55, 78);
            this.friendOverheadFormat.Name = "friendOverheadFormat";
            this.friendOverheadFormat.Size = new System.Drawing.Size(102, 23);
            this.friendOverheadFormat.TabIndex = 143;
            this.friendOverheadFormat.Text = "[Friend]";
            this.friendOverheadFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.friendOverheadFormat.TextChanged += new System.EventHandler(this.friendOverheadFormat_TextChanged);
            // 
            // friendFormat
            // 
            this.friendFormat.Location = new System.Drawing.Point(6, 78);
            this.friendFormat.Name = "friendFormat";
            this.friendFormat.Size = new System.Drawing.Size(151, 23);
            this.friendFormat.TabIndex = 144;
            this.friendFormat.Text = "Format:";
            this.friendFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // friendsGroupRemove
            // 
            this.friendsGroupRemove.Location = new System.Drawing.Point(157, 21);
            this.friendsGroupRemove.Name = "friendsGroupRemove";
            this.friendsGroupRemove.Size = new System.Drawing.Size(21, 25);
            this.friendsGroupRemove.TabIndex = 11;
            this.friendsGroupRemove.Text = "-";
            this.friendsGroupRemove.UseVisualStyleBackColor = true;
            this.friendsGroupRemove.Click += new System.EventHandler(this.friendsGroupRemove_Click);
            // 
            // friendsGroupAdd
            // 
            this.friendsGroupAdd.Location = new System.Drawing.Point(130, 21);
            this.friendsGroupAdd.Name = "friendsGroupAdd";
            this.friendsGroupAdd.Size = new System.Drawing.Size(21, 25);
            this.friendsGroupAdd.TabIndex = 10;
            this.friendsGroupAdd.Text = "+";
            this.friendsGroupAdd.UseVisualStyleBackColor = true;
            this.friendsGroupAdd.Click += new System.EventHandler(this.friendsGroupAdd_Click);
            // 
            // friendClearList
            // 
            this.friendClearList.Location = new System.Drawing.Point(174, 274);
            this.friendClearList.Name = "friendClearList";
            this.friendClearList.Size = new System.Drawing.Size(79, 33);
            this.friendClearList.TabIndex = 8;
            this.friendClearList.Text = "Clear List";
            this.friendClearList.UseVisualStyleBackColor = true;
            this.friendClearList.Click += new System.EventHandler(this.friendClearList_Click);
            // 
            // friendRemoveSelected
            // 
            this.friendRemoveSelected.Location = new System.Drawing.Point(94, 274);
            this.friendRemoveSelected.Name = "friendRemoveSelected";
            this.friendRemoveSelected.Size = new System.Drawing.Size(74, 33);
            this.friendRemoveSelected.TabIndex = 7;
            this.friendRemoveSelected.Text = "Remove";
            this.friendRemoveSelected.UseVisualStyleBackColor = true;
            this.friendRemoveSelected.Click += new System.EventHandler(this.friendRemoveSelected_Click);
            // 
            // friendAddTarget
            // 
            this.friendAddTarget.Location = new System.Drawing.Point(6, 274);
            this.friendAddTarget.Name = "friendAddTarget";
            this.friendAddTarget.Size = new System.Drawing.Size(82, 33);
            this.friendAddTarget.TabIndex = 5;
            this.friendAddTarget.Text = "Add (Target)";
            this.friendAddTarget.UseVisualStyleBackColor = true;
            this.friendAddTarget.Click += new System.EventHandler(this.FriendAddTarget_Click);
            // 
            // friendsList
            // 
            this.friendsList.FormattingEnabled = true;
            this.friendsList.ItemHeight = 15;
            this.friendsList.Location = new System.Drawing.Point(5, 114);
            this.friendsList.Name = "friendsList";
            this.friendsList.Size = new System.Drawing.Size(247, 154);
            this.friendsList.TabIndex = 4;
            this.friendsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.friendsList_KeyDown);
            this.friendsList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.friendsList_MouseDown);
            // 
            // friendsGroup
            // 
            this.friendsGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.friendsGroup.FormattingEnabled = true;
            this.friendsGroup.Location = new System.Drawing.Point(6, 22);
            this.friendsGroup.Name = "friendsGroup";
            this.friendsGroup.Size = new System.Drawing.Size(118, 23);
            this.friendsGroup.TabIndex = 3;
            this.friendsGroup.SelectedIndexChanged += new System.EventHandler(this.friendsGroup_SelectedIndexChanged);
            // 
            // friendsListEnabled
            // 
            this.friendsListEnabled.Location = new System.Drawing.Point(184, 23);
            this.friendsListEnabled.Name = "friendsListEnabled";
            this.friendsListEnabled.Size = new System.Drawing.Size(69, 20);
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
            this.screenshotTab.Location = new System.Drawing.Point(4, 44);
            this.screenshotTab.Name = "screenshotTab";
            this.screenshotTab.Size = new System.Drawing.Size(519, 322);
            this.screenshotTab.TabIndex = 8;
            this.screenshotTab.Text = "Screen Shots";
            // 
            // openScreenshotFolder
            // 
            this.openScreenshotFolder.Location = new System.Drawing.Point(266, 201);
            this.openScreenshotFolder.Name = "openScreenshotFolder";
            this.openScreenshotFolder.Size = new System.Drawing.Size(155, 23);
            this.openScreenshotFolder.TabIndex = 62;
            this.openScreenshotFolder.Text = "Open Screen Shot Folder";
            this.openScreenshotFolder.UseVisualStyleBackColor = true;
            this.openScreenshotFolder.Click += new System.EventHandler(this.openScreenshotFolder_Click);
            // 
            // captureOwnDeathDelay
            // 
            this.captureOwnDeathDelay.Location = new System.Drawing.Point(419, 263);
            this.captureOwnDeathDelay.Name = "captureOwnDeathDelay";
            this.captureOwnDeathDelay.Size = new System.Drawing.Size(32, 23);
            this.captureOwnDeathDelay.TabIndex = 61;
            this.captureOwnDeathDelay.Text = "0.5";
            this.captureOwnDeathDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.captureOwnDeathDelay.TextChanged += new System.EventHandler(this.CaptureOwnDeathDelay_TextChanged);
            // 
            // lblOwnDeathMs
            // 
            this.lblOwnDeathMs.Location = new System.Drawing.Point(453, 267);
            this.lblOwnDeathMs.Name = "lblOwnDeathMs";
            this.lblOwnDeathMs.Size = new System.Drawing.Size(26, 18);
            this.lblOwnDeathMs.TabIndex = 60;
            this.lblOwnDeathMs.Text = "s";
            // 
            // captureOwnDeath
            // 
            this.captureOwnDeath.Location = new System.Drawing.Point(235, 266);
            this.captureOwnDeath.Name = "captureOwnDeath";
            this.captureOwnDeath.Size = new System.Drawing.Size(158, 20);
            this.captureOwnDeath.TabIndex = 59;
            this.captureOwnDeath.Text = "Capture your own death";
            this.captureOwnDeath.CheckedChanged += new System.EventHandler(this.CaptureOwnDeath_CheckedChanged);
            // 
            // captureOthersDeathDelay
            // 
            this.captureOthersDeathDelay.Location = new System.Drawing.Point(419, 239);
            this.captureOthersDeathDelay.Name = "captureOthersDeathDelay";
            this.captureOthersDeathDelay.Size = new System.Drawing.Size(32, 23);
            this.captureOthersDeathDelay.TabIndex = 58;
            this.captureOthersDeathDelay.Text = "0.5";
            this.captureOthersDeathDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.captureOthersDeathDelay.TextChanged += new System.EventHandler(this.CaptureOthersDeathDelay_TextChanged);
            // 
            // lblCaptureOthersDeathMs
            // 
            this.lblCaptureOthersDeathMs.Location = new System.Drawing.Point(453, 242);
            this.lblCaptureOthersDeathMs.Name = "lblCaptureOthersDeathMs";
            this.lblCaptureOthersDeathMs.Size = new System.Drawing.Size(26, 18);
            this.lblCaptureOthersDeathMs.TabIndex = 57;
            this.lblCaptureOthersDeathMs.Text = "s";
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
            this.imgFmt.Location = new System.Drawing.Point(103, 237);
            this.imgFmt.Name = "imgFmt";
            this.imgFmt.Size = new System.Drawing.Size(98, 23);
            this.imgFmt.TabIndex = 11;
            this.imgFmt.SelectedIndexChanged += new System.EventHandler(this.imgFmt_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 237);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(92, 20);
            this.label12.TabIndex = 10;
            this.label12.Text = "Image Format:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // capNow
            // 
            this.capNow.Location = new System.Drawing.Point(266, 173);
            this.capNow.Name = "capNow";
            this.capNow.Size = new System.Drawing.Size(155, 22);
            this.capNow.TabIndex = 8;
            this.capNow.Text = "Take Screen Shot Now";
            this.capNow.Click += new System.EventHandler(this.capNow_Click);
            // 
            // screenPath
            // 
            this.screenPath.Location = new System.Drawing.Point(8, 12);
            this.screenPath.Name = "screenPath";
            this.screenPath.Size = new System.Drawing.Size(463, 23);
            this.screenPath.TabIndex = 7;
            this.screenPath.TextChanged += new System.EventHandler(this.screenPath_TextChanged);
            // 
            // radioUO
            // 
            this.radioUO.Location = new System.Drawing.Point(13, 260);
            this.radioUO.Name = "radioUO";
            this.radioUO.Size = new System.Drawing.Size(75, 26);
            this.radioUO.TabIndex = 6;
            this.radioUO.Text = "UO Only";
            this.radioUO.CheckedChanged += new System.EventHandler(this.radioUO_CheckedChanged);
            // 
            // radioFull
            // 
            this.radioFull.Location = new System.Drawing.Point(103, 260);
            this.radioFull.Name = "radioFull";
            this.radioFull.Size = new System.Drawing.Size(88, 26);
            this.radioFull.TabIndex = 5;
            this.radioFull.Text = "Full Screen";
            this.radioFull.CheckedChanged += new System.EventHandler(this.radioFull_CheckedChanged);
            // 
            // captureOthersDeath
            // 
            this.captureOthersDeath.Location = new System.Drawing.Point(235, 240);
            this.captureOthersDeath.Name = "captureOthersDeath";
            this.captureOthersDeath.Size = new System.Drawing.Size(216, 20);
            this.captureOthersDeath.TabIndex = 4;
            this.captureOthersDeath.Text = "Capture death of other players";
            this.captureOthersDeath.CheckedChanged += new System.EventHandler(this.CaptureOthersDeath_CheckedChanged);
            // 
            // setScnPath
            // 
            this.setScnPath.Location = new System.Drawing.Point(477, 12);
            this.setScnPath.Name = "setScnPath";
            this.setScnPath.Size = new System.Drawing.Size(35, 23);
            this.setScnPath.TabIndex = 3;
            this.setScnPath.Text = "...";
            this.setScnPath.Click += new System.EventHandler(this.setScnPath_Click);
            // 
            // screensList
            // 
            this.screensList.IntegralHeight = false;
            this.screensList.ItemHeight = 15;
            this.screensList.Location = new System.Drawing.Point(8, 41);
            this.screensList.Name = "screensList";
            this.screensList.Size = new System.Drawing.Size(252, 190);
            this.screensList.Sorted = true;
            this.screensList.TabIndex = 1;
            this.screensList.SelectedIndexChanged += new System.EventHandler(this.screensList_SelectedIndexChanged);
            this.screensList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screensList_MouseDown);
            // 
            // screenPrev
            // 
            this.screenPrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.screenPrev.Location = new System.Drawing.Point(266, 41);
            this.screenPrev.Name = "screenPrev";
            this.screenPrev.Size = new System.Drawing.Size(246, 126);
            this.screenPrev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.screenPrev.TabIndex = 0;
            this.screenPrev.TabStop = false;
            this.screenPrev.Click += new System.EventHandler(this.screenPrev_Click);
            // 
            // dispTime
            // 
            this.dispTime.Location = new System.Drawing.Point(13, 289);
            this.dispTime.Name = "dispTime";
            this.dispTime.Size = new System.Drawing.Size(206, 20);
            this.dispTime.TabIndex = 9;
            this.dispTime.Text = "Include Timestamp on images";
            this.dispTime.CheckedChanged += new System.EventHandler(this.dispTime_CheckedChanged);
            // 
            // advancedTab
            // 
            this.advancedTab.BackColor = System.Drawing.SystemColors.Control;
            this.advancedTab.Controls.Add(this.groupBox16);
            this.advancedTab.Controls.Add(this.enableUOAAPI);
            this.advancedTab.Controls.Add(this.disableSmartCPU);
            this.advancedTab.Controls.Add(this.negotiate);
            this.advancedTab.Controls.Add(this.openRazorDataDir);
            this.advancedTab.Controls.Add(this.msglvl);
            this.advancedTab.Controls.Add(this.label17);
            this.advancedTab.Controls.Add(this.logPackets);
            this.advancedTab.Controls.Add(this.statusBox);
            this.advancedTab.Controls.Add(this.features);
            this.advancedTab.Location = new System.Drawing.Point(4, 44);
            this.advancedTab.Name = "advancedTab";
            this.advancedTab.Size = new System.Drawing.Size(519, 322);
            this.advancedTab.TabIndex = 12;
            this.advancedTab.Text = "Advanced";
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.lastBackup);
            this.groupBox16.Controls.Add(this.openBackupFolder);
            this.groupBox16.Controls.Add(this.setBackupFolder);
            this.groupBox16.Controls.Add(this.createBackup);
            this.groupBox16.Location = new System.Drawing.Point(10, 184);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(225, 132);
            this.groupBox16.TabIndex = 80;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Backup";
            // 
            // lastBackup
            // 
            this.lastBackup.Location = new System.Drawing.Point(100, 63);
            this.lastBackup.Name = "lastBackup";
            this.lastBackup.Size = new System.Drawing.Size(119, 44);
            this.lastBackup.TabIndex = 82;
            this.lastBackup.Text = "Last Backup: 01/01/2019 5:54PM";
            this.lastBackup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openBackupFolder
            // 
            this.openBackupFolder.Location = new System.Drawing.Point(6, 70);
            this.openBackupFolder.Name = "openBackupFolder";
            this.openBackupFolder.Size = new System.Drawing.Size(88, 30);
            this.openBackupFolder.TabIndex = 81;
            this.openBackupFolder.Text = "Open Folder";
            this.openBackupFolder.UseVisualStyleBackColor = true;
            this.openBackupFolder.Click += new System.EventHandler(this.openBackupFolder_Click);
            // 
            // setBackupFolder
            // 
            this.setBackupFolder.Location = new System.Drawing.Point(134, 22);
            this.setBackupFolder.Name = "setBackupFolder";
            this.setBackupFolder.Size = new System.Drawing.Size(85, 30);
            this.setBackupFolder.TabIndex = 80;
            this.setBackupFolder.Text = "Set Folder";
            this.setBackupFolder.UseVisualStyleBackColor = true;
            this.setBackupFolder.Click += new System.EventHandler(this.setBackupFolder_Click);
            // 
            // createBackup
            // 
            this.createBackup.Location = new System.Drawing.Point(6, 22);
            this.createBackup.Name = "createBackup";
            this.createBackup.Size = new System.Drawing.Size(122, 30);
            this.createBackup.TabIndex = 79;
            this.createBackup.Text = "Create Backup";
            this.createBackup.UseVisualStyleBackColor = true;
            this.createBackup.Click += new System.EventHandler(this.createBackup_Click);
            // 
            // enableUOAAPI
            // 
            this.enableUOAAPI.Checked = true;
            this.enableUOAAPI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableUOAAPI.Location = new System.Drawing.Point(241, 102);
            this.enableUOAAPI.Name = "enableUOAAPI";
            this.enableUOAAPI.Size = new System.Drawing.Size(146, 23);
            this.enableUOAAPI.TabIndex = 75;
            this.enableUOAAPI.Text = "Enable UOA API";
            this.enableUOAAPI.CheckedChanged += new System.EventHandler(this.enableUOAAPI_CheckedChanged);
            // 
            // disableSmartCPU
            // 
            this.disableSmartCPU.Location = new System.Drawing.Point(392, 156);
            this.disableSmartCPU.Name = "disableSmartCPU";
            this.disableSmartCPU.Size = new System.Drawing.Size(119, 22);
            this.disableSmartCPU.TabIndex = 74;
            this.disableSmartCPU.Text = "Disable SmartCPU";
            this.disableSmartCPU.UseVisualStyleBackColor = true;
            this.disableSmartCPU.Click += new System.EventHandler(this.disableSmartCPU_Click);
            // 
            // negotiate
            // 
            this.negotiate.Location = new System.Drawing.Point(241, 76);
            this.negotiate.Name = "negotiate";
            this.negotiate.Size = new System.Drawing.Size(197, 20);
            this.negotiate.TabIndex = 72;
            this.negotiate.Text = "Negotiate features with server";
            this.negotiate.CheckedChanged += new System.EventHandler(this.negotiate_CheckedChanged);
            // 
            // openRazorDataDir
            // 
            this.openRazorDataDir.Location = new System.Drawing.Point(241, 8);
            this.openRazorDataDir.Name = "openRazorDataDir";
            this.openRazorDataDir.Size = new System.Drawing.Size(271, 33);
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
            this.msglvl.Location = new System.Drawing.Point(343, 127);
            this.msglvl.Name = "msglvl";
            this.msglvl.Size = new System.Drawing.Size(168, 23);
            this.msglvl.TabIndex = 69;
            this.msglvl.SelectedIndexChanged += new System.EventHandler(this.msglvl_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(238, 128);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(99, 18);
            this.label17.TabIndex = 68;
            this.label17.Text = "Razor messages:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // logPackets
            // 
            this.logPackets.Location = new System.Drawing.Point(241, 47);
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
            this.statusBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.HideSelection = false;
            this.statusBox.Location = new System.Drawing.Point(10, 8);
            this.statusBox.Multiline = true;
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            this.statusBox.Size = new System.Drawing.Size(225, 170);
            this.statusBox.TabIndex = 66;
            // 
            // features
            // 
            this.features.Cursor = System.Windows.Forms.Cursors.No;
            this.features.Location = new System.Drawing.Point(241, 184);
            this.features.Multiline = true;
            this.features.Name = "features";
            this.features.ReadOnly = true;
            this.features.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.features.Size = new System.Drawing.Size(271, 132);
            this.features.TabIndex = 65;
            this.features.Visible = false;
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
            this.aboutTab.Location = new System.Drawing.Point(4, 44);
            this.aboutTab.Name = "aboutTab";
            this.aboutTab.Size = new System.Drawing.Size(519, 322);
            this.aboutTab.TabIndex = 9;
            this.aboutTab.Text = "About";
            // 
            // linkGitHub
            // 
            this.linkGitHub.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkGitHub.Location = new System.Drawing.Point(9, 157);
            this.linkGitHub.Name = "linkGitHub";
            this.linkGitHub.Size = new System.Drawing.Size(506, 20);
            this.linkGitHub.TabIndex = 25;
            this.linkGitHub.TabStop = true;
            this.linkGitHub.Text = "https://github.com/markdwags/Razor";
            this.linkGitHub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGitHub_LinkClicked);
            // 
            // lblCredits3
            // 
            this.lblCredits3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits3.Location = new System.Drawing.Point(8, 259);
            this.lblCredits3.Name = "lblCredits3";
            this.lblCredits3.Size = new System.Drawing.Size(503, 20);
            this.lblCredits3.TabIndex = 24;
            this.lblCredits3.Text = "Cross-platform implementation by DarkLotus";
            this.lblCredits3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkHelp
            // 
            this.linkHelp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkHelp.Location = new System.Drawing.Point(434, 10);
            this.linkHelp.Name = "linkHelp";
            this.linkHelp.Size = new System.Drawing.Size(78, 20);
            this.linkHelp.TabIndex = 23;
            this.linkHelp.TabStop = true;
            this.linkHelp.Text = "Need Help?";
            this.linkHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHelp_LinkClicked);
            // 
            // lblCredits2
            // 
            this.lblCredits2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits2.Location = new System.Drawing.Point(9, 239);
            this.lblCredits2.Name = "lblCredits2";
            this.lblCredits2.Size = new System.Drawing.Size(503, 20);
            this.lblCredits2.TabIndex = 22;
            this.lblCredits2.Text = "Major design changes including ClassicUO integration by Jaedan";
            this.lblCredits2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(96, 120);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(343, 17);
            this.label20.TabIndex = 21;
            this.label20.Text = "For feedback, support and the latest releases please visit:\r\n";
            // 
            // lblCredits1
            // 
            this.lblCredits1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredits1.Location = new System.Drawing.Point(6, 219);
            this.lblCredits1.Name = "lblCredits1";
            this.lblCredits1.Size = new System.Drawing.Size(506, 20);
            this.lblCredits1.TabIndex = 19;
            this.lblCredits1.Text = "Razor was designed by Zippy, modified and maintained by Quick";
            this.lblCredits1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // aboutSubInfo
            // 
            this.aboutSubInfo.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutSubInfo.Location = new System.Drawing.Point(6, 75);
            this.aboutSubInfo.Name = "aboutSubInfo";
            this.aboutSubInfo.Size = new System.Drawing.Size(506, 19);
            this.aboutSubInfo.TabIndex = 17;
            this.aboutSubInfo.Text = "Community Edition";
            this.aboutSubInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkMain
            // 
            this.linkMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkMain.Location = new System.Drawing.Point(9, 137);
            this.linkMain.Name = "linkMain";
            this.linkMain.Size = new System.Drawing.Size(506, 20);
            this.linkMain.TabIndex = 16;
            this.linkMain.TabStop = true;
            this.linkMain.Text = "http://www.razorce.com";
            this.linkMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkMain.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
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
            this.aboutVer.Location = new System.Drawing.Point(6, 40);
            this.aboutVer.Name = "aboutVer";
            this.aboutVer.Size = new System.Drawing.Size(506, 35);
            this.aboutVer.TabIndex = 14;
            this.aboutVer.Text = "Razor v{0}";
            this.aboutVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(530, 372);
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
            this.Load += new System.EventHandler(this.MainForm_StartLoad);
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
            this.advancedTab.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.aboutTab.ResumeLayout(false);
            this.aboutTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabPage scriptsTab;
        private TabControl subTabScripts;
        private TabPage subScripts;
        private Button saveScript;
        private Button newScript;
        private Button setScriptHotkey;
        private Button recordScript;
        private Button playScript;
        private TabPage subScriptOptions;
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
        private CheckBox waypointOnDeath;
        private Label lblWaypointTiles;
        private TextBox hideWaypointDist;
        private CheckBox hideWaypointWithin;
        private TextBox txtWaypointDistanceSec;
        private Label lblWaypointSeconds;
        private CheckBox showWaypointDistance;
        private CheckBox showWaypointOverhead;
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
        private ListBox waypointList;
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
        private ListView cliLocOverheadView;
        private ColumnHeader cliLocOriginal;
        private ColumnHeader cliLocOverheadMessage;
        private Button editOverheadMessage;
        private Button setColorHue;
        private Button removeOverheadMessage;
        private Label label14;
        private RadioButton unicodeStyle;
        private RadioButton asciiStyle;
        private GroupBox groupBox16;
        private Label lastBackup;
        private Button openBackupFolder;
        private Button setBackupFolder;
        private Button createBackup;
        private CheckBox overrideSpellFormat;
        private CheckBox reequipHandsPotion;
        private Button agentSetHotKey;
        private TabPage subFilterText;
        private GroupBox gbFilterText;
        private Button removeFilterText;
        private Button addFilterText;
        private Button editFilterText;
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
        private CheckBox disableScriptTooltips;
        private SplitContainer scriptSplitContainer;
        private TreeView scriptTree;
        private TextBox scriptFilter;
        private FastColoredTextBoxNS.FastColoredTextBox scriptEditor;
        private Label scriptHotkey;
        private Button openScreenshotFolder;
        private CheckBox buyAgentIgnoreGold;
    }
}
