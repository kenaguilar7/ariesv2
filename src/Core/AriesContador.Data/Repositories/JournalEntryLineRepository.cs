﻿using AriesContador.Core.Models.JournalEntries;
using AriesContador.Core.Repositories;
using AriesContador.Data.Internal.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AriesContador.Data.Repositories
{
    public class JournalEntryLineRepository : IJournalEntryLineRepository
    {
        private readonly IConnectionString _connectionString;
        public JournalEntryLineRepository(IConnectionString connectionString)
        {
            this._connectionString = connectionString;
        }

        public void Add(JournalEntryLine entity)
        {
            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            entity.Id = dataAccess.SaveData<JournalEntryLine, int>("SP_InsertJournalEntryLine", entity);
        }

        public IEnumerable<JournalEntryLine> FindByAccountIdAndPostingPeriodId(int accountId, int postingPeriodId)
        {
            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            var jEntryLines = dataAccess.LoadData<JournalEntryLine, dynamic>
                                ("SP_GetAllJournalEntyLineByAccoudIdAndPostingPeriodId", new { accountId, postingPeriodId });
            return jEntryLines;
        }

        public IEnumerable<JournalEntryLine> FindByJournalEntryId(int journalEntryId)
        {
            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            var jEntryLines = dataAccess.LoadData<JournalEntryLine, dynamic>
                                ("SP_GetJournalEntryLineByJournalEntryId", new { JournalEntryId = journalEntryId });
            return jEntryLines;
        }

        public async Task<IEnumerable<JournalEntryLine>> FindByJournalEntryIdAsync(int journalEntryId)
        {
            try
            {
                MySqlDataAccessAsync dataAccess = new MySqlDataAccessAsync(_connectionString);
                var jEntryLines = await  dataAccess.LoadData<JournalEntryLine, dynamic>
                                    ("SP_GetJournalEntryLineByJournalEntryId", new { JournalEntryId = journalEntryId });
                return jEntryLines;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public JournalEntryLine GetById(int id)
        {
            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            var output = dataAccess.LoadData<JournalEntryLine, dynamic>("SP_GetJournalEntryLineById", new { Id = id });
            return output.FirstOrDefault();
        }

        public async Task Remove(JournalEntryLine entity)
        {
            MySqlDataAccessAsync dataAccess = new MySqlDataAccessAsync(_connectionString);
            await dataAccess.SaveData<JournalEntryLine>("SP_DesactivateJournalEntryLine", entity);
        }

        public void Update(JournalEntryLine entity)
        {
            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            dataAccess.SaveData<JournalEntryLine>("SP_UpdateJournalEntryLine", entity);

        }

        public async Task UpdateAsync(JournalEntryLine entity)
        {
            MySqlDataAccessAsync dataAccess = new MySqlDataAccessAsync(_connectionString);
            await dataAccess.SaveData<JournalEntryLine>("SP_UpdateJournalEntryLine", entity);

        }

        public IEnumerable<JournalEntryLineDeletedReport> GetDeletedItemByDateRange(BasicReportParam reportParam)
        {
            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            var jEntryLines = dataAccess.LoadData<JournalEntryLineDeletedReport, dynamic>
                ("SP_GetJournalEntyLineDeletedByDateRange", reportParam);
            return jEntryLines;
        }

        public void RestoreJournalEntryLine(JournalEntryLine entryLine)
        {
            MySqlDataAccess dataAccess = new MySqlDataAccess(_connectionString);
            dataAccess.SaveData("SP_RestoreJournalEntryLine", entryLine);
        }

        public async Task<int> AddAsyncWithReturnId(JournalEntryLine entity)
        {
            MySqlDataAccessAsync dataAccess = new MySqlDataAccessAsync(_connectionString);
            var id = await dataAccess.SaveData<JournalEntryLine, int>("SP_InsertJournalEntryLine", entity);
            return id;
        }

        public Task AddAsync(JournalEntryLine entity)
        {
            throw new NotImplementedException();
        }
    }
}
