using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**
 * Contact Storing Utility Editor Form Class
 * 
 * @author Shawn M. Crawford
 **/
namespace ContactStorer {
    public partial class FormEdit : Form {

        ClassBackend backend;

        public FormEdit() {
            InitializeComponent();
            backend = new ClassBackend();
        }

        private void FormEdit_Load(object sender, EventArgs e) {
            backend.loadFirstRecord();
            populateFields();
        }

        private void populateFields() {
            textBoxFirstName.Text = backend.getFirstName();
            textBoxLastName.Text = backend.getLastName();
            textBoxCity.Text = backend.getCity();
            textBoxEmailAddress.Text = backend.getEmailAddress();
            textBoxHomeAddress.Text = backend.getHomeAddress();
            textBoxPhoneNumber.Text = backend.getPhoneNumber();
            textBoxState.Text = backend.getState();
            textBoxZip.Text = backend.getZip();
            if (backend.getActive().Equals(1)) {
                checkBoxActive.Checked = true;
            } else {
                checkBoxActive.Checked = false;
            }
            textBoxId.Text = backend.getId().ToString();
        }

        private void buttonFirstRecord_Click(object sender, EventArgs e) {
            backend.loadFirstRecord();
            populateFields();
        }

        private void buttonPreviousRecord_Click(object sender, EventArgs e) {
            backend.loadPreviousRecord();
            populateFields();
        }

        private void buttonNextRecord_Click(object sender, EventArgs e) {
            backend.loadNextRecord();
            populateFields();
        }

        private void buttonLastRecord_Click(object sender, EventArgs e) {
            backend.loadLastRecord();
            populateFields();
        }

        private void buttonUpdate_Click(object sender, EventArgs e) {


            if (checkFields().Equals(0)) {

                int active = 0;
                if (checkBoxActive.Checked.Equals(true)) {
                    active = 1;
                }

                int id = Int32.Parse(textBoxId.Text);

                backend.updateRecord(id, textBoxFirstName.Text, textBoxLastName.Text, textBoxEmailAddress.Text, textBoxPhoneNumber.Text, textBoxHomeAddress.Text, textBoxCity.Text, textBoxState.Text, textBoxZip.Text, active);
                populateFields();
            } else {
                MessageBox.Show("Please fill out all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSetInactiveAndUpdate_Click(object sender, EventArgs e) {
            checkBoxActive.Checked = false;
            buttonUpdate_Click(sender, e);
        }

        private void buttonPurge_Click(object sender, EventArgs e) {
            
            int id = Int32.Parse(textBoxId.Text);
            backend.updateRecord(id, "", "", "", "", "", "", "", "", 0);
            populateFields();
        }

        private int checkFields() {
            int result = 0;
            if (string.IsNullOrWhiteSpace(textBoxFirstName.Text) || string.IsNullOrWhiteSpace(textBoxLastName.Text) || string.IsNullOrWhiteSpace(textBoxEmailAddress.Text) || string.IsNullOrWhiteSpace(textBoxPhoneNumber.Text) || string.IsNullOrWhiteSpace(textBoxHomeAddress.Text) || string.IsNullOrWhiteSpace(textBoxCity.Text) || string.IsNullOrWhiteSpace(textBoxState.Text) || string.IsNullOrWhiteSpace(textBoxZip.Text)) {
                result = 1;
            }

            return result;
        }
    }
}
