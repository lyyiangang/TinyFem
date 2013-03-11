using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;
using Kitware.mummy;

namespace TinyFem.Modeling
{
    public enum ModelType
    {
        /// <summary>
        /// 3维1/4模型
        /// </summary>
        DEGREE_90_3D,
        /// <summary>
        /// 3维1/2模型
        /// </summary>
        DEGREE_180_3D,
        /// <summary>
        /// 3维360度模型
        /// </summary>
        DEGREE_360_3D,

        /// <summary>
        /// 2维1/2模型
        /// </summary>
        HALF_DEGREE_2D,
        /// <summary>
        /// 2维整体模型
        /// </summary>
        FULL_DEGREE_2D,
        None,
    }
    /// <summary>
    /// 截面类型 
    /// </summary>
    public enum SECTIONTYPE
    {
        SECTION_HALF,
        SECTION_FULL,
    }
    /// <summary>
    /// 产生弹体和靶体的过程：输入弹体类型：ModelType，和ModelingBaseInfo，先利用ModelingBaseInfo构造截面边缘的Points，
    /// 再将points塞到一个Polygon中去，这样就得到一个截面，然后根据需要将截面按一定角度拉伸得到不同的体
    /// </summary>
    public class ModelingFactory
    {
        /// <summary>
        /// 创建模型实例的数据信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static vtkPolyData CreateModelingInstance(ModelType type, ModelingBaseInfo info)
        {
            vtkPolyData resultPolyData = new vtkPolyData();
            switch (type)
            {
                case ModelType.FULL_DEGREE_2D:
                    {
                        vtkPolyData polyData = GetBulletSection(info, SECTIONTYPE.SECTION_FULL);//获取整个截面
                        resultPolyData = Extrude2DSection(polyData, info);
                    }
                    break;
                case ModelType.HALF_DEGREE_2D:
                    {
                        vtkPolyData polyData = GetBulletSection(info, SECTIONTYPE.SECTION_HALF);//获取一半的截面
                        resultPolyData = Extrude2DSection(polyData, info);
                    }
                    break;
                case ModelType.DEGREE_360_3D:
                    {//对截面映射360度,旋转平移
                        vtkPolyData polyData = GetBulletSection(info, SECTIONTYPE.SECTION_HALF);
                        resultPolyData = Extrude3DSection(360, polyData, info);
                    }
                    break;
                case ModelType.DEGREE_180_3D:
                    {//对截面映射180度
                        vtkPolyData polyData = GetBulletSection(info, SECTIONTYPE.SECTION_HALF);
                        resultPolyData = Extrude3DSection(180, polyData, info);
                    }
                    break;
                case ModelType.DEGREE_90_3D:
                    {//对截面映射90度
                        vtkPolyData polyData = GetBulletSection(info, SECTIONTYPE.SECTION_HALF);
                        resultPolyData = Extrude3DSection(90, polyData, info);
                    }
                    break;
                default:
                    break;
            }
            return resultPolyData;
        }

        public static vtkActor CreateBulletInstance(ModelType type, ModelingBaseInfo info)
        {//根据传入参数来创建不同的弹体
            vtkActor actor = vtkActor.New();
            try
            {
                switch (type)
                {
                    case ModelType.FULL_DEGREE_2D:
                        {
                            vtkPolyData polyData = GetBulletSection(info, SECTIONTYPE.SECTION_FULL);//获取整个截面
                            actor = FormActor2D( polyData, info);
                        }
                        break;
                    case ModelType.HALF_DEGREE_2D:
                        {
                            vtkPolyData polyData = GetBulletSection(info, SECTIONTYPE.SECTION_HALF);//获取一半的截面
                            actor = FormActor2D(polyData, info);
                        }
                        break;
                    case ModelType.DEGREE_360_3D:
                        {//对截面映射360度,旋转平移
                            vtkPolyData polyData = GetBulletSection(info, SECTIONTYPE.SECTION_HALF);//获取一半的截面
                            actor = FormActor3D(360, polyData, info);
                        }
                        break;
                    case ModelType.DEGREE_180_3D:
                        {//对截面映射180度
                            vtkPolyData polyData = GetBulletSection(info, SECTIONTYPE.SECTION_HALF);//获取一半的截面
                            actor = FormActor3D(180, polyData, info);
                        }
                        break;
                    case ModelType.DEGREE_90_3D:
                        {//对截面映射90度
                            vtkPolyData polyData = GetBulletSection(info, SECTIONTYPE.SECTION_HALF);//获取一半的截面
                            actor = FormActor3D(90, polyData, info);
                        } 
                        break;
                    default: break;
                }
                //
                //vtkPolyDataMapper mapper = (vtkPolyDataMapper)actor.GetMapper();
                //vtkPolyData polyd = mapper.GetInput();
                //vtkPoints points = polyd.GetPoints();
            }
            catch (Exception e)
            {
                TinyFem.Utils.Logger .WriteLogMessage("BulletFactory:CreateBulletInstance:" + e.Message);
            }
            return actor;
        }

