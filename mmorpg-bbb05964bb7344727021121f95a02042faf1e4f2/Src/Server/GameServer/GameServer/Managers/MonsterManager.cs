﻿using GameServer.Entities;
using GameServer.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class MonsterManager
    {
        Map Map;
        public Dictionary<int, Monster> Monsters = new Dictionary<int, Monster>();
        public void Init(Map map)
        {
            this.Map = map;
        }

        public Monster Create(int SpawnMonId,int spawnLevel,NVector3 position,NVector3 direction)
        {
            Monster monster = new Monster(SpawnMonId, spawnLevel, position, direction);
            EntityManager.Instance.AddEntity(this.Map.ID, monster);
            monster.Id = monster.entityId;
            monster.Info.Id = monster.entityId;
            monster.Info.mapId = this.Map.ID;
            Monsters[monster.Id] = monster;

            this.Map.MonsterEnter(monster);
            return monster;
        }
    }
}
