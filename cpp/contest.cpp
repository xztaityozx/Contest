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

string S;
int Len;
bool used[500];
int dfs(int n){
  if(n==Len) return Len;

  //まずm
  if(S[n]!='m') return Len;
  used[n]=true;
  //CAT
  n++;
  bool rt=true;
  //mが見つかるたび潜る
  while(n<Len&&S[n]=='m'){
    n=dfs(n);
  }
  //次はe
  if(n>=Len||S[n]!='e') return Len;
  used[n]=true;
  n++;
  //mが見つかるたび潜る
  while(n<Len&&S[n]=='m'){
    n=dfs(n);
  }
  //最後はw
  if(n>=Len||S[n]!='w') return Len;
  used[n]=true;
  n++;
  return n;
}

bool solve(){
  cin >> S;
  Len = S.size();
  rep(i,Len) used[i]=false;
  auto res = dfs(0);
  if(res!=Len) return false;

  rep(i,Len) if(!used[i]) return false;
  return true;
}

int main(){
  out(solve()?"Cat":"Rabbit");
}
