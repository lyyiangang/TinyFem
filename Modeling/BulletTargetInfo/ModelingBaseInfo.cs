using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TinyFem.Utils;

namespace TinyFem.Modeling
{
    /// <summary>
    /// 建模的基本信息
    /// </summary>
    public  class ModelingBaseInfo
    {
        private FloatCoordinate baseCoordinate = new FloatCoordinate();
        private float angle = 0;//弹体的攻角,或者靶的角度

        /// <summary>
        /// 位置参考点
        /// </summary>
        public FloatCoordinate BaseCoordinate
        {
            get { return baseCoordinate; }
            set { baseCoordinate = value; }
        }

        /// <summary>
        /// 弹体的攻角或者靶体的角度
        /// </summary>
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
    }
}
