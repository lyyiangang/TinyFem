using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TinyFem.Base;
using TinyFem.Modeling;
namespace TinyFem.FemElement
{
    /// <summary>
    /// 定义部件
    /// </summary>
   public class Part:IGridOnwer
    {
       Dictionary<int, Node> m_nodesDict = new Dictionary<int, Node>();
       Dictionary<int, Element> m_elementsDict = new Dictionary<int, Element>();
       GridActor m_gridActor=null ;
       ExtruedActor m_extrudeActor;
       public Part(ModelType type, ModelingBaseInfo baseinfo)
       {
           m_extrudeActor = new ExtruedActor(type, baseinfo);
       }

        #region IGridOnwer 成员

        public bool AddElement(Element ele)
        {
            if (m_gridActor == null)
                m_gridActor = new GridActor();
            if (m_elementsDict.ContainsKey(ele.EleId))
            {
                TinyFem.Utils.Logger.WriteLogMessage(ele.EleId.ToString() + " " + "单元编号已经存在");
                return false;
            }
            else
            {
                m_elementsDict.Add(ele.EleId, ele);
                m_gridActor.AddTetraElement(ele);
            }
            return true;
        }

        public bool AddNode(Node node)
        {
            if (m_gridActor == null)
                m_gridActor = new GridActor();
            if (m_nodesDict.ContainsKey(node.NodeId))
            {
                TinyFem.Utils.Logger.WriteLogMessage(node.NodeId.ToString() + " " + "节点编号已经存在");
                return false;
            }
            else
            {
                m_nodesDict.Add(node.NodeId, node);
                m_gridActor.AddNode(node);
            }
            return true;
        }
       /// <summary>
       /// 当所哟节点和单元都添加完毕
       /// </summary>
        public void AfterAdd(bool hasException)
        {
            if (hasException)
                m_gridActor = null;
            m_gridActor.ConstructActor();
        }
        #endregion
       /// <summary>
       /// 网格展示对象
       /// </summary>
        public GridActor GridActor
        {
            get { return m_gridActor; }
            set { m_gridActor = value; }
        }
       /// <summary>
       ///几何形体
       /// </summary>
        public ExtruedActor ExtrudeActor
        {
            get { return m_extrudeActor; }
            set { m_extrudeActor = value; }
        }


    }
}
