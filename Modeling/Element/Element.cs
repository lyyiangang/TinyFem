using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TinyFem.Utils;
namespace TinyFem.Modeling
{
    /// <summary>
    /// 单元类型
    /// </summary>
    public enum eEleType
    {
        /// <summary>
        /// 三角形
        /// </summary>
        Triangle=3, 
        /// <summary>
        /// 四边形
        /// </summary>
        Square=4, 
        /// <summary>
        /// 四面体
        /// </summary>
        Tetra=4, 
        /// <summary>
        /// 六面体
        /// </summary>
        Hex=8,
    }
    /// <summary>
    /// 单元
    /// </summary>
   public abstract class Element
    {
        protected  int m_eleNodeNum;
        protected  eEleType m_eleType = eEleType.Tetra;//默认为四面体
        protected int m_eleId;
        protected List<int> m_nodesId;
       /// <summary>
       /// 单元中的节点个数
       /// </summary>
       public int EleNodeNum { get { return m_eleNodeNum; } }
       /// <summary>
       /// 单元类型
       /// </summary>
       public  eEleType EleType
       {
           get { return m_eleType; }
       }
       public int EleId
       {
           get { return m_eleId; }
       }
       /// <summary>
       /// 每个单元的所有节点id
       /// </summary>
       public virtual List<int> NodeIds
       {
           get { return m_nodesId; }
       }
    }
    /// <summary>
    /// 四面体单元
    /// </summary>
   public class TetraElement : Element
   {
       
       public TetraElement(int node1, int node2, int node3, int node4, int eleid)
       {
           m_nodesId = new List<int>((int)eEleType.Tetra);
           m_nodesId.Add(node1);
           m_nodesId.Add(node2);
           m_nodesId.Add(node3);
           m_nodesId.Add(node4);
           m_eleId = eleid;
           m_eleType = eEleType.Tetra;
           m_eleNodeNum = (int)m_eleType;
       }
   }

   public class Node
   {
       FloatCoordinate m_cor;
       int  m_nodeid ;
       int  m_referencEleId;
       public Node(float x, float y, float z)
       {
           m_cor = new FloatCoordinate(x, y, z);
       }
       public Node(float x, float y, float z, int  nodeId)
       {
           m_cor = new FloatCoordinate(x, y, z);
           m_nodeid = nodeId;
       }

       /// <summary>
       /// 节点坐标
       /// </summary>
       public FloatCoordinate Cor
       {
           get { return m_cor; }
           set { m_cor = value; }
       }

       /// <summary>
       /// 本节点惯量的单元编号
       /// </summary>
       public int  ReferenceEleId
       {
           get { return m_referencEleId; }
           set { m_referencEleId = value; }
       }
       /// <summary>
       /// 节点编号
       /// </summary>
       public int  NodeId
       {
           get { return m_nodeid; }
           //set { m_nodeid = value; }
       }
   }

}
