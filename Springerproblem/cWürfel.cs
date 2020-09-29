using System;
using System.Drawing;

namespace Springerproblem
{
	/// <summary>
	/// Beschreibt ein Feld
	/// </summary>
	public class Würfel
	{
		private const int BREITE = 50;
		private const int HÖHE = 50;

		public Würfel()
		{			
		}

		public Rectangle getWürfelriss(int x, int y)
		{
			return new Rectangle(x, y, BREITE, HÖHE);
		}

		public Rectangle getWürfelfläche(int x, int y)
		{
			return new Rectangle(x, y, BREITE+1, HÖHE+1);
		}

		public Rectangle getWürfelinnenfläche(int x, int y)
		{
			return new Rectangle(x+1, y+1, BREITE-1, HÖHE-1);
		}

		public int Breite
		{
			get { return BREITE; }
		}

		public int Höhe
		{
			get { return HÖHE; }
		}
	}
}
