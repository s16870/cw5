using Cw5.DTO.Requests;
using Cw5.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Cw5.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16870;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                var response = new EnrollStudentResponse();
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;
                com.CommandText = "SELECT IdStudy FROM studies WHERE name=@name";
                com.Parameters.AddWithValue("name", request.Studies);
                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    tran.Rollback();
                    throw new ArgumentException("Studia " + request.Studies + " nie isnieją");
                }
                int idstudies = (int)dr["IdStudy"];
                response.IdStudies = idstudies;
                response.Semester = 1;
                dr.Close();
                com.Parameters.Clear();
                com.CommandText = "SELECT TOP 1 IdEnrollment, StartDate FROM enrollment WHERE semester = 1 AND IdStudy = @idStudy order by StartDate desc";
                com.Parameters.AddWithValue("idStudy", idstudies);
                dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    com.CommandText = "INSERT INTO ENROLLMENT(IdEnrollment,Semester,IdStudy,StartDate) OUTPUT INSERTED.IdEnrollment VALUES((SELECT MAX(E.IdEnrollment) FROM Enrollment E) + 1,1,@idStudy,@startDate";
                    var studiesStartDate = DateTime.Now;
                    com.Parameters.AddWithValue("startDate", studiesStartDate);
                    dr = com.ExecuteReader();
                    dr.Read();
                    response.IdEnrollment = (int)dr["IdEnrollment"];
                    response.StartDate = studiesStartDate;
                }
                else
                {
                    response.IdEnrollment = (int)dr["IdEnrollment"];
                    response.StartDate = (DateTime)dr["StartDate"];
                }
                dr.Close();
                com.Parameters.Clear();
                com.CommandText = "INSERT INTO StudentAPBD(IndexNumber,FirstName,LastName,BirthDate,IdEnrollment) VALUES(@index,@fname,@lname,@bdate,@idenrollment)";
                com.Parameters.AddWithValue("index", request.IndexNumber);
                com.Parameters.AddWithValue("fname", request.FirstName);
                com.Parameters.AddWithValue("lname", request.LastName);
                com.Parameters.AddWithValue("bdate", request.BirthDate);
                com.Parameters.AddWithValue("idenrollment", response.IdEnrollment);
                try
                {
                    dr = com.ExecuteReader();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dr.Close();
                    tran.Rollback();
                    throw new ArgumentException("Duplikat numeru indeksu");
                }
                response.IndexNumber = request.IndexNumber;
                dr.Close();
                tran.Commit();
                return response;
            }
        }

        public PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16870;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                com.CommandText = "EXEC PromoteStudents @Studies, @Semester";
                com.Parameters.AddWithValue("Studies", request.Studies);
                com.Parameters.AddWithValue("Semester", request.Semester);
                var dr = com.ExecuteReader();

                if (dr.Read())
                {
                    return new PromoteStudentsResponse
                    {
                        IdEnrollment = (int)dr["IdEnrollment"],
                        Semester = (int)dr["Semester"],
                        IdStudy = (int)dr["IdStudy"],
                        StartDate = (DateTime)dr["StartDate"]
                    };
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
