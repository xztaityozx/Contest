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

int N,M,K,D[16];
bool used[1<<16];
int G[101][101];

int solve(){
  cin >> N>>M>>K;
  rep(i,1<<16) used[i]=false;
  rep(i,M) cin>>D[i],D[i]--;
  rep(i,N)rep(j,K){
    int v;
    cin >> v;
    v--;
    G[i][j]=-1;
    rep(k,M) if(i==D[k]) rep(l,M) if(v==D[l]) G[i][j]=l;
  }

  queue<pii> Q;
  Q.push(pii((1<<M)-1,0));

  while(!Q.empty()){
    int s,distance;
    tie(s,distance)=Q.front();Q.pop();

    if(used[s]) continue;
    used[s]=true;
    if(s==0) return distance;

    rep(i,K){
      int t=0;
      rep(j,M) if(((s>>j)&1) && G[D[j]][i] >= 0) t|=1<<G[D[j]][i];
      Q.push(pii(t,distance+1));
    }
  }
  return 0;
}

int main(){
  out(solve());
}
