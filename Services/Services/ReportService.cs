using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Repository.Interface;

namespace Services.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public List<Report> GetAll()
        {
            return _reportRepository.GetAll().ToList();
        }

        public void Add(Report report)
        {
            _reportRepository.Add(report);
        }

        public void Update(Report report)
        {
            _reportRepository.Update(report);
        }

        public void Remove(Report report)
        {
            _reportRepository.Delete(report);
        }
    }
}
