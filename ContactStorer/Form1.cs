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
 * Contact Storing Utility Main Form Class
 * 
 * @author Shawn M. Crawford
 **/
namespace ContactStorer {
    public partial class Form1 : Form {

        ClassBackend backend;

        public Form1() {
            InitializeComponent();
            backend = new ClassBackend();
        }

        private void contactsDatabaseDataSetBindingSource_CurrentChanged(object sender, EventArgs e) {

        }

        private void Form1_Load(object sender, EventArgs e) {
            fillTable();
        }

        private void buttonAdd_Click(object sender, EventArgs e) {

            if (checkFields().Equals(0)) {
                int active = activeCheck();

                backend.addRecord(textBoxFirstName.Text, textBoxLastName.Text, textBoxEmailAddress.Text, textBoxPhoneNumber.Text, textBoxHomeAddress.Text, textBoxCity.Text, textBoxState.Text, textBoxZip.Text, active);

                fillTable();
                clearFields();
            } else {
                MessageBox.Show("Please fill out all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearFields() {
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxEmailAddress.Text = "";
            textBoxPhoneNumber.Text = "";
            textBoxHomeAddress.Text = "";
            textBoxCity.Text = "";
            textBoxState.Text = "";
            textBoxZip.Text = "";
            checkBoxActive.Checked = false;
        }

        private int checkFields() {
            int result = 0;
            if (string.IsNullOrWhiteSpace(textBoxFirstName.Text) || string.IsNullOrWhiteSpace(textBoxLastName.Text) || string.IsNullOrWhiteSpace(textBoxEmailAddress.Text) || string.IsNullOrWhiteSpace(textBoxPhoneNumber.Text) || string.IsNullOrWhiteSpace(textBoxHomeAddress.Text) || string.IsNullOrWhiteSpace(textBoxCity.Text) || string.IsNullOrWhiteSpace(textBoxState.Text) || string.IsNullOrWhiteSpace(textBoxZip.Text)) {
                result = 1;
            }

            return result;
        }

        private int activeCheck() {
            return checkBoxActive.Checked ? 1 : 0; 
        }

        private int displayActiveUsersOnlyCheck() {
            return checkBoxDisplayActiveUsers.Checked ? 1 : 0; 
        }

        private void buttonRefresh_Click(object sender, EventArgs e) {
            fillTable();
        }

        private void fillTable() {
            int displayActiveUsers = displayActiveUsersOnlyCheck();

            if (displayActiveUsers.Equals(1)) {
                this.contactsTableTableAdapter.FillBy(this.contactsDatabaseDataSet.ContactsTable);
            } else {
                this.contactsTableTableAdapter.Fill(this.contactsDatabaseDataSet.ContactsTable);
            }
        }

        private void checkBoxDisplayActiveUsers_CheckedChanged(object sender, EventArgs e) {
            fillTable();
        }

        private void editRecordsToolStripMenuItem_Click(object sender, EventArgs e) {

            if (backend.checkTableHasData().Equals(-1)) {
                MessageBox.Show("There is nothing in the database to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                FormEdit formEdit = new FormEdit();

                formEdit.ShowDialog();

                fillTable();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutBox1 aboutBox = new AboutBox1();

            aboutBox.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
