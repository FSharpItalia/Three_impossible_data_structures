using System;
using System.Collections.Generic;

public class Program
{
	public class Person
	{
		public string Name {get;set;}
		public int Index {get;set;}
		public double P	{get;set;}
		public bool Accept	{get;set;}
	}

	public class Duhkha
	{
		private List<Person> _circle = new List<Person>();
		private static Random r = new Random();
		public Duhkha(int N)
		{
			for (int i = 0; i < N; i++)
			{
				_circle.Add(new Person()
				{Name = String.Format("a{0}", i), Index = i, P = Prob(i), Accept = true});
			}
		}

		public int Count()
		{return _circle.Count;}

		public static double Prob(int N)
		{return 1.0 / Math.Pow(2, N);}

		public static bool Roll(int index)
		{
			index = Convert.ToInt32(Math.Pow(2, index));
			int rnd = r.Next(0, index);			
			return (rnd == index - 1 ? true: false);
		}

		public void Run(Action<Person> print, int N)
		{
			var i = 0;
			while (i < N)
			{
				Person current = null;
				if (i >= _circle.Count)
					current = _circle[i % _circle.Count];
				else
					current = _circle[i];
				current.Accept = Roll(i);
				if (current.Accept)
				{i = 0;
					current.P = 1;}
				else
					current.P = Prob(i);
				current.Index = i;
				print(current);
				i++;
			}
		}
    
	}

	private static void Print(Person person)
	{
		Console.WriteLine(String.Format("Index:[{0}] Accept:[{1}] Prob:[{2}]", person.Index, person.Accept, person.P));
	}

	public static void Main()
	{
		var d = new Duhkha(3);
		d.Run(Print, 29);
	}
}