        /// <summary>
        /// 进行2D转换
        /// </summary>
        /// <param name="polyData"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static vtkPolyData Extrude2DSection(vtkPolyData polyData, ModelingBaseInfo info)
        {
            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            vtkTransformFilter transFilter = vtkTransformFilter.New();
            vtkTransform transform = vtkTransform.New();
            transform.RotateX(-90);// 旋转到xoy面上

            transform.RotateY(info.Angle);//顺时针为正逆时针为负
           // transform.Translate(info.BaseCoordinate.X, info.BaseCoordinate.Y, info.BaseCoordinate.Z);//先旋转后平移
            transform.Translate(info.BaseCoordinate.X, info.BaseCoordinate.Z, info.BaseCoordinate.Y);//先旋转后平移

            transFilter.SetTransform(transform);
            transFilter.SetInput(polyData);
            return transFilter.GetPolyDataOutput();
        }
        /// <summary>
        /// 进行二维结构的转换
        /// </summary>
        /// <param name="polyData"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static vtkActor FormActor2D(vtkPolyData polyData, ModelingBaseInfo info)
        {//
            vtkActor actor = vtkActor.New();

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            vtkTransformFilter transFilter = vtkTransformFilter.New();
            vtkTransform transform = vtkTransform.New();
            transform.RotateX(-90);// 旋转到xoy面上

            transform.RotateY(info.Angle);//顺时针为正逆时针为负
            transform.Translate(info.BaseCoordinate.X, info.BaseCoordinate.Y, info.BaseCoordinate.Z);//先旋转后平移
           // transform.Translate(info.BaseCoordinate.X, info.BaseCoordinate.Z,info.BaseCoordinate.Y );//先旋转后平移

            transFilter.SetTransform(transform);
            transFilter.SetInput(polyData);

            mapper.SetInputConnection(transFilter.GetOutputPort());
            actor.SetMapper(mapper);
            return actor;
        }
        /// <summary>
        /// 进行三维结构的转换
        /// </summary>
        /// <param name="degree"></param>
        /// <param name="polyData"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static vtkPolyData Extrude3DSection(float degree, vtkPolyData polyData, ModelingBaseInfo info)
        {
            //进行拉伸以构造的三维实体
            vtkRotationalExtrusionFilter extrude = vtkRotationalExtrusionFilter.New();
            extrude.SetInput(polyData);
            extrude.SetResolution(80);
            extrude.SetAngle(degree);//旋转
            extrude.CappingOn();

            vtkPolyDataNormals normals = vtkPolyDataNormals.New();
            normals.SetInput(extrude.GetOutput());
            normals.SetFeatureAngle(100);
            //进行转换
            vtkTransform transform = vtkTransform.New();
            transform.RotateX(info.Angle);//逆时针为正，顺时针为负
            transform.Translate(info.BaseCoordinate.X, info.BaseCoordinate.Y, info.BaseCoordinate.Z);//先旋转后平移

            vtkTransformFilter transFilter = vtkTransformFilter.New();
            transFilter.SetTransform(transform);
            transFilter.SetInput(normals.GetOutput());

            //vtkTriangleFilter filter = vtkTriangleFilter.New();
            //filter.SetInput(transFilter.GetOutput());

            //return filter.GetPolyDataInput(0);

            vtkCleanPolyData clearPolydata = vtkCleanPolyData.New();//清除重合的点和片
            clearPolydata.SetInput(transFilter.GetOutput());
            return clearPolydata.GetOutput();
            //return transFilter.GetPolyDataOutput();
        }
        /// <summary>
        /// 进行三维结构的转换
        /// </summary>
        /// <param name="degree"></param>
        /// <param name="polyData"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static vtkActor FormActor3D(float degree, vtkPolyData polyData, ModelingBaseInfo info)
        {//旋转拉伸一个截面, 先绕Z拉伸后旋转一定攻角，再平移
            vtkActor actor = vtkActor.New();

            vtkPolyData polydata = Extrude3DSection(degree, polyData, info);
            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInput(polyData);
            actor.SetMapper(mapper);
            return actor;
        }

