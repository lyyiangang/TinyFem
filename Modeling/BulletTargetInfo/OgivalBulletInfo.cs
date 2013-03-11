using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyFem.Modeling
{
    /// <summary>
    /// 圆顶形弹体
    /// </summary>
    class OgivalBulletInfo : ModelingBaseInfo 
    {
        float warheadHeight;// 弹头长度
        float arcRadius;//圆弧半径
        float height;//每个弹体都有高度

        /// <summary>
        /// 弹头的长度
        /// </summary>
        public float WarHeadHeight
        {
            get { return warheadHeight; }
            set { warheadHeight = value; }
        }
        /// <summary>
        /// 弹头圆弧的半径
        /// </summary>
        public float ArcRadius
        {
            get { return arcRadius; }
            set { arcRadius = value; }
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
