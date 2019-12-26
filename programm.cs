using System;
using System.Windows.Forms;

namespace WaterCourier {
	class Programm {
		[STAThread]
		public static void Main() {
			MyForm form = new MyForm();
			Application.Run(form);
		}
	}
}