        /// <summary>
        /// 获取弹体截面的一半形状
        /// </summary>
        /// <returns></returns>
        private  static vtkPolyData GetBulletSection(ModelingBaseInfo info,SECTIONTYPE reflect)
        {//传入弹体参数，返回截面，该截面在ZOY面上
            vtkPolyData polyData = vtkPolyData.New();
            vtkPoints points = vtkPoints.New();
            vtkPolygon polygon = vtkPolygon.New();

            // 获取截面 点和拓扑结构
            ProcessBulletInfo(info, ref points, ref polygon, reflect);

            vtkCellArray cellArray = vtkCellArray.New();
            cellArray.InsertNextCell(polygon);
            polyData.SetPoints(points);
            polyData.SetPolys(cellArray);// 形成半个多边形
           
            return polyData;
        }
        /// <summary>
        /// 处理弹体信息，根据基本几何建模信息构造polygon（先找出边界点，再放入vtkPolygon中)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="points"></param>
        /// <param name="polygon"></param>
        /// <param name="reflcet"></param>
        private static void ProcessBulletInfo(ModelingBaseInfo info, ref vtkPoints points, ref vtkPolygon polygon, SECTIONTYPE reflcet)
        {
            points = GetHalfEdgePoint(info);
            int numpoints;
            if (reflcet == SECTIONTYPE.SECTION_HALF)
            {// 如果不做镜像
                numpoints = points.GetNumberOfPoints();
            }
            else
            {//如果做镜像,将点的个数增加一倍
                points = ReflectPointsByXOZ(points);
                numpoints = points.GetNumberOfPoints();
            }
            polygon.GetPointIds().SetNumberOfIds(numpoints);
            for (int i = 0; i < numpoints; i++)
            {
                polygon.GetPointIds().SetId(i, i);
            }
        }

        /// <summary>
        /// 得到边界点,（通过建模几何参数得到边界关键点）
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static vtkPoints GetHalfEdgePoint(ModelingBaseInfo info)
        {// 获取一半模型的点坐标
            vtkPoints points = vtkPoints.New();
            if (info is CylinderBulletInfo)
            {// 如果为圆柱形弹体
                CylinderBulletInfo cylinderInof = (CylinderBulletInfo)info;
                points.InsertNextPoint(0, 0, 0);
                points.InsertNextPoint(cylinderInof.Diameter / 2, 0, 0);
                points.InsertNextPoint(cylinderInof.Diameter / 2, 0, cylinderInof.Height);
                points.InsertNextPoint(0, 0, cylinderInof.Height);
            }
            else if (info is ConeBulletInfo)
            {//如果是锥形弹体
                ConeBulletInfo coneInfo = (ConeBulletInfo)info;
                points.InsertNextPoint(0, 0, 0);//在XZ面上建立剖面
                points.InsertNextPoint(coneInfo.Diameter / 2, 0, coneInfo.WarHeadHeight);
                points.InsertNextPoint(coneInfo.Diameter / 2, 0, coneInfo.Height);
                points.InsertNextPoint(0, 0, coneInfo.Height);
            }
            else if (info is OgivalBulletInfo)
            {//如果是圆顶形弹体
            }
            else if (info is RoundTargetInfo)
            {// 如果是圆形靶体
                RoundTargetInfo RoundInfo = (RoundTargetInfo)info;
                points.InsertNextPoint(0, 0, 0);
                points.InsertNextPoint(RoundInfo.Radius, 0, 0);
                points.InsertNextPoint(RoundInfo.Radius, 0, RoundInfo.Thickness);
                points.InsertNextPoint(0, 0, RoundInfo.Thickness);
            }
            else if (info is RectTargetInfo)
            {//如果是矩形靶体
                RectTargetInfo RectInfo = (RectTargetInfo)info;
                points.InsertNextPoint(0, 0, 0);
                points.InsertNextPoint(RectInfo.Width / 2, 0, 0);
                points.InsertNextPoint(RectInfo.Width / 2, 0, RectInfo.Thickness);
                points.InsertNextPoint(0, 0, RectInfo.Thickness);
            }
            return points;
        }

        /// <summary>
        /// 将点做映射,按照xoz面来影射点，为2维的弹体做准备
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private static vtkPoints ReflectPointsByXOZ(vtkPoints points)
        {
            vtkPoints tempPoints = vtkPoints.New();
            int numPoints = points.GetNumberOfPoints();
            //点数扩大两倍
            tempPoints.SetNumberOfPoints(numPoints * 2);
            //构造相应的点
            for (int i = 0; i < numPoints; i++)
            {
                double[] temp = points.GetPoint(i);
                tempPoints.SetPoint(numPoints * 2 - 1 - i, temp[0] * (-1), temp[1], temp[2]);//x坐标变为相反数
                tempPoints.SetPoint(i, temp[0], temp[1], temp[2]);
            }
            return tempPoints;
        }
    }
}
