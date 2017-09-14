﻿using System;
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
			public void Solve()
			{
				int R, N, M;
				ReadMulti(out N,out M,out R);

				var costGraph=new Library.CostGraph(N);
				var Root = ReadInts();
				REP(M, x =>
				{
					int a, b, c;
					ReadMulti(out a,out b,out c);
					a--;
					b--;
					costGraph.Add(a,b,c);
				});
				costGraph.WarshallFloyd();

				var ans = int.MaxValue;

				foreach (var item in Root) {
					var tmp = Root.Where(x => x != item).ToList();
					var current = item;
					long sum = 0;
					while (tmp.Count>0) {
						var min = long.MaxValue / 2;
						var idx = 0;
						foreach (var indexT in tmp.Select(x => costGraph[current, x]).ToIndexEnumerable()) {
							if (indexT.Value >= min) continue;
							min = indexT.Value;
							idx = indexT.Index;
						}
						current = tmp[idx];
						sum += min;
						tmp.RemoveAt(idx);
					}

				}

				return;
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
}
