using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace Services.Interface
{
    public interface IReportService
    {
        public List<Report> GetAll();

        public void Add(Report report);

        public void Update(Report report);

        public void Remove(Report report);
    }
}
