using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Math;
using static System.Console;

namespace CS_Contest {
	public static class Local
	{
		public static void Main(string[] args) {
			Stopwatch stopwatch=new Stopwatch();
			var sw = new StreamWriter(OpenStandardOutput()) { AutoFlush = false };
			SetOut(sw);
			stopwatch.Start();
			new Program.Calc().Solve();
			Out.Flush();
			stopwatch.Stop();
			"===============End of Solve================".WL();
			(stopwatch.ElapsedMilliseconds+" ms").WL();
			Out.Flush();
			
		}
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

			public struct Edge :IComparable {
				public int From { get; set; }
				public int To { get; set; }
				public long Cost { get; set; }

				public Edge(int f, int t, long c) {
					From = f; To = t; Cost = c;
				}

				public static bool operator <(Edge e1, Edge e2) => e1.Cost < e2.Cost;

				public static bool operator >(Edge e1, Edge e2) => e1.Cost > e2.Cost;

				public int CompareTo(object obj) {
					return Cost.CompareTo(obj);
				}
			}

			public CostGraph(int size) {
				Size = size;
				list = Enumerable.Range(0, Size).Select(x => new List<long>()).ToArray();
				Utils.REP(Size, i => {
					Utils.REP(Size, k => {
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
				sorted = sorted.OrderBy(x => x.Cost).ToList();
				var uf = new UnionFind(sorted.Count);
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
	}

	public class Deque<T> {
		private T[] buf;
		private int offset, count, capacity;
		public int Count => count;

		public Deque(int cap) {
			buf = new T[capacity = cap];
		}

		public Deque() {
			buf = new T[capacity = 16];
		}

		public T this[int index] {
			get => buf[getIndex(index)];
			set => buf[getIndex(index)] = value;
		}

		private int getIndex(int index) {
			if (index >= capacity)
				throw new IndexOutOfRangeException("out of range");
			var ret = index + offset;
			if (ret >= capacity)
				ret -= capacity;
			return ret;
		}

		public void PushFront(T item) {
			if (count == capacity) Extend();
			if (--offset < 0) offset += buf.Length;
			buf[offset] = item;
			++count;
		}

		public T PopFront() {
			if (count == 0)
				throw new InvalidOperationException("collection is empty");
			--count;
			var ret = buf[offset++];
			if (offset >= capacity) offset -= capacity;
			return ret;
		}

		public void PushBack(T item) {
			if (count == capacity) Extend();
			var id = count++ + offset;
			if (id >= capacity) id -= capacity;
			buf[id] = item;
		}

		public T PopBack() {
			if (count == 0)
				throw new InvalidOperationException("collection is empty");
			return buf[getIndex(--count)];
		}

		public void Insert(int index, T item) {
			if (index > count) throw new IndexOutOfRangeException();
			this.PushFront(item);
			for (var i = 0; i < index; i++)
				this[i] = this[i + 1];
			this[index] = item;
		}

		public T RemoveAt(int index) {
			if (index < 0 || index >= count) throw new IndexOutOfRangeException();
			var ret = this[index];
			for (var i = index; i > 0; i--)
				this[i] = this[i - 1];
			this.PopFront();
			return ret;
		}

		private void Extend() {
			var newBuffer = new T[capacity << 1];
			if (offset > capacity - count) {
				var len = buf.Length - offset;
				Array.Copy(buf, offset, newBuffer, 0, len);
				Array.Copy(buf, 0, newBuffer, len, count - len);
			}
			else Array.Copy(buf, offset, newBuffer, 0, count);
			buf = newBuffer;
			offset = 0;
			capacity <<= 1;
		}

		public T[] Items//デバッグ時に中身を調べるためのプロパティ
		{
			get {
				var a = new T[count];
				for (var i = 0; i < count; i++)
					a[i] = this[i];
				return a;
			}
		}
	}

	public class XDictionary<TKey, TSource> : Dictionary<TKey, TSource>, IEnumerable<KeyValuePair<TKey, TSource>> {

		new public TSource this[TKey index] {
			get { if (ContainsKey(index)) { return this[index]; } else { return default(TSource); } }
			set { if (ContainsKey(index)) { this[index] = value; } else { Add(index, value); } }
		}

		/// <summary>
		/// Keyの追加を試みます
		/// </summary>
		/// <param name="key"></param>
		/// <param name="src"></param>
		/// <returns></returns>
		public bool TryAdd(TKey key, TSource src) {
			if (ContainsKey(key)) return false;
			Add(key, src);
			return true;
		}
	}

	public class Graph {
		private List<int> graph;

		public Graph(int size) {
			size++;
			graph = new List<int>(size);
		}

		public void Add(int A, int B) {
			graph[A] = B;
			graph[B] = A;
		}

		public int this[int a] {
			get => graph[a];
			set => graph[a] = value;
		}


		/// <summary>
		/// 橋の数を数える
		/// </summary>
		/// <returns></returns>
		public int GetBridgeCount() {
			var g = new List<List<int>>();

			int N, M;
			Utils.ReadMulti(out N, out M);
			Utils.REP(N, _ => g.Add(new List<int>()));
			var used_v = Utils.Range(N, x => false);
			var used_e = Utils.Range(N, _ => Utils.Range(N, x => false));
			var ord = new int[N];
			var lowlink = new int[N];
			int k = 0;

			var edges = new List<Tuple<int, int>>();

			Utils.REP(M, _ =>
			{
				int a, b;
				Utils.ReadMulti(out a, out b);
				a--;
				b--;
				g[a].Add(b);
				g[b].Add(a);
				edges.Add(new Tuple<int, int>(a, b));
				edges.Add(new Tuple<int, int>(b, a));

			});

			Func<int, bool> dfs = null;
			dfs = (v) =>
			{
				used_v[v] = true;
				ord[v] = lowlink[v] = k++;
				for (int i = 0; i < g[v].Count; i++)
				{
					if (!used_v[g[v][i]])
					{
						used_e[v][g[v][i]] = true;
						dfs(g[v][i]);
						lowlink[v] = Min(lowlink[v], lowlink[g[v][i]]);
					}
					else if (!used_e[g[v][i]][v])
					{
						lowlink[v] = Min(lowlink[v], ord[g[v][i]]);
					}
				}
				return true;
			};

			dfs(0);

			int ans = 0;

			foreach (var tuple in edges)
			{
				var u = tuple.Item1;
				var v = tuple.Item2;
				if (ord[u] < lowlink[v]) ans++;
			}

			return ans;
		}
	}
}