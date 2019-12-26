using System.Drawing;
using System.Windows.Forms;

namespace WaterCourier {
	class WaterBlock {
		Point cord;
		
		public WaterBlock(Point Cord) {
			cord = Cord;
		}

		public void Redraw(Graphics g) {
			g.FillRectangle(new SolidBrush(Color.Blue), cord.X, cord.Y, 40, 20);
		}
	}
}