using System;
using System.Collections;

namespace Springerproblem
{
	/// <summary>
	/// Beschreibt den Springer
	/// </summary>
	public class Springer
	{
		private int xPosition;
		private int yPosition;
		private int schachzug;
		private bool mitHeuristik;
		private Brett brett;

		// 1. 0 in x, 1 in y | 2. 0 in x, 1 in y | 3. 1 in x, 0 in y
		private int[,] wegDesSpringers = {	
			{0, 1, 0, 1, -1, 0},
			{0, 1, 0, 1, 1, 0},
			{1, 0, 1, 0, 0, 1},
			{1, 0, 1, 0, 0, -1},
			{0, -1, 0, -1, 1, 0},
			{0, -1, 0, -1, -1, 0},
			{-1, 0, -1, 0, 0, -1},
			{-1, 0, -1, 0, 0, 1} };

		public Springer(int xStartposition, int yStartposition, ref Brett brett, bool mitHeuristik)
		{
			this.xPosition = xStartposition;
			this.yPosition = yStartposition;
			this.brett = brett;
			this.brett.SetWert(1, xStartposition, yStartposition);
			this.mitHeuristik = mitHeuristik;
			this.schachzug = 1;
		}

		public int XPosition
		{
			get { return xPosition; }
			set { xPosition = value; }
		}

		public int YPosition
		{
			get { return yPosition; }
			set { yPosition = value; }
		}

		/// <summary>
		/// Springt auf das genannte Feld
		/// </summary>
		/// <param name="richtung">Richtungsangabe für das nächste Feld Zahl von 1-8</param>
		public bool SpringeAufNächstesFeld(int richtung)
		{
			if(!IstSprungAufNächstesFeldMöglich(richtung))
				return false;

			int neuesX = xPosition;
			int neuesY = yPosition;

			if(richtung <1 || richtung >8)
				return false;
			else
				richtung--;

			for(int i=0; i<wegDesSpringers.GetLength(1); i++)
			{
				if(i % 2 == 0)
				{	// Gerade Zahl -> x
					neuesX += wegDesSpringers[richtung, i];
				}
				else
				{	// Ungerade Zahl -> y
					neuesY += wegDesSpringers[richtung, i];
				}
			}

			schachzug++;
			brett.SetWert(schachzug, neuesX, neuesY);
			xPosition = neuesX;
			yPosition = neuesY;

			return true;
		}

		/// <summary>
		/// Gibt alle mögliche Richtungen von 1-8 in einem Array zurück.
		/// Wenn die Heuristik aktiviert ist, werden die möglichen Richtungen
		/// so geordnet, dass automatisch die Richtung, welche die wenigsten
		/// nachfolgenden Möglichkeiten hat, an erster stelle gestellt.
		/// </summary>
		public ArrayList GetMöglicheSprünge()
		{
			ArrayList möglicheSprünge = new ArrayList();
			int index = 0;

			for(int i=1; i<=8; i++)
			{
				if(IstSprungAufNächstesFeldMöglich(i))
				{
					möglicheSprünge.Insert(index, i);
					index++;
				}
			}

			if(mitHeuristik)
			{
				möglicheSprünge = möglicheSprüngeHeuristischOrdnen(möglicheSprünge);
			}

			return möglicheSprünge;
		}

		private ArrayList möglicheSprüngeHeuristischOrdnen(ArrayList möglicheSprünge)
		{
			ArrayList anzahlMöglicherSprünge = new ArrayList();

			for(int i=0; i<möglicheSprünge.Count; i++)
			{
				SpringeAufNächstesFeld((int)möglicheSprünge[i]);
				this.mitHeuristik = false;
				anzahlMöglicherSprünge.Insert(i, GetMöglicheSprünge().Count);
				this.mitHeuristik = true;
				SpringZurück();
			}

			for(int i=0; i<anzahlMöglicherSprünge.Count-1; i++)
			{
				for(int j=0; j<anzahlMöglicherSprünge.Count-1; j++)
				{
					if((int)anzahlMöglicherSprünge[j] > (int)anzahlMöglicherSprünge[j+1])
					{
						int tmp = (int)anzahlMöglicherSprünge[j];
						anzahlMöglicherSprünge[j] = (int)anzahlMöglicherSprünge[j+1];
						anzahlMöglicherSprünge[j+1] = tmp;

						tmp = (int)möglicheSprünge[j];
						möglicheSprünge[j] = (int)möglicheSprünge[j+1];
						möglicheSprünge[j+1] = tmp;
					}
				}
			}

			return möglicheSprünge;
		}

		/// <summary>
		/// Prüft ob ein Sprung auf ein anderes Feld möglich ist
		/// </summary>
		/// <param name="richtung">1-8</param>
		/// <returns></returns>
		public bool IstSprungAufNächstesFeldMöglich(int richtung)
		{
			int neuesX = xPosition;
			int neuesY = yPosition;

			if(richtung <1 || richtung >8)
				return false;
			else
				richtung--;

			for(int i=0; i<wegDesSpringers.GetLength(1); i++)
			{
				if(i % 2 == 0)
				{	// Gerade Zahl -> x
					neuesX += wegDesSpringers[richtung, i];
				}
				else
				{	// Ungerade Zahl -> y
					neuesY += wegDesSpringers[richtung, i];
				}
			}

			if(istFeldVorhanden(neuesX, neuesY) && brett.GetWert(neuesX, neuesY) == 0)
			{
				return true;
			}

			return false;
		}

		public int Schachzug
		{
			get { return schachzug; }
		}

		public void SpringZurück()
		{
			brett.SetWert(0, xPosition, yPosition);
			schachzug--;

			for(int i=0; i<brett.AnzahlFelderHorizontal; i++)
			{
				for(int j=0; j<brett.AnzahlFelderVertikal; j++)
				{
					if(brett.GetWert(i, j) == schachzug)
					{
						xPosition = i;
						yPosition = j;
						return;
					}
				}
			}
		}

		private bool istFeldVorhanden(int x, int y)
		{
			return (x < brett.AnzahlFelderHorizontal && x >= 0) &&
					(y < brett.AnzahlFelderVertikal && y >= 0);
		}
	}
}