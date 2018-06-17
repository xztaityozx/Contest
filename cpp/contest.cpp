#include<bits/stdc++.h>

using namespace std;

#define rep(i,n) for(int i=0;i<(n);++i)
#define out(S) cout<<(S)<<endl;
#define ShowAll(collection) for(auto i:collection){out(i);}
#define beginend(v) v.begin(),v.end()

using pii=pair<int,int>;
using ll=long long;
using ull=unsigned long long;
using vi=vector<int>;
using vvi=vector<vi>;
using vvc=vector<vector<char>>;
using ti3=tuple<int,int,int>;
template<typename T> void removeAt(vector<T>& v, int index) { v.erase(v.begin() + index); }


int solve(){
  int n,d,x;
  cin >> n>> d>> x;
  vvi box(d,vi(n));
  rep(i,d)rep(j,n) cin>>box[i][j];

  rep(day,d-1){
    vi dp(100001,0);
    rep(i,x+1)dp[i]=i;
    rep(i,n){
      auto W=box[day][i];
      for(int w=W;w<=x;++w){
        dp[w]=max(dp[w],dp[w-W]+box[day+1][i]);
      }
    }
    int y=0;
    rep(w,x+1) y=max(y,dp[w]);
    x=y;
  }
  return x;
}


int main(){
  cout << solve() << endl;
}
