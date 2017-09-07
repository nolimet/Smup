using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.Update
{

    public interface IUpdatable
    {
        //Update called once a frame
        void IUpdate();
    }

    public interface IFixedUpdateble
    {
        //Update called once every physics update
        void IFixedUpdate();
    }

    public interface IContinuesUpdateAble
    {
        //game state independent update. Does not react to pause
        void IContinuesUpdate();
    }
}