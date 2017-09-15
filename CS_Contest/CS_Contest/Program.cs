using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static System.Console;
using static CS_Contest.Utils;
using System.IO;
//using static CS_Contest.Library;

namespace CS_Contest {
	using Li = List<int>;
	using LLi = List<List<int>>;
	using Ls = List<string>;
	using Ll = List<long>;
	using LLl = List<List<long>>;


	internal class Program {
		private static void Main(string[] args) {
			var sw = new StreamWriter(OpenStandardOutput()) { AutoFlush = false };
			SetOut(sw);
			new Calc().Solve();
			Out.Flush();
        }

		private class Calc {
			int R, N, M;
			Li Root;
			private bool[] Used;
			Library.CostGraph costGraph;
			public void Solve()
			{
				ReadMulti(out N,out M,out R);
				Used = Enumerable.Repeat(false, 9).ToArray();
				costGraph=new Library.CostGraph(N);
				Root = ReadInts().Select(x => x - 1).ToList();
				REP(M, x =>
				{
					int a, b, c;
					ReadMulti(out a,out b,out c);
					a--;
					b--;
					costGraph.Add(a,b,c);
				});
				costGraph.WarshallFloyd();

				Dfs(1, -1, 0);
				Ans.WL();
				return;
			}
			private long Ans = long.MaxValue;
			private void Dfs(int cnt,int last,long distance) {
				if (cnt == R + 1) { Ans = Min(Ans, distance);return; }

				for (int i = 0; i < R; i++) if(!Used[i]) {
						Used[i] = true;
						if (last == -1) Dfs(cnt + 1, i, 0);
						else Dfs(cnt + 1, i, distance + costGraph[Root[last], Root[i]]);
						Used[i] = false;
				}
			}
			
		}
	}


	public static class Utils {
		public static int ModValue = (int)(1000000007);
		public static long INF = long.MaxValue;
		public static long Mod(long x) => x % ModValue;
		public static long ModPow(long x, long n) { long tmp = 1; while (n != 0) { if (n % 2 == 1) { tmp = Mod(tmp * x); } x = Mod(x * x); n /= 2; } return tmp; }
		public static long DivMod(long x, long y) => Mod(x * ModPow(y, (long)(1e9 + 5)));
		public static void WL(this object obj) => WriteLine(obj);
		public static void WL(this string obj) => WriteLine(obj);
		public static void WL<T>(this IEnumerable<T> list) => list.ToList().ForEach(x => x.WL());
		public static int ReadInt() => int.Parse(ReadLine());
		public static List<int> ReadInts(char s=' ') => ReadLine().Split(s).Where(x => x != "").Select(int.Parse).ToList();
		public static long ReadLong() => long.Parse(ReadLine());
		public static List<long> ReadLongs(char s=' ') => ReadLine().Split(s).Select(long.Parse).ToList();
		public static void ReadMulti(out int x, out int y) {
			var i = ReadInts(' ');
			x = i[0]; y = i[1];

		}
		public static void ReadMulti(out long x, out long y) {
			var i = ReadLongs(' ');
			x = i[0]; y = i[1];
		}
		public static void ReadMulti(out int x, out int y, out int z) {
			var i = ReadInts(' ');
			x = i[0]; y = i[1]; z = i[2];
		}
		public static void ReadMulti(out int x, out int y, out int z, out int v) {
			var i = ReadInts(' ');
			x = i[0]; y = i[1]; z = i[2]; v = i[3];
		}

		public static string StringJoin<T>(this IEnumerable<T> l, string separator="") => string.Join(separator, l);

		public static long GCD(long m, long n) {
			long tmp;
			if (m < n) { tmp = n; n = m; m = tmp; }
			while (m % n != 0) {
				tmp = n;
				n = m % n;
				m = tmp;
			}
			return n;
		}
		public static long LCM(long m, long n) => m * (n / GCD(m, n));

		public static void REP(int n, Action<int> act) {
			for (int i = 0; i < n; i++) {
				act(i);
			}
		}
		public static void RREP(int n, Action<int> act) {
			for (int i = n - 1; i >= 0; i--) {
				act(i);
			}
		}

		public static void Yes() => "Yes".WL();
		public static void No() => "No".WL();
		public static void YES() => "YES".WL();
		public static void NO() => "NO".WL();


		public static int ManhattanDistance(int x1, int y1, int x2, int y2) => Abs(x2 - x1) + Abs(y2 - y1);


		public struct IndexT<T> {
			public T Value { get; set; }
			public int Index { get; set; }
			public IndexT(T v,int i) { Value = v; Index = i; }
		}
		public static IEnumerable<IndexT<T>> ToIndexEnumerable<T>(this IEnumerable<T> list) => list.Select((x, i) => new IndexT<T>(x, i));

		public static Queue<T> ToQueue<T>(this IEnumerable<T> iEnumerable) {
			var rt=new Queue<T>();
			foreach (var item in iEnumerable) {
				rt.Enqueue(item);
			}
			return rt;
		}


		public static Tuple<TKey, TSource> ToTuple<TKey, TSource>(this KeyValuePair<TKey, TSource> kvp) => new Tuple<TKey, TSource>(kvp.Key, kvp.Value);
		public static IEnumerable<Tuple<TKey, TSource>> ToTupleList<TKey, TSource>(this Dictionary<TKey, TSource> d) => d.Select(_ => _.ToTuple());
	}
	public static class Library {
		public struct UnionFind {
			private int[] data;

