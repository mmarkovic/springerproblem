using System;

namespace Springerproblem
{
	/// <summary>
	/// Definiert das Schachbrett
	/// </summary>
	public class Brett
	{
		private const int ANZFELDERHORIZONTAL = 8;
		private const int ANZFELDERVERTIKAL = 8;
		private int[,] brett;

		public Brett()
		{
			brett = new int[ANZFELDERHORIZONTAL, ANZFELDERVERTIKAL];

			for(int x=0; x<ANZFELDERHORIZONTAL; x++)
			{
				for(int y=0; y<ANZFELDERVERTIKAL; y++)
				{
					brett[x, y] = 0;
				}
			}
		}

		public int AnzahlFelderHorizontal
		{
			get { return ANZFELDERHORIZONTAL; }
		}

		public int AnzahlFelderVertikal
		{
			get { return ANZFELDERVERTIKAL; }
		}

		/// <summary>
		/// Setzt den Wert für einen Würfel des Brettes
		/// </summary>
		/// <param name="wert">Wert</param>
		/// <param name="x">x Koordinate</param>
		/// <param name="y">y Koordinate</param>
		public void SetWert(int wert, int x, int y)
		{
			brett[x, y] = wert;
		}

		public int GetWert(int x, int y)
		{
			return brett[x, y];
		}
	}
}
