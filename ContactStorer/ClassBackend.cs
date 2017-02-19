using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**
 * Backend Utility Class for Database.
 * 
 * @author Shawn M. Crawford
 **/
namespace ContactStorer {
    class ClassBackend {

        private int id;
        private string firstName;
        private string lastName;
        private string emailAddress;
        private string phoneNumber;
        private string homeAddress;
        private string city;
        private string state;
        private string zip;
        private int active;

        public ClassBackend() {
            id = -1;
            firstName = "";
            lastName = "";
            emailAddress = "";
            phoneNumber = "";
            homeAddress = "";
            city = "";
            state = "";
            zip = "";
            active = -1;
        }

        public int checkTableHasData() {
            int data = -1;

            SqlConnection sqlConnection = new SqlConnection(global::ContactStorer.Properties.Settings.Default.ContactsDatabaseConnectionString);
            try {
                string firstSQL = "SELECT TOP 1 Id,FirstName,LastName,EmailAddress,PhoneNumber,HomeAddress,City,State,Zip,Active FROM ContactsTable ORDER BY Id";
                Console.WriteLine("SQL: " + firstSQL);
                SqlCommand sqlCommand = new SqlCommand(firstSQL, sqlConnection);
                sqlConnection.Open();
                var result = sqlCommand.ExecuteScalar();
                int i = Convert.ToInt32(result);
                if (i == 0) {
                    Console.WriteLine("Data does not exist");
                } else {
                    Console.WriteLine("Data does exist");
                    data = 0;
                }
            } catch (Exception ex) {

            } finally {
                sqlConnection.Close();
            }

            return data;
        }

        public void loadLastRecord() {
            string lastSQL = "SELECT TOP 1 Id,FirstName,LastName,EmailAddress,PhoneNumber,HomeAddress,City,State,Zip,Active FROM ContactsTable ORDER BY Id DESC";
            executeSQLString(lastSQL);
        }

        public void loadFirstRecord() {
            string firstSQL = "SELECT TOP 1 Id,FirstName,LastName,EmailAddress,PhoneNumber,HomeAddress,City,State,Zip,Active FROM ContactsTable ORDER BY Id";
            executeSQLString(firstSQL);
        }

        public void loadPreviousRecord() {
            int previousRecordId = getId() - 1;
            string previousSQL;

            if (previousRecordId <= 0) {
                previousSQL = "SELECT TOP 1 Id,FirstName,LastName,EmailAddress,PhoneNumber,HomeAddress,City,State,Zip,Active FROM ContactsTable ORDER BY Id";
            } else {
                previousSQL = "SELECT * FROM ContactsTable WHERE Id = " + previousRecordId;
            }
            executeSQLString(previousSQL);
        }

        public void loadNextRecord() {
            int currentRecordId = getId();
            int nextRecordId = currentRecordId + 1;
            string lastSQL = "SELECT TOP 1 Id,FirstName,LastName,EmailAddress,PhoneNumber,HomeAddress,City,State,Zip,Active FROM ContactsTable ORDER BY Id DESC";
            string nextSQL;
            executeSQLString(lastSQL);
            int lastRecordId = getId();

            if (nextRecordId >= lastRecordId) {
                // this is the last record, do nothing
                Console.WriteLine("do nothing");
            } else {
                nextSQL = "SELECT * FROM ContactsTable WHERE Id = " + nextRecordId;
                executeSQLString(nextSQL);
            }

        }

        public void addRecord(string newFirstName, string newLastName, string newEmailAddress, string newPhoneNumber, string newHomeAddress, string newCity, string newState, string newZip, int newActive) {

            SqlConnection sqlConnection = new SqlConnection(global::ContactStorer.Properties.Settings.Default.ContactsDatabaseConnectionString);
            try {
                string sql = "INSERT INTO ContactsTable (FirstName,LastName,EmailAddress,PhoneNumber,HomeAddress,City,State,Zip,Active) values('" + newFirstName + "','" + newLastName + "','" + newEmailAddress + "','" + newPhoneNumber + "','" + newHomeAddress + "','" + newCity + "','" + newState + "','" + newZip + "'," + newActive + ")";
                Console.WriteLine("SQL: " + sql);
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Record Added.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                sqlConnection.Close();
            }
        }
        
