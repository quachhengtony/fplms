﻿using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private static StudentRepository? instance;
        private static readonly object instanceLock = new object();
        private static FplmsManagementContext? dbContext;

        public static StudentRepository Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new FplmsManagementContext();
                        instance = new StudentRepository();
                    }
                    return instance;
                }
            }
        }

        public async Task<Group> FindGroupByStudentIdAndClassId(int studentId, int classId)
        {
            return await dbContext.Groups.FromSqlRaw("SELECT * FROM `GROUP` WHERE id = (SELECT GROUP_id FROM STUDENT_GROUP WHERE STUDENT_id = {0} AND CLASS_id = {1})", studentId, classId).FirstOrDefaultAsync();
        }

        public async Task<int> FindStudentIdByEmail(string email)
        {
            return await dbContext.Students.AnyAsync(s => s.Email == email).Select(s => s.Id).FirstOrDefaultAsync();
        }

        public async Task<Student> FindOneById(int studentId)
        {
            return await dbContext.Students.FindAsync(studentId);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            return await dbContext.Students.AnyAsync(s => s.Email == email);
        }
    }
}