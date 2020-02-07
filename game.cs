using System;
using System.Windows.Forms;
using System.Drawing;

namespace WaterCourier {
	delegate void WaterConteinerHandler();
	delegate void CourierHandler();
	delegate void GameHandler();

	class Game {
		public event GameHandler GameEndEvent;

		WaterConteiner wc;
		Courier c;

		int FPS = 60;
		double delay = 1.0;

		Point conteinerPosition;
		Point courierStartPosition;

		public Game(Point ContPos, Point CourPos) {
			conteinerPosition = ContPos;
			courierStartPosition = CourPos;

			Start();
		}

		public void Start() {
			wc = new WaterConteiner(5);
			c = new Courier(new Point(courierStartPosition.X, courierStartPosition.Y));

			c.CourierTookWaterEvent += new CourierHandler(wc.CourierTookWaterHandler);
			c.CourierFinishedEvent += new CourierHandler(wc.CourierFinishedHandler);
			c.CourierFinishedEvent += new CourierHandler(End);

			wc.WaterConteinerFullEvent += new WaterConteinerHandler(c.WaterConteinerFullHandler);

			c.ConteinerPositionX = conteinerPosition.X;
			c.Speed = 10;//courierSpeed;
		}

		public void Update(int tickCount) {
			if(wc != null && tickCount > (FPS * delay)) {
				int frameCount = tickCount% ((int)(FPS * delay));
				if(frameCount == (tickCount / ((int)(FPS * delay)))) wc.AddWaterBlock(new Point(conteinerPosition.X, conteinerPosition.Y - ((frameCount - 1) * 22)));
			}
			if(c != null) c.Move();
		}

		public void End() {
			GameEndEvent();
		}

		public void Redraw(object sender, PaintEventArgs e) {
			wc.RedrawConteiner(e.Graphics);
			c.Redraw(e.Graphics);
		}
	}
}