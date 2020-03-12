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
        private LinkLabel linkLabel1;
        private Label label20;
        private Button disableSmartCPU;
        private CheckBox logSkillChanges;
        private CheckBox screenShotOpenBrowser;
        private CheckBox screenShotNotification;
        private ListBox imgurUploads;
        private CheckBox screenShotClipboard;
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
        private Button overHeadMessages;
        private CheckBox showOverheadMessages;
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
        private TabPage tabPage1;
        private TabPage subFiltersTab;
        private CheckedListBox filters;
        private GroupBox groupBox16;
        private Button setBackupFolder;
        private Label lastBackup;
        private Button createBackup;
        private ComboBox clientPrio;
        private RadioButton systray;
        private RadioButton taskbar;
        private ComboBox langSel;
        private Label label7;
        private Label label11;
        private CheckBox showWelcome;
        private TrackBar opacity;
        private CheckBox alwaysTop;
        private Label opacityLabel;
        private Label label9;
        private GroupBox groupBox4;
        private Button saveProfile;
        private Button cloneProfile;
        private Button delProfile;
        private Button newProfile;
        private ComboBox profiles;
        private ComboBox drakeAnimationList;
        private CheckBox filterDrakeGraphics;
        private ComboBox dragonAnimationList;
        private CheckBox filterDragonGraphics;
        private Button openBackupFolder;
        private TextBox targetIndictorFormat;
        private Label lblTargetFormat;
        private Label lblStealthFormat;
        private TextBox stealthStepsFormat;
        private CheckBox dispDeltaOverhead;
        private Label lblCredits2;
        private LinkLabel linkGitHub;
        private Label lblCredits3;
        private GroupBox groupBox15;
        private Button openUOAM;
        private Button openUltimaMapper;
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
        private TabPage subOptionsFriendsTab;
        private GroupBox friendsGroupBox;
        private CheckBox friendsListEnabled;
        private Button friendClearList;
        private Button friendRemoveSelected;
        private Button friendAddTarget;
        private ListBox friendsList;
        private ComboBox friendsGroup;
        private Button friendsGroupRemove;
        private Button friendsGroupAdd;
        private CheckBox showFriendOverhead;
        private CheckBox autoAcceptParty;
        private CheckBox nextPrevIgnoresFriends;
        private CheckBox autoFriend;
        private Label friendFormat;
        private TextBox friendOverheadFormat;
        private Button setFriendsFormatHue;
        private Button setTargetIndicatorHue;
        private CheckBox filterRazorMessages;
        private CheckBox filterSystemMessages;
        private CheckBox filterSnoop;
        private Label lblFilterDelaySeconds;
        private Label lblFilterDelay;
        private TextBox filterDelaySeconds;
        private CheckBox filterOverheadMessages;
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
        private TabPage subTargetFilterTab;
        private CheckBox targetFilterEnabled;
        private ListBox targetFilter;
        private Button targetFilterClear;
        private Button targetFilterRemove;
        private Button targetFilterAdd;
        private Button setMacroHotKey;
        private ComboBox daemonAnimationList;
        private CheckBox filterDaemonGraphics;
        private Label lblTargetFilter;
        private TabPage subSoundMusicTab;
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

            m_NotifyIcon.ContextMenu =
                new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Show Razor", new EventHandler(DoShowMe)),
                    new MenuItem("Hide Razor", new EventHandler(HideMe)),
                    new MenuItem("-"),
                    new MenuItem("Toggle Razor Visibility", new EventHandler(ToggleVisible)),
                    new MenuItem("-"),
                    new MenuItem("Close Razor && UO", new EventHandler(OnClose))
                });
            m_NotifyIcon.ContextMenu.MenuItems[0].DefaultItem = true;
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.boatControl = new System.Windows.Forms.Button();
            this.openUOAM = new System.Windows.Forms.Button();
            this.openUltimaMapper = new System.Windows.Forms.Button();
            this.btnMap = new System.Windows.Forms.Button();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.openBackupFolder = new System.Windows.Forms.Button();
            this.setBackupFolder = new System.Windows.Forms.Button();
            this.lastBackup = new System.Windows.Forms.Label();
            this.createBackup = new System.Windows.Forms.Button();
            this.clientPrio = new System.Windows.Forms.ComboBox();
            this.systray = new System.Windows.Forms.RadioButton();
            this.taskbar = new System.Windows.Forms.RadioButton();
            this.langSel = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.showWelcome = new System.Windows.Forms.CheckBox();
            this.opacity = new System.Windows.Forms.TrackBar();
            this.alwaysTop = new System.Windows.Forms.CheckBox();
            this.opacityLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.saveProfile = new System.Windows.Forms.Button();
            this.cloneProfile = new System.Windows.Forms.Button();
            this.delProfile = new System.Windows.Forms.Button();
            this.newProfile = new System.Windows.Forms.Button();
            this.profiles = new System.Windows.Forms.ComboBox();
            this.subFiltersTab = new System.Windows.Forms.TabPage();
            this.daemonAnimationList = new System.Windows.Forms.ComboBox();
            this.filterDaemonGraphics = new System.Windows.Forms.CheckBox();
            this.filterOverheadMessages = new System.Windows.Forms.CheckBox();
            this.lblFilterDelaySeconds = new System.Windows.Forms.Label();
            this.lblFilterDelay = new System.Windows.Forms.Label();
            this.filterDelaySeconds = new System.Windows.Forms.TextBox();
            this.filterRazorMessages = new System.Windows.Forms.CheckBox();
            this.filterSystemMessages = new System.Windows.Forms.CheckBox();
            this.filterSnoop = new System.Windows.Forms.CheckBox();
            this.drakeAnimationList = new System.Windows.Forms.ComboBox();
            this.filterDrakeGraphics = new System.Windows.Forms.CheckBox();
            this.dragonAnimationList = new System.Windows.Forms.ComboBox();
            this.filterDragonGraphics = new System.Windows.Forms.CheckBox();
            this.filters = new System.Windows.Forms.CheckedListBox();
            this.subTargetFilterTab = new System.Windows.Forms.TabPage();
            this.lblTargetFilter = new System.Windows.Forms.Label();
            this.targetFilterClear = new System.Windows.Forms.Button();
            this.targetFilterRemove = new System.Windows.Forms.Button();
            this.targetFilterAdd = new System.Windows.Forms.Button();
            this.targetFilter = new System.Windows.Forms.ListBox();
            this.targetFilterEnabled = new System.Windows.Forms.CheckBox();
            this.subSoundMusicTab = new System.Windows.Forms.TabPage();
            this.playableMusicList = new System.Windows.Forms.ComboBox();
            this.playMusic = new System.Windows.Forms.Button();
            this.showPlayingMusic = new System.Windows.Forms.CheckBox();
            this.showPlayingSoundInfo = new System.Windows.Forms.CheckBox();
            this.showFilteredSound = new System.Windows.Forms.CheckBox();
            this.playInClient = new System.Windows.Forms.CheckBox();
            this.playSound = new System.Windows.Forms.Button();
            this.soundFilterEnabled = new System.Windows.Forms.CheckBox();
            this.soundFilterList = new System.Windows.Forms.CheckedListBox();
            this.moreOptTab = new System.Windows.Forms.TabPage();
            this.optionsTabCtrl = new System.Windows.Forms.TabControl();
            this.subOptionsSpeechTab = new System.Windows.Forms.TabPage();
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
            this.overHeadMessages = new System.Windows.Forms.Button();
            this.showOverheadMessages = new System.Windows.Forms.CheckBox();
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
            this.targetIndictorFormat = new System.Windows.Forms.TextBox();
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
            this.subOptionsFriendsTab = new System.Windows.Forms.TabPage();
            this.setFriendsFormatHue = new System.Windows.Forms.Button();
            this.friendOverheadFormat = new System.Windows.Forms.TextBox();
            this.showFriendOverhead = new System.Windows.Forms.CheckBox();
            this.autoAcceptParty = new System.Windows.Forms.CheckBox();
            this.nextPrevIgnoresFriends = new System.Windows.Forms.CheckBox();
            this.autoFriend = new System.Windows.Forms.CheckBox();
            this.friendsGroupBox = new System.Windows.Forms.GroupBox();
            this.friendsGroupRemove = new System.Windows.Forms.Button();
            this.friendsGroupAdd = new System.Windows.Forms.Button();
            this.friendClearList = new System.Windows.Forms.Button();
            this.friendRemoveSelected = new System.Windows.Forms.Button();
            this.friendAddTarget = new System.Windows.Forms.Button();
            this.friendsList = new System.Windows.Forms.ListBox();
            this.friendsGroup = new System.Windows.Forms.ComboBox();
            this.friendsListEnabled = new System.Windows.Forms.CheckBox();
            this.friendFormat = new System.Windows.Forms.Label();
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
            this.renameScript = new System.Windows.Forms.Button();
            this.linkScriptGuide = new System.Windows.Forms.LinkLabel();
            this.saveScript = new System.Windows.Forms.Button();
            this.deleteScript = new System.Windows.Forms.Button();
            this.scriptEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.scriptList = new System.Windows.Forms.ListBox();
            this.newScript = new System.Windows.Forms.Button();
            this.setScriptHotkey = new System.Windows.Forms.Button();
            this.recordScript = new System.Windows.Forms.Button();
            this.playScript = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.autoSaveScriptPlay = new System.Windows.Forms.CheckBox();
            this.autoSaveScript = new System.Windows.Forms.CheckBox();
            this.scriptVariablesBox = new System.Windows.Forms.GroupBox();
            this.changeScriptVariable = new System.Windows.Forms.Button();
            this.removeScriptVariable = new System.Windows.Forms.Button();
            this.addScriptVariable = new System.Windows.Forms.Button();
            this.scriptVariables = new System.Windows.Forms.ListBox();
            this.screenshotTab = new System.Windows.Forms.TabPage();
            this.captureOwnDeathDelay = new System.Windows.Forms.TextBox();
            this.lblOwnDeathMs = new System.Windows.Forms.Label();
            this.captureOwnDeath = new System.Windows.Forms.CheckBox();
            this.captureOthersDeathDelay = new System.Windows.Forms.TextBox();
            this.lblCaptureOthersDeathMs = new System.Windows.Forms.Label();
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
            this.captureOthersDeath = new System.Windows.Forms.CheckBox();
            this.setScnPath = new System.Windows.Forms.Button();
            this.screensList = new System.Windows.Forms.ListBox();
            this.screenPrev = new System.Windows.Forms.PictureBox();
            this.dispTime = new System.Windows.Forms.CheckBox();
            this.advancedTab = new System.Windows.Forms.TabPage();
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
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lblCredits1 = new System.Windows.Forms.Label();
            this.aboutSubInfo = new System.Windows.Forms.Label();
            this.linkMain = new System.Windows.Forms.LinkLabel();
            this.label21 = new System.Windows.Forms.Label();
            this.aboutVer = new System.Windows.Forms.Label();
            this.highlightFriend = new System.Windows.Forms.CheckBox();
            this.tabs.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.subGeneralTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.subFiltersTab.SuspendLayout();
            this.subTargetFilterTab.SuspendLayout();
            this.subSoundMusicTab.SuspendLayout();
            this.moreOptTab.SuspendLayout();
            this.optionsTabCtrl.SuspendLayout();
            this.subOptionsSpeechTab.SuspendLayout();
            this.subOptionsTargetTab.SuspendLayout();
            this.groupSmartTarget.SuspendLayout();
            this.subOptionsMiscTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lightLevelBar)).BeginInit();
            this.subOptionsFriendsTab.SuspendLayout();
            this.friendsGroupBox.SuspendLayout();
            this.displayTab.SuspendLayout();
            this.displayCountersTabCtrl.SuspendLayout();
            this.subDisplayTab.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.subCountersTab.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.subBandageTimerTab.SuspendLayout();
            this.dressTab.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.skillsTab.SuspendLayout();
            this.agentsTab.SuspendLayout();
            this.agentGroup.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.scriptEditor)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.scriptVariablesBox.SuspendLayout();
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
            this.tabs.Controls.Add(this.displayTab);
            this.tabs.Controls.Add(this.dressTab);
            this.tabs.Controls.Add(this.skillsTab);
            this.tabs.Controls.Add(this.agentsTab);
            this.tabs.Controls.Add(this.hotkeysTab);
            this.tabs.Controls.Add(this.macrosTab);
            this.tabs.Controls.Add(this.scriptsTab);
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
            this.subGeneralTab.Controls.Add(this.tabPage1);
            this.subGeneralTab.Controls.Add(this.subFiltersTab);
            this.subGeneralTab.Controls.Add(this.subTargetFilterTab);
            this.subGeneralTab.Controls.Add(this.subSoundMusicTab);
            this.subGeneralTab.Location = new System.Drawing.Point(6, 3);
            this.subGeneralTab.Name = "subGeneralTab";
            this.subGeneralTab.SelectedIndex = 0;
            this.subGeneralTab.Size = new System.Drawing.Size(510, 314);
            this.subGeneralTab.TabIndex = 63;
            this.subGeneralTab.SelectedIndexChanged += new System.EventHandler(this.subGeneralTab_IndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.groupBox15);
            this.tabPage1.Controls.Add(this.groupBox16);
            this.tabPage1.Controls.Add(this.clientPrio);
            this.tabPage1.Controls.Add(this.systray);
            this.tabPage1.Controls.Add(this.taskbar);
            this.tabPage1.Controls.Add(this.langSel);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.showWelcome);
            this.tabPage1.Controls.Add(this.opacity);
            this.tabPage1.Controls.Add(this.alwaysTop);
            this.tabPage1.Controls.Add(this.opacityLabel);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(502, 286);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.boatControl);
            this.groupBox15.Controls.Add(this.openUOAM);
            this.groupBox15.Controls.Add(this.openUltimaMapper);
            this.groupBox15.Controls.Add(this.btnMap);
            this.groupBox15.Location = new System.Drawing.Point(6, 232);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(490, 48);
            this.groupBox15.TabIndex = 77;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Maps / Boat";
            // 
            // boatControl
            // 
            this.boatControl.Location = new System.Drawing.Point(388, 16);
            this.boatControl.Name = "boatControl";
            this.boatControl.Size = new System.Drawing.Size(96, 26);
            this.boatControl.TabIndex = 70;
            this.boatControl.Text = "Boat Control";
            this.boatControl.UseVisualStyleBackColor = true;
            this.boatControl.Click += new System.EventHandler(this.boatControl_Click);
            // 
            // openUOAM
            // 
            this.openUOAM.Location = new System.Drawing.Point(238, 16);
            this.openUOAM.Name = "openUOAM";
            this.openUOAM.Size = new System.Drawing.Size(107, 26);
            this.openUOAM.TabIndex = 69;
            this.openUOAM.Text = "UOAM";
            this.openUOAM.Click += new System.EventHandler(this.openUOAM_Click);
            // 
            // openUltimaMapper
            // 
            this.openUltimaMapper.Location = new System.Drawing.Point(122, 16);
            this.openUltimaMapper.Name = "openUltimaMapper";
            this.openUltimaMapper.Size = new System.Drawing.Size(107, 26);
            this.openUltimaMapper.TabIndex = 68;
            this.openUltimaMapper.Text = "Ultima Mapper";
            this.openUltimaMapper.Click += new System.EventHandler(this.openUltimaMapper_Click);
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
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.openBackupFolder);
            this.groupBox16.Controls.Add(this.setBackupFolder);
            this.groupBox16.Controls.Add(this.lastBackup);
            this.groupBox16.Controls.Add(this.createBackup);
            this.groupBox16.Location = new System.Drawing.Point(6, 107);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(229, 119);
            this.groupBox16.TabIndex = 74;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Backup Profiles && Macros";
            // 
            // openBackupFolder
            // 
            this.openBackupFolder.Location = new System.Drawing.Point(119, 81);
            this.openBackupFolder.Name = "openBackupFolder";
            this.openBackupFolder.Size = new System.Drawing.Size(104, 30);
            this.openBackupFolder.TabIndex = 75;
            this.openBackupFolder.Text = "Open Folder";
            this.openBackupFolder.UseVisualStyleBackColor = true;
            this.openBackupFolder.Click += new System.EventHandler(this.openBackupFolder_Click);
            // 
            // setBackupFolder
            // 
            this.setBackupFolder.Location = new System.Drawing.Point(6, 81);
            this.setBackupFolder.Name = "setBackupFolder";
            this.setBackupFolder.Size = new System.Drawing.Size(107, 30);
            this.setBackupFolder.TabIndex = 74;
            this.setBackupFolder.Text = "Set Folder";
            this.setBackupFolder.UseVisualStyleBackColor = true;
            this.setBackupFolder.Click += new System.EventHandler(this.setBackupFolder_Click);
            // 
            // lastBackup
            // 
            this.lastBackup.Location = new System.Drawing.Point(6, 55);
            this.lastBackup.Name = "lastBackup";
            this.lastBackup.Size = new System.Drawing.Size(217, 23);
            this.lastBackup.TabIndex = 73;
            this.lastBackup.Text = "Last Backup: 01/01/2019 5:54PM";
            this.lastBackup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // createBackup
            // 
            this.createBackup.Location = new System.Drawing.Point(61, 22);
            this.createBackup.Name = "createBackup";
            this.createBackup.Size = new System.Drawing.Size(107, 30);
            this.createBackup.TabIndex = 72;
            this.createBackup.Text = "Create Backup";
            this.createBackup.UseVisualStyleBackColor = true;
            this.createBackup.Click += new System.EventHandler(this.createBackup_Click);
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
            this.clientPrio.Location = new System.Drawing.Point(371, 127);
            this.clientPrio.Name = "clientPrio";
            this.clientPrio.Size = new System.Drawing.Size(125, 23);
            this.clientPrio.TabIndex = 73;
            this.clientPrio.SelectedIndexChanged += new System.EventHandler(this.clientPrio_SelectedIndexChanged);
            // 
            // systray
            // 
            this.systray.Location = new System.Drawing.Point(371, 89);
            this.systray.Name = "systray";
            this.systray.Size = new System.Drawing.Size(88, 23);
            this.systray.TabIndex = 69;
            this.systray.Text = "System Tray";
            this.systray.CheckedChanged += new System.EventHandler(this.systray_CheckedChanged);
            // 
            // taskbar
            // 
            this.taskbar.Location = new System.Drawing.Point(301, 89);
            this.taskbar.Name = "taskbar";
            this.taskbar.Size = new System.Drawing.Size(79, 23);
            this.taskbar.TabIndex = 68;
            this.taskbar.Text = "Taskbar";
            this.taskbar.CheckedChanged += new System.EventHandler(this.taskbar_CheckedChanged);
            // 
            // langSel
            // 
            this.langSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.langSel.Location = new System.Drawing.Point(371, 156);
            this.langSel.Name = "langSel";
            this.langSel.Size = new System.Drawing.Size(125, 23);
            this.langSel.TabIndex = 71;
            this.langSel.SelectedIndexChanged += new System.EventHandler(this.langSel_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(241, 159);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 18);
            this.label7.TabIndex = 70;
            this.label7.Text = "Language:";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(241, 93);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 16);
            this.label11.TabIndex = 67;
            this.label11.Text = "Show in:";
            // 
            // showWelcome
            // 
            this.showWelcome.Location = new System.Drawing.Point(244, 28);
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
            this.opacity.Location = new System.Drawing.Point(327, 194);
            this.opacity.Maximum = 100;
            this.opacity.Minimum = 10;
            this.opacity.Name = "opacity";
            this.opacity.Size = new System.Drawing.Size(169, 21);
            this.opacity.TabIndex = 64;
            this.opacity.TickFrequency = 0;
            this.opacity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.opacity.Value = 100;
            this.opacity.Scroll += new System.EventHandler(this.opacity_Scroll);
            // 
            // alwaysTop
            // 
            this.alwaysTop.Location = new System.Drawing.Point(244, 57);
            this.alwaysTop.Name = "alwaysTop";
            this.alwaysTop.Size = new System.Drawing.Size(162, 23);
            this.alwaysTop.TabIndex = 63;
            this.alwaysTop.Text = "Use Smart Always on Top";
            this.alwaysTop.CheckedChanged += new System.EventHandler(this.alwaysTop_CheckedChanged);
            // 
            // opacityLabel
            // 
            this.opacityLabel.Location = new System.Drawing.Point(241, 196);
            this.opacityLabel.Name = "opacityLabel";
            this.opacityLabel.Size = new System.Drawing.Size(89, 19);
            this.opacityLabel.TabIndex = 65;
            this.opacityLabel.Text = "Opacity: 100%";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(241, 130);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(129, 18);
            this.label9.TabIndex = 72;
            this.label9.Text = "Default Client Priority:";
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
            this.groupBox4.Size = new System.Drawing.Size(229, 95);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Profiles";
            // 
            // saveProfile
            // 
            this.saveProfile.Location = new System.Drawing.Point(63, 56);
            this.saveProfile.Name = "saveProfile";
            this.saveProfile.Size = new System.Drawing.Size(50, 30);
            this.saveProfile.TabIndex = 4;
            this.saveProfile.Text = "Save";
            this.saveProfile.Click += new System.EventHandler(this.saveProfile_Click);
            // 
            // cloneProfile
            // 
            this.cloneProfile.Location = new System.Drawing.Point(119, 56);
            this.cloneProfile.Name = "cloneProfile";
            this.cloneProfile.Size = new System.Drawing.Size(50, 30);
            this.cloneProfile.TabIndex = 3;
            this.cloneProfile.Text = "Clone";
            this.cloneProfile.Click += new System.EventHandler(this.cloneProfile_Click);
            // 
            // delProfile
            // 
            this.delProfile.Location = new System.Drawing.Point(175, 56);
            this.delProfile.Name = "delProfile";
            this.delProfile.Size = new System.Drawing.Size(50, 30);
            this.delProfile.TabIndex = 2;
            this.delProfile.Text = "Delete";
            this.delProfile.Click += new System.EventHandler(this.delProfile_Click);
            // 
            // newProfile
            // 
            this.newProfile.Location = new System.Drawing.Point(7, 56);
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
            this.profiles.Size = new System.Drawing.Size(217, 28);
            this.profiles.TabIndex = 0;
            this.profiles.SelectedIndexChanged += new System.EventHandler(this.profiles_SelectedIndexChanged);
            // 
            // subFiltersTab
            // 
            this.subFiltersTab.BackColor = System.Drawing.SystemColors.Control;
            this.subFiltersTab.Controls.Add(this.daemonAnimationList);
            this.subFiltersTab.Controls.Add(this.filterDaemonGraphics);
            this.subFiltersTab.Controls.Add(this.filterOverheadMessages);
            this.subFiltersTab.Controls.Add(this.lblFilterDelaySeconds);
            this.subFiltersTab.Controls.Add(this.lblFilterDelay);
            this.subFiltersTab.Controls.Add(this.filterDelaySeconds);
            this.subFiltersTab.Controls.Add(this.filterRazorMessages);
            this.subFiltersTab.Controls.Add(this.filterSystemMessages);
            this.subFiltersTab.Controls.Add(this.filterSnoop);
            this.subFiltersTab.Controls.Add(this.drakeAnimationList);
            this.subFiltersTab.Controls.Add(this.filterDrakeGraphics);
            this.subFiltersTab.Controls.Add(this.dragonAnimationList);
            this.subFiltersTab.Controls.Add(this.filterDragonGraphics);
            this.subFiltersTab.Controls.Add(this.filters);
            this.subFiltersTab.Location = new System.Drawing.Point(4, 22);
            this.subFiltersTab.Name = "subFiltersTab";
            this.subFiltersTab.Padding = new System.Windows.Forms.Padding(3);
            this.subFiltersTab.Size = new System.Drawing.Size(502, 288);
            this.subFiltersTab.TabIndex = 1;
            this.subFiltersTab.Text = "Filters";
            // 
            // daemonAnimationList
            // 
            this.daemonAnimationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.daemonAnimationList.DropDownWidth = 250;
            this.daemonAnimationList.FormattingEnabled = true;
            this.daemonAnimationList.Location = new System.Drawing.Point(313, 64);
            this.daemonAnimationList.Name = "daemonAnimationList";
            this.daemonAnimationList.Size = new System.Drawing.Size(183, 23);
            this.daemonAnimationList.TabIndex = 113;
            this.daemonAnimationList.SelectedIndexChanged += new System.EventHandler(this.daemonAnimationList_SelectedIndexChanged);
            // 
            // filterDaemonGraphics
            // 
            this.filterDaemonGraphics.AutoSize = true;
            this.filterDaemonGraphics.Location = new System.Drawing.Point(209, 66);
            this.filterDaemonGraphics.Name = "filterDaemonGraphics";
            this.filterDaemonGraphics.Size = new System.Drawing.Size(104, 19);
            this.filterDaemonGraphics.TabIndex = 112;
            this.filterDaemonGraphics.Text = "Filter daemons";
            this.filterDaemonGraphics.UseVisualStyleBackColor = true;
            this.filterDaemonGraphics.CheckedChanged += new System.EventHandler(this.filterDaemonGraphics_CheckedChanged);
            // 
            // filterOverheadMessages
            // 
            this.filterOverheadMessages.Location = new System.Drawing.Point(209, 190);
            this.filterOverheadMessages.Name = "filterOverheadMessages";
            this.filterOverheadMessages.Size = new System.Drawing.Size(220, 20);
            this.filterOverheadMessages.TabIndex = 111;
            this.filterOverheadMessages.Text = "Filter repeating overhead messages";
            this.filterOverheadMessages.CheckedChanged += new System.EventHandler(this.filterOverheadMessages_CheckedChanged);
            // 
            // lblFilterDelaySeconds
            // 
            this.lblFilterDelaySeconds.Location = new System.Drawing.Point(346, 220);
            this.lblFilterDelaySeconds.Name = "lblFilterDelaySeconds";
            this.lblFilterDelaySeconds.Size = new System.Drawing.Size(59, 18);
            this.lblFilterDelaySeconds.TabIndex = 110;
            this.lblFilterDelaySeconds.Text = "seconds";
            // 
            // lblFilterDelay
            // 
            this.lblFilterDelay.AutoSize = true;
            this.lblFilterDelay.Location = new System.Drawing.Point(230, 220);
            this.lblFilterDelay.Name = "lblFilterDelay";
            this.lblFilterDelay.Size = new System.Drawing.Size(68, 15);
            this.lblFilterDelay.TabIndex = 109;
            this.lblFilterDelay.Text = "Filter Delay:";
            // 
            // filterDelaySeconds
            // 
            this.filterDelaySeconds.Location = new System.Drawing.Point(304, 217);
            this.filterDelaySeconds.Name = "filterDelaySeconds";
            this.filterDelaySeconds.Size = new System.Drawing.Size(36, 23);
            this.filterDelaySeconds.TabIndex = 108;
            this.filterDelaySeconds.Text = "3.5";
            this.filterDelaySeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.filterDelaySeconds.TextChanged += new System.EventHandler(this.filterDelaySeconds_TextChanged);
            // 
            // filterRazorMessages
            // 
            this.filterRazorMessages.Location = new System.Drawing.Point(209, 164);
            this.filterRazorMessages.Name = "filterRazorMessages";
            this.filterRazorMessages.Size = new System.Drawing.Size(220, 20);
            this.filterRazorMessages.TabIndex = 107;
            this.filterRazorMessages.Text = "Filter repeating Razor messages";
            this.filterRazorMessages.CheckedChanged += new System.EventHandler(this.filterRazorMessages_CheckedChanged);
            // 
            // filterSystemMessages
            // 
            this.filterSystemMessages.Location = new System.Drawing.Point(209, 138);
            this.filterSystemMessages.Name = "filterSystemMessages";
            this.filterSystemMessages.Size = new System.Drawing.Size(220, 20);
            this.filterSystemMessages.TabIndex = 106;
            this.filterSystemMessages.Text = "Filter repeating system messages";
            this.filterSystemMessages.CheckedChanged += new System.EventHandler(this.filterSystemMessages_CheckedChanged);
            // 
            // filterSnoop
            // 
            this.filterSnoop.Location = new System.Drawing.Point(209, 112);
            this.filterSnoop.Name = "filterSnoop";
            this.filterSnoop.Size = new System.Drawing.Size(220, 20);
            this.filterSnoop.TabIndex = 105;
            this.filterSnoop.Text = "Filter snooping messages";
            this.filterSnoop.CheckedChanged += new System.EventHandler(this.filterSnoop_CheckedChanged);
            // 
            // drakeAnimationList
            // 
            this.drakeAnimationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drakeAnimationList.DropDownWidth = 250;
            this.drakeAnimationList.FormattingEnabled = true;
            this.drakeAnimationList.Location = new System.Drawing.Point(313, 35);
            this.drakeAnimationList.Name = "drakeAnimationList";
            this.drakeAnimationList.Size = new System.Drawing.Size(183, 23);
            this.drakeAnimationList.TabIndex = 104;
            this.drakeAnimationList.SelectedIndexChanged += new System.EventHandler(this.drakeAnimationList_SelectedIndexChanged);
            // 
            // filterDrakeGraphics
            // 
            this.filterDrakeGraphics.AutoSize = true;
            this.filterDrakeGraphics.Location = new System.Drawing.Point(209, 37);
            this.filterDrakeGraphics.Name = "filterDrakeGraphics";
            this.filterDrakeGraphics.Size = new System.Drawing.Size(89, 19);
            this.filterDrakeGraphics.TabIndex = 103;
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
            this.dragonAnimationList.Size = new System.Drawing.Size(183, 23);
            this.dragonAnimationList.TabIndex = 102;
            this.dragonAnimationList.SelectedIndexChanged += new System.EventHandler(this.dragonAnimationList_SelectedIndexChanged);
            // 
            // filterDragonGraphics
            // 
            this.filterDragonGraphics.AutoSize = true;
            this.filterDragonGraphics.Location = new System.Drawing.Point(209, 8);
            this.filterDragonGraphics.Name = "filterDragonGraphics";
            this.filterDragonGraphics.Size = new System.Drawing.Size(98, 19);
            this.filterDragonGraphics.TabIndex = 101;
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
            this.filters.Size = new System.Drawing.Size(197, 274);
            this.filters.TabIndex = 3;
            this.filters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnFilterCheck);
            // 
            // subTargetFilterTab
            // 
            this.subTargetFilterTab.BackColor = System.Drawing.SystemColors.Control;
            this.subTargetFilterTab.Controls.Add(this.lblTargetFilter);
            this.subTargetFilterTab.Controls.Add(this.targetFilterClear);
            this.subTargetFilterTab.Controls.Add(this.targetFilterRemove);
            this.subTargetFilterTab.Controls.Add(this.targetFilterAdd);
            this.subTargetFilterTab.Controls.Add(this.targetFilter);
            this.subTargetFilterTab.Controls.Add(this.targetFilterEnabled);
            this.subTargetFilterTab.Location = new System.Drawing.Point(4, 22);
            this.subTargetFilterTab.Name = "subTargetFilterTab";
            this.subTargetFilterTab.Padding = new System.Windows.Forms.Padding(3);
            this.subTargetFilterTab.Size = new System.Drawing.Size(502, 288);
            this.subTargetFilterTab.TabIndex = 2;
            this.subTargetFilterTab.Text = "Target Filter";
            // 
            // lblTargetFilter
            // 
            this.lblTargetFilter.Location = new System.Drawing.Point(260, 6);
            this.lblTargetFilter.Name = "lblTargetFilter";
            this.lblTargetFilter.Size = new System.Drawing.Size(224, 31);
            this.lblTargetFilter.TabIndex = 12;
            this.lblTargetFilter.Text = "Targets added to this list will be ignored by Razor completely";
            // 
            // targetFilterClear
            // 
            this.targetFilterClear.Location = new System.Drawing.Point(174, 181);
            this.targetFilterClear.Name = "targetFilterClear";
            this.targetFilterClear.Size = new System.Drawing.Size(79, 33);
            this.targetFilterClear.TabIndex = 11;
            this.targetFilterClear.Text = "Clear List";
            this.targetFilterClear.UseVisualStyleBackColor = true;
            this.targetFilterClear.Click += new System.EventHandler(this.TargetFilterClear_Click);
            // 
            // targetFilterRemove
            // 
            this.targetFilterRemove.Location = new System.Drawing.Point(94, 181);
            this.targetFilterRemove.Name = "targetFilterRemove";
            this.targetFilterRemove.Size = new System.Drawing.Size(74, 33);
            this.targetFilterRemove.TabIndex = 10;
            this.targetFilterRemove.Text = "Remove";
            this.targetFilterRemove.UseVisualStyleBackColor = true;
            this.targetFilterRemove.Click += new System.EventHandler(this.TargetFilterRemove_Click);
            // 
            // targetFilterAdd
            // 
            this.targetFilterAdd.Location = new System.Drawing.Point(6, 181);
            this.targetFilterAdd.Name = "targetFilterAdd";
            this.targetFilterAdd.Size = new System.Drawing.Size(82, 33);
            this.targetFilterAdd.TabIndex = 9;
            this.targetFilterAdd.Text = "Add (Target)";
            this.targetFilterAdd.UseVisualStyleBackColor = true;
            this.targetFilterAdd.Click += new System.EventHandler(this.TargetFilterAdd_Click);
            // 
            // targetFilter
            // 
            this.targetFilter.FormattingEnabled = true;
            this.targetFilter.ItemHeight = 15;
            this.targetFilter.Location = new System.Drawing.Point(6, 6);
            this.targetFilter.Name = "targetFilter";
            this.targetFilter.Size = new System.Drawing.Size(247, 169);
            this.targetFilter.TabIndex = 2;
            // 
            // targetFilterEnabled
            // 
            this.targetFilterEnabled.AutoSize = true;
            this.targetFilterEnabled.Location = new System.Drawing.Point(263, 50);
            this.targetFilterEnabled.Name = "targetFilterEnabled";
            this.targetFilterEnabled.Size = new System.Drawing.Size(133, 19);
            this.targetFilterEnabled.TabIndex = 0;
            this.targetFilterEnabled.Text = "Target Filter Enabled";
            this.targetFilterEnabled.UseVisualStyleBackColor = true;
            this.targetFilterEnabled.CheckedChanged += new System.EventHandler(this.TargetFilterEnabled_CheckedChanged);
            // 
            // subSoundMusicTab
            // 
            this.subSoundMusicTab.BackColor = System.Drawing.SystemColors.Control;
            this.subSoundMusicTab.Controls.Add(this.playableMusicList);
            this.subSoundMusicTab.Controls.Add(this.playMusic);
            this.subSoundMusicTab.Controls.Add(this.showPlayingMusic);
            this.subSoundMusicTab.Controls.Add(this.showPlayingSoundInfo);
            this.subSoundMusicTab.Controls.Add(this.showFilteredSound);
            this.subSoundMusicTab.Controls.Add(this.playInClient);
            this.subSoundMusicTab.Controls.Add(this.playSound);
            this.subSoundMusicTab.Controls.Add(this.soundFilterEnabled);
            this.subSoundMusicTab.Controls.Add(this.soundFilterList);
            this.subSoundMusicTab.Location = new System.Drawing.Point(4, 22);
            this.subSoundMusicTab.Name = "subSoundMusicTab";
            this.subSoundMusicTab.Size = new System.Drawing.Size(502, 288);
            this.subSoundMusicTab.TabIndex = 3;
            this.subSoundMusicTab.Text = "Sound & Music  ";
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
            this.optionsTabCtrl.Controls.Add(this.subOptionsFriendsTab);
            this.optionsTabCtrl.Location = new System.Drawing.Point(6, 3);
            this.optionsTabCtrl.Name = "optionsTabCtrl";
            this.optionsTabCtrl.SelectedIndex = 0;
            this.optionsTabCtrl.Size = new System.Drawing.Size(510, 314);
            this.optionsTabCtrl.TabIndex = 93;
            // 
            // subOptionsSpeechTab
            // 
            this.subOptionsSpeechTab.BackColor = System.Drawing.SystemColors.Control;
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
            this.subOptionsSpeechTab.Controls.Add(this.overHeadMessages);
            this.subOptionsSpeechTab.Controls.Add(this.showOverheadMessages);
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
            // buffDebuffOptions
            // 
            this.buffDebuffOptions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buffDebuffOptions.Location = new System.Drawing.Point(208, 236);
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
            this.damageTakenOverhead.Location = new System.Drawing.Point(394, 209);
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
            this.showDamageTaken.Location = new System.Drawing.Point(260, 209);
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
            this.damageDealtOverhead.Location = new System.Drawing.Point(394, 184);
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
            this.showDamageDealt.Location = new System.Drawing.Point(260, 184);
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
            this.showBuffDebuffOverhead.Location = new System.Drawing.Point(9, 237);
            this.showBuffDebuffOverhead.Name = "showBuffDebuffOverhead";
            this.showBuffDebuffOverhead.Size = new System.Drawing.Size(172, 19);
            this.showBuffDebuffOverhead.TabIndex = 91;
            this.showBuffDebuffOverhead.Text = "Show buff/debuff overhead";
            this.showBuffDebuffOverhead.UseVisualStyleBackColor = true;
            this.showBuffDebuffOverhead.CheckedChanged += new System.EventHandler(this.showBuffDebuffOverhead_CheckedChanged);
            // 
            // healthFmt
            // 
            this.healthFmt.Location = new System.Drawing.Point(377, 134);
            this.healthFmt.Name = "healthFmt";
            this.healthFmt.Size = new System.Drawing.Size(53, 23);
            this.healthFmt.TabIndex = 89;
            this.healthFmt.Text = "[{0}%]";
            this.healthFmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.healthFmt.TextChanged += new System.EventHandler(this.healthFmt_TextChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(283, 135);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 18);
            this.label10.TabIndex = 88;
            this.label10.Text = "Health Format:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // showHealthOH
            // 
            this.showHealthOH.Location = new System.Drawing.Point(260, 112);
            this.showHealthOH.Name = "showHealthOH";
            this.showHealthOH.Size = new System.Drawing.Size(231, 20);
            this.showHealthOH.TabIndex = 87;
            this.showHealthOH.Text = "Show health above people/creatures";
            this.showHealthOH.CheckedChanged += new System.EventHandler(this.showHealthOH_CheckedChanged);
            // 
            // chkPartyOverhead
            // 
            this.chkPartyOverhead.Location = new System.Drawing.Point(260, 158);
            this.chkPartyOverhead.Name = "chkPartyOverhead";
            this.chkPartyOverhead.Size = new System.Drawing.Size(238, 20);
            this.chkPartyOverhead.TabIndex = 90;
            this.chkPartyOverhead.Text = "Show mana/stam above party members";
            this.chkPartyOverhead.CheckedChanged += new System.EventHandler(this.chkPartyOverhead_CheckedChanged);
            // 
            // containerLabels
            // 
            this.containerLabels.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.containerLabels.Location = new System.Drawing.Point(438, 87);
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
            this.showContainerLabels.Location = new System.Drawing.Point(260, 87);
            this.showContainerLabels.Name = "showContainerLabels";
            this.showContainerLabels.Size = new System.Drawing.Size(141, 19);
            this.showContainerLabels.TabIndex = 85;
            this.showContainerLabels.Text = "Show container labels";
            this.showContainerLabels.UseVisualStyleBackColor = true;
            this.showContainerLabels.CheckedChanged += new System.EventHandler(this.showContainerLabels_CheckedChanged);
            // 
            // overHeadMessages
            // 
            this.overHeadMessages.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overHeadMessages.Location = new System.Drawing.Point(438, 62);
            this.overHeadMessages.Name = "overHeadMessages";
            this.overHeadMessages.Size = new System.Drawing.Size(33, 19);
            this.overHeadMessages.TabIndex = 84;
            this.overHeadMessages.Text = "...";
            this.overHeadMessages.UseVisualStyleBackColor = true;
            this.overHeadMessages.Click += new System.EventHandler(this.overHeadMessages_Click);
            // 
            // showOverheadMessages
            // 
            this.showOverheadMessages.AutoSize = true;
            this.showOverheadMessages.Location = new System.Drawing.Point(260, 62);
            this.showOverheadMessages.Name = "showOverheadMessages";
            this.showOverheadMessages.Size = new System.Drawing.Size(161, 19);
            this.showOverheadMessages.TabIndex = 83;
            this.showOverheadMessages.Text = "Show overhead messages";
            this.showOverheadMessages.UseVisualStyleBackColor = true;
            this.showOverheadMessages.CheckedChanged += new System.EventHandler(this.showMessagesOverhead_CheckedChanged);
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
            this.txtSpellFormat.Location = new System.Drawing.Point(89, 208);
            this.txtSpellFormat.Name = "txtSpellFormat";
            this.txtSpellFormat.Size = new System.Drawing.Size(152, 23);
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
            this.label3.Location = new System.Drawing.Point(9, 208);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 20);
            this.label3.TabIndex = 54;
            this.label3.Text = "Spell Format:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // subOptionsTargetTab
            // 
            this.subOptionsTargetTab.BackColor = System.Drawing.SystemColors.Control;
            this.subOptionsTargetTab.Controls.Add(this.groupSmartTarget);
            this.subOptionsTargetTab.Controls.Add(this.setTargetIndicatorHue);
            this.subOptionsTargetTab.Controls.Add(this.targetIndictorFormat);
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
            this.friendBeneficialOnly.Size = new System.Drawing.Size(232, 19);
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
            this.onlyNextPrevBeneficial.Size = new System.Drawing.Size(223, 19);
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
            // targetIndictorFormat
            // 
            this.targetIndictorFormat.Location = new System.Drawing.Point(64, 246);
            this.targetIndictorFormat.Name = "targetIndictorFormat";
            this.targetIndictorFormat.Size = new System.Drawing.Size(107, 23);
            this.targetIndictorFormat.TabIndex = 93;
            this.targetIndictorFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.targetIndictorFormat.TextChanged += new System.EventHandler(this.targetIndictorFormat_TextChanged);
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
            this.showTargetMessagesOverChar.Size = new System.Drawing.Size(225, 19);
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
            this.potionEquip.Text = "Auto Un/Re-equip hands for potions";
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
            // subOptionsFriendsTab
            // 
            this.subOptionsFriendsTab.BackColor = System.Drawing.SystemColors.Control;
            this.subOptionsFriendsTab.Controls.Add(this.highlightFriend);
            this.subOptionsFriendsTab.Controls.Add(this.setFriendsFormatHue);
            this.subOptionsFriendsTab.Controls.Add(this.friendOverheadFormat);
            this.subOptionsFriendsTab.Controls.Add(this.showFriendOverhead);
            this.subOptionsFriendsTab.Controls.Add(this.autoAcceptParty);
            this.subOptionsFriendsTab.Controls.Add(this.nextPrevIgnoresFriends);
            this.subOptionsFriendsTab.Controls.Add(this.autoFriend);
            this.subOptionsFriendsTab.Controls.Add(this.friendsGroupBox);
            this.subOptionsFriendsTab.Controls.Add(this.friendFormat);
            this.subOptionsFriendsTab.Location = new System.Drawing.Point(4, 24);
            this.subOptionsFriendsTab.Name = "subOptionsFriendsTab";
            this.subOptionsFriendsTab.Size = new System.Drawing.Size(502, 286);
            this.subOptionsFriendsTab.TabIndex = 3;
            this.subOptionsFriendsTab.Text = "Friends";
            // 
            // setFriendsFormatHue
            // 
            this.setFriendsFormatHue.Location = new System.Drawing.Point(438, 115);
            this.setFriendsFormatHue.Name = "setFriendsFormatHue";
            this.setFriendsFormatHue.Size = new System.Drawing.Size(59, 24);
            this.setFriendsFormatHue.TabIndex = 133;
            this.setFriendsFormatHue.Text = "Set Hue";
            this.setFriendsFormatHue.UseVisualStyleBackColor = true;
            this.setFriendsFormatHue.Click += new System.EventHandler(this.setFriendsFormatHue_Click);
            // 
            // friendOverheadFormat
            // 
            this.friendOverheadFormat.Location = new System.Drawing.Point(330, 116);
            this.friendOverheadFormat.Name = "friendOverheadFormat";
            this.friendOverheadFormat.Size = new System.Drawing.Size(102, 23);
            this.friendOverheadFormat.TabIndex = 131;
            this.friendOverheadFormat.Text = "[Friend]";
            this.friendOverheadFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.friendOverheadFormat.TextChanged += new System.EventHandler(this.friendOverheadFormat_TextChanged);
            // 
            // showFriendOverhead
            // 
            this.showFriendOverhead.Location = new System.Drawing.Point(267, 90);
            this.showFriendOverhead.Name = "showFriendOverhead";
            this.showFriendOverhead.Size = new System.Drawing.Size(184, 23);
            this.showFriendOverhead.TabIndex = 130;
            this.showFriendOverhead.Text = "Show [Friend] overhead";
            this.showFriendOverhead.UseVisualStyleBackColor = true;
            this.showFriendOverhead.CheckedChanged += new System.EventHandler(this.showFriendOverhead_CheckedChanged);
            // 
            // autoAcceptParty
            // 
            this.autoAcceptParty.Location = new System.Drawing.Point(267, 64);
            this.autoAcceptParty.Name = "autoAcceptParty";
            this.autoAcceptParty.Size = new System.Drawing.Size(230, 20);
            this.autoAcceptParty.TabIndex = 119;
            this.autoAcceptParty.Text = "Auto-accept party invites from friends";
            this.autoAcceptParty.CheckedChanged += new System.EventHandler(this.autoAcceptParty_CheckedChanged);
            // 
            // nextPrevIgnoresFriends
            // 
            this.nextPrevIgnoresFriends.AutoSize = true;
            this.nextPrevIgnoresFriends.Location = new System.Drawing.Point(267, 39);
            this.nextPrevIgnoresFriends.Name = "nextPrevIgnoresFriends";
            this.nextPrevIgnoresFriends.Size = new System.Drawing.Size(203, 19);
            this.nextPrevIgnoresFriends.TabIndex = 97;
            this.nextPrevIgnoresFriends.Text = "Next/Prev Target ignores \'Friends\'";
            this.nextPrevIgnoresFriends.UseVisualStyleBackColor = true;
            this.nextPrevIgnoresFriends.CheckedChanged += new System.EventHandler(this.nextPrevIgnoresFriends_CheckedChanged);
            // 
            // autoFriend
            // 
            this.autoFriend.Location = new System.Drawing.Point(267, 13);
            this.autoFriend.Name = "autoFriend";
            this.autoFriend.Size = new System.Drawing.Size(217, 20);
            this.autoFriend.TabIndex = 96;
            this.autoFriend.Text = "Treat party members as \'Friends\'";
            this.autoFriend.CheckedChanged += new System.EventHandler(this.autoFriend_CheckedChanged);
            // 
            // friendsGroupBox
            // 
            this.friendsGroupBox.Controls.Add(this.friendsGroupRemove);
            this.friendsGroupBox.Controls.Add(this.friendsGroupAdd);
            this.friendsGroupBox.Controls.Add(this.friendClearList);
            this.friendsGroupBox.Controls.Add(this.friendRemoveSelected);
            this.friendsGroupBox.Controls.Add(this.friendAddTarget);
            this.friendsGroupBox.Controls.Add(this.friendsList);
            this.friendsGroupBox.Controls.Add(this.friendsGroup);
            this.friendsGroupBox.Controls.Add(this.friendsListEnabled);
            this.friendsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.friendsGroupBox.Name = "friendsGroupBox";
            this.friendsGroupBox.Size = new System.Drawing.Size(258, 280);
            this.friendsGroupBox.TabIndex = 0;
            this.friendsGroupBox.TabStop = false;
            this.friendsGroupBox.Text = "Friends Groups && Lists";
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
            this.friendClearList.Location = new System.Drawing.Point(174, 241);
            this.friendClearList.Name = "friendClearList";
            this.friendClearList.Size = new System.Drawing.Size(79, 33);
            this.friendClearList.TabIndex = 8;
            this.friendClearList.Text = "Clear List";
            this.friendClearList.UseVisualStyleBackColor = true;
            this.friendClearList.Click += new System.EventHandler(this.friendClearList_Click);
            // 
            // friendRemoveSelected
            // 
            this.friendRemoveSelected.Location = new System.Drawing.Point(94, 241);
            this.friendRemoveSelected.Name = "friendRemoveSelected";
            this.friendRemoveSelected.Size = new System.Drawing.Size(74, 33);
            this.friendRemoveSelected.TabIndex = 7;
            this.friendRemoveSelected.Text = "Remove";
            this.friendRemoveSelected.UseVisualStyleBackColor = true;
            this.friendRemoveSelected.Click += new System.EventHandler(this.friendRemoveSelected_Click);
            // 
            // friendAddTarget
            // 
            this.friendAddTarget.Location = new System.Drawing.Point(6, 241);
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
            this.friendsList.Location = new System.Drawing.Point(6, 51);
            this.friendsList.Name = "friendsList";
            this.friendsList.Size = new System.Drawing.Size(247, 184);
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
            // friendFormat
            // 
            this.friendFormat.Location = new System.Drawing.Point(276, 116);
            this.friendFormat.Name = "friendFormat";
            this.friendFormat.Size = new System.Drawing.Size(145, 23);
            this.friendFormat.TabIndex = 132;
            this.friendFormat.Text = "Format:";
            this.friendFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.displayCountersTabCtrl.Location = new System.Drawing.Point(6, 3);
            this.displayCountersTabCtrl.Name = "displayCountersTabCtrl";
            this.displayCountersTabCtrl.SelectedIndex = 0;
            this.displayCountersTabCtrl.Size = new System.Drawing.Size(510, 314);
            this.displayCountersTabCtrl.TabIndex = 51;
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
            this.removeDress.Location = new System.Drawing.Point(121, 156);
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
            this.dressList.Size = new System.Drawing.Size(175, 132);
            this.dressList.TabIndex = 3;
            this.dressList.SelectedIndexChanged += new System.EventHandler(this.dressList_SelectedIndexChanged);
            // 
            // undressConflicts
            // 
            this.undressConflicts.Location = new System.Drawing.Point(6, 195);
            this.undressConflicts.Name = "undressConflicts";
            this.undressConflicts.Size = new System.Drawing.Size(175, 38);
            this.undressConflicts.TabIndex = 6;
            this.undressConflicts.Text = "Automatically move conflicting items";
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
            this.agentSubList.Location = new System.Drawing.Point(6, 15);
            this.agentSubList.Name = "agentSubList";
            this.agentSubList.Size = new System.Drawing.Size(356, 288);
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
            this.rangeCheckDoubleClick.Size = new System.Drawing.Size(208, 19);
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
            this.rangeCheckTargetByType.Size = new System.Drawing.Size(190, 19);
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
            this.stepThroughMacro.Size = new System.Drawing.Size(135, 19);
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
            this.targetByTypeDifferent.Size = new System.Drawing.Size(183, 19);
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
            this.subTabScripts.Controls.Add(this.tabPage3);
            this.subTabScripts.Location = new System.Drawing.Point(6, 3);
            this.subTabScripts.Name = "subTabScripts";
            this.subTabScripts.SelectedIndex = 0;
            this.subTabScripts.Size = new System.Drawing.Size(506, 313);
            this.subTabScripts.TabIndex = 14;
            // 
            // subScripts
            // 
            this.subScripts.BackColor = System.Drawing.SystemColors.Control;
            this.subScripts.Controls.Add(this.renameScript);
            this.subScripts.Controls.Add(this.linkScriptGuide);
            this.subScripts.Controls.Add(this.saveScript);
            this.subScripts.Controls.Add(this.deleteScript);
            this.subScripts.Controls.Add(this.scriptEditor);
            this.subScripts.Controls.Add(this.scriptList);
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
            // renameScript
            // 
            this.renameScript.Location = new System.Drawing.Point(432, 195);
            this.renameScript.Name = "renameScript";
            this.renameScript.Size = new System.Drawing.Size(60, 26);
            this.renameScript.TabIndex = 24;
            this.renameScript.Text = "Rename";
            this.renameScript.UseVisualStyleBackColor = true;
            this.renameScript.Click += new System.EventHandler(this.renameScript_Click);
            // 
            // linkScriptGuide
            // 
            this.linkScriptGuide.Location = new System.Drawing.Point(6, 254);
            this.linkScriptGuide.Name = "linkScriptGuide";
            this.linkScriptGuide.Size = new System.Drawing.Size(114, 25);
            this.linkScriptGuide.TabIndex = 23;
            this.linkScriptGuide.TabStop = true;
            this.linkScriptGuide.Text = "Scripting Guide";
            this.linkScriptGuide.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkScriptGuide.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkScriptGuide_LinkClicked);
            // 
            // saveScript
            // 
            this.saveScript.Location = new System.Drawing.Point(432, 163);
            this.saveScript.Name = "saveScript";
            this.saveScript.Size = new System.Drawing.Size(60, 26);
            this.saveScript.TabIndex = 22;
            this.saveScript.Text = "Save";
            this.saveScript.UseVisualStyleBackColor = true;
            this.saveScript.Click += new System.EventHandler(this.saveScript_Click);
            // 
            // deleteScript
            // 
            this.deleteScript.Location = new System.Drawing.Point(432, 227);
            this.deleteScript.Name = "deleteScript";
            this.deleteScript.Size = new System.Drawing.Size(60, 26);
            this.deleteScript.TabIndex = 21;
            this.deleteScript.Text = "Delete";
            this.deleteScript.UseVisualStyleBackColor = true;
            this.deleteScript.Click += new System.EventHandler(this.deleteScript_Click);
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
            this.scriptEditor.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.scriptEditor.ForeColor = System.Drawing.Color.White;
            this.scriptEditor.IsReplaceMode = false;
            this.scriptEditor.LeftBracket = '(';
            this.scriptEditor.LeftBracket2 = '[';
            this.scriptEditor.LineNumberColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(144)))), ((int)(((byte)(175)))));
            this.scriptEditor.Location = new System.Drawing.Point(123, 6);
            this.scriptEditor.Name = "scriptEditor";
            this.scriptEditor.Paddings = new System.Windows.Forms.Padding(0);
            this.scriptEditor.RightBracket = ')';
            this.scriptEditor.RightBracket2 = ']';
            this.scriptEditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.scriptEditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("scriptEditor.ServiceColors")));
            this.scriptEditor.Size = new System.Drawing.Size(303, 273);
            this.scriptEditor.TabIndex = 20;
            this.scriptEditor.Zoom = 100;
            this.scriptEditor.LostFocus += new System.EventHandler(this.scriptEditor_LostFocus);
            this.scriptEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scriptEditor_MouseDown);
            // 
            // scriptList
            // 
            this.scriptList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scriptList.FormattingEnabled = true;
            this.scriptList.ItemHeight = 17;
            this.scriptList.Location = new System.Drawing.Point(6, 7);
            this.scriptList.Name = "scriptList";
            this.scriptList.Size = new System.Drawing.Size(114, 242);
            this.scriptList.TabIndex = 19;
            this.scriptList.SelectedIndexChanged += new System.EventHandler(this.scriptList_SelectedIndexChanged);
            this.scriptList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scriptList_MouseDown);
            // 
            // newScript
            // 
            this.newScript.Location = new System.Drawing.Point(432, 131);
            this.newScript.Name = "newScript";
            this.newScript.Size = new System.Drawing.Size(60, 26);
            this.newScript.TabIndex = 18;
            this.newScript.Text = "New";
            this.newScript.UseVisualStyleBackColor = true;
            this.newScript.Click += new System.EventHandler(this.newScript_Click);
            // 
            // setScriptHotkey
            // 
            this.setScriptHotkey.Location = new System.Drawing.Point(432, 83);
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
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.autoSaveScriptPlay);
            this.tabPage3.Controls.Add(this.autoSaveScript);
            this.tabPage3.Controls.Add(this.scriptVariablesBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(498, 287);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Options";
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
            // screenshotTab
            // 
            this.screenshotTab.Controls.Add(this.captureOwnDeathDelay);
            this.screenshotTab.Controls.Add(this.lblOwnDeathMs);
            this.screenshotTab.Controls.Add(this.captureOwnDeath);
            this.screenshotTab.Controls.Add(this.captureOthersDeathDelay);
            this.screenshotTab.Controls.Add(this.lblCaptureOthersDeathMs);
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
            // captureOwnDeathDelay
            // 
            this.captureOwnDeathDelay.Location = new System.Drawing.Point(196, 235);
            this.captureOwnDeathDelay.Name = "captureOwnDeathDelay";
            this.captureOwnDeathDelay.Size = new System.Drawing.Size(32, 23);
            this.captureOwnDeathDelay.TabIndex = 61;
            this.captureOwnDeathDelay.Text = "0.5";
            this.captureOwnDeathDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.captureOwnDeathDelay.TextChanged += new System.EventHandler(this.CaptureOwnDeathDelay_TextChanged);
            // 
            // lblOwnDeathMs
            // 
            this.lblOwnDeathMs.Location = new System.Drawing.Point(230, 239);
            this.lblOwnDeathMs.Name = "lblOwnDeathMs";
            this.lblOwnDeathMs.Size = new System.Drawing.Size(26, 18);
            this.lblOwnDeathMs.TabIndex = 60;
            this.lblOwnDeathMs.Text = "s";
            // 
            // captureOwnDeath
            // 
            this.captureOwnDeath.Location = new System.Drawing.Point(12, 238);
            this.captureOwnDeath.Name = "captureOwnDeath";
            this.captureOwnDeath.Size = new System.Drawing.Size(158, 20);
            this.captureOwnDeath.TabIndex = 59;
            this.captureOwnDeath.Text = "Capture your own death";
            this.captureOwnDeath.CheckedChanged += new System.EventHandler(this.CaptureOwnDeath_CheckedChanged);
            // 
            // captureOthersDeathDelay
            // 
            this.captureOthersDeathDelay.Location = new System.Drawing.Point(196, 211);
            this.captureOthersDeathDelay.Name = "captureOthersDeathDelay";
            this.captureOthersDeathDelay.Size = new System.Drawing.Size(32, 23);
            this.captureOthersDeathDelay.TabIndex = 58;
            this.captureOthersDeathDelay.Text = "0.5";
            this.captureOthersDeathDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.captureOthersDeathDelay.TextChanged += new System.EventHandler(this.CaptureOthersDeathDelay_TextChanged);
            // 
            // lblCaptureOthersDeathMs
            // 
            this.lblCaptureOthersDeathMs.Location = new System.Drawing.Point(230, 214);
            this.lblCaptureOthersDeathMs.Name = "lblCaptureOthersDeathMs";
            this.lblCaptureOthersDeathMs.Size = new System.Drawing.Size(26, 18);
            this.lblCaptureOthersDeathMs.TabIndex = 57;
            this.lblCaptureOthersDeathMs.Text = "s";
            // 
            // imgurUploads
            // 
            this.imgurUploads.FormattingEnabled = true;
            this.imgurUploads.ItemHeight = 15;
            this.imgurUploads.Location = new System.Drawing.Point(266, 203);
            this.imgurUploads.Name = "imgurUploads";
            this.imgurUploads.Size = new System.Drawing.Size(246, 109);
            this.imgurUploads.TabIndex = 15;
            this.imgurUploads.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgurUploads_MouseDown);
            // 
            // screenShotClipboard
            // 
            this.screenShotClipboard.Location = new System.Drawing.Point(12, 264);
            this.screenShotClipboard.Name = "screenShotClipboard";
            this.screenShotClipboard.Size = new System.Drawing.Size(239, 20);
            this.screenShotClipboard.TabIndex = 14;
            this.screenShotClipboard.Text = "Copy link to clipboard after upload";
            this.screenShotClipboard.CheckedChanged += new System.EventHandler(this.screenShotClipboard_CheckedChanged);
            // 
            // screenShotNotification
            // 
            this.screenShotNotification.Location = new System.Drawing.Point(266, 173);
            this.screenShotNotification.Name = "screenShotNotification";
            this.screenShotNotification.Size = new System.Drawing.Size(216, 20);
            this.screenShotNotification.TabIndex = 13;
            this.screenShotNotification.Text = "Show upload notification";
            this.screenShotNotification.CheckedChanged += new System.EventHandler(this.screenShotNotification_CheckedChanged);
            // 
            // screenShotOpenBrowser
            // 
            this.screenShotOpenBrowser.Location = new System.Drawing.Point(12, 290);
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
            this.capNow.Location = new System.Drawing.Point(371, 9);
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
            this.screenPath.Size = new System.Drawing.Size(316, 23);
            this.screenPath.TabIndex = 7;
            this.screenPath.TextChanged += new System.EventHandler(this.screenPath_TextChanged);
            // 
            // radioUO
            // 
            this.radioUO.Location = new System.Drawing.Point(12, 157);
            this.radioUO.Name = "radioUO";
            this.radioUO.Size = new System.Drawing.Size(75, 26);
            this.radioUO.TabIndex = 6;
            this.radioUO.Text = "UO Only";
            this.radioUO.CheckedChanged += new System.EventHandler(this.radioUO_CheckedChanged);
            // 
            // radioFull
            // 
            this.radioFull.Location = new System.Drawing.Point(102, 157);
            this.radioFull.Name = "radioFull";
            this.radioFull.Size = new System.Drawing.Size(88, 26);
            this.radioFull.TabIndex = 5;
            this.radioFull.Text = "Full Screen";
            this.radioFull.CheckedChanged += new System.EventHandler(this.radioFull_CheckedChanged);
            // 
            // captureOthersDeath
            // 
            this.captureOthersDeath.Location = new System.Drawing.Point(12, 212);
            this.captureOthersDeath.Name = "captureOthersDeath";
            this.captureOthersDeath.Size = new System.Drawing.Size(216, 20);
            this.captureOthersDeath.TabIndex = 4;
            this.captureOthersDeath.Text = "Capture death of other players";
            this.captureOthersDeath.CheckedChanged += new System.EventHandler(this.CaptureOthersDeath_CheckedChanged);
            // 
            // setScnPath
            // 
            this.setScnPath.Location = new System.Drawing.Point(330, 9);
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
            this.screensList.Size = new System.Drawing.Size(252, 92);
            this.screensList.Sorted = true;
            this.screensList.TabIndex = 1;
            this.screensList.SelectedIndexChanged += new System.EventHandler(this.screensList_SelectedIndexChanged);
            this.screensList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.screensList_MouseDown);
            // 
            // screenPrev
            // 
            this.screenPrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.screenPrev.Location = new System.Drawing.Point(266, 36);
            this.screenPrev.Name = "screenPrev";
            this.screenPrev.Size = new System.Drawing.Size(246, 131);
            this.screenPrev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.screenPrev.TabIndex = 0;
            this.screenPrev.TabStop = false;
            this.screenPrev.Click += new System.EventHandler(this.screenPrev_Click);
            // 
            // dispTime
            // 
            this.dispTime.Location = new System.Drawing.Point(12, 186);
            this.dispTime.Name = "dispTime";
            this.dispTime.Size = new System.Drawing.Size(206, 20);
            this.dispTime.TabIndex = 9;
            this.dispTime.Text = "Include Timestamp on images";
            this.dispTime.CheckedChanged += new System.EventHandler(this.dispTime_CheckedChanged);
            // 
            // advancedTab
            // 
            this.advancedTab.BackColor = System.Drawing.SystemColors.Control;
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
            this.statusBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox.HideSelection = false;
            this.statusBox.Location = new System.Drawing.Point(10, 8);
            this.statusBox.Multiline = true;
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            this.statusBox.Size = new System.Drawing.Size(225, 255);
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
            this.features.Size = new System.Drawing.Size(271, 79);
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
            this.aboutTab.Controls.Add(this.linkLabel1);
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
            this.linkGitHub.Location = new System.Drawing.Point(6, 163);
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
            this.label20.Location = new System.Drawing.Point(93, 126);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(343, 17);
            this.label20.TabIndex = 21;
            this.label20.Text = "For feedback, support and the latest releases please visit:\r\n";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(6, 94);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(506, 20);
            this.linkLabel1.TabIndex = 20;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.uorenaissance.com";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
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
            this.aboutSubInfo.Text = "UO Renaissance Community Edition";
            this.aboutSubInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkMain
            // 
            this.linkMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkMain.Location = new System.Drawing.Point(6, 143);
            this.linkMain.Name = "linkMain";
            this.linkMain.Size = new System.Drawing.Size(506, 20);
            this.linkMain.TabIndex = 16;
            this.linkMain.TabStop = true;
            this.linkMain.Text = "http://www.uor-razor.com";
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
            // highlightFriend
            // 
            this.highlightFriend.Location = new System.Drawing.Point(267, 142);
            this.highlightFriend.Name = "highlightFriend";
            this.highlightFriend.Size = new System.Drawing.Size(184, 23);
            this.highlightFriend.TabIndex = 134;
            this.highlightFriend.Text = "Next/Prev highlights \'Friends\'";
            this.highlightFriend.UseVisualStyleBackColor = true;
            this.highlightFriend.CheckedChanged += new System.EventHandler(this.highlightFriend_CheckedChanged);
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
            this.tabPage1.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.subFiltersTab.ResumeLayout(false);
            this.subFiltersTab.PerformLayout();
            this.subTargetFilterTab.ResumeLayout(false);
            this.subTargetFilterTab.PerformLayout();
            this.subSoundMusicTab.ResumeLayout(false);
            this.subSoundMusicTab.PerformLayout();
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
            this.subOptionsFriendsTab.ResumeLayout(false);
            this.subOptionsFriendsTab.PerformLayout();
            this.friendsGroupBox.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.scriptEditor)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.scriptVariablesBox.ResumeLayout(false);
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

        private TabPage scriptsTab;
        private TabControl subTabScripts;
        private TabPage subScripts;
        private Button saveScript;
        private Button deleteScript;
        private FastColoredTextBoxNS.FastColoredTextBox scriptEditor;
        private ListBox scriptList;
        private Button newScript;
        private Button setScriptHotkey;
        private Button recordScript;
        private Button playScript;
        private TabPage tabPage3;
        private GroupBox scriptVariablesBox;
        private Button changeScriptVariable;
        private Button removeScriptVariable;
        private Button addScriptVariable;
        private ListBox scriptVariables;
        private LinkLabel linkScriptGuide;
        private CheckBox autoSaveScript;
        private CheckBox autoSaveScriptPlay;
        private Button renameScript;
        private CheckBox highlightFriend;
    }
}