			public UnionFind(int size) {
				data = new int[size];
				for (var i = 0; i < size; i++) data[i] = -1;
			}

			public bool Unite(int x, int y) {
				x = Root(x);
				y = Root(y);

				if (x != y) {
					if (data[y] < data[x]) {
						var tmp = y;
						y = x;
						x = tmp;
					}
					data[x] += data[y];
					data[y] = x;
				}
				return x != y;
			}

			public bool IsSameGroup(int x, int y) {
				return Root(x) == Root(y);
			}

			private int Root(int x) {
				return data[x] < 0 ? x : data[x] = Root(data[x]);
			}
		}

		public class CostGraph {
			private List<long>[] list;
			private int Size;
			private const long INF = long.MaxValue / 2;
			private List<List<Edge>> edge;
			public struct Edge {
				public int From { get; set; }
				public int To { get; set; }
				public long Cost { get; set; }
				public Edge(int f, int t, long c) { From = f; To = t; Cost = c; }
				public static bool operator <(Edge e1, Edge e2) => e1.Cost < e2.Cost;
				public static bool operator >(Edge e1, Edge e2) => e1.Cost > e2.Cost;
			}

			public CostGraph(int size) {
				Size = size;
				list = Enumerable.Range(0, Size).Select(x => new List<long>()).ToArray();
				Utils.REP(Size, i => {
					Utils.REP(Size, k =>
					{
						list[i].Add(i == k ? 0 : INF);
					});
				});
				edge = Enumerable.Range(0, Size).Select(x => new List<Edge>()).ToList();
			}

			public void Add(int A, int B, long C, bool direction = true) {
				this[A, B] = Min(this[A, B], C);
				edge[A].Add(new Edge(A, B, C));
				if (!direction) return;
				this[B, A] = this[A, B]; edge[B].Add(new Edge(B, A, C));
			}
			public long this[int A, int B] {
				get { return list[A][B]; }
				set { list[A][B] = value; }
			}

			public List<long> this[int index] {
				get { return list[index]; }
				set { list[index] = value; }
			}

			public void Clear() {
				for (var i = 0; i < Size; i++) {
					list[i].Clear();
				}
				edge.Clear();
			}
			/// <summary>
			/// ダイクストラ
			/// </summary>
			/// <param name="s"></param>
			/// <param name="t"></param>
			/// <returns></returns>
			public long Dijkstra(int s, int t) {
				var dist = Enumerable.Repeat(INF, Size).ToList();
				dist[s] = 0;
				var priorityQueue = new PriorityQueue<Tuple<long, int>>();
				priorityQueue.Enqueue(new Tuple<long, int>(0, s));
				while (priorityQueue.Count != 0) {
					var src = priorityQueue.Dequeue();
					if (dist[src.Item2] < src.Item1) continue;

					for (var dest = 0; dest < Size; dest++) {
						var cost = this[src.Item2, dest];
						if (cost == INF || dist[dest] <= dist[src.Item2] + cost) continue;
						dist[dest] = dist[src.Item2] + cost;
						priorityQueue.Enqueue(new Tuple<long, int>(dist[dest], dest));
					}
				}
				return dist[t];
			}

			public void WarshallFloyd() {
				Utils.REP(Size, k => {
					Utils.REP(Size, i => {
						Utils.REP(Size, j => this[i, j] = Min(this[i, j], this[i, k] + this[k, j]));
					});
				});
			}
			/// <summary>
			/// べルマンフォード
			/// </summary>
			/// <param name="s">start</param>
			/// <param name="n">node count</param>
			/// <returns></returns>
			public Tuple<List<long>, bool> BellmanFord(int s, int n) {
				var dist = Enumerable.Repeat(INF, Size).ToList();
				dist[s] = 0;
				for (var i = 0; i < n; i++) {
					for (var v = 0; v < n; v++) {
						foreach (var item in edge[v]) {
							if (dist[v] != INF && dist[item.To] > dist[v] + item.Cost) {
								dist[item.To] = dist[v] + item.Cost;
								if (i == n - 1) return new Tuple<List<long>, bool>(dist, true);
							}
						}
					}
				}
				return new Tuple<List<long>, bool>(dist, false);
			}

			public long Kruskal() {
				var sorted = new List<Edge>();
				foreach (var item in edge) {
					sorted.AddRange(item);
				}
				sorted = sorted.OrderBy(x => x).ToList();
				var uf = new UnionFind();
				long min_cost = 0;

				foreach (var e in sorted) {
					if (!uf.IsSameGroup(e.From, e.To)) {
						min_cost += e.Cost;
						uf.Unite(e.From, e.To);
					}
				}
				return min_cost;
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
			public PriorityQueue() : this(Comparer<T>.Default) { }
			public PriorityQueue(IComparer<T> comparer) : this(16, comparer.Compare) { }
			public PriorityQueue(Comparison<T> comparison) : this(16, comparison) { }
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
			public T Peek() { return heap[0]; }
			public int Count => size;
			public bool Any() { return size > 0; }
		}


	}

}
