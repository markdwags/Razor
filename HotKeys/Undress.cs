using System;
using Assistant;
using Assistant.Macros;

namespace Assistant.HotKeys
{
	public class UndressHotKeys
	{
		public static void Initialize()
		{
			HotKey.Add( HKCategory.Dress, LocString.ArmDisarmRight, new HotKeyCallback( ToggleRight ) );
			HotKey.Add( HKCategory.Dress, LocString.ArmDisarmLeft, new HotKeyCallback( ToggleLeft ) );

			HotKey.Add( HKCategory.Dress, LocString.UndressAll, new HotKeyCallback( OnUndressAll ) );
			HotKey.Add( HKCategory.Dress, LocString.UndressHands, new HotKeyCallback( OnUnequipBothHands ) );
			HotKey.Add( HKCategory.Dress, LocString.UndressLeft, new HotKeyCallback( OnUnequipLeft ) );
			HotKey.Add( HKCategory.Dress, LocString.UndressRight, new HotKeyCallback( OnUnequipRight ) );
			HotKey.Add( HKCategory.Dress, LocString.UndressHat, new HotKeyCallback( OnUnequipHat ) );
			HotKey.Add( HKCategory.Dress, LocString.UndressJewels, new HotKeyCallback( OnUnequipJewelry ) );
		}

		private static Item m_Right, m_Left;
		public static void ToggleRight()
		{
			if ( World.Player == null )
				return;

			Item item = World.Player.GetItemOnLayer( Layer.RightHand );
			if ( item == null )
			{
				if ( m_Right != null )
					m_Right = World.FindItem( m_Right.Serial );

				if ( m_Right != null && m_Right.IsChildOf( World.Player.Backpack ) )
				{
					// try to also undress conflicting hand(s)
					Item conflict = World.Player.GetItemOnLayer( Layer.LeftHand );
					if ( conflict != null && ( conflict.IsTwoHanded || m_Right.IsTwoHanded ) )
					{
						Item ub = DressList.FindUndressBag( conflict );
						if ( ub != null )
							DragDropManager.DragDrop( conflict, ub );
					}

					DragDropManager.DragDrop( m_Right, World.Player, DressList.GetLayerFor(m_Right) );
				}
				else 
				{
					World.Player.SendMessage( MsgLevel.Force, LocString.MustDisarm );
				}
			}
			else
			{
				Item ub = DressList.FindUndressBag( item );
				if ( ub != null )
					DragDropManager.DragDrop( item, ub );
				m_Right = item;
			}
		}

		public static void ToggleLeft()
		{
			if ( World.Player == null || World.Player.Backpack == null )
				return;

			Item item = World.Player.GetItemOnLayer( Layer.LeftHand );
			if ( item == null )
			{
				if ( m_Left != null )
					m_Left = World.FindItem( m_Left.Serial );
				
				if ( m_Left != null && m_Left.IsChildOf( World.Player.Backpack ) )
				{
					Item conflict = World.Player.GetItemOnLayer( Layer.RightHand );
					if ( conflict != null && ( conflict.IsTwoHanded || m_Left.IsTwoHanded ) )
					{
						Item ub = DressList.FindUndressBag( conflict );
						if ( ub != null )
							DragDropManager.DragDrop( conflict, ub );
					}
					
					DragDropManager.DragDrop( m_Left, World.Player, DressList.GetLayerFor(m_Left) );
				}
				else 
				{
					World.Player.SendMessage( MsgLevel.Force, LocString.MustDisarm );
				}
			}
			else
			{
				Item ub = DressList.FindUndressBag( item );
				if ( ub != null )
					DragDropManager.DragDrop( item, ub );
				m_Left = item;
			}
		}

		public static bool Unequip( Layer layer )
		{
			if ( layer == Layer.Invalid || layer > Layer.LastUserValid )
				return false;

			//if ( Macros.MacroManager.AcceptActions )
			//	MacroManager.Action( new Macros.UnDressAction( (byte)Layer ) );

			Item item = World.Player.GetItemOnLayer( layer );
			if ( item != null )
			{
				Item pack = DressList.FindUndressBag( item );
				if ( pack != null )
				{
					DragDropManager.DragDrop( item, pack );
					return true;
				}
			}
			return false;
		}

		public static void OnUndressAll()
		{
			for ( int i=0;i<World.Player.Contains.Count;i++ )
			{
				Item item = (Item)World.Player.Contains[i];
				if ( item.Layer <= Layer.LastUserValid && item.Layer != Layer.Backpack && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair )
				{
					Item pack = DressList.FindUndressBag( item );
					if ( pack != null )
						DragDropManager.DragDrop( item, pack );
				}
			}
			
			//if ( Macros.MacroManager.AcceptActions )
			//	MacroManager.Action( new Macros.UnDressAction( (byte)0 ) );
		}

		public static void OnUnequipJewelry()
		{
			Unequip( Layer.Ring ); // ring
			Unequip( Layer.Bracelet ); // bracelet
			Unequip( Layer.Earrings ); // earrings
		}
		
		public static void OnUnequipHat()
		{
			Unequip( Layer.Head );
		}
		
		public static void OnUnequipBothHands()
		{
			Unequip( Layer.RightHand );
			Unequip( Layer.LeftHand );
		}
		
		public static void OnUnequipRight()
		{
			Unequip( Layer.RightHand );
		}
		
		public static void OnUnequipLeft()
		{
			Unequip( Layer.LeftHand );
		}
	}
}
