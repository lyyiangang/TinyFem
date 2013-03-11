using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyFem.Modeling
{
    /// <summary>
    /// 圆形靶体
    /// </summary>
    public class RoundTargetInfo : ModelingBaseInfo
    {
        private float radius;//靶体的半径
        private float thickness;//靶体的厚度

        /// <summary>
        /// 靶体的半径
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        /// <summary>
        /// 靶体的厚度
        /// </summary>
        public float Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }
    }
}
