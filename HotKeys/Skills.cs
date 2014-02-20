using System;
using System.Collections;
using Assistant;

namespace Assistant.HotKeys
{
	public class SkillHotKeys
	{
		private static HotKeyCallbackState m_Callback;
		private static int[] m_UsableSkills = new int[]
		{
			1, // anatomy
			2, // animal lore
			3, // item identification
			4, // arms lore
			6, // begging
			9, // peacemaking
			12, // cartography
			14, // detect hidden
			15, // Discordance
			16, // evaluate intelligence
			19, // forensic evaluation
			21, // hiding
			22, // provocation
			23, // inscription
			30, // poisoning
			32, // spirit speak
			33, // stealing
			35, // taming
			36, // taste id
			38, // tracking
			46, // Meditation
			47, // Stealth
			48, // RemoveTrap
		};

		public static void Initialize()
		{
			m_Callback = new HotKeyCallbackState( OnHotKey );
			//1044060 = Alchemy in UO cliloc
			for (int i=0;i<m_UsableSkills.Length;i++)
				HotKey.Add( HKCategory.Skills, (1044060+m_UsableSkills[i]), m_Callback, m_UsableSkills[i] );
		}

		private static void OnHotKey( ref object state )
		{
			int sk = (int)state;
			ClientCommunication.SendToServer( new UseSkill( sk ) );
			if ( World.Player != null )
				World.Player.LastSkill = sk;
			if ( sk == (int)SkillName.Stealth && !World.Player.Visible )
				StealthSteps.Hide();
			//if ( Macros.MacroManager.AcceptActions )
			//	Macros.MacroManager.Action( new Macros.UseSkillAction( sk ) );
		}
	}
}

