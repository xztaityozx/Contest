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
using ulong=unsigned long;
template<typename T> void removeAt(vector<T>& v, int index) { v.erase(v.begin() + index); }
#define OCB(c) (c&1)|(c&2)<<2|(c&4)<<4|(c&8)<<6|(c&16)<<8|(c&32)<<10|(c&64)<<12|(c&128)<<14

bool solve(){
    int N,H,W;
    cin >> N>>W>>H;
    vi ximos(W+1,0),yimos(H+1,0);
    rep(i,N){
        int xi,yi,wi;
        cin >> xi>> yi>> wi;
        {
            int mm=max(0,xi-wi);
            int mx=min(W,xi+wi);
            ximos[mm]++;
            ximos[mx]--;
        }
        {
            int mm=max(0,yi-wi);
            int mx=min(H,yi+wi);
            yimos[mm]++;
            yimos[mx]--;
        }
    }
    auto cnt=0;
    bool rt=true;
    rep(i,W){
        cnt+=ximos[i];
        if(cnt<=0) rt=false;
    }
    cnt=0;
    rep(i,H){
        cnt+=yimos[i];
        if(cnt<=0 && !rt) return false;
    }
    return true;
}

int main(){
    out(solve()?"Yes":"No");
}
