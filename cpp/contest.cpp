#include<iostream>
#include<vector>
#include<string>
#include<algorithm>
#include<functional>

using namespace std;

#define rep(i,n) for(int i=0;i<n;i++)
#define out(S) cout<<(S)<<endl;
#define ShowAll(collection) for(auto i:collection){out(i);}


using pii=pair<int,int>;
using ll=long long;
template<typename T> void removeAt(vector<T>& v, int index) { v.erase(v.begin() + index); }

pair<pii,pii> cut(const pii& cake, int si){


  auto w=cake.first;
  auto d=cake.second;
  auto r=w*2+d*2;
  si%=r;
  //cout << w << " " << d << endl;
  if(si<=w){
    return pair<pii,pii>(
            pii(min(si,w-si),d),
            pii(max(si,w-si),d)
          );
  }else if(si<=w+d){
    return pair<pii,pii>(
          pii(w,min(si-w,d-(si-w))),
          pii(w,max(si-w,d-(si-w)))
        );
  }else if(si<=w*2+d){
    return pair<pii,pii>(
          pii(min(si-d-w,w-(si-d-w)),d),
          pii(max(si-d-w,w-(si-d-w)),d)
        );
  }
  return pair<pii,pii>(
          pii(w,min(si-w-w-d,d-(si-w-w-d))),
          pii(w,max(si-w-w-d,d-(si-w-w-d)))
        );
}

void solve(int n, int w, int d){
  vector<pii> box;
  box.push_back(pii(w,d));

  rep(i,n){
    int pi,si;
    cin >> pi>>si;
    auto res = cut(box[pi-1],si);
    box.push_back(res.first);
    box.push_back(res.second);
    removeAt(box,pi-1);
    //cout << "end step" << endl;
    //for(auto item : box) {
      //cout << item.first << " " << item.second << endl;
    //}
  }
  
  vector<ll> ans;
  for(auto item : box){
    ans.push_back(item.first*item.second);
  }
  sort(ans.begin(),ans.end());

  auto len=ans.size();
  for(int i=0;i<len-1;i++){
    cout << ans[i] << " ";
  }
  cout << ans[len-1] << endl;
}

int main()
{
  int n,w,d;
  cin >> n>>w>>d;
  while(n||w||d){
    solve(n,w,d);
    cin >> n>>w>>d;
  }
  return 0;
}
