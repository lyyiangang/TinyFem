using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

using System.Text.RegularExpressions;
using Kitware.mummy;
using Kitware.VTK;
using TinyFem.Utils;
using TinyFem.Base;
using System.Diagnostics;
namespace TinyFem.Modeling
{
    /// <summary>
    /// 进行网格剖分的类
    /// </summary>
    public class Mesher3D
    {// 剖分网格
        private string tempStlFileName = @"Mesher\temp.stl";
        private string strGridFile = @"Mesher\grid.vol";//网格文件
        //private string MesherDir = @"Mesher\";
        private string strMesher_3D = @"\Mesher\netgen.exe";

        static int m_numTotalPoints = 1;//记录导入的总节点个数,导入多个网格文件时用到
        static int m_numTotalEles = 1;//记录导入的总单元个数
        //veryfine,fine,moderate,coarse,verycoarse
        /// <summary>
        /// 网格剖分精细程度
        /// </summary>
        public enum eGridFiness
        {
            VeryFine,Fine,Moderate,Coarse,VeryCoarse,
        }


        public int NumTotalPoints
        {
            get { return m_numTotalPoints; }
            set { m_numTotalPoints = value; }
        }
        public int NumTotalEles
        {
            get { return m_numTotalEles; }
            set { m_numTotalEles = value; }
        }
        /// <summary>
        /// 初始化节点和单元的编号
        /// </summary>
        public void InitElePointNum()
        {
            m_numTotalEles = 0;
            m_numTotalPoints = 0;
        }

        /// <summary>
        /// 三维网格划分
        /// </summary>
        /// <param name="actor"></param>
        public bool MeshActor(vtkActor actor,eGridFiness finess)
        {
            bool result = true;
            try
            {
                if (File.Exists(tempStlFileName))//删除stl文件
                    File.Delete(tempStlFileName);
                if (File.Exists(strGridFile))//删除网格文件
                    File.Delete(strGridFile);

                SaveActorToStl(actor, tempStlFileName);// 将文件写入到stl文件中去
                if (!File.Exists(tempStlFileName))
                {
                    Debug.WriteLine("stl 文件不存在");
                    throw new Exception("");
                }
                Process mesherProcess = new Process();
                //netgen.exe -geofile=sweepBullet.stl -veryfine -meshfile=mesh.vol -batchmode 
                mesherProcess.StartInfo.FileName = Directory.GetCurrentDirectory() + strMesher_3D;                
                mesherProcess.StartInfo.Arguments =
                    " -geofile=" + tempStlFileName + "  -" + finess.ToString().ToLower() +
                    " -meshfile=" + strGridFile+" -batchmode ";
                mesherProcess.StartInfo.CreateNoWindow = false;
                mesherProcess.StartInfo.UseShellExecute = true;
                mesherProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                mesherProcess.Start();//生成网格文件
                mesherProcess.WaitForExit();
            }
            catch (Exception e)
            {
                Logger .WriteLogMessage("Mesher:MeshActor:" + e.Message);
                result = false;
            }
            return result;
        }        

        /// <summary>
        /// 将一个actor保存为stl格式
        /// </summary>
        /// <param name="actor"></param>
         void SaveActorToStl(vtkActor actor, string stlFileName)
        {
            //通过actor模型生成stl文件
            vtkPolyDataMapper mapper = (vtkPolyDataMapper)actor.GetMapper();//获取mapper
            vtkPolyData polyData = vtkPolyData.New();
            polyData = mapper.GetInput();//从mapper中提取数据
            vtkTriangleFilter filter = vtkTriangleFilter.New();
            filter.SetInput(polyData);//三角化后才能保存为正常的stl格式

            vtkSTLWriter stlWriter = vtkSTLWriter.New();
            stlWriter.SetFileName(stlFileName);
            stlWriter.SetInputConnection(filter.GetOutputPort());//提取数据
            stlWriter.Write();
        }
        /// <summary>
        /// 读取STL文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
         vtkActor ReadStlFile(string fileName)
        {// 读取stl模型
            vtkSTLReader stlReader = vtkSTLReader.New();
            stlReader.SetFileName(fileName);
            stlReader.Update();

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(stlReader.GetOutputPort());

            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            return actor;
        }
        
