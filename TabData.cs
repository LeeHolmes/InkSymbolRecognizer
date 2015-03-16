using System;

namespace SymbolTrainer
{
    [Serializable]
	public class TabData
	{
        private byte[] ink;
        private string text;

		public TabData(byte[] ink, string text)
		{
            this.ink = ink;
            this.text = text;
		}

        public byte[] InkData { get { return ink; } }
        public string TextPane { get { return text; } }
	}
}
