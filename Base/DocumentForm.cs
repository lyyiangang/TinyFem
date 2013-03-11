using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TinyFem.Base
{
    public  partial   class DocumentForm : DevComponents.DotNetBar.Office2007Form
    {
        public DocumentForm()
        {
            InitializeComponent();
        }
       protected  string m_formId = string.Empty;
        /// <summary>
        /// 窗体的id
        /// </summary>
        public   string FormId
        {
            get { return m_formId; }
            set { m_formId = value; }
        }
    }
}
