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


struct BitBoard {
    const bool BT=true;
    const bool WT=false;

    bool Now;
    ulong player;
    ulong opponent;

    BitBoard(ulong player,ulong opponent):player(player),opponent(opponent){}

    const ulong coordinateToBit(int x,int y){
        ulong mask=0x8000000000000000;
        
        switch (x)
        {
            case 1:
                mask>>=1;
                break;
            case 2:
                mask>>=2;
                break;
            case 3:
                mask>>=3;
                break;
            case 4:
                mask>>=4;
                break;
            case 5:
                mask>>=5;
                break;
            case 6:
                mask>>=6;
                break;
            case 7:
                mask>>=7;
                break;
            default:
                break;
        }
        mask = mask >> ((y-1)*8);
        return mask;
    }

    bool canPut(ulong put){
        ulong lboard=makeLegalBoard(*this);

        return (put&lboard) == put;
    }

    ulong makeLegalBoard(BitBoard& board){
        ulong hwb=board.opponent&0x7e7e7e7e7e7e7e7e;
        ulong vwb=board.opponent&0x00FFFFFFFFFFFF00;
        ulong awb=board.opponent&0x007e7e7e7e7e7e00;
        ulong blank=~(board.player|board.opponent);

        ulong tmp,legal;

        tmp=hwb&(board.player<<1);
        rep(i,5) tmp|=hwb&(tmp<<1);
        legal=blank&(tmp<<1);

        tmp=hwb&(board.player>>1);
        rep(i,5) tmp|=hwb&(tmp>>1);
        legal|=blank&(tmp>>1);

        tmp=vwb&(board.player<<8);
        rep(i,5) tmp|=vwb&(tmp<<8);
        legal|=blank&(tmp<<8);
        
        tmp=vwb&(board.player>>8);
        rep(i,5) tmp|=vwb&(tmp>>8);
        legal|=blank&(tmp>>8);

        tmp=awb&(board.player<<7);
        rep(i,5) tmp|=vwb&(tmp<<7);
        legal|=blank&(tmp<<7);
        
        tmp=awb&(board.player<<9);
        rep(i,5) tmp|=vwb&(tmp<<9);
        legal|=blank&(tmp<<9);
        
        tmp=awb&(board.player>>9);
        rep(i,5) tmp|=vwb&(tmp>>9);
        legal|=blank&(tmp>>9);
        
        tmp=awb&(board.player>>7);
        rep(i,5) tmp|=vwb&(tmp>>7);
        legal|=blank&(tmp>>7);
        
        return legal;
    }

    pair<ulong,ulong> reverse(ulong put){
        ulong rev=0UL;
        auto p=player;
        auto o=opponent;
        rep(k,8){
            auto _rev=0UL;
            auto mask = transfar(put,k);
            while(mask!=0&&((mask&o)!=0)){
                _rev|=mask;
                mask=transfar(mask,k);
            }
            if((mask&p)!=0) rev|=_rev;

        }
            p^=put|rev;
            o^=rev;
        return make_pair(p,o);
    }

    ulong transfar(ulong put,int k){
        switch (k)
         {
            case 0: //上
                return (put << 8) & 0xffffffffffffff00;
            case 1: //右上
                return (put << 7) & 0x7f7f7f7f7f7f7f00;
            case 2: //右
                return (put >> 1) & 0x7f7f7f7f7f7f7f7f;
            case 3: //右下
                return (put >> 9) & 0x007f7f7f7f7f7f7f;
            case 4: //下
                return (put >> 8) & 0x00ffffffffffffff;
            case 5: //左下
                return (put >> 7) & 0x00fefefefefefefe;
            case 6: //左
                return (put << 1) & 0xfefefefefefefefe;
            case 7: //左上
                return (put << 9) & 0xfefefefefefefe00;
            default:
                return 0;
        }
    }

    bool isGameEnd() {
        auto plboard=makeLegalBoard(*this);
        BitBoard board(opponent,player);
        board.Now=!Now;

        auto olboard=makeLegalBoard(board);

        return plboard==0 && olboard==0;
    }

    void boardSwap(){
        auto tmp=player;
        player=opponent;
        opponent=tmp;
        Now=!Now;
    }

    bool isPass(){
        auto plboard=makeLegalBoard(*this);
        BitBoard board(opponent,player);
        board.Now=!Now;
        auto olboard=makeLegalBoard(board);

        return plboard==0 && olboard!=0;
    }

    int Count(ulong b){
        const int SIZE=64;
        ulong mask=0x8000000000000000;
        auto cnt=0;
        rep(i,SIZE){
            if((mask&b)!=0) cnt++;
            mask>>=1;
        }
        return cnt;
    }

};

void output(BitBoard board){
    auto pb=board.player;
    auto ob=board.opponent;
    rep(i,8) {
        rep(j,8){
        if(pb&0x8000000000000000) cout << "o";
        else if(ob&0x8000000000000000) cout << "x";
        else cout << ".";
        pb<<=1;
        ob<<=1;
        }
        cout << endl;
    }
}

int main(){
    BitBoard board(0,0);
    rep(i,8) rep(j,8){
        char sij;
        cin >> sij;
        board.player<<=1;
        board.opponent<<=1;
        if(sij=='o') board.player|=1;
        if(sij=='x') board.opponent|=1;
    }
    board.Now=board.WT;
    
    while(!board.isGameEnd()){
        if(board.isPass()){ board.boardSwap();}
        else {
            auto mx=board.Count(board.player);
            ulong pq=0UL,oq=0UL;
            rep(i,8) rep(j, 8){
                auto put=board.coordinateToBit(i,j);
                if(!board.canPut(put)) continue;
                ulong p,o;
                tie(p,o)=board.reverse(put);
                auto res=board.Count(p);
                if(res>mx){
                    mx=res;
                    oq=o;
                    pq=p;
                }
            }
            board.opponent=oq;
            board.player=pq;
            board.boardSwap();
        }

        if(board.isGameEnd()) {board.boardSwap();break;}

        if(board.isPass()){ board.boardSwap();out("opponent !!!")}
        else {
            auto mx=board.Count(board.player);
            ulong pq=0UL,oq=0UL;
            rep(i,8) rep(j, 8){
                auto put=board.coordinateToBit(i,j);
                if(!board.canPut(put)) continue;
                ulong p,o;
                tie(p,o)=board.reverse(put);
                auto res=board.Count(p);
                if(res>=mx){
                    mx=res;
                    oq=o;
                    pq=p;
                }
            }
            board.opponent=oq;
            board.player=pq;
            board.boardSwap();
        }
    }

    output(board);
}
