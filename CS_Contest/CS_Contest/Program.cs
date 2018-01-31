using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using static System.Console;
using static System.Math;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using CS_Contest.Graph;
using CS_Contest.Loop;
//using CS_Contest.Utils;
using static Nakov.IO.Cin;
using static CS_Contest.Utils.Utils;

namespace CS_Contest {
	using Li = List<int>;
	using LLi = List<List<int>>;
	using Ll = List<long>;
	using ti3=Tuple<int,int,int>;
	internal class Program {
		private static void Main(string[] args) {
			var sw = new StreamWriter(OpenStandardOutput()) { AutoFlush = false };
			SetOut(sw);
			new Calc().Solve();
			Out.Flush();
		}

		public class Calc {
			public void Solve() {
				int N = NextInt(), M = NextInt(), R = NextInt();
				var rList = GetIntList().Select(_ => _ - 1).ToList();
				var wf=new WarshallFloyd(N);
				M.REP(i =>
				{
					int ai = NextInt(), bi = NextInt(), ci = NextInt();
					ai--;
					bi--;
					wf.Add(ai, bi, ci, false);
				});

				var res=wf.Run();

				var min = long.MaxValue;

				var used = Enumerable.Repeat(false, R).ToList();

				Func<int, int, long, bool> dfs = null;
				dfs = (step, before, distance) =>
				{
					if (step == R + 1) {
						min = Min(min, distance);
						return true;
					}

					R.REP(i =>
					{
						if (used[i]) return;
						used[i] = true;
						if (before == -1) dfs(step + 1, i, 0);
						else dfs(step + 1, i, distance + res[rList[i]][rList[before]]);
						used[i] = false;
					});
					return true;
				};
				dfs(1, -1, 0);
				min.WL();

				return;
			}
		}
	}





}
namespace Nakov.IO {
	using System;
	using System.Text;
	using System.Globalization;

	public static class Cin {
		public static string NextToken() {
			StringBuilder tokenChars = new StringBuilder();
			bool tokenFinished = false;
			bool skipWhiteSpaceMode = true;
			while (!tokenFinished) {
				int nextChar = Console.Read();
				if (nextChar == -1) {
					tokenFinished = true;
				} else {
					char ch = (char)nextChar;
					if (char.IsWhiteSpace(ch)) {
						if (!skipWhiteSpaceMode) {
							tokenFinished = true;
							if (ch == '\r' && (Environment.NewLine == "\r\n")) {
								Console.Read();
							}
						}
					} else {
						skipWhiteSpaceMode = false;
						tokenChars.Append(ch);
					}
				}
			}

			string token = tokenChars.ToString();
			return token;
		}

		public static int NextInt() {
			string token = Cin.NextToken();
			return int.Parse(token);
		}
		public static long NextLong() {
			string token = Cin.NextToken();
			return long.Parse(token);
		}
		public static double NextDouble(bool acceptAnyDecimalSeparator = true) {
			string token = Cin.NextToken();
			if (acceptAnyDecimalSeparator) {
				token = token.Replace(',', '.');
				double result = double.Parse(token, CultureInfo.InvariantCulture);
				return result;
			} else {
				double result = double.Parse(token);
				return result;
			}
		}
		public static decimal NextDecimal(bool acceptAnyDecimalSeparator = true) {
			string token = Cin.NextToken();
			if (acceptAnyDecimalSeparator) {
				token = token.Replace(',', '.');
				decimal result = decimal.Parse(token, CultureInfo.InvariantCulture);
				return result;
			} else {
				decimal result = decimal.Parse(token);
				return result;
			}
		}

	}
}

namespace CS_Contest.Graph
{
	using Ll=List<long>;
	using Li=List<int>;
	public class BellmanFord : CostGraph {
		public BellmanFord(int size) : base(size) {
		}

		public List<long> Distance { get; set; }

		private bool[] _negative;
		public bool HasCycle => _negative[Size - 1];

