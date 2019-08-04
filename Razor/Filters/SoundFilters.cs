using System;
using Assistant;

namespace Assistant.Filters
{
    public class SoundFilter : Filter
    {
        public static void Initialize()
        {
            Filter.Register(new SoundFilter(LocString.BardMusic, 0x38, 0x39, 0x43, 0x44, 0x45, 0x46, 0x4C, 0x4D, 0x52,
                0x53));
            Filter.Register(new SoundFilter(LocString.DogSounds, GetRange(0x85, 0x89)));
            Filter.Register(new SoundFilter(LocString.CatSounds, GetRange(0x69, 0x6D)));
            Filter.Register(new SoundFilter(LocString.HorseSounds, GetRange(0xA8, 0xAC)));
            Filter.Register(new SoundFilter(LocString.SheepSounds, GetRange(0xD6, 0xDA)));
            Filter.Register(new SoundFilter(LocString.SS_Sound, 0x24A));
            Filter.Register(new SoundFilter(LocString.FizzleSound, 0x5C));
            Filter.Register(new SoundFilter(LocString.PackSound, 0x48));
            Filter.Register(new SoundFilter(LocString.DeerSounds, 0x82, 0x83, 0x84, 0x85, 0x2BE, 0x2BF, 0x2C0, 0x4CB,
                0x4CC));
            Filter.Register(new SoundFilter(LocString.CyclopTitanSounds, 0x25D, 0x25E, 0x25F, 0x260, 0x261, 0x262,
                0x263, 0x264, 0x265, 0x266));
            Filter.Register(new SoundFilter(LocString.BullSounds, 0x065, 0x066, 0x067, 0x068, 0x069));

            Filter.Register(new SoundFilter(LocString.DragonSounds, 0x2C8, 0x2C9, 0x2CA, 0x2CB, 0x2CC, 0x2CD, 0x2CE,
                0x2CF,
                0x2D0, 0x2D1, 0x2D2, 0x2D3, 0x2D4, 0x2D5, 0x2D6, 0x16B, 0x16C, 0x16D, 0x16E, 0x16F, 0x15F, 0x160,
                0x161));

            Filter.Register(new SoundFilter(LocString.ChickenSounds, 0x06F, 0x070, 0x071, 0x072, 0x073));
        }

        public static ushort[] GetRange(ushort min, ushort max)
        {
            if (max < min)
                return new ushort[0];

            ushort[] range = new ushort[max - min + 1];
            for (ushort i = min; i <= max; i++)
                range[i - min] = i;
            return range;
        }

        private LocString m_Name;
        private ushort[] m_Sounds;

        private SoundFilter(LocString name, params ushort[] blockSounds)
        {
            m_Name = name;
            m_Sounds = blockSounds;
        }

        public override byte[] PacketIDs
        {
            get { return new byte[] {0x54}; }
        }

        public override LocString Name
        {
            get { return m_Name; }
        }

        public override void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            p.ReadByte(); // flags

            ushort sound = p.ReadUInt16();
            for (int i = 0; i < m_Sounds.Length; i++)
            {
                if (m_Sounds[i] == sound)
                {
                    args.Block = true;
                    return;
                }
            }
        }
    }
}