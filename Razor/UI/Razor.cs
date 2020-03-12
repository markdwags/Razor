#region license

// Razor: An Ultima Online Assistant
// Copyright (C) 2020 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using Assistant.Filters;
using Assistant.Macros;
using System.Diagnostics;
using Assistant.Boat;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Assistant.Agents;
using Assistant.Core;
using Assistant.Scripts;
using Assistant.UI;
using Ultima;
using ContainerLabels = Assistant.UI.ContainerLabels;
using Exception = System.Exception;
using OverheadMessages = Assistant.UI.OverheadMessages;

namespace Assistant
{
    public partial class MainForm
    {
        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == Client.WM_UONETEVENT)
                msg.Result =
                    (IntPtr) (Client.Instance.OnMessage(this, (uint) msg.WParam.ToInt32(), msg.LParam.ToInt32())
                        ? 1
                        : 0);
            else if (msg.Msg == Client.WM_COPYDATA)
                msg.Result = (IntPtr) (Client.Instance.OnCopyData(msg.WParam, msg.LParam) ? 1 : 0);
            else if (Config.GetBool("EnableUOAAPI") && msg.Msg >= (int) UOAssist.UOAMessage.First &&
                     msg.Msg <= (int) UOAssist.UOAMessage.Last)
                msg.Result = (IntPtr) UOAssist.OnUOAMessage(this, msg.Msg, msg.WParam.ToInt32(), msg.LParam.ToInt32());
            else
                base.WndProc(ref msg);
        }

        private void DisableCloseButton()
        {
            Platform.DisableCloseButton(this.Handle);
            m_CanClose = false;
        }

        private void MainForm_StartLoad(object sender, System.EventArgs e)
        {
            Hide();
            new StatsTimer(this).Start();
            Language.LoadControlNames(this);

            FriendsManager.SetControls(friendsGroup, friendsList);
            DressList.SetControls(dressList, dressItems);
            TargetFilterManager.SetControls(targetFilter);
            SoundMusicManager.SetControls(soundFilterList, playableMusicList);
            ScriptManager.SetControls(scriptEditor, scriptList);

            bool st = Config.GetBool("Systray");
            taskbar.Checked = this.ShowInTaskbar = !st;
            systray.Checked = m_NotifyIcon.Visible = st;

            UpdateTitle();

            Engine.ActiveWindow = this;

            DisableCloseButton();

            if (!Client.Instance.InstallHooks(this.Handle))
            {
                m_CanClose = true;
                SplashScreen.End();
                this.Close();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        public void MainForm_EndLoad()
        {
            Ultima.Multis.PostHSFormat = Engine.UsePostHSChanges;

            PacketsTable.AdjustPacketSizeByVersion(Engine.ClientVersion);

            SplashScreen.Message = LocString.Welcome;
            InitConfig();
            Show();
            BringToFront();
            tabs_IndexChanged(this, null); // load first tab
            ScriptManager.InitScriptEditor();

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

            opacity.SafeAction(s =>
            {
                s.Value = Config.GetInt("Opacity");
                s.AutoSize = false;
            });

            this.SafeAction(s =>
            {
                s.Opacity = ((float) opacity.Value) / 100.0;
                s.TopMost = s.alwaysTop.Checked = Config.GetBool("AlwaysOnTop");
                s.Location = new System.Drawing.Point(Config.GetInt("WindowX"), Config.GetInt("WindowY"));
                s.TopLevel = true;
                if (!IsOnScreen(s))
                {
                    s.Location = new System.Drawing.Point(400, 400);
                }
            });

            opacityLabel.SafeAction(s => { s.Text = Language.Format(LocString.OpacityA1, opacity.Value); });

            spellUnequip.SafeAction(s => { });

            spellUnequip.SafeAction(s => { s.Checked = Config.GetBool("SpellUnequip"); });

            ltRange.SafeAction(s =>
            {
                s.Enabled = rangeCheckLT.Checked = Config.GetBool("RangeCheckLT");
                s.Text = Config.GetInt("LTRange").ToString();
            });

            counters.SafeAction(s =>
            {
                s.BeginUpdate();

                if (Config.GetBool("SortCounters"))
                {
                    s.Sorting = SortOrder.None;
                    s.ListViewItemSorter = CounterLVIComparer.Instance;
                    s.Sort();
                }
                else
                {
                    s.ListViewItemSorter = null;
                    s.Sorting = SortOrder.Ascending;
                }

                s.EndUpdate();
                s.Refresh();
            });

            incomingMob.SafeAction(s => { s.Checked = Config.GetBool("ShowMobNames"); });

            incomingCorpse.SafeAction(s => { s.Checked = Config.GetBool("ShowCorpseNames"); });

            checkNewConts.SafeAction(s => { s.Checked = Config.GetBool("AutoSearch"); });


            excludePouches.SafeAction(s =>
            {
                s.Checked = Config.GetBool("NoSearchPouches");
                s.Enabled = checkNewConts.Checked;
            });

            warnNum.SafeAction(s =>
            {
                warnNum.Enabled = warnCount.Checked = Config.GetBool("CounterWarn");
                warnNum.Text = Config.GetInt("CounterWarnAmount").ToString();
            });

            QueueActions.SafeAction(s => { s.Checked = Config.GetBool("QueueActions"); });

            queueTargets.SafeAction(s => { s.Checked = Config.GetBool("QueueTargets"); });

            chkForceSpeechHue.SafeAction(s => { s.Checked = setSpeechHue.Enabled = Config.GetBool("ForceSpeechHue"); });

            chkForceSpellHue.SafeAction(s =>
            {
                chkForceSpellHue.Checked = setBeneHue.Enabled =
                    setNeuHue.Enabled = setHarmHue.Enabled = Config.GetBool("ForceSpellHue");
            });

            lthilight.SafeAction(s =>
            {
                if (Config.GetInt("LTHilight") != 0)
                {
                    InitPreviewHue(s, "LTHilight");
                    s.Checked = setLTHilight.Enabled = true;
                }
                else
                {
                    s.Checked = setLTHilight.Enabled = false;
                }
            });

            txtSpellFormat.SafeAction(s => { s.Text = Config.GetString("SpellFormat"); });

            txtObjDelay.SafeAction(s => { s.Text = Config.GetInt("ObjectDelay").ToString(); });

            chkStealth.SafeAction(s => { s.Checked = Config.GetBool("CountStealthSteps"); });

            radioUO.SafeAction(s => { s.Checked = !(radioFull.Checked = Config.GetBool("CapFullScreen")); });

            screenPath.SafeAction(s => { s.Text = Config.GetString("CapPath"); });

            dispTime.SafeAction(s => { s.Checked = Config.GetBool("CapTimeStamp"); });

            blockDis.SafeAction(s => { s.Checked = Config.GetBool("BlockDismount"); });

            autoOpenDoors.SafeAction(s => { s.Checked = Config.GetBool("AutoOpenDoors"); });

            objectDelay.SafeAction(s => { s.Checked = Config.GetBool("ObjectDelayEnabled"); });

            txtObjDelay.SafeAction(s => { s.Enabled = Config.GetBool("ObjectDelayEnabled"); });

            msglvl.SafeAction(s => { s.SelectedIndex = Config.GetInt("MessageLevel"); });

            imgFmt.SafeAction(s =>
            {
                try
                {
                    s.SelectedItem = Config.GetString("ImageFormat");
                }
                catch
                {
                    s.SelectedIndex = 0;
                    Config.SetProperty("ImageFormat", "jpg");
                }
            });

            lblExHue.SafeAction(s => { InitPreviewHue(s, "ExemptColor"); });

            lblMsgHue.SafeAction(s => { InitPreviewHue(s, "SysColor"); });

            lblWarnHue.SafeAction(s => { InitPreviewHue(s, "WarningColor"); });

            chkForceSpeechHue.SafeAction(s => { InitPreviewHue(s, "SpeechHue"); });

            lblBeneHue.SafeAction(s => { InitPreviewHue(s, "BeneficialSpellHue"); });

            lblHarmHue.SafeAction(s => { InitPreviewHue(s, "HarmfulSpellHue"); });

            lblNeuHue.SafeAction(s => { InitPreviewHue(s, "NeutralSpellHue"); });

            undressConflicts.SafeAction(s => { s.Checked = Config.GetBool("UndressConflicts"); });

            taskbar.SafeAction(s => { s.Checked = !(systray.Checked = Config.GetBool("Systray")); });

            titlebarImages.SafeAction(s => { s.Checked = Config.GetBool("TitlebarImages"); });

            highlightSpellReags.SafeAction(s => { s.Checked = Config.GetBool("HighlightReagents"); });

            dispDelta.SafeAction(s => { s.Checked = Config.GetBool("DisplaySkillChanges"); });

            titleStr.SafeAction(s =>
            {
                s.Enabled = showInBar.Checked = Config.GetBool("TitleBarDisplay");
                s.Text = Config.GetString("TitleBarText");
            });

            showNotoHue.SafeAction(s => { s.Checked = Config.GetBool("ShowNotoHue"); });

            corpseRange.SafeAction(s =>
            {
                s.Enabled = openCorpses.Checked = Config.GetBool("AutoOpenCorpses");
                s.Text = Config.GetInt("CorpseRange").ToString();
            });

            actionStatusMsg.SafeAction(s => { s.Checked = Config.GetBool("ActionStatusMsg"); });

            autoStackRes.SafeAction(s => { s.Checked = Config.GetBool("AutoStack"); });

            rememberPwds.SafeAction(s => { s.Checked = Config.GetBool("RememberPwds"); });

            filterSnoop.SafeAction(s => { s.Checked = Config.GetBool("FilterSnoopMsg"); });

            preAOSstatbar.SafeAction(s => { s.Checked = Config.GetBool("OldStatBar"); });

            showtargtext.SafeAction(s => { s.Checked = Config.GetBool("LastTargTextFlags"); });

            smartLT.SafeAction(s => { s.Checked = Config.GetBool("SmartLastTarget"); });

            autoFriend.SafeAction(s => { s.Checked = Config.GetBool("AutoFriend"); });

            clientPrio.SafeAction(s =>
            {
                try
                {
                    s.SelectedItem = Config.GetString("ClientPrio");
                }
                catch
                {
                    s.SelectedItem = "Normal";
                }
            });

            forceSizeX.SafeAction(s => { s.Text = Config.GetInt("ForceSizeX").ToString(); });

            forceSizeY.SafeAction(s => { s.Text = Config.GetInt("ForceSizeY").ToString(); });

            gameSize.SafeAction(s => { s.Checked = Config.GetBool("ForceSizeEnabled"); });

            potionEquip.SafeAction(s => { s.Checked = Config.GetBool("PotionEquip"); });

            blockHealPoison.SafeAction(s => { s.Checked = Config.GetBool("BlockHealPoison"); });

            negotiate.SafeAction(s => { s.Checked = Config.GetBool("Negotiate"); });

            logPackets.SafeAction(s => { s.Checked = Config.GetBool("LogPacketsByDefault"); });

            healthFmt.SafeAction(s =>
            {
                s.Enabled = showHealthOH.Checked = Config.GetBool("ShowHealth");
                s.Text = Config.GetString("HealthFmt");
            });

            chkPartyOverhead.SafeAction(s => { s.Checked = Config.GetBool("ShowPartyStats"); });

            dressList.SafeAction(s => { s.SelectedIndex = -1; });

            hotkeyTree.SafeAction(s => { s.SelectedNode = null; });

            targetByTypeDifferent.SafeAction(s => { s.Checked = Config.GetBool("DiffTargetByType"); });

            stepThroughMacro.SafeAction(s => { s.Checked = Config.GetBool("StepThroughMacro"); });

            showTargetMessagesOverChar.SafeAction(s =>
            {
                s.Checked = Config.GetBool("ShowTargetSelfLastClearOverhead");
            });

            showOverheadMessages.SafeAction(s => { s.Checked = Config.GetBool("ShowOverheadMessages"); });

            logSkillChanges.SafeAction(s => { s.Checked = Config.GetBool("LogSkillChanges"); });

            lightLevelBar.SafeAction(s => { s.Value = s.Maximum - Config.GetInt("LightLevel"); });

            double percent = Math.Round((lightLevelBar.Value / (double) lightLevelBar.Maximum) * 100.0);

            lightLevel.SafeAction(s => { s.Text = $"Light: {percent}%"; });

            captureMibs.SafeAction(s => { s.Checked = Config.GetBool("CaptureMibs"); });

            stealthOverhead.SafeAction(s => { s.Checked = Config.GetBool("StealthOverhead"); });

            blockOpenCorpsesTwice.SafeAction(s => { s.Checked = Config.GetBool("BlockOpenCorpsesTwice"); });

            screenShotOpenBrowser.SafeAction(s => { s.Checked = Config.GetBool("ScreenshotUploadOpenBrowser"); });

            screenShotClipboard.SafeAction(s => { s.Checked = Config.GetBool("ScreenshotUploadClipboard"); });

            screenShotNotification.SafeAction(s => { s.Checked = Config.GetBool("ScreenshotUploadNotifications"); });

            showContainerLabels.SafeAction(s => { s.Checked = Config.GetBool("ShowContainerLabels"); });

            seasonList.SafeAction(s => { s.SelectedIndex = Config.GetInt("Season"); });

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

            showAttackTarget.SafeAction(s => { s.Checked = Config.GetBool("ShowAttackTargetOverhead"); });

            showBuffDebuffOverhead.SafeAction(s => { s.Checked = Config.GetBool("ShowBuffDebuffOverhead"); });

            rangeCheckTargetByType.SafeAction(s => { s.Checked = Config.GetBool("RangeCheckTargetByType"); });

            rangeCheckDoubleClick.SafeAction(s => { s.Checked = Config.GetBool("RangeCheckDoubleClick"); });

            blockTradeRequests.SafeAction(s => { s.Checked = Config.GetBool("BlockTradeRequests"); });

            blockPartyInvites.SafeAction(s => { s.Checked = Config.GetBool("BlockPartyInvites"); });

            autoAcceptParty.SafeAction(s => { s.Checked = Config.GetBool("AutoAcceptParty"); });

            minMaxLightLevel.SafeAction(s => { s.Checked = Config.GetBool("MinMaxLightLevelEnabled"); });

            showTextTargetIndicator.SafeAction(s => { s.Checked = Config.GetBool("ShowTextTargetIndicator"); });

            showAttackTargetNewOnly.SafeAction(s => { s.Checked = Config.GetBool("ShowAttackTargetNewOnly"); });

            macroVariableTypeList.SafeAction(s => { s.SelectedIndex = 0; });

            filterDragonGraphics.SafeAction(s => { s.Checked = Config.GetBool("FilterDragonGraphics"); });

            filterDrakeGraphics.SafeAction(s => { s.Checked = Config.GetBool("FilterDrakeGraphics"); });

            LoadAnimationLists();

            showDamageDealt.SafeAction(s => { s.Checked = Config.GetBool("ShowDamageDealt"); });

            damageDealtOverhead.SafeAction(s => { s.Checked = Config.GetBool("ShowDamageDealtOverhead"); });

            showDamageTaken.SafeAction(s => { s.Checked = Config.GetBool("ShowDamageTaken"); });

            damageDealtOverhead.SafeAction(s => { s.Checked = Config.GetBool("ShowDamageTakenOverhead"); });

            razorTitleBar.SafeAction(s => { s.Text = Config.GetString("RazorTitleBarText"); });

            showInRazorTitleBar.SafeAction(s => { s.Checked = Config.GetBool("ShowInRazorTitleBar"); });

            enableUOAAPI.SafeAction(s => { s.Checked = Config.GetBool("EnableUOAAPI"); });

            lastBackup.SafeAction(s => { s.Text = $"Last Backup: {Config.GetAppSetting<string>("BackupTime")}"; });

            targetIndictorFormat.SafeAction(s => { s.Text = Config.GetString("TargetIndicatorFormat"); });

            nextPrevIgnoresFriends.SafeAction(s => { s.Checked = Config.GetBool("NextPrevTargetIgnoresFriends"); });

            stealthStepsFormat.SafeAction(s => { s.Text = Config.GetString("StealthStepsFormat"); });

            showStaticWalls.SafeAction(s => { s.Checked = Config.GetBool("ShowStaticWalls"); });

            showStaticWallLabels.SafeAction(s => { s.Checked = Config.GetBool("ShowStaticWallLabels"); });

            showFriendOverhead.SafeAction(s => { s.Checked = Config.GetBool("ShowFriendOverhead"); });

            dispDeltaOverhead.SafeAction(s => { s.Checked = Config.GetBool("DisplaySkillChangesOverhead"); });

            macroActionDelay.SafeAction(s => { s.Checked = Config.GetBool("MacroActionDelay"); });

            autoOpenDoorWhenHidden.SafeAction(s => { s.Checked = Config.GetBool("AutoOpenDoorWhenHidden"); });

            disableMacroPlayFinish.SafeAction(s => { s.Checked = Config.GetBool("DisableMacroPlayFinish"); });


            showBandageTimer.SafeAction(s => { s.Checked = Config.GetBool("ShowBandageTimer"); });
            bandageTimerLocation.SafeAction(s => { s.SelectedIndex = Config.GetInt("ShowBandageTimerLocation"); });
            onlyShowBandageTimerSeconds.SafeAction(s => { s.Checked = Config.GetBool("OnlyShowBandageTimerEvery"); });
            bandageTimerSeconds.SafeAction(s => { s.Text = Config.GetInt("OnlyShowBandageTimerSeconds").ToString(); });
            bandageTimerFormat.SafeAction(s => { s.Text = Config.GetString("ShowBandageTimerFormat"); });
            lblBandageCountFormat.SafeAction(s => { InitPreviewHue(s, "ShowBandageTimerHue"); });

            friendOverheadFormat.SafeAction(s => { s.Text = Config.GetString("FriendOverheadFormat"); });

            friendFormat.SafeAction(s => { InitPreviewHue(s, "FriendOverheadFormatHue"); });

            lblTargetFormat.SafeAction(s => { InitPreviewHue(s, "TargetIndicatorHue"); });

            filterSystemMessages.SafeAction(s => { s.Checked = Config.GetBool("FilterSystemMessages"); });
            filterRazorMessages.SafeAction(s => { s.Checked = Config.GetBool("FilterRazorMessages"); });
            filterDelaySeconds.SafeAction(s => { s.Text = Config.GetDouble("FilterDelay").ToString(); });
            filterOverheadMessages.SafeAction(s => { s.Checked = Config.GetBool("FilterOverheadMessages"); });

            onlyNextPrevBeneficial.SafeAction(s => { s.Checked = Config.GetBool("OnlyNextPrevBeneficial"); });
            friendBeneficialOnly.SafeAction(s => { s.Checked = Config.GetBool("FriendlyBeneficialOnly"); });
            nonFriendlyHarmfulOnly.SafeAction(s => { s.Checked = Config.GetBool("NonFriendlyHarmfulOnly"); });

            showBandageStart.SafeAction(s => { s.Checked = Config.GetBool("ShowBandageStart"); });
            showBandageEnd.SafeAction(s => { s.Checked = Config.GetBool("ShowBandageEnd"); });
            bandageStartMessage.SafeAction(s => { s.Text = Config.GetString("BandageStartMessage"); });
            bandageEndMessage.SafeAction(s => { s.Text = Config.GetString("BandageEndMessage"); });

            captureOthersDeathDelay.SafeAction(
                s => { s.Text = Config.GetDouble("CaptureOthersDeathDelay").ToString(); });
            captureOwnDeathDelay.SafeAction(s => { s.Text = Config.GetDouble("CaptureOwnDeathDelay").ToString(); });
            captureOthersDeath.SafeAction(s => { s.Checked = Config.GetBool("CaptureOthersDeath"); });
            captureOwnDeath.SafeAction(s => { s.Checked = Config.GetBool("CaptureOwnDeath"); });

            targetFilterEnabled.SafeAction(s => { s.Checked = Config.GetBool("TargetFilterEnabled"); });

            filterDaemonGraphics.SafeAction(s => { s.Checked = Config.GetBool("FilterDaemonGraphics"); });

            soundFilterEnabled.SafeAction(s => { s.Checked = Config.GetBool("SoundFilterEnabled"); });
            showFilteredSound.SafeAction(s => { s.Checked = Config.GetBool("ShowFilteredSound"); });
            showPlayingSoundInfo.SafeAction(s => { s.Checked = Config.GetBool("ShowPlayingSoundInfo"); });
            showPlayingMusic.SafeAction(s => { s.Checked = Config.GetBool("ShowMusicInfo"); });

            autoSaveScript.SafeAction(s => { s.Checked = Config.GetBool("AutoSaveScript"); });
            autoSaveScriptPlay.SafeAction(s => { s.Checked = Config.GetBool("AutoSaveScriptPlay"); });

            highlightFriend.SafeAction(s => { s.Checked = Config.GetBool("HighlightFriend"); });

            // Disable SmartCPU in case it was enabled before the feature was removed
            Client.Instance.SetSmartCPU(false);

            if (!Client.IsOSI)
                DisableCUOFeatures();

            m_Initializing = false;
        }

        private class AnimData
        {
            public string name { get; set; }
            public string body { get; set; }
        }

        private List<AnimData> _animationData = new List<AnimData>();

        private void LoadAnimationLists()
        {
            int hue = 0;

            using (StreamReader r = new StreamReader(Path.Combine(Config.GetInstallDirectory(), "animdata.json")))
            {
                string json = r.ReadToEnd();
                List<AnimData> items = JsonConvert.DeserializeObject<List<AnimData>>(json);

                _animationData.Clear();
                dragonAnimationList.Items.Clear();
                drakeAnimationList.Items.Clear();

                foreach (AnimData animData in items)
                {
                    try
                    {
                        Frame[] frames =
                            Animations.GetAnimation(Convert.ToInt32(animData.body), 0, 1, ref hue, false, false);

                        if (frames != null)
                        {
                            _animationData.Add(animData);
                            dragonAnimationList.Items.Add(animData.name);
                            drakeAnimationList.Items.Add(animData.name);
                            daemonAnimationList.Items.Add(animData.name);
                        }
                    }
                    catch //Unable to verify animation, lets add it anyway
                    {
                        _animationData.Add(animData);
                        dragonAnimationList.Items.Add(animData.name);
                        drakeAnimationList.Items.Add(animData.name);
                        daemonAnimationList.Items.Add(animData.name);
                    }
                }
            }

            int animIndex = 0;
            foreach (AnimData animData in _animationData)
            {
                if (animData.body.Equals(Config.GetInt("DragonGraphic").ToString()))
                {
                    dragonAnimationList.SelectedIndex = animIndex;
                    break;
                }

                animIndex++;
            }

            animIndex = 0;
            foreach (AnimData animData in _animationData)
            {
                if (animData.body.Equals(Config.GetInt("DrakeGraphic").ToString()))
                {
                    drakeAnimationList.SelectedIndex = animIndex;
                    break;
                }

                animIndex++;
            }

            animIndex = 0;
            foreach (AnimData animData in _animationData)
            {
                if (animData.body.Equals(Config.GetInt("DaemonGraphic").ToString()))
                {
                    daemonAnimationList.SelectedIndex = animIndex;
                    break;
                }

                animIndex++;
            }
        }

        private void tabs_IndexChanged(object sender, System.EventArgs e)
        {
            if (tabs == null)
                return;

            if (tabs.SelectedTab == generalTab)
            {
                Filter.Draw(filters);
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
            }
            else if (tabs.SelectedTab == dressTab)
            {
                int sel = dressList.SelectedIndex;
                dressItems.Items.Clear();
                DressList.Redraw();
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
            }
            else if (tabs.SelectedTab == scriptsTab)
            {
                RedrawScripts();

                if (scriptList.SelectedIndex < 0)
                    ScriptManager.RedrawScripts();
            }
            else if (tabs.SelectedTab == moreOptTab)
            {
                FriendsManager.RedrawGroup();
            }
        }

        private void subGeneralTab_IndexChanged(object sender, EventArgs e)
        {
            if (subGeneralTab == null)
                return;

            if (subGeneralTab.SelectedTab == subTargetFilterTab)
            {
                TargetFilterManager.RedrawList();
            }

            if (subGeneralTab.SelectedTab == subSoundMusicTab)
            {
                SoundMusicManager.RedrawList();
            }
        }

        private void RebuildHotKeyCache()
        {
            _hotkeyTreeViewCache = new TreeView();

            foreach (TreeNode node in hotkeyTree.Nodes)
            {
                _hotkeyTreeViewCache.Nodes.Add((TreeNode) node.Clone());
            }
        }

        private void RebuildMacroCache()
        {
            _macroTreeViewCache = new TreeView();

            foreach (TreeNode node in macroTree.Nodes)
            {
                _macroTreeViewCache.Nodes.Add((TreeNode) node.Clone());
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
            if (!Client.Instance.ClientRunning)
                Close();

            uint ps = m_OutPrev;
            uint pr = m_InPrev;
            m_OutPrev = Client.Instance.TotalDataOut();
            m_InPrev = Client.Instance.TotalDataIn();

            tabs.SafeAction(s =>
            {
                if (s.SelectedTab != advancedTab)
                {
                    return;
                }
            });

            int time = 0;
            if (Client.Instance.ConnectionStart != DateTime.MinValue)
                time = (int) ((DateTime.UtcNow - Client.Instance.ConnectionStart).TotalSeconds);


            statusBox.SafeAction(s =>
            {
                if (string.IsNullOrEmpty(s.SelectedText))
                {
                    s.Lines = Language.Format(LocString.RazorStatus1,
                        m_Ver,
                        Utility.FormatSize(System.GC.GetTotalMemory(false)),
                        Utility.FormatSize(m_OutPrev), Utility.FormatSize((long) ((m_OutPrev - ps))),
                        Utility.FormatSize(m_InPrev), Utility.FormatSize((long) ((m_InPrev - pr))),
                        Utility.FormatTime(time),
                        (World.Player != null ? (uint) World.Player.Serial : 0),
                        (World.Player != null && World.Player.Backpack != null
                            ? (uint) World.Player.Backpack.Serial
                            : 0),
                        World.Items.Count,
                        World.Mobiles.Count).Split('\n');

                    if (World.Player != null)
                        statusBox.SafeAction(x =>
                            x.AppendText(
                                $"\r\nCoordinates: {World.Player.Position.X} {World.Player.Position.Y} {World.Player.Position.Z}"));
                }
            });

            if (PacketHandlers.PlayCharTime < DateTime.UtcNow &&
                PacketHandlers.PlayCharTime + TimeSpan.FromSeconds(5) < DateTime.UtcNow)
            {
                if (Config.GetBool("Negotiate"))
                {
                    bool allAllowed = true;
                    StringBuilder text = new StringBuilder();

                    text.Append(Language.GetString(LocString.NegotiateTitle));
                    text.Append("\r\n");

                    for (int i = 0; i < FeatureBit.MaxBit; i++)
                    {
                        if (!Client.Instance.AllowBit(i))
                        {
                            allAllowed = false;

                            text.Append(Language.GetString((LocString) (((int) LocString.FeatureDescBase) + i)));
                            text.Append(' ');
                            text.Append(Language.GetString(LocString.NotAllowed));
                            text.Append("\r\n");
                        }
                    }

                    if (allAllowed)
                        text.Append(Language.GetString(LocString.AllFeaturesEnabled));

                    text.Append("\r\n");

                    features.SafeAction(x => x.Visible = true);
                    features.SafeAction(x => x.Text = text.ToString());
                }
                else
                {
                    features.SafeAction(x => x.Visible = false);
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
                string skillLog = Path.Combine(Config.GetInstallDirectory(), "SkillLog");
                skillLog = Path.Combine(skillLog, $"{World.Player.Name}_{World.Player.Serial}_SkillLog.csv");


                if (!Directory.Exists(Path.GetDirectoryName(skillLog)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(skillLog));
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
                        $"{DateTime.Now},{(SkillName) skill.Index},{skill.Value},{skill.Base},{skill.Delta},{skill.Cap}");
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

        public void ToggleDamageTracker(bool enable)
        {
            if (World.Player == null)
            {
                DamageTracker.Stop();
                return;
            }

            if (enable)
            {
                trackDps.Checked = true;
            }
            else
            {
                trackDps.Checked = false;
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
                    items[0] = Language.Skill2Str(i); //((SkillName)i).ToString();
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
            ((Filter) filters.Items[e.Index]).OnCheckChanged(e.NewValue);
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
                        new MenuItem(Language.GetString(LocString.SetSLUp), new EventHandler(onSetSkillLockUP)),
                        new MenuItem(Language.GetString(LocString.SetSLDown), new EventHandler(onSetSkillLockDOWN)),
                        new MenuItem(Language.GetString(LocString.SetSLLocked), new EventHandler(onSetSkillLockLOCKED))
                    });
                }

                for (int i = 0; i < 3; i++)
                    m_SkillMenu.MenuItems[i].Checked = ((int) s.Lock) == i;

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
                Client.Instance.SendToServer(new SetSkillLock(s.Index, lockType));

                s.Lock = lockType;
                UpdateSkill(s);

                Client.Instance.SendToClient(new SkillUpdate(s));
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

            public static int Column
            {
                set { Instance.m_Col = value; }
            }

            public static bool Asc
            {
                set { Instance.m_Asc = value; }
            }

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
                    double dx = Convert.ToDouble(((ListViewItem) x).SubItems[m_Col].Text);
                    double dy = Convert.ToDouble(((ListViewItem) y).SubItems[m_Col].Text);

                    if (dx > dy)
                        return m_Asc ? -1 : 1;
                    else if (dx == dy)
                        return 0;
                    else //if ( dx > dy )
                        return m_Asc ? 1 : -1;
                }
                catch
                {
                    return ((ListViewItem) x).Text.CompareTo(((ListViewItem) y).Text) * (m_Asc ? 1 : -1);
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

            LockType type = (LockType) locks.SelectedIndex;

            for (short i = 0; i < Skill.Count; i++)
            {
                World.Player.Skills[i].Lock = type;
                Client.Instance.SendToServer(new SetSkillLock(i, type));
            }

            Client.Instance.SendToClient(new SkillsList());
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
                        c.Set((ushort) ac.ItemID, ac.Hue, ac.NameStr, ac.FmtStr, ac.DisplayImage);
                        break;
                }
            }
        }

        private void addCounter_Click(object sender, System.EventArgs e)
        {
            AddCounter dlg = new AddCounter();

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Counter.Register(new Counter(dlg.NameStr, dlg.FmtStr, (ushort) dlg.ItemID, (int) dlg.Hue,
                    dlg.DisplayImage));
                Counter.Redraw(counters);
            }
        }

        private void showInBar_CheckedChanged(object sender, System.EventArgs e)
        {
            titleStr.Enabled = showInBar.Checked;
            Config.SetProperty("TitleBarDisplay", showInBar.Checked);
            Client.Instance.RequestTitlebarUpdate();
        }

        private void titleStr_TextChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("TitleBarText", titleStr.Text.TrimEnd());
            if (Config.GetBool("TitleBarDisplay"))
                Client.Instance.RequestTitlebarUpdate();
        }

        private void counters_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (e.Index >= 0 && e.Index < Counter.List.Count && !Counter.SupressChecks)
            {
                ((Counter) (counters.Items[e.Index].Tag)).SetEnabled(e.NewValue == CheckState.Checked);
                Client.Instance.RequestTitlebarUpdate();
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

            string name = (string) profiles.Items[profiles.SelectedIndex];
            if (MessageBox.Show(this, Language.Format(LocString.ProfLoadQ, name), "Load?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Config.Save();
                if (!Config.LoadProfile(name))
                {
                    MessageBox.Show(this, Language.GetString(LocString.ProfLoadE), "Load Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    InitConfig();
                    if (World.Player != null)
                        Config.SetProfileFor(World.Player);
                }

                Client.Instance.RequestTitlebarUpdate();
            }
            else
            {
                m_ProfileConfirmLoad = false;
                for (int i = 0; i < profiles.Items.Count; i++)
                {
                    if ((string) profiles.Items[i] == Config.CurrentProfile.Name)
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

            string remove = (string) profiles.Items[profiles.SelectedIndex];

            if (remove == "default")
            {
                MessageBox.Show(this, Language.GetString(LocString.NoDelete), "Not Allowed", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
                if ((string) profiles.Items[i] == "default")
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
                    if (lwr == ((string) profiles.Items[i]).ToLower())
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

                            Client.Instance.RequestTitlebarUpdate();
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
            get { return m_CanClose; }
            set { m_CanClose = value; }
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!m_CanClose && Client.Instance.ClientRunning)
            {
                DisableCloseButton();
                e.Cancel = true;
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
                sb.AppendFormat("{0,-20} {1,-5:F1} {2,-5:F1} {3}{4,-5:F1} {5,-5:F1}\n", (SkillName) i, sk.Value,
                    sk.Base, sk.Delta > 0 ? "+" : "", sk.Delta, sk.Cap);
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

                dressList.SafeAction(s =>
                {
                    s.Items.Add(list);
                    s.SelectedItem = list;
                });
            }
        }

        private void removeDress_Click(object sender, System.EventArgs e)
        {
            DressList dress = (DressList) dressList.SelectedItem;

            if (dress != null && MessageBox.Show(this, Language.GetString(LocString.DelDressQ), "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dress.Items.Clear();

                dressList.SafeAction(s =>
                {
                    s.Items.Remove(dress);
                    s.SelectedIndex = -1;
                });

                dressItems.SafeAction(s => s.Items.Clear());

                DressList.Remove(dress);
            }
        }

        private void dressNow_Click(object sender, System.EventArgs e)
        {
            DressList dress = (DressList) dressList.SelectedItem;
            if (dress != null && World.Player != null)
                dress.Dress();
        }

        private void undressList_Click(object sender, System.EventArgs e)
        {
            DressList dress = (DressList) dressList.SelectedItem;
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
                DressList list = (DressList) dressList.SelectedItem;

                if (list == null)
                    return;

                list.Items.Add(serial);
                Item item = World.FindItem(serial);

                dressItems.SafeAction(s =>
                {
                    if (item == null)
                        s.Items.Add(Language.Format(LocString.OutOfRangeA1, serial));
                    else
                        s.Items.Add(item.ToString());
                });
            }
        }

        private void dressDelSel_Click(object sender, System.EventArgs e)
        {
            DressList list = (DressList) dressList.SelectedItem;
            if (list == null)
                return;

            int sel = dressItems.SelectedIndex;
            if (sel < 0 || sel >= list.Items.Count)
                return;

            if (MessageBox.Show(this, Language.GetString(LocString.DelDressItemQ), "Confirm", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    list.Items.RemoveAt(sel);
                    dressItems.SafeAction(s => s.Items.RemoveAt(sel));
                }
                catch
                {
                }
            }
        }

        private void clearDress_Click(object sender, System.EventArgs e)
        {
            DressList list = (DressList) dressList.SelectedItem;
            if (list == null)
                return;

            if (MessageBox.Show(this, Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                list.Items.Clear();
                dressItems.SafeAction(s => s.Items.Clear());
            }
        }

        private DressList undressBagList = null;

        private void undressBag_Click(object sender, System.EventArgs e)
        {
            if (World.Player == null)
                return;

            DressList list = (DressList) dressList.SelectedItem;
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
            DressList list = (DressList) dressList.SelectedItem;

            dressItems.BeginUpdate();
            dressItems.Items.Clear();
            if (list != null)
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    if (list.Items[i] is Serial)
                    {
                        Serial serial = (Serial) list.Items[i];
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
            DressList list = (DressList) dressList.SelectedItem;
            if (World.Player == null)
                return;
            if (list == null)
                return;

            for (int i = 0; i < World.Player.Contains.Count; i++)
            {
                Item item = (Item) World.Player.Contains[i];
                if (item.Layer <= Layer.LastUserValid && item.Layer != Layer.Backpack && item.Layer != Layer.Hair &&
                    item.Layer != Layer.FacialHair)
                    list.Items.Add(item.Serial);
            }

            dressList.SafeAction(s =>
            {
                s.SelectedItem = null;
                s.SelectedItem = list;
            });
        }

        private void hotkeyTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            ClearHKCtrls();

            if (e.Node == null || !(e.Node.Tag is KeyData))
                return;
            KeyData hk = (KeyData) e.Node.Tag;

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
                            key.Text = (((Keys) hk.Key).ToString());
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

            if ((hk.LocName >= (int) LocString.DrinkHeal && hk.LocName <= (int) LocString.DrinkAg &&
                 !Client.Instance.AllowBit(FeatureBit.PotionHotkeys)) ||
                (hk.LocName >= (int) LocString.TargCloseRed && hk.LocName <= (int) LocString.TargCloseCriminal &&
                 !Client.Instance.AllowBit(FeatureBit.ClosestTargets)) ||
                (((hk.LocName >= (int) LocString.TargRandRed && hk.LocName <= (int) LocString.TargRandNFriend) ||
                  (hk.LocName >= (int) LocString.TargRandEnemyHuman && hk.LocName <= (int) LocString.TargRandCriminal)
                 ) && !Client.Instance.AllowBit(FeatureBit.RandomTargets)))
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
                if (MessageBox.Show(this, Language.Format(LocString.KeyUsed, g.DispName, hk.DispName),
                        "Hot Key Conflict", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
            m_LastKV = (int) e.KeyCode;
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

                ScriptManager.AddToScript($"hotkey '{hk.DispName}'");

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
                Client.Instance.SetCustomNotoHue(0);
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
                Client.Instance.SetCustomNotoHue(Config.GetInt("LTHilight"));
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
                Agent.Select(agentList.SelectedIndex, agentList, agentSubList, agentGroup, agentB1, agentB2, agentB3,
                    agentB4, agentB5, agentB6);
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

        public bool IsOnScreen(Form form)
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                Point formTopLeft = new Point(form.Left, form.Top);

                if (screen.WorkingArea.Contains(formTopLeft))
                {
                    return true;
                }
            }

            return false;
        }

        private void MainForm_Move(object sender, System.EventArgs e)
        {
            // atempt to dock to the side of the screen.  Also try not to save the X/Y when we are minimized (which is -32000, -32000)
            System.Drawing.Point pt = this.Location;

            Rectangle screen = Screen.GetWorkingArea(this);
            if (this.WindowState != FormWindowState.Minimized && pt.X + this.Width / 2 >= screen.Left &&
                pt.Y + this.Height / 2 >= screen.Top && pt.X <= screen.Right && pt.Y <= screen.Bottom)
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
                Config.SetProperty("WindowX", (int) pt.X);
                Config.SetProperty("WindowY", (int) pt.Y);
            }
        }

        private void opacity_Scroll(object sender, System.EventArgs e)
        {
            int o = opacity.Value;
            Config.SetProperty("Opacity", o);
            opacityLabel.Text = String.Format("Opacity: {0}%", o);
            this.Opacity = ((double) o) / 100.0;
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
                if (m_Initializing || MessageBox.Show(this, Language.GetString(LocString.PacketLogWarn), "Caution!",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
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
                Client.Instance.RequestTitlebarUpdate();
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
                m_DressItemsMenu = new ContextMenu(new MenuItem[]
                    {new MenuItem(Language.GetString(LocString.Conv2Type), new EventHandler(OnMakeType))});
                m_DressItemsMenu.Show(dressItems, new Point(e.X, e.Y));
            }
        }

        private void OnMakeType(object sender, System.EventArgs e)
        {
            DressList list = (DressList) dressList.SelectedItem;
            if (list == null)
                return;
            int sel = dressItems.SelectedIndex;
            if (sel < 0 || sel >= list.Items.Count)
                return;

            if (list.Items[sel] is Serial)
            {
                Serial s = (Serial) list.Items[sel];
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

        private static char[] m_InvalidNameChars = new char[] {'/', '\\', ';', '?', ':', '*'};

        private void newMacro_Click(object sender, System.EventArgs e)
        {
            if (InputBox.Show(this, Language.GetString(LocString.NewMacro), Language.GetString(LocString.EnterAName)))
            {
                string name = InputBox.GetString();
                if (name == null || name == "" || name.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                    name.IndexOfAny(m_InvalidNameChars) != -1)
                {
                    MessageBox.Show(this, Language.GetString(LocString.InvalidChars),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                TreeNode node = GetMacroDirNode();
                string path = (node == null || !(node.Tag is string))
                    ? Config.GetUserDirectory("Macros")
                    : (string) node.Tag;
                path = Path.Combine(path, name + ".macro");
                if (File.Exists(path))
                {
                    MessageBox.Show(this, Language.GetString(LocString.MacroExists),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    bool createFromClipboard = false;

                    // TODO: Instead of contains, do we want to look at the first X number of characters
                    if (Clipboard.GetText().Contains("Assistant.Macros."))
                    {
                        createFromClipboard = MessageBox.Show(this, Language.GetString(LocString.NewClipboardMacro),
                                                  "Create new macro from clipboard?", MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question) == DialogResult.Yes;

                        if (createFromClipboard)
                        {
                            string[] macroCommands = Clipboard.GetText()
                                .Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);

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
                    // access issue or other issue, create empty macro and move on
                    File.CreateText(path).Close();
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

            filterMacros.Text = string.Empty;

            RedrawMacros();
        }

        public Macro GetMacroSel()
        {
            if (macroTree.SelectedNode == null || !(macroTree.SelectedNode.Tag is Macro))
                return null;
            else
                return (Macro) macroTree.SelectedNode.Tag;
        }

        public ScriptManager.RazorScript GetScriptSel()
        {
            if (scriptList.SelectedItem == null || !(scriptList.SelectedItem is ScriptManager.RazorScript))
                return null;
            else
                return (ScriptManager.RazorScript) scriptList.SelectedItem;
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
                    rec = MessageBox.Show(this, Language.GetString(LocString.MacroConfRec), "Overwrite?",
                              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

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

        public void PlayScript()
        {
            playScript.Text = "Stop";
            recordScript.Enabled = false;
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
                        new MenuItem("Add Category", new EventHandler(Macro_AddCategory)),
                        new MenuItem("Delete Category", new EventHandler(Macro_DeleteCategory)),
                        new MenuItem("Move to Category", new EventHandler(Macro_Move2Category)),
                        new MenuItem("-"),
                        new MenuItem("Copy to Clipboard", new EventHandler(Macro_CopyToClipboard)),
                        new MenuItem("Rename Macro", new EventHandler(Macro_Rename)),
                        new MenuItem("Open Externally", new EventHandler(Open_Externally)),
                        new MenuItem("-"),
                        new MenuItem("Refresh Macro List", new EventHandler(Macro_RefreshList))
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
            if (path == null || path == "" || path.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                path.IndexOfAny(m_InvalidNameChars) != -1)
            {
                MessageBox.Show(this, Language.GetString(LocString.InvalidChars), "Invalid Path", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            TreeNode node = GetMacroDirNode();

            try
            {
                if (node == null || !(node.Tag is string))
                    path = Path.Combine(Config.GetUserDirectory("Macros"), path);
                else
                    path = Path.Combine((string) node.Tag, path);
                Engine.EnsureDirectory(path);
            }
            catch
            {
                MessageBox.Show(this, Language.Format(LocString.CanCreateDir, path), "Unabled to Create Directory",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show(this, Language.GetString(LocString.CantDelDir), "Unable to Delete Directory",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    : Path.Combine(Config.GetUserDirectory("Macros"),
                        $"{InputDropdown.GetString()}/{Path.GetFileName(sel.Filename)}"));
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
                MessageBox.Show(this, Language.GetString(LocString.ReadError), "Copy Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void Macro_Rename(object sender, EventArgs args)
        {
            Macro sel = GetMacroSel();
            if (sel == null)
                return;

            if (InputBox.Show(this, Language.GetString(LocString.MacroRename),
                Language.GetString(LocString.EnterAName)))
            {
                string name = InputBox.GetString();
                if (string.IsNullOrEmpty(name) || name.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                    name.IndexOfAny(m_InvalidNameChars) != -1)
                {
                    MessageBox.Show(this, Language.GetString(LocString.InvalidChars),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string newMacro = $"{Path.GetDirectoryName(sel.Filename)}/{name}.macro";


                if (File.Exists(newMacro))
                {
                    MessageBox.Show(this, Language.GetString(LocString.MacroExists),
                        Language.GetString(LocString.Invalid),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    Engine.MainWindow.SafeAction(s =>
                    {
                        File.Move(sel.Filename, newMacro);
                        MacroManager.Remove(sel);
                    });
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
                MessageBox.Show(this, Language.GetString(LocString.UnableToOpenMacro),
                    Language.GetString(LocString.ReadError),
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

            RebuildMacroCache();

            MacroManager.DisplayMacroVariables(macroVariables);
        }

        private void RedrawScripts()
        {
            ScriptManager.LoadScripts();
            ScriptManager.DisplayScriptVariables(scriptVariables);
        }

        public Macro LastSelectedMacro { get; set; }

        private void macroTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (MacroManager.Recording)
                return;

            Macro m = e.Node.Tag as Macro;
            macroActGroup.Visible = m != null;
            MacroManager.Select(m, actionList, playMacro, recMacro, loopMacro);

            LastSelectedMacro = m;

            if (m == null)
                return;

            Engine.MainWindow.SafeAction(s =>
            {
                if (hotkeyTree.TopNode == null)
                {
                    HotKey.RebuildList(hotkeyTree);
                    RebuildHotKeyCache();
                }

                TreeNode resultNode = SearchTreeView(m.GetName(), hotkeyTree.Nodes);

                if (resultNode != null)
                {
                    KeyData hk = (KeyData) resultNode.Tag;

                    if (hk != null && !string.IsNullOrEmpty(hk.KeyString()))
                    {
                        macroActGroup.Text = $"Actions ({hk.KeyString()})";
                    }
                    else
                    {
                        macroActGroup.Text = $"Actions (Not Set)";
                    }
                }
            });
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

            if (m.Actions.Count <= 0 || MessageBox.Show(this, Language.Format(LocString.DelConf, m.ToString()),
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

            RebuildMacroCache();
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

                    menu.MenuItems.Add("Copy Line", new EventHandler(onMacroCopyLine));
                    menu.MenuItems.Add("Paste Line", new EventHandler(onMacroPasteLine));

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
                    new MenuItem(Language.GetString(LocString.InsertOverheadMessage),
                        new EventHandler(onMacroInsertOverheadMessage)),
                    new MenuItem(Language.GetString(LocString.InsertWaitForTarget),
                        new EventHandler(onMacroInsertWaitForTarget)),
                    new MenuItem("-"),
                    new MenuItem(Language.GetString(LocString.InsIF), new EventHandler(onMacroInsertIf)),
                    new MenuItem(Language.GetString(LocString.InsELSE), new EventHandler(onMacroInsertElse)),
                    new MenuItem(Language.GetString(LocString.InsENDIF), new EventHandler(onMacroInsertEndIf)),
                    new MenuItem("-"),
                    new MenuItem(Language.GetString(LocString.InsFOR), new EventHandler(onMacroInsertFor)),
                    new MenuItem(Language.GetString(LocString.InsENDFOR), new EventHandler(onMacroInsertEndFor)),
                    new MenuItem("-"),
                    new MenuItem(Language.GetString(LocString.InsertWhile), new EventHandler(onMacroInsertWhile)),
                    new MenuItem(Language.GetString(LocString.InsertEndWhile), new EventHandler(onMacroInsertEndWhile)),
                    new MenuItem("-"),
                    new MenuItem(Language.GetString(LocString.InsertDo), new EventHandler(onMacroInsertDo)),
                    new MenuItem(Language.GetString(LocString.InsertDoWhile), new EventHandler(onMacroInsertDoWhile)),
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
                        else if (a.GetType().Name.Equals("DoWhileAction"))
                        {
                            aMenus[0].PerformClick();
                        }
                        else if (a.GetType().Name.Equals("WhileAction"))
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
                        else if (a.GetType().Name.Equals("OverheadMessageAction"))
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
            Macro m = GetMacroSel();
            ;
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

        private void onMacroInsertOverheadMessage(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            if (InputBox.Show(Language.GetString(LocString.InsertOverheadMessage)))
            {
                m.Actions.Insert(a + 1,
                    new OverheadMessageAction((ushort) Config.GetInt("SysColor"), InputBox.GetString()));
                RedrawActionList(m);
            }
        }

        private void onMacroInsertWaitForTarget(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            m.Actions.Insert(a + 1, new WaitForTargetAction());
            RedrawActionList(m);
        }

        private void onMacroInsertSetMacroVariable(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            MenuItem mnu = (MenuItem) sender;

            m.Actions.Insert(a + 1, new SetMacroVariableTargetAction(mnu.Text));
            RedrawActionList(m);
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
            Macro m = GetMacroSel();
            ;
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
            Macro m = GetMacroSel();
            ;
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

        private void onMacroInsertWhile(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            MacroInsertWhile ins = new MacroInsertWhile(m, a);
            if (ins.ShowDialog(this) == DialogResult.OK)
                RedrawActionList(m);
        }

        private void onMacroInsertEndWhile(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            m.Actions.Insert(a + 1, new EndWhileAction());
            RedrawActionList(m);
        }

        private void onMacroInsertDo(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            m.Actions.Insert(a + 1, new StartDoWhileAction());
            RedrawActionList(m);
        }

        private void onMacroInsertDoWhile(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a >= m.Actions.Count) // -1 is valid, will insert @ top
                return;

            MacroInsertDoWhile ins = new MacroInsertDoWhile(m, a);
            if (ins.ShowDialog(this) == DialogResult.OK)
                RedrawActionList(m);
        }

        private void OnMacroActionMoveUp(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            ;
            if (m == null)
                return;

            int move = actionList.SelectedIndex;
            if (move > 0 && move < m.Actions.Count)
            {
                MacroAction a = (MacroAction) m.Actions[move - 1];
                m.Actions[move - 1] = m.Actions[move];
                m.Actions[move] = a;

                RedrawActionList(m);
                actionList.SelectedIndex = move - 1;
            }
        }

        private void OnMacroActionMoveDown(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            ;
            if (m == null)
                return;

            int move = actionList.SelectedIndex;
            if (move + 1 < m.Actions.Count)
            {
                MacroAction a = (MacroAction) m.Actions[move + 1];
                m.Actions[move + 1] = m.Actions[move];
                m.Actions[move] = a;

                RedrawActionList(m);
                actionList.SelectedIndex = move + 1;
            }
        }


        private void OnMacroKeyMoveUp()
        {
            Macro m = GetMacroSel();
            ;
            if (m == null)
                return;

            int move = actionList.SelectedIndex;
            if (move > 0 && move < m.Actions.Count)
            {
                MacroAction a = (MacroAction) m.Actions[move - 1];
                m.Actions[move - 1] = m.Actions[move];
                m.Actions[move] = a;

                RedrawActionList(m);
            }
        }

        private void OnMacroKeyMoveDown()
        {
            Macro m = GetMacroSel();
            ;
            if (m == null)
                return;

            int move = actionList.SelectedIndex;
            if (move + 1 < m.Actions.Count)
            {
                MacroAction a = (MacroAction) m.Actions[move + 1];
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

        private MacroAction _macroActionToCopy;

        private void onMacroCopyLine(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a < 0 || a >= m.Actions.Count)
                return;

            _macroActionToCopy = (MacroAction) m.Actions[a];
        }

        private void onMacroPasteLine(object sender, System.EventArgs e)
        {
            if (_macroActionToCopy == null)
                return;

            Macro m = GetMacroSel();
            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a < 0 || a >= m.Actions.Count)
                return;

            m.Actions.Insert(a + 1, _macroActionToCopy);

            RedrawActionList(m);

            actionList.SelectedIndex = a + 1;

            onMacroSave(sender, e);
            onMacroReload(sender, e);
        }

        private void onMacroActionDelete(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();

            if (m == null)
                return;

            int a = actionList.SelectedIndex;
            if (a < 0 || a >= m.Actions.Count)
                return;

            if (MessageBox.Show(this, Language.Format(LocString.DelConf, m.Actions[a].ToString()), "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m.Actions.RemoveAt(a);
                actionList.Items.RemoveAt(a);
            }
        }

        private void onMacroBegRecHere(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();

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
            Macro m = GetMacroSel();

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
            Macro m = GetMacroSel();

            if (m == null)
                return;

            m.Load();
            RedrawActionList(m);
        }

        private void onMacroSave(object sender, System.EventArgs e)
        {
            Macro m = GetMacroSel();

            if (m == null)
                return;

            m.Save();
            RedrawActionList(m);
        }

        private void onMacroInsertSetLT(object sender, EventArgs e)
        {
            Macro m = GetMacroSel();

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
            Macro m = GetMacroSel();

            if (m == null)
                return;
            m.Loop = loopMacro.Checked;
        }

        private void jump2SearchEx_Click(object sender, System.EventArgs e)
        {
            tabs.SelectedTab = agentsTab;
            agentList.SelectedItem = SearchExemptionAgent.Instance;
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

            if (!File.Exists(Path.Combine(Config.GetInstallDirectory(), "ImgurUploads.csv")))
                return;

            string[] lines = File.ReadAllLines(Path.Combine(Config.GetInstallDirectory(), "ImgurUploads.csv"));

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
                if (MessageBox.Show(this,
                        "Are you sure you want to delete this from Imgur? It will permanently be removed and cannot be undone.",
                        "Delete Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                using (var w = new WebClient()
                ) //HttpClient would be preferred, but I didn't want to add a new package to Razor
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
                MessageBox.Show(this, ex.Message, "Error deleting image on Imgur", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
                MessageBox.Show(this, Language.Format(LocString.FileNotFoundA1, file), "File Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            string file = Path.Combine(Config.GetString("CapPath"), (string) screensList.SelectedItem);


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
                        {"image", Convert.ToBase64String(File.ReadAllBytes(file))}
                    };

                    byte[] response = w.UploadValues("https://api.imgur.com/3/upload", values);

                    dynamic jsonResponse = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(response));

                    if (jsonResponse["status"] == 200)
                    {
                        m_LastImgurUrl = jsonResponse["data"].link.Value;

                        imgurUploads.Invoke((MethodInvoker) delegate
                        {
                            // Running on the UI thread
                            LogImgurUpload(m_LastImgurUrl, jsonResponse["data"].deletehash.Value);

                            if (Config.GetBool("ScreenshotUploadNotifications"))
                            {
                                m_NotifyIcon.ShowBalloonTip(2000, "Screenshot Uploaded",
                                    "Click here to view in your browser.", ToolTipIcon.Info);
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
                MessageBox.Show(this, ex.Message, "Error Uploading to Imgur", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
                string path = Path.Combine(Config.GetInstallDirectory(), "ImgurUploads.csv");
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine($"{url},{deleteHash},{DateTime.Now}");
                }

                ReloadImgurUploadList();
            }
            catch
            {
                //ignore
            }
        }

        private void SaveImgurUploadsToLog()
        {
            try
            {
                string path = Path.Combine(Config.GetInstallDirectory(), "ImgurUploads.csv");

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    foreach (ImgurUpload upload in m_ImgurUploads)
                    {
                        sw.WriteLine($"{upload.Url},{upload.DeleteHash},{upload.UploadTime}");
                    }
                }

                ReloadImgurUploadList();
            }
            catch
            {
            }
        }

        private void DeleteScreenCap(object sender, System.EventArgs e)
        {
            int sel = screensList.SelectedIndex;
            if (sel == -1)
                return;

            string file = Path.Combine(Config.GetString("CapPath"), (string) screensList.SelectedItem);
            if (MessageBox.Show(this, Language.Format(LocString.DelConf, file), "Delete Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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
            if (MessageBox.Show(this, Language.Format(LocString.Confirm, dir), "Delete Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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
                MessageBox.Show(this,
                    Language.Format(LocString.FileDelError, failed, failed != 1 ? "s" : "", sb.ToString()), "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                System.Diagnostics.Process.Start(site); //"iexplore", site );
            }
            catch
            {
                MessageBox.Show(String.Format("Unable to open browser to '{0}'", site), "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
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
                /*if (!this.ShowInTaskbar)
                    MessageBox.Show(this, Language.GetString(LocString.NextRestart), "Notice", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);*/
            }
        }

        private void systray_CheckedChanged(object sender, System.EventArgs e)
        {
            if (systray.Checked)
            {
                taskbar.Checked = false;
                Config.SetProperty("Systray", true);
                /*if (this.ShowInTaskbar)
                    MessageBox.Show(this, Language.GetString(LocString.NextRestart), "Notice", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);*/
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
                if (Config.GetBool("ShowInRazorTitleBar") && !string.IsNullOrEmpty(razorTitleBar.Text))
                {
                    Text = razorTitleBar.Text.Replace("{name}", World.Player.Name).Replace("{shard}", World.ShardName)
                        .Replace("{version}", Engine.Version).Replace("{profile}", Config.CurrentProfile.Name)
                        .Replace("{account}", World.AccountName);
                }
                else
                {
                    string title = $"{World.Player.Name} ({World.ShardName}) - {str}";
                    Text = title;
                }
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
                    if (Config.GetBool("ShowInRazorTitleBar") && !string.IsNullOrEmpty(razorTitleBar.Text))
                    {
                        m_NotifyIcon.Text = razorTitleBar.Text.Replace("{name}", World.Player.Name)
                            .Replace("{shard}", World.ShardName)
                            .Replace("{version}", Engine.Version);
                    }
                    else
                    {
                        m_NotifyIcon.Text = String.Format("Razor - {0} ({1})", World.Player.Name, World.ShardName);
                    }
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
            // In CUO, this can cause an error.
            try
            {
                Platform.SetForegroundWindow(Handle);
                Platform.BringToFront(Handle);
            }
            catch
            {
                BringToFront();
            }

            try
            {
                this.Show();
            }
            catch
            {
            }

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
            m_NotifyIcon.Visible = false;
            m_NotifyIcon.Dispose();
            m_CanClose = true;
            this.Close();
        }

        private void titlebarImages_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("TitlebarImages", titlebarImages.Checked);
            Client.Instance.RequestTitlebarUpdate();
        }

        private void highlightSpellReags_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("HighlightReagents", highlightSpellReags.Checked);
            Client.Instance.RequestTitlebarUpdate();
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
                if (MessageBox.Show(this, Language.GetString(LocString.PWWarn), "Security Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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
                    MessageBox.Show(this, "Unable to load that language.", "Load Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
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
            //HotKey.KeyDown(e.KeyData);
        }

        private void MainForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //HotKey.KeyDown(e.KeyData);
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
            string str = (string) clientPrio.SelectedItem;
            Config.SetProperty("ClientPrio", str);
            try
            {
                Client.Instance.ClientProcess.PriorityClass =
                    (System.Diagnostics.ProcessPriorityClass) Enum.Parse(
                        typeof(System.Diagnostics.ProcessPriorityClass), str, true);
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
            Client.Instance.RequestStatbarPatch(preAOSstatbar.Checked);
            if (World.Player != null && !m_Initializing)
                MessageBox.Show(this, "Close and re-open your status bar for the change to take effect.",
                    "Status Window Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                DressList list = (DressList) dressList.SelectedItem;
                if (list == null)
                    return;

                int sel = dressItems.SelectedIndex;
                if (sel < 0 || sel >= list.Items.Count)
                    return;

                if (MessageBox.Show(this, Language.GetString(LocString.DelDressItemQ), "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

        private void autoFriend_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.SetProperty("AutoFriend", autoFriend.Checked);
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

        private Timer m_ResizeTimer = Timer.DelayedCallback(TimeSpan.FromSeconds(1.0), new TimerCallback(ForceSize));

        private static void ForceSize()
        {
            int x, y;

            if (Config.GetBool("ForceSizeEnabled"))
            {
                x = Config.GetInt("ForceSizeX");
                y = Config.GetInt("ForceSizeY");

                if (x > 100 && x < 2000 && y > 100 && y < 2000)
                    Client.Instance.SetGameSize(x, y);
                else
                    MessageBox.Show(Engine.MainWindow, Language.GetString(LocString.ForceSizeBad), "Bad Size",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                    MessageBox.Show(this, Language.GetString(LocString.ForceSizeBad), "Bad Size", MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                else
                    Client.Instance.SetGameSize(x, y);
            }
            else
            {
                Client.Instance.SetGameSize(800, 600);
            }

            if (!m_Initializing)
                MessageBox.Show(this, Language.GetString(LocString.ApplyOptionsRequired), "Additional Step",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                Client.Instance.SetNegotiate(negotiate.Checked);
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
            MessageBox.Show(this, Language.GetString(LocString.FeatureDisabledText),
                Language.GetString(LocString.FeatureDisabled), MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                        System.Resources.ResourceManager resources =
                            new System.Resources.ResourceManager(typeof(MainForm));
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
                        newLockBox.Location =
                            new System.Drawing.Point(locked.Location.X + x_off, locked.Location.Y + y_off);
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
                    ((Control) box.Tag).Enabled = true;
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
                    ((Control) box.Tag).Enabled = true;
            }

            m_LockBoxes.Clear();

            if (!Client.Instance.AllowBit(FeatureBit.SmartLT))
                LockControl(this.smartLT);

            if (!Client.Instance.AllowBit(FeatureBit.RangeCheckLT))
                LockControl(this.rangeCheckLT);

            if (!Client.Instance.AllowBit(FeatureBit.AutoOpenDoors))
                LockControl(this.autoOpenDoors);

            if (!Client.Instance.AllowBit(FeatureBit.UnequipBeforeCast))
                LockControl(this.spellUnequip);

            if (!Client.Instance.AllowBit(FeatureBit.AutoPotionEquip))
                LockControl(this.potionEquip);

            if (!Client.Instance.AllowBit(FeatureBit.BlockHealPoisoned))
                LockControl(this.blockHealPoison);

            if (!Client.Instance.AllowBit(FeatureBit.LoopingMacros))
                LockControl(this.loopMacro);

            if (!Client.Instance.AllowBit(FeatureBit.OverheadHealth))
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
                    MapWindow = new MapUO.MapWindow();

                MapWindow.SafeAction(s =>
                {
                    s.Show();
                    s.BringToFront();
                });
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

            string profileToClone = (string) profiles.Items[profiles.SelectedIndex];

            if (InputBox.Show(this, Language.GetString(LocString.EnterProfileName),
                Language.GetString(LocString.EnterAName)))
            {
                string newProfileName = InputBox.GetString();
                if (string.IsNullOrEmpty(newProfileName) ||
                    newProfileName.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                    newProfileName.IndexOfAny(m_InvalidNameChars) != -1)
                {
                    MessageBox.Show(this, Language.GetString(LocString.InvalidChars),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                File.Copy(Path.Combine(Config.GetUserDirectory("Profiles"), $"{profileToClone}.xml"),
                    Path.Combine(Config.GetUserDirectory("Profiles"), $"{newProfileName}.xml"));

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
                        foreach (TreeNode _subChildNode in _childNode.Nodes
                        ) // we want all these sub child nodes to be filters
                        {
                            if (_subChildNode.Text.ToLower().Contains(filterHotkeys.Text.ToLower()))
                            {
                                this.hotkeyTree.Nodes.Add((TreeNode) _subChildNode.Clone());
                            }

                            if (_subChildNode.Nodes.Count > 0) // Just in case
                            {
                                foreach (TreeNode _subSubChildNode in _subChildNode.Nodes)
                                {
                                    if (_subSubChildNode.Text.ToLower().Contains(filterHotkeys.Text.ToLower()))
                                    {
                                        this.hotkeyTree.Nodes.Add((TreeNode) _subSubChildNode.Clone());
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

        private void filterMacros_TextChanged(object sender, EventArgs e)
        {
            this.macroTree.BeginUpdate();
            this.macroTree.Nodes.Clear();

            if (filterMacros.Text != string.Empty)
            {
                foreach (TreeNode _parentNode in _macroTreeViewCache.Nodes) // We won't filter on the top parent domain
                {
                    if (_parentNode.Text.ToLower().Contains(filterMacros.Text.ToLower()))
                    {
                        macroTree.Nodes.Add((TreeNode) _parentNode.Clone());
                    }

                    if (_parentNode.Nodes.Count > 0) // Just in case
                    {
                        foreach (TreeNode _subSubChildNode in _parentNode.Nodes)
                        {
                            if (_subSubChildNode.Text.ToLower().Contains(filterMacros.Text.ToLower()))
                            {
                                this.macroTree.Nodes.Add((TreeNode) _subSubChildNode.Clone());
                            }
                        }
                    }
                }
            }
            else
            {
                MacroManager.DisplayTo(macroTree);
            }

            //enables redrawing tree after all objects have been added
            this.macroTree.EndUpdate();
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
                MessageBox.Show(this, ex.Message, "Unable to open directory", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void createBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string backupTime = $"{DateTime.Now:yyyyMMdd-HHmmss}";
                string backupDir = Path.Combine(Config.GetAppSetting<string>("BackupPath"), backupTime);
                ;

                if (string.IsNullOrEmpty(backupDir))
                    return;

                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }

                // Backup the macros
                Directory.CreateDirectory(Path.Combine(backupDir, "Macros"));

                // Create folders
                var macrosDirectory = Path.Combine(Config.GetUserDirectory(), "Macros");
                foreach (string dirPath in Directory.GetDirectories(macrosDirectory, "*",
                    SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(macrosDirectory,
                        Path.Combine(backupDir, "Macros")));
                }

                // Copy macros
                foreach (string newPath in Directory.GetFiles(macrosDirectory, "*.*",
                    SearchOption.AllDirectories))
                {
                    File.Copy(newPath,
                        newPath.Replace(macrosDirectory,
                            Path.Combine(backupDir, "Macros")), true);
                }

                // Backup the profiles
                Directory.CreateDirectory(Path.Combine(backupDir, "Profiles"));
                var profilesDirectory = Path.Combine(Config.GetUserDirectory(), "Profiles");

                foreach (string newPath in Directory.GetFiles(profilesDirectory, "*.*",
                    SearchOption.AllDirectories))
                {
                    File.Copy(newPath,
                        newPath.Replace(profilesDirectory,
                            Path.Combine(backupDir, "Profiles")), true);
                }

                MessageBox.Show(this, $"Backup created: {backupDir}", "Razor Backup", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Config.SetAppSetting("BackupTime", DateTime.Now.ToString());
                lastBackup.Text = $"Last Backup: {Config.GetAppSetting<string>("BackupTime")}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Unable to create backup", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void titleBarParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (titleBarParams.SelectedItem.ToString().Contains("{") && titleBarParams.Focused)
            {
                titleStr.AppendText($" {titleBarParams.SelectedItem} ");
            }
        }

        private void OnMacroVariableAddTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            TargetInfo t = new TargetInfo
            {
                Gfx = gfx,
                Serial = serial,
                Type = (byte) (ground ? 1 : 0),
                X = pt.X,
                Y = pt.Y,
                Z = pt.Z
            };

            if (InputBox.Show(this, Language.GetString(LocString.NewMacroVariable),
                Language.GetString(LocString.EnterAName)))
            {
                string name = InputBox.GetString();

                foreach (MacroVariables.MacroVariable mV in MacroVariables.MacroVariableList
                )
                {
                    if (mV.Name.ToLower().Equals(name.ToLower()))
                    {
                        MessageBox.Show(this, "Pick a unique Macro Variable name and try again",
                            "New Macro Variable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                MacroVariables.MacroVariableList.Add(
                    new MacroVariables.MacroVariable(name, t));

                // Save and reload the macros and vars
                MacroManager.DisplayMacroVariables(macroVariables);
            }

            Engine.MainWindow.ShowMe();
        }

        private void OnMacroVariableReTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            TargetInfo t = new TargetInfo
            {
                Gfx = gfx,
                Serial = serial,
                Type = (byte) (ground ? 1 : 0),
                X = pt.X,
                Y = pt.Y,
                Z = pt.Z
            };

            MacroVariables.MacroVariableList[macroVariables.SelectedIndex].TargetInfo = t;

            // Save and reload the macros and vars
            MacroManager.DisplayMacroVariables(macroVariables);

            Engine.MainWindow.ShowMe();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.uor-razor.com");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.uorenaissance.com");
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
            Client.Instance.SetSmartCPU(false);
        }

        private void lightLevelBar_Scroll(object sender, EventArgs e)
        {
            if (Client.Instance.AllowBit(FeatureBit.LightFilter) && World.Player != null)
            {
                byte selectedLightLevel = Convert.ToByte(lightLevelBar.Maximum - lightLevelBar.Value);

                World.Player.LocalLightLevel = 0;
                World.Player.GlobalLightLevel = selectedLightLevel;

                Client.Instance.SendToClient(new GlobalLightLevel(selectedLightLevel));
                Client.Instance.SendToClient(new PersonalLightLevel(World.Player));

                double percent = Math.Round((lightLevelBar.Value / (double) lightLevelBar.Maximum) * 100.0);

                lightLevel.Text = $"Light: {percent}%";

                Config.SetProperty("LightLevel", (int) selectedLightLevel);
            }
        }

        private BoatWindow _boatWindowForm = null;

        private void boatControl_Click(object sender, EventArgs e)
        {
            if (_boatWindowForm != null)
            {
                _boatWindowForm.Show();
            }
            else
            {
                _boatWindowForm = new BoatWindow();
                _boatWindowForm.Show();
            }
        }

        private void showTargetMessagesOverChar_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowTargetSelfLastClearOverhead", showTargetMessagesOverChar.Checked);
        }

        private void showMessagesOverhead_CheckedChanged(object sender, EventArgs e)
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
                //SetParent( MapWindow.Handle, Client.FindUOWindow() );
                //MapWindow.Owner = (Form)Form.FromHandle( Client.FindUOWindow() );
                MapWindow.Show();
                MapWindow.BringToFront();
            }*/
        }

        private void logSkillChanges_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("LogSkillChanges", logSkillChanges.Checked);
        }

        private OverheadMessages _overMessagesForm = null;

        private void overHeadMessages_Click(object sender, EventArgs e)
        {
            if (_overMessagesForm != null)
            {
                _overMessagesForm.SafeAction(s => s.Show());
            }
            else
            {
                _overMessagesForm = new OverheadMessages();

                _overMessagesForm.SafeAction(s => s.Show());
            }
        }

        private void saveProfile_Click(object sender, EventArgs e)
        {
            if (profiles.SelectedIndex < 0)
                return;

            Config.Save();

            string profileToClone = (string) profiles.Items[profiles.SelectedIndex];
            MessageBox.Show(SplashScreen.Instance,
                $"Saved current settings to profile {Path.Combine(Config.GetUserDirectory("Profiles"), $"{profileToClone}.xml")}",
                "Save Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (World.Player == null)
            {
                GoldPerHourTimer.Stop();
                return;
            }

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

            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                Agent a = agentList.SelectedItem as Agent;

                switch (agentList.SelectedItem)
                {
                    case RestockAgent _:
                        a.OnButtonPress(3);
                        break;
                    case BuyAgent _:
                        a.OnButtonPress(2);
                        break;
                }
            }

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

        private ContainerLabels _containerLabelsForm = null;

        private void containerLabels_Click(object sender, EventArgs e)
        {
            if (_containerLabelsForm != null)
            {
                _containerLabelsForm.Show();
            }
            else
            {
                _containerLabelsForm = new ContainerLabels();
                _containerLabelsForm.Show();
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

            if (seasonList.SelectedIndex < 0)
                return;

            Config.SetProperty("Season", seasonList.SelectedIndex);

            if (seasonList.SelectedIndex < 5)
            {
                Client.Instance.ForceSendToClient(new SeasonChange(seasonList.SelectedIndex, true));
            }
        }

        private void blockPartyInvites_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("BlockPartyInvites", blockPartyInvites.Checked);
        }

        private void blockTradeRequests_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("BlockTradeRequests", blockTradeRequests.Checked);
        }

        private void autoAcceptParty_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("AutoAcceptParty", autoAcceptParty.Checked);
        }

        private void setMaxLightLevel_Click(object sender, EventArgs e)
        {
            int lightLevel = lightLevelBar.Maximum - lightLevelBar.Value;

            if (lightLevel > Config.GetInt("MinLightLevel"))
            {
                MessageBox.Show(this,
                    $"Selected maximum light level {lightLevel} exceeds minimum light level {Config.GetInt("MinLightLevel")}",
                    "Set Max Light Level", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Config.SetProperty("MaxLightLevel", lightLevel);
            }
        }

        private void setMinLightLevel_Click(object sender, EventArgs e)
        {
            int lightLevel = lightLevelBar.Maximum - lightLevelBar.Value;

            if (lightLevel < Config.GetInt("MaxLightLevel"))
            {
                MessageBox.Show(this,
                    $"Selected minimum light level {lightLevel} exceeds maximum light level {Config.GetInt("MaxLightLevel")}",
                    "Set Min Light Level", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Config.SetProperty("MinLightLevel", lightLevel);
            }
        }

        private void minMaxLightLevel_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("MinMaxLightLevelEnabled", minMaxLightLevel.Checked);
        }

        private void showStaticWalls_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowStaticWalls", showStaticWalls.Checked);
        }

        private void showStaticWallLabels_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowStaticWallLabels", showStaticWallLabels.Checked);
        }

        private void showTextTargetIndicator_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowTextTargetIndicator", showTextTargetIndicator.Checked);
        }

        private void showAttackTargetNewOnly_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowAttackTargetNewOnly", showAttackTargetNewOnly.Checked);
        }

        private void addMacroVariable_Click(object sender, EventArgs e)
        {
            if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                return;

            //switch (macroVariableTypeList.SelectedIndex)
            //{
            //    case 0:
            Targeting.OneTimeTarget(OnMacroVariableAddTarget);
            //        break;
            //    case 1:
            //        Targeting.OneTimeTarget(OnDoubleClickAddTarget);
            //        break;
            //}


            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void insertMacroVariable_Click(object sender, EventArgs e)
        {
            if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                return;

            Macro m = GetMacroSel();

            if (m == null)
                return;

            int a = actionList.SelectedIndex;

            if (macroVariables.SelectedIndex < 0)
            {
                return;
            }

            string[] macroVariableName = {MacroVariables.MacroVariableList[macroVariables.SelectedIndex].Name};

            switch (macroVariableTypeList.SelectedIndex)
            {
                case 0:
                    m.Actions.Insert(a + 1, new AbsoluteTargetVariableAction(macroVariableName));
                    break;
                case 1:
                    m.Actions.Insert(a + 1, new DoubleClickVariableAction(macroVariableName));
                    break;
                case 2:
                    m.Actions.Insert(a + 1, new SetMacroVariableTargetAction(macroVariableName));
                    break;
            }

            RedrawActionList(m);
        }

        private void retargetMacroVariable_Click(object sender, EventArgs e)
        {
            if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                return;

            if (macroVariables.SelectedIndex < 0)
                return;

            Targeting.OneTimeTarget(OnMacroVariableReTarget);

            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void removeMacroVariable_Click(object sender, EventArgs e)
        {
            if (MacroManager.Playing || MacroManager.Recording || World.Player == null)
                return;

            if (macroVariables.SelectedIndex < 0)
                return;

            MacroVariables.MacroVariableList.RemoveAt(macroVariables.SelectedIndex);

            // Save and reload the macros and vars
            MacroManager.Save();
            MacroManager.DisplayMacroVariables(macroVariables);
        }

        private void macroVariableTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MacroManager.DisplayMacroVariables(macroVariables);
        }

        private void filterDragonGraphics_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("FilterDragonGraphics", filterDragonGraphics.Checked);
        }

        private void dragonAnimationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dragonAnimationList.SelectedIndex < 0)
                    return;

                Config.SetProperty("DragonGraphic",
                    Convert.ToInt32(_animationData[dragonAnimationList.SelectedIndex].body));
            }
            catch
            {
                MessageBox.Show(this, "Unable to find animation in file", "Animation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void drakeAnimationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (drakeAnimationList.SelectedIndex < 0)
                    return;

                Config.SetProperty("DrakeGraphic",
                    Convert.ToInt32(_animationData[drakeAnimationList.SelectedIndex].body));
            }
            catch
            {
                MessageBox.Show(this, "Unable to find animation in file", "Animation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void trackDps_CheckedChanged(object sender, EventArgs e)
        {
            if (trackDps.Checked)
            {
                DamageTracker.Start();
            }
            else
            {
                DamageTracker.Stop();
            }
        }

        private void showDamageDealt_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowDamageDealt", showDamageDealt.Checked);
        }

        private void damageDealtOverhead_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowDamageDealtOverhead", damageDealtOverhead.Checked);
        }

        private void damageTakenOverhead_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowDamageTakenOverhead", damageTakenOverhead.Checked);
        }

        private void showDamageTaken_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowDamageTaken", showDamageTaken.Checked);
        }

        private void filterDrakeGraphics_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("FilterDrakeGraphics", filterDrakeGraphics.Checked);
        }

        private void showInRazorTitleBar_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowInRazorTitleBar", showInRazorTitleBar.Checked);

            UpdateTitle();
        }

        private void razorTitleBar_TextChanged(object sender, EventArgs e)
        {
            Config.SetProperty("RazorTitleBarText", razorTitleBar.Text.TrimEnd());
            if (Config.GetBool("ShowInRazorTitleBar"))
                UpdateTitle();
        }

        private void razorTitleBarKey_Click(object sender, EventArgs e)
        {
            //Replace("{name}", World.Player.Name).Replace("{shard}", World.ShardName)
            //.Replace("{version}", Engine.Version).Replace("{profile}", Config.CurrentProfile.Name).Replace("{account}", World.AccountName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("You can insert these variables into the Razor Title Bar to display specific information.");
            sb.AppendLine(string.Empty);
            sb.AppendLine("{name} - Character name");
            sb.AppendLine("{shard} - Shard/Server name");
            sb.AppendLine("{version} - Razor version");
            sb.AppendLine("{profile} - Selected profile name");
            sb.AppendLine("{account} - Account name");

            MessageBox.Show(this, sb.ToString(), "Razor Title Bar Variables", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void linkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.uor-razor.com/help/");
        }

        private void enableUOAAPI_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("EnableUOAAPI", enableUOAAPI.Checked);
        }

        private void openUOAM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Config.GetString("UOAMPath")))
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "Executables (*.exe)|*.exe";
                file.FilterIndex = 1;
                file.Multiselect = false;

                if (file.ShowDialog(this) == DialogResult.OK)
                {
                    Config.SetProperty("UOAMPath", file.FileName);
                    Process.Start(Config.GetString("UOAMPath"));
                }
            }
            else
            {
                Process.Start(Config.GetString("UOAMPath"));
            }
        }

        private void openUltimaMapper_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Config.GetString("UltimaMapperPath")))
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "Executables (*.exe)|*.exe";
                file.FilterIndex = 1;
                file.Multiselect = false;

                if (file.ShowDialog(this) == DialogResult.OK)
                {
                    Config.SetProperty("UltimaMapperPath", file.FileName);
                    Process.Start(Config.GetString("UltimaMapperPath"));
                }
            }
            else
            {
                Process.Start(Config.GetString("UltimaMapperPath"));
            }
        }

        private void setBackupFolder_Click(object sender, EventArgs e)
        {
            //Config.SetAppSetting("UODataDir", dataDir.Text);

            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Select Backup Folder";
            folder.SelectedPath = Config.GetAppSetting<string>("BackupPath");
            folder.ShowNewFolderButton = true;

            if (folder.ShowDialog(this) == DialogResult.OK)
            {
                Config.SetAppSetting("BackupPath", folder.SelectedPath);
            }
        }

        private void openBackupFolder_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Config.GetAppSetting<string>("BackupPath"));
            }
            catch
            {
            }
        }

        private void targetIndictorFormat_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(targetIndictorFormat.Text))
            {
                Config.SetProperty("TargetIndicatorFormat", "* Target *");
                targetIndictorFormat.Text = "* Target *";
            }

            Config.SetProperty("TargetIndicatorFormat", targetIndictorFormat.Text);
        }

        private void nextPrevIgnoresFriends_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("NextPrevTargetIgnoresFriends", nextPrevIgnoresFriends.Checked);
        }

        private void stealthStepsFormat_TextChanged(object sender, EventArgs e)
        {
            Config.SetProperty("StealthStepsFormat", stealthStepsFormat.Text);
        }

        private void showFriendOverhead_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowFriendOverhead", showFriendOverhead.Checked);
        }

        private void dispDeltaOverhead_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("DisplaySkillChangesOverhead", dispDeltaOverhead.Checked);
        }

        private void linkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/markdwags/Razor");
        }

        /// <summary>
        /// Quickly disable UI elements not used when ClassicUO is the client
        /// </summary>
        public void DisableCUOFeatures()
        {
            forceSizeX.SafeAction(s =>
            {
                s.Enabled = false;
                s.Text = "0";
            });

            forceSizeY.SafeAction(s =>
            {
                s.Enabled = false;
                s.Text = "0";
            });

            gameSize.SafeAction(s =>
            {
                s.Enabled = false;
                s.Checked = false;
            });

            rememberPwds.SafeAction(s =>
            {
                s.Enabled = false;
                s.Checked = false;
            });

            highlightSpellReags.SafeAction(s =>
            {
                s.Enabled = false;
                s.Checked = false;
            });

            showNotoHue.SafeAction(s =>
            {
                s.Enabled = false;
                s.Checked = false;
            });

            titlebarImages.SafeAction(s =>
            {
                s.Enabled = false;
                s.Checked = false;
            });

            showWelcome.SafeAction(s =>
            {
                s.Enabled = false;
                s.Checked = false;
            });

            showHealthOH.SafeAction(s =>
            {
                s.Enabled = false;
                s.Checked = false;
            });

            healthFmt.SafeAction(s => { s.Enabled = false; });
        }

        private void macroActionDelay_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("MacroActionDelay", macroActionDelay.Checked);

            if (m_Initializing)
                return;

            MessageBox.Show(this, Language.GetString(LocString.NextRestart), "Notice", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void autoDoorWhenHidden_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("AutoOpenDoorWhenHidden", autoOpenDoorWhenHidden.Checked);
        }

        private void disableMacroPlayFinish_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("DisableMacroPlayFinish", disableMacroPlayFinish.Checked);
        }

        private void showBandageTimer_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowBandageTimer", showBandageTimer.Checked);
        }

        private void bandageTimerLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowBandageTimerLocation", bandageTimerLocation.SelectedIndex);
        }

        private void onlyShowBandageTimerSeconds_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("OnlyShowBandageTimerEvery", onlyShowBandageTimerSeconds.Checked);
        }

        private void bandageTimerSeconds_TextChanged(object sender, EventArgs e)
        {
            Config.SetProperty("OnlyShowBandageTimerSeconds", Utility.ToInt32(bandageTimerSeconds.Text.Trim(), 1));
        }

        private void bandageTimerFormat_TextChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowBandageTimerFormat", bandageTimerFormat.Text);

            if (string.IsNullOrEmpty(friendOverheadFormat.Text))
            {
                Config.SetProperty("ShowBandageTimerFormat", "Bandage: {count}s");
                bandageTimerFormat.Text = "Bandage: {count}s";
            }

            Config.SetProperty("ShowBandageTimerFormat", bandageTimerFormat.Text);
        }

        private void setBandageHue_Click(object sender, EventArgs e)
        {
            lblBandageCountFormat.SafeAction(s => { SetHue(s, "ShowBandageTimerHue"); });
        }

        private void friendRemoveSelected_Click(object sender, EventArgs e)
        {
            if (friendsGroup.SelectedIndex < 0 || friendsList.SelectedIndex < 0)
                return;

            FriendsManager.RemoveFriend((FriendsManager.FriendGroup) friendsGroup.SelectedItem,
                friendsList.SelectedIndex);
        }

        private void friendsGroupAdd_Click(object sender, EventArgs e)
        {
            if (InputBox.Show(this, "Add Friend Group", "Enter the name of this new Friend Group"))
            {
                string name = InputBox.GetString();

                if (!string.IsNullOrEmpty(name) && !FriendsManager.FriendsGroupExists(name))
                {
                    FriendsManager.AddFriendGroup(name);
                }
                else
                {
                    MessageBox.Show(this, "Invalid name, or friends group already exists", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void friendsGroupRemove_Click(object sender, EventArgs e)
        {
            if (friendsGroup.SelectedIndex < 0)
                return;

            if (MessageBox.Show(this, Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (FriendsManager.DeleteFriendGroup((FriendsManager.FriendGroup) friendsGroup.SelectedItem))
                {
                    FriendsManager.RedrawGroup();

                    if (friendsGroup.Items.Count > 0)
                    {
                        friendsGroup.SafeAction(s => s.SelectedIndex = 0);
                    }
                    else
                    {
                        friendsList.SafeAction(s => s.Items.Clear());
                    }
                }
            }
        }

        private void friendsGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (friendsGroup.SelectedIndex < 0)
                return;

            var friendGroup = (FriendsManager.FriendGroup) friendsGroup.SelectedItem;

            friendsListEnabled.SafeAction(s => s.Checked = friendsGroup.Enabled);

            FriendsManager.RedrawList(friendGroup);
        }

        private void friendClearList_Click(object sender, EventArgs e)
        {
            if (friendsGroup.SelectedIndex < 0)
                return;

            if (MessageBox.Show(this, Language.GetString(LocString.Confirm), Language.GetString(LocString.ClearList),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FriendsManager.FriendGroup friendGroup = (FriendsManager.FriendGroup) friendsGroup.SelectedItem;
                friendGroup.Friends.Clear();

                FriendsManager.RedrawList(friendGroup);
            }
        }

        private void friendsListEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (friendsGroup.SelectedIndex < 0)
                return;

            FriendsManager.EnableFriendsGroup((FriendsManager.FriendGroup) friendsGroup.SelectedItem,
                friendsListEnabled.Checked);
        }

        private void friendOverheadFormat_TextChanged(object sender, EventArgs e)
        {
            //FriendOverheadFormat
            if (string.IsNullOrEmpty(friendOverheadFormat.Text))
            {
                Config.SetProperty("FriendOverheadFormat", "[Friend]");
                targetIndictorFormat.SafeAction(s => s.Text = "[Friend]");
            }

            Config.SetProperty("FriendOverheadFormat", friendOverheadFormat.Text);
        }

        private void setFriendsFormatHue_Click(object sender, EventArgs e)
        {
            friendFormat.SafeAction(s => { SetHue(s, "FriendOverheadFormatHue"); });
        }

        private void friendAddTarget_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (World.Player == null)
                return;

            if (friendsGroup.SelectedIndex < 0)
                return;

            if ((e.Button & MouseButtons.Right) != 0)
            {
                ContextMenu menu = new ContextMenu();
                menu.MenuItems.Add(Language.GetString(LocString.AddAllMobileFriends),
                    new EventHandler(onAddAllMobilesAsFriends));
                menu.MenuItems.Add(Language.GetString(LocString.AddAllHumanoidsAsFriends),
                    new EventHandler(onAddAllHumanoidsAsFriends));

                menu.Show(friendAddTarget, new Point(e.X, e.Y));
            }
            else
            {
                FriendsManager.OnTargetAddFriend((FriendsManager.FriendGroup) friendsGroup.SelectedItem);
            }
        }

        private void onAddAllMobilesAsFriends(object sender, System.EventArgs e)
        {
            if (friendsGroup.SelectedIndex < 0)
                return;

            if (World.Player == null)
                return;

            FriendsManager.FriendGroup friendGroup = (FriendsManager.FriendGroup) friendsGroup.SelectedItem;
            friendGroup.AddAllMobileAsFriends();
        }

        private void onAddAllHumanoidsAsFriends(object sender, System.EventArgs e)
        {
            if (friendsGroup.SelectedIndex < 0)
                return;

            if (World.Player == null)
                return;

            FriendsManager.FriendGroup friendGroup = (FriendsManager.FriendGroup) friendsGroup.SelectedItem;
            friendGroup.AddAllHumanoidsAsFriends();
        }

        private void friendsList_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (friendsList.SelectedIndex < 0 || friendsGroup.SelectedIndex < 0)
                    return;

                FriendsManager.RemoveFriend((FriendsManager.FriendGroup) friendsGroup.SelectedItem,
                    friendsList.SelectedIndex);
            }
        }

        private void friendsList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (friendsGroup.SelectedIndex < 0)
                return;

            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();
                menu.MenuItems.Add("Import 'Friends' from clipboard", new EventHandler(onImportFriends));
                menu.MenuItems.Add("Export 'Friends' to clipboard", new EventHandler(onExportFriends));

                menu.Show(friendsList, new Point(e.X, e.Y));
            }
        }

        private void onImportFriends(object sender, System.EventArgs e)
        {
            if (friendsGroup.SelectedIndex < 0)
                return;

            try
            {
                if (Clipboard.GetText().Contains("!Razor.Friends.Import"))
                {
                    List<string> friendsImport = Clipboard.GetText()
                        .Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None).ToList();

                    friendsImport.RemoveAt(0);

                    foreach (string import in friendsImport)
                    {
                        if (string.IsNullOrEmpty(import))
                            continue;

                        string[] friend = import.Split('#');

                        FriendsManager.FriendGroup friendGroup = (FriendsManager.FriendGroup) friendsGroup.SelectedItem;

                        friendGroup.AddFriend(friend[0], Serial.Parse(friend[1]));
                    }

                    Clipboard.Clear();
                }
            }
            catch
            {
            }
        }

        private void onExportFriends(object sender, System.EventArgs e)
        {
            if (friendsGroup.SelectedIndex < 0 || friendsList.Items.Count == 0)
                return;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("!Razor.Friends.Import");


            FriendsManager.FriendGroup friendGroup = (FriendsManager.FriendGroup) friendsGroup.SelectedItem;

            foreach (FriendsManager.Friend friend in friendGroup.Friends)
            {
                sb.AppendLine($"{friend.Name}#{friend.Serial}");
            }


            Clipboard.SetDataObject(sb.ToString(), true);
        }

        private void setTargetIndicatorHue_Click(object sender, EventArgs e)
        {
            lblTargetFormat.SafeAction(s => { SetHue(s, "TargetIndicatorHue"); });
        }

        private void filterSystemMessages_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("FilterSystemMessages", filterSystemMessages.Checked);
        }

        private void filterRazorMessages_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("FilterRazorMessages", filterRazorMessages.Checked);
        }

        private void filterDelaySeconds_TextChanged(object sender, EventArgs e)
        {
            Config.SetProperty("FilterDelay", Utility.ToDouble(filterDelaySeconds.Text.Trim(), 3.5));

            if (Config.GetDouble("FilterDelay") < 0 || Config.GetDouble("FilterDelay") > 20)
            {
                Config.SetProperty("FilterDelay", 3.5);
                filterDelaySeconds.SafeAction(s => s.Text = "3.5");
            }
        }

        private void filterOverheadMessages_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("FilterOverheadMessages", filterOverheadMessages.Checked);
        }

        private void onlyNextPrevBeneficial_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("OnlyNextPrevBeneficial", onlyNextPrevBeneficial.Checked);
        }

        private void friendBeneficialOnly_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("FriendlyBeneficialOnly", friendBeneficialOnly.Checked);
        }

        private void nonFriendlyHarmfulOnly_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("NonFriendlyHarmfulOnly", nonFriendlyHarmfulOnly.Checked);
        }

        private void ShowBandageStart_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowBandageStart", showBandageStart.Checked);
        }

        private void ShowBandageEnd_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowBandageEnd", showBandageEnd.Checked);
        }

        private void BandageStartMessage_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bandageStartMessage.Text))
            {
                Config.SetProperty("BandageStartMessage", "Bandage: Starting");
                bandageStartMessage.SafeAction(s => s.Text = "Bandage: Starting");
            }

            Config.SetProperty("BandageStartMessage", bandageStartMessage.Text);
        }

        private void BandageEndMessage_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bandageEndMessage.Text))
            {
                Config.SetProperty("BandageEndMessage", "Bandage: Ending");
                bandageEndMessage.SafeAction(s => s.Text = "Bandage: Ending");
            }

            Config.SetProperty("BandageEndMessage", bandageEndMessage.Text);
        }

        private BuffDebuff _buffDebuffOptions = null;

        private void BuffDebuffOptions_Click(object sender, EventArgs e)
        {
            if (_buffDebuffOptions != null)
            {
                _buffDebuffOptions.SafeAction(s => s.Show());
            }
            else
            {
                _buffDebuffOptions = new BuffDebuff();

                _buffDebuffOptions.SafeAction(s => s.Show());
            }
        }

        private void CaptureOthersDeathDelay_TextChanged(object sender, EventArgs e)
        {
            Config.SetProperty("CaptureOthersDeathDelay", Utility.ToDouble(captureOthersDeathDelay.Text.Trim(), 0.5));

            if (Config.GetDouble("CaptureOthersDeathDelay") < 0 || Config.GetDouble("CaptureOthersDeathDelay") > 5)
            {
                Config.SetProperty("CaptureOthersDeathDelay", 0.5);
                captureOthersDeathDelay.SafeAction(s => s.Text = "0.5");
            }
        }

        private void CaptureOthersDeath_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("CaptureOthersDeath", captureOthersDeath.Checked);
        }

        private void CaptureOwnDeath_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("CaptureOwnDeath", captureOwnDeath.Checked);
        }

        private void CaptureOwnDeathDelay_TextChanged(object sender, EventArgs e)
        {
            Config.SetProperty("CaptureOwnDeathDelay", Utility.ToDouble(captureOwnDeathDelay.Text.Trim(), 0.5));

            if (Config.GetDouble("CaptureOwnDeathDelay") < 0 || Config.GetDouble("CaptureOwnDeathDelay") > 5)
            {
                Config.SetProperty("CaptureOwnDeathDelay", 0.5);
                captureOwnDeathDelay.SafeAction(s => s.Text = "0.5");
            }
        }

        private void TargetFilterRemove_Click(object sender, EventArgs e)
        {
            if (targetFilter.SelectedIndex < 0)
                return;

            TargetFilterManager.RemoveTargetFilter(targetFilter.SelectedIndex);
        }

        private void TargetFilterClear_Click(object sender, EventArgs e)
        {
            TargetFilterManager.ClearTargetFilters();
        }

        private void TargetFilterEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("TargetFilterEnabled", targetFilterEnabled.Checked);
        }

        /*private void onAddAllMobilesTargetFilter(object sender, System.EventArgs e)
        {
            if (targetFilter.SelectedIndex < 0)
                return;

            if (World.Player == null)
                return;
            
            TargetFilterManager.AddAllMobileAsTargetFilters();
        }

        private void onAddAllMobilesHumanoidTargetFilter(object sender, System.EventArgs e)
        {
            if (targetFilter.SelectedIndex < 0)
                return;

            if (World.Player == null)
                return;

            TargetFilterManager.AddAllHumanoidsAsTargetFilters();
        }*/

        private void TargetFilterAdd_Click(object sender, EventArgs e)
        {
            if (World.Player == null)
                return;

            TargetFilterManager.OnTargetAddTargetFilter();
        }

        private void FriendAddTarget_Click(object sender, EventArgs e)
        {
            if (World.Player == null)
                return;

            if (friendsGroup.SelectedIndex < 0)
                return;

            ((FriendsManager.FriendGroup) friendsGroup.SelectedItem).AddFriendToGroup();
        }

        private TreeNode SearchTreeView(string p_sSearchTerm, TreeNodeCollection p_Nodes)
        {
            foreach (TreeNode node in p_Nodes)
            {
                if (node.Text.Contains(p_sSearchTerm))
                    return node;

                if (node.Nodes.Count > 0)
                {
                    TreeNode child = SearchTreeView(p_sSearchTerm, node.Nodes);
                    if (child != null) return child;
                }
            }

            return null;
        }

        private void SetMacroHotKey_Click(object sender, EventArgs e)
        {
            try
            {
                Engine.MainWindow.SafeAction(s =>
                {
                    Macro sel = GetMacroSel();

                    tabs.SelectedTab = hotkeysTab;

                    TreeNode resultNode = SearchTreeView(sel.GetName(), hotkeyTree.Nodes);

                    if (resultNode != null)
                    {
                        KeyData hk = (KeyData) resultNode.Tag;

                        hotkeyTree.SelectedNode = resultNode;
                        key.Focus();
                    }
                });
            }
            catch
            {
                // ignore
            }
        }

        public void SaveMacroVariables()
        {
            MacroManager.DisplayMacroVariables(macroVariables);
        }

        public void SaveScriptVariables()
        {
            ScriptManager.DisplayScriptVariables(scriptVariables);
        }

        private void filterDaemonGraphics_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("FilterDaemonGraphics", filterDaemonGraphics.Checked);
        }

        private void daemonAnimationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (daemonAnimationList.SelectedIndex < 0)
                    return;

                Config.SetProperty("DaemonGraphic",
                    Convert.ToInt32(_animationData[daemonAnimationList.SelectedIndex].body));
            }
            catch
            {
                MessageBox.Show(this, "Unable to find animation in file", "Animation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void soundFilterEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("SoundFilterEnabled", soundFilterEnabled.Checked);
        }

        private void soundFilterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (soundFilterList.SelectedIndex < 0)
                return;

            SoundMusicManager.Sound sound = (SoundMusicManager.Sound) soundFilterList.SelectedItem;

            if (soundFilterList.GetItemChecked(soundFilterList.SelectedIndex))
            {
                SoundMusicManager.AddSoundFilter(sound);
            }
            else
            {
                SoundMusicManager.RemoveSoundFilter(sound);
            }
        }

        private System.Media.SoundPlayer sp = new System.Media.SoundPlayer();

        private void playSound_Click(object sender, EventArgs e)
        {
            sp.Stop();

            if (soundFilterList.SelectedIndex < 0)
                return;

            SoundMusicManager.Sound sound = (SoundMusicManager.Sound) soundFilterList.SelectedItem;

            if (playInClient.Checked && World.Player != null)
            {
                Client.Instance.SendToClient(new PlaySound(sound.Serial - 1));
            }
            else
            {
                Ultima.UOSound s = Ultima.Sounds.GetSound(sound.Serial - 1);
                using (MemoryStream m = new MemoryStream(s.buffer))
                {
                    sp.Stream = m;
                    sp.Play();
                }
            }
        }

        private void showFilteredSound_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowFilteredSound", showFilteredSound.Checked);
        }

        private void showPlayingSoundInfo_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowPlayingSoundInfo", showPlayingSoundInfo.Checked);
        }

        private void playMusic_Click(object sender, EventArgs e)
        {
            if (playableMusicList.SelectedIndex < 0 || World.Player == null)
                return;

            Client.Instance.SendToClient(new PlayMusic(Convert.ToUInt16(playableMusicList.SelectedIndex)));
        }

        private void showPlayingMusic_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("ShowMusicInfo", showPlayingMusic.Checked);
        }

        private void playScript_Click(object sender, EventArgs e)
        {
            if (ScriptManager.Running)
            {
                ScriptManager.StopScript();
                return;
            }

            if (string.IsNullOrEmpty(scriptEditor.Text))
            {
                return;
            }

            if (Config.GetBool("AutoSaveScriptPlay"))
            {
                SaveScript();
            }

            // We want to play the contents of the script editor
            ScriptManager.PlayScript(scriptEditor.Lines.ToArray());
        }

        public void LockScriptUI(bool enabled)
        {
            Engine.MainWindow.SafeAction(s =>
            {
                scriptEditor.Enabled = !enabled;
                recordScript.Enabled = !enabled;
                setScriptHotkey.Enabled = !enabled;
                scriptList.Enabled = !enabled;

                saveScript.Enabled = !enabled;
                deleteScript.Enabled = !enabled;
                newScript.Enabled = !enabled;
                renameScript.Enabled = !enabled;

                playScript.Text = !enabled ? "Play" : "Stop";
            });
        }

        private void onScriptReload(object sender, System.EventArgs e)
        {
            /*Macro m = GetMacroSel();

            if (m == null)
                return;

            m.Load();
            RedrawActionList(m);*/
        }

        private void onScriptSave(object sender, System.EventArgs e)
        {
            /*Macro m = GetMacroSel();

            if (m == null)
                return;

            m.Save();
            RedrawActionList(m);*/
        }

        private void scriptEditor_LostFocus(object sender, EventArgs e)
        {
            if (Config.GetBool("AutoSaveScript"))
            {
                SaveScript();
            }
        }

        private void SaveScript()
        {
            if (scriptList.SelectedIndex < 0)
            {
                string filePath = $"{ScriptManager.ScriptPath}\\auto-{Guid.NewGuid().ToString().Substring(0, 4)}.razor";

                File.WriteAllText(filePath, scriptEditor.Text);

                ScriptManager.RazorScript script = new ScriptManager.RazorScript
                {
                    Lines = File.ReadAllLines(filePath),
                    Name = Path.GetFileNameWithoutExtension(filePath),
                    Path = filePath
                };

                ScriptManager.RedrawScripts();

                for (int i = 0; i < scriptList.Items.Count; i++)
                {
                    ScriptManager.RazorScript scriptItem = (ScriptManager.RazorScript) scriptList.Items[i];
                    if (scriptItem.Name.Equals(script.Name))
                    {
                        scriptList.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                int curIndex = scriptList.SelectedIndex;

                ScriptManager.RazorScript script = (ScriptManager.RazorScript) scriptList.SelectedItem;
                File.WriteAllText(script.Path, scriptEditor.Text);

                ScriptManager.RedrawScripts();

                scriptList.SelectedIndex = curIndex;
            }
        }

        private void scriptList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (scriptList.SelectedIndex < 0)
                return;

            scriptEditor.Text =
                File.ReadAllText(Path.Combine(ScriptManager.ScriptPath, $"{scriptList.SelectedItem}.razor"));
        }

        private void recordScript_Click(object sender, EventArgs e)
        {
            if (World.Player == null)
                return;

            if (ScriptManager.Recording) // stop recording
            {
                recordScript.Text = "Record";
                ScriptManager.Recording = false;
                scriptEditor.Enabled = true;
                playScript.Enabled = true;
            }
            else //start recording
            {
                recordScript.Text = "Stop";
                ScriptManager.Recording = true;
                scriptEditor.Enabled = false;
                playScript.Enabled = false;
            }
        }

        private void newScript_Click(object sender, EventArgs e)
        {
            if (InputBox.Show(this, "New Razor Script", "Enter the name of the script"))
            {
                string name = InputBox.GetString();
                if (string.IsNullOrEmpty(name) || name.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                    name.IndexOfAny(m_InvalidNameChars) != -1)
                {
                    MessageBox.Show(this, Language.GetString(LocString.InvalidChars),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string path = Path.Combine(ScriptManager.ScriptPath, $"{name}.razor");

                if (File.Exists(path))
                {
                    MessageBox.Show(this, Language.GetString(LocString.MacroExists),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                File.CreateText(path).Close();

                ScriptManager.RazorScript script = new ScriptManager.RazorScript
                {
                    Lines = File.ReadAllLines(path),
                    Name = name,
                    Path = path
                };

                ScriptManager.AddHotkey(script.Name);

                ScriptManager.RedrawScripts();

                for (int i = 0; i < scriptList.Items.Count; i++)
                {
                    if (scriptList.Items[i].ToString().Equals(script.Name))
                    {
                        scriptList.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void saveScript_Click(object sender, EventArgs e)
        {
            SaveScript();
        }

        private void deleteScript_Click(object sender, EventArgs e)
        {
            if (scriptList.SelectedIndex < 0)
                return;

            ScriptManager.RazorScript script = (ScriptManager.RazorScript) scriptList.SelectedItem;

            if (MessageBox.Show(this, Language.Format(LocString.DelConf, $"{scriptList.SelectedItem}"),
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    File.Delete(script.Path);
                    ScriptManager.RemoveHotkey(script.Name);
                }
                catch
                {
                    return;
                }
            }

            ScriptManager.RedrawScripts();
        }

        private void setScriptHotkey_Click(object sender, EventArgs e)
        {
            try
            {
                Engine.MainWindow.SafeAction(s =>
                {
                    ScriptManager.RazorScript script = GetScriptSel();

                    tabs.SelectedTab = hotkeysTab;

                    TreeNode resultNode = SearchTreeView($"{Language.Format(LocString.PlayScript, script)}",
                        hotkeyTree.Nodes);

                    if (resultNode != null)
                    {
                        KeyData hk = (KeyData) resultNode.Tag;

                        hotkeyTree.SelectedNode = resultNode;
                        key.Focus();
                    }
                });
            }
            catch
            {
                // ignore
            }
        }

        private void addScriptVariable_Click(object sender, EventArgs e)
        {
            if (ScriptManager.Running || ScriptManager.Recording || World.Player == null)
                return;

            Targeting.OneTimeTarget(OnScriptVariableAddTarget);

            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void OnScriptVariableAddTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            TargetInfo t = new TargetInfo
            {
                Gfx = gfx,
                Serial = serial,
                Type = (byte) (ground ? 1 : 0),
                X = pt.X,
                Y = pt.Y,
                Z = pt.Z
            };

            if (InputBox.Show(this, Language.GetString(LocString.NewScriptVariable),
                Language.GetString(LocString.EnterAName)))
            {
                string name = InputBox.GetString();

                foreach (ScriptVariables.ScriptVariable mV in ScriptVariables.ScriptVariableList
                )
                {
                    if (mV.Name.ToLower().Equals(name.ToLower()))
                    {
                        MessageBox.Show(this, "Pick a unique Script Variable name and try again",
                            "New Script Variable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                ScriptVariables.ScriptVariableList.Add(
                    new ScriptVariables.ScriptVariable(name, t));

                ScriptVariables.RegisterVariable(name);

                ScriptManager.DisplayScriptVariables(scriptVariables);
            }

            Engine.MainWindow.ShowMe();
        }

        private void OnScriptVariableReTarget(bool ground, Serial serial, Point3D pt, ushort gfx)
        {
            TargetInfo t = new TargetInfo
            {
                Gfx = gfx,
                Serial = serial,
                Type = (byte) (ground ? 1 : 0),
                X = pt.X,
                Y = pt.Y,
                Z = pt.Z
            };

            ScriptVariables.ScriptVariableList[scriptVariables.SelectedIndex].TargetInfo = t;

            ScriptManager.DisplayScriptVariables(scriptVariables);

            Engine.MainWindow.ShowMe();
        }

        private void changeScriptVariable_Click(object sender, EventArgs e)
        {
            if (ScriptManager.Running || ScriptManager.Recording || World.Player == null)
                return;

            if (scriptVariables.SelectedIndex < 0)
                return;

            Targeting.OneTimeTarget(OnScriptVariableReTarget);

            World.Player.SendMessage(MsgLevel.Force, LocString.SelTargAct);
        }

        private void removeScriptVariable_Click(object sender, EventArgs e)
        {
            if (ScriptManager.Running || ScriptManager.Recording || World.Player == null)
                return;

            if (scriptVariables.SelectedIndex < 0)
                return;

            ScriptVariables.UnregisterVariable(ScriptVariables.ScriptVariableList[scriptVariables.SelectedIndex].Name);
            ScriptVariables.ScriptVariableList.RemoveAt(scriptVariables.SelectedIndex);

            ScriptManager.DisplayScriptVariables(scriptVariables);
        }

        private void linkScriptGuide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.uor-razor.com/guide/");
        }

        private void autoSaveScript_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("AutoSaveScript", autoSaveScript.Checked);
        }

        private void scriptList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (scriptList.SelectedIndex < 0)
                return;

            /*if (e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                if (ScriptManager.Running || ScriptManager.Recording || World.Player == null)
                    return;

                ScriptManager.RazorScript script = (ScriptManager.RazorScript) scriptList.SelectedItem;

                ContextMenu menu = new ContextMenu();
                
                menu.MenuItems.Add("New", OnScriptNew);
                menu.MenuItems.Add($"Rename '{script.Name}'", OnScriptRename);
                menu.MenuItems.Add($"Delete '{script.Name}'", OnScriptDelete);

                menu.Show(scriptList, new Point(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                if (ScriptManager.Running || ScriptManager.Recording || World.Player == null)
                    return;
            }*/
        }

        private void scriptEditor_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Clicks == 1)
            {
                if (ScriptManager.Running || ScriptManager.Recording || World.Player == null)
                    return;

                if (!string.IsNullOrEmpty(scriptEditor.SelectedText))
                {
                    ContextMenu menu = new ContextMenu();

                    menu.MenuItems.Add("Comment", OnScriptComment);
                    menu.MenuItems.Add("Uncomment", OnScriptUncomment);

                    if (!string.IsNullOrEmpty(scriptEditor.SelectedText))
                    {
                        menu.MenuItems.Add("-");
                        menu.MenuItems.Add("Play selected script code", OnScriptPlaySelected);

                        int space = scriptEditor.SelectedText.IndexOf(" ", StringComparison.Ordinal);

                        if (space > -1)
                        {
                            string command = scriptEditor.SelectedText.Substring(0, space);

                            if (command.Equals("dclick"))
                            {
                                menu.MenuItems.Add("-");
                                menu.MenuItems.Add("Convert to 'dclicktype' by gfxid", OnScriptDclickTypeId);
                                menu.MenuItems.Add("Convert to 'dclicktype' by name", OnScriptDclickTypeName);
                            }
                        }
                    }

                    menu.Show(scriptEditor, new Point(e.X, e.Y));
                }
            }
            else if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                if (ScriptManager.Running || ScriptManager.Recording || World.Player == null)
                    return;
            }
        }

        private void OnScriptComment(object sender, System.EventArgs e)
        {
            string[] lines = scriptEditor.SelectedText.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);

            scriptEditor.SelectedText = "";
            for (int i = 0; i < lines.Count(); i++)
            {
                scriptEditor.SelectedText += "#" + lines[i];
                if (i < lines.Count() - 1)
                    scriptEditor.SelectedText += "\r\n";
            }
        }

        private void OnScriptUncomment(object sender, System.EventArgs e)
        {
            string[] lines = scriptEditor.SelectedText.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);

            scriptEditor.SelectedText = "";
            for (int i = 0; i < lines.Count(); i++)
            {
                scriptEditor.SelectedText += lines[i].TrimStart('#');
                if (i < lines.Count() - 1)
                    scriptEditor.SelectedText += "\r\n";
            }
        }

        private void OnScriptDclickTypeId(object sender, System.EventArgs e)
        {
            Serial itemId = Serial.Zero;

            try
            {
                itemId = Serial.Parse(scriptEditor.SelectedText.Split(' ')[1]);
            }
            catch
            {
                return;
            }

            Item item = World.FindItem(itemId);

            if (item == null)
                return;

            scriptEditor.SelectedText = "";
            scriptEditor.SelectedText = $"dclicktype '{item.ItemID.Value}'";
        }

        private void OnScriptDclickTypeName(object sender, System.EventArgs e)
        {
            Serial gfxid = Serial.Zero;

            try
            {
                gfxid = Serial.Parse(scriptEditor.SelectedText.Split(' ')[1]);
            }
            catch
            {
                return;
            }


            Item item = World.FindItem(gfxid);

            if (item == null)
                return;

            scriptEditor.SelectedText = "";
            scriptEditor.SelectedText = $"dclicktype '{item.DisplayName}'";
        }

        private void OnScriptPlaySelected(object sender, System.EventArgs e)
        {
            if (ScriptManager.Running)
            {
                ScriptManager.StopScript();
                return;
            }

            if (string.IsNullOrEmpty(scriptEditor.SelectedText))
                return;

            string[] lines = scriptEditor.SelectedText.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);

            ScriptManager.PlayScript(lines);
        }

        private void autoSaveScriptPlay_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("AutoSaveScriptPlay", autoSaveScriptPlay.Checked);
        }

        private void renameScript_Click(object sender, EventArgs e)
        {
            if (scriptList.SelectedIndex < 0)
                return;

            if (InputBox.Show(this, "Enter a new name for the script", "Rename Script"))
            {
                string name = InputBox.GetString();

                if (string.IsNullOrEmpty(name) || name.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                    name.IndexOfAny(m_InvalidNameChars) != -1)
                {
                    MessageBox.Show(this, Language.GetString(LocString.InvalidChars),
                        Language.GetString(LocString.Invalid), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ScriptManager.RazorScript script = (ScriptManager.RazorScript) scriptList.SelectedItem;

                string newScriptPath = Path.Combine(ScriptManager.ScriptPath, $"{name}.razor");

                if (File.Exists(newScriptPath))
                {
                    MessageBox.Show(this, "A script with that name already exists.",
                        Language.GetString(LocString.Invalid),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    Engine.MainWindow.SafeAction(s =>
                    {
                        ScriptManager.RemoveHotkey(script.Name);

                        File.Move(script.Path, newScriptPath);

                        script.Path = newScriptPath;
                        script.Name = Path.GetFileNameWithoutExtension(newScriptPath);

                        ScriptManager.RedrawScripts();

                        ScriptManager.AddHotkey(script.Name);
                    });
                }
                catch
                {
                    // ignore
                }
            }
        }

        private void highlightFriend_CheckedChanged(object sender, EventArgs e)
        {
            Config.SetProperty("HighlightFriend", highlightFriend.Checked);
        }
    }
}