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
using vvc=vector<vector<char>>;
using ti3=tuple<int,int,int>;
template<typename T> void removeAt(vector<T>& v, int index) { v.erase(v.begin() + index); }


ll solve(int n,int m,int l,int k,int a,int h){
  vector<bool> coldlist(n,false);
  rep(i,l) {
    int li;
    cin >> li;
    coldlist[li]=true;
  }
  //coldlist[h]=true;
  //coldlist[a]=true;

  vector<pii> adj[n];
  rep(i,k){
    int a,b,c;
    cin >> a>>b>>c;
    adj[a].push_back(pii(b,c));
    adj[b].push_back(pii(a,c));
  }

  priority_queue<ti3,vector<ti3>,greater<ti3>> pq;
  pq.push(make_tuple(a,m,0));

  ll dp[101][101]; //dp[i][j]=i番目の街にj分残して到達できる最短時間
  rep(i,101)rep(j,101) dp[i][j]=1e9L;
  dp[a][m]=0;

  while(pq.size()!=0){
    int to,cost,limit;
    tie(to,limit,cost) = pq.top();pq.pop();
    if(dp[to][limit]<cost) continue;

    for(auto e :adj[to]){
      int ct,cc;
      tie(ct,cc)=e;
      if(coldlist[to]){
        //現在地点が回復ポイント
        //m分になるまでt分休む
        rep(t,m+1) if(limit+t<=m){
          auto nl=limit+t-cc;
          if(nl<0) continue;

          auto nc=cc+t+cost;
          if(nc<dp[ct][nl]){
            dp[ct][nl]=nc;
            pq.push(make_tuple(ct,nl,nc));
          }
        }
      }else{
        //回復ポイントでない
        auto nl=limit-cc;
        if(nl<0) continue;
        auto nc=cost+cc;
        if(nc<dp[ct][nl]){
          dp[ct][nl]=nc;
          pq.push(make_tuple(ct,nl,nc));
        }
      }
    }
  }
  ll mm=1e9L;
  rep(i,m) mm=min(mm,dp[h][i]);
  return mm;
}

int main(){
  int N,M,L,K,A,H;
  while(cin >> N >> M >> L>>K>>A>>H,(N|M|L|K|A|H)){
    auto res=solve(N,M,L,K,A,H);
    if(res==1e9L) cout << "Help!" << endl;
    else cout << res << endl;
  }
  
}
