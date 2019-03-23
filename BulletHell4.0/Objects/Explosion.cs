using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Aiv.Audio;
using OpenTK;

namespace BulletHell4_0
{
    public class Explosion : GameObject
    {
        AudioClip explosionClip;

        public Explosion(Vector2 pos) : base(pos, "explosion_0", DrawMgr.Layer.Playground)
        {
            Create();

            explosionClip = AudioMgr.GetClip("explosion");
        }

        public void PlayExplodeSound(AudioSource source)
        {
            source.Play(explosionClip);
        }
    }
}
