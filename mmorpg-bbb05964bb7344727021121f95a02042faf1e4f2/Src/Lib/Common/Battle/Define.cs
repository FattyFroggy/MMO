using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Battle
{
    class Define
    {
        public enum AttributeType
        {
            None=-1,
            MaxHP=0,
            MaxMP=1,
            STR=2,//力量
            INT=3,//智力
            DEX=4,//敏捷
            AD=5,
            AP=6,
            DEF=7,//物防
            MDEF=8,//魔防
            SPD=9,//攻速
            CRI=10,//暴击概率
            MAX,
        }
    }
}