        /// <summary>
         /// 解析网格文件，并将节点和单元信息放到IGridOnwer中去
        /// </summary>
        /// <param name="impactModel"></param>
        /// <returns></returns>
        public bool ParseGridFile3D(IGridOnwer gridOnwer)
        {
            if (!File.Exists(strGridFile))
            {
                Debug.WriteLine("{0} 网格文件不存在", strGridFile);
                return false;
            }
            bool isExtracted = true;// 默认解析成功
            try
            {
                using (StreamReader streamReader = new StreamReader(strGridFile))
                {
                    string content = streamReader.ReadToEnd();//读取所有行
                    using (MemoryStream memoryStream = new MemoryStream(System.Text.Encoding.Default.GetBytes(content)))
                    {
                        using (StreamReader reader = new StreamReader(memoryStream))
                            //从内存流中读取节点和单元信息
                            isExtracted &= ReadGridFile_3D(reader, gridOnwer);
                    }
                }
            }
            catch (Exception e)
            {
                isExtracted = false;
                Logger.WriteLogMessage("Mesher:ParseGridFile_3D:" + e.Message);
            }
            return isExtracted;
        }
        
        /// <summary>
        /// 解析三维网格数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private bool ReadGridFile_3D(StreamReader reader, IGridOnwer gridOnwer)
        {
            #region
            //// //读取节点信息
            //// /*  #          X             Y             Z
            //// points
            //// 378
            //// 1.0000000000000000     -3.4201999999999999      9.3969299999999993
            ////*/
            #endregion
            bool result = true;
            try
            {
                #region 读取节点
                string strCurrent = "";
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                while ((strCurrent = reader.ReadLine()) != null)
                {
                    if (strCurrent.Trim().ToLower() == "points")// 节点信息
                    {//如果这一行是关键字，则把当前关键字和其对应的信息交给ParseKeyword处理
                        int numPoints = int.Parse(reader.ReadLine().Trim());// 读取点的个数
                       // nodeReflection = new Dictionary<string, string>();
                        for (int i = 1; i <= numPoints; i++)
                        {
                            string strTemp = reader.ReadLine();
                            List<string> strList = SplitStringBySpace(strTemp);//分割字符
                            bool readSuccess = false;
                                float x = float.Parse(strList[0]);
                                float y = float.Parse(strList[1]);
                                float z = float.Parse(strList[2]);
                                int nodeid =  m_numTotalPoints;
                                Node node = new Node(x, y, z, nodeid);
                                readSuccess = gridOnwer.AddNode(node);
                                if (readSuccess)
                                {
                                    m_numTotalPoints += 1;// 成功读入一个节点
                                }
                                else
                                {
                                    Debug.WriteLine("Error when parse points");
                                    continue;//读入下一个节点
                                }
                          }   
                    }
                }
                #endregion
                #region 读取单元
                //// /*#  matnr      np      p1      p2      p3      p4
                //// volumeelements
                //// 1080
                ////   1       4      53     230     137     314
                ////   1       4      61     324     198     217*/
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                while ((strCurrent = reader.ReadLine()) != null)
                {//k文件用*标识，hypermesh用<<标识
                    if (strCurrent.Trim().ToLower() == "volumeelements")//如果是单元信息
                    {
                        int numEle = int.Parse(reader.ReadLine().Trim());//获取单元的个数 
                        //int flag = 0;
                        for (int i = 1; i <= numEle; i++)
                        {
                            string strTemp = reader.ReadLine();
                            List<string> strList = SplitStringBySpace(strTemp);//分割字符

                            bool readSuccess = false;
                            int n1 = int.Parse(strList[2]);
                            int n2 = int.Parse(strList[3]);
                            int n3 = int.Parse(strList[4]);
                            int n4 = int.Parse(strList[5]);
                            int id = m_numTotalEles ;
                            TetraElement ele=new TetraElement(n1,n2,n3,n4,id );
                            readSuccess = gridOnwer.AddElement(ele);
                            if (readSuccess)
                            {
                                m_numTotalEles += 1;// 成功读入一个节点
                            }
                            else
                            {
                                Debug.WriteLine("Error when parse volumeelements");
                                continue;//读入下一个节点
                            }
                        }
                    }
                }
                gridOnwer.AfterAdd(false );//根据传入的节点和单元信息构造actor
                #endregion
            }
            catch (Exception e)
            {
                gridOnwer.AfterAdd(false);
                Logger.WriteLogMessage("Mesher:ReadGridFile_3D:" + e.Message);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 将字符串按空格分割
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static List<string> SplitStringBySpace(string str)
        {//将字符串中得Tab和空格去除，将数字放在list中
            str = str.Replace("\t", " ");
            string[] strTemp = str.Split(' ');
            List<string> strOut = new List<string>();
            foreach (string myStr in strTemp)
            {
                if (myStr != " " && myStr != "")
                    strOut.Add(myStr);
            }
            return strOut;
        }       

    }
}
