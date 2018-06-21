#include<bits/stdc++.h>

using namespace std;

#define rep(i,n) for(int i=0;i<(n);++i)
#define out(S) cout<<(S)<<endl;
#define ShowAll(collection) for(auto i:collection){out(i);}
#define beginend(v) v.begin(),v.end()
#define Foreach(item,collection) for(auto item : collection)

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


int main(){
  auto f=[](int X)->int{
    if(X<0) return 0;
    if(X>255) return 255;
    return X;
  };
  for(int x=-200;x<=300;++x) out(f(x));
}
