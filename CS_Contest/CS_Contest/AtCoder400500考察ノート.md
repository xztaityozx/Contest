AtCoder 400,500考察ノート
# ABC051 D Candidates of No Shortest Paths
- ワーシャルフロイドで全点間距離を求める
- 入力された`edge[i,j]`が使われているかどうかは、各点sから
    - ```distance[s,i] + edge[i,j].Cost == distance[s,j]```
- を調べる。

# ABC054 D Mixing Experiment
- 典型的なDP
- `dp[N,aiの合計,biの合計] = 最小コスト `
- `dp[i+1,i,j] = Min(dp[i,j,k],dp[i+1,j,k])`
- `dp[i+1,i+ai,j+bi] = Min(dp[i,j,k]+ci,dp[i+1,j+ai,k+bi])`

# ABC057 D Maximum Average Sets
- DPっぽいけど違った
- `v`を降順にソートし先頭からA個の平均が最大値（これは自明）
    - A+1個目を足しても平均は下がる一方（降順に並んでるから）
- v[0]とv[A-1]が同じ値のとき
    - 最大値がA個以上続いているかもしれない
    - vの中にv[0]がX個あるとき
    - 組み合わせ`XCi` `(A<=i<=B)`の合計が答え
- v[0]とv[A-1]が違うとき
    - 先頭からA個のうちに最大値がY個あったとすると答えは XCYになる
- 組み合わせはパスカルの三角形で求める
    - 前計算はO(n^2)
    - アクセスはO(1)
```cs
public static long[,] CombinationTable(int n) {
    var rt = new long[n+1, n+1];
    for (int i = 0; i <= n; i++) {
        for (int j = 0; j <= i; j++) {
            if (j == 0 || i == j) rt[i, j] = 1L;
            else rt[i, j] = (rt[i - 1, j - 1] + rt[i - 1, j]);
        }
    }
    return rt;
}
```

# ABC061 D Score Attack
- 閉路検出が必要だったのでトポロジカルソートかと思ったけど違った
- コストの正負を逆にして最短経路を出す。負数が表れるので使うのはベルマンフォード法
- BellmanFordクラスを作ってループがあるかどうかのプロパティも持たせた
- ベルマンフォードで閉路が見つかったら`inf`、無ければ`-Distance[N-1]`が答え

# ABC070 D Transit Tree Path
- 重み付き木に対して`xi⇒K⇒yi`を通る最短距離の質問に`Q`個答える問題
- 事前に点KからダイクストラしておけばTLEしない
- その後Q個の質問に対して`Distance[xi]+Distance[yi]`を出力すればOK

# ABC073 D joisino's travel
- 訪問しなければならない点を全て訪れたときの最小コストを求める問題
  - 順番は何でもOK
- 制約が小さいのでワーシャルフロイド法＋DFSで解く
- 1次配列で訪れた点を記憶する方法いつも忘れるのでメモ
```cs
//Rは訪れる点の個数
//resはワーシャルフロイドの結果
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
```

# ABC076 D AtCoder Express
- 与えられたN個条件を満たすような台形の面積を求める問題。
- 各点を記憶していくのは実装がバグるので絶対やめた方がいい
- この問題では時刻Xがすべて整数なので、時刻が0.5の倍数のとき傾きが変化する。
- そのため`時刻a(aは0.5の倍数)`での出せる最大の速度を`v(a)`とすると、`(a(a)+a(a+0.5))/4`の合計が答えになる
- 0.5刻みの配列を表現するために時間を2倍に拡張して1要素が0.5を表すようにする。
```cs
var time = 0;

N.REP(i=>(T[i]*2).REP(k =>
{
	box[time] = Min(box[time], V[i]);
	time++;
	box[time] = Min(box[time], V[i]);
}));
```
- 時刻`time`のときにとりうる最大値を`Min`を使って得る。超えてはいけないのでMin
```cs
for (var i = 1; i < sum; i++) 
    box[i] = Min(box[i], box[i - 1] + 0.5, box[i + 1] + 0.5);
for (var i = sum-1; i >= 1; i--) 
    box[i] = Min(box[i], box[i - 1] + 0.5, box[i + 1] + 0.5);
```
- 次に前後から整合性を取る。
  - 時刻`i`の速度はその前後の`±0.5`かそれ自身であるはずなのでそのうちの最小値が正確な値となる
- あとは和を計算して終わり

# ABC079 D Wall
- 数値`x`を`1`にする問題。`x`から`1`にするには`A[1][x]`のコストがかかる。
- `A[][]`が`10×10`なのでワーシャルフロイドで全点間最小距離を求めて-1,1を省いたあとsumすればおわり
- やるだけって感じだけど有向グラフなので注意