		public void Run(int s) {
			Distance = new Ll();
			Size.REP(i => Distance.Add(Inf));
			Distance[s] = 0;
			_negative = new bool[Size];

			(Size - 1).REP(i => Size.REP(j => Adjacency[j].Count.REP(k =>
				{
					var src = Adjacency[j][k];
					if (Distance[src.To] > Distance[j] + src.Cost) Distance[src.To] = Distance[j] + src.Cost;
				}
			)));

			for (int i = 0; i < Size; i++) {
				Size.REP(j => {
					Adjacency[j].Count.REP(k => {
						var src = Adjacency[j][k];
						if (Distance[src.To] > Distance[j] + src.Cost) {
							Distance[src.To] = Distance[j] + src.Cost;
							_negative[src.To] = true;
						}
						if (_negative[j]) _negative[src.To] = true;
					});
				});
			}
		}
	}
	public class CostGraph {
		public struct Edge {
			public int To { get; set; }
			public long Cost { get; set; }


			public Edge(int to, long cost) {
				To = to;
				Cost = cost;
			}

		}

		public int Size { get; set; }
		public List<List<Edge>> Adjacency { get; set; }
		public const long Inf = (long)1e15;

		public CostGraph(int size) {
			Size = size;
			Adjacency = new List<List<Edge>>();
			Size.REP(_ => Adjacency.Add(new List<Edge>()));
		}

		public void Add(int s, int t, long c, bool dir = true) {
			Adjacency[s].Add(new Edge(t, c));
			if (!dir) Adjacency[t].Add(new Edge(s, c));
		}

	}

	/// <summary>
	/// 優先度付きキュー
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PriorityQueue<T> {
		private readonly List<T> heap;
		private readonly Comparison<T> compare;
		private int size;

		public PriorityQueue() : this(Comparer<T>.Default) {
		}

		public PriorityQueue(IComparer<T> comparer) : this(16, comparer.Compare) {
		}

		public PriorityQueue(Comparison<T> comparison) : this(16, comparison) {
		}

		public PriorityQueue(int capacity, Comparison<T> comparison) {
			this.heap = new List<T>(capacity);
			this.compare = comparison;
		}

		public void Enqueue(T item) {
			this.heap.Add(item);
			var i = size++;
			while (i > 0) {
				var p = (i - 1) >> 1;
				if (compare(this.heap[p], item) <= 0)
					break;
				this.heap[i] = heap[p];
				i = p;
			}
			this.heap[i] = item;
		}

		public T Dequeue() {
			var ret = this.heap[0];
			var x = this.heap[--size];
			var i = 0;
			while ((i << 1) + 1 < size) {
				var a = (i << 1) + 1;
				var b = (i << 1) + 2;
				if (b < size && compare(heap[b], heap[a]) < 0) a = b;
				if (compare(heap[a], x) >= 0)
					break;
				heap[i] = heap[a];
				i = a;
			}
			heap[i] = x;
			heap.RemoveAt(size);
			return ret;
		}

		public T Peek() {
			return heap[0];
		}

		public int Count => size;

		public bool Any() {
			return size > 0;
		}
	}


	public class Dijkstra : CostGraph {
		public Dijkstra(int size) : base(size) { }
		public int[] PreviousNodeList { get; set; }
		public long[] Distance { get; set; }

		public void Run(int s) {
			PreviousNodeList = new int[Size];
			Distance = new long[Size];
			Size.REP(_ => Distance[_] = Inf);

			var pq = new PriorityQueue<Edge>((x, y) => x.Cost.CompareTo(y.Cost));
			Distance[s] = 0;
			pq.Enqueue(new Edge(s, 0));
			while (pq.Any()) {
				var src = pq.Dequeue();
				if (Distance[src.To] < src.Cost) continue;
				for (var i = 0; i < Adjacency[src.To].Count; i++) {
					var tmp = Adjacency[src.To][i];
					var cost = tmp.Cost + src.Cost;
					if (cost >= Distance[tmp.To]) continue;
					Distance[tmp.To] = cost;
					pq.Enqueue(new Edge(tmp.To, cost));
					PreviousNodeList[tmp.To] = src.To;
				}
			}
		}
	}


