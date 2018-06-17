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

struct pos {
  int lx,ly,rx,ry;
  pos(int lx,int ly,int rx,int ry):lx(lx),rx(rx),ly(ly),ry(ry){};
};


bool used[51][51][51][51];
bool bfs(int w, int h){
  vvc roomL(h,vector<char>(w)),roomR(h,vector<char>(w));
  rep(i,51)rep(j,51)rep(k,51)rep(l,51) used[i][j][k][l]=false;
  auto s=pos(0,0,0,0);
  rep(i,h){
    rep(j,w) {cin>>roomL[i][j];if(roomL[i][j]=='L'){s.lx=j;s.ly=i;}}
    rep(j,w) {cin>>roomR[i][j];if(roomR[i][j]=='R'){s.rx=j;s.ry=i;}}
  }

  queue<pos> Q;
  Q.push(s);

  pii dx[4]{pii(1,-1),pii(-1,1),pii(0,0),pii(0,0)};
  pii dy[4]{pii(0,0), pii(0,0) ,pii(1,1),pii(-1,-1)};

  while(!Q.empty()){
    int lx,ly,rx,ry;
    auto src=Q.front();Q.pop();
    lx=src.lx;rx=src.rx;ly=src.ly;ry=src.ry;

    for (int i = 0; i < 4; ++i) {
      int LX,LY,RX,RY;
      LX=lx+dx[i].first;
      RX=rx+dx[i].second;
      LY=ly+dy[i].first;
      RY=ry+dy[i].second;

      if(LX<0||LX>=w) LX=lx;
      if(RX<0||RX>=w) RX=rx;
      if(LY<0||LY>=h) LY=ly;
      if(RY<0||RY>=h) RY=ry;
      if(roomR[RY][RX]=='#') RY=ry,RX=rx;
      if(roomL[LY][LX]=='#') LY=ly,LX=lx;

      
      if(roomR[RY][RX]=='%'&&roomL[LY][LX]=='%') return true;
      if(roomR[RY][RX]=='%') continue;
      if(roomL[LY][LX]=='%') continue;
      
      if(!used[LX][LY][RX][RY])Q.push(pos(LX,LY,RX,RY)),used[LX][LY][RX][RY]=true;
    }
  }
  return false;

}

int main(){
  int W,H;
  while(cin>>W>>H,(W|H)){
    cout << (bfs(W,H)?"Yes":"No") << endl;
  }
}
