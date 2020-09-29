using System;
using System.Drawing;

namespace Springerproblem
{
	/// <summary>
	/// Beschreibt ein Feld
	/// </summary>
	public class W�rfel
	{
		private const int BREITE = 50;
		private const int H�HE = 50;

		public W�rfel()
		{			
		}

		public Rectangle getW�rfelriss(int x, int y)
		{
			return new Rectangle(x, y, BREITE, H�HE);
		}

		public Rectangle getW�rfelfl�che(int x, int y)
		{
			return new Rectangle(x, y, BREITE+1, H�HE+1);
		}

		public Rectangle getW�rfelinnenfl�che(int x, int y)
		{
			return new Rectangle(x+1, y+1, BREITE-1, H�HE-1);
		}

		public int Breite
		{
			get { return BREITE; }
		}

		public int H�he
		{
			get { return H�HE; }
		}
	}
}
