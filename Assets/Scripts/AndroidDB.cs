using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;

public class AndroidDB : MonoBehaviour
{
    
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;
    string DatabaseName = "Employers.s3db";
    int id;
    void Start()
    {

    //Application database Path android
        string filepath = Application.persistentDataPath + "/" + DatabaseName;
        if (!File.Exists(filepath))
        {
            // If not found on android will create Tables and database
            Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +
                             Application.dataPath + "!/assets/Employers");
            // UNITY_ANDROID
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/Employers.s3db");
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);      
        }

        conn = "URI=file:" + filepath;

        Debug.Log("Stablishing connection to: " + conn);
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        string query;
        query = "CREATE TABLE plan_tasks (id_user integer, task_name varchar(100),how_long integer default 1, date_start date,date_finish date, text_comment text,status integer default 0,how_long_finish integer default 1,date_finish_real date,text_comment_finish text);"+
        "CREATE TABLE job_duties (id_user integer, task_name varchar(100),priority integer default 1, text_comment varchar(230));"
        +"CREATE TABLE users (id_user INTEGER PRIMARY KEY   AUTOINCREMENT, login varchar(100), name varchar(100), secondname varchar(100), password varchar(200), email varchar(200),mycost INTEGER DEFAULT 10);";
        try
        {
            dbcmd = dbconn.CreateCommand(); // create empty command
            dbcmd.CommandText = query; // fill the command
            reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        }
        catch (Exception e)
        {

            Debug.Log(e);

        }

        CloseConnection();

    }
    public void CloseConnection()
    {
        dbconn.Close();
        dbcmd.Dispose();
    }
    
    // Update is called once per frame
    public string[] checkLogin(string str)
    {
        string[] data = new string[7];
        using (dbconn = new SqliteConnection(conn))
        {
            
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT login,password,id_user,name,secondname,email,mycost " + "FROM users where login ='" +str+"'";// table name
            dbcmd.CommandText = sqlQuery;
            try{
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
               // print(reader.GetString(1));

                //  string id = reader.GetString(0);
                data[0] = reader.GetString(0);//login
                data[1] = reader.GetString(1);
                data[2] = reader.GetInt32(2).ToString();//id
                data[3] = reader.GetString(3);//name
                data[4] = reader.GetString(4);//secondname
                data[5] = reader.GetString(5);//email
                data[6] = reader.GetInt32(6).ToString();
                //data_staff.text += Name_readers_Search + " - " + Address_readers_Search + "\n";
                id = reader.GetInt32(2);

                //Debug.Log(" name =" + data[0] + "Address=" + data[1]);

            }
            
            reader.Close();
            reader = null;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
        }
        return data;
    }
    public void insertUser(string login, string name, string secondname, string email, string password)
    {
        using (dbconn = new SqliteConnection(conn))
        {
        dbconn.Open(); //Open connection to the database.
        dbcmd = dbconn.CreateCommand();
        sqlQuery = string.Format("insert into users (login, password, name, secondname, email) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")", login, password,name,secondname,email);// table name
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteScalar();
        dbconn.Close();
        }

        Debug.Log("Insert Done  ");
    }
    public void updateUserData(string name, string secondname, string email, string login,string cost)
    {
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            dbcmd = dbconn.CreateCommand();
            sqlQuery = string.Format("UPDATE users set name = @name ,secondname = @secondname,email = @email, login = @login, mycost = @cost where id_user = @id ");

            SqliteParameter P_update_name = new SqliteParameter("@name", name);
            SqliteParameter P_update_secondname = new SqliteParameter("@secondname", secondname);
            SqliteParameter P_update_email = new SqliteParameter("@email", email);
            SqliteParameter P_update_login = new SqliteParameter("@login", login);
            SqliteParameter P_update_cost = new SqliteParameter("@cost", int.Parse(cost));
            SqliteParameter P_update_id = new SqliteParameter("@id", id);

            dbcmd.Parameters.Add(P_update_name);
            dbcmd.Parameters.Add(P_update_secondname);
            dbcmd.Parameters.Add(P_update_email);
            dbcmd.Parameters.Add(P_update_login);
            dbcmd.Parameters.Add(P_update_cost);
            dbcmd.Parameters.Add(P_update_id);

            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            dbconn.Close();
        }
    }
    public List<string> getListJobDutis()
    {
        List<string> list = new List<string>();
        using (dbconn = new SqliteConnection(conn))
        {
            
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT task_name,priority,text_comment " + "FROM job_duties where id_user =" +id+"";// table name
            dbcmd.CommandText = sqlQuery;
            try{
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                string str = reader.GetString(0)+"|" + reader.GetInt32(1).ToString()+"|"+reader.GetString(2);
                list.Add(str);
            }
            
            reader.Close();
            reader = null;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
        }
        return list;
    }
    public void insertJobDuties(string task_name,int priority,string text_comment)
    {
        using (dbconn = new SqliteConnection(conn))
        {
        dbconn.Open(); //Open connection to the database.
        dbcmd = dbconn.CreateCommand();
        sqlQuery = string.Format("insert into job_duties values (\"{0}\",\"{1}\",\"{2}\",\"{3}\")", id, task_name,priority,text_comment);// table name
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteScalar();
        dbconn.Close();
        }

        Debug.Log("Insert Done  ");
    }
    public void deleteJobDuties(string task_name,int priority,string text_comment)
    {
        using (dbconn = new SqliteConnection(conn))
        {

            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "DELETE FROM job_duties where id_user =" + id+" AND task_name ='" +task_name+"' AND priority ="+priority+" AND text_comment ='"+text_comment+"'";// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            //data_staff.text = Delete_by_id + " Delete  Done ";

        }
        Debug.Log("Deleted");
    }
    public List<string> getListTasks()
    {
        List<string> list = new List<string>();
        using (dbconn = new SqliteConnection(conn))
        {
            
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT task_name,how_long,date_start,date_finish,text_comment " + "FROM plan_tasks where id_user =" +id+" AND status=0";// table name
            dbcmd.CommandText = sqlQuery;
            try{
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                string str = reader.GetString(0)+"|" + reader.GetInt32(1).ToString()+"|"+reader.GetString(2)+"|"+reader.GetString(3)+"|"+reader.GetString(4);
                list.Add(str);
            }
            
            reader.Close();
            reader = null;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
        }
        return list;
    }
    public void insertTask(string task_name,int count_hour,string date_start, string date_finish,string text_comment)
    {
        using (dbconn = new SqliteConnection(conn))
        {
        dbconn.Open(); //Open connection to the database.
        dbcmd = dbconn.CreateCommand();
        sqlQuery = string.Format("insert into plan_tasks (id_user,task_name,how_long,date_start,date_finish,text_comment) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\")", id, task_name,count_hour,date_start,date_finish,text_comment);// table name
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteScalar();
        dbconn.Close();
        }

        Debug.Log("Insert Done  ");
    }
    public void deleteTask(string task_name,int count,string text_comment)
    {
        using (dbconn = new SqliteConnection(conn))
        {

            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "DELETE FROM plan_tasks where id_user =" + id+" AND task_name = '" +task_name+"' AND how_long ="+count;// table name
            print(sqlQuery);
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            //data_staff.text = Delete_by_id + " Delete  Done ";

        }
        Debug.Log("Deleted");
    }
    public void updatePlanTask(string name, int how_long, int how_long_finish,string date_finish,string comment_finish)
    {
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            dbcmd = dbconn.CreateCommand();
            sqlQuery = string.Format("UPDATE plan_tasks set status = 1 , how_long_finish = @how_long_finish,date_finish_real = @date_finish_real, text_comment_finish = @text_comment_finish where id_user = @id and task_name = @name and how_long = @how_long");

            SqliteParameter P_update_name = new SqliteParameter("@how_long_finish", how_long_finish);
            SqliteParameter P_update_secondname = new SqliteParameter("@date_finish_real", date_finish);
            SqliteParameter P_update_email = new SqliteParameter("@text_comment_finish", comment_finish);
            SqliteParameter P_update_login = new SqliteParameter("@id", id);
            SqliteParameter P_update_cost = new SqliteParameter("@name", name);
            SqliteParameter P_update_id = new SqliteParameter("@how_long", how_long);

            dbcmd.Parameters.Add(P_update_name);
            dbcmd.Parameters.Add(P_update_secondname);
            dbcmd.Parameters.Add(P_update_email);
            dbcmd.Parameters.Add(P_update_login);
            dbcmd.Parameters.Add(P_update_cost);
            dbcmd.Parameters.Add(P_update_id);

            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            dbconn.Close();
        }
    }
    public List<string> getListCompletedTasks()
    {
        List<string> list = new List<string>();
        using (dbconn = new SqliteConnection(conn))
        {
            
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT task_name,how_long_finish,date_start,date_finish_real,text_comment,how_long " + "FROM plan_tasks where id_user =" +id+" AND status=1";// table name
            dbcmd.CommandText = sqlQuery;
            try{
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                string str = reader.GetString(0)+"|" + reader.GetInt32(1).ToString()+"|"+reader.GetString(2)+"|"+reader.GetString(3)+"|"+reader.GetString(4)+"|"+reader.GetInt32(5);
                list.Add(str);
            }
            
            reader.Close();
            reader = null;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
        }
        return list;
    }
}
