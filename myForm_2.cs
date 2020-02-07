using System;
using System.Windows.Forms;
using System.Drawing;

namespace WaterCourier {
	class MyForm : Form{ 
		Timer timer;
		int tickCount = 0;

		Point conteinerPosition = new Point(10, 400);
		Point courierStartPosition = new Point(600, 400);

		int sizeOfConteiner = 5;
		int courierSpeed = 10;
		int FPS = 60;
		double delay = 1.0;

		Game game;

		public MyForm() {
			Size = new Size(640, 480);
			game = new Game(conteinerPosition, courierStartPosition);
			Paint += game.Redraw;

			timer = new Timer();
			timer.Interval = 1000/FPS;
			timer.Tick += TimerTick;

			game.GameEndEvent += new GameHandler(GameEndEventHandler);

			timer.Start();
		}

		public void TimerTick(object sender, EventArgs e) {
			game.Update(tickCount);
			tickCount++;

			this.Refresh();
		}

		public void GameEndEventHandler() {
			tickCount = -1;
		}
	}
}