using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TinyFem.Modeling;
using TinyFem.VTKForm;
using TinyFem.FemElement;
namespace TinyFem
{
    public partial class ViewForm : Base.DocumentForm
    {
        VTKFormBase m_vtkfrom = null;
        public ViewForm()
        {
            InitializeComponent();
            InitForm();
        }
        void InitForm()
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            this.FormClosing += new FormClosingEventHandler(VTKForm_FormClosing);

            m_vtkfrom = new VTKFormBase();
            m_vtkfrom.Show();
            this.Controls.Add(m_vtkfrom);
        }

        void VTKForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
        private void VTKForm_Load(object sender, EventArgs e)
        {

        }
        public void TestShowActor()
        {
            CylinderBulletInfo baseinfo = new CylinderBulletInfo();
            baseinfo.Diameter = 0.5f;
            baseinfo.Height = 4;

            //ConeBulletInfo baseinfo = new ConeBulletInfo();
            //baseinfo.Diameter = 2;
            //baseinfo.Height = 10;
            //baseinfo.WarHeadHeight = 3;
            Part part =new Part(ModelType.DEGREE_360_3D, baseinfo);

            Mesher3D mesher = new Mesher3D();
           //if( mesher.MeshActor(part.ExtrudeActor , Mesher3D.eGridFiness.Moderate ))
            mesher.ParseGridFile3D(part);//网格剖分成功后才能解析网格文件 

            if(part.GridActor !=null )//如果在解析网格文件时出现异常，则不会构造actor
            m_vtkfrom.AddActor(part.GridActor);

           
        }
        public void TestMeshGrid()
        {

        }
        public void TestSelect()
        {//选择测试
                frmSelect frm = new frmSelect(m_vtkfrom);
                frm.Show();
        }
    }
}
