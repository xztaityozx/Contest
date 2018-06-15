#include<bits/stdc++.h>

using namespace std;

#define rep(i,n) for(int i=0;i<(n);++i)
#define out(S) cout<<(S)<<endl;
#define ShowAll(collection) for(auto i:collection){out(i);}
#define beginend(v) v.begin(),v.end()

using pii=pair<int,int>;
using ll=long long;
template<typename T> void removeAt(vector<T>& v, int index) { v.erase(v.begin() + index); }


bool box[21][41];
void solve(int r,int n){
  rep(y,21) rep(x,41) box[y][x]=false;
  rep(i,n) {
    int li,ri,hi;
    cin >> li >> ri >> hi;

    for(int y=0;y<hi;++y) {
      for(int x=li+20;x<ri+20;++x){
        box[y][x]=true;
      }
    }
  }
  auto ans=numeric_limits<double>::max();
  rep(y,21){
    for(int x=-r+20;x<r+20;++x) {
      if(!box[y][x]){
        double time;
        if(x<20) time = (double) y+r-sqrt(pow((double)r,2.0)-pow(x-19.0, 2.0));
        else time = (double) y+r-sqrt(pow((double)r,2.0)-pow(x-20.0, 2.0));
        ans=min(ans,time);
      }
    }
  }
  printf("%.4lf\n",ans);
}

int main()
{
  int r,n;
  while(cin>>r>>n && r){
    solve(r,n);
  }
  return 0;
}
