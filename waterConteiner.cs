using System.Drawing;
using System.Windows.Forms;

namespace WaterCourier {
	class WaterConteiner {
		public event WaterConteinerHandler WaterConteinerFullEvent;

		WaterBlock[] blocks;

		int fullness;
		bool isStarted;

		public int Fullness {
			get {
				return fullness;
			}
		}

		public WaterConteiner(int count) {
			blocks = new WaterBlock[count];
			fullness = 0;
			isStarted = true;
		}

		public void AddWaterBlock(Point Cord) {
			if(isStarted && (fullness < blocks.Length) ) {
				blocks[fullness] = new WaterBlock(Cord);
				fullness++;
			}
			if(fullness == blocks.Length) {
				WaterConteinerFullEvent();
				fullness++;
				isStarted = false;
			}
		}

		public void RedrawConteiner(Graphics g) {
			for(int i = 0; i < blocks.Length; i++) {
				if(blocks[i] != null) blocks[i].Redraw(g);
			}
		}

		public void Clear() {
			for(int i = 0; i < blocks.Length; i++) {
				blocks[i] = null;
			}
			fullness = 0;
		}

		public void CourierTookWaterHandler() {
			Clear();
		}

		public void CourierFinishedHandler() {
			isStarted = true;
		}
	}
}