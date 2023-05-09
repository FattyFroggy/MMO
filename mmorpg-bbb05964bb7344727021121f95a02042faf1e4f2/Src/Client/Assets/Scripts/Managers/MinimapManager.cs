using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Managers
{
    public class MinimapManager : Singleton<MinimapManager>
    {
        public UIMinimap minimap;
        private Collider miniMapBoundingBox;
        public Collider MiniMapBoundingBox
        {
            get
            {
                return miniMapBoundingBox;
            }

        }
        // Start is called before the first frame update
        public Transform PlayerTransform
        {
            get
            {
                if (User.Instance.CurrentCharacter == null)
                {
                    return null;
                }
                return User.Instance.CurrentCharacterObject.transform;
            }
        }
        public Sprite LoadCurrenMinimap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }

        public void UpdateMinimap(Collider miniMapBoundingBox)
        {
            this.miniMapBoundingBox = miniMapBoundingBox;
            if (this.minimap != null)
                this.minimap.UpdateMap();
        }
    }
}