# ABC080 D Recording
- 累積和問題。ばぐらせてつらい
- S+0.5の期間～～みたいなことが書いてあるので取りうる数値を2倍にした配列用意してimosする
- このとき各チャンネルごとにimosすると楽になる
- レコーダーは`S-0.5`から使えなくなるので
```cs
imos[ci-1,si*2-1]++;
imos[ci-1,ti*2]--;
```
- とするとうまくいく
```cs
C.REP(i=>{
    (100005*2).REP(j=>{
        if(j!=0) imos[i,j]+=imos[i,j-1];
        if(imos[i,j]>=1)REC[j]++;
    });
});
REC.Max().WL(); // こたえ
```

# ABC084 D 2017-like Number
- 時間中に解けなくて辛すぎた問題。
- 条件にあう素数リストを事前に求めてimosで数え上げをする。
- `yield return`でエラトステネスの篩を高速化（コピペ）したのでメモ
```cs
public static IEnumerable<int> Primes(int maxnum) {
	yield return 2;
	yield return 3;
	var sieve = new BitArray(maxnum + 1);
	int squareroot = (int)Math.Sqrt(maxnum);
	for (int i = 2; i <= squareroot; i++) {
		if (sieve[i] == false) {
			for (int n = i * 2; n <= maxnum; n += i)
				sieve[n] = true;
		}
		for (var n = i * i + 1; n <= maxnum && n < (i + 1) * (i + 1); n++) {
			if (!sieve[n])
				yield return n;
		}
	}
}
```
- `IEnumerable`で`yield`使うのすっかり忘れてた・・・反省したい
  - これに`Where(x=>IsPrime((x+1)/2))`すれば条件に合う素数リストができる
```cs
public static bool IsPrime(int n) {
	if (n == 2) return true;
	if (n < 2||n%2==0) return false;
	var i = 3;
	int sq = (int)Sqrt(n);
	while (i <= sq) {
		if (n % i == 0) return false;
		i += 2;
	}
	return true;
}
```
- 時間中にバグらせてたのは範囲の指定を`imos[ri+1]-imos[li]`じゃなくて`imos[ri]-imos[li-1]`で計算してたこと
  - imosむずい

# ARC058 D いろはちゃんとマス目 / Iroha and a Grid
- 問題をかみ砕くと升目上を最短経路で移動するとき、`(0,0)=>(H-A-1,i)=>(W-1,H-1)`へ移動するパターンを数え上げる問題になる
  - ただし`B <= i < W`
- これを逆に考えると通ってはいけないルートを通るパターンを全体から引けば良いというのが分かる
- `n×k`マスの升目の最短経路の個数は`(n+k)Ck`で計算できる。
  - これを毎回計算するとTLEするのでテーブルを作る
  - `nCk`は、`(n+k)!/(n!k!)`で求められるのでこれを用意したい。
- パスカルの三角形より大きな数を用意できるけど、小さな値ならあっちのがはやい
```cs
var factorial = new long[100001 * 2];
var inverse = new long[100001 * 2];
factorial[0] = inverse[0] = 1;
for (var i = 1; i <  factorial.Length; i++) {
	factorial[i] = Mod(factorial[i - 1] * i);
	inverse[i] = ModPow(factorial[i], ModValue - 2);
}

Func<int, int, long> Combination = (n, k) =>
{
	if (n - k < 0) return 0;
	var rt = factorial[n];
	rt *= inverse[k];
	rt %= ModValue;
	rt *= inverse[n - k];
	return Mod(rt);
};
```
- これで組み合わせに`O(1)`でアクセスできるのでこれを利用する。
- あとは`A×B`の各点を経由して右下まで移動するパターンを全体から引いていくことでAC

# ARC089 D アンバランス / Unbalanced
- 部分文字列に1つの文字が過半数含まれているかを探す問題
  - 部分点はSの長さが100なので全探索でも間に合う
  - 満点取るには全探索じゃTLE
- ある文字列についてその過半数がアルファベットXであるということは
  - XX
  - XYX(Yは任意)
- のどちらかが含まれていることになる。
  - これらがない場合1種類の文字の間に必ず2文字別の文字が挟まることになる
  - 過半数を超えることはないためアンバランスになりえない
- これをチェックすればいいので`O(|S|)`で解ける

# ARC061 D すぬけ君の塗り絵 / Snuke's Coloring
- H×Wマス内の全ての3×3マスに何個の黒マスがあるかを数え上げる問題。
  - 制約が大きいため全探索は不可能
  - 平面imosっぽいけど全然違った
- あるマス(x,y)が黒であったとき、そのマスを含む9個の3×3マスが存在する。
- この9個のマスが何回出てきたかを記録する。
  - 出てきた回数がその3×3マスに含まれる黒マスの個数になる
  - 0個含まれるのは(H-2)×(W-2)から↑で求めた総数を引くと解る
- こういうX,Yの組み合わせを`Dictionary`の`Key`に使う場合`Tuple<long,long>`を使うとTLEする。（した）
  - これを爆速にするには`long`にしまい込む方法がある。
  - 今回は`ai,bi`ともに10^9なので`pos=ai*1000000000+bi`とすれば一意の組み合わせを`Tuple`とか使わずに表現できる。
    - 10倍ぐらい速度が違う