        public void updateRecord(int newId, string newFirstName, string newLastName, string newEmailAddress, string newPhoneNumber, string newHomeAddress, string newCity, string newState, string newZip, int newActive) {
            string updateSQLString = "UPDATE ContactsTable SET FirstName=@FirstName,LastName=@LastName,EmailAddress=@EmailAddress,PhoneNumber=@PhoneNumber,HomeAddress=@HomeAddress,City=@City,State=@State,Zip=@Zip,Active=@Active WHERE Id=@Id";
            SqlConnection sqlConnection = new SqlConnection(global::ContactStorer.Properties.Settings.Default.ContactsDatabaseConnectionString);

            try {
                SqlCommand sqlCommand = new SqlCommand(updateSQLString, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", newId);
                sqlCommand.Parameters.AddWithValue("@FirstName", newFirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", newLastName);
                sqlCommand.Parameters.AddWithValue("@EmailAddress", newEmailAddress);
                sqlCommand.Parameters.AddWithValue("@PhoneNumber", newPhoneNumber);
                sqlCommand.Parameters.AddWithValue("@HomeAddress", newHomeAddress);
                sqlCommand.Parameters.AddWithValue("@City", newCity);
                sqlCommand.Parameters.AddWithValue("@State", newState);
                sqlCommand.Parameters.AddWithValue("@Zip", newZip);
                sqlCommand.Parameters.AddWithValue("@Active", newActive);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();

                firstName = newFirstName;
                lastName = newLastName;
                emailAddress = newEmailAddress;
                phoneNumber = newPhoneNumber;
                homeAddress = newHomeAddress;
                city = newCity;
                state = newState;
                zip = newZip;
                active = newActive;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error: " + ex);
            } finally {
                sqlConnection.Close();
            }
        }

        private void executeSQLString(string sqlString) {
            SqlConnection sqlConnection = new SqlConnection(global::ContactStorer.Properties.Settings.Default.ContactsDatabaseConnectionString);
            SqlDataReader sqlDataReader;
            try {
                SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                sqlDataReader.Read();

                id = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id"));
                firstName = sqlDataReader.GetString(sqlDataReader.GetOrdinal("FirstName"));
                lastName = sqlDataReader.GetString(sqlDataReader.GetOrdinal("LastName"));
                emailAddress = sqlDataReader.GetString(sqlDataReader.GetOrdinal("EmailAddress"));
                phoneNumber = sqlDataReader.GetString(sqlDataReader.GetOrdinal("PhoneNumber"));
                homeAddress = sqlDataReader.GetString(sqlDataReader.GetOrdinal("HomeAddress"));
                city = sqlDataReader.GetString(sqlDataReader.GetOrdinal("City"));
                state = sqlDataReader.GetString(sqlDataReader.GetOrdinal("State"));
                zip = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Zip"));
                active = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Active"));

                Console.WriteLine("Info: " + id + " " + firstName + " " + lastName + " " + emailAddress + " " + phoneNumber + " " + homeAddress + " " + city + " " + state + " " + zip + " " + active);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error: " + ex);
            } finally {
                sqlConnection.Close();
            }
        }

        public int getId() {
            return id;
        }

        public string getFirstName() {
            return firstName;
        }

        public string getLastName() {
            return lastName;
        }

        public string getEmailAddress() {
            return emailAddress;
        }

        public string getPhoneNumber() {
            return phoneNumber;
        }

        public string getHomeAddress() {
            return homeAddress;
        }

        public string getCity() {
            return city;
        }

        public string getState() {
            return state;
        }

        public string getZip() {
            return zip;
        }

        public int getActive() {
            return active;
        }
    }
}
