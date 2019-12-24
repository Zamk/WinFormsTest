using System;
using System.Windows.Forms;
using System.Drawing;

namespace WaterCourier {
	delegate void WaterConteinerHandler();
	delegate void CourierHandler();

	class MyForm : Form {
		int conteinerPositionY = 400;
		int conteinerPositionX = 10;
		int courierStartPositionX = 600;
		int sizeOfConteiner = 5;
		int courierSpeed = 10;
		int FPS = 60;
		double delay = 1.0;
		static string version = "0.4"; 

		WaterConteiner wc;
		Courier c;
		MainMenu topMenu = new MainMenu();

		Timer timer;
		int tickCount = 0;
		int frameCount = 0;

		public MyForm() {
			Size = new Size(640, 480);
			Text = "Test game";
			StartPosition = FormStartPosition.CenterScreen;

			//TODO: 	Двойная буферизация

			MenuItem menu1 = new MenuItem("Game");
			topMenu.MenuItems.Add(menu1);
			
			MenuItem menu1Pause = new MenuItem("Pause");
			MenuItem menu1Resume = new MenuItem("Resume");
			MenuItem menu1Stop = new MenuItem("Stop");
			MenuItem menu1Start = new MenuItem("Start");
			MenuItem menu1ExitToMenu = new MenuItem("ExitToMenu");

			menu1.MenuItems.Add(menu1Pause);
			menu1.MenuItems.Add(menu1Resume);
			menu1.MenuItems.Add(menu1Start);
			menu1.MenuItems.Add(menu1Stop);
			menu1.MenuItems.Add(menu1ExitToMenu);

			menu1Resume.Click += new EventHandler(M1ResumeClick);
			menu1Start.Click += new EventHandler(OnStartGameClick);
			menu1Pause.Click += new EventHandler(M1PauseClick);
			menu1Stop.Click += new EventHandler(M1StopClick);
			menu1ExitToMenu.Click += new EventHandler(M1ETMClick);
			
			Menu = topMenu;

			DrawMainMenu();
		}

		private void TimerTick(object sender, EventArgs e) {

			if(wc != null && tickCount > (FPS * delay)) {
				frameCount = tickCount% ((int)(FPS * delay));
				if(frameCount == (tickCount / ((int)(FPS * delay)))) wc.AddWaterBlock(new Point(conteinerPositionX, conteinerPositionY - ((frameCount - 1) * 22)));
			}
			if(c != null) c.Move();

			this.Refresh();
			tickCount++;
		}

		private void StopGame() {
			if(timer != null) timer.Stop();
			
			if(wc != null) {
				wc.Clear();
				wc = null;
			}
			if(c != null) { 
				c.Stop();
				c = null;
			}
			TicksReset();
			this.Refresh();
		}

		private void TicksReset() {
			tickCount = -1;
		}

		private void DrawMainMenu() {
			Button startGame = new Button();
			startGame.Text = "Start";
			startGame.Location = new Point(280, 200);
			startGame.Click += new EventHandler(OnStartGameClick);
			Controls.Add(startGame);
			
			Button settings = new Button();
			settings.Text = "Settings";
			settings.Location = new Point(280, 240);
			settings.Click += new EventHandler(OnSettingsClick);
			Controls.Add(settings);

			Button aboutDev = new Button();
			aboutDev.Text = "Info";
			aboutDev.Location = new Point(280, 280);
			aboutDev.Click += new EventHandler(OnAboutDevClick);
			Controls.Add(aboutDev);

		}

		private void DrawGame() {
			wc = new WaterConteiner();
			c = new Courier();
			timer = new Timer();
			timer.Interval = 1000/FPS;
			timer.Tick += TimerTick;

			wc.CreateWaterConteiner(sizeOfConteiner);
			Paint += wc.RedrawConteiner;

			c.CreateCourier(new Point(courierStartPositionX, conteinerPositionY));
			Paint += c.Redraw;

			c.CourierTookWaterEvent += new CourierHandler(wc.CourierTookWaterHandler);
			c.CourierFinishedEvent += new CourierHandler(wc.CourierFinishedHandler);
			c.CourierFinishedEvent += new CourierHandler(TicksReset);


			wc.WaterConteinerFullEvent += new WaterConteinerHandler(c.WaterConteinerFullHandler);

			c.ConteinerPositionX = conteinerPositionX;
			c.Speed = courierSpeed;

			timer.Start();
		}

