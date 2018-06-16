#include<bits/stdc++.h>

using namespace std;

#define rep(i,n) for(int i=0;i<(n);++i)
#define out(S) cout<<(S)<<endl;
#define ShowAll(collection) for(auto i:collection){out(i);}
#define beginend(v) v.begin(),v.end()

using pii=pair<int,int>;
using ll=long long;
template<typename T> void removeAt(vector<T>& v, int index) { v.erase(v.begin() + index); }

struct node {
    vector<node*> next;
    ll cost;
    node* before;
    node(ll c) : cost(c){};
    node():cost(0){};
};

node* make(string s){
    node* root = new node();
    auto pos=root;
    int c=0,n=s.size();
    while (1){
        if(c>=n) break;
        if(s[c]=='['){
            node* tmp=new node();
            tmp->before=pos;
            pos->next.push_back(tmp);
            pos=tmp;
            c++;
        }else if(s[c]==']'){
            pos=pos->before;
            c++;
        }else{
            int i=c;
            int num=0;
            while(1) {
                if(!isdigit(s[i])) break; else num=num*10+(s[i]-'0');
                i++;
            }
            c=i;
            pos->cost=(num+1)/2;
        }
    }
    return root;
}

ll dfs(node* now){
    if(now->cost!=0) return now->cost;
    int n=(now->next).size();
    vector<ll> sum;
    rep(i,n){
        sum.push_back(dfs(now->next[i]));
    }
    sort(beginend(sum));
    ll rt=0;
    for (int j = 0; j < (n + 1) / 2; ++j) {
        rt+=sum[j];
    }
    return rt;
}

int solve(){
  string s;
  cin >> s;
  auto root=make(s);
  cout<<dfs(root)<<endl;
}

int main()
{
    int n;
    cin >> n;
    rep(i,n) solve();
  return 0;
}
