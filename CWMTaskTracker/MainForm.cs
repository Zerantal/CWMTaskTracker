using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CWMTaskTracker.DAL;

namespace CWMTaskTracker
{
    public partial class MainForm : Form
    {
        private Database _db;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(Database db) : this()
        {
            _db = db;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string tasksViewSql = @"
SELECT  JobID,
        ProjectName AS Project,
        JobTitle AS 'Job Title',
        Job.Description,
        ComplexityName AS Complexity,
        PriorityName AS Priority,
        strftime('%Y-%m-%d', Deadline) AS Deadline,
        StatusName AS Status
FROM    Job, Project, Complexity, Priority, Status
WHERE   Job.ProjectID = Project.ProjectID AND
        Job.ComplexityID = Complexity.ComplexityID AND
        Job.PriorityID = Priority.PriorityID AND
        Job.StatusID = Status.StatusID";

            using (SQLiteConnection dbConn = _db.OpenDbConnection())
            {
                SQLiteDataAdapter sqlda = new SQLiteDataAdapter(tasksViewSql, dbConn);

                SQLiteTransaction tr = dbConn.BeginTransaction();
                using (DataTable dt = new DataTable())
                {
                    sqlda.Fill(dt);
                    taskDGV.DataSource = dt;
                }
                tr.Commit();
            }
        }

        private void addJobBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
