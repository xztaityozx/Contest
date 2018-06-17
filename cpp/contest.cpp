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

// x=>ABC
string encode(const string s,const char x){
  string rt="";
  for(auto c:s){
    if(c==x) rt+="ABC";
    else rt += c;
  }
  return rt;
}

// ABC=>x
string decode(string s,char x){
  auto n=s.size();
  string rt="";
  
  for(int i=0;i<n;++i){
    if(i+2<n&&s[i]=='A'&&s[i+1]=='B'&&s[i+2]=='C'){
      rt+=x;
      i+=2;
    }else rt+=s[i];
  }
  
  return rt;
}

bool check(const string& s){
  int cnt[3]{0};
  for(auto c:s) cnt[c-'A']++;

  return (cnt[0]>0&&cnt[1]>0&&cnt[2]>0);
}

bool solve(){
  string S;
  cin >> S;

  while(1){
    if(!check(S))break;
    if(S=="ABC") return true;

    char item='k';
    for(auto c='A';c<='C';c++){
      if(S==encode(decode(S,c),c)){
        item=c;
      }
    }
    if(item=='k') break;
    S=decode(S,item);
  }
  return false;
}

int main(){
  cout << (solve()?"Yes":"No") << endl;
}
