using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Data;

namespace Springerproblem
{
	/// <summary>
	/// Zusammenfassung f�r Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnGo;
		private W�rfel meinW�rfel;
		private Brett meinBrett;
		private Springer meinSpringer;
		private int startPositionX;
		private int startPositionY;
		private int anzahlZ�geGesammt;
		private System.Windows.Forms.Button btnStop;
		private Thread tSpringer;
		
		// Konstatnen, die sich nach belieben �ndern lassen.
		private const int WARTEZEITMILLISEKUNDEN = 100;
		private const bool MITHEURISTIK = true;
		private const bool GRAFIKANZEIGEN = true;
		private const bool WEGPROTOKOLLIEREN = false;
		private const bool SCHLUSSKOORDINATENAUGEBEN = true;

		public Form1()
		{
			meinW�rfel = new W�rfel();
			meinBrett = new Brett();
			meinSpringer = new Springer(0, 0, ref meinBrett, MITHEURISTIK);

			startPositionX = 10;
			startPositionY = 30;

			anzahlZ�geGesammt = 0;
			tSpringer = new Thread(new ThreadStart(l�seSpringerproble));

			InitializeComponent();
		}

		#region Ignore
		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnGo = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(24, 0);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "Go";
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(112, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 0;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(606, 539);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnStop);
            this.Name = "Form1";
            this.Text = "Springerproblem";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Der Haupteinstiegspunkt f�r die Anwendung.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
		#endregion

		private void Form1_Load(object sender, System.EventArgs e)
		{
		}

		#region Brett zeichnen
		protected override void OnPaint(PaintEventArgs e)
		{
			anzahlZ�geGesammt++;

			if(GRAFIKANZEIGEN)
			{
				Graphics g = e.Graphics;
				Pen pinsel = new Pen(Color.DarkRed, 1);
				System.Drawing.Font font = new Font(FontFamily.GenericSansSerif, 14);
				Brush b�rste = new SolidBrush(Color.DarkRed);
				Brush b�rste2 = new SolidBrush(Color.Yellow);
				Brush b�rsteBlau = new SolidBrush(Color.DarkBlue);
				Brush b�rsteWeiss = new SolidBrush(Color.White);
				Image bild = Resource1.knight;

				for(int i=0; i<meinBrett.AnzahlFelderHorizontal; i++)
				{
					for(int j=0; j<meinBrett.AnzahlFelderVertikal; j++)
					{
						if(meinBrett.GetWert(i, j) == 0)
						{
							switch((j+i) % 4)
							{
								case 0: case 2:
									g.DrawRectangle(pinsel, meinW�rfel.getW�rfelriss(
										startPositionX + i*meinW�rfel.Breite,
										startPositionY + j*meinW�rfel.H�he));
									g.FillRectangle(b�rste2, meinW�rfel.getW�rfelinnenfl�che(
										startPositionX + i*meinW�rfel.Breite,
										startPositionY + j*meinW�rfel.H�he));
									break;
					
								case 1: case 3:
									g.FillRectangle(b�rste, meinW�rfel.getW�rfelfl�che(
										startPositionX + i*meinW�rfel.Breite,
										startPositionY + j*meinW�rfel.H�he));
									break;
							}
						}
						else
						{
							g.FillRectangle(b�rsteBlau, meinW�rfel.getW�rfelfl�che(
								startPositionX + i*meinW�rfel.Breite,
								startPositionY + j*meinW�rfel.H�he));
							g.DrawString(meinBrett.GetWert(i, j).ToString(), font, b�rsteWeiss,
								startPositionX + i*meinW�rfel.Breite,
								startPositionY + j*meinW�rfel.H�he);
						}
					}
				}

				g.DrawImage(bild, startPositionX + 3 + (meinW�rfel.Breite * meinSpringer.XPosition),
					startPositionY + 3 + (meinW�rfel.H�he * meinSpringer.YPosition),
					meinW�rfel.Breite-6, meinW�rfel.H�he-6);
				g.DrawString(meinSpringer.Schachzug.ToString(), font, b�rsteBlau, 1, 1);
			}

			if(WEGPROTOKOLLIEREN)
			{
				sprungpositionenAlsTextAusgeben();
			}
		}

		private void sprungpositionenAlsTextAusgeben()
		{
			//Debuginformationen
			for(int i=0; i<meinBrett.AnzahlFelderHorizontal; i++)
			{
				string zeile = "";
				for(int j=0; j<meinBrett.AnzahlFelderVertikal; j++)
				{
					if(meinBrett.GetWert(j, i).ToString().Length == 1)
					{
						zeile += "0" + meinBrett.GetWert(j, i).ToString() + "\t";
					}
					else
					{
						zeile += meinBrett.GetWert(j, i).ToString() + "\t";
					}
				}
				System.Diagnostics.Debug.WriteLine(zeile);
			}
			System.Diagnostics.Debug.WriteLine("");
		}
		#endregion

		/// <summary>
		/// Eigentlicher Backward-Rekursionsalgorithmus.
		/// </summary>
		/// <param name="sprungindex">
		/// Gibt den Versuchsindex (0 Basierend) der aktuellen Position an. Beim ersten Aufruf,
		/// der Methode, ist der Sprungindex 0.
		/// </param>
		/// <returns>
		/// true, wenn L�sung gefunden. Sonst false.
		/// </returns>
		private bool l�sungFinden(int sprungindex)
		{
			if(meinSpringer.Schachzug == meinBrett.AnzahlFelderHorizontal*meinBrett.AnzahlFelderVertikal)
			{
				if(SCHLUSSKOORDINATENAUGEBEN)
				{
					sprungpositionenAlsTextAusgeben();
				}
				return true;
			}
			else
			{
				ArrayList m�glicheSpr�nge = meinSpringer.GetM�glicheSpr�nge();

				if(m�glicheSpr�nge.Count > 0)
				{
					while(sprungindex < m�glicheSpr�nge.Count)
					{
						meinSpringer.SpringeAufN�chstesFeld((int)m�glicheSpr�nge[sprungindex]);
						sleep();
						this.Refresh();
						
						if(!l�sungFinden(0))
						{
							sprungindex++;
						}
						else
							return true;
					}
				}
				
				// keine L�sung gefunden => Backward
				meinSpringer.SpringZur�ck();
				sleep();
				this.Refresh();

				return false;
			}
		}

		private void btnGo_Click(object sender, System.EventArgs e)
		{
			if(tSpringer.ThreadState == ThreadState.Suspended || (int)tSpringer.ThreadState == 96)
			{
				tSpringer.Resume();
			}
			else if(tSpringer.ThreadState == ThreadState.Unstarted)
			{
				tSpringer.Start();	
			}
		}

		private void l�seSpringerproble()
		{
			if(l�sungFinden(0))
			{
				MessageBox.Show(
					String.Format("L�sung in {0}. Z�gen gefunden :)", anzahlZ�geGesammt) );
			}
			else
			{
				MessageBox.Show("Keine L�sung gefunden :`(");
			}
		}

		public void sleep() 
		{
			if(WARTEZEITMILLISEKUNDEN > 0)
			{
				Thread.Sleep(WARTEZEITMILLISEKUNDEN);
			}
		}

		private void btnStop_Click(object sender, System.EventArgs e)
		{
			if(tSpringer.IsAlive)
				tSpringer.Suspend();
		}
	}
}