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
template<typename T> void removeAt(vector<T>& v, int index) { v.erase(v.begin() + index); }

void solve(int n){
  bool box[n][31]{{false}};
  rep(i,n) {
    int f;
    cin >> f;
    rep(k,f) {
      int x;
      cin >> x;
      box[i][x]=true;
    }
  }
  ull dp[n];
  rep(i,n) dp[i] = 1ULL << i;

  for(int i=1;i<=30;++i){
    vector<bool> get(n,false);
    ull sub = 0ULL;
    rep(k,n){
      for(int m=k+1;m<n;++m) if(box[k][i]&&box[m][i]){
        get[k]=get[m]=true;
        sub|=dp[k]|dp[m];
      }
    }
    if(sub==(1ULL<<n)-1){
      cout << i<< endl;
      return;
    }
    rep(i,n) if(get[i]) dp[i]=sub;
  }
  cout << -1 << endl;
}

int main()
{
  int n;
  while(cin>>n,n){
    solve(n);
  }
  return 0;
}
