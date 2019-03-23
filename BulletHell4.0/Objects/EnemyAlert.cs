using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace BulletHell4_0
{
    public class EnemyAlert : GameObject
    {
        BlinkingEffect effect;

        public EnemyAlert(Vector2 pos, string textureName) : base(pos, textureName, DrawMgr.Layer.GUI)
        {
            effect = new BlinkingEffect(this, 2, 4);
        }

        public void StartAlert()
        {
            IsActive = true;
            Create();
            effect.BeginEffect();
        }

        public override void Update()
        {
            if (effect.OnEnding)
            {
                Destroy();
            }
        }
    }
}
