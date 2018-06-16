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
template<typename T> void removeAt(vector<T>& v, int index) { v.erase(v.begin() + index); }


bool isRect(const vvc& sq,int l,int r,int t,int b,char c){
  for(int y=t;y<=b;++y)for(int x=l;x<=r;++x){
    if(sq[y][x]!='-'&&sq[y][x]!=c) return false;
  }
  return true;
}

void trim(vvc& sq,int l,int r,int t,int b){
  for(int y=t;y<=b;++y)for(int x=l;x<=r;++x) sq[y][x]='-';
}

bool isClear(vvc& sq,int h,int w){
  rep(y,h)rep(x,w) if(sq[y][x]!='.'&&sq[y][x]!='-') return false;
  return true;
}

bool solve3(vvc& sq,int h,int w,char c){
  int t=h,b=0,l=w,r=0;
  rep(y,h) rep(x,w) if(sq[y][x]==c){
    t=min(t,y);
    b=max(b,y);
    l=min(l,x);
    r=max(r,x);
  }


  if(isRect(sq,l,r,t,b,c)){
    trim(sq,l,r,t,b);
    return true;
  }
  return false;
}

bool solve2(vvc& sq,int h,int w){
  map<char,bool> hash;
  hash['-']=true;
  hash['.']=true;
  for(char c='A';c<='Z';c++) hash[c]=false;
  rep(y,h)rep(x,w) {
    if(!hash[sq[y][x]]){
      if(solve3(sq,h,w,sq[y][x])) return true;
    }
    hash[sq[y][x]]=true;
  }
  return false;
}

bool solve(){
  int h,w;
  cin >> h>>w;
  vvc box(h,vector<char>(w));
  rep(y,h)rep(x,w){
    cin >> box[y][x];
  }

  while(solve2(box,h,w));
  

  return isClear(box,h,w);
}

int main(){
  int n;
  cin >> n;
  rep(i,n) if(solve()) cout << "SAFE" << endl; else cout << "SUSPICIOUS" << endl;
}
