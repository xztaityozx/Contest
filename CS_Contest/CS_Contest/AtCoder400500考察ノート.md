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