using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyFem.Modeling
{
    /// <summary>
    /// 圆柱形弹体参数建模信息
    /// </summary>
    public class CylinderBulletInfo : ModelingBaseInfo
    {
        private float diameter;// 直径
        private float height;//每个弹体都有高度

        /// <summary>
        /// 弹体的直径
        /// </summary>
        public float Diameter
        {
            get { return diameter; }
            set { diameter = value; }
        }
        /// <summary>
        /// 弹体的高度
        /// </summary>
        public float Height
        {
            get { return height; }
            set { height = value; }
        }
    }
}
