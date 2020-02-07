using System.Drawing;
using System.Windows.Forms;

namespace WaterCourier{
	class Courier {
		public event CourierHandler CourierTookWaterEvent;
		public event CourierHandler CourierFinishedEvent;

		Point cord;
		int courierStartPositionX;
		int speed;
		int conteinerPositionX;
		bool isExist;
		bool isStarted;

		public Courier(Point Cord) {
			cord = Cord;
			courierStartPositionX = cord.X;
			isStarted = false;
			isExist = true;
		}

		public int ConteinerPositionX {
			set {
				conteinerPositionX = value;
			}
		}

		public int Speed {
			set {
				speed = -value;
			}
		}

		public void Move() {
			if(isStarted) {
				cord.X += speed;

				if(cord.X >= courierStartPositionX) {
					isStarted = false;
					this.Reverse();
					CourierFinishedEvent();
				} else if(cord.X <= conteinerPositionX) {
					this.Reverse();
					CourierTookWaterEvent();
				}
			}

		}

		public void Reverse() {
			speed = -speed;
		}

		public void Stop() {
			isExist = false;
		}

		public void Start() {
			isExist = true;
		}
		
		public void Redraw(Graphics g) {
			if(isExist)g.FillEllipse(new SolidBrush(Color.Red), cord.X, cord.Y, 20, 20);
		}

		public void WaterConteinerFullHandler() {
			isStarted = true;
		}
	}
}