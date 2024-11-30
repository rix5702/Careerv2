using CareerProject.Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace CareerProject.Models.Service
{
    public class JobSearchService
    {
        CareerDBContext db;
        public JobSearchService()
        {
            db = new CareerDBContext();
        }
        public List<tbl_Job> SearchJobs(string keyword)
        {

            var query = @"
            SELECT tbl_Job.*, TBL.RANK
            FROM tbl_Job
            JOIN CONTAINSTABLE(tbl_Job,*, N'{0}') AS TBL
            ON tbl_Job.ID = TBL.[KEY]
            ORDER BY TBL.RANK DESC;";


            // Execute the raw SQL query and return the results as a list of Job objects
            var results = db.tbl_Job
                .SqlQuery(query, keyword)
                .ToList();

            return results;
        }
    }
}