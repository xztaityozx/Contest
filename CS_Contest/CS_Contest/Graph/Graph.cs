using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CS_Contest.Utils;
using static System.Math;

namespace CS_Contest.Graph {
	using Ll=List<long>;

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
			REP(Size, _ => Adjacency.Add(new List<Edge>()));
		}

		public void Add(int s, int t, long c, bool dir = true) {
			Adjacency[s].Add(new Edge(t, c));
			if (!dir) Adjacency[t].Add(new Edge(s, c));
		}
	}

	public class Dijkstra : CostGraph {
		public Dijkstra(int size) : base(size) { }
		public int[] PreviousNodeList { get; set; }
		public long[] Distance { get; set; }

		public void Run(int s) {
			PreviousNodeList = new int[Size];
			Distance = new long[Size];
			REP(Size, _ => Distance[_] = Inf);

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
			REP(Size, _ => rt.Add(new Ll()));

			REP(Size, i => REP(Size, k => rt[i].Add(i == k ? 0 : Inf)));

			ForeachWith(Adjacency, (i, item) => {
				foreach (var k in item) {
					rt[i][k.To] = k.Cost;
				}
			});

			REP(Size, i => REP(Size, j => REP(Size, k => {
				rt[j][k] = Min(rt[j][k], rt[j][i] + rt[i][k]);
			})));

			return rt;
		}
	}
}
