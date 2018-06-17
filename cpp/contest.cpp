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

ll get(ll x,int k){
  return k*((x)/(k-1))+(x%(k-1))+1;
}

ll make(int n,int k){
  ll x=0;
  rep(_,n-1){
    x=get(x,k);
  }
  return x;
}

int main(){
  int n,k;
  cin >> n>> k;
  out(make(n,k));
}