	public class WarshallFloyd : CostGraph {
		public WarshallFloyd(int size) : base(size) {
		}

		public List<Ll> Run() {
			var rt = new List<Ll>();
			Size.REP(_ => rt.Add(new Ll()));

			Size.REP(i => Size.REP(k => rt[i].Add(i == k ? 0 : Inf)));

			ForeachWith(Adjacency, (i, item) => {
				foreach (var k in item) {
					rt[i][k.To] = k.Cost;
				}
			});

			Size.REP(i => Size.REP(j => Size.REP(k => {
				rt[j][k] = Min(rt[j][k], rt[j][i] + rt[i][k]);
			})));

			return rt;
		}
	}

}

namespace CS_Contest.Loop
{
	public static class Loop
	{
		public static void REP(this int n, Action<int> act) {
			for (var i = 0; i < n; i++) {
				act(i);
			}
		}
	}
}

namespace CS_Contest.Utils
{
	using Li=List<int>;
	using Ll=List<long>;
	public static class Utils {


		public static long[,] CombinationTable(int n) {
			var rt = new long[n + 1, n + 1];
			for (int i = 0; i <= n; i++) {
				for (int j = 0; j <= i; j++) {
					if (j == 0 || i == j) rt[i, j] = 1L;
					else rt[i, j] = (rt[i - 1, j - 1] + rt[i - 1, j]);
				}
			}
			return rt;
		}

		public static void WL(this object obj) => WriteLine(obj);

		public static void WL(this string obj) => WriteLine(obj);


		public static void WL<T>(this IEnumerable<T> list) => list.ToList().ForEach(x => x.WL());

		public static Li GetIntList() => ReadLine().Split().Select(int.Parse).ToList();
		public static Ll GetLongList() => ReadLine().Split().Select(long.Parse).ToList();

		public static string StringJoin<T>(this IEnumerable<T> l, string separator = "") => string.Join(separator, l);

		public static void ForeachWith<T>(IEnumerable<T> ie, Action<int, T> act) {
			var i = 0;
			foreach (var item in ie) {
				act(i, item);
				i++;
			}
		}


		public static int ManhattanDistance(int x1, int y1, int x2, int y2) => Abs(x2 - x1) + Abs(y2 - y1);

		public struct IndexT<T> {
			public T Value { get; set; }
			public int Index { get; set; }

			public IndexT(T v, int i) {
				Value = v; Index = i;
			}
			public override string ToString() {
				return Value + " " + Index;
			}
		}

		public static IEnumerable<IndexT<T>> ToIndexEnumerable<T>(this IEnumerable<T> list) => list.Select((x, i) => new IndexT<T>(x, i));

		public static Queue<T> ToQueue<T>(this IEnumerable<T> iEnumerable) {
			var rt = new Queue<T>();
			foreach (var item in iEnumerable) {
				rt.Enqueue(item);
			}
			return rt;
		}

		public static IndexT<T> IndexOf<T>(this IEnumerable<T> ie, Func<IndexT<T>, IndexT<T>, IndexT<T>> func) =>
			ie.ToIndexEnumerable().Aggregate(func);

		public static void Swap<T>(ref T x, ref T y) {
			var tmp = x;
			x = y;
			y = tmp;
		}
		public static Dictionary<TKey, int> CountUp<TKey>(this IEnumerable<TKey> l) {
			var dic = new Dictionary<TKey, int>();
			foreach (var item in l) {
				if (dic.ContainsKey(item)) dic[item]++;
				else dic.Add(item, 1);
			}
			return dic;
		}
		public static int Count<T>(this IEnumerable<T> l, T target) => l.Count(x => x.Equals(target));
	}


}