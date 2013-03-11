using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyFem.Modeling
{
    /// <summary>
    /// 锥顶弹体
    /// </summary>
    public class ConeBulletInfo : ModelingBaseInfo
    {
        private float warheadHeight;//弹头长度
        private float height;//每个弹体都有高度
        private float diameter;// 弹体直径

        /// <summary>
        /// 弹头长度
        /// </summary>
        public float WarHeadHeight
        {
            get { return warheadHeight; }
            set { warheadHeight = value; }
        }
        /// <summary>
        /// 每个弹体都有高度
        /// </summary>
        public float Height
        {
            get { return height; }
            set { height = value; }
        }
        /// <summary>
        /// 弹体直径
        /// </summary>
        public float Diameter
        {
            get { return diameter; }
            set { diameter = value; }
        }
    }
}
