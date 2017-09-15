using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace CS_Contest {
	public class Deque<T> {
		T[] buf;
		int offset, count, capacity;
		public int Count => count;
		public Deque(int cap) { buf = new T[capacity = cap]; }
		public Deque() { buf = new T[capacity = 16]; }
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


	public class Graph {
		private List<int> graph;
		

		public Graph(int size) {
			size++;
			graph = new List<int>(size);
		}
		public void Add(int A,int B) {
			graph[A] = B;
			graph[B] = A;
		}

		
		public int this[int a] {
			get => graph[a];
			set => graph[a] = value;
		}
	}

	
}