		private void DrawAboutDev() {
			Label info = new Label();
			info.AutoSize = true;
			info.Text = "Water Courier Game\n\nby Zamk\n\nversion: "  + version;
			info.Location = new Point(260, 200);
			Controls.Add(info);

			Button back = new Button();
			back.Text = "Back";
			back.Location = new Point(280, 280);
			back.Click += new EventHandler(OnBackClick);
			Controls.Add(back);
		}

		TextBox t1, t2, t3;
		private void DrawSettings() {
			Label l1 = new Label();
			l1.Text = "Size of water conteiner";
			l1.Location = new Point(20, 20);
			l1.AutoSize = true;
			Controls.Add(l1);

			t1 = new TextBox();
			t1.Location = new Point(200, 20);
			Controls.Add(t1);

			Label l2 = new Label();
			l2.Text = "Courier speed";
			l2.Location = new Point(20, 60);
			l2.AutoSize = true;
			Controls.Add(l2);

			t2 = new TextBox();
			t2.Location = new Point(200, 60);
			Controls.Add(t2);

			Label l3 = new Label();
			l3.Text = "Delay (float)";
			l3.Location = new Point(20, 100);
			l3.AutoSize = true;
			Controls.Add(l3);

			t3 = new TextBox();
			t3.Location = new Point(200, 100);
			Controls.Add(t3);

			Button save = new Button();
			save.Location = new Point(280, 280);
			save.Text = "Save settings";
			save.Click += new EventHandler(SetSettings);
			Controls.Add(save);
		}

		private void SetSettings(object sender, EventArgs e) {
			if(t1.Text.Length != 0) sizeOfConteiner = Convert.ToInt32(t1.Text);
			if(t2.Text.Length != 0) courierSpeed = Convert.ToInt32(t2.Text);
			if(t3.Text.Length != 0) delay = Convert.ToDouble(t3.Text);

			Controls.Clear();
			DrawMainMenu();
		}
		
		private void OnBackClick(object sender, EventArgs e) {
			Controls.Clear();
			DrawMainMenu();
		}

		public void OnStartGameClick(object sender, EventArgs e) {
			Controls.Clear();
			DrawGame();
		}

		public void OnAboutDevClick(object sender, EventArgs e) {
			Controls.Clear();
			DrawAboutDev();
		}

		public void OnSettingsClick(object sender, EventArgs e) {
			Controls.Clear();
			DrawSettings();
		}

		public void M1ResumeClick(object sender, EventArgs e) {
			if(c != null) {
				Controls.Clear();
				c.Start();
			}
			if(timer != null) timer.Start();
		}

		public void M1PauseClick(object sender, EventArgs e) {
			if(timer != null) timer.Stop();
		}

		public void M1StopClick(object sender, EventArgs e) {
			StopGame();
		}

		public void M1ETMClick(object sender, EventArgs e) {
			Controls.Clear();
			StopGame();
			DrawMainMenu();
		}
	}

	class WaterBlock {
		Point cord;

		public WaterBlock(Point Cord) {
			cord = Cord;
		}

		public void Redraw(Graphics g) {
			g.FillRectangle(new SolidBrush(Color.Blue), cord.X, cord.Y, 40, 20);
		}

	}

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

		public void CreateWaterConteiner(int count) {
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

		public void RedrawConteiner(object sender, PaintEventArgs e) {
			for(int i = 0; i < blocks.Length; i++) {
				if(blocks[i] != null) blocks[i].Redraw(e.Graphics);
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

	class Courier {
		public event CourierHandler CourierTookWaterEvent;
		public event CourierHandler CourierFinishedEvent;

		Point cord;
		int courierStartPositionX;
		int speed;
		int conteinerPositionX;
		bool isExist;
		bool isStarted;

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

		public void CreateCourier(Point Cord) {
			cord = Cord;
			courierStartPositionX = cord.X;
			isStarted = false;
			isExist = true;
		}

		public void Stop() {
			isExist = false;
		}

		public void Start() {
			isExist = true;
		}

		public void Redraw(object sender, PaintEventArgs e) {
			if(isExist)e.Graphics.FillEllipse(new SolidBrush(Color.Red), cord.X, cord.Y, 20, 20);
		}

		public void WaterConteinerFullHandler() {
			isStarted = true;
		}
	}

	class Programm {
		[STAThread]
		public static void Main() {
			MyForm form = new MyForm();
			Application.Run(form);
		}
	}
}