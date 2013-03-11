using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TinyFem.FemElement;
using TinyFem.Modeling;
using TinyFem.VTKForm;
namespace TinyFem.Base
{
    /// <summary>
    /// actor的类型，是展示的actor还是包含网格节点信息的actor
    /// </summary>
    public  enum eActorType
    {
        ViewActor,GridActor,None
    }
    /// <summary>
    /// 所有的有限元对象都要继承的
    /// </summary>
    public interface IFemPart
    {
        Part BelongPart { get; set; }        
        eActorType ActorType{get;set;}
    }
    /// <summary>
    /// 对选择做出响应改变自身颜色的对象要继承此接口
    /// </summary>
    public interface ISelectResonser
    {
        void ClearAllSelected();//清除所有选择的对象
        void AddPointId(int id, ePickBollean boolean);//仅添加一个id
        void AddCellId(int id, ePickBollean boolean);
        void AddPointId(Kitware.VTK.vtkIdTypeArray ids, ePickBollean boolean);//一次添加多个id
        void AddCellId(Kitware.VTK.vtkIdTypeArray ids, ePickBollean boolean);//一次添加多个id
        bool IsSelected { get; set; }//响应的对象被选中
        void UpdateSelectedColor();//更新选择部分的颜色
        ePickBollean BooleanOperation { get; set; }//布尔操作
    }
    /// <summary>
    /// 包含网格信息的对象
    /// </summary>
    public interface IGridOnwer
    {
        bool  AddElement(Element ele);
        bool AddNode(Node node);
        void AfterAdd(bool hasException);
    }
}
