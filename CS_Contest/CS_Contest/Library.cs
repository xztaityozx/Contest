using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static CS_Contest.IO.IO;
using static System.Math;
using static System.Console;

namespace CS_Contest {
	public static class Utils2 {
		

		public static int UpperBound<T>(this IEnumerable<T> list, T target) where T : IComparable {
			var idx = list.ToList().BinarySearch(target);
			idx = idx < 0 ? ~idx : (idx + 1);
			return Min(idx, list.Count());
		}

		public static int LowerBound<T>(this IEnumerable<T> list, T target) where T : IComparable {
			var idx = list.ToList().BinarySearch(target);
			idx = idx < 0 ? ~idx : idx;
			return Max(0, idx - 1);
		}
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

	}

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
}