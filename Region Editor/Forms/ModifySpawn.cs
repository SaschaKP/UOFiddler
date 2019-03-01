using System;
using System.Windows.Forms;

namespace Region_Editor
{
    public partial class ModifySpawn : Form
    {
        public bool Canceled = false;
        public Spawn Spawn = new Spawn();

        public ModifySpawn()
        {
            InitializeComponent();
        }

        public void Initialize(Spawn spawn)
        {
            Spawn = spawn;

            id.Text = spawn.SpawnID.ToString();
            type.Text = spawn.SpawnType;
            min.Text = spawn.SpawnMinSeconds.ToString();
            max.Text = spawn.SpawnMaxSeconds.ToString();
            amount.Text = spawn.SpawnAmount.ToString();
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            if (id.Text == "" || type.Text == "" || min.Text == "" || max.Text == "" || amount.Text == "")
            {
                MessageBox.Show("All fields must to completed before saving.");
                return;
            }

            int _id;
            int _min;
            int _max;
            int _amount;

            try { _id = Convert.ToInt32(id.Text); }
            catch { _id = 0; }

            try { _min = Convert.ToInt32(min.Text); }
            catch { _min = 0; }

            try { _max = Convert.ToInt32(max.Text); }
            catch { _max = 0; }

            try { _amount = Convert.ToInt32(amount.Text); }
            catch { _amount = 0; }

            Spawn = new Spawn(_id, type.Text, _min, _max, _amount);

            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Canceled = true;
            Close();
        }
    }
}
