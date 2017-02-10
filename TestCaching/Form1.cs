
using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCaching
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var person = Caching.Container.Get<IPerson>();
            person.UpdatePerson(new Person() { Id = 1, Name = "Name1" });
            var item = person.GetPerson(new Person() { Id = 1, Name = "Name1" });

        }
    }
}
