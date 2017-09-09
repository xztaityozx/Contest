using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace CS_Contest {
	public static class Library {
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
			public int Count { get { return size; } }
			public bool Any() { return size > 0; }
		}
		/// <summary>
		/// Vector2
		/// </summary>
		public class Vector2 {
			public int X { get; set; }
			public int Y { get; set; }
			public Vector2() { X = 0; Y = 0; }
			public Vector2(int x, int y) { X = x; Y = y; }
			public int ManhattanDistance(Vector2 v2) {
				return Utils.ManhattanDistance(X, Y, v2.X, v2.Y);
			}
			public static int ManhattanDistance(Vector2 v1, Vector2 v2) => v2.ManhattanDistance(v2);
			public double Distance(Vector2 v2) => Sqrt(Pow(X - v2.X, 2) + Pow(Y - v2.Y, 2));
			public static Vector2 Zero { get { return new Vector2(); } }
			public void Clear() { X = 0; Y = 0; }

		}
		public class LongVector2 {
			public long X { get; set; }
			public long Y { get; set; }
			public LongVector2() { X = 0; Y = 0; }
			public LongVector2(long x, long y) { X = x; Y = y; }
			public long ManhattanDistance(Vector2 v2) {
				return Abs(X - v2.X) + Abs(Y - v2.Y);
			}
			public double Distance(Vector2 v2) => Sqrt(Pow(X - v2.X, 2) + Pow(Y - v2.Y, 2));
			public static Vector2 Zero { get { return new Vector2(); } }
			public void Clear() { X = 0; Y = 0; }

		}


		public class Dijkstras {
			List<List<int>> Cost_l;
			List<int> Prev;
			public Dijkstras(List<List<int>> Cost) { Cost_l = Cost; }
			public long Dijkstra(int s,int g) {
				int N = Cost_l.Count;
				var dist = Enumerable.Repeat(long.MaxValue, N).ToList();
				Prev = Enumerable.Repeat(-1, N).ToList();
				dist[s] = 0;
				PriorityQueue<Tuple<long,int>> Q = new PriorityQueue<Tuple<long,int>>();
				Q.Enqueue(new Tuple<long, int>(0, s));
				while (Q.Count!=0) {
					var src = Q.Dequeue();
					if (dist[src.Item2] < src.Item1) continue;

					Utils.REP(N, dest => {
						var cost = Cost_l[src.Item2][dest];
						if (cost != int.MaxValue && dist[dest] > dist[src.Item2] + cost) {
							dist[dest] = dist[src.Item2] + cost;
							Q.Enqueue(new Tuple<long, int>(dist[dest], dest));
							Prev[dest] = src.Item2;
						}
					});
				}
				return dist[g];
			}
			public List<int> GetPath(int g) {
				var rt = new List<int>();
				rt.Add(g);
				int dest = g;
				while (Prev[dest] != -1) {
					rt.Add(Prev[dest]);
					dest = Prev[dest];
				}
				rt.Reverse();
				return rt;
			}
		}
	}
	public class Deque<T> {
		T[] buf;
		int offset, count, capacity;
		public int Count { get { return count; } }
		public Deque(int cap) { buf = new T[capacity = cap]; }
		public Deque() { buf = new T[capacity = 16]; }
		public T this[int index] {
			get { return buf[getIndex(index)]; }
			set { buf[getIndex(index)] = value; }
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
			for (int i = 0; i < index; i++)
				this[i] = this[i + 1];
			this[index] = item;
		}
		public T RemoveAt(int index) {
			if (index < 0 || index >= count) throw new IndexOutOfRangeException();
			var ret = this[index];
			for (int i = index; i > 0; i--)
				this[i] = this[i - 1];
			this.PopFront();
			return ret;
		}
		private void Extend() {
			T[] newBuffer = new T[capacity << 1];
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
				for (int i = 0; i < count; i++)
					a[i] = this[i];
				return a;
			}
		}
	}

	public class XDictionary<TKey, TSource> :Dictionary<TKey,TSource>, IEnumerable<KeyValuePair<TKey, TSource>> {
		new public TSource  this [TKey index] {
			get { if (ContainsKey(index)) { return this[index]; } else { return default(TSource); } }
			set { if (ContainsKey(index)) { this[index] = value; } else { Add(index, value); } }
		}
		/// <summary>
		/// Keyの追加を試みます
		/// </summary>
		/// <param name="key"></param>
		/// <param name="src"></param>
		/// <returns></returns>
		public bool TryAdd(TKey key,TSource src) {
			if (ContainsKey(key)) return false;
			Add(key, src);
			return true;
		}
	}
}
