#include<bits/stdc++.h>

using namespace std;

#define rep(i,n) for(int i=0;i<(n);++i)
#define out(S) cout<<(S)<<endl;
#define ShowAll(collection) for(auto i:collection){out(i);}
#define beginend(v) v.begin(),v.end()

using pii=pair<int,int>;
using vb=vector<bool>;
using ll=long long;
using ull=unsigned long long;
using vi=vector<int>;
using vvi=vector<vi>;
using vvc=vector<vector<char>>;
using ti3=tuple<int,int,int>;
template<typename T> void removeAt(vector<T>& v, int index) { v.erase(v.begin() + index); }
#define OCB(c) (c&1)|(c&2)<<2|(c&4)<<4|(c&8)<<6|(c&16)<<8|(c&32)<<10|(c&64)<<12|(c&128)<<14

//dp[i]=iで表せるステージをクリアしたときの最小の攻略時間
int dp[1<<16];

int solve(int n){
  vvi t(n,vi(n+1,0));
  rep(i,n) rep(j,n+1) cin>>t[i][j];
  rep(i,(1<<n)) dp[i]=1e9;

  dp[0]=0;

  rep(bit,1<<n){
    rep(j,n) if(!((bit>>j)&1)) {
      int ns=bit|(1<<j);
      int nc=t[j][0];
      rep(k,n) if((bit>>k)&1) nc=min(nc,t[j][k+1]);
      nc+=dp[bit];
      dp[ns]=min(nc,dp[ns]);
    }
  }
  
  return dp[(1<<n)-1];
}

int main(){
  int n;
  while(cin>>n,n){
    out(solve(n));
  }
}
