using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using N01399603_Cumulative_Part_2.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Web.Http.Cors;



namespace N01399603_Cumulative_Part_2.Controllers
{
    public class TeacherDataController : ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the teachers table of our school database. Non-Deterministic.
        /// <summary>
        /// Returns a list of teachers in the system
        /// </summary>
        /// <returns>
        /// A list of teacher Objects with fields mapped to the database column values
        /// (first name, last name, employee number, hiredate, and salary).
        /// </returns>
        /// <example>GET api/TeacherData/ListTeachers -> {Teacher Object, Teacher Object, Teacher Object...}</example>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.ForDatabaseAccess();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teachers
            List<Teacher> listTeacher = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int teacherid = (int)ResultSet["teacherid"];
                string teacherfname = ResultSet["teacherfname"].ToString();
                string teacherlname = ResultSet["teacherlname"].ToString();
                string employeenumber = ResultSet["employeenumber"].ToString();
                DateTime? hiredate = Convert.ToDateTime(ResultSet["hiredate"].ToString());
                decimal? salary = Convert.ToDecimal(ResultSet["salary"].ToString());

                Teacher NewTeacher = new Teacher();
                NewTeacher.teacherid = teacherid;
                NewTeacher.teacherfname = teacherfname;
                NewTeacher.teacherlname = teacherlname;
                NewTeacher.employeenumber = employeenumber;
                NewTeacher.hiredate = hiredate;
                NewTeacher.salary = salary;

                //Add the Teachers Name to the List
                listTeacher.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers names
            return listTeacher;
        }

        /// <summary>
        /// Finds an teacher from the MySQL Database through an id. Non-Deterministic.
        /// </summary>
        /// <param name="id">The Teacher ID</param>
        /// <returns>teacher object containing information about the teacher with a matching ID. 
        /// Empty teacher Object if the ID does not match any Teachers in the system.</returns>
        /// <example>api/TeacherData/FindTeacher/3 -> {teacher Object}</example>
        /// <example>api/TeacherData/FindTeacher/9 -> {teacher Object}</example>
        
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection
            MySqlConnection Conn = School.ForDatabaseAccess();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int teacherid = (int)ResultSet["teacherid"];
                string teacherfname = ResultSet["teacherfname"].ToString();
                string teacherlname = ResultSet["teacherlname"].ToString();
                string employeenumber = ResultSet["employeenumber"].ToString();
                DateTime? hiredate = Convert.ToDateTime(ResultSet["hiredate"].ToString());
                decimal? salary = Convert.ToDecimal(ResultSet["salary"].ToString());

                NewTeacher.teacherid = teacherid;
                NewTeacher.teacherfname = teacherfname;
                NewTeacher.teacherlname = teacherlname;
                NewTeacher.employeenumber = employeenumber;
                NewTeacher.hiredate = hiredate;
                NewTeacher.salary = salary;

            }
            Conn.Close();

            return NewTeacher;
        }

        /// <summary>
        /// Deletes a teacher from the connected MySQL Database if the ID of that teacher exists. 
        /// Does NOT maintain relational integrity. Non-Deterministic.
        /// </summary>
        /// <param name="id">The ID of the Teacher.</param>
        /// <example>POST /api/TeacherData/DeleteTeacher/4</example>
        
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.ForDatabaseAccess();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

        /// <summary>
        /// Add a teacher to the MySQL Database.
        /// </summary>
        /// <param name="Newteacher">An object with fields that map to the columns of the teacher's table. 
        /// Non-Deterministic.</param>
        /// <example>
        /// POST api/TeacherData/Addteacher
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///     "TeacherFname":"Jeff",
        ///	    "TeacherLname":"Smith",
        ///	    "TeacherEmployeenumber":"T562",
        ///	    "TeacherSalary":"45.20"
        ///	}
        /// </example>
        
        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.ForDatabaseAccess();

            Debug.WriteLine(NewTeacher.teacherfname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into teachers (teacherfname,teacherlname,employeenumber,hiredate,salary) values (@teacherfname,@teacherlname,@employeenumber, CURRENT_DATE(), @salary)";
            cmd.Parameters.AddWithValue("@teacherfname", NewTeacher.teacherfname);
            cmd.Parameters.AddWithValue("@teacherlname", NewTeacher.teacherlname);
            cmd.Parameters.AddWithValue("@employeenumber", NewTeacher.employeenumber);
            cmd.Parameters.AddWithValue("@salary", NewTeacher.salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

    }
}
