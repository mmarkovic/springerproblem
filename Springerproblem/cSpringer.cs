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
		/// <param name="richtung">Richtungsangabe f�r das n�chste Feld Zahl von 1-8</param>
		public bool SpringeAufN�chstesFeld(int richtung)
		{
			if(!IstSprungAufN�chstesFeldM�glich(richtung))
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
		/// Gibt alle m�gliche Richtungen von 1-8 in einem Array zur�ck.
		/// Wenn die Heuristik aktiviert ist, werden die m�glichen Richtungen
		/// so geordnet, dass automatisch die Richtung, welche die wenigsten
		/// nachfolgenden M�glichkeiten hat, an erster stelle gestellt.
		/// </summary>
		public ArrayList GetM�glicheSpr�nge()
		{
			ArrayList m�glicheSpr�nge = new ArrayList();
			int index = 0;

			for(int i=1; i<=8; i++)
			{
				if(IstSprungAufN�chstesFeldM�glich(i))
				{
					m�glicheSpr�nge.Insert(index, i);
					index++;
				}
			}

			if(mitHeuristik)
			{
				m�glicheSpr�nge = m�glicheSpr�ngeHeuristischOrdnen(m�glicheSpr�nge);
			}

			return m�glicheSpr�nge;
		}

		private ArrayList m�glicheSpr�ngeHeuristischOrdnen(ArrayList m�glicheSpr�nge)
		{
			ArrayList anzahlM�glicherSpr�nge = new ArrayList();

			for(int i=0; i<m�glicheSpr�nge.Count; i++)
			{
				SpringeAufN�chstesFeld((int)m�glicheSpr�nge[i]);
				this.mitHeuristik = false;
				anzahlM�glicherSpr�nge.Insert(i, GetM�glicheSpr�nge().Count);
				this.mitHeuristik = true;
				SpringZur�ck();
			}

			for(int i=0; i<anzahlM�glicherSpr�nge.Count-1; i++)
			{
				for(int j=0; j<anzahlM�glicherSpr�nge.Count-1; j++)
				{
					if((int)anzahlM�glicherSpr�nge[j] > (int)anzahlM�glicherSpr�nge[j+1])
					{
						int tmp = (int)anzahlM�glicherSpr�nge[j];
						anzahlM�glicherSpr�nge[j] = (int)anzahlM�glicherSpr�nge[j+1];
						anzahlM�glicherSpr�nge[j+1] = tmp;

						tmp = (int)m�glicheSpr�nge[j];
						m�glicheSpr�nge[j] = (int)m�glicheSpr�nge[j+1];
						m�glicheSpr�nge[j+1] = tmp;
					}
				}
			}

			return m�glicheSpr�nge;
		}

		/// <summary>
		/// Pr�ft ob ein Sprung auf ein anderes Feld m�glich ist
		/// </summary>
		/// <param name="richtung">1-8</param>
		/// <returns></returns>
		public bool IstSprungAufN�chstesFeldM�glich(int richtung)
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

		public void SpringZur�ck()
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