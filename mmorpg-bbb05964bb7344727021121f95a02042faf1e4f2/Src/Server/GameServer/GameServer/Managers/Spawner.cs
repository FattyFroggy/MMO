﻿using Common;
using Common.Data;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class Spawner
    {
        public SpawnRuleDefine Define { get; set; }

        public Map Map;

        private float spawnTime = 0;
        private float unspawnTime = 0;
        private bool spawned = false;

        private SpawnPointDefine spawnPoint = null;

        public Spawner(SpawnRuleDefine define,Map map)
        {
            this.Define = define;
            this.Map = map;

            if (DataManager.Instance.SpawnPoints.ContainsKey(this.Map.ID))
            {
                if (DataManager.Instance.SpawnPoints[this.Map.ID].ContainsKey(this.Define.SpawnPoint))
                {
                    spawnPoint = DataManager.Instance.SpawnPoints[this.Map.ID][this.Define.SpawnPoint];
                }
                else
                {
                    Log.ErrorFormat("SpawnerRule[{0}] SpawnPpont[{1}] not existed", this.Define.ID, this.Define.SpawnPoint);
                }
            }
        }

        public void Update()
        {
            if (this.CanSpwan())
            {
                this.Spawn();
            }
        }
        private bool CanSpwan()
        {
            if (this.spawned)
                return false;
            if (this.unspawnTime + this.Define.SpawnPeriod > Time.time)
                return false;
            return true;
        }
        private void Spawn()
        {
            this.spawned = true;
            Log.InfoFormat("Map[{0}]spawn[{1}:Mon{2} ,LV{3}]At point{4}", this.Map.ID, this.Define.ID, this.Define.SpawnMonID, this.Define.SpawnLevel, this.spawnPoint.ID);
            this.Map.MonsterManager.Create(this.Define.SpawnMonID, this.Define.SpawnLevel, this.spawnPoint.Position, this.spawnPoint.Direction);

        }


    }